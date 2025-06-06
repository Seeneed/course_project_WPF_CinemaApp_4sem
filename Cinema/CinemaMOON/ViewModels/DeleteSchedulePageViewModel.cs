using CinemaMOON.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using CommunityToolkit.Mvvm.Input;
using CinemaMOON.Data;

namespace CinemaMOON.ViewModels
{
    public class DeleteSchedulePageViewModel : ViewModelBase
	{
		private readonly AppDbContext _dbContext;
		private readonly Movie _targetMovie;
		private ObservableCollection<Schedule> _showtimes;
		private Schedule _selectedSchedule;

		public string MovieTitle => _targetMovie?.Title ?? (GetResourceString("DeleteSchedulePage_NoMovieSelected") ?? "Фильм не выбран");

		public ObservableCollection<Schedule> Showtimes
		{
			get => _showtimes;
			private set => SetProperty(ref _showtimes, value);
		}

		public Schedule SelectedSchedule
		{
			get => _selectedSchedule;
			set
			{
				if (SetProperty(ref _selectedSchedule, value))
				{
					DeleteSelectedCommand.NotifyCanExecuteChanged();
				}
			}
		}

		public IAsyncRelayCommand DeleteSelectedCommand { get; }
		public ICommand GoBackCommand { get; }

		public DeleteSchedulePageViewModel(AppDbContext dbContext, Movie movie)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_targetMovie = movie ?? throw new ArgumentNullException(nameof(movie));
			_showtimes = new ObservableCollection<Schedule>();

			DeleteSelectedCommand = new AsyncRelayCommand(ExecuteLogicallyDeleteSelectedAsync, CanExecuteDeleteSelected);
			GoBackCommand = new RelayCommand(ExecuteGoBack);

			_ = LoadShowtimesAsync();
		}

		private async Task LoadShowtimesAsync()
		{
			Showtimes.Clear();
			SelectedSchedule = null;
			try
			{
				var movieSchedules = await _dbContext.Schedules
					.Where(s => s.MovieId == _targetMovie.Id &&
						   !s.IsDeleted &&
						   s.ShowTime >= DateTime.Now)
					.OrderBy(s => s.ShowTime)
					.Include(s => s.Movie)
					.ToListAsync();

				foreach (var schedule in movieSchedules)
				{
					Showtimes.Add(schedule);
				}
			}
			catch (Exception ex)
			{
				ShowMessageFormat("DeleteSchedulePage_Error_LoadFailed", "AdminPanel_Title_Error", MessageBoxImage.Warning, _targetMovie.Title, ex.Message);
			}
			finally
			{
				DeleteSelectedCommand.NotifyCanExecuteChanged();
			}
		}

		private bool CanExecuteDeleteSelected()
		{
			return SelectedSchedule != null;
		}

		private async Task ExecuteLogicallyDeleteSelectedAsync()
		{
			if (!CanExecuteDeleteSelected()) return;

			var scheduleToLogicallyDelete = SelectedSchedule;
			string movieTitle = scheduleToLogicallyDelete.Movie?.Title ?? _targetMovie.Title;
			DateTime showTime = scheduleToLogicallyDelete.ShowTime;

			string confirmMsg = string.Format(
				GetResourceString("DeleteSchedulePage_ConfirmLogicalDelete") ?? "Пометить сеанс фильма '{0}' на {1:dd.MM.yyyy HH:mm} как удаленный? Активные заказы будут отменены.",
				movieTitle, showTime);
			string confirmTitle = GetResourceString("DeleteSchedulePage_ConfirmDelete_Title") ?? "Подтверждение удаления";

			if (MessageBox.Show(confirmMsg, confirmTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
			{
				try
				{
					var scheduleInDb = await _dbContext.Schedules
						.Include(s => s.Orders)
						.FirstOrDefaultAsync(s => s.Id == scheduleToLogicallyDelete.Id);

					if (scheduleInDb != null)
					{
						scheduleInDb.IsDeleted = true;

						string canceledStatusKey = "OrderStatus_Canceled";
						string bookedStatusKey = "OrderStatus_Booked";

						int canceledOrdersCount = 0;
						foreach (var order in scheduleInDb.Orders)
						{
							if (order.OrderStatus == bookedStatusKey)
							{
								order.OrderStatus = canceledStatusKey;
								order.UserRating = null;
								canceledOrdersCount++;
							}
						}

						await _dbContext.SaveChangesAsync();

						Showtimes.Remove(scheduleToLogicallyDelete);
						SelectedSchedule = null;

						ShowMessage("DeleteSchedulePage_Success_ScheduleLogicallyDeleted", "AdminPanel_Title_Success", MessageBoxImage.Information);
					}
					else
					{
						ShowMessage("DeleteSchedulePage_Error_NotFound", "AdminPanel_Title_Error", MessageBoxImage.Warning);
						await LoadShowtimesAsync();
					}
				}
				catch (Exception ex)
				{
					ShowMessageFormat("DeleteSchedulePage_Error_GeneralDelete", "AdminPanel_Title_Error", MessageBoxImage.Error, ex.Message);
					await LoadShowtimesAsync();
				}
			}
		}

		private void ExecuteGoBack(object parameter)
		{
			NavigationService navService = null;
			if (parameter is Page page)
			{
				navService = NavigationService.GetNavigationService(page);
			}
			else
			{
				var mainFrame = (Application.Current.MainWindow?.Content as Frame);
				if (mainFrame != null)
				{
					navService = mainFrame.NavigationService;
				}
			}

			if (navService?.CanGoBack == true)
			{
				navService.GoBack();
			}
		}

		private string GetResourceString(string key) => Application.Current.TryFindResource(key) as string;

		private void ShowMessage(string messageKey, string titleKey, MessageBoxImage icon)
		{
			string message = GetResourceString(messageKey) ?? $"[{messageKey}]";
			string title = GetResourceString(titleKey) ?? $"[{titleKey}]";
			MessageBox.Show(message, title, MessageBoxButton.OK, icon);
		}

		private void ShowMessageFormat(string messageKey, string titleKey, MessageBoxImage icon, params object[] args)
		{
			try
			{
				string m = string.Format(GetResourceString(messageKey), args);
				MessageBox.Show(m, GetResourceString(titleKey), MessageBoxButton.OK, icon);
			}
			catch
			{
				MessageBox.Show(GetResourceString(messageKey) + "(Format Error)", GetResourceString(titleKey), MessageBoxButton.OK, icon);
			}
		}
	}
}
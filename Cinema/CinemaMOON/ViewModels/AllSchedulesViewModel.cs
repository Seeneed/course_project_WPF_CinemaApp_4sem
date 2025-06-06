using CinemaMOON.Data;
using CinemaMOON.Models;
using CommunityToolkit.Mvvm.Input; 
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace CinemaMOON.ViewModels
{
	public class HallFilterItem : ViewModelBase 
	{
		public string DisplayNameKey { get; set; }
		public string ActualHallNameKey { get; set; }
		public string DisplayName => Application.Current.TryFindResource(DisplayNameKey) as string ?? DisplayNameKey;
		public override string ToString() => DisplayName;
	}

	public class AllSchedulesViewModel : ViewModelBase
	{
		private readonly AppDbContext _dbContext;
		private ObservableCollection<Schedule> _displayedSchedules;
		private Schedule _selectedSchedule;
		private ObservableCollection<HallFilterItem> _hallFilters;
		private HallFilterItem _selectedHallFilter;
		public ObservableCollection<Schedule> DisplayedSchedules
		{
			get => _displayedSchedules;
			set => SetProperty(ref _displayedSchedules, value);
		}

		public Schedule SelectedSchedule
		{
			get => _selectedSchedule;
			set
			{
				if (SetProperty(ref _selectedSchedule, value))
				{
					DeleteScheduleCommand.NotifyCanExecuteChanged();
				}
			}
		}

		public ObservableCollection<HallFilterItem> HallFilters
		{
			get => _hallFilters;
			set => SetProperty(ref _hallFilters, value);
		}

		public HallFilterItem SelectedHallFilter
		{
			get => _selectedHallFilter;
			set
			{
				if (SetProperty(ref _selectedHallFilter, value))
				{
					_ = LoadSchedulesAsync();
				}
			}
		}

		public IAsyncRelayCommand LoadSchedulesCommand { get; }
		public IAsyncRelayCommand DeleteScheduleCommand { get; }
		public ICommand GoBackCommand { get; }

		public AllSchedulesViewModel(AppDbContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			DisplayedSchedules = new ObservableCollection<Schedule>();

			InitializeHallFilters();

			LoadSchedulesCommand = new AsyncRelayCommand(LoadSchedulesAsync);
			DeleteScheduleCommand = new AsyncRelayCommand(ExecuteDeleteScheduleAsync, CanExecuteDeleteSchedule);
			GoBackCommand = new RelayCommand<Page>(ExecuteGoBack);
		}

		private void InitializeHallFilters()
		{
			HallFilters = new ObservableCollection<HallFilterItem>
			{
				new HallFilterItem { DisplayNameKey = "AllSchedulesPage_HallFilter_All", ActualHallNameKey = null },
				new HallFilterItem { DisplayNameKey = "AllSchedulesPage_HallFilter_Large", ActualHallNameKey = "HallPage_LargeHallName" },
				new HallFilterItem { DisplayNameKey = "AllSchedulesPage_HallFilter_Medium", ActualHallNameKey = "HallPage_MediumHallName" },
				new HallFilterItem { DisplayNameKey = "AllSchedulesPage_HallFilter_Small", ActualHallNameKey = "HallPage_SmallHallName" }
			};
			_selectedHallFilter = HallFilters.FirstOrDefault(f => f.ActualHallNameKey == null);

			OnPropertyChanged(nameof(HallFilters));
			OnPropertyChanged(nameof(SelectedHallFilter));
		}

		private async Task LoadSchedulesAsync()
		{
			DisplayedSchedules.Clear();
			SelectedSchedule = null;
			try
			{
				var query = _dbContext.Schedules
									  .Include(s => s.Movie)
									  .Include(s => s.Hall)
									  .Where(s => !s.IsDeleted);

				if (SelectedHallFilter != null && !string.IsNullOrEmpty(SelectedHallFilter.ActualHallNameKey))
				{
					query = query.Where(s => s.Hall.Name == SelectedHallFilter.ActualHallNameKey);
				}

				var schedulesFromDb = await query.OrderBy(s => s.ShowTime).ToListAsync();

				foreach (var schedule in schedulesFromDb)
				{
					DisplayedSchedules.Add(schedule);
				}
			}
			catch (Exception ex)
			{
				ShowMessageFormat("AllSchedulesPage_Error_LoadFailed", "AdminPanel_Title_Error", MessageBoxImage.Error, ex.Message);
			}
			finally
			{
				DeleteScheduleCommand.NotifyCanExecuteChanged();
			}
		}

		private bool CanExecuteDeleteSchedule()
		{
			return SelectedSchedule != null;
		}

		private async Task ExecuteDeleteScheduleAsync()
		{
			if (!CanExecuteDeleteSchedule())
			{
				ShowMessage("AllSchedulesPage_Info_NoScheduleSelected", "AdminPanel_Title_Warning", MessageBoxImage.Warning);
				return;
			}

			var scheduleToLogicallyDelete = SelectedSchedule;
			string movieTitle = scheduleToLogicallyDelete.Movie?.Title ?? GetResourceString("N/A");
			DateTime showTime = scheduleToLogicallyDelete.ShowTime;
			string hallNameKey = scheduleToLogicallyDelete.Hall?.Name ?? GetResourceString("N/A");
			string localizedHallName = GetResourceString(hallNameKey) ?? hallNameKey;

			string confirmMsgFormat = GetResourceString("AllSchedulesPage_ConfirmLogicalDeleteSchedule");
			string confirmMsg = string.Format(
				confirmMsgFormat ?? "Пометить сеанс фильма '{0}' на {1:dd.MM.yyyy HH:mm} в зале '{2}' как удаленный? Активные заказы будут отменены.",
				movieTitle, showTime, localizedHallName);

			string confirmTitle = GetResourceString("AllSchedulesPage_ConfirmDeleteSchedule_Title") ?? "Подтверждение удаления";

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

						foreach (var order in scheduleInDb.Orders)
						{
							if (order.OrderStatus == bookedStatusKey)
							{
								order.OrderStatus = canceledStatusKey;
								order.UserRating = null;
							}
						}

						await _dbContext.SaveChangesAsync();

						DisplayedSchedules.Remove(scheduleToLogicallyDelete);
						SelectedSchedule = null;

						ShowMessage("AllSchedulesPage_Success_ScheduleLogicallyDeleted", "AdminPanel_Title_Success", MessageBoxImage.Information);
					}
					else
					{
						ShowMessage("DeleteSchedulePage_Error_NotFound", "AdminPanel_Title_Error", MessageBoxImage.Warning);
						await LoadSchedulesAsync();
					}
				}
				catch (Exception ex)
				{
					ShowMessageFormat("AllSchedulesPage_Error_DeleteFailed", "AdminPanel_Title_Error", MessageBoxImage.Error, ex.Message);
					await LoadSchedulesAsync();
				}
			}
		}

		private void ExecuteGoBack(Page currentPage)
		{
			if (currentPage?.NavigationService?.CanGoBack == true)
			{
				currentPage.NavigationService.GoBack();
			}
		}

		private string GetResourceString(string key) => Application.Current.TryFindResource(key) as string ?? $"[{key}]";

		private void ShowMessage(string messageKey, string titleKey, MessageBoxImage icon)
		{
			MessageBox.Show(GetResourceString(messageKey), GetResourceString(titleKey), MessageBoxButton.OK, icon);
		}

		private void ShowMessageFormat(string messageKey, string titleKey, MessageBoxImage icon, params object[] args)
		{
			try
			{
				string msgFormat = GetResourceString(messageKey);
				if (string.IsNullOrEmpty(msgFormat))
				{
					MessageBox.Show($"Resource key '{messageKey}' not found.", GetResourceString(titleKey), MessageBoxButton.OK, icon);
					return;
				}
				string msg = string.Format(msgFormat, args);
				MessageBox.Show(msg, GetResourceString(titleKey), MessageBoxButton.OK, icon);
			}
			catch (FormatException fe)
			{
				MessageBox.Show(GetResourceString(messageKey) + $" (Format Error: {fe.Message})",
					GetResourceString(titleKey), MessageBoxButton.OK, icon);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"An unexpected error occurred while formatting message for key '{messageKey}': {ex.Message}",
				   GetResourceString(titleKey), MessageBoxButton.OK, icon);
			}
		}

		public void UpdateLocalization()
		{
			var currentHallFilterKey = SelectedHallFilter?.ActualHallNameKey;
			InitializeHallFilters();
			SelectedHallFilter = HallFilters.FirstOrDefault(f => f.ActualHallNameKey == currentHallFilterKey)
								 ?? HallFilters.First();

			CommandManager.InvalidateRequerySuggested();
		}
	}
}
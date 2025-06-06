using CinemaMOON.Data;
using CinemaMOON.Models;
using CinemaMOON.Views;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace CinemaMOON.ViewModels
{
    public class AddSchedulePageViewModel : ViewModelBase
	{
		private readonly AppDbContext _dbContext;

		private Movie _targetMovie;
		private string _showDateString = DateTime.Today.ToString("yyyy.MM.dd");
		private string _showTimeString = DateTime.Now.ToString("HH:mm");

		private ObservableCollection<Hall> _availableHalls;
		private Hall _selectedHall;
		private string _hallError;
		private bool _isHallErrorVisible;

		private string _dateError;
		private string _timeError;
		private bool _isDateErrorVisible;
		private bool _isTimeErrorVisible;

		public string MovieTitle => _targetMovie?.Title ?? "Фильм не выбран";

		public ObservableCollection<Hall> AvailableHalls
		{
			get => _availableHalls;
			private set => SetProperty(ref _availableHalls, value);
		}

		public Hall SelectedHall
		{
			get => _selectedHall;
			set
			{
				if (SetProperty(ref _selectedHall, value))
				{
					ValidateHall();
					AddScheduleCommand.NotifyCanExecuteChanged();
				}
			}
		}

		public string HallError
		{
			get => _hallError;
			private set => SetProperty(ref _hallError, value);
		}

		public bool IsHallErrorVisible
		{
			get => _isHallErrorVisible;
			private set => SetProperty(ref _isHallErrorVisible, value);
		}

		public string ShowDateString
		{
			get => _showDateString;
			set
			{
				if (SetProperty(ref _showDateString, value))
				{
					ValidateDate();
					AddScheduleCommand.NotifyCanExecuteChanged();
				}
			}
		}

		public string ShowTimeString
		{
			get => _showTimeString;
			set
			{
				if (SetProperty(ref _showTimeString, value))
				{
					ValidateTime();
					AddScheduleCommand.NotifyCanExecuteChanged();
				}
			}
		}

		public string DateError
		{
			get => _dateError;
			private set => SetProperty(ref _dateError, value);
		}

		public string TimeError
		{
			get => _timeError;
			private set => SetProperty(ref _timeError, value);
		}

		public bool IsDateErrorVisible
		{
			get => _isDateErrorVisible;
			private set => SetProperty(ref _isDateErrorVisible, value);
		}

		public bool IsTimeErrorVisible
		{
			get => _isTimeErrorVisible;
			private set => SetProperty(ref _isTimeErrorVisible, value);
		}

		public IAsyncRelayCommand AddScheduleCommand { get; }
		public ICommand GoBackCommand { get; }

		public AddSchedulePageViewModel(AppDbContext dbContext, Movie movie)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_targetMovie = movie ?? throw new ArgumentNullException(nameof(movie));

			AvailableHalls = new ObservableCollection<Hall>();

			AddScheduleCommand = new AsyncRelayCommand(ExecuteAddSchedule, CanExecuteAddSchedule);
			GoBackCommand = new RelayCommand(ExecuteGoBack);

			_ = LoadHallsAsync();

			ValidateDate();
			ValidateTime();
			ValidateHall();
		}

		private async Task LoadHallsAsync()
		{
			try
			{
				var halls = await _dbContext.Halls.OrderBy(h => h.Name).ToListAsync();
				AvailableHalls = new ObservableCollection<Hall>(halls);
			}
			catch (Exception ex)
			{
				ShowMessage("AddSchedule_Error_LoadingHalls", "AddSchedule_Error_LoadingHalls_Title", MessageBoxImage.Error);
			}
			finally
			{
				AddScheduleCommand.NotifyCanExecuteChanged();
			}
		}

		private bool ValidateDate()
		{
			DateError = null;
			bool isValid = true;

			if (string.IsNullOrWhiteSpace(ShowDateString))
			{
				DateError = Application.Current.TryFindResource("Validation_Error_DateEmpty") as string ?? "Дата не может быть пустой.";
				isValid = false;
			}
			else if (!DateTime.TryParseExact(ShowDateString, "yyyy.MM.dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
			{
				DateError = Application.Current.TryFindResource("Validation_Error_DateFormat") as string ?? "Неверный формат даты. Ожидается гггг.мм.дд";
				isValid = false;
			}
			else if (parsedDate.Date < DateTime.Today)
			{
				DateError = Application.Current.TryFindResource("Validation_Error_DatePast") as string ?? "Нельзя добавить расписание на прошедшую дату.";
				isValid = false;
			}

			IsDateErrorVisible = !isValid;
			return isValid;
		}

		private bool ValidateTime()
		{
			TimeError = null;
			bool isValid = true;

			if (string.IsNullOrWhiteSpace(ShowTimeString))
			{
				TimeError = Application.Current.TryFindResource("Validation_Error_TimeEmpty") as string ?? "Время не может быть пустым.";
				isValid = false;
			}
			else if (!DateTime.TryParseExact(ShowTimeString, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
			{
				TimeError = Application.Current.TryFindResource("Validation_Error_TimeFormat") as string ?? "Неверный формат времени. Ожидается чч:мм";
				isValid = false;
			}
			else if (DateTime.TryParseExact(ShowDateString, "yyyy.MM.dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate) &&
					 DateTime.TryParseExact(ShowTimeString, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedTime))
			{
				DateTime combinedDateTime = parsedDate.Date.Add(parsedTime.TimeOfDay);
				if (combinedDateTime < DateTime.Now)
				{
					TimeError = Application.Current.TryFindResource("Validation_Error_TimePast") as string ?? "Нельзя добавить сеанс на прошедшее время.";
					isValid = false;
				}
			}

			IsTimeErrorVisible = !isValid;
			return isValid;
		}

		private bool ValidateHall()
		{
			HallError = null;
			bool isValid = SelectedHall != null;
			if (!isValid)
			{
				HallError = GetResourceString("Validation_Error_HallEmpty") ?? "Необходимо выбрать зал.";
			}
			IsHallErrorVisible = !isValid;
			return isValid;
		}

		private bool CanExecuteAddSchedule()
		{
			return !IsDateErrorVisible && !IsTimeErrorVisible && !IsHallErrorVisible && SelectedHall != null;
		}

		private async Task ExecuteAddSchedule()
		{
			bool isDateValid = ValidateDate();
			bool isTimeValid = ValidateTime();
			bool isHallValid = ValidateHall();

			if (!isDateValid || !isTimeValid || !isHallValid)
			{
				ShowMessage("AddSchedule_Error_ValidationFailed", "AddSchedule_Error_ValidationFailed_Title", MessageBoxImage.Warning);
				return;
			}

			try
			{
				DateTime showDateTime = DateTime.ParseExact($"{ShowDateString} {ShowTimeString}", "yyyy.MM.dd HH:mm", CultureInfo.InvariantCulture);

				bool isDuplicate = await _dbContext.Schedules
					.AnyAsync(s => s.MovieId == _targetMovie.Id
								&& s.HallId == SelectedHall.Id
								&& s.ShowTime == showDateTime
								&& !s.IsDeleted);

				if (isDuplicate)
				{
					string localizedHallName = GetResourceString(SelectedHall.Name) ?? SelectedHall.Name;

					string duplicateErrorMsg = string.Format(
						GetResourceString("AddSchedule_Error_Duplicate") ?? "Сеанс для фильма '{0}' в зале '{1}' на {2:dd.MM.yyyy HH:mm} уже существует.",
						_targetMovie.Title,
						localizedHallName,
						showDateTime);
					string duplicateErrorTitle = GetResourceString("AddSchedule_Error_Duplicate_Title") ?? "Дубликат сеанса";
					MessageBox.Show(duplicateErrorMsg, duplicateErrorTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
					return;
				}

				var newSchedule = new Schedule
				{
					Id = Guid.NewGuid(),
					MovieId = _targetMovie.Id,
					HallId = SelectedHall.Id,
					ShowTime = showDateTime,
					AvailableSeats = SelectedHall.Capacity,
					IsDeleted = false
				};

				_dbContext.Schedules.Add(newSchedule);
				await _dbContext.SaveChangesAsync();

				string localizedHallNameForSuccess = GetResourceString(SelectedHall.Name) ?? SelectedHall.Name;

				string successMsg = string.Format(
					GetResourceString("AddSchedule_SuccessMessage") ?? "Расписание для фильма '{0}' в зале '{1}' на {2:dd.MM.yyyy HH:mm} успешно добавлено.",
					_targetMovie.Title,
					localizedHallNameForSuccess,
					showDateTime);
				string successTitle = GetResourceString("AddSchedule_SuccessTitle") ?? "Успех";
				MessageBox.Show(successMsg, successTitle, MessageBoxButton.OK, MessageBoxImage.Information);

				ExecuteGoBack(null);
			}
			catch (FormatException ex)
			{
				string formatErrorMsg = Application.Current.TryFindResource("AddSchedule_Error_Format") as string ?? "Произошла ошибка при обработке даты или времени. Убедитесь, что формат верный.";
				string formatErrorTitle = Application.Current.TryFindResource("AddSchedule_Error_Format_Title") as string ?? "Ошибка формата";
				MessageBox.Show($"{formatErrorMsg}\n({ex.Message})", formatErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
			}
			catch (DbUpdateException dbEx)
			{
				ShowMessageFormat("AddSchedule_Error_SavingFailed_DB", "AddSchedule_Title_Error", MessageBoxImage.Error, dbEx.InnerException?.Message ?? dbEx.Message);
			}
			catch (Exception ex)
			{
				string generalErrorMsg = string.Format(
					Application.Current.TryFindResource("AddSchedule_Error_General") as string ?? "Произошла ошибка при добавлении расписания: {0}",
					ex.Message);
				string generalErrorTitle = Application.Current.TryFindResource("AddSchedule_Error_General_Title") as string ?? "Критическая ошибка";
				MessageBox.Show(generalErrorMsg, generalErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
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
				string m = string.Format(GetResourceString(messageKey) ?? messageKey, args);
				MessageBox.Show(m, GetResourceString(titleKey) ?? titleKey, MessageBoxButton.OK, icon);
			}
			catch (FormatException)
			{
				MessageBox.Show(GetResourceString(messageKey) + " (Format Error)", GetResourceString(titleKey) ?? titleKey, MessageBoxButton.OK, icon);
			}
		}
	}
}
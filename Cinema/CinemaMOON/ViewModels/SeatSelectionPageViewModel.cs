using CinemaMOON.Models;
using CinemaMOON.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using CommunityToolkit.Mvvm.Input;
using CinemaMOON.Data;
using System.Net.Mail;
using System.Net;
using System.Globalization;

namespace CinemaMOON.ViewModels
{
    public class SeatSelectionPageViewModel : ViewModelBase
	{
		private readonly AppDbContext _dbContext;
		private readonly Schedule _selectedSchedule;
		private readonly User _currentUser;
		private Hall _currentHall;

		private string _hallName = "...";
		private ObservableCollection<string> _selectedSeatIdsInternal; 
		private string _selectedSeatsText;

		private const int MaxSelectedSeats = 3; 
		private bool _hasExistingBookingForThisShowtime = false; 

		public string MovieTitle => _selectedSchedule?.Movie?.Title ?? GetResourceString("UnknownMovie");
		public DateTime ShowTime => _selectedSchedule?.ShowTime ?? DateTime.MinValue;

		public string HallName
		{
			get => _hallName; 
			private set => SetProperty(ref _hallName, value); 
		}

		public string CurrentHallType => _currentHall?.Type?.ToLowerInvariant();
		public string SelectedSeatsText 
		{
			get => _selectedSeatsText; 
			private set => SetProperty(ref _selectedSeatsText, value); 
		}

		public bool CanConfirmBooking => _selectedSeatIdsInternal != null && _selectedSeatIdsInternal.Any() && !_hasExistingBookingForThisShowtime; 
		public HashSet<string> BookedSeatIds { get; private set; } = new HashSet<string>();

		public ICommand SelectSeatCommand { get; }
		public IAsyncRelayCommand ConfirmBookingCommand { get; }
		public ICommand GoBackCommand { get; }

		public SeatSelectionPageViewModel(AppDbContext dbContext, Schedule schedule, User user)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_selectedSchedule = schedule ?? throw new ArgumentNullException(nameof(schedule));
			_currentUser = user ?? throw new ArgumentNullException(nameof(user));

			_selectedSeatIdsInternal = new ObservableCollection<string>();
			_selectedSeatIdsInternal.CollectionChanged += SelectedSeatIdsInternal_CollectionChanged;

			SelectSeatCommand = new RelayCommand<string>(ExecuteSelectSeat, CanExecuteSelectSeat);
			ConfirmBookingCommand = new AsyncRelayCommand(ExecuteConfirmBookingAsync, () => CanConfirmBooking); 
			GoBackCommand = new RelayCommand(ExecuteGoBack);

			_ = InitializeAsync();
		}

		private async Task InitializeAsync()
		{
			await LoadHallDataAsync();
			await RefreshBookedSeatsAndUserStatusAsync();
			UpdateSelectedSeatsText();
			OnPropertyChanged(nameof(CurrentHallType));
			OnPropertyChanged(nameof(HallName));
		}

		public async Task RefreshDataOnPageLoadAsync()
		{
			await RefreshBookedSeatsAndUserStatusAsync();
		}

		private async Task RefreshBookedSeatsAndUserStatusAsync()
		{
			await LoadBookedSeatsAsync();
			await RefreshCurrentUserBookingStatusAsync();
			CommandManager.InvalidateRequerySuggested();
		}

		private async Task RefreshCurrentUserBookingStatusAsync()
		{
			try
			{
				string canceledStatusKey = "OrderStatus_Canceled";

				_hasExistingBookingForThisShowtime = await _dbContext.Orders
					.AnyAsync(o => o.UserId == _currentUser.Id &&
								   o.ScheduleId == _selectedSchedule.Id &&
								   o.OrderStatus != canceledStatusKey);
			}
			catch (Exception ex)
			{
				_hasExistingBookingForThisShowtime = false;
			}
			OnPropertyChanged(nameof(CanConfirmBooking));
			ConfirmBookingCommand.NotifyCanExecuteChanged();
		}

		private async Task LoadHallDataAsync()
		{
			try
			{
				_currentHall = _selectedSchedule.Hall;
				if (_currentHall == null && _selectedSchedule.HallId != Guid.Empty) 
				{
					_currentHall = await _dbContext.Halls.FindAsync(_selectedSchedule.HallId);
				}


				if (_currentHall != null)
				{
					string localizedHallName = GetResourceString(_currentHall.Name) ?? _currentHall.Name;
					HallName = localizedHallName;
					OnPropertyChanged(nameof(CurrentHallType));
				}
				else
				{
					HallName = GetResourceString("UnknownHall");
					OnPropertyChanged(nameof(CurrentHallType));
				}
			}
			catch (Exception ex)
			{
				HallName = GetResourceString("ErrorTitle");
				OnPropertyChanged(nameof(CurrentHallType));
			}
		}

		private async Task LoadBookedSeatsAsync()
		{
			var tempBookedIds = new HashSet<string>();
			try
			{
				string canceledStatusKey = "OrderStatus_Canceled";
				var bookedSeatStrings = await _dbContext.Orders
					.Where(o => o.ScheduleId == _selectedSchedule.Id &&
								o.OrderStatus != canceledStatusKey)
					.Select(o => o.Seats)
					.ToListAsync();

				foreach (var seatString in bookedSeatStrings.Where(s => !string.IsNullOrEmpty(s)))
				{
					var ids = seatString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
										.Select(s => s.Trim());
					foreach (var id in ids)
					{
						tempBookedIds.Add(id);
					}
				}
			}
			catch (Exception ex)
			{

			}
			finally 
			{
				BookedSeatIds = tempBookedIds;
				OnPropertyChanged(nameof(BookedSeatIds));
			}
		}

		private bool CanExecuteSelectSeat(string seatId)
		{
			if (string.IsNullOrEmpty(seatId) || BookedSeatIds.Contains(seatId)) 
				return false;

			bool isCurrentlySelected = _selectedSeatIdsInternal.Contains(seatId);
			if (_hasExistingBookingForThisShowtime) 
				return isCurrentlySelected;
			if (!isCurrentlySelected && _selectedSeatIdsInternal.Count >= MaxSelectedSeats)
				return false;

			return true;
		}

		private void ExecuteSelectSeat(string seatId)
		{
			if (string.IsNullOrEmpty(seatId)) return;

			if (_selectedSeatIdsInternal.Contains(seatId))
			{
				_selectedSeatIdsInternal.Remove(seatId);
			}
			else
			{
				if (_hasExistingBookingForThisShowtime)
				{
					ShowMessage("Booking_Error_AlreadyBookedCannotSelectNew", "InfoTitle", MessageBoxImage.Warning);
					return;
				}
				if (_selectedSeatIdsInternal.Count >= MaxSelectedSeats)
				{
					ShowMessage("Booking_Error_MaxSeatsSelected", "InfoTitle", MessageBoxImage.Warning);
					return;
				}
				_selectedSeatIdsInternal.Add(seatId);
			}
		}

		private async Task ExecuteConfirmBookingAsync()
		{
			if (_currentUser == null)
			{
				ShowMessage("Booking_Error_UserNotIdentified", "ErrorTitle", MessageBoxImage.Warning);
				return;
			}

			await RefreshCurrentUserBookingStatusAsync();

			if (!CanConfirmBooking) 
			{
				if (_hasExistingBookingForThisShowtime)
				{
					ShowMessage("Booking_Error_AlreadyBookedThisShowtime", "ErrorTitle", MessageBoxImage.Warning);
				}
				else if (_selectedSeatIdsInternal == null || !_selectedSeatIdsInternal.Any())
				{
					ShowMessage("SeatSelectionPage_NoSeatsSelected", "ErrorTitle", MessageBoxImage.Warning);
				}
				return;
			}

			var currentSelection = new List<string>(_selectedSeatIdsInternal);
			if (!currentSelection.Any())
			{
				ShowMessage("SeatSelectionPage_NoSeatsSelected", "ErrorTitle", MessageBoxImage.Warning);
				return;
			}

			try
			{
				var newOrder = new Order
				{
					Id = Guid.NewGuid(),
					Seats = string.Join(",", currentSelection.OrderBy(s => s)),
					TotalPrice = CalculateTotalPrice(currentSelection),
					OrderStatus = "OrderStatus_Booked", 
					BookingTimestamp = DateTime.Now,
					UserId = _currentUser.Id,
					ScheduleId = _selectedSchedule.Id
				};

				_dbContext.Orders.Add(newOrder);
				await _dbContext.SaveChangesAsync();

				try
				{
					await Task.Run(() => SendBookingConfirmationEmail(currentSelection.Count));
				}
				catch (Exception emailEx)
				{
				}

				ShowMessage("Booking_Success", "Success_Title", MessageBoxImage.Information);

				_hasExistingBookingForThisShowtime = true; 

				bool newItemsAddedToGlobalBooked = false;

				foreach (var bookedId in currentSelection)
				{
					if (BookedSeatIds.Add(bookedId)) newItemsAddedToGlobalBooked = true;
				}

				if (newItemsAddedToGlobalBooked)
				{
					BookedSeatIds = new HashSet<string>(BookedSeatIds); 
					OnPropertyChanged(nameof(BookedSeatIds));
				}
				_selectedSeatIdsInternal.Clear();

				UpdateSelectedSeatsText();

				CommandManager.InvalidateRequerySuggested();
			}
			catch (DbUpdateException dbEx)
			{
				ShowMessageFormat("Booking_Error_DbSave", "ErrorTitle", MessageBoxImage.Error, dbEx.InnerException?.Message ?? dbEx.Message);
				await RefreshBookedSeatsAndUserStatusAsync(); 
			}
			catch (Exception ex)
			{
				ShowMessageFormat("Booking_Error_General", "ErrorTitle", MessageBoxImage.Error, ex.Message);
			}
		}

		private void SendBookingConfirmationEmail(int seatCount)
		{
			if (_currentUser == null || string.IsNullOrEmpty(_currentUser.Email) || _selectedSchedule == null)
			{
				return;
			}

			try
			{
				string greeting = string.Format(GetResourceString("Email_Booking_Greeting") ?? "Здравствуйте, {0}!", _currentUser.Name);
				string bodyIntro = string.Format(GetResourceString("Email_Booking_BodyIntro") ?? "Вы успешно забронировали {0} мест(а) на следующий сеанс:", seatCount);
				string movieTitle = _selectedSchedule.Movie?.Title ?? GetResourceString("UnknownMovie");
				string showTimeDate = _selectedSchedule.ShowTime.ToString("dd MMMM yyyy", CultureInfo.CurrentUICulture); 
				string showTimeTime = _selectedSchedule.ShowTime.ToString("HH:mm", CultureInfo.CurrentUICulture);     
				string pleasantViewing = GetResourceString("Email_Booking_PleasantViewing") ?? "Приятного просмотра!";
				string feedbackRequest = GetResourceString("Email_Booking_FeedbackRequest") ?? "Пожалуйста, оставьте отзыв о фильме после сеанса.";
				string subject = GetResourceString("Email_Booking_Subject") ?? "Бронирование билета: успешно!";

				var mailFrom = new MailAddress("УКАЗАТЬ СВОЮ ПОЧТУ", GetResourceString("Email_SenderName") ?? "Кинотеатр MOON");
				var mailTo = new MailAddress(_currentUser.Email, _currentUser.Name);

				using (var message = new MailMessage(mailFrom, mailTo))
				{
					message.Subject = subject;
					message.Body = $"{greeting}\n{bodyIntro}\n\n" +
								   $"Фильм: {movieTitle}\n" +
								   $"Дата: {showTimeDate}\n" +
								   $"Время: {showTimeTime}\n\n" +
								   $"{pleasantViewing}\n{feedbackRequest}";
					message.IsBodyHtml = false;

					using (var client = new SmtpClient())
					{
						client.Host = "smtp.gmail.com";
						client.Port = 587;
						client.EnableSsl = true;
						client.DeliveryMethod = SmtpDeliveryMethod.Network;
						client.UseDefaultCredentials = false;
						client.Credentials = new NetworkCredential(mailFrom.Address, "УКАЗАТЬ СВОЙ 16-ЗНАЧНЫЙ КОД");

						client.Send(message);
					}
				}
			}
			catch (SmtpException smtpEx)
			{

			}
			catch (Exception ex)
			{

			}
		}

		private void ExecuteGoBack(object parameter)
		{
			var navService = GetNavigationServiceFromParameterOrWindow(parameter);
			if (navService?.CanGoBack == true) { navService.GoBack(); }
		}

		private void SelectedSeatIdsInternal_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			UpdateSelectedSeatsText();
			OnPropertyChanged(nameof(CanConfirmBooking));
			ConfirmBookingCommand.NotifyCanExecuteChanged();
			CommandManager.InvalidateRequerySuggested();
		}

		private void UpdateSelectedSeatsText()
		{
			if (_selectedSeatIdsInternal == null || !_selectedSeatIdsInternal.Any())
			{
				SelectedSeatsText = GetResourceString("SeatSelectionPage_NoSeatsSelected") ?? "Нет";
			}
			else
			{
				SelectedSeatsText = string.Join(", ", _selectedSeatIdsInternal
												 .OrderBy(ParseSeatRow)
												 .ThenBy(ParseSeatNumber)
												 .Select(FormatSeatId));
			}
		}

		private string FormatSeatId(string seatId)
		{
			if (TryParseSeatId(seatId, out int r, out int s))
			{
				return $"{GetResourceString("SeatSelectionPage_RowAbbreviation") ?? "Р"}{r} {GetResourceString("SeatSelectionPage_SeatAbbreviation") ?? "М"}{s}";
			}
			return seatId;
		}

		private int ParseSeatRow(string seatId) => TryParseSeatId(seatId, out int row, out _) ? row : int.MaxValue;   
		private int ParseSeatNumber(string seatId) => TryParseSeatId(seatId, out _, out int seat) ? seat : int.MaxValue;
		private bool TryParseSeatId(string seatId, out int row, out int seat)
		{
			row = 0; seat = 0;
			if (string.IsNullOrEmpty(seatId) || !seatId.StartsWith("R") || !seatId.Contains("S")) 
				return false;
			try 
			{ 
				int sIndex = seatId.IndexOf('S'); 
				return int.TryParse(seatId.Substring(1, sIndex - 1), out row) && int.TryParse(seatId.Substring(sIndex + 1), out seat);
			}
			catch 
			{ 
				return false; 
			}
		}

		private string GetSeatTypeFromId(string seatId)
		{
			if (_currentHall == null || string.IsNullOrEmpty(seatId)) return "standard";
			TryParseSeatId(seatId, out int row, out _);

			string hallTypeLower = _currentHall.Type?.ToLowerInvariant();

			switch (hallTypeLower)
			{
				case "small":
					if (row == 3) return "loveseat";
					if (row == 4) return "recliner";
					return "standard";
				case "medium":
					if (row == 1) return "sofa";
					if (row == 5) return "loveseat";
					return "standard";
				case "large":
					if (row == 1) return "sofa";
					if (row == 5) return "loveseat";
					if (row == 6) return "recliner";
					return "standard";
				default:
					return "standard";
			}
		}

		private decimal CalculateTotalPrice(IEnumerable<string> selectedSeatIds)
		{
			decimal totalPrice = 0;
			if (selectedSeatIds == null) return 0;

			foreach (var seatId in selectedSeatIds)
			{
				totalPrice += GetSeatPrice(GetSeatTypeFromId(seatId));
			}
			return totalPrice;
		}

		private decimal GetSeatPrice(string seatType)
		{
			const decimal standardPrice = 15m, lovePrice = 30m, sofaPrice = 35m, reclinerPrice = 20m;
			switch (seatType?.ToLowerInvariant())
			{
				case "loveseat": return lovePrice;
				case "sofa": return sofaPrice;
				case "recliner": return reclinerPrice;
				default: return standardPrice; 
			}
		}

		private string GetResourceString(string key) => Application.Current.TryFindResource(key) as string;

		private void ShowMessage(string msgKey, string titleKey, MessageBoxImage icon)
		{
			string m = GetResourceString(msgKey) ?? "[" + msgKey + "]";
			string t = GetResourceString(titleKey) ?? "[" + titleKey + "]";
			MessageBox.Show(m, t, MessageBoxButton.OK, icon); 
		}

		private void ShowMessageFormat(string msgKey, string titleKey, MessageBoxImage icon, params object[] args) 
		{
			try
			{
				string fmt = GetResourceString(msgKey) ?? msgKey; 
				string msg = string.Format(fmt, args); 
				string ttl = GetResourceString(titleKey) ?? titleKey; 
				MessageBox.Show(msg, ttl, MessageBoxButton.OK, icon); 
			}
			catch (FormatException) 
			{
				ShowMessage(msgKey + "(FmtErr)", titleKey, icon); 
			}
		}

		private NavigationService GetNavigationServiceFromParameterOrWindow(object p) 
		{
			if (p is Page pg && NavigationService.GetNavigationService(pg) != null) 
				return NavigationService.GetNavigationService(pg); 
			return GetNavigationServiceFromActiveWindow();
		}

		private NavigationService GetNavigationServiceFromActiveWindow() 
		{
			var win = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive) ?? Application.Current.MainWindow; 
			if (win is AdminWindow aw && aw.FindName("frameMainForAdmin") is Frame af) return af.NavigationService; 
			if (win is MainWindowForUsers uw && uw.FindName("frameMain") is Frame uf) return uf.NavigationService;
			if (win?.Content is Frame rf) return rf.NavigationService; 
			var frame = FindVisualChild<Frame>(win); 
			return frame != null ? NavigationService.GetNavigationService(frame) : null; 
		}

		public static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject 
		{
			if (parent == null) return null; 
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++) 
			{
				var child = VisualTreeHelper.GetChild(parent, i); 
				if (child is T t) return t; 
				var childOfChild = FindVisualChild<T>(child); 
				if (childOfChild != null) return childOfChild; 
			} 
			return null; 
		}
	}
}
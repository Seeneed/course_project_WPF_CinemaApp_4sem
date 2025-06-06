using CinemaMOON.Models;
using CinemaMOON.Views;
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
using System.Windows.Media;
using System.Windows.Navigation;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using CinemaMOON.Data;

namespace CinemaMOON.ViewModels
{
    public class AccountPageViewModel : ViewModelBase
	{
		private readonly AppDbContext _dbContext;
		private User _currentUser;
		private ObservableCollection<Order> _userOrders;
		private Order _selectedOrder;

		public string UserName => _currentUser?.Name ?? GetResourceString("N/A");
		public string UserSurname => _currentUser?.Surname ?? GetResourceString("N/A");
		public string UserEmail => _currentUser?.Email ?? GetResourceString("N/A");

		public ObservableCollection<Order> UserOrders
		{
			get => _userOrders;
			private set => SetProperty(ref _userOrders, value);
		}

		public Order SelectedOrder
		{
			get => _selectedOrder;
			set
			{
				if (SetProperty(ref _selectedOrder, value))
				{
					CancelOrderCommand.NotifyCanExecuteChanged();
					RateMovieCommand.NotifyCanExecuteChanged();
				}
			}
		}

		public ICommand ChangeLoginCommand { get; }
		public ICommand ChangePasswordCommand { get; }
		public ICommand LogoutCommand { get; }
		public ICommand SwitchLanguageCommand { get; }
		public ICommand ToggleThemeCommand { get; }
		public IAsyncRelayCommand CancelOrderCommand { get; }
		public IAsyncRelayCommand LoadOrdersCommand { get; }
		public IAsyncRelayCommand RateMovieCommand { get; }

		public AccountPageViewModel(AppDbContext dbContext, User currentUser)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
			UserOrders = new ObservableCollection<Order>();

			LoadOrdersCommand = new AsyncRelayCommand(InitializePageDataAsync);
			ChangeLoginCommand = new RelayCommand<Page>(ExecuteChangeLogin, CanExecuteAccountActions);
			ChangePasswordCommand = new RelayCommand<Page>(ExecuteChangePassword, CanExecuteAccountActions);
			LogoutCommand = new RelayCommand<Page>(ExecuteLogout, CanExecuteAccountActions);
			SwitchLanguageCommand = new RelayCommand<string>(ExecuteSwitchLanguage);
			ToggleThemeCommand = new RelayCommand(ExecuteToggleTheme);
			CancelOrderCommand = new AsyncRelayCommand(ExecuteCancelOrderAsync, CanExecuteCancelOrder);
			RateMovieCommand = new AsyncRelayCommand<Page>(ExecuteRateMovieAsync, CanExecuteRateMovie);
		}

		private bool CanExecuteRateMovie(Page pageParameter)
		{
			if (SelectedOrder == null)
			{
				return false;
			}

			bool isViewed = SelectedOrder.OrderStatus == "OrderStatus_Viewed";
			bool notRatedYet = !SelectedOrder.UserRating.HasValue;

			return isViewed && notRatedYet;
		}

		private async Task ExecuteRateMovieAsync(Page currentPage)
		{
			if (SelectedOrder == null)
			{
				return;
			}

			if (currentPage?.NavigationService != null)
			{
				var ratePage = new RateMoviePage(_dbContext, SelectedOrder, _currentUser);
				currentPage.NavigationService.Navigate(ratePage);
			}
			else
			{
				ShowMessage("AccountPage_Error_CannotNavigateOrUserMissing", "ErrorTitle", MessageBoxImage.Warning);
			}
			await Task.CompletedTask;
		}

		private bool CanExecuteToggleTheme(object parameter)
		{
			return true;
		}

		private void ExecuteToggleTheme(object parameter)
		{
			App.ToggleTheme();
		}

		private bool CanExecuteAccountActions(object parameter)
		{
			return _currentUser != null;
		}

		private bool CanExecuteCancelOrder()
		{
			if (SelectedOrder == null) return false;

			return SelectedOrder.OrderStatus == "OrderStatus_Booked";
		}

		private void ExecuteChangeLogin(object parameter)
		{
			if (parameter is Page currentPage && _currentUser != null)
			{
				var changeLoginPage = new ChangeLoginPage(_dbContext, _currentUser);
				currentPage.NavigationService?.Navigate(changeLoginPage);
			}
			else
			{
				ShowMessage("AccountPage_Error_CannotNavigateOrUserMissing", "ErrorTitle", MessageBoxImage.Warning);
			}
		}

		private void ExecuteChangePassword(object parameter)
		{
			if (parameter is Page currentPage && _currentUser != null)
			{
				var changePasswordPage = new ChangePasswordPage(_dbContext, _currentUser);
				currentPage.NavigationService?.Navigate(changePasswordPage);
			}
			else
			{
				ShowMessage("AccountPage_Error_CannotNavigateOrUserMissing", "ErrorTitle", MessageBoxImage.Warning);
			}
		}

		private void ExecuteLogout(object parameter)
		{
			_currentUser = null;

			Window windowToClose = null;
			if (parameter is Page currentPage)
			{
				windowToClose = Window.GetWindow(currentPage);
			}
			else
			{
				windowToClose = Application.Current.Windows.OfType<Window>()
											 .FirstOrDefault(w => w.IsActive && !(w is MainWindow));
				if (windowToClose == null)
				{
					windowToClose = Application.Current.Windows.OfType<Window>()
											 .FirstOrDefault(w => !(w is MainWindow));
				}
			}

			try
			{
				var dbContextForLogin = App.ServiceProvider.GetRequiredService<AppDbContext>();

				var loginViewModel = new MainWindowViewModel(dbContextForLogin);

				var loginWindow = new MainWindow
				{
					DataContext = loginViewModel
				};
				loginWindow.Show();

				windowToClose?.Close();
			}
			catch (Exception ex)
			{
				ShowMessageFormat("AccountPage_Error_LogoutFailed_Generic", "AccountPage_Error_LogoutTitle", MessageBoxImage.Error, ex.Message);
			}
		}

		private void ExecuteSwitchLanguage(string cultureCode)
		{
			if (!string.IsNullOrEmpty(cultureCode))
			{
				try
				{
					App.Language = new CultureInfo(cultureCode);

					UpdateLocalization();

					_ = LoadUserOrdersAsync();
				}
				catch (Exception ex)
				{
					ShowMessageFormat("LanguageSwitchErrorGeneric", "ErrorTitle", MessageBoxImage.Error, ex.Message);
				}
			}
		}

		private async Task ExecuteCancelOrderAsync()
		{
			if (!CanExecuteCancelOrder()) return;

			var orderToCancel = SelectedOrder;

			string movieTitle = orderToCancel.Schedule?.Movie?.Title ?? GetResourceString("UnknownMovie");
			string question = string.Format(GetResourceString("AccountPage_Confirm_CancelOrder") ?? "Отменить бронь: {0}?", movieTitle);
			string title = GetResourceString("AccountPage_Confirm_CancelOrderTitle") ?? "Отмена";

			if (MessageBox.Show(question, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
			{
				try
				{
					var orderInDb = await _dbContext.Orders.FindAsync(orderToCancel.Id);

					if (orderInDb != null)
					{
						orderInDb.OrderStatus = "OrderStatus_Canceled";
						await _dbContext.SaveChangesAsync();

						await LoadUserOrdersAsync();
					}
					else
					{
						ShowMessage("AccountPage_Error_OrderNotFoundInDb", "ErrorTitle", MessageBoxImage.Warning);
						await LoadUserOrdersAsync();
					}

					ShowMessage("AccountPage_Success_OrderCanceled", "AccountPage_Success_Title", MessageBoxImage.Information);
				}
				catch (DbUpdateException dbEx)
				{
					ShowMessageFormat("AccountPage_Error_CancelDbFailed", "ErrorTitle", MessageBoxImage.Error, dbEx.InnerException?.Message ?? dbEx.Message);
				}
				catch (Exception ex)
				{
					ShowMessageFormat("AccountPage_Error_CancelOrderFailed", "ErrorTitle", MessageBoxImage.Error, ex.Message);
				}
			}
		}

		private async Task InitializePageDataAsync()
		{
			UpdateLocalization();
			await LoadUserOrdersAsync();
		}

		private async Task LoadUserOrdersAsync()
		{
			UserOrders.Clear();
			SelectedOrder = null;
			if (_currentUser == null)
			{
				return;
			}

			try
			{
				var orders = await _dbContext.Orders
					.Where(o => o.UserId == _currentUser.Id)
					.Include(o => o.Schedule)
						.ThenInclude(s => s.Movie)
					.Include(o => o.Schedule)
						.ThenInclude(s => s.Hall)
					.OrderByDescending(o => o.BookingTimestamp)
					.ToListAsync();

				UserOrders = new ObservableCollection<Order>(orders);
			}
			catch (Exception ex)
			{
				ShowMessageFormat("AccountPage_Error_LoadingOrders", "AdminPanel_Title_Error", MessageBoxImage.Error, ex.Message);
				UserOrders.Clear();
			}
			finally
			{
				CancelOrderCommand.NotifyCanExecuteChanged();
				RateMovieCommand.NotifyCanExecuteChanged();
			}
		}

		private string GetResourceString(string key)
		{
			return Application.Current.TryFindResource(key) as string ?? $"[{key}]";
		}

		private void ShowMessage(string messageKey, string titleKey, MessageBoxImage icon)
		{
			MessageBox.Show(GetResourceString(messageKey), GetResourceString(titleKey), MessageBoxButton.OK, icon);
		}

		private void ShowMessageFormat(string messageKey, string titleKey, MessageBoxImage icon, params object[] args)
		{
			string messageFormat = GetResourceString(messageKey);
			string message = string.Format(messageFormat, args);
			MessageBox.Show(message, GetResourceString(titleKey), MessageBoxButton.OK, icon);
		}

		public void UpdateLocalization()
		{
			OnPropertyChanged(nameof(UserName));
			OnPropertyChanged(nameof(UserSurname));
			OnPropertyChanged(nameof(UserEmail));
			CommandManager.InvalidateRequerySuggested();
		}
	}
}
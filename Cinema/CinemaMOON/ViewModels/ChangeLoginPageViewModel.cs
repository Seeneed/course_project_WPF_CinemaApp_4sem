using CinemaMOON.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using CinemaMOON.Views;
using CinemaMOON.Data;

namespace CinemaMOON.ViewModels
{
    public class ChangeLoginPageViewModel : ViewModelBase
	{
		private readonly AppDbContext _dbContext;

		private readonly User _currentUser;
		private string _newEmail;
		private string _newEmailError;
		private bool _isNewEmailErrorVisible;

		private readonly Regex _emailRegex = new Regex(@"^([a-z0-9_\.-]+)@([a-z0-9_\.-]+)\.([a-z\.]{2,3})$", RegexOptions.IgnoreCase);

		public string NewEmail
		{
			get => _newEmail;
			set
			{
				if (SetProperty(ref _newEmail, value?.Trim()))
				{
					ValidateNewEmailAsync();
				}
			}
		}

		public string NewEmailError
		{
			get => _newEmailError;
			private set => SetProperty(ref _newEmailError, value);
		}

		public bool IsNewEmailErrorVisible
		{
			get => _isNewEmailErrorVisible;
			private set => SetProperty(ref _isNewEmailErrorVisible, value);
		}

		public IAsyncRelayCommand ChangeLoginCommand { get; }
		public ICommand GoBackCommand { get; }

		public ChangeLoginPageViewModel(AppDbContext dbContext, User currentUser)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser), "Текущий пользователь не может быть null.");

			ChangeLoginCommand = new AsyncRelayCommand(ExecuteChangeLoginAsync, CanExecuteChangeLogin);
			GoBackCommand = new RelayCommand<Page>(ExecuteGoBack);

			App.LanguageChanged += OnLanguageChanged;
		}

		private void OnLanguageChanged(object sender, EventArgs e)
		{
			ValidateNewEmailAsync();
		}

		private async Task<bool> ValidateNewEmailAsync()
		{
			ResetSpecificErrors();

			if (string.IsNullOrWhiteSpace(NewEmail))
			{
				SetValidationError(GetResourceString("Validation_Error_Required"));
				return false;
			}

			if (!_emailRegex.IsMatch(NewEmail))
			{
				SetValidationError(GetResourceString("ErrorEmailInvalid"));
				return false;
			}

			if (NewEmail.Equals(_currentUser.Email, StringComparison.OrdinalIgnoreCase))
			{
				SetValidationError(GetResourceString("ChangeLoginPage_Error_SameAsCurrent"));
				return false;
			}

			try
			{
				bool emailExists = await _dbContext.Users
										   .AnyAsync(u => u.Id != _currentUser.Id
													   && u.Email == NewEmail);
				if (emailExists)
				{
					SetValidationError(GetResourceString("ErrorEmailExists"));
					return false;
				}
			}
			catch (Exception ex)
			{
				SetValidationError(GetResourceString("ErrorDbCheckFailed"));
				return false;
			}

			ResetValidationError();
			return true;
		}

		private void SetValidationError(string errorMessage)
		{
			NewEmailError = errorMessage;
			IsNewEmailErrorVisible = true;
			ChangeLoginCommand.NotifyCanExecuteChanged();
		}

		private void ResetValidationError()
		{
			NewEmailError = string.Empty;
			IsNewEmailErrorVisible = false;
			ChangeLoginCommand.NotifyCanExecuteChanged();
		}

		private void ResetSpecificErrors()
		{
			if (IsNewEmailErrorVisible)
			{
				string sameAsCurrentError = GetResourceString("ChangeLoginPage_Error_SameAsCurrent");
				string existsError = GetResourceString("ErrorEmailExists");
				string dbCheckError = GetResourceString("ErrorDbCheckFailed");

				if (NewEmailError == sameAsCurrentError || NewEmailError == existsError || NewEmailError == dbCheckError)
				{
					ResetValidationError();
				}
			}
		}

		private bool CanExecuteChangeLogin()
		{
			return !IsNewEmailErrorVisible && !string.IsNullOrWhiteSpace(NewEmail);
		}

		private async Task ExecuteChangeLoginAsync()
		{
			if (!await ValidateNewEmailAsync())
			{
				return;
			}

			try
			{
				var userToUpdate = await _dbContext.Users.FindAsync(_currentUser.Id);

				if (userToUpdate == null)
				{
					ShowMessage("AccountPage_Error_UserDataUnavailable", "AdminPanel_Title_Error", MessageBoxImage.Error);
					return;
				}

				userToUpdate.Email = NewEmail;

				await _dbContext.SaveChangesAsync();

				_currentUser.Email = NewEmail;

				ShowMessage("ChangeLoginPage_Success_LoginChanged", "SuccessRegistrationTitle", MessageBoxImage.Information);

				ExecuteGoBack(null);

			}
			catch (DbUpdateException dbEx)
			{
				ShowMessageFormat("ChangeLoginPage_Error_UpdateDbFailed", "AdminPanel_Title_Error", MessageBoxImage.Error, dbEx.InnerException?.Message ?? dbEx.Message);
			}
			catch (Exception ex)
			{
				ShowMessageFormat("ChangeLoginPage_Error_UpdateFailed", "AdminPanel_Title_Error", MessageBoxImage.Error, ex.Message);
			}
		}

		private void ExecuteGoBack(object parameter)
		{
			Page currentPage = parameter as Page;
			if (currentPage == null)
			{
				var frame = Application.Current.MainWindow?.Content as Frame;
				currentPage = frame?.Content as Page;
			}

			if (currentPage?.NavigationService?.CanGoBack ?? false)
			{
				currentPage.NavigationService.GoBack();
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
			try
			{
				string message = string.Format(messageFormat, args);
				MessageBox.Show(message, GetResourceString(titleKey), MessageBoxButton.OK, icon);
			}
			catch (FormatException)
			{
				MessageBox.Show($"{GetResourceString(messageKey)} (Format Error)", GetResourceString(titleKey), MessageBoxButton.OK, icon);
			}
		}

		~ChangeLoginPageViewModel()
		{
			App.LanguageChanged -= OnLanguageChanged;
		}
	}
}
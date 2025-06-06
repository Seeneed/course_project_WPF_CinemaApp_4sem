using CinemaMOON.Models;
using CinemaMOON.Services;
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
using CinemaMOON.Data;

namespace CinemaMOON.ViewModels
{
    public class ChangePasswordPageViewModel : ViewModelBase
	{
		private readonly AppDbContext _dbContext;
		private readonly User _currentUser;

		private string _currentOldPassword = string.Empty;
		private string _currentNewPassword = string.Empty;
		private string _currentConfirmPassword = string.Empty;

		private string _oldPasswordError;
		private string _newPasswordError;
		private string _confirmPasswordError;
		private bool _isOldPasswordErrorVisible;
		private bool _isNewPasswordErrorVisible;
		private bool _isConfirmPasswordErrorVisible;

		private readonly Regex _passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,20}$");

		public string OldPasswordError
		{
			get => _oldPasswordError; 
			private set => SetProperty(ref _oldPasswordError, value); 
		}

		public string NewPasswordError 
		{
			get => _newPasswordError; 
			private set => SetProperty(ref _newPasswordError, value); 
		}

		public string ConfirmPasswordError 
		{ 
			get => _confirmPasswordError;
			private set => SetProperty(ref _confirmPasswordError, value); 
		}

		public bool IsOldPasswordErrorVisible
		{ 
			get => _isOldPasswordErrorVisible; 
			private set => SetProperty(ref _isOldPasswordErrorVisible, value); 
		}

		public bool IsNewPasswordErrorVisible 
		{ 
			get => _isNewPasswordErrorVisible; 
			private set => SetProperty(ref _isNewPasswordErrorVisible, value); 
		}

		public bool IsConfirmPasswordErrorVisible 
		{ 
			get => _isConfirmPasswordErrorVisible;
			private set => SetProperty(ref _isConfirmPasswordErrorVisible, value); 
		}

		public IAsyncRelayCommand ChangePasswordCommand { get; } 
		public ICommand GoBackCommand { get; }
		public ICommand OldPasswordChangedCommand { get; }
		public ICommand NewPasswordChangedCommand { get; }
		public ICommand ConfirmPasswordChangedCommand { get; }

		public ChangePasswordPageViewModel(AppDbContext dbContext, User currentUser)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser), "Текущий пользователь не может быть null.");

			ChangePasswordCommand = new AsyncRelayCommand(ExecuteChangePasswordAsync, CanExecuteChangePassword);
			GoBackCommand = new RelayCommand<Page>(ExecuteGoBack);

			OldPasswordChangedCommand = new RelayCommand(ExecuteOldPasswordChanged);
			NewPasswordChangedCommand = new RelayCommand(ExecuteNewPasswordChanged);
			ConfirmPasswordChangedCommand = new RelayCommand(ExecuteConfirmPasswordChanged);

			App.LanguageChanged += OnLanguageChanged;
			ResetValidationMessages();
		}

		private void OnLanguageChanged(object sender, EventArgs e)
		{
			ResetValidationMessages();
			ValidateOldPassword(_currentOldPassword);
			ValidateNewPassword(_currentNewPassword);
			ValidateConfirmPassword(_currentNewPassword, _currentConfirmPassword);
		}

		private void ExecuteOldPasswordChanged(object parameter)
		{
			if (parameter is object[] controls && controls.Length > 0 && controls[0] is PasswordBox oldPassBox)
			{
				_currentOldPassword = oldPassBox.Password;
				ValidateOldPassword(_currentOldPassword);
			}
		}

		private void ExecuteNewPasswordChanged(object parameter)
		{
			if (parameter is object[] controls && controls.Length > 1 &&
				controls[1] is PasswordBox newPassBox && controls[2] is PasswordBox confirmPassBox)
			{
				_currentNewPassword = newPassBox.Password;
				_currentConfirmPassword = confirmPassBox.Password;
				ValidateNewPassword(_currentNewPassword);
				ValidateConfirmPassword(_currentNewPassword, _currentConfirmPassword);
			}
		}

		private void ExecuteConfirmPasswordChanged(object parameter)
		{
			if (parameter is object[] controls && controls.Length > 1 &&
			   controls[1] is PasswordBox newPassBox && controls[2] is PasswordBox confirmPassBox)
			{
				_currentNewPassword = newPassBox.Password;
				_currentConfirmPassword = confirmPassBox.Password;
				ValidateConfirmPassword(_currentNewPassword, _currentConfirmPassword);
			}
		}

		private bool ValidateOldPassword(string password)
		{
			ResetSpecificError(nameof(OldPasswordError), nameof(IsOldPasswordErrorVisible), "ChangePasswordPage_Error_IncorrectOldPassword");

			if (string.IsNullOrWhiteSpace(password))
			{
				SetValidationError(nameof(OldPasswordError), nameof(IsOldPasswordErrorVisible), GetResourceString("Validation_Error_Required"));
				return false;
			}
			ResetValidationError(nameof(OldPasswordError), nameof(IsOldPasswordErrorVisible));
			return true;
		}

		private bool ValidateNewPassword(string password)
		{
			ResetSpecificError(nameof(NewPasswordError), nameof(IsNewPasswordErrorVisible), "ChangePasswordPage_Error_SameAsOld");

			if (string.IsNullOrWhiteSpace(password))
			{
				SetValidationError(nameof(NewPasswordError), nameof(IsNewPasswordErrorVisible), GetResourceString("Validation_Error_Required"));
				return false;
			}
			if (!_passwordRegex.IsMatch(password))
			{
				SetValidationError(nameof(NewPasswordError), nameof(IsNewPasswordErrorVisible), GetResourceString("ErrorPasswordFormat"));
				return false;
			}

			ResetValidationError(nameof(NewPasswordError), nameof(IsNewPasswordErrorVisible));
			return true;
		}

		private bool ValidateConfirmPassword(string newPassword, string confirmPassword)
		{
			if (string.IsNullOrWhiteSpace(confirmPassword))
			{
				SetValidationError(nameof(ConfirmPasswordError), nameof(IsConfirmPasswordErrorVisible), GetResourceString("Validation_Error_Required"));
				return false;
			}
			if (newPassword != confirmPassword)
			{
				SetValidationError(nameof(ConfirmPasswordError), nameof(IsConfirmPasswordErrorVisible), GetResourceString("ErrorConfirmPasswordMismatch"));
				return false;
			}
			ResetValidationError(nameof(ConfirmPasswordError), nameof(IsConfirmPasswordErrorVisible));
			return true;
		}

		private void SetValidationError(string errorProp, string visibilityProp, string message)
		{ 
			GetType().GetProperty(errorProp)?.SetValue(this, message);
			GetType().GetProperty(visibilityProp)?.SetValue(this, true); 
			ChangePasswordCommand.NotifyCanExecuteChanged();
		}

		private void ResetValidationError(string errorProp, string visibilityProp)
		{ 
			GetType().GetProperty(errorProp)?.SetValue(this, string.Empty);
			GetType().GetProperty(visibilityProp)?.SetValue(this, false); 
			ChangePasswordCommand.NotifyCanExecuteChanged(); 
		}

		private void ResetSpecificError(string errorProp, string visibilityProp, string specificErrorKey)
		{ 
			if ((bool)(GetType().GetProperty(visibilityProp)?.GetValue(this) ?? false) && GetType().GetProperty(errorProp)?.GetValue(this) as string == GetResourceString(specificErrorKey)) 
				ResetValidationError(errorProp, visibilityProp); 
		}

		private bool CanExecuteChangePassword()
		{
			return !IsOldPasswordErrorVisible &&
				   !IsNewPasswordErrorVisible &&
				   !IsConfirmPasswordErrorVisible &&
				   !string.IsNullOrEmpty(_currentOldPassword) &&
				   !string.IsNullOrEmpty(_currentNewPassword) &&
				   !string.IsNullOrEmpty(_currentConfirmPassword);
		}

		private async Task ExecuteChangePasswordAsync()
		{
			bool isOldValid = ValidateOldPassword(_currentOldPassword);
			bool isNewValid = ValidateNewPassword(_currentNewPassword);
			bool isConfirmValid = ValidateConfirmPassword(_currentNewPassword, _currentConfirmPassword);

			if (!isOldValid || !isNewValid || !isConfirmValid)
			{
				return; 
			}

			var userFromDb = await _dbContext.Users.FindAsync(_currentUser.Id);
			if (userFromDb == null)
			{
				ShowMessage("AccountPage_Error_UserDataUnavailable", "AdminPanel_Title_Error", MessageBoxImage.Error);
				return;
			}

			if (!PasswordHasher.VerifyPassword(_currentOldPassword, userFromDb.PasswordHash, userFromDb.Salt))
			{
				SetValidationError(nameof(OldPasswordError), nameof(IsOldPasswordErrorVisible), GetResourceString("ChangePasswordPage_Error_IncorrectOldPassword"));
				return; 
			}

			ResetValidationError(nameof(OldPasswordError), nameof(IsOldPasswordErrorVisible));

			if (_currentOldPassword == _currentNewPassword)
			{
				SetValidationError(nameof(NewPasswordError), nameof(IsNewPasswordErrorVisible), GetResourceString("ChangePasswordPage_Error_SameAsOld"));
				return; 
			}

			ResetValidationError(nameof(NewPasswordError), nameof(IsNewPasswordErrorVisible));

			try
			{
				var (newHash, newSalt) = PasswordHasher.HashPassword(_currentNewPassword);

				userFromDb.PasswordHash = newHash;
				userFromDb.Salt = newSalt;

				await _dbContext.SaveChangesAsync();

				_currentUser.PasswordHash = newHash;
				_currentUser.Salt = newSalt;

				ShowMessage("ChangePasswordPage_Success_PasswordChanged", "SuccessRegistrationTitle", MessageBoxImage.Information);

				ExecuteGoBack(null);

			}
			catch (DbUpdateException dbEx)
			{
				ShowMessageFormat("ChangePasswordPage_Error_UpdateDbFailed", "AdminPanel_Title_Error", MessageBoxImage.Error, dbEx.InnerException?.Message ?? dbEx.Message);
			}
			catch (Exception ex)
			{
				ShowMessageFormat("ChangePasswordPage_Error_UpdateFailed", "AdminPanel_Title_Error", MessageBoxImage.Error, ex.Message);
			}
		}

		private Page GetAssociatedPage(object commandParameter)
		{
			if (commandParameter is object[] controls && controls.Length > 0 && controls[0] is FrameworkElement element)
			{
				DependencyObject parent = element;
				while (parent != null && !(parent is Page))
				{
					parent = LogicalTreeHelper.GetParent(parent) ?? VisualTreeHelper.GetParent(parent);
				}
				return parent as Page;
			}
			return null;
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

		private void ResetValidationMessages()
		{
			OldPasswordError = NewPasswordError = ConfirmPasswordError = string.Empty;
			IsOldPasswordErrorVisible = IsNewPasswordErrorVisible = IsConfirmPasswordErrorVisible = false;
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

		~ChangePasswordPageViewModel()
		{
			App.LanguageChanged -= OnLanguageChanged;
		}
	}
}
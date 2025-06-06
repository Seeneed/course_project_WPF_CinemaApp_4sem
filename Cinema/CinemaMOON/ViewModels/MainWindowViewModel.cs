using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaMOON.Models;
using CinemaMOON.Views;
using CinemaMOON.Services;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using CinemaMOON.Data;

namespace CinemaMOON.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
	{
		private string _email;
		private string _emailError;
		private bool _isEmailErrorVisible;
		private string _passwordError;
		private bool _isPasswordErrorVisible;

		private readonly Regex _emailRegex = new Regex(@"^([a-z0-9_\.-]+)@([a-z0-9_\.-]+)\.([a-z\.]{2,3})$", RegexOptions.IgnoreCase);

		private readonly AppDbContext _dbContext;

		public string Email
		{
			get => _email;
			set
			{
				if (SetProperty(ref _email, value))
				{
					ValidateEmailInput();
				}
			}
		}
		public string EmailError
		{ 
			get => _emailError;
			private set => SetProperty(ref _emailError, value); 
		}

		public bool IsEmailErrorVisible 
		{ 
			get => _isEmailErrorVisible; 
			private set => SetProperty(ref _isEmailErrorVisible, value); 
		}

		public string PasswordError 
		{
			get => _passwordError; 
			private set => SetProperty(ref _passwordError, value); 
		}

		public bool IsPasswordErrorVisible
		{
			get => _isPasswordErrorVisible; 
			private set => SetProperty(ref _isPasswordErrorVisible, value);
		}

		public ICommand LoginCommand { get; }
		public ICommand RegisterCommand { get; }
		public ICommand SwitchLanguageCommand { get; }
		public ICommand PasswordChangedCommand { get; }

		public MainWindowViewModel(AppDbContext dbContext)
		{
			_dbContext = dbContext;

			LoginCommand = new RelayCommand(async (param) => await ExecuteLogin(param), CanExecuteLogin);
			RegisterCommand = new RelayCommand(ExecuteRegister);
			SwitchLanguageCommand = new RelayCommand(ExecuteSwitchLanguage);
			PasswordChangedCommand = new RelayCommand(ExecutePasswordChanged);
			App.LanguageChanged += OnLanguageChanged;
		}

		private void OnLanguageChanged(object sender, EventArgs e)
		{
			ResetValidationMessages();
		}

		private void ExecuteSwitchLanguage(object parameter)
		{
			if (parameter is string langCode)
			{
				try { App.Language = new CultureInfo(langCode); }
				catch (Exception ex) { MessageBox.Show($"Error switching language: {ex.Message}"); }
			}
		}

		private void ExecuteRegister(object parameter)
		{
			ResetValidationMessages();
			var registrationViewModel = new RegistrationWindowViewModel(_dbContext);

			var registrationWindow = new RegistrationWindow
			{
				DataContext = registrationViewModel
			};
			registrationWindow.Show();
			CloseCurrentWindow();
		}

		private void ExecutePasswordChanged(object parameter)
		{
			var passwordBox = parameter as PasswordBox;
			if (passwordBox == null) return;
			ValidatePasswordInput(passwordBox.Password);
		}

		private async Task ExecuteLogin(object parameter)
		{
			var passwordBox = parameter as PasswordBox;
			if (passwordBox == null) return;
			string password = passwordBox.Password;

			ResetLoginAttemptErrors();

			bool isEmailValid = ValidateEmailInput();
			bool isPasswordValid = ValidatePasswordInput(password);

			if (!isEmailValid || !isPasswordValid) return;

			try
			{
				string emailToCompare = Email.ToLower();

				var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == emailToCompare);

				if (user == null)
				{
					EmailError = GetResourceString("ErrorEmailNotFound");
					IsEmailErrorVisible = true;
					return;
				}

				if (!PasswordHasher.VerifyPassword(password, user.PasswordHash, user.Salt))
				{
					PasswordError = GetResourceString("ErrorPasswordIncorrect");
					IsPasswordErrorVisible = true;
					return;
				}

				if (user.IsAdmin)
				{
					OpenAdminWindow();
				}
				else
				{
					OpenUserWindow(user);
				}

				CloseCurrentWindow();
			}
			catch (Exception ex)
			{
				PasswordError = string.Format(GetResourceString("ErrorLoginGeneral"), ex.Message);
				IsPasswordErrorVisible = true;
			}
		}

		private bool CanExecuteLogin(object parameter) => true;

		private bool ValidateEmailInput()
		{
			string errorEmailNotFound = GetResourceString("ErrorEmailNotFound");
			if (!string.IsNullOrEmpty(EmailError) && EmailError == errorEmailNotFound)
			{
				EmailError = string.Empty;
				IsEmailErrorVisible = false;
			}

			if (string.IsNullOrWhiteSpace(Email))
			{
				EmailError = GetResourceString("ErrorEmailEmpty");
				IsEmailErrorVisible = true;
				return false;
			}

			if (!_emailRegex.IsMatch(Email))
			{
				EmailError = GetResourceString("ErrorEmailInvalid");
				IsEmailErrorVisible = true;
				return false;
			}

			EmailError = string.Empty;
			IsEmailErrorVisible = false;
			return true;
		}

		private bool ValidatePasswordInput(string password)
		{
			ResetLoginAttemptErrors();

			if (string.IsNullOrWhiteSpace(password))
			{
				PasswordError = GetResourceString("ErrorPasswordEmpty");
				IsPasswordErrorVisible = true;
				return false;
			}

			PasswordError = string.Empty;
			IsPasswordErrorVisible = false;
			return true;
		}

		private string GetResourceString(string key)
		{
			return Application.Current.TryFindResource(key) as string ?? $"[{key}]";
		}

		private void ResetLoginAttemptErrors()
		{
			string errorEmailNotFound = GetResourceString("ErrorEmailNotFound");
			if (!string.IsNullOrEmpty(EmailError) && EmailError == errorEmailNotFound)
			{
				EmailError = string.Empty;
				IsEmailErrorVisible = false;
			}

			string errorPasswordIncorrect = GetResourceString("ErrorPasswordIncorrect");
			string errorLoginGeneralFormat = GetResourceString("ErrorLoginGeneral");

			if (!string.IsNullOrEmpty(PasswordError))
			{
				bool resetPasswordError = false;
				if (PasswordError == errorPasswordIncorrect)
				{
					resetPasswordError = true;
				}
				else if (errorLoginGeneralFormat != null && PasswordError.StartsWith(errorLoginGeneralFormat.Split('{')[0]))
				{
					resetPasswordError = true;
				}

				if (resetPasswordError)
				{
					PasswordError = string.Empty;
					IsPasswordErrorVisible = false;
				}
			}
		}

		private void ResetValidationMessages()
		{
			EmailError = string.Empty;
			IsEmailErrorVisible = false;
			PasswordError = string.Empty;
			IsPasswordErrorVisible = false;
		}

		private void OpenUserWindow(User loggedInUser)
		{
			var userWindow = new MainWindowForUsers(loggedInUser);
			userWindow.Show();
		}

		private void OpenAdminWindow()
		{
			new AdminWindow().Show();
		}

		private void CloseCurrentWindow()
		{
			var window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault(w => w.DataContext == this);
			window?.Close();
		}

		~MainWindowViewModel()
		{
			App.LanguageChanged -= OnLanguageChanged;
		}
	}
}
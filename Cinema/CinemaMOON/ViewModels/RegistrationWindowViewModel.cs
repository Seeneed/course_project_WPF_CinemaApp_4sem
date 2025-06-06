using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CinemaMOON.Models;
using CinemaMOON.Services;
using CinemaMOON.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using CinemaMOON.Data;

namespace CinemaMOON.ViewModels
{
    public class RegistrationWindowViewModel : ViewModelBase
	{
		private string _name;
		private string _surname;
		private string _email;

		private string _nameError;
		private string _surnameError;
		private string _emailError;
		private string _passwordError;
		private string _confirmPasswordError;

		private bool _isNameErrorVisible;
		private bool _isSurnameErrorVisible;
		private bool _isEmailErrorVisible;
		private bool _isPasswordErrorVisible;
		private bool _isConfirmPasswordErrorVisible;

		private readonly Regex _nameRegex = new Regex(@"^[a-zA-Zа-яА-ЯёЁ]{2,20}$");
		private readonly Regex _surnameRegex = new Regex(@"^[a-zA-Zа-яА-ЯёЁ]{2,25}(-[a-zA-Zа-яА-ЯёЁ]{2,})?$");
		private readonly Regex _emailRegex = new Regex(@"^([a-z0-9_\.-]+)@([a-z0-9_\.-]+)\.([a-z\.]{2,3})$", RegexOptions.IgnoreCase);
		private readonly Regex _passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,20}$");

		public string Name
		{
			get => _name;
			set
			{
				string formattedValue = CapitalizeFirstLetter(value);
				if (SetProperty(ref _name, formattedValue)) { ValidateName(); }
			}
		}
		public string Surname
		{
			get => _surname;
			set
			{
				string formattedValue = CapitalizeFirstLetter(value);
				if (SetProperty(ref _surname, formattedValue)) { ValidateSurname(); }
			}
		}
		public string Email
		{
			get => _email;
			set
			{
				if (SetProperty(ref _email, value)) { ValidateEmail(); }
			}
		}

		public string NameError
		{
			get => _nameError; 
			private set => SetProperty(ref _nameError, value); 
		}

		public string SurnameError 
		{ 
			get => _surnameError;
			private set => SetProperty(ref _surnameError, value); 
		}

		public string EmailError 
		{ 
			get => _emailError;
			private set => SetProperty(ref _emailError, value); 
		}

		public string PasswordError 
		{
			get => _passwordError;
			private set => SetProperty(ref _passwordError, value); 
		}

		public string ConfirmPasswordError 
		{
			get => _confirmPasswordError; 
			private set => SetProperty(ref _confirmPasswordError, value);
		}

		public bool IsNameErrorVisible 
		{
			get => _isNameErrorVisible; 
			private set => SetProperty(ref _isNameErrorVisible, value);
		}

		public bool IsSurnameErrorVisible 
		{
			get => _isSurnameErrorVisible; 
			private set => SetProperty(ref _isSurnameErrorVisible, value); 
		}

		public bool IsEmailErrorVisible 
		{
			get => _isEmailErrorVisible; 
			private set => SetProperty(ref _isEmailErrorVisible, value);
		}

		public bool IsPasswordErrorVisible 
		{
			get => _isPasswordErrorVisible; 
			private set => SetProperty(ref _isPasswordErrorVisible, value); 
		}

		public bool IsConfirmPasswordErrorVisible 
		{
			get => _isConfirmPasswordErrorVisible;
			private set => SetProperty(ref _isConfirmPasswordErrorVisible, value); 
		}

		public ICommand RegisterCommand { get; }
		public ICommand BackCommand { get; }
		public ICommand SwitchLanguageCommand { get; }
		public ICommand PasswordChangedCommand { get; }
		public ICommand ConfirmPasswordChangedCommand { get; }

		private readonly AppDbContext _dbContext;

		public RegistrationWindowViewModel(AppDbContext dbContext)
		{
			_dbContext = dbContext;

			RegisterCommand = new RelayCommand(async (param) => await ExecuteRegister(param), CanExecuteRegister); BackCommand = new RelayCommand(ExecuteBack);
			SwitchLanguageCommand = new RelayCommand(ExecuteSwitchLanguage);
			PasswordChangedCommand = new RelayCommand(ExecutePasswordChanged);
			ConfirmPasswordChangedCommand = new RelayCommand(ExecuteConfirmPasswordChanged);

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
				try
				{
					App.Language = new CultureInfo(langCode);
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Error switching language: {ex.Message}", "Language Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		private void ExecuteBack(object parameter)
		{
			NavigateToLoginWindow();
		}

		private async Task ExecuteRegister(object parameter)
		{
			if (!(parameter is object[] controls) || controls.Length < 2 ||
				!(controls[0] is PasswordBox passBox) || !(controls[1] is PasswordBox confirmPassBox))
			{
				MessageBox.Show(
					GetResourceString("ErrorRegistrationGeneral").Replace("{0}", "Internal error: Password controls missing."),
					"Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			string password = passBox.Password;
			string confirmPassword = confirmPassBox.Password;

			bool isFormValid = ValidateAll(password, confirmPassword);

			if (!isFormValid)
			{
				return;
			}

			try
			{
				bool isEmailTaken = await _dbContext.Users.AnyAsync(u => u.Email == Email);
				if (isEmailTaken)
				{
					EmailError = GetResourceString("ErrorEmailExists");
					IsEmailErrorVisible = true;
					return;
				}

				var (hash, salt) = PasswordHasher.HashPassword(password);

				var newUser = new User
				{
					Id = Guid.NewGuid(),
					Name = Name,
					Surname = Surname,
					Email = Email.Trim(),
					PasswordHash = hash,
					Salt = salt,
					IsAdmin = false
				};

				await _dbContext.Users.AddAsync(newUser);
				await _dbContext.SaveChangesAsync();

				MessageBox.Show(
					GetResourceString("SuccessRegistrationMessage"),
					GetResourceString("SuccessRegistrationTitle"),
					MessageBoxButton.OK, MessageBoxImage.Information);

				NavigateToLoginWindow();
			}
			catch (DbUpdateException dbEx)
			{
				MessageBox.Show(
					string.Format(GetResourceString("ErrorRegistrationGeneral"),
					$"Database error: {dbEx.InnerException?.Message ?? dbEx.Message}"),
					"Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					string.Format(GetResourceString("ErrorRegistrationGeneral"), ex.Message),
					"Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private bool CanExecuteRegister(object parameter) => true;

		private void ExecutePasswordChanged(object parameter)
		{
			if (parameter is object[] controls && controls.Length >= 2 &&
			   controls[0] is PasswordBox passBox && controls[1] is PasswordBox confirmPassBox)
			{
				string password = passBox.Password;
				string confirmPassword = confirmPassBox.Password;

				ValidatePassword(password);
				ValidateConfirmPassword(password, confirmPassword);
			}
		}

		private void ExecuteConfirmPasswordChanged(object parameter)
		{
			if (parameter is object[] controls && controls.Length >= 2 &&
			  controls[0] is PasswordBox passBox && controls[1] is PasswordBox confirmPassBox)
			{
				string password = passBox.Password;
				string confirmPassword = confirmPassBox.Password;
				ValidateConfirmPassword(password, confirmPassword);
			}
		}

		private bool ValidateName()
		{
			if (string.IsNullOrWhiteSpace(Name)) 
			{
				NameError = GetResourceString("ErrorNameEmpty"); 
				IsNameErrorVisible = true; 
				return false;
			}

			if (!_nameRegex.IsMatch(Name)) 
			{
				NameError = GetResourceString("ErrorNameFormat"); 
				IsNameErrorVisible = true; 
				return false;
			}

			IsNameErrorVisible = false;
			return true;
		}

		private bool ValidateSurname()
		{
			if (string.IsNullOrWhiteSpace(Surname))
			{
				SurnameError = GetResourceString("ErrorSurnameEmpty"); 
				IsSurnameErrorVisible = true; 
				return false; 
			}

			if (!_surnameRegex.IsMatch(Surname)) 
			{
				SurnameError = GetResourceString("ErrorSurnameFormat");
				IsSurnameErrorVisible = true; 
				return false;
			}

			IsSurnameErrorVisible = false; 
			return true;
		}

		private bool ValidateEmail()
		{
			if (IsEmailErrorVisible && EmailError == GetResourceString("ErrorEmailExists"))
			{
				IsEmailErrorVisible = false;
				EmailError = string.Empty;
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

			IsEmailErrorVisible = false;
			return true;
		}

		private bool ValidatePassword(string password)
		{
			if (string.IsNullOrWhiteSpace(password))
			{
				PasswordError = GetResourceString("ErrorPasswordEmpty"); 
				IsPasswordErrorVisible = true; 
				return false; 
			}

			if (!_passwordRegex.IsMatch(password)) 
			{ 
				PasswordError = GetResourceString("ErrorPasswordFormat");
				IsPasswordErrorVisible = true; 
				return false; 
			}

			IsPasswordErrorVisible = false; 
			return true;
		}

		private bool ValidateConfirmPassword(string password, string confirmPassword)
		{
			if (string.IsNullOrWhiteSpace(confirmPassword)) 
			{
				ConfirmPasswordError = GetResourceString("ErrorConfirmPasswordEmpty"); 
				IsConfirmPasswordErrorVisible = true; 
				return false; 
			}

			if (password != confirmPassword)
			{ 
				ConfirmPasswordError = GetResourceString("ErrorConfirmPasswordMismatch"); 
				IsConfirmPasswordErrorVisible = true; 
				return false;
			}

			IsConfirmPasswordErrorVisible = false;
			return true;
		}

		private bool ValidateAll(string password, string confirmPassword)
		{
			bool isNameValid = ValidateName();
			bool isSurnameValid = ValidateSurname();
			bool isEmailValid = ValidateEmail();
			bool isPasswordValid = ValidatePassword(password);
			bool isConfirmPasswordValid = ValidateConfirmPassword(password, confirmPassword);

			return isNameValid && isSurnameValid && isEmailValid && isPasswordValid && isConfirmPasswordValid;
		}

		private void ResetValidationMessages()
		{
			NameError = SurnameError = EmailError = PasswordError = ConfirmPasswordError = string.Empty;
			IsNameErrorVisible = IsSurnameErrorVisible = IsEmailErrorVisible = IsPasswordErrorVisible = IsConfirmPasswordErrorVisible = false;
		}

		private string GetResourceString(string key)
		{
			return Application.Current.TryFindResource(key) as string ?? $"[{key}]";
		}

		private string CapitalizeFirstLetter(string input)
		{
			if (string.IsNullOrWhiteSpace(input)) return input;
			input = input.ToLower();
			StringBuilder result = new StringBuilder(input.Length);
			bool capitalizeNext = true;
			foreach (char c in input)
			{
				if (char.IsLetter(c))
				{
					result.Append(capitalizeNext ? char.ToUpper(c) : c);
					capitalizeNext = false;
				}
				else
				{
					result.Append(c);
					capitalizeNext = (c == ' ' || c == '-');
				}
			}
			return result.ToString();
		}

		private void NavigateToLoginWindow()
		{
			try
			{
				AppDbContext dbContextForLogin = App.ServiceProvider.GetRequiredService<AppDbContext>();
				var loginViewModel = new MainWindowViewModel(dbContextForLogin);
				var loginWindow = new MainWindow
				{
					DataContext = loginViewModel
				};
				loginWindow.Show();
				Application.Current.Windows.OfType<RegistrationWindow>().FirstOrDefault()?.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error navigating to login window: {ex.Message}", "Navigation Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		~RegistrationWindowViewModel()
		{
			App.LanguageChanged -= OnLanguageChanged;
		}
	}
}
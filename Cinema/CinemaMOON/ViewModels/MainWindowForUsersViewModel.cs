using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using CinemaMOON.Views;
using CinemaMOON.Models;
using Microsoft.Extensions.DependencyInjection;
using CinemaMOON.Data;

namespace CinemaMOON.ViewModels
{
    public class MainWindowForUsersViewModel : ViewModelBase
	{
		private Frame _mainFrame;
		private User _loggedInUser;
		private readonly AppDbContext _dbContext;

		public User CurrentUser => _loggedInUser;

		public ICommand NavigateCommand { get; }
		public ICommand SwitchLanguageCommand { get; }
		public MainWindowForUsersViewModel(Frame mainFrame, User user)
		{
			_mainFrame = mainFrame ?? throw new ArgumentNullException(nameof(mainFrame));
			_loggedInUser = user ?? throw new ArgumentNullException(nameof(user));
			_dbContext = App.ServiceProvider.GetRequiredService<AppDbContext>();

			NavigateCommand = new RelayCommand(ExecuteNavigate);
			SwitchLanguageCommand = new RelayCommand(ExecuteSwitchLanguage);

			InitializeNavigation();

		}
		public void InitializeNavigation()
		{
			try
			{
				_mainFrame.Navigate(new HomePage(_dbContext));
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при начальной навигации: {ex.Message}",
					"Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void ExecuteNavigate(object parameter)
		{
			if (!(parameter is string pageKey)) return;

			try
			{
				Page pageToNavigate = pageKey switch
				{
					"Home" => new HomePage(_dbContext),
					"Poster" => new PosterPage(_dbContext, _loggedInUser),
					"Halls" => new HallPage(_dbContext),
					"AboutUs" => new AboutUsPage(),
					"Account" => new AccountPage(_dbContext, _loggedInUser),
					_ => new HomePage(_dbContext)
				};

				if (_mainFrame.Content?.GetType() != pageToNavigate.GetType())
				{
					_mainFrame.Navigate(pageToNavigate);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при переходе на страницу: {ex.Message}",
					"Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
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
					MessageBox.Show($"Ошибка смены языка: {ex.Message}");
				}
			}
		}
	}
}

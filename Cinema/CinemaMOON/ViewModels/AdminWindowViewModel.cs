using CinemaMOON.Data;
using CinemaMOON.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CinemaMOON.ViewModels
{
    public class AdminWindowViewModel : ViewModelBase
	{
		private Frame _mainFrame;
		private readonly AppDbContext _dbContext;

		public ICommand NavigateCommand { get; }
		public ICommand SwitchLanguageCommand { get; }

		public AdminWindowViewModel(Frame mainAdminFrame)
		{
			_mainFrame = mainAdminFrame ?? throw new ArgumentNullException(nameof(mainAdminFrame));
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
					"Poster" => new PosterPage(_dbContext, null),
					"Halls" => new HallPage(_dbContext),
					"AboutUs" => new AboutUsPage(),
					"AdminPanel" => new AdminPanelPage(_dbContext),
					_ => new AdminPanelPage(_dbContext)
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

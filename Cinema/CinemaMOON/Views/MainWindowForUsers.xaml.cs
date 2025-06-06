using CinemaMOON.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CinemaMOON.Models;

namespace CinemaMOON.Views
{
	public partial class MainWindowForUsers : Window
	{
		private User _currentUser;
		public MainWindowForUsers(User loggedInUser)
		{
			InitializeComponent();
			this.Icon = BitmapFrame.Create(new Uri("pack://application:,,,/Resources/Images/IconForCinemaApp.png"));

			_currentUser = loggedInUser ?? throw new ArgumentNullException(nameof(loggedInUser), "Пользователь не может быть null при создании окна.");


			var viewModel = new MainWindowForUsersViewModel(this.frameMain, _currentUser);
			this.DataContext = viewModel;

			this.Loaded += MainWindowForUsers_Loaded;
		}

		private void MainWindowForUsers_Loaded(object sender, RoutedEventArgs e)
		{
			if (this.DataContext is MainWindowForUsersViewModel viewModel)
			{
				viewModel.InitializeNavigation();
			}
			else
			{
				MessageBox.Show("Не удалось получить ViewModel для инициализации навигации.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
			}

			this.Loaded -= MainWindowForUsers_Loaded;
		}

		private void buttonMinimize_Click(object sender, RoutedEventArgs e) { WindowState = WindowState.Minimized; }
		private void buttonMaximize_Click(object sender, RoutedEventArgs e)
		{
			if (this.WindowState == WindowState.Maximized)
			{
				this.WindowState = WindowState.Normal;
				maximizeImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/MaximizeWindow.png"));
			}
			else
			{
				this.WindowState = WindowState.Maximized;
				maximizeImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/RestoreWindow.png"));
			}
		}
		private void buttonClose_Click(object sender, RoutedEventArgs e) { Close(); }
		private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed) DragMove();
		}
	}
}

using CinemaMOON.ViewModels;
using CinemaMOON.Views;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CinemaMOON.Views
{
	public partial class AdminWindow : Window
	{
		public AdminWindow()
		{
			InitializeComponent();
			this.Icon = BitmapFrame.Create(new Uri("pack://application:,,,/Resources/Images/IconForCinemaApp.png"));

			var viewModel = new AdminWindowViewModel(this.frameMainForAdmin);
			this.DataContext = viewModel;
		}

		private void buttonMinimize_Click(object sender, RoutedEventArgs e)
		{
			WindowState = WindowState.Minimized;
		}

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

		private void buttonClose_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed &&
				this.Visibility == Visibility.Visible &&
				this.IsLoaded)
			{
				DragMove();
			}
		}
	}
}

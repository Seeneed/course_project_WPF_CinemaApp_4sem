using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls; 
using System.Windows.Input;
using System.Windows.Media; 
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading; 

namespace CinemaMOON.Views
{
	public partial class RegistrationWindow : Window
	{
		private readonly Random _random = new Random();
		private readonly DispatcherTimer _starCreationTimer = new DispatcherTimer();
		private readonly DispatcherTimer _movementTimer = new DispatcherTimer();
		private const int MaxStars = 100;

		public RegistrationWindow()
		{
			InitializeComponent();
			this.Icon = BitmapFrame.Create(new Uri("pack://application:,,,/Resources/Images/IconForCinemaApp.png"));

			Loaded += (s, e) => InitializeStarSystem();
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
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				this.DragMove();
			}
		}

		private void InitializeStarSystem()
		{
			if (!_starCreationTimer.IsEnabled)
			{
				_starCreationTimer.Interval = TimeSpan.FromMilliseconds(300);
				_starCreationTimer.Tick += StarCreationTimer_Tick;
				_starCreationTimer.Start();
			}
			if (!_movementTimer.IsEnabled)
			{
				_movementTimer.Interval = TimeSpan.FromMilliseconds(16);
				_movementTimer.Tick += MovementTimer_Tick;
				_movementTimer.Start();
			}
		}

		private void StarCreationTimer_Tick(object sender, EventArgs e) => CreateNewStar();
		private void MovementTimer_Tick(object sender, EventArgs e) => MoveStars();

		private void CreateNewStar()
		{
			if (StarField.Children.Count >= MaxStars || StarField.ActualWidth <= 0) return;

			var star = new Ellipse
			{
				Width = _random.Next(1, 4),
				Height = _random.Next(1, 4), 
				Fill = Brushes.White,
				Opacity = _random.NextDouble() * 0.5 + 0.3
			};

			Canvas.SetLeft(star, _random.Next(0, (int)StarField.ActualWidth));
			Canvas.SetTop(star, -star.Height);
			star.Tag = _random.NextDouble() * 1.0 + 0.3; 

			StarField.Children.Add(star);
		}

		private void MoveStars()
		{
			if (StarField.ActualHeight <= 0) return;

			var starsToRemove = new List<UIElement>();
			foreach (var child in StarField.Children)
			{
				if (child is Ellipse star && star.Tag is double speed)
				{
					double newTop = Canvas.GetTop(star) + speed;
					if (newTop > StarField.ActualHeight)
					{
						starsToRemove.Add(star); 
					}
					else
					{
						Canvas.SetTop(star, newTop); 
					}
				}
			}

			foreach (var star in starsToRemove)
			{
				StarField.Children.Remove(star);
			}
		}

		protected override void OnClosed(EventArgs e)
		{
			_starCreationTimer.Stop();
			_starCreationTimer.Tick -= StarCreationTimer_Tick;
			_movementTimer.Stop();
			_movementTimer.Tick -= MovementTimer_Tick;
			base.OnClosed(e);
		}
	}
}
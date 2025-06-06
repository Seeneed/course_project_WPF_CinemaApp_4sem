using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using CinemaMOON.ViewModels;
using CinemaMOON.Views;

namespace CinemaMOON.Views
{
	public partial class MainWindow : Window
	{
		private readonly Random _random = new Random();
		private readonly DispatcherTimer _starCreationTimer = new DispatcherTimer();
		private readonly DispatcherTimer _movementTimer = new DispatcherTimer();
		private const int MaxStars = 100;

		public MainWindow()
		{
			InitializeComponent();
			this.Icon = BitmapFrame.Create(new Uri("pack://application:,,,/Resources/Images/IconForCinemaApp.png"));
			Loaded += MainWindow_Loaded;
			Closing += MainWindow_Closing;
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			InitializeStarSystem();
		}

		private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_starCreationTimer.Stop();
			_movementTimer.Stop();
		}

		private void InitializeStarSystem()
		{
			if (StarField.ActualWidth == 0 || StarField.ActualHeight == 0)
			{
				Dispatcher.BeginInvoke(new Action(InitializeStarSystem), DispatcherPriority.ContextIdle);
				return;
			}

			_starCreationTimer.Interval = TimeSpan.FromMilliseconds(300);
			_starCreationTimer.Tick += (s, e) => CreateNewStar();
			_starCreationTimer.Start();

			_movementTimer.Interval = TimeSpan.FromMilliseconds(16);
			_movementTimer.Tick += (s, e) => MoveStars();
			_movementTimer.Start();
		}

		private void CreateNewStar()
		{
			if (StarField.Children.Count >= MaxStars || StarField.ActualWidth <= 0) return;

			var star = new Ellipse
			{
				Width = _random.Next(1, 4),
				Height = _random.Next(1, 4),
				Fill = new SolidColorBrush(Color.FromArgb(
					(byte)_random.Next(150, 255),
					(byte)_random.Next(200, 255),
					(byte)_random.Next(200, 255),
					(byte)_random.Next(200, 255))),
			};

			star.Tag = _random.NextDouble() * 1.5 + 0.5;

			Canvas.SetLeft(star, _random.Next(0, (int)StarField.ActualWidth));
			Canvas.SetTop(star, -star.Height);
			StarField.Children.Add(star);
		}

		private void MoveStars()
		{
			if (!StarField.IsVisible) return;

			var starsToRemove = new List<UIElement>();

			foreach (UIElement child in StarField.Children)
			{
				if (child is Ellipse star && star.Tag is double speed)
				{
					double currentTop = Canvas.GetTop(star);
					double newY = currentTop + speed;
					Canvas.SetTop(star, newY);

					if (newY > StarField.ActualHeight + star.Height)
					{
						starsToRemove.Add(star);
					}
				}
			}

			foreach (var star in starsToRemove)
			{
				StarField.Children.Remove(star);
			}
		}
		private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				this.DragMove();
			}
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
			this.Close();
		}
	}
}
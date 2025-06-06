using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using CinemaMOON.Data;
using CinemaMOON.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CinemaMOON.Views
{
    public partial class HomePage : Page
	{
		private readonly AppDbContext _dbContext;

		public HomePage(AppDbContext dbContext)
		{
			_dbContext = dbContext;
			InitializeComponent();
			this.Loaded += HomePage_Loaded;
			this.Unloaded += HomePage_Unloaded;

			DataContext = new HomePageViewModel(_dbContext);
		}

		private HomePageViewModel ViewModel => DataContext as HomePageViewModel;

		private void HomePage_Loaded(object sender, RoutedEventArgs e)
		{
			if (ViewModel != null)
			{
				ViewModel.RequestAnimate += ViewModel_RequestAnimate;
				ViewModel.LoadData();
			}
		}

		private void HomePage_Unloaded(object sender, RoutedEventArgs e)
		{
			if (ViewModel != null)
			{
				ViewModel.RequestAnimate -= ViewModel_RequestAnimate;
			}
		}

		private void ViewModel_RequestAnimate(object sender, CarouselAnimationEventArgs e)
		{
			AnimateCarousel(e.TargetOffset, e.OnAnimationComplete);
		}

		private void AnimateCarousel(double targetOffset, Action onCompleteAction)
		{
			if (filmCarousel == null) return; 

			if (!(filmCarousel.RenderTransform is TranslateTransform transform))
			{
				transform = new TranslateTransform();
				filmCarousel.RenderTransform = transform;
			}

			transform.BeginAnimation(TranslateTransform.XProperty, null);
			transform.X = 0;

			var animation = new DoubleAnimation
			{
				To = targetOffset,
				Duration = TimeSpan.FromMilliseconds(350), 
														   
				EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
			};

			animation.Completed += (s, e) =>
			{
				transform.BeginAnimation(TranslateTransform.XProperty, null);
				transform.X = 0;

				onCompleteAction?.Invoke();
			};

			transform.BeginAnimation(TranslateTransform.XProperty, animation);
		}
	}
}
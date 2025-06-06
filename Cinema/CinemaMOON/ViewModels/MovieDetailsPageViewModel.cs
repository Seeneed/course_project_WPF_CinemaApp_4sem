using CinemaMOON.Models;
using CinemaMOON.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Media;
using CinemaMOON.Data;

namespace CinemaMOON.ViewModels
{
    public class MovieDetailsPageViewModel : ViewModelBase
	{
		private readonly AppDbContext _dbContext; 
		private readonly User? _currentUser; 

		private Movie _selectedMovie;
		private ObservableCollection<Schedule> _showtimes;

		public Movie SelectedMovie
		{
			get => _selectedMovie;
			private set => SetProperty(ref _selectedMovie, value);
		}

		public ObservableCollection<Schedule> Showtimes
		{
			get => _showtimes;
			private set => SetProperty(ref _showtimes, value);
		}

		public ICommand GoBackCommand { get; }
		public IAsyncRelayCommand SelectShowtimeCommand { get; }

		public MovieDetailsPageViewModel(AppDbContext dbContext, Movie movie, User? currentUser)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_selectedMovie = movie ?? throw new ArgumentNullException(nameof(movie), "Movie data cannot be null.");
			_currentUser = currentUser;


			Showtimes = new ObservableCollection<Schedule>();

			GoBackCommand = new RelayCommand<Page>(ExecuteGoBack, CanExecuteGoBack); 
			SelectShowtimeCommand = new AsyncRelayCommand<object>(ExecuteSelectShowtimeAsync, CanExecuteSelectShowtime);

			_ = LoadShowtimesAsync();
		}

		private async Task LoadShowtimesAsync()
		{
			Showtimes.Clear();
			try
			{
				var movieSchedules = await _dbContext.Schedules
					.Where(s => s.MovieId == _selectedMovie.Id 
						   && !s.IsDeleted                     
						   && s.ShowTime >= DateTime.Now)     
					.OrderBy(s => s.ShowTime)                 
					.ToListAsync();                           

				foreach (var schedule in movieSchedules)
				{
					Showtimes.Add(schedule);
				}
			}
			catch (Exception ex)
			{
				ShowMessageFormat("MovieDetails_Error_LoadingSchedule", "Error_Loading_Title", MessageBoxImage.Warning, _selectedMovie.Title, ex.Message);
				Showtimes.Clear();
			}
			SelectShowtimeCommand.NotifyCanExecuteChanged();
		}

		private bool CanExecuteGoBack(Page page) => true;

		private void ExecuteGoBack(Page currentPage)
		{
			if (currentPage?.NavigationService?.CanGoBack == true)
			{
				currentPage.NavigationService.GoBack();
			}
			else
			{
				ShowMessage("Navigation_Error_CannotGoBack", "ErrorTitle", MessageBoxImage.Warning);
			}
		}

		private bool CanExecuteSelectShowtime(object parameter)
		{
			return parameter is FrameworkElement element &&
				   element.DataContext is Schedule schedule &&
				   _currentUser != null;
		}

		private async Task ExecuteSelectShowtimeAsync(object parameter)
		{
		    if (!(parameter is FrameworkElement button && button.DataContext is Schedule selectedSchedule))
		    {
		        return;
		    }
		
		    if (_currentUser == null) 
		    {
		        return;
		    }
				
		    try
		    {
		        if (selectedSchedule.Hall == null)
		        {
		            selectedSchedule.Hall = await _dbContext.Halls.FindAsync(selectedSchedule.HallId);
		            if (selectedSchedule.Hall == null)
		            {
		                ShowMessage("Error_HallNotFoundForSchedule", "ErrorTitle", MessageBoxImage.Error);
		                return; 
		            }
		        }
		
		        if (selectedSchedule.Movie == null)
		        {
		             selectedSchedule.Movie = await _dbContext.Movies.FindAsync(selectedSchedule.MovieId);
		             if (selectedSchedule.Movie == null)
		             {
		                  ShowMessage("Error_MovieNotFoundForSchedule", "ErrorTitle", MessageBoxImage.Error);
		                  return; 
		             }
		        }
		
		        var currentPage = FindParent<Page>(button);
		        NavigationService navigationService = null;
		        if (currentPage != null)
		        {
		            navigationService = NavigationService.GetNavigationService(currentPage);
		        }
		
		        if (navigationService != null)
		        {
		            var bookingPage = new SeatSelectionPage(_dbContext, selectedSchedule, _currentUser);
		            navigationService.Navigate(bookingPage);
		        }
		        else
		        {
		            ShowMessage("Navigation_Error_ServiceNotFound", "ErrorTitle", MessageBoxImage.Error);
		        }
		    }
		    catch (Exception ex)
		    {
		        ShowMessageFormat("Navigation_Error", "ErrorTitle", MessageBoxImage.Error, ex.Message);
		    }
		}

		public static T FindParent<T>(DependencyObject child) where T : DependencyObject
		{
			DependencyObject parentObject = VisualTreeHelper.GetParent(child);
			if (parentObject == null) return null;
			T parent = parentObject as T;
			if (parent != null)
				return parent;
			else
				return FindParent<T>(parentObject);
		}

		private bool IsUserAdmin()
		{
			return Application.Current.Windows.OfType<AdminWindow>().Any(w => w.IsVisible);
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
			string message = string.Format(messageFormat ?? $"[{messageKey}](Format Error)", args);
			MessageBox.Show(message, GetResourceString(titleKey ?? "InformationTitle"), MessageBoxButton.OK, icon);
		}
	}
}
using CinemaMOON.Models;
using CinemaMOON.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using CommunityToolkit.Mvvm.Input;
using CinemaMOON.Data;

namespace CinemaMOON.ViewModels
{
    public class PosterPageViewModel : ViewModelBase
	{
		private readonly AppDbContext _dbContext;
		private List<Movie> _allMovies; 
		private readonly User? _currentUser;

		private ObservableCollection<Movie> _displayedMovies;
		private string _selectedGenre;
		private string _searchText = "";
		private List<string> _availableGenres;
		private NavigationService _navigationService;

		public ObservableCollection<Movie> DisplayedMovies
		{
			get => _displayedMovies;
			set => SetProperty(ref _displayedMovies, value);
		}

		public List<string> AvailableGenres => _availableGenres;

		public string SelectedGenre
		{
			get => _selectedGenre;
			set
			{
				if (SetProperty(ref _selectedGenre, value))
				{
					ApplyFilterAndSearch();
				}
			}
		}

		public string SearchText
		{
			get => _searchText;
			set
			{
				if (SetProperty(ref _searchText, value))
				{
					ApplyFilterAndSearch();
				}
			}
		}

		private bool _showNoMoviesFoundMessage;
		public bool ShowNoMoviesFoundMessage
		{
			get => _showNoMoviesFoundMessage;
			private set => SetProperty(ref _showNoMoviesFoundMessage, value);
		}

		public IAsyncRelayCommand LoadDataCommand { get; }
		public ICommand ViewDetailsCommand { get; }

		public PosterPageViewModel(AppDbContext dbContext, User? currentUser)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext)); 
			_currentUser = currentUser;
			_allMovies = new List<Movie>();
			DisplayedMovies = new ObservableCollection<Movie>();
			_availableGenres = new List<string>(); 

			LoadAvailableGenreList();
			_selectedGenre = _availableGenres.FirstOrDefault();

			LoadDataCommand = new AsyncRelayCommand(LoadMoviesAsync);
			ViewDetailsCommand = new RelayCommand<FrameworkElement>(ExecuteViewDetails, element => element?.DataContext is Movie);
		}

		private async Task LoadMoviesAsync()
		{
			_allMovies.Clear();
			DisplayedMovies.Clear();
			ShowNoMoviesFoundMessage = false; 

			try
			{
				_allMovies = await _dbContext.Movies
							   .Where(m => !m.IsDeleted && m.IsActive)
							   .Include(m => m.Schedules.Where(s => !s.IsDeleted && s.ShowTime >= DateTime.Now.Date))
									   .OrderBy(m => m.Title)
									   .ToListAsync();
				ApplyFilterAndSearch();
			}
			catch (Exception ex)
			{
				ShowMessageFormat("PosterPage_Error_Loading", "ErrorTitle", MessageBoxImage.Error, ex.Message);
				_allMovies.Clear();
				DisplayedMovies.Clear();
				ApplyFilterAndSearch();
			}
			finally
			{
				CommandManager.InvalidateRequerySuggested();
			}
		}

		private void LoadAvailableGenreList()
		{
			_availableGenres = new List<string>
			{
				GetResourceString("PosterPage_AllGenres"),
				GetResourceString("Genre_Action"),
				GetResourceString("Genre_Comedy"),
				GetResourceString("Genre_Drama"),
				GetResourceString("Genre_Romance"),
				GetResourceString("Genre_SciFi"),
				GetResourceString("Genre_Fantasy"),
				GetResourceString("Genre_Thriller"),
				GetResourceString("Genre_Horror"),
				GetResourceString("Genre_War"),
				GetResourceString("Genre_Kids"),
				GetResourceString("Genre_Crime")
			};
			_availableGenres.RemoveAll(string.IsNullOrEmpty); 
			OnPropertyChanged(nameof(AvailableGenres));
		}

		private void ExecuteViewDetails(FrameworkElement commandSource)
		{
			if (commandSource?.DataContext is Movie selectedMovie)
			{
				try
				{
					var navigationService = GetNavigationService(commandSource);

					if (navigationService != null)
					{
						var detailsPage = new MovieDetailsPage(_dbContext, selectedMovie, _currentUser);
						navigationService.Navigate(detailsPage);
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
		}

		private NavigationService GetNavigationService(object commandParameter)
		{
			if (_navigationService != null) return _navigationService;

			if (commandParameter is FrameworkElement element)
			{
				var page = FindParent<Page>(element); 
				if (page?.NavigationService != null)
				{
					_navigationService = page.NavigationService; 
					return _navigationService;
				}
			}

			string frameName = IsUserAdmin() ? "frameMainForAdmin" : "MainFrame"; 
			if (Application.Current.MainWindow?.FindName(frameName) is Frame namedFrame && namedFrame.NavigationService != null)
			{
				_navigationService = namedFrame.NavigationService;
				return _navigationService;
			}
			if (Application.Current.MainWindow?.Content is Frame rootFrame && rootFrame.NavigationService != null)
			{
				_navigationService = rootFrame.NavigationService;
				return _navigationService;
			}


			return null; 
		}

		private bool IsUserAdmin()
		{
			return Application.Current.MainWindow?.GetType().Name == "AdminWindow";
		}


		public static T FindParent<T>(DependencyObject child) where T : DependencyObject
		{
			if (child == null) return null;

			DependencyObject parentObject = VisualTreeHelper.GetParent(child);

			if (parentObject == null) return null;

			T parent = parentObject as T;
			if (parent != null)
			{
				return parent;
			}
			else
			{
				return FindParent<T>(parentObject);
			}
		}

		private void ApplyFilterAndSearch()
		{
			if (_allMovies == null)
			{
				DisplayedMovies = new ObservableCollection<Movie>();
				ShowNoMoviesFoundMessage = true;
				return;
			}

			IEnumerable<Movie> filteredMovies = _allMovies;

			string allGenresText = GetResourceString("PosterPage_AllGenres");
			if (!string.IsNullOrEmpty(SelectedGenre) && SelectedGenre != allGenresText)
			{
				filteredMovies = filteredMovies.Where(m => m.Genre != null && m.Genre.Equals(SelectedGenre, StringComparison.OrdinalIgnoreCase));
			}

			if (!string.IsNullOrWhiteSpace(SearchText))
			{
				string lowerSearch = SearchText.ToLowerInvariant();
				filteredMovies = filteredMovies.Where(m =>
					(m.Title != null && m.Title.ToLowerInvariant().Contains(lowerSearch)) ||
					(m.Director != null && m.Director.ToLowerInvariant().Contains(lowerSearch)));
			}

			var resultList = filteredMovies.ToList();
			DisplayedMovies = new ObservableCollection<Movie>(resultList);

			ShowNoMoviesFoundMessage = !resultList.Any();
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

		public void UpdateLocalization()
		{
			LoadAvailableGenreList();
			SelectedGenre = AvailableGenres.FirstOrDefault();
			ApplyFilterAndSearch();
			CommandManager.InvalidateRequerySuggested();
		}
	}
}
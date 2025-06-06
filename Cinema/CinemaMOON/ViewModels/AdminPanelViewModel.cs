using CinemaMOON.Data;
using CinemaMOON.Models;
using CinemaMOON.Views;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace CinemaMOON.ViewModels
{
	public interface IUndoableCommand
	{
		string Description { get; } 
		Task ExecuteAsync();      
		Task UndoAsync();         
	}

	public class LogicalDeleteMovieCommand : IUndoableCommand
	{
		private readonly AppDbContext _dbContext;
		private readonly Guid _movieId;
		private readonly AdminPanelViewModel _viewModel;
		private List<Guid> _logicallyDeletedScheduleIds = new List<Guid>();
		private string _movieTitleCache;

		public string Description => $"Логическое удаление/восстановление фильма (ID: {_movieId}, Название: '{_movieTitleCache ?? _movieId.ToString()}')";

		public LogicalDeleteMovieCommand(AppDbContext dbContext, Guid movieId, AdminPanelViewModel viewModel)
		{
			_dbContext = dbContext;
			_movieId = movieId;
			_viewModel = viewModel;
		}

		public async Task ExecuteAsync()
		{
			var movieInDb = await _dbContext.Movies
				.Include(m => m.Schedules)
				.FirstOrDefaultAsync(m => m.Id == _movieId);

			if (movieInDb == null)
			{
				_viewModel.ShowMessage("AdminPanel_Error_MovieNotFoundInDb", "AdminPanel_Title_DeletionError", MessageBoxImage.Error);
				return;
			}
			_movieTitleCache = movieInDb.Title;

			if (movieInDb.IsDeleted) return; 

			movieInDb.IsDeleted = true;
			_logicallyDeletedScheduleIds.Clear();

			string canceledStatusKey = _viewModel.GetResourceString("OrderStatus_Canceled");
			string bookedStatusKey = _viewModel.GetResourceString("OrderStatus_Booked");

			foreach (var schedule in movieInDb.Schedules.Where(s => !s.IsDeleted))
			{
				schedule.IsDeleted = true;
				_logicallyDeletedScheduleIds.Add(schedule.Id);

				var ordersToCancel = await _dbContext.Orders
										   .Where(o => o.ScheduleId == schedule.Id && o.OrderStatus == bookedStatusKey)
										   .ToListAsync();
				foreach (var order in ordersToCancel)
				{
					order.OrderStatus = canceledStatusKey;
					order.UserRating = null;
				}
			}
			await _dbContext.SaveChangesAsync();
			_viewModel.ShowMessageFormat("AdminPanel_Success_MovieLogicallyDeleted", "AdminPanel_Title_Success", MessageBoxImage.Information, _movieTitleCache);
		}

		public async Task UndoAsync() 
		{
			var movieInDb = await _dbContext.Movies
				.Include(m => m.Schedules)
				.FirstOrDefaultAsync(m => m.Id == _movieId);

			if (movieInDb == null)
			{
				_viewModel.ShowMessage("AdminPanel_Error_MovieNotFoundInDb", "AdminPanel_Title_Error", MessageBoxImage.Error);
				return;
			}
			_movieTitleCache = movieInDb.Title;

			if (!movieInDb.IsDeleted && !_logicallyDeletedScheduleIds.Any(id => movieInDb.Schedules.Any(s => s.Id == id && s.IsDeleted)))
			{

			}

			bool changed = false;
			if (movieInDb.IsDeleted)
			{
				movieInDb.IsDeleted = false;
				changed = true;
			}

			foreach (var scheduleId in _logicallyDeletedScheduleIds)
			{
				var scheduleToRestore = movieInDb.Schedules.FirstOrDefault(s => s.Id == scheduleId);
				if (scheduleToRestore != null && scheduleToRestore.IsDeleted) 
				{
					scheduleToRestore.IsDeleted = false;
					changed = true;
				}
			}

			if (changed)
			{
				await _dbContext.SaveChangesAsync();
				_viewModel.ShowMessageFormat("AdminPanel_Info_MovieRestored", "AdminPanel_Title_Success", MessageBoxImage.Information, _movieTitleCache);
			}
		}
	}

	public class AdminPanelViewModel : ViewModelBase
	{
		private readonly AppDbContext _dbContext;
		private ObservableCollection<Movie> _displayedMovies;
		private Movie _selectedMovie;
		private string _searchText = string.Empty;
		private string _selectedGenre;
		private string _selectedSortOption;

		private List<Movie> _allMovies = new List<Movie>();
		private List<string> _availableGenres;
		private List<string> _availableSortOptions;

		private readonly Stack<IUndoableCommand> _undoStack = new Stack<IUndoableCommand>();
		private readonly Stack<IUndoableCommand> _redoStack = new Stack<IUndoableCommand>();

		public List<string> AvailableGenres => _availableGenres;
		public List<string> AvailableSortOptions => _availableSortOptions;

		public ObservableCollection<Movie> DisplayedMovies
		{
			get => _displayedMovies;
			set => SetProperty(ref _displayedMovies, value);
		}

		public Movie SelectedMovie
		{
			get => _selectedMovie;
			set
			{
				if (SetProperty(ref _selectedMovie, value))
				{
					EditMovieCommand.NotifyCanExecuteChanged();
					DeleteMovieCommand.NotifyCanExecuteChanged();
					AddScheduleCommand.NotifyCanExecuteChanged();
					DeleteScheduleCommand.NotifyCanExecuteChanged();
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
					ApplyFiltersAndSorting();
				}
			}
		}

		public string SelectedGenre
		{
			get => _selectedGenre;
			set
			{
				if (SetProperty(ref _selectedGenre, value))
				{
					ApplyFiltersAndSorting();
				}
			}
		}

		public string SelectedSortOption
		{
			get => _selectedSortOption;
			set
			{
				if (SetProperty(ref _selectedSortOption, value))
				{
					ApplyFiltersAndSorting();
				}
			}
		}

		public IAsyncRelayCommand AddMovieCommand { get; }
		public IAsyncRelayCommand EditMovieCommand { get; }
		public IAsyncRelayCommand DeleteMovieCommand { get; }
		public IAsyncRelayCommand AddScheduleCommand { get; }
		public IAsyncRelayCommand DeleteScheduleCommand { get; }
		public IAsyncRelayCommand ViewOrdersCommand { get; }
		public IAsyncRelayCommand ShowAllMoviesCommand { get; }
		public IAsyncRelayCommand RefreshMoviesCommand { get; }
		public ICommand ClearMoviesCommand { get; }
		public ICommand LogoutCommand { get; }
		public IAsyncRelayCommand ViewAllSchedulesCommand { get; }
		public IAsyncRelayCommand UndoCommand { get; }
		public IAsyncRelayCommand RedoCommand { get; }


		public AdminPanelViewModel(AppDbContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

			_availableGenres = new List<string>();
			_availableSortOptions = new List<string>();
			LoadLocalizationStrings();
			DisplayedMovies = new ObservableCollection<Movie>();

			AddMovieCommand = new AsyncRelayCommand(ExecuteAddMovieAsync);
			EditMovieCommand = new AsyncRelayCommand(ExecuteEditMovieAsync, CanEditDeleteMovie);
			DeleteMovieCommand = new AsyncRelayCommand(ExecuteDeleteMovieAsync, CanEditDeleteMovie);
			AddScheduleCommand = new AsyncRelayCommand(ExecuteAddScheduleAsync, CanEditDeleteSchedule);
			DeleteScheduleCommand = new AsyncRelayCommand(ExecuteDeleteScheduleAsync, CanEditDeleteSchedule);
			ViewOrdersCommand = new AsyncRelayCommand(ExecuteViewOrdersAsync);
			ShowAllMoviesCommand = new AsyncRelayCommand(ExecuteShowAllMoviesAsync);
			ClearMoviesCommand = new RelayCommand(ExecuteClearMovies);
			RefreshMoviesCommand = new AsyncRelayCommand(ExecuteRefreshMoviesAsync);
			LogoutCommand = new RelayCommand<Page>(ExecuteLogout, CanExecuteLogout);
			ViewAllSchedulesCommand = new AsyncRelayCommand(ExecuteViewAllSchedulesAsync);
			UndoCommand = new AsyncRelayCommand(ExecuteUndoAsync, CanExecuteUndo);
            RedoCommand = new AsyncRelayCommand(ExecuteRedoAsync, CanExecuteRedo);

			_selectedGenre = _availableGenres.FirstOrDefault() ?? GetResourceString("AdminPanel_GenreFilter_All");
			_selectedSortOption = _availableSortOptions.FirstOrDefault() ?? GetResourceString("AdminPanel_Sort_None");

			_ = ShowAllMoviesCommand.ExecuteAsync(null);
		}

		private async Task ExecuteViewAllSchedulesAsync()
		{
			var navigationService = FindNavigationService();
			if (navigationService != null)
			{
				try
				{
					navigationService.Navigate(new AllSchedulesPage(_dbContext));
				}
				catch (Exception ex)
				{
					ShowMessageFormat("AdminPanel_Error_PageCreationFailed", "AdminPanel_Title_NavigationError",
						MessageBoxImage.Error, "AllSchedulesPage", ex.Message);
				}
			}
			else
			{
				ShowMessage("AdminPanel_Error_NavigationFailed", "AdminPanel_Title_NavigationError", MessageBoxImage.Error);
			}
			await Task.CompletedTask;
		}

		private async Task ExecuteRefreshMoviesAsync()
		{
			try
			{
				_allMovies = await _dbContext.Movies.Where(m => !m.IsDeleted).ToListAsync();
				ApplyFiltersAndSorting(); 
			}
			catch (Exception ex)
			{
				_allMovies.Clear();
				DisplayedMovies.Clear();
				ShowMessageFormat("AdminPanel_Error_LoadingGeneric", "AdminPanel_Title_Error", MessageBoxImage.Error, ex.Message);
			}
			finally
			{
				EditMovieCommand.NotifyCanExecuteChanged();
				DeleteMovieCommand.NotifyCanExecuteChanged();
				AddScheduleCommand.NotifyCanExecuteChanged();
				DeleteScheduleCommand.NotifyCanExecuteChanged();
				CommandManager.InvalidateRequerySuggested();
			}
		}

		private bool CanExecuteLogout(Page currentPage)
		{
			return true;
		}

		private void ExecuteLogout(Page currentPageParameter)
		{
			Window windowToClose = null;
			if (currentPageParameter != null)
			{
				windowToClose = Window.GetWindow(currentPageParameter);
			}
			else
			{
				windowToClose = Application.Current.Windows.OfType<AdminWindow>().FirstOrDefault(w => w.IsActive);
				if (windowToClose == null)
				{
					windowToClose = Application.Current.Windows.OfType<AdminWindow>().FirstOrDefault();
				}
			}

			if (windowToClose == null)
			{
				windowToClose = Application.Current.Windows.OfType<Window>()
											.FirstOrDefault(w => w.IsActive && !(w is MainWindow));
				if (windowToClose == null)
				{
					windowToClose = Application.Current.Windows.OfType<Window>()
											 .FirstOrDefault(w => !(w is MainWindow));
				}
			}


			try
			{
				var dbContextForLogin = App.ServiceProvider.GetRequiredService<AppDbContext>();

				var loginViewModel = new MainWindowViewModel(dbContextForLogin);

				var loginWindow = new MainWindow
				{
					DataContext = loginViewModel
				};
				loginWindow.Show();

				windowToClose?.Close();
			}
			catch (Exception ex)
			{
				ShowMessageFormat("AccountPage_Error_LogoutFailed_Generic", "AccountPage_Error_LogoutTitle", MessageBoxImage.Error, ex.Message);
			}
		}

		private bool CanEditDeleteMovie() => SelectedMovie != null;
		private bool CanEditDeleteSchedule() => SelectedMovie != null;

		private async Task ExecuteAddMovieAsync()
		{
			var navigationService = FindNavigationService();
			if (navigationService != null)
			{
				try
				{
					navigationService.Navigate(new AddingFilmPage(_dbContext));
				}
				catch (Exception ex)
				{
					ShowMessageFormat("AdminPanel_Error_PageCreationFailed", "AdminPanel_Title_NavigationError",
						MessageBoxImage.Error, "AddingFilmPage", ex.Message);
				}
			}
			else
			{
				ShowMessage("AdminPanel_Error_NavigationFailed", "AdminPanel_Title_NavigationError", MessageBoxImage.Error);
			}
			await Task.CompletedTask;
		}

		private async Task ExecuteEditMovieAsync()
		{
			var navigationService = FindNavigationService();
			if (navigationService != null)
			{
				try
				{
					navigationService.Navigate(new EditingFilmPage(_dbContext, SelectedMovie));
				}
				catch (Exception ex)
				{
					ShowMessageFormat("AdminPanel_Error_NavigationEditFailed", "AdminPanel_Title_NavigationError",
						MessageBoxImage.Error, ex.Message);
				}
			}
			else
			{
				ShowMessage("AdminPanel_Error_NavigationFailed", "AdminPanel_Title_NavigationError", MessageBoxImage.Error);
			}
			await Task.CompletedTask;

		}

		private async Task ExecuteDeleteMovieAsync()
		{
			if (SelectedMovie == null)
			{
				ShowMessage("AdminPanel_Info_SelectMovieToDelete", "AdminPanel_Title_MovieNotSelected", MessageBoxImage.Warning);
				return;
			}

			var movieToDeleteId = SelectedMovie.Id;
			var movieTitle = SelectedMovie.Title;

			string confirmationMessage = string.Format(
				GetResourceString("AdminPanel_ConfirmLogicalDeleteMovie") ?? "Пометить фильм '{0}' и все его сеансы как удаленные? Активные заказы на эти сеансы будут отменены.",
				movieTitle
			);

			var result = MessageBox.Show(
				confirmationMessage,
				GetResourceString("AdminPanel_Title_ConfirmDelete"),
				MessageBoxButton.YesNo,
				MessageBoxImage.Warning);

			if (result == MessageBoxResult.No)
			{
				return;
			}

			var command = new LogicalDeleteMovieCommand(_dbContext, movieToDeleteId, this);
			await ExecuteAndRegisterCommandAsync(command);
		}

		private async Task ExecuteAndRegisterCommandAsync(IUndoableCommand command)
		{
			try
			{
				await command.ExecuteAsync();
				_undoStack.Push(command);
				_redoStack.Clear();
				UpdateUndoRedoCanExecute();
				await ExecuteRefreshMoviesAsync();
			}
			catch (Exception ex)
			{
				ShowMessageFormat("AdminPanel_Error_CommandExecutionFailed", "AdminPanel_Title_Error", MessageBoxImage.Error, command.Description, ex.Message);
				await ExecuteRefreshMoviesAsync(); 
			}
		}

		private async Task ExecuteUndoAsync()
		{
			if (_undoStack.Any())
			{
				var command = _undoStack.Pop();
				try
				{
					await command.UndoAsync();
					_redoStack.Push(command);
				}
				catch (Exception ex)
				{
					ShowMessageFormat("AdminPanel_Error_UndoFailed", "AdminPanel_Title_Error", MessageBoxImage.Error, ex.Message);
					_undoStack.Push(command); 
				}
				finally
				{
					UpdateUndoRedoCanExecute();
					await ExecuteRefreshMoviesAsync();
				}
			}
		}

		private bool CanExecuteUndo() => _undoStack.Any();

		private async Task ExecuteRedoAsync()
		{
			if (_redoStack.Any())
			{
				var command = _redoStack.Pop();
				try
				{
					await command.ExecuteAsync();
					_undoStack.Push(command);
				}
				catch (Exception ex)
				{
					ShowMessageFormat("AdminPanel_Error_RedoFailed", "AdminPanel_Title_Error", MessageBoxImage.Error, ex.Message);
					_redoStack.Push(command); 
				}
				finally
				{
					UpdateUndoRedoCanExecute();
					await ExecuteRefreshMoviesAsync();
				}
			}
		}

		private bool CanExecuteRedo() => _redoStack.Any();

		private void UpdateUndoRedoCanExecute()
		{
			UndoCommand.NotifyCanExecuteChanged();
			RedoCommand.NotifyCanExecuteChanged();
		}

		private async Task ExecuteAddScheduleAsync()
		{
			var navigationService = FindNavigationService();

			if (navigationService != null)
			{
				try
				{
					navigationService.Navigate(new AddSchedulePage(_dbContext, SelectedMovie));
				}
				catch (Exception ex)
				{
					ShowMessageFormat("AdminPanel_Error_NavigationGeneric", "AdminPanel_Title_Error",
						MessageBoxImage.Error, ex.Message);
				}
			}
			else
			{
				ShowMessage("AdminPanel_Error_NavigationFailed", "AdminPanel_Title_NavigationError", MessageBoxImage.Error);
			}
			await Task.CompletedTask;
		}

		private async Task ExecuteDeleteScheduleAsync()
		{
			var navigationService = FindNavigationService();

			if (navigationService != null)
			{
				try
				{
					navigationService.Navigate(new DeleteSchedulePage(_dbContext, SelectedMovie));
				}
				catch (Exception ex)
				{
					ShowMessageFormat("AdminPanel_Error_NavigationGeneric", "AdminPanel_Title_Error",
						MessageBoxImage.Error, ex.Message);
				}
			}
			else
			{
				ShowMessage("AdminPanel_Error_NavigationFailed", "AdminPanel_Title_NavigationError", MessageBoxImage.Error);
			}
			await Task.CompletedTask;

		}

		private async Task ExecuteViewOrdersAsync()
		{
			var navigationService = FindNavigationService();
			if (navigationService != null)
			{
				try
				{
					navigationService.Navigate(new AdminOrdersPage(_dbContext));
				}
				catch (Exception ex)
				{
					ShowMessageFormat("AdminPanel_Error_PageCreationFailed", "AdminPanel_Title_NavigationError",
						MessageBoxImage.Error, "AdminOrdersPage", ex.Message);
				}
			}
			else
			{
				ShowMessage("AdminPanel_Error_NavigationFailed", "AdminPanel_Title_NavigationError", MessageBoxImage.Error);
			}
			await Task.CompletedTask;
		}

		private async Task ExecuteShowAllMoviesAsync()
		{
			try
			{
				SearchText = string.Empty;
				var genreAll = GetResourceString("AdminPanel_GenreFilter_All");
				var sortNone = GetResourceString("AdminPanel_Sort_None");

				bool genreChanged = _selectedGenre != genreAll;
				bool sortChanged = _selectedSortOption != sortNone;

				if (genreChanged)
					SelectedGenre = _availableGenres?.Contains(genreAll) == true ? genreAll : _availableGenres?.FirstOrDefault();
				if (sortChanged)
					SelectedSortOption = _availableSortOptions?.Contains(sortNone) == true ? sortNone : _availableSortOptions?.FirstOrDefault();

				_allMovies = await _dbContext.Movies.Where(m => !m.IsDeleted).ToListAsync();

				if (!genreChanged && !sortChanged)
				{
					ApplyFiltersAndSorting();
				}
			}
			catch (Exception ex)
			{
				_allMovies.Clear();
				DisplayedMovies = new ObservableCollection<Movie>();
				ShowMessageFormat("AdminPanel_Error_LoadingGeneric", "AdminPanel_Title_Error", MessageBoxImage.Error, ex.Message);
			}
			finally
			{
				CommandManager.InvalidateRequerySuggested();
			}
		}

		private void ExecuteClearMovies(object parameter)
		{
			DisplayedMovies.Clear();
			SearchText = "";
			SelectedMovie = null;
		}

		private int ParseRating(string ratingString)
		{
			if (string.IsNullOrWhiteSpace(ratingString))
			{
				return -1; 
			}
			string numericPart = new string(ratingString.Where(char.IsDigit).ToArray());
			if (int.TryParse(numericPart, out int ratingValue))
			{
				return ratingValue;
			}
			return -1; 
		}

		private void ApplyFiltersAndSorting()
		{
			if (_allMovies == null) return;

			IEnumerable<Movie> processedMovies = _allMovies;

			if (!string.IsNullOrWhiteSpace(SearchText))
			{
				string lowerSearch = SearchText.ToLowerInvariant();
				processedMovies = processedMovies.Where(movie =>
					(movie.Title != null && movie.Title.ToLowerInvariant().Contains(lowerSearch)) ||
					(movie.Director != null && movie.Director.ToLowerInvariant().Contains(lowerSearch))
				);
			}

			string genreAll = GetResourceString("AdminPanel_GenreFilter_All");
			if (!string.IsNullOrEmpty(SelectedGenre) && SelectedGenre != genreAll)
			{
				processedMovies = processedMovies.Where(m =>
					m.Genre != null &&
					m.Genre.Equals(SelectedGenre, StringComparison.OrdinalIgnoreCase));
			}

			string sortNone = GetResourceString("AdminPanel_Sort_None");
			string sortRatingDesc = GetResourceString("AdminPanel_Sort_RatingDesc");
			string sortRatingAsc = GetResourceString("AdminPanel_Sort_RatingAsc");
			string sortDurationDesc = GetResourceString("AdminPanel_Sort_DurationDesc");
			string sortDurationAsc = GetResourceString("AdminPanel_Sort_DurationAsc");
			string sortTitleAZ = GetResourceString("AdminPanel_Sort_TitleAZ");
			string sortTitleZA = GetResourceString("AdminPanel_Sort_TitleZA");

			if (SelectedSortOption == sortRatingDesc)
				processedMovies = processedMovies.OrderByDescending(m => ParseRating(m.Rating));
			else if (SelectedSortOption == sortRatingAsc)
				processedMovies = processedMovies.OrderBy(m => ParseRating(m.Rating));
			else if (SelectedSortOption == sortDurationDesc) 
				processedMovies = processedMovies.OrderByDescending(m => m.Duration);
			else if (SelectedSortOption == sortDurationAsc)
				processedMovies = processedMovies.OrderBy(m => m.Duration);
			else if (SelectedSortOption == sortTitleAZ) 
				processedMovies = processedMovies.OrderBy(m => m.Title ?? string.Empty, StringComparer.CurrentCultureIgnoreCase);
			else if (SelectedSortOption == sortTitleZA)
				processedMovies = processedMovies.OrderByDescending(m => m.Title ?? string.Empty, StringComparer.CurrentCultureIgnoreCase);
			else processedMovies = processedMovies.OrderBy(m => m.Title ?? string.Empty, StringComparer.CurrentCultureIgnoreCase);

			DisplayedMovies = new ObservableCollection<Movie>(processedMovies.ToList());
			SelectedMovie = null;
		}

		private void LoadLocalizationStrings()
		{
			_availableGenres = new List<string>
			{
				GetResourceString("AdminPanel_GenreFilter_All"),
				GetResourceString("AdminPanel_GenreFilter_Action"),
				GetResourceString("AdminPanel_GenreFilter_Comedy"),
				GetResourceString("AdminPanel_GenreFilter_Drama"),
				GetResourceString("AdminPanel_GenreFilter_Romance"),
				GetResourceString("AdminPanel_GenreFilter_SciFi"),
				GetResourceString("AdminPanel_GenreFilter_Fantasy"),
				GetResourceString("AdminPanel_GenreFilter_Thriller"),
				GetResourceString("AdminPanel_GenreFilter_Horror"),
				GetResourceString("AdminPanel_GenreFilter_War"),
				GetResourceString("AdminPanel_GenreFilter_Kids"),
				GetResourceString("AdminPanel_GenreFilter_Crime")
			};

			_availableSortOptions = new List<string>
			{
				GetResourceString("AdminPanel_Sort_None"),
				GetResourceString("AdminPanel_Sort_RatingDesc"),
				GetResourceString("AdminPanel_Sort_RatingAsc"),
				GetResourceString("AdminPanel_Sort_DurationDesc"),
				GetResourceString("AdminPanel_Sort_DurationAsc"),
				GetResourceString("AdminPanel_Sort_TitleAZ"),
				GetResourceString("AdminPanel_Sort_TitleZA")
			};

			OnPropertyChanged(nameof(AvailableGenres));
			OnPropertyChanged(nameof(AvailableSortOptions));
		}

		public string GetResourceString(string key)
		{
			return Application.Current.TryFindResource(key) as string ?? $"[{key}]";
		}

		public void ShowMessage(string messageKey, string titleKey, MessageBoxImage icon)
		{
			MessageBox.Show(GetResourceString(messageKey), GetResourceString(titleKey), MessageBoxButton.OK, icon);
		}

		public void ShowMessageFormat(string messageKey, string titleKey, MessageBoxImage icon, params object[] args)
		{
			try
			{
				string msg = string.Format(GetResourceString(messageKey), args);
				MessageBox.Show(msg, GetResourceString(titleKey), MessageBoxButton.OK, icon);
			}
			catch (FormatException)
			{
				MessageBox.Show(GetResourceString(messageKey) + " (Format Error)",
					GetResourceString(titleKey), MessageBoxButton.OK, icon);
			}
		}

		private NavigationService FindNavigationService()
		{
			string frameName = IsUserAdmin() ? "frameMainForAdmin" : "frameMain";
			if (Application.Current.MainWindow?.FindName(frameName) is Frame namedFrame)
			{
				return namedFrame.NavigationService;
			}

			if (Application.Current.MainWindow?.Content is Frame rootFrame)
			{
				return rootFrame.NavigationService;
			}

			if (Application.Current.Windows.OfType<AdminWindow>().FirstOrDefault(w => w.IsActive) is AdminWindow aw && aw.FindName("frameMainForAdmin") is Frame af)
			{
				return af.NavigationService;
			}

			return null;
		}

		private bool IsUserAdmin()
		{
			return Application.Current.MainWindow is AdminWindow;
		}

		public void UpdateLocalization()
		{
			LoadLocalizationStrings();

			string currentGenre = SelectedGenre;
			string currentSort = SelectedSortOption;
			SelectedGenre = null;
			SelectedGenre = _availableGenres.Contains(currentGenre) ? currentGenre : (_availableGenres.FirstOrDefault() ?? GetResourceString("AdminPanel_GenreFilter_All"));
			SelectedSortOption = null;
			SelectedSortOption = _availableSortOptions.Contains(currentSort) ? currentSort : (_availableSortOptions.FirstOrDefault() ?? GetResourceString("AdminPanel_Sort_None")); 
			CommandManager.InvalidateRequerySuggested();
		}
	}
}
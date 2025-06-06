using CinemaMOON.Data;
using CinemaMOON.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.ComponentModel.DataAnnotations;

namespace CinemaMOON.ViewModels
{
    public class AddingFilmPageViewModel : ViewModelBase
	{
		private string _title;
		private string _director;
		private string _selectedGenre;
		private string _duration;
		private string _rating;
		private string _description;
		private string _imagePath;
		private BitmapImage _previewImageSource;
		private bool _isRemovePhotoEnabled;
		private List<string> _availableGenres;
		private List<string> _allowedRatings;

		private AppDbContext _dbContext;

		private string _titleError, _directorError, _genreError, _durationError, _ratingError, _descriptionError, _imageError;
		private bool _isTitleErrorVisible, _isDirectorErrorVisible, _isGenreErrorVisible, _isDurationErrorVisible, _isRatingErrorVisible, _isDescriptionErrorVisible, _isImageErrorVisible;
		
		private const int TitleMaxLength = 128;
		private const int DirectorMaxLength = 128;
		private const int DescriptionMaxLength = 1024;

		public List<string> AvailableGenres => _availableGenres;
		public List<string> AllowedRatings => _allowedRatings;

		public string Title 
		{ 
			get => _title; 
			set 
			{ 
				if (SetProperty(ref _title, value)) 
					ValidateTitle(); 
			} 
		}

		public string Director
		{ 
			get => _director; 
			set 
			{ 
				if (SetProperty(ref _director, value)) 
					ValidateDirector(); 
			} 
		}

		public string SelectedGenre 
		{ 
			get => _selectedGenre; 
			set 
			{ 
				if (SetProperty(ref _selectedGenre, string.IsNullOrEmpty(value) ? null : value)) 
					ValidateGenre(); 
			} 
		}

		public string Duration 
		{ 
			get => _duration; 
			set 
			{ 
				if (SetProperty(ref _duration, value)) 
					ValidateDuration(); 
			} 
		}

		public string Rating 
		{ 
			get => _rating; 
			set 
			{ 
				if (SetProperty(ref _rating, string.IsNullOrEmpty(value) ? null : value)) 
					ValidateRating(); 
			} 
		}

		public string Description 
		{ 
			get => _description; 
			set 
			{ 
				if (SetProperty(ref _description, value)) 
					ValidateDescription(); 
			} 
		}

		public string ImagePath 
		{ 
			get => _imagePath; 
			private set 
			{ 
				if (SetProperty(ref _imagePath, value))
				{ 
					IsRemovePhotoEnabled = !string.IsNullOrEmpty(_imagePath); 
					ValidateImage(); 
				} 
			} 
		}

		public BitmapImage PreviewImageSource
		{ 
			get => _previewImageSource; 
			private set => SetProperty(ref _previewImageSource, value); 
		}

		public bool IsRemovePhotoEnabled 
		{ 
			get => _isRemovePhotoEnabled; 
			private set => SetProperty(ref _isRemovePhotoEnabled, value); 
		}

		public string TitleError 
		{ 
			get => _titleError; 
			private set => SetProperty(ref _titleError, value); 
		}

		public string DirectorError 
		{ 
			get => _directorError; 
			private set => SetProperty(ref _directorError, value); 
		}

		public string GenreError 
		{ 
			get => _genreError; 
			private set => SetProperty(ref _genreError, value); 
		}

		public string DurationError 
		{ 
			get => _durationError; 
			private set => SetProperty(ref _durationError, value);
		}

		public string RatingError 
		{ 
			get => _ratingError; 
			private set => SetProperty(ref _ratingError, value); 
		}

		public string DescriptionError 
		{
			get => _descriptionError; 
			private set => SetProperty(ref _descriptionError, value); 
		}

		public string ImageError 
		{
			get => _imageError; 
			private set => SetProperty(ref _imageError, value); 
		}

		public bool IsTitleErrorVisible 
		{ 
			get => _isTitleErrorVisible;
			private set => SetProperty(ref _isTitleErrorVisible, value); 
		}

		public bool IsDirectorErrorVisible 
		{ 
			get => _isDirectorErrorVisible; 
			private set => SetProperty(ref _isDirectorErrorVisible, value); 
		}

		public bool IsGenreErrorVisible 
		{ 
			get => _isGenreErrorVisible; 
			private set => SetProperty(ref _isGenreErrorVisible, value); 
		}

		public bool IsDurationErrorVisible 
		{ 
			get => _isDurationErrorVisible; 
			private set => SetProperty(ref _isDurationErrorVisible, value); 
		}

		public bool IsRatingErrorVisible 
		{
			get => _isRatingErrorVisible;
			private set => SetProperty(ref _isRatingErrorVisible, value); 
		}

		public bool IsDescriptionErrorVisible
		{
			get => _isDescriptionErrorVisible; 
			private set => SetProperty(ref _isDescriptionErrorVisible, value);
		}

		public bool IsImageErrorVisible 
		{ 
			get => _isImageErrorVisible;
			private set => SetProperty(ref _isImageErrorVisible, value); 
		}

		public ICommand AddFilmCommand { get; }
		public ICommand ClearFormCommand { get; }
		public ICommand SelectImageCommand { get; }
		public ICommand RemoveImageCommand { get; }
		public ICommand GoBackCommand { get; }

		public AddingFilmPageViewModel(AppDbContext dbContext)
		{
			_dbContext = dbContext;
			LoadAvailableGenres(); LoadAllowedRatings();
			AddFilmCommand = new RelayCommand(ExecuteAddFilm, CanExecuteAddFilm);
			ClearFormCommand = new RelayCommand(ExecuteClearForm);
			SelectImageCommand = new RelayCommand(ExecuteSelectImage);
			RemoveImageCommand = new RelayCommand(ExecuteRemoveImage, CanExecuteRemoveImage);
			GoBackCommand = new RelayCommand(ExecuteGoBack, CanExecuteGoBack);
		}

		private bool CanExecuteAddFilm(object parameter)
		{
			return !IsTitleErrorVisible && !IsDirectorErrorVisible && !IsGenreErrorVisible &&
				   !IsDurationErrorVisible && !IsRatingErrorVisible && !IsDescriptionErrorVisible &&
				   !IsImageErrorVisible && !string.IsNullOrEmpty(ImagePath);
		}

		private async void ExecuteAddFilm(object parameter)
		{
			if (!await ValidateAll())
			{
				ShowMessage("AddFilmPage_Error_ValidationFails", "AddFilmPage_Title_InputError", MessageBoxImage.Warning);
				return;
			}

			int.TryParse(Duration, out int durationMinutes);
			try
			{
				var newMovie = new Movie
				{
					Title = Title.Trim(),
					Director = Director.Trim(),
					Genre = SelectedGenre,
					Duration = durationMinutes,
					Rating = Rating,
					Description = Description.Trim(),
					Photo = ImagePath
				};

				var existingMovie = await _dbContext.Movies.FirstOrDefaultAsync(m => !m.IsDeleted && m.Title == newMovie.Title);
				if (existingMovie != null)
				{
					SetValidationError(nameof(TitleError), nameof(IsTitleErrorVisible), GetResourceString("AddFilmPage_Error_TitleDuplicate"));
					return;
				}

				_dbContext.Movies.Add(newMovie);
				await _dbContext.SaveChangesAsync();

				ShowMessageFormat("AddFilmPage_Success_MovieAdded", "AddFilmPage_Title_Success", MessageBoxImage.Information, newMovie.Title);
				ExecuteClearForm(null);
			}
			catch (Exception ex)
			{
				ShowMessageFormat("AddFilmPage_Error_AddingFailed", "AddFilmPage_Title_Error", MessageBoxImage.Error, ex.Message);
			}
		}

		private void ExecuteClearForm(object parameter)
		{
			Title = Director = Duration = Description = ImagePath = null;
			SelectedGenre = null;
			Rating = null;
			PreviewImageSource = null;
			ResetAllValidationErrors();
		}

		private async void ExecuteSelectImage(object parameter)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*",
				InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
				Title = GetResourceString("AddFilmPage_FileDialog_Title")
			};

			if (openFileDialog.ShowDialog() == true)
			{
				try
				{
					string selectedPath = openFileDialog.FileName;
					if (!File.Exists(selectedPath))
					{
						ShowMessage("AddFilmPage_Error_FileNotFound", "AddFilmPage_Title_FileError", MessageBoxImage.Error);
						return;
					}

					var existingMovie = await _dbContext.Movies.FirstOrDefaultAsync(m => !m.IsDeleted && m.Photo == selectedPath);
					if (existingMovie != null)
					{
						SetValidationError(nameof(ImageError), nameof(IsImageErrorVisible), GetResourceString("AddFilmPage_Error_ImageDuplicate"));
						ImagePath = null;
						PreviewImageSource = null;
						CommandManager.InvalidateRequerySuggested();
						return;
					}

					BitmapImage bitmap = new BitmapImage();
					bitmap.BeginInit();
					bitmap.UriSource = new Uri(selectedPath);
					bitmap.CacheOption = BitmapCacheOption.OnLoad;
					bitmap.EndInit();
					bitmap.Freeze();
					PreviewImageSource = bitmap;
					ImagePath = selectedPath;
				}
				catch (NotSupportedException)
				{
					SetValidationError(nameof(ImageError), nameof(IsImageErrorVisible), GetResourceString("AddFilmPage_Error_ImageFormat"));
					ImagePath = null;
					PreviewImageSource = null;
				}
				catch (Exception ex)
				{
					ShowMessageFormat("AddFilmPage_Error_ImageLoad", "AddFilmPage_Title_Error", MessageBoxImage.Error, ex.Message);
					ImagePath = null;
					PreviewImageSource = null;
				}
			}
		}

		private bool CanExecuteRemoveImage(object parameter) 
		{ 
			return IsRemovePhotoEnabled;
		}

		private void ExecuteRemoveImage(object parameter) 
		{ 
			PreviewImageSource = null; 
			ImagePath = null; 
		}

		private bool CanExecuteGoBack(object parameter)
		{ 
			var navService = GetNavigationService(parameter as Page); 
			return navService?.CanGoBack ?? false; 
		}

		private void ExecuteGoBack(object parameter)
		{ 
			var navService = GetNavigationService(parameter as Page); 
			if (navService != null && navService.CanGoBack) 
			{ 
				navService.GoBack();
			} 
			else
			{ 
				ShowMessage("AddFilmPage_Error_CannotGoBack", "AddFilmPage_Title_NavigationError", MessageBoxImage.Warning); 
			} 
		}

		private async Task<bool> ValidateAll()
		{
			bool isValid = await ValidateTitle();
			isValid &= ValidateDirector();
			isValid &= ValidateGenre();
			isValid &= ValidateDuration();
			isValid &= ValidateRating();
			isValid &= await ValidateDescription();
			isValid &= await ValidateImage();
			return isValid;
		}

		private void SetValidationError(string errorProperty, string visibilityProperty, string message)
		{
			GetType().GetProperty(errorProperty)?.SetValue(this, message);
			GetType().GetProperty(visibilityProperty)?.SetValue(this, true);
		}

		private void ResetValidationError(string errorProperty, string visibilityProperty)
		{
			GetType().GetProperty(errorProperty)?.SetValue(this, string.Empty);
			GetType().GetProperty(visibilityProperty)?.SetValue(this, false);
		}

		private async Task<bool> ValidateTitle()
		{
			bool isValid = true;
			string currentTitleTrimmed = Title?.Trim(); 

			if (string.IsNullOrWhiteSpace(currentTitleTrimmed))
			{
				SetValidationError(nameof(TitleError), nameof(IsTitleErrorVisible), GetResourceString("AddFilmPage_Error_FieldRequired"));
				isValid = false;
			}
			else if (currentTitleTrimmed.Length > TitleMaxLength)
			{
				SetValidationError(nameof(TitleError), nameof(IsTitleErrorVisible),
					string.Format(GetResourceString("AddFilmPage_Error_TitleMaxLength") ?? "Название не должно превышать {0} символов.", TitleMaxLength));
				isValid = false;
			}
			else
			{
				var existingMovie = await _dbContext.Movies.FirstOrDefaultAsync(m => m.Title == currentTitleTrimmed && !m.IsDeleted);
				if (existingMovie != null)
				{
					SetValidationError(nameof(TitleError), nameof(IsTitleErrorVisible), GetResourceString("AddFilmPage_Error_TitleDuplicate"));
					isValid = false;
				}
				else
				{
					ResetValidationError(nameof(TitleError), nameof(IsTitleErrorVisible));
				}
			}
			CommandManager.InvalidateRequerySuggested();
			return isValid;
		}

		private bool ValidateDirector()
		{
			bool isValid = true;
			string currentDirectorTrimmed = Director?.Trim();

			if (string.IsNullOrWhiteSpace(currentDirectorTrimmed))
			{
				SetValidationError(nameof(DirectorError), nameof(IsDirectorErrorVisible), GetResourceString("AddFilmPage_Error_FieldRequired"));
				isValid = false;
			}
			else if (currentDirectorTrimmed.Length > DirectorMaxLength) 
			{
				SetValidationError(nameof(DirectorError), nameof(IsDirectorErrorVisible),
					string.Format(GetResourceString("AddFilmPage_Error_DirectorMaxLength") ?? "Режиссер не должен превышать {0} символов.", DirectorMaxLength));
				isValid = false;
			}
			else
			{
				ResetValidationError(nameof(DirectorError), nameof(IsDirectorErrorVisible));
			}
			CommandManager.InvalidateRequerySuggested();
			return isValid;
		}

		private bool ValidateGenre()
		{
			bool isValid = !string.IsNullOrWhiteSpace(SelectedGenre);
			if (isValid)
			{
				ResetValidationError(nameof(GenreError), nameof(IsGenreErrorVisible));
			}
			else
			{
				SetValidationError(nameof(GenreError), nameof(IsGenreErrorVisible), GetResourceString("AddFilmPage_Error_GenreRequired"));
			}
			CommandManager.InvalidateRequerySuggested();
			return isValid;
		}

		private bool ValidateDuration()
		{
			bool isValid = true;
			if (string.IsNullOrWhiteSpace(Duration))
			{
				SetValidationError(nameof(DurationError), nameof(IsDurationErrorVisible), GetResourceString("AddFilmPage_Error_FieldRequired"));
				isValid = false;
			}
			else if (!int.TryParse(Duration, out int d) || d <= 0 || d > 300)
			{
				SetValidationError(nameof(DurationError), nameof(IsDurationErrorVisible), GetResourceString("AddFilmPage_Error_DurationRange"));
				isValid = false;
			}
			else
			{
				ResetValidationError(nameof(DurationError), nameof(IsDurationErrorVisible));
			}
			CommandManager.InvalidateRequerySuggested();
			return isValid;
		}

		private bool ValidateRating()
		{
			bool isValid = !string.IsNullOrWhiteSpace(Rating) && _allowedRatings.Contains(Rating);
			if (isValid)
			{
				ResetValidationError(nameof(RatingError), nameof(IsRatingErrorVisible));
			}
			else
			{
				SetValidationError(nameof(RatingError), nameof(IsRatingErrorVisible), GetResourceString("AddFilmPage_Error_RatingInvalid"));
			}
			CommandManager.InvalidateRequerySuggested();
			return isValid;
		}

		private async Task<bool> ValidateDescription()
		{
			bool isValid = true;
			string currentDescTrimmed = Description?.Trim();

			if (string.IsNullOrWhiteSpace(currentDescTrimmed))
			{
				SetValidationError(nameof(DescriptionError), nameof(IsDescriptionErrorVisible), GetResourceString("AddFilmPage_Error_FieldRequired"));
				isValid = false;
			}
			else if (currentDescTrimmed.Length > DescriptionMaxLength) 
			{
				SetValidationError(nameof(DescriptionError), nameof(IsDescriptionErrorVisible),
					string.Format(GetResourceString("AddFilmPage_Error_DescriptionMaxLength") ?? "Описание не должно превышать {0} символов.", DescriptionMaxLength));
				isValid = false;
			}
			else
			{
				var existingMovie = await _dbContext.Movies.FirstOrDefaultAsync(m => !m.IsDeleted && m.Description == currentDescTrimmed);
				if (existingMovie != null)
				{
					SetValidationError(nameof(DescriptionError), nameof(IsDescriptionErrorVisible), GetResourceString("AddFilmPage_Error_DescriptionDuplicate"));
					isValid = false;
				}
				else
				{
					ResetValidationError(nameof(DescriptionError), nameof(IsDescriptionErrorVisible));
				}
			}
			CommandManager.InvalidateRequerySuggested();
			return isValid;
		}

		private async Task<bool> ValidateImage()
		{
			bool isValid = true;
			if (string.IsNullOrEmpty(ImagePath))
			{
				SetValidationError(nameof(ImageError), nameof(IsImageErrorVisible), GetResourceString("AddFilmPage_Error_ImageRequired"));
				isValid = false;
			}
			else if (!File.Exists(ImagePath))
			{
				SetValidationError(nameof(ImageError), nameof(IsImageErrorVisible), GetResourceString("AddFilmPage_Error_FileNotFound"));
				isValid = false;
			}
			else
			{
				var existingMovie = await _dbContext.Movies.FirstOrDefaultAsync(m => !m.IsDeleted && m.Photo == ImagePath);
				if (existingMovie != null)
				{
					SetValidationError(nameof(ImageError), nameof(IsImageErrorVisible), GetResourceString("AddFilmPage_Error_ImageDuplicate"));
					isValid = false;
				}
				else
				{
					ResetValidationError(nameof(ImageError), nameof(IsImageErrorVisible));
				}
			}
			CommandManager.InvalidateRequerySuggested();
			return isValid;
		}

		private void ResetAllValidationErrors()
		{
			ResetValidationError(nameof(TitleError), nameof(IsTitleErrorVisible));
			ResetValidationError(nameof(DirectorError), nameof(IsDirectorErrorVisible));
			ResetValidationError(nameof(GenreError), nameof(IsGenreErrorVisible));
			ResetValidationError(nameof(DurationError), nameof(IsDurationErrorVisible));
			ResetValidationError(nameof(RatingError), nameof(IsRatingErrorVisible));
			ResetValidationError(nameof(DescriptionError), nameof(IsDescriptionErrorVisible));
			ResetValidationError(nameof(ImageError), nameof(IsImageErrorVisible));
		}

		private void LoadAvailableGenres()
		{
			if (_availableGenres == null) _availableGenres = new List<string>();
			_availableGenres.Clear();
			_availableGenres.AddRange(new[]
			{
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
			});
			OnPropertyChanged(nameof(AvailableGenres));
		}

		private void LoadAllowedRatings()
		{
			if (_allowedRatings == null) _allowedRatings = new List<string>();
			_allowedRatings.Clear();
			_allowedRatings.AddRange(new[] { "0+", "6+", "12+", "16+", "18+" });
			OnPropertyChanged(nameof(AllowedRatings));
		}

		private NavigationService GetNavigationService(Page currentPage = null)
		{
			if (currentPage?.NavigationService != null)
			{
				return currentPage.NavigationService;
			}
			if (Application.Current.MainWindow?.FindName("frameMainForAdmin") is Frame mainFrame)
			{
				return mainFrame.NavigationService;
			}
			if (Application.Current.MainWindow?.Content is Frame rootFrame)
			{
				return rootFrame.NavigationService;
			}
			return null;
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
			try
			{
				string m = string.Format(GetResourceString(messageKey), args);
				MessageBox.Show(m, GetResourceString(titleKey), MessageBoxButton.OK, icon);
			}
			catch
			{
				MessageBox.Show(GetResourceString(messageKey) + "(Format Error)", GetResourceString(titleKey), MessageBoxButton.OK, icon);
			}
		}
	}
}
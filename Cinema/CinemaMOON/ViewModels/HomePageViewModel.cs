using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CinemaMOON.Data;

namespace CinemaMOON.ViewModels
{
    public class HomePageViewModel : ViewModelBase
	{
		private readonly AppDbContext _dbContext;

		private readonly List<CarouselItemViewModel> _allItems;
		private ObservableCollection<CarouselItemViewModel> _displayItems;
		private int _currentCenterIndex;
		private bool _isAnimating;
		private bool _isLoadingData = false;

		private readonly DispatcherTimer _autoSlideTimer;

		public event EventHandler<CarouselAnimationEventArgs> RequestAnimate;

		public ObservableCollection<CarouselItemViewModel> DisplayItems
		{
			get => _displayItems;
			private set => SetProperty(ref _displayItems, value);
		}

		public ICommand MoveNextCommand { get; }
		public ICommand MovePreviousCommand { get; }
		public ICommand SwitchLanguageCommand { get; }
		public ICommand ToggleThemeCommand { get; }
		public IAsyncRelayCommand LoadCommand { get; }
		public ICommand UnloadCommand { get; }

		public HomePageViewModel(AppDbContext dbContext)
		{
			_dbContext = dbContext;
			_allItems = new List<CarouselItemViewModel>();
			_displayItems = new ObservableCollection<CarouselItemViewModel>();
			_currentCenterIndex = 0;
			_isAnimating = false;

			_autoSlideTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
			_autoSlideTimer.Tick += AutoSlideTimer_Tick;

			MoveNextCommand = new RelayCommand(param => MoveNext(), param => CanMoveNext());
			MovePreviousCommand = new RelayCommand(param => MovePrevious(), param => CanMovePrevious());
			SwitchLanguageCommand = new RelayCommand(SwitchLanguage);
			ToggleThemeCommand = new RelayCommand(ExecuteToggleTheme, CanExecuteToggleTheme);
			LoadCommand = new AsyncRelayCommand(LoadData, CanLoadData); 
			UnloadCommand = new RelayCommand(Unload);
		}

		private bool CanExecuteToggleTheme(object parameter)
		{
			return true;
		}

		private void ExecuteToggleTheme(object parameter)
		{
			App.ToggleTheme();
		}

		private bool CanLoadData()
		{
			return !_isLoadingData;
		}

		private void Load(object parameter)
		{
			if (LoadCommand.CanExecute(null))
			{
				_ = LoadCommand.ExecuteAsync(null);
			}
			else if (_allItems.Any())
			{
				StartAutoSlide();
			}
		}

		private void Unload(object parameter)
		{
			StopAutoSlide();
			RequestAnimate = null;
		}

		private int GetEffectiveIndex(int index)
		{
			if (_allItems == null || _allItems.Count == 0) return 0;
			return (index % _allItems.Count + _allItems.Count) % _allItems.Count;
		}

		private void UpdateDisplayItems()
		{
			if (_allItems == null || _allItems.Count == 0) return;

			int prevIndex = GetEffectiveIndex(_currentCenterIndex - 1);
			int currentIndex = GetEffectiveIndex(_currentCenterIndex);
			int nextIndex = GetEffectiveIndex(_currentCenterIndex + 1);

			var newDisplayItems = new List<CarouselItemViewModel>();

			if (_allItems.Count >= 3)
			{
				newDisplayItems.Add(_allItems[prevIndex]);
				newDisplayItems.Add(_allItems[currentIndex]);
				newDisplayItems.Add(_allItems[nextIndex]);
			}
			else if (_allItems.Count == 2)
			{
				newDisplayItems.Add(_allItems[prevIndex]);
				newDisplayItems.Add(_allItems[currentIndex]);
				newDisplayItems.Add(_allItems[prevIndex]);
			}
			else
			{
				newDisplayItems.Add(_allItems[currentIndex]);
				newDisplayItems.Add(_allItems[currentIndex]);
				newDisplayItems.Add(_allItems[currentIndex]);
			}

			DisplayItems = new ObservableCollection<CarouselItemViewModel>(newDisplayItems);
			UpdateScaling();
		}

		private void UpdateScaling()
		{
			if (DisplayItems == null || DisplayItems.Count < 1) return;

			if (DisplayItems.Count >= 3)
			{
				DisplayItems[0].ScaleFactor = 0.8; DisplayItems[0].ZIndex = 90;
				DisplayItems[1].ScaleFactor = 1.0; DisplayItems[1].ZIndex = 100;
				DisplayItems[2].ScaleFactor = 0.8; DisplayItems[2].ZIndex = 90;
			}
			else if (DisplayItems.Count == 1)
			{
				DisplayItems[0].ScaleFactor = 1.0; DisplayItems[0].ZIndex = 100;
			}
		}

		private bool CanMoveNext() => _allItems != null && _allItems.Count > 1 && !_isAnimating && !_isLoadingData;
		private bool CanMovePrevious() => _allItems != null && _allItems.Count > 1 && !_isAnimating && !_isLoadingData;

		private void MoveNext()
		{
			StopAutoSlide();
			_isAnimating = true;
			CommandManager.InvalidateRequerySuggested();

			RequestAnimate?.Invoke(this, new CarouselAnimationEventArgs(-260, () =>
			{
				_currentCenterIndex = GetEffectiveIndex(_currentCenterIndex + 1);
				UpdateDisplayItems();
				_isAnimating = false;
				StartAutoSlide();
				CommandManager.InvalidateRequerySuggested();
			}));
		}

		private void MovePrevious()
		{
			StopAutoSlide();
			_isAnimating = true;
			CommandManager.InvalidateRequerySuggested();

			RequestAnimate?.Invoke(this, new CarouselAnimationEventArgs(260, () =>
			{
				_currentCenterIndex = GetEffectiveIndex(_currentCenterIndex - 1);
				UpdateDisplayItems();
				_isAnimating = false;
				StartAutoSlide();
				CommandManager.InvalidateRequerySuggested();
			}));
		}

		private void AutoSlideTimer_Tick(object sender, EventArgs e)
		{
			if (!_isAnimating)
			{
				MoveNext();
			}
		}

		private void StartAutoSlide()
		{
			_autoSlideTimer.Start();
		}

		private void StopAutoSlide()
		{
			_autoSlideTimer.Stop();
		}

		private void SwitchLanguage(object parameter)
		{
			if (parameter is string langCode)
			{
				try
				{
					App.Language = new CultureInfo(langCode);
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Error switching language to {langCode}: {ex.Message}", "Language Error", MessageBoxButton.OK, MessageBoxImage.Warning);
				}
			}
		}

		public async Task LoadData()
		{
			if (_isLoadingData) return;
			_isLoadingData = true;
			CommandManager.InvalidateRequerySuggested(); 
			StopAutoSlide();

			_allItems.Clear();
			DisplayItems.Clear();
			_currentCenterIndex = 0;

			try
			{
				var moviesQuery = _dbContext.Movies.Where(m => !m.IsDeleted && m.IsActive);

				var movies = await moviesQuery.OrderBy(m => m.Title).Take(5).ToListAsync();

				if (movies != null && movies.Any())
				{
					_allItems.AddRange(movies.Select(movie => new CarouselItemViewModel(movie)));
					UpdateDisplayItems(); 
					StartAutoSlide();
				}
				else
				{
					StopAutoSlide();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
				StopAutoSlide();
			}
			finally
			{
				_isLoadingData = false; 
				CommandManager.InvalidateRequerySuggested();
			}
		}

	}

	public class CarouselAnimationEventArgs : EventArgs
	{
		public double TargetOffset { get; }
		public Action OnAnimationComplete { get; }

		public CarouselAnimationEventArgs(double targetOffset, Action onAnimationComplete)
		{
			TargetOffset = targetOffset;
			OnAnimationComplete = onAnimationComplete;
		}
	}
}
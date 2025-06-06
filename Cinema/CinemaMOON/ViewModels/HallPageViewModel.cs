using System.Windows.Input;
using CinemaMOON.Models;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using CinemaMOON.Data;

namespace CinemaMOON.ViewModels
{
    public class HallPageViewModel : ViewModelBase
	{
		private readonly AppDbContext _dbContext; 
		private List<Hall> _hallInfoList;
		private int _currentHallIndex;

		private string _currentHallTitle = "Зал Загружается...";
		public string CurrentHallTitle
		{
			get => _currentHallTitle;
			private set => SetProperty(ref _currentHallTitle, value);
		}

		private bool _isSmallHallVisible;
		public bool IsSmallHallVisible
		{
			get => _isSmallHallVisible;
			private set => SetProperty(ref _isSmallHallVisible, value);
		}

		private bool _isMediumHallVisible;
		public bool IsMediumHallVisible
		{
			get => _isMediumHallVisible;
			private set => SetProperty(ref _isMediumHallVisible, value);
		}

		private bool _isLargeHallVisible;
		public bool IsLargeHallVisible
		{
			get => _isLargeHallVisible;
			private set => SetProperty(ref _isLargeHallVisible, value);
		}

		public ICommand PreviousCommand { get; }
		public ICommand NextCommand { get; }
		public IAsyncRelayCommand LoadHallsCommand { get; }


		public HallPageViewModel(AppDbContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_hallInfoList = new List<Hall>(); 

			PreviousCommand = new RelayCommand(ExecutePrevious, CanExecutePreviousOrNext); 
			NextCommand = new RelayCommand(ExecuteNext, CanExecutePreviousOrNext);       
			LoadHallsCommand = new AsyncRelayCommand(LoadHallsAsync);

			_ = LoadHallsAsync();
		}

		private async Task LoadHallsAsync()
		{
			try
			{
				_hallInfoList = await _dbContext.Halls.OrderBy(h => h.Name).ToListAsync();

				if (_hallInfoList.Any())
				{
					_currentHallIndex = 0; 
					UpdateHallState();
				}
				else
				{
					CurrentHallTitle = (string)App.Current.FindResource("HallPage_ErrorNoHalls");
					IsSmallHallVisible = false;
					IsMediumHallVisible = false;
					IsLargeHallVisible = false;
				}
			}
			catch (Exception ex)
			{
				CurrentHallTitle = (string)App.Current.FindResource("HallPage_ErrorLoading");
				IsSmallHallVisible = false;
				IsMediumHallVisible = false;
				IsLargeHallVisible = false;
			}
			finally
			{
				CommandManager.InvalidateRequerySuggested();
			}
		}

		private void ExecutePrevious(object parameter)
		{
			int newIndex = _currentHallIndex - 1;
			if (newIndex < 0)
			{
				newIndex = _hallInfoList.Count - 1;
			}
			_currentHallIndex = newIndex;
			UpdateHallState();
		}

		private bool CanExecutePreviousOrNext(object parameter)
		{
			return _hallInfoList != null && _hallInfoList.Count > 1; 
		}

		private void ExecuteNext(object parameter)
		{
			int newIndex = _currentHallIndex + 1;
			if (newIndex >= _hallInfoList.Count)
			{
				newIndex = 0;
			}
			_currentHallIndex = newIndex;
			UpdateHallState();
		}

		private void UpdateHallState()
		{
			if (_currentHallIndex >= 0 && _currentHallIndex < _hallInfoList.Count)
			{
				Hall current = _hallInfoList[_currentHallIndex];

				string localizedHallName = (string)Application.Current.TryFindResource(current.Name) ?? current.Name;

				string format = (string)App.Current.FindResource("HallPage_HallTitleFormat");
				CurrentHallTitle = string.Format(format, localizedHallName, current.Capacity);

				IsSmallHallVisible = current.Type.Equals("small", StringComparison.OrdinalIgnoreCase);
				IsMediumHallVisible = current.Type.Equals("medium", StringComparison.OrdinalIgnoreCase);
				IsLargeHallVisible = current.Type.Equals("large", StringComparison.OrdinalIgnoreCase);
			}
			else
			{
				CurrentHallTitle = (string)App.Current.FindResource("HallPage_ErrorLoading");
				IsSmallHallVisible = false;
				IsMediumHallVisible = false;
				IsLargeHallVisible = false;
			}
			CommandManager.InvalidateRequerySuggested();
		}
	}
}
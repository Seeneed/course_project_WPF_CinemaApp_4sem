using CinemaMOON.Data;
using CinemaMOON.Models;
using CommunityToolkit.Mvvm.Input;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CinemaMOON.ViewModels
{
    public class RateMovieViewModel : ViewModelBase
	{
		private readonly AppDbContext _dbContext;
		private readonly Order _orderToRate;
		private readonly User _currentUser;

		private string _movieTitle;
		private string _ratingInputString; 
		private string _ratingError;
		private bool _isRatingErrorVisible;
		private bool _isAlreadyRated;
		private string _alreadyRatedText;

		public string MovieTitle
		{
			get => _movieTitle;
			private set => SetProperty(ref _movieTitle, value);
		}

		public string RatingInputString
		{
			get => _ratingInputString;
			set
			{
				string processedValue = string.Concat(value?.Where(char.IsDigit) ?? Enumerable.Empty<char>());
				if (processedValue.Length > 2) processedValue = processedValue.Substring(0, 2);

				if (SetProperty(ref _ratingInputString, processedValue))
				{
					ValidateRatingInput();
					SubmitRatingCommand.NotifyCanExecuteChanged();
				}
			}
		}

		public string RatingError
		{
			get => _ratingError;
			private set => SetProperty(ref _ratingError, value);
		}

		public bool IsRatingErrorVisible
		{
			get => _isRatingErrorVisible; 
			private set => SetProperty(ref _isRatingErrorVisible, value); 
		}

		public bool IsAlreadyRated 
		{
			get => _isAlreadyRated; 
			private set => SetProperty(ref _isAlreadyRated, value); 
		}

		public string AlreadyRatedText 
		{
			get => _alreadyRatedText;
			private set => SetProperty(ref _alreadyRatedText, value);
		}

		public IAsyncRelayCommand SubmitRatingCommand { get; }
		public ICommand GoBackCommand { get; }

		public RateMovieViewModel(AppDbContext dbContext, Order orderToRate, User currentUser)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_orderToRate = orderToRate ?? throw new ArgumentNullException(nameof(orderToRate));
			_currentUser = currentUser;

			MovieTitle = _orderToRate.Schedule?.Movie?.Title ?? GetResourceString("UnknownMovie");

			if (_orderToRate.UserRating.HasValue)
			{
				IsAlreadyRated = true;
				_ratingInputString = _orderToRate.UserRating.Value.ToString();
				OnPropertyChanged(nameof(RatingInputString));
				AlreadyRatedText = string.Format(GetResourceString("RateMoviePage_AlreadyRated") ?? "Уже оценено: {0}/10", _orderToRate.UserRating.Value);
			}
			else
			{
				IsAlreadyRated = false;
				_ratingInputString = string.Empty;
				_ratingInputString = string.Empty;
			}

			SubmitRatingCommand = new AsyncRelayCommand(ExecuteSubmitRatingAsync, CanExecuteSubmitRating); 
			GoBackCommand = new RelayCommand<Page>(ExecuteGoBack);

			ValidateRatingInput();
		}

		private bool ValidateRatingInput()
		{
			if (IsAlreadyRated)
			{
				RatingError = string.Empty;
				IsRatingErrorVisible = false;
				return false; 
			}

			if (string.IsNullOrWhiteSpace(RatingInputString))
			{
				RatingError = GetResourceString("RateMoviePage_Error_RatingEmpty");
				IsRatingErrorVisible = true;
				return false;
			}

			if (!int.TryParse(RatingInputString, NumberStyles.Integer, CultureInfo.InvariantCulture, out int ratingValue))
			{
				RatingError = GetResourceString("RateMoviePage_Error_RatingNotANumber");
				IsRatingErrorVisible = true;
				return false;
			}

			if (ratingValue < 1 || ratingValue > 10)
			{
				RatingError = GetResourceString("RateMoviePage_Error_RatingInvalidRange");
				IsRatingErrorVisible = true;
				return false;
			}

			RatingError = string.Empty;
			IsRatingErrorVisible = false;
			return true;
		}

		private bool CanExecuteSubmitRating()
		{
			if (IsAlreadyRated) return false;
			return !string.IsNullOrWhiteSpace(RatingInputString) && string.IsNullOrEmpty(RatingError);
		}

		private async Task ExecuteSubmitRatingAsync()
		{
			if (!ValidateRatingInput())
			{
				SubmitRatingCommand.NotifyCanExecuteChanged(); 
				return;
			}

			try
			{
				var orderInDb = await _dbContext.Orders.FindAsync(_orderToRate.Id);
				if (orderInDb == null)
				{
					ShowMessage("AccountPage_Error_OrderNotFoundInDb", "AdminPanel_Title_Error", MessageBoxImage.Error);
					return;
				}

				int newRating = int.Parse(RatingInputString, CultureInfo.InvariantCulture);
				orderInDb.UserRating = newRating;
				await _dbContext.SaveChangesAsync();

				_orderToRate.UserRating = newRating;

				ShowMessage("RateMoviePage_Success_RatingSaved", "AccountPage_Success_Title", MessageBoxImage.Information);

				IsAlreadyRated = true;
				AlreadyRatedText = string.Format(GetResourceString("RateMoviePage_AlreadyRated") ?? "Вы уже оценили: {0}/10", newRating);
				SubmitRatingCommand.NotifyCanExecuteChanged();
			}
			catch (Exception ex)
			{
				ShowMessage("RateMoviePage_Error_SavingRating", "AdminPanel_Title_Error", MessageBoxImage.Error);
			}
		}

		private void ExecuteGoBack(Page currentPage)
		{
			if (currentPage?.NavigationService?.CanGoBack == true)
			{
				currentPage.NavigationService.GoBack();
			}
		}

		private string GetResourceString(string key) => Application.Current.TryFindResource(key) as string ?? $"[{key}]";

		private void ShowMessage(string messageKey, string titleKey, MessageBoxImage icon)
		{
			MessageBox.Show(GetResourceString(messageKey), GetResourceString(titleKey), MessageBoxButton.OK, icon);
		}

		private void ShowMessageFormat(string messageKey, string titleKey, MessageBoxImage icon, params object[] args)
		{
			string messageFormat = GetResourceString(messageKey);
			string message = string.Format(messageFormat, args);
			MessageBox.Show(message, GetResourceString(titleKey), MessageBoxButton.OK, icon);
		}
	}
}
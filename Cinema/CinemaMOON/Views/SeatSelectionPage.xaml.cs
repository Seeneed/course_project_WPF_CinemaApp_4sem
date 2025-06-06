using CinemaMOON.Data;
using CinemaMOON.Models;
using CinemaMOON.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CinemaMOON.Views
{
    public partial class SeatSelectionPage : Page
	{
		private SeatSelectionPageViewModel _viewModel;

		public SeatSelectionPage(AppDbContext dbContext, Schedule selectedSchedule, User currentUser)
		{
			InitializeComponent();
			_viewModel = new SeatSelectionPageViewModel(dbContext, selectedSchedule, currentUser);
			this.DataContext = _viewModel;

			this.Loaded += SeatSelectionPage_Loaded;
		}

		private async void SeatSelectionPage_Loaded(object sender, RoutedEventArgs e)
		{
			if (_viewModel != null)
			{
				await _viewModel.RefreshDataOnPageLoadAsync();
			}
		}

		private void Seat_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (_viewModel != null && sender is FrameworkElement seatElement)
			{
				string seatId = seatElement.Tag as string;

				if (!string.IsNullOrEmpty(seatId))
				{
					if (_viewModel.SelectSeatCommand.CanExecute(seatId))
					{
						_viewModel.SelectSeatCommand.Execute(seatId);
					}
				}
			}
		}
	}
}
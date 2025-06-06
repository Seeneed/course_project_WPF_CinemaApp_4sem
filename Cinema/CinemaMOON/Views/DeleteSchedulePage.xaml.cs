using CinemaMOON.Data;
using CinemaMOON.Models;
using CinemaMOON.ViewModels;
using System.Windows.Controls;

namespace CinemaMOON.Views
{
    public partial class DeleteSchedulePage : Page
	{
		public DeleteSchedulePage(AppDbContext dbContext, Movie movie)
		{
			InitializeComponent();
			this.DataContext = new DeleteSchedulePageViewModel(dbContext, movie);
		}
	}
}
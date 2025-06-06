using CinemaMOON.Data;
using CinemaMOON.Models;
using CinemaMOON.ViewModels;
using System.Windows.Controls;

namespace CinemaMOON.Views
{
    public partial class MovieDetailsPage : Page
	{
		public MovieDetailsPage(AppDbContext dbContext, Movie selectedMovie, User? currentUser)
		{
			InitializeComponent();
			this.DataContext = new MovieDetailsPageViewModel(dbContext, selectedMovie, currentUser);
		}
	}
}
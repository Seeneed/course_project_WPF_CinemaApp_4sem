using System.Windows.Controls;
using CinemaMOON.Data;
using CinemaMOON.Models;
using CinemaMOON.ViewModels;

namespace CinemaMOON.Views
{
    public partial class PosterPage : Page
	{
		public PosterPage(AppDbContext dbContext, User? currentUser)
		{
			InitializeComponent();
			this.DataContext = new PosterPageViewModel(dbContext, currentUser);
		}
	}
}
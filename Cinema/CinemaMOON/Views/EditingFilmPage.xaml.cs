using System.Windows.Controls;
using CinemaMOON.ViewModels;
using CinemaMOON.Models;
using CinemaMOON.Data; 

namespace CinemaMOON.Views 
{
    public partial class EditingFilmPage : Page
	{
		public EditingFilmPage(AppDbContext dbContext, Movie movieToEdit)
		{
			InitializeComponent();
			this.DataContext = new EditingFilmPageViewModel(dbContext, movieToEdit);
		}
	}
}
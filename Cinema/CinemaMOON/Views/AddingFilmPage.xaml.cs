using CinemaMOON.Data;
using CinemaMOON.ViewModels;
using System.Windows.Controls;

namespace CinemaMOON.Views
{
    public partial class AddingFilmPage : Page
	{
		public AddingFilmPage(AppDbContext dbContext)
		{
			InitializeComponent();
			DataContext = new AddingFilmPageViewModel(dbContext);
		}
	}
}
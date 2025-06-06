using CinemaMOON.Data;
using CinemaMOON.ViewModels;
using System.Windows.Controls;

namespace CinemaMOON.Views
{
	public partial class AllSchedulesPage : Page
	{
		public AllSchedulesPage(AppDbContext dbContext)
		{
			InitializeComponent();
			DataContext = new AllSchedulesViewModel(dbContext);
		}
	}
}
using CinemaMOON.Data;
using CinemaMOON.ViewModels;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace CinemaMOON.Views
{
    public partial class AdminPanelPage : Page
	{
		public AdminPanelPage(AppDbContext dbContext)
		{
			InitializeComponent();
			DataContext = new AdminPanelViewModel(dbContext);
		}
	}
}
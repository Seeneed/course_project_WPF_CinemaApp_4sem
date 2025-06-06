using System.Windows.Controls;
using CinemaMOON.Data;
using CinemaMOON.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaMOON.Views
{
    public partial class HallPage : Page
	{
		public HallPage(AppDbContext dbContext)
		{
			InitializeComponent();
			DataContext = new HallPageViewModel(dbContext);
		}
	}
}
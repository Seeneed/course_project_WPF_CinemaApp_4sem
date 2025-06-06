using CinemaMOON.Data;
using CinemaMOON.Models;
using CinemaMOON.ViewModels;
using System.Windows.Controls;

namespace CinemaMOON.Views
{
    public partial class ChangeLoginPage : Page
	{
		public ChangeLoginPage(AppDbContext dbContext, User currentUser)
		{
			InitializeComponent();
			this.DataContext = new ChangeLoginPageViewModel(dbContext, currentUser);
		}
	}
}
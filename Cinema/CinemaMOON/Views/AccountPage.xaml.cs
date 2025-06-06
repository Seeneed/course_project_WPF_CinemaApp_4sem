using CinemaMOON.Data;
using CinemaMOON.Models;
using CinemaMOON.ViewModels;
using System.Windows.Controls;

namespace CinemaMOON.Views
{
    public partial class AccountPage : Page
	{
		public AccountPage(AppDbContext dbContext, User loggedInUser)
		{
			InitializeComponent();
			this.DataContext = new AccountPageViewModel(dbContext, loggedInUser);
		}
	}
}
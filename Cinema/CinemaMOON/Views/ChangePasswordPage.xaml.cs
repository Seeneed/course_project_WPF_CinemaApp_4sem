using CinemaMOON.Data;
using CinemaMOON.Models;
using CinemaMOON.ViewModels;
using System.Windows.Controls;

namespace CinemaMOON.Views
{
    public partial class ChangePasswordPage : Page
	{
		public ChangePasswordPage(AppDbContext dbContext, User currentUser)
		{
			InitializeComponent();
			this.DataContext = new ChangePasswordPageViewModel(dbContext, currentUser);
		}
	}
}
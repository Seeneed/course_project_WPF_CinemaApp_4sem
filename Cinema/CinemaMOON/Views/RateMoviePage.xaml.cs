using CinemaMOON.Data;
using CinemaMOON.Models;
using CinemaMOON.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CinemaMOON.Views
{
    public partial class RateMoviePage : Page
	{
		public RateMoviePage(AppDbContext dbContext, Order orderToRate, User currentUser)
		{
			InitializeComponent();
			this.DataContext = new RateMovieViewModel(dbContext, orderToRate, currentUser);
		}
	}
}

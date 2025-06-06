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
    public partial class AddSchedulePage : Page
	{
		public AddSchedulePage(AppDbContext dbContext, Movie movie)
		{
			InitializeComponent();
			this.DataContext = new AddSchedulePageViewModel(dbContext, movie);
		}
	}
}

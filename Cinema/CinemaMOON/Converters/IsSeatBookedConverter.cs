using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CinemaMOON.Converters
{
	public class IsSeatBookedConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values == null || values.Length < 2)
			{
				return false;
			}

			var seatTag = values[0] as string;

			var bookedSeatIds = values[1] as HashSet<string>;

			if (string.IsNullOrEmpty(seatTag) || bookedSeatIds == null)
			{
				return false;
			}

			bool isBooked = bookedSeatIds.Contains(seatTag);
			return isBooked;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}

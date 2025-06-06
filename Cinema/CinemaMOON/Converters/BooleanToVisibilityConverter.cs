using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CinemaMOON.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = false;

            if (value is bool)
            {
                boolValue = (bool)value;
            }
            else if (value is int)
            {
                boolValue = (int)value > 0;
            }

            bool inverse = parameter != null && parameter.ToString().Equals("inverse", StringComparison.OrdinalIgnoreCase);

            if (inverse)
            {
                boolValue = !boolValue;
            }

            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
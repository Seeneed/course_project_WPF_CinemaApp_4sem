using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace CinemaMOON.Converters
{
	public class ImagePathConverter : IValueConverter
	{
		private static BitmapImage _placeholder;
		private static readonly object _lock = new object();

		private static BitmapImage Placeholder
		{
			get
			{
				if (_placeholder == null)
				{
					lock (_lock)
					{
						if (_placeholder == null)
						{
							try
							{
								_placeholder = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/placeholder.png"));
								_placeholder.Freeze();
							}
							catch (Exception)
							{
								return null;
							}
						}
					}
				}
				return _placeholder;
			}
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is string imagePath && !string.IsNullOrWhiteSpace(imagePath))
			{
				if (File.Exists(imagePath))
				{
					try
					{
						BitmapImage bitmap = new BitmapImage();
						bitmap.BeginInit();
						bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
						bitmap.CacheOption = BitmapCacheOption.OnLoad;
						bitmap.EndInit();
						bitmap.Freeze();
						return bitmap;
					}
					catch (Exception)
					{
						return Placeholder;
					}
				}
				else
				{
					return Placeholder;
				}
			}

			return Placeholder;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
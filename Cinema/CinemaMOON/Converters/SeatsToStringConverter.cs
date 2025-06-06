using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CinemaMOON.Converters
{
	public class SeatsToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is string seatsString && !string.IsNullOrEmpty(seatsString))
			{
				var rowAbbr = Application.Current.TryFindResource("OrderSeats_RowAbbreviation") as string ?? (culture.Name.StartsWith("ru") ? "Р" : "R");
				var seatAbbr = Application.Current.TryFindResource("OrderSeats_SeatAbbreviation") as string ?? (culture.Name.StartsWith("ru") ? "М" : "S");

				var seatParts = seatsString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				var formattedSeats = new StringBuilder();

				foreach (var part in seatParts.Select(s => s.Trim()))
				{
					if (part.Length > 1 && char.IsLetter(part[0]) && char.IsDigit(part[1]))
					{
						try
						{
							int firstDigitIndex = -1;
							for (int i = 0; i < part.Length; i++)
							{
								if (char.IsDigit(part[i]))
								{
									firstDigitIndex = i;
									break;
								}
							}

							if (firstDigitIndex > 0)
							{
								int seatLetterIndex = -1;
								for (int i = firstDigitIndex; i < part.Length; i++)
								{
									if (char.IsLetter(part[i]))
									{
										seatLetterIndex = i;
										break;
									}
								}

								if (seatLetterIndex > firstDigitIndex && seatLetterIndex < part.Length - 1)
								{
									string rowNum = part.Substring(firstDigitIndex, seatLetterIndex - firstDigitIndex);
									string seatNum = part.Substring(seatLetterIndex + 1);

									if (formattedSeats.Length > 0)
									{
										formattedSeats.Append(", ");
									}
									formattedSeats.AppendFormat("{0}{1} {2}{3}", rowAbbr, rowNum, seatAbbr, seatNum);
								}
								else
								{
									AppendRawPart(formattedSeats, part);
								}
							}
							else
							{
								AppendRawPart(formattedSeats, part);
							}
						}
						catch
						{
							AppendRawPart(formattedSeats, part);
						}
					}
					else
					{
						AppendRawPart(formattedSeats, part);
					}
				}
				return formattedSeats.ToString();
			}
			return value;
		}

		private void AppendRawPart(StringBuilder sb, string part)
		{
			if (sb.Length > 0) sb.Append(", ");
			sb.Append(part);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}

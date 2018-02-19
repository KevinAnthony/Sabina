using System;
using System.Globalization;
using System.Windows.Data;

namespace Noside.Wyvern.CoinCounter.Converters {
	public class SubButtonConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			return value is uint val && val != 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
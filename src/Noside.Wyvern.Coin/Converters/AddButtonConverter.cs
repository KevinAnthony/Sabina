using System;
using System.Globalization;
using System.Windows.Data;

namespace Noside.Wyvern.CoinCounter.Converters {
	public class AddButtonConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
			var cashed = values[0] as uint? ?? 0;
			var toCash = values[1] as uint? ?? 0;
			return cashed < toCash;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}
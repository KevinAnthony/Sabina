#region Using

using System;
using System.Globalization;
using System.Windows.Data;

#endregion

namespace Noside.Wyvern.CoinCounter.Converters
{
    internal class IsRollMaxConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
	        return value is Models.Coin coin && coin.CashedRolls <= coin.RollsToCash;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
#region Using

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Noside.CoinCounter.Models;

#endregion

namespace Noside.CoinCounter.Converters
{
    internal class IsRollMaxConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Coin coin = value as Coin;
            if (coin == null) return false;
            return coin.CashedRolls <= coin.RollsToCash;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Globalization;
using System.Windows.Data;

namespace Noside.Wyvern.Common.Converters
{
    public class DoubleToHexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)(value as double? ?? 0.0)).ToString("X");        
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.TryParse(value as string,NumberStyles.HexNumber, null, out int hex))
            {
                return hex;
            }
            return 0;
        }
    }
}

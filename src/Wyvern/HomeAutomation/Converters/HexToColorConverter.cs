using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Noside.HomeAutomation.Converters
{
    class HexToColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 3)
                return new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
            return new SolidColorBrush(Color.FromArgb(0xFF, System.Convert.ToByte(values[0]), System.Convert.ToByte(values[1]), System.Convert.ToByte(values[2])));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

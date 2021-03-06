﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Noside.Rgb.Converters
{
    class DoubleToHexConverter : IValueConverter
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

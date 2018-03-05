#region Using

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Shapes;
using Microsoft.Practices.ServiceLocation;
using Noside.Wyvern.Weather.Interfaces;

#endregion

namespace Noside.Wyvern.Weather.Converters {
	public class IconStrToPathConverter : IValueConverter {
		#region Fields

		private readonly IWeatherIconLocator _iconLocator;

		#endregion

		#region Constructors and Destructors

		public IconStrToPathConverter() {
			this._iconLocator = ServiceLocator.Current.GetInstance<IWeatherIconLocator>();
		}

		#endregion

		#region Public Methods

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (!(value is string icon)) return value;
			IWeatherIcon iconMappping = this._iconLocator.GetWeatherIcon();
			Path path = iconMappping.GetPath(icon);
			return path;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}

		#endregion
	}
}
#region Using

using System;
using System.Globalization;
using System.Windows.Data;
using Microsoft.Practices.ServiceLocation;

#endregion

namespace Noside.Wyvern.Weather.ViewModels {
	public class IconStrToPathConverter : IValueConverter {
		private IWeatherIconLocator _iconLocator;

		#region Constructors and Destructors

		public IconStrToPathConverter() {
			this._iconLocator = ServiceLocator.Current.GetInstance<IWeatherIconLocator>();
		}
		#endregion

		#region Public Methods

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (!(value is string icon)) return value;
			var iconMappping = this._iconLocator.GetWeatherIcon();
			var path = iconMappping.GetPath(icon);
			return path;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}

		#endregion
	}

	public interface IWeatherIconLocator {
		#region Public Methods

		IWeatherIcon GetWeatherIcon();

		#endregion
	}

	public class WeatherIconLocator : IWeatherIconLocator {
		#region Constructors and Destructors

		public WeatherIconLocator() {
			this.Day = ServiceLocator.Current.GetInstance<Day>();
			this.Night = ServiceLocator.Current.GetInstance<Night>();
		}

		#endregion

		#region Properties

		public IWeatherIcon Day { get; set; }

		public IWeatherIcon Night { get; set; }

		#endregion

		#region Public Methods

		public IWeatherIcon GetWeatherIcon() {
			return ServiceLocator.Current.GetInstance<Day>();
			//return this.Day;
		}

		#endregion
	}
}
using Microsoft.Practices.ServiceLocation;
using Noside.Wyvern.Weather.Interfaces;
using Noside.Wyvern.Weather.Models;

namespace Noside.Wyvern.Weather.Helpers {
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
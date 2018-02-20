using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;

namespace Noside.Wyvern.Weather.ViewModels {
	public class Night : IWeatherIcon {
		#region Fields

		private readonly Dictionary<string, Path> _mapping;

		#endregion

		#region Constructors and Destructors

		public Night() {
			this._mapping = new Dictionary<string, Path> {
				{"chanceflurries", Application.Current.TryFindResource("DaySnow") as Path},
				{"chancerain", Application.Current.TryFindResource("DaySprinkle") as Path},
				{"chancesleet", Application.Current.TryFindResource("DaySleet") as Path},
				{"chancesnow", Application.Current.TryFindResource("DaySnow") as Path},
				{"chancetstorms", Application.Current.TryFindResource("DayThunderstorm") as Path},
				{"clear", Application.Current.TryFindResource("DaySunny") as Path},
				{"cloudy", Application.Current.TryFindResource("DayCloudy") as Path},
				{"flurries", Application.Current.TryFindResource("DaySnow") as Path},
				{"fog", Application.Current.TryFindResource("DayFog") as Path},
				{"hazy", Application.Current.TryFindResource("DayHaze") as Path},
				{"mostlycloudy", Application.Current.TryFindResource("DayCloudy") as Path},
				{"mostlysunny", Application.Current.TryFindResource("DaySunny") as Path},
				{"partlycloudy", Application.Current.TryFindResource("DaySunnyOvercast") as Path},
				{"partlysunny", Application.Current.TryFindResource("DaySunnyOvercast") as Path},
				{"sleet", Application.Current.TryFindResource("DaySleet") as Path},
				{"rain", Application.Current.TryFindResource("DayRain") as Path},
				{"snow", Application.Current.TryFindResource("DaySnow") as Path},
				{"sunny", Application.Current.TryFindResource("DaySunny") as Path},
				{"tstorms", Application.Current.TryFindResource("DayThunderstorm") as Path},
				{"unknown", Application.Current.TryFindResource("Na") as Path}
			};
		}

		#endregion

		#region Properties

		public Path ChanceFlurries => this._mapping["chanceflurries"];

		public Path ChanceRain => this._mapping["chancerain"];

		public Path ChanceSleet => this._mapping["chancesleet"];

		public Path ChanceSnow => this._mapping["chancesnow"];

		public Path ChanceThunderStorms => this._mapping["chancetstorms"];

		public Path Clear => this._mapping["clear"];

		public Path Cloudy => this._mapping["cloudy"];

		public Path Flurries => this._mapping["flurries"];

		public Path Fog => this._mapping["fog"];

		public Path Hazy => this._mapping["hazy"];

		public Path MostlyCouldy => this._mapping["mostlycloudy"];

		public Path MostlySunny => this._mapping["mostlysunny"];

		public Path PartlyCloudy => this._mapping["partlycloudy"];

		public Path PartlySunny => this._mapping["partlysunny"];

		public Path Rain => this._mapping["rain"];

		public Path Sleet => this._mapping["sleet"];

		public Path Snow => this._mapping["snow"];

		public Path Sunny => this._mapping["sunny"];

		public Path ThunderStroms => this._mapping["tstorms"];

		public Path Unknown => this._mapping["unknown"];

		#endregion

		#region Public Methods

		public Path GetPath(string icon) {
			return this._mapping.ContainsKey(icon) ? this._mapping[icon] : this.Unknown;
		}

		#endregion
	}
}
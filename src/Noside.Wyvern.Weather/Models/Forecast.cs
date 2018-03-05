#region Using

using Prism.Mvvm;

#endregion

namespace Noside.Wyvern.Weather.Models {
	public class Forecast : BindableBase {
		#region Fields

		private string _day;

		private double _high;
		private string _icon;
		private double _low;
		private string _weatherText;

		#endregion

		#region Properties

		public string Day {
			get => this._day;
			set => this.SetProperty(ref this._day, value);
		}

		public double High {
			get => this._high;
			set => this.SetProperty(ref this._high, value);
		}

		public string Icon {
			get => this._icon;
			set => this.SetProperty(ref this._icon, value);
		}

		public double Low {
			get => this._low;
			set => this.SetProperty(ref this._low, value);
		}

		public string WeatherText {
			get => this._weatherText;
			set => this.SetProperty(ref this._weatherText, value);
		}

		#endregion
	}
}
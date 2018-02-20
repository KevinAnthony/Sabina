using Prism.Mvvm;

namespace Noside.Wyvern.Weather.ViewModels {
	public class Forecast : BindableBase {
		private string _icon;

		public string Icon {
			get => this._icon;
			set => this.SetProperty(ref this._icon, value);
		}

		private double _high;

		public double High
		{
			get => this._high;
			set => this.SetProperty(ref this._high, value);
		}
		private double _low;
		private string _day;
		private string _weatherText;

		public double Low
		{
			get => this._low;
			set => this.SetProperty(ref this._low, value);
		}

		public string Day
		{
			get => this._day;
			set => this.SetProperty(ref this._day, value);
		}

		public string WeatherText
		{
			get => this._weatherText;
			set => this.SetProperty(ref this._weatherText, value);
		}
	}
}
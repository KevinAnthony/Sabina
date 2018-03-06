#region Using

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CreativeGurus.Weather.Wunderground;
using CreativeGurus.Weather.Wunderground.Models;
using CreativeGurus.Weather.Wunderground.ResultModels;
using Noside.Wyvern.Weather.Interfaces;
using Prism.Mvvm;
using Forecast = Noside.Wyvern.Weather.Models.Forecast;

#endregion

namespace Noside.Wyvern.Weather.ViewModels {
	internal class WeatherViewModel : BindableBase, IWeatherViewModel {
		#region Fields

		private readonly IWeatherClient _client;
		private string _cityName;
		private ObservableCollection<Forecast> _fourDay;
		private double _high;
		private string _icon;
		private double _low;
		private double _tempature;
		private string _weatherText;

		#endregion

		#region Constructors and Destructors

		public WeatherViewModel(IWeatherClient weather) {
			this._client = weather;
			Task.Factory.StartNew(this.Populate);
			this.FourDay = new ObservableCollection<Forecast>();
		}

		#endregion

		#region Properties

		public string CityName {
			get => this._cityName;
			set => this.SetProperty(ref this._cityName, value);
		}

		public ObservableCollection<Forecast> FourDay {
			get => this._fourDay;
			set => this.SetProperty(ref this._fourDay, value);
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

		public double Tempature {
			get => this._tempature;
			set => this.SetProperty(ref this._tempature, value);
		}

		public string WeatherText {
			get => this._weatherText;
			set => this.SetProperty(ref this._weatherText, value);
		}

		#endregion

		#region Methods

		private async Task Populate() {
			ConditionsResponse weatherResponse = await this._client.GetConditionsAsync(QueryType.AutoIp);
			while (weatherResponse?.CurrentObservation == null) {
				Debug.WriteLine(weatherResponse?.Response.Error["description"]);
				await Task.Delay(2500);
				weatherResponse = await this._client.GetConditionsAsync(QueryType.ZipCode, new QueryOptions {ZipCode = "08820"});
			}

			await Application.Current.Dispatcher.BeginInvoke(new Action(() => {
				this.Tempature = weatherResponse.CurrentObservation.TempF;
				this.CityName = weatherResponse.CurrentObservation.DisplayLocation.Full;
				this.Icon = weatherResponse.CurrentObservation.Icon;
				this.WeatherText = weatherResponse.CurrentObservation.Weather;
			}));
			ForecastResponse forcastResponse = await this._client.GetForecast10DayAsync(QueryType.GPS, new QueryOptions {Latitude = weatherResponse.CurrentObservation.DisplayLocation.Latitude, Longitude = weatherResponse.CurrentObservation.DisplayLocation.Longitude});
			while (forcastResponse?.Forecast == null) {
				await Task.Delay(2500);
				forcastResponse = await this._client.GetForecast10DayAsync(QueryType.GPS, new QueryOptions {Latitude = weatherResponse.CurrentObservation.DisplayLocation.Latitude, Longitude = weatherResponse.CurrentObservation.DisplayLocation.Longitude});
			}

			await Application.Current.Dispatcher.BeginInvoke(new Action(() => {
				this.High = forcastResponse.Forecast.SimpleForecast.ForecastDay[0].High.Fahrenheit ?? 0.0;
				this.Low = forcastResponse.Forecast.SimpleForecast.ForecastDay[0].Low.Fahrenheit ?? 0.0;
				foreach (SimpleForecastDay day in forcastResponse.Forecast.SimpleForecast.ForecastDay.Skip(1).Take(4)) {
					Forecast fc = new Forecast {
						Icon = day.Icon,
						High = day.High.Fahrenheit ?? 0.0,
						Low = day.Low.Fahrenheit ?? 0.0,
						Day = day.Date.Weekday,
						WeatherText = day.Conditions
					};
					this.FourDay.Add(fc);
				}
			}));
		}

		#endregion
	}
}
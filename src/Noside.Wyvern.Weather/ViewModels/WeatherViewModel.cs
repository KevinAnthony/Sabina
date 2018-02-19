#region Using

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using CreativeGurus.Weather.Wunderground;
using CreativeGurus.Weather.Wunderground.Models;
using CreativeGurus.Weather.Wunderground.ResultModels;
using Prism.Mvvm;

#endregion

namespace Noside.Wyvern.Weather.ViewModels {
	internal class WeatherViewModel : BindableBase, IWeatherViewModel
	{
		#region Fields

		private string _cityName;
		private readonly IWeatherClient _client;

		private string _conditions;
		private double _tempature;
		private ObservableCollection<Forecast> _fourDay;

		public ObservableCollection<Forecast> FourDay
		{
			get => this._fourDay;
			set => this.SetProperty(ref this._fourDay, value);
		}
		#endregion

		#region Constructors and Destructors

		public WeatherViewModel(IWeatherClient weather) {
			this._client = weather;
			Task.Factory.StartNew(this.Populate);
			this.FourDay = new ObservableCollection<Forecast>();
		}

		private async Task Populate() {
			ConditionsResponse weatherResponse = await this._client.GetConditionsAsync(QueryType.AutoIp);
			while (weatherResponse?.CurrentObservation == null) {
				await Task.Delay(2500);
				weatherResponse = await this._client.GetConditionsAsync(QueryType.AutoIp);
			}
			this.Tempature = weatherResponse.CurrentObservation.TempF;
			this.CityName = weatherResponse.CurrentObservation.DisplayLocation.Full;
			this.Conditions = weatherResponse.CurrentObservation.Weather;
			var forcastResponse = await this._client.GetForecastAsync(QueryType.GPS, new QueryOptions { Latitude = weatherResponse.CurrentObservation.DisplayLocation.Latitude, Longitude = weatherResponse.CurrentObservation.DisplayLocation.Longitude });
			while (forcastResponse?.Forecast == null) {
				await Task.Delay(2500);
				forcastResponse = await this._client.GetForecastAsync(QueryType.GPS, new QueryOptions{Latitude = weatherResponse.CurrentObservation.DisplayLocation.Latitude, Longitude = weatherResponse.CurrentObservation.DisplayLocation.Longitude});
			}
			await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
			{
				foreach (var day in forcastResponse.Forecast.SimpleForecast.ForecastDay)
				{
					var fc = new Forecast();
					fc.Weather = day.Icon;
					fc.Tempature = day.High.Fahrenheit ?? 0.0;
					this.FourDay.Add(fc);
				}
			}));

		}

		#endregion

		#region Properties

		public string CityName {
			get => this._cityName;
			set => this.SetProperty(ref this._cityName, value);
		}

		public string Conditions {
			get => this._conditions;
			set => this.SetProperty(ref this._conditions, value);
		}


		public double Tempature {
			get => this._tempature;
			set => this.SetProperty(ref this._tempature, value);
		}

		#endregion
	}

	public class Forecast : BindableBase {
		private string _weather;

		public string Weather {
			get => this._weather;
			set => this.SetProperty(ref this._weather, value);
		}

		private double _tempature;

		public double Tempature {
			get => this._tempature;
			set => this.SetProperty(ref this._tempature, value);
		}
	}
}
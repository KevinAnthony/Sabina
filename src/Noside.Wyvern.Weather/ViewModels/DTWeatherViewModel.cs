﻿using System;
using System.Collections.ObjectModel;
using Noside.Wyvern.Weather.Interfaces;
using Noside.Wyvern.Weather.Models;
using Prism.Mvvm;

namespace Noside.Wyvern.Weather.ViewModels
{
	class DtWeatherViewModel : BindableBase, IWeatherViewModel
	{

		private string _cityName;
		private string _conditions;
		private double _tempature;
		private ObservableCollection<Forecast> _fourDay;

		public DtWeatherViewModel() {
			this.CityName = "Weather, NS";
			this.Tempature = 32.5;
			this.Icon = "Sunny";
			this.FourDay = new ObservableCollection<Forecast>();
			for (int i = 0; i < 4; i++) {
				this.FourDay[i] = new Forecast {Icon = "Clear", High = 103.9, Low=-201};
			}

		}

		public string CityName
		{
			get => this._cityName;
			set => this.SetProperty(ref this._cityName, value);
		}

		public string Icon
		{
			get => this._conditions;
			set => this.SetProperty(ref this._conditions, value);
		}


		public double Tempature
		{
			get => this._tempature;
			set => this.SetProperty(ref this._tempature, value);
		}

		public ObservableCollection<Forecast> FourDay
		{
			get => this._fourDay;
			set => this.SetProperty(ref this._fourDay, value);
		}
	}
}

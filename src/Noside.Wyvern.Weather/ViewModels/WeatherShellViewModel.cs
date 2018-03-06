using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace Noside.Wyvern.Weather.ViewModels
{
	class WeatherShellViewModel :BindableBase
	{
		private double _windowLeft;
		private double _windowTop;

		public WeatherShellViewModel() {

		}
		public double WindowLeft
		{
			get => this._windowLeft;
			set => this.SetProperty(ref this._windowLeft, value);
		}

		public double WindowTop
		{
			get => this._windowTop;
			set => this.SetProperty(ref this._windowTop, value);
		}

	}
}

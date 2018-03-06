using System.Collections.ObjectModel;
using Noside.Wyvern.Weather.Models;

namespace Noside.Wyvern.Weather.Interfaces
{
	internal interface IWeatherViewModel
	{
		string CityName { get; set; }
		string Icon { get; set; }
		double Tempature { get; set; }
		ObservableCollection<Forecast> FourDay { get; set; }
	}
}
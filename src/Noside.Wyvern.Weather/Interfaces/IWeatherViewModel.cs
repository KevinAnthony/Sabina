using System.Collections.ObjectModel;

namespace Noside.Wyvern.Weather.ViewModels
{
	internal interface IWeatherViewModel
	{
		string CityName { get; set; }
		string Conditions { get; set; }
		double Tempature { get; set; }
		ObservableCollection<Forecast> FourDay { get; set; }
	}
}
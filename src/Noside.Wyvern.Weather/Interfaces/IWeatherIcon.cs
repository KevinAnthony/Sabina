using System.Windows.Shapes;

namespace Noside.Wyvern.Weather.Interfaces
{
	public interface IWeatherIcon
	{
		Path ChanceFlurries { get; }
		Path ChanceRain { get; }
		Path ChanceSleet { get; }
		Path ChanceSnow { get; }
		Path ChanceThunderStorms { get; }
		Path Clear { get; }
		Path Cloudy { get; }
		Path Flurries { get; }
		Path Fog { get; }
		Path Hazy { get; }
		Path MostlyCouldy { get; }
		Path MostlySunny { get; }
		Path PartlyCloudy { get; }
		Path PartlySunny { get; }
		Path Rain { get; }
		Path Sleet { get; }
		Path Snow { get; }
		Path Sunny { get; }
		Path ThunderStroms { get; }
		Path Unknown { get; }

		Path GetPath(string icon);
	}
}
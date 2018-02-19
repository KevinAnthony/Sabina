using Prism.Modularity;
using Prism.Regions;


namespace Noside.Wyvern.Weather.Modules {
	public class WeatherModule : IModule {


		#region Fields

		private readonly IRegionManager _regionManager;

		#endregion

		#region Constructors and Destructors

		public WeatherModule(IRegionManager regionManger) {
			this._regionManager = regionManger;
		}

		#endregion

		#region Public Methods

		public void Initialize() {
			this._regionManager.RegisterViewWithRegion("WeatherWidgetArea", typeof(Views.Weather));
		}

		#endregion
	}

}


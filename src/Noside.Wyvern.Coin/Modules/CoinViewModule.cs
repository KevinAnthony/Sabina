#region Using

using Noside.Wyvern.CoinCounter.Views;
using Prism.Modularity;
using Prism.Regions;

#endregion

namespace Noside.Wyvern.CoinCounter.Modules {
	public class CoinViewModule : IModule {
		#region Fields

		private readonly IRegionManager _regionManager;

		#endregion

		#region Constructors and Destructors

		public CoinViewModule(IRegionManager regionManger) {
			this._regionManager = regionManger;
		}

		#endregion

		#region Public Methods

		public void Initialize() {
			this._regionManager.RegisterViewWithRegion("CoinViewArea", typeof(CoinView));
		}

		#endregion
	}
}
using Noside.Wyvern.CoinCounter.Views;
using Prism.Modularity;
using Prism.Regions;

namespace Noside.Wyvern.CoinCounter.Modules {
	public class RollCoinsModule : IModule
	{
		#region Fields

		private readonly IRegionManager _regionManager;

		#endregion

		#region Constructors and Destructors

		public RollCoinsModule(IRegionManager regionManger)
		{
			this._regionManager = regionManger;
		}

		#endregion

		#region Public Methods

		public void Initialize()
		{
			this._regionManager.RegisterViewWithRegion("RollCoinArea", typeof(CashRolls));
		}

		#endregion
	}
}
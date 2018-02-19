using Noside.Wyvern.CoinCounter.Views;
using Prism.Modularity;
using Prism.Regions;

namespace Noside.Wyvern.CoinCounter.Modules {
	public class AddCoinswModule : IModule
	{
		#region Fields

		private readonly IRegionManager _regionManager;

		#endregion

		#region Constructors and Destructors

		public AddCoinswModule(IRegionManager regionManger)
		{
			this._regionManager = regionManger;
		}

		#endregion

		#region Public Methods

		public void Initialize()
		{
			this._regionManager.RegisterViewWithRegion("AddCoinArea", typeof(AddCoin));
		}

		#endregion
	}
}
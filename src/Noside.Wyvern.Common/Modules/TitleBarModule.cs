using Noside.Wyvern.Common.Views;
using Prism.Modularity;
using Prism.Regions;

namespace Noside.Wyvern.Common.Modules {
	public class TitleBarModule : IModule
	{
		#region Fields

		private readonly IRegionManager _regionManager;

		#endregion

		#region Constructors and Destructors

		public TitleBarModule(IRegionManager regionManger)
		{
			this._regionManager = regionManger;
		}

		#endregion

		#region Public Methods

		public void Initialize()
		{
			this._regionManager.RegisterViewWithRegion("TitleBarArea", typeof(TitleBar));
			this._regionManager.RegisterViewWithRegion("TitleBarArea2", typeof(TitleBar));
		}

		#endregion
	}
}
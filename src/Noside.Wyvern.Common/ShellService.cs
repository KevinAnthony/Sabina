#region Using

using System.Windows;
using Microsoft.Practices.Unity;
using Prism.Regions;

#endregion

namespace Noside.Wyvern {
	internal class ShellService : IShellService {
		#region Fields

		private readonly IRegionManager _regionManager;
		private readonly IUnityContainer _unityContainer;

		#endregion

		#region Constructors and Destructors

		public ShellService(IUnityContainer unityContainer, IRegionManager regionManager) {
			this._unityContainer = unityContainer;
			this._regionManager = regionManager;
		}

		#endregion

		#region Public Methods

		
		public void ShowShell<T>() where T : Window {
			T shell = this._unityContainer.Resolve<T>();
			IRegionManager scopedRegion = this._regionManager.CreateRegionManager();
			RegionManager.SetRegionManager(shell, scopedRegion);
			shell.Show();
		}
		#endregion
	}
}
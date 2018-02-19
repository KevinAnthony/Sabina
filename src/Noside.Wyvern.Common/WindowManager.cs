using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Unity;
using Noside.Wyvern.Common.Interfaces;
using Prism.Regions;

namespace Noside.Wyvern.Common {
	public class WindowManager : IWindowManager {
		#region Fields

		private readonly IUnityContainer _container;
		private readonly IRegionManager _regionManager;
		private readonly IList<Window> _windows = new List<Window>();

		#endregion

		#region Constructors and Destructors

		public WindowManager(IRegionManager regionManager, IUnityContainer container) {
			this._regionManager = regionManager;
			this._container = container;
		}

		#endregion

		#region Public Methods

		public Window CreateShell<T>() where T : Window {
			T shell = this._container.Resolve<T>();
			var irm = this._regionManager.CreateRegionManager();
			RegionManager.SetRegionManager(shell, irm);
			this._windows.Add(shell);
			shell.Closed += this.Shell_Closed;
			shell.StateChanged += this.Shell_StateChanged;
			return shell;
		}

		#endregion

		#region Methods

		private void Shell_Closed(object sender, EventArgs e) {
			foreach (var window in this._windows.Where(w => !w.Equals(sender))) window.Close();
		}

		private void Shell_StateChanged(object sender, EventArgs e) {
			if (!(sender is Window shell)) return;
			foreach (var window in this._windows.Where(w => !w.Equals(shell))) window.WindowState = shell.WindowState;
		}

		#endregion
	}
}
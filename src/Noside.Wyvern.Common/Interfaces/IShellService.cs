using System.Windows;
using Prism.Regions;

namespace Noside.Wyvern {
	internal interface IShellService {
		#region Public Methods
		
		void ShowShell<T>() where T : Window;

		#endregion
	}
}
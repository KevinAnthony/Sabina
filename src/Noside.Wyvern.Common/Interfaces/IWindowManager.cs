#region Using

using System.Windows;

#endregion

namespace Noside.Wyvern.Common.Interfaces {
	public interface IWindowManager {
		#region Public Methods

		Window CreateShell<T>() where T : Window;

		#endregion
	}
}
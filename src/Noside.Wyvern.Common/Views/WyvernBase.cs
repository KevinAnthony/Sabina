#region Using

using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

#endregion

namespace Noside.Wyvern.Common.Views {
	public class WyvernBase : Window {
		#region Constructors and Destructors

		public WyvernBase() {
			this.Style = (Style)this.FindResource("NsdWindowStyle");
			this.Loaded += this.OnWindowLoaded;
			this.MouseDown += this.OnDragMouseDown;
		}

		#endregion

		#region Methods

		private void OnDragMouseDown(object sender, MouseButtonEventArgs e) {
			if (e.LeftButton != MouseButtonState.Pressed) return;
			this.DragMove();
		}

		private void OnWindowLoaded(object sender, RoutedEventArgs e) {
			Window wnd = (Window) sender;
			GlassHelper.ExtendGlassFrame(wnd);
		}

		#endregion
	}
}
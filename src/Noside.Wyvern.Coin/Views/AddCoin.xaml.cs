
using System.Windows;
using System.Windows.Input;
using Noside.Wyvern.Common;

namespace Noside.Wyvern.CoinCounter.Views {
	/// <summary>
	///     Interaction logic for AddCoin.xaml
	/// </summary>
	public partial class AddCoin {
		public AddCoin() {
			this.InitializeComponent();
		}

		private void OnDragMouseDown(object sender, MouseButtonEventArgs e) {
			if (e.LeftButton != MouseButtonState.Pressed) return;
			var rent = this.Parent as FrameworkElement;
			var window = rent as Window;
			while (window == null) {
				rent = rent?.Parent as FrameworkElement;
				window = rent as Window;
			}

			window.DragMove();
		}

		private void OnControlLoaded(object sender, RoutedEventArgs e) {
			var rent = sender as FrameworkElement;
			var window = rent as Window;
			while (window == null)
			{
				rent = rent?.Parent as FrameworkElement;
				window = rent as Window;
				if (rent == null) return;
			}
			GlassHelper.ExtendGlassFrame(window);
		}
	}
}
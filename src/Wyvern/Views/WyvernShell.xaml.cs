#region Using

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Noside.Wyvern.Common;

#endregion

namespace Noside.Wyvern.Views {
	/// <summary>
	///     Interaction logic for WyvernShell.xaml
	/// </summary>
	public partial class WyvernShell {
		#region Fields

		#endregion

		#region Constructors and Destructors

		public WyvernShell() {
			this.InitializeComponent();
		}

		#endregion

		#region Methods
		//TODO Move to VM
		private void OnDragMouseDown(object sender, MouseButtonEventArgs e) {
			if (e.LeftButton != MouseButtonState.Pressed) return;
			this.DragMove();
		}


		private async void OnWindowLoaded(object sender, RoutedEventArgs e) {
			Window wnd = (Window) sender;
			GlassHelper.ExtendGlassFrame(wnd);
		}

		#endregion

//		private void Timer_Elapsed(object sender, ElapsedEventArgs e) {
//			var uri = theme ? new Uri("Noside.Wyvern.Theme;component/Orange.xaml", UriKind.Relative) : new Uri("Noside.Wyvern.Theme;component/Blue.xaml", UriKind.Relative);
//			theme = !theme;
//			this._eventAggregator.GetEvent<ChangeThemeEvent>().Publish(uri);
//		}
	}
}
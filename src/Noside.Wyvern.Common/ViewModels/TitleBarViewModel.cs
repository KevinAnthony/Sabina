#region Using

using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Noside.Wyvern.Common.Interfaces;
using Noside.Wyvern.Common.Properties;
using Prism.Commands;
using Prism.Mvvm;
using MessageBox = Noside.Wyvern.Common.Windows.MessageBox;

#endregion

namespace Noside.Wyvern.Common.ViewModels {
	internal class TitleBarViewModel : BindableBase {
		#region Fields

		private readonly ICoinDatabase _database;

		#endregion

		#region Constructors and Destructors

		public TitleBarViewModel(ICoinDatabase database) {
			this._database = database;
			if (Application.Current.MainWindow != null) Application.Current.MainWindow.Closing += this.OnWindowClosing;
			this.CloseCommand = new DelegateCommand<FrameworkElement>(this.OnClose);
			this.MinimizeCommand = new DelegateCommand<FrameworkElement>(this.OnMinimize);
		}

		#endregion

		#region Properties

		public DelegateCommand<FrameworkElement> CloseCommand { get; set; }

		public DelegateCommand<FrameworkElement> MinimizeCommand { get; set; }
		

		#endregion

		#region Methods

		private void OnClose(FrameworkElement sender) {
			var rent = sender;
			var window = rent as Window;
			while (window == null)
			{
				rent = rent?.Parent as FrameworkElement;
				window = rent as Window;
				if (rent == null) return;
			}

			window.Close();
		}

		private void OnMinimize(FrameworkElement sender) {
			var rent = sender as FrameworkElement;
			var window = rent as Window;
			while (window == null)
			{
				rent = rent?.Parent as FrameworkElement;
				window = rent as Window;
				if (rent == null) return;
			}

			window.WindowState = WindowState.Minimized;
		}

		private void OnWindowClosing(object sender, CancelEventArgs e) {

			if (!this._database.Dirty) return;
			MessageBoxResult result = MessageBox.Show(Resources.Wyvern_MainWindow_OnClosing_Save_Results_, Resources.Generic_Save, MessageBoxButton.YesNoCancel);
			if (result == MessageBoxResult.Cancel) e.Cancel = true;
			if (result == MessageBoxResult.Yes) this._database.Save();
		}

		#endregion
	}
}
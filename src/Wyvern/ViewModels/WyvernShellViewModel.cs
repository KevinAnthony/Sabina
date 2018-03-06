#region Using

using System;
using System.Collections.ObjectModel;
using System.Windows;
using Noside.Wyvern.Theme;
using Prism.Events;
using Prism.Mvvm;

#endregion

namespace Noside.Wyvern.ViewModels {
	internal class WyvernShellViewModel : BindableBase {
		#region Fields

		private double _windowLeft;

		private double _windowTop;

		#endregion

		#region Constructors and Destructors

		public WyvernShellViewModel(IEventAggregator eventAggregator) {

			eventAggregator.GetEvent<ChangeThemeEvent>().Subscribe(this.OnThemeChanged);
		}

		#endregion

		#region Properties

		public double WindowLeft {
			get => this._windowLeft;
			set => this.SetProperty(ref this._windowLeft, value);
		}

		public double WindowTop {
			get => this._windowTop;
			set => this.SetProperty(ref this._windowTop, value);
		}

		#endregion

		#region Methods

		private void OnThemeChanged(Uri uri) {
			Collection<ResourceDictionary> merge = Application.Current.Resources.MergedDictionaries;
			ResourceDictionary blue = Application.LoadComponent(uri) as ResourceDictionary;
			merge.Clear();
			merge.Add(blue);
		}

		#endregion
	}
}
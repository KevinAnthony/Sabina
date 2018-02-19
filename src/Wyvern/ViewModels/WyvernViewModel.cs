#region Using

using System;
using System.Collections.ObjectModel;
using System.Windows;
using Noside.Wyvern.Theme;
using Prism.Events;

#endregion

namespace Noside.Wyvern.ViewModels {
	internal class WyvernViewModel {
		#region Constructors and Destructors

		public WyvernViewModel(IEventAggregator eventAggregator) {
			eventAggregator.GetEvent<ChangeThemeEvent>().Subscribe(this.OnThemeChanged);
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
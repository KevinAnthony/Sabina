using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using Noside.Wyvern.Theme;
using Prism.Events;

namespace Noside.Wyvern.Common.Views
{
	/// <summary>
	/// Interaction logic for TitleBar.xaml
	/// </summary>
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public partial class TitleBar
	{
		public TitleBar(IEventAggregator eventAggregator)
		{
			this.InitializeComponent();
			eventAggregator.GetEvent<ChangeThemeEvent>().Subscribe(this.OnThemeChanged);
		}

		private void OnThemeChanged(Uri uri) {
			this.Dispatcher.BeginInvoke(new Action(() => {
				Collection<ResourceDictionary> merge = this.Resources.MergedDictionaries;
				ResourceDictionary blue = Application.LoadComponent(uri) as ResourceDictionary;
				merge.Clear();
				merge.Add(blue);
			}));

		}

		private void OnMouseDown(object sender, MouseButtonEventArgs e) {
			if (Mouse.LeftButton != MouseButtonState.Pressed) return;
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

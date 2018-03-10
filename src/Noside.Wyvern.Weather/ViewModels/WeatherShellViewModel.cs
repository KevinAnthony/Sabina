#region Using

using System;
using System.Collections.ObjectModel;
using System.Windows;
using Noside.Wyvern.Common;
using Noside.Wyvern.Theme;
using Prism.Events;
using Prism.Mvvm;

#endregion

namespace Noside.Wyvern.Weather.ViewModels {
	internal class WeatherShellViewModel : BindableBase {
		#region Fields

		private const string Id = "WeatherWindow";
		private readonly ISettings _settings;
		private double _windowLeft;
		private double _windowTop;

		#endregion

		#region Constructors and Destructors

		public WeatherShellViewModel(IEventAggregator eventAggregator, ISettings settings) {
			eventAggregator.GetEvent<ChangeThemeEvent>().Subscribe(this.OnThemeChanged);
			this._settings = settings;
			Point location = this._settings.GetWindowLocation(Id);
			if (location.X.Equals(-1) || location.Y.Equals(-1)) {
				location.X = 80;
				location.Y = 80;
			}
			this.WindowLeft = location.X;
			this.WindowTop = location.Y;
			Application.Current.Dispatcher.ShutdownStarted += (s, e) => {
				this._settings.SetWindowLocation(Id, new Point(this.WindowLeft, this.WindowTop));
				this._settings.Save();
			};
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
using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Windows;
using CreativeGurus.Weather.Wunderground;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Noside.Wyvern.CoinCounter.Models;
using Noside.Wyvern.CoinCounter.Modules;
using Noside.Wyvern.Common;
using Noside.Wyvern.Common.Interfaces;
using Noside.Wyvern.Common.ThirdParty;
using Noside.Wyvern.Common.Views;
using Noside.Wyvern.Views;
using Noside.Wyvern.Weather;
using Noside.Wyvern.Weather.Modules;
using Noside.Wyvern.Weather.ViewModels;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Unity;
using Coin = Noside.Wyvern.CoinCounter.Models.Coin;

namespace Noside.Wyvern {
	class Bootstrapper : UnityBootstrapper {
		private IWindowManager _windowManager;

		protected override DependencyObject CreateShell() {
			this._windowManager = ServiceLocator.Current.GetInstance<IWindowManager>();
			var mainshell = this._windowManager.CreateShell<WyvernShell>();

			var tb1 = ServiceLocator.Current.GetInstance<TitleBar>();
			var tb2 = ServiceLocator.Current.GetInstance<TitleBar>();
			if (tb1 == tb2) {
				Debugger.Break();
			}
			return mainshell;
		}

		protected override void InitializeShell() {
			Application.Current.MainWindow = (Window) this.Shell;
			Application.Current.MainWindow?.Show();
			var weathershell = this._windowManager.CreateShell<WeatherShell>();
			weathershell.Show();
		}
		
		protected override void ConfigureModuleCatalog() {
			var catalog = (ModuleCatalog) this.ModuleCatalog;
			catalog.AddModule(typeof(Common.Modules.TitleBarModule));

			catalog.AddModule(typeof(CoinViewModule));
			catalog.AddModule(typeof(AddCoinswModule));
			catalog.AddModule(typeof(RollCoinsModule));
			catalog.AddModule(typeof(WeatherModule));
		}

		protected override void ConfigureContainer() {
			base.ConfigureContainer();
			/* For the Each */
			this.Container.RegisterType<ICoin, Coin>(new PerResolveLifetimeManager());

			/* For the Lifetime*/
			this.Container.RegisterType<ICoinDatabase, CoinGoogleSheetsDatabase>(new ContainerControlledLifetimeManager());
			this.Container.RegisterType<IGoogleApi, GoogleApi>(new ContainerControlledLifetimeManager());
			this.Container.RegisterType<IWindowManager, WindowManager>(new ContainerControlledLifetimeManager());
			this.Container.RegisterType<IWeatherClient, WeatherClient>(new ContainerControlledLifetimeManager(),new InjectionConstructor("5181017c5a062221", true));
			this.Container.RegisterType<IWeatherIconLocator, WeatherIconLocator>(new ContainerControlledLifetimeManager());

			ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(x =>
			{
				var viewName = x.FullName;
				viewName = viewName.Replace(".Views.", ".ViewModels.");
				var viewAssemblyName = x.GetTypeInfo().Assembly.FullName;
				var suffix = viewName.EndsWith("View") ? "Model" : "ViewModel";
				var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}{1}, {2}", viewName, suffix, viewAssemblyName);
				var type = Type.GetType(viewModelName);
				return type;
			});
			ViewModelLocationProvider.SetDefaultViewModelFactory(type => Container.Resolve(type));
		}
	}
}
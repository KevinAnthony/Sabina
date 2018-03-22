#region Using

using System.Windows;
using CreativeGurus.Weather.Wunderground;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Noside.Wyvern.CoinCounter.Models;
using Noside.Wyvern.CoinCounter.Modules;
using Noside.Wyvern.Common;
using Noside.Wyvern.Common.Interfaces;
using Noside.Wyvern.Common.Modules;
using Noside.Wyvern.Common.ThirdParty;
using Noside.Wyvern.Views;
using Noside.Wyvern.Weather.Helpers;
using Noside.Wyvern.Weather.Interfaces;
using Noside.Wyvern.Weather.Modules;
using Prism.Modularity;
using Prism.Unity;

#endregion

namespace Noside.Wyvern {
	internal class Bootstrapper : UnityBootstrapper {
		#region Methods

		protected override void ConfigureContainer() {
			base.ConfigureContainer();
			/* For the Each */
			this.Container.RegisterType<ICoin, Coin>(new PerResolveLifetimeManager());

			/* For the Lifetime*/
			this.Container.RegisterType<ISettings, Settings>(new ContainerControlledLifetimeManager());
			this.Container.RegisterType<IShellService, ShellService>(new ContainerControlledLifetimeManager());

			this.Container.RegisterType<ICoinDatabase, CoinGoogleSheetsDatabase>(new ContainerControlledLifetimeManager());
			this.Container.RegisterType<IGoogleApi, GoogleApi>(new ContainerControlledLifetimeManager());

			this.Container.RegisterType<IWeatherClient, WeatherClient>(new ContainerControlledLifetimeManager(), new InjectionConstructor("5181017c5a062221", true));
			this.Container.RegisterType<IWeatherIconLocator, WeatherIconLocator>(new ContainerControlledLifetimeManager());
		}

		protected override void ConfigureModuleCatalog() {
			ModuleCatalog catalog = (ModuleCatalog) this.ModuleCatalog;
			catalog.AddModule(typeof(TitleBarModule));

			catalog.AddModule(typeof(CoinViewModule));
			catalog.AddModule(typeof(AddCoinswModule));
			catalog.AddModule(typeof(RollCoinsModule));
			catalog.AddModule(typeof(WeatherModule));
		}

		protected override DependencyObject CreateShell() {
			Window mainshell = ServiceLocator.Current.GetInstance<WyvernShell>();
			return mainshell;
		}

		protected override void InitializeShell() {
			Application.Current.MainWindow = (Window) this.Shell;
			Application.Current.MainWindow?.Show();
		}

		#endregion
	}
}
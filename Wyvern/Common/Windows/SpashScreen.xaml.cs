using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Noside.Common.Load;

namespace Noside.Common.Windows
{
	/// <summary>
	/// Interaction logic for SpashScreen.xaml
	/// </summary>
	public partial class SpashScreen : Window {

		private static readonly List<Loadable> RegisteredLoadables = new List<Loadable>();
		List<LoadInfo> allActions = new List<LoadInfo>();

		public SpashScreen()
		{
			this.InitializeComponent();
		}

		public static async Task<SpashScreen> Load() {
			var ss = new SpashScreen();
			ss.FindLoadables();
			ss.Show();

			await ss.ExecLoadables();
			return ss;
		}

		private async Task ExecLoadables() {
			while (!LoadProgress.Value.Equals(LoadProgress.Maximum)) {
				var li = allActions[(int) LoadProgress.Value];
				this.LoadDescription.Text = allActions[(int)LoadProgress.Value].Description;
				await li.LoadAction();
				await Task.Delay(300);
				LoadProgress.Value++;
			}
		}

		private void FindLoadables() {
			foreach (var loadable in RegisteredLoadables) {
				allActions.AddRange(loadable.LoadList);
			}
			LoadProgress.Value = 0;
			LoadProgress.Maximum = allActions.Count;
			LoadDescription.Text = allActions[0].Description;
		}

		public static void Register(Loadable loadable) {
			RegisteredLoadables.Add(loadable);
		}

		private void SpashScreen_OnLoaded(object sender, RoutedEventArgs e) {
			Window wnd = sender as Window;
			GlassHelper.ExtendGlassFrame(wnd);
		}
	}
}

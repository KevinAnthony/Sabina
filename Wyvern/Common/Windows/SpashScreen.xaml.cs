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
        
		public SpashScreen()
		{
			this.InitializeComponent();
            LoadQueue.CountChanged += LoadCountChanged;

        }

		public static async Task<SpashScreen> Load() {
			var ss = new SpashScreen();
			ss.Show();

			await ss.ExecLoadables();
			return ss;
		}

		private async Task ExecLoadables() {
            LoadProgress.Value = 0;
            LoadProgress.Maximum = LoadQueue.Count;
            LoadDescription.Text = LoadQueue.Get(0).Description;
            while (!LoadProgress.Value.Equals(LoadProgress.Maximum)) {
				var li = LoadQueue.Get((int) LoadProgress.Value);
				this.LoadDescription.Text = LoadQueue.Get((int)LoadProgress.Value).Description;
				await li.LoadAction();
				await Task.Delay(300);
				LoadProgress.Value++;
			}
		}

		private void LoadCountChanged(object sender, EventArgs e) {		
			LoadProgress.Maximum = LoadQueue.Count;
		}


		private void SpashScreen_OnLoaded(object sender, RoutedEventArgs e) {
			Window wnd = sender as Window;
			GlassHelper.ExtendGlassFrame(wnd);
		}
	}
}

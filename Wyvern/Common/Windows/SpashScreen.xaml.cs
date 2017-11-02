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
            LoadQueue.CountChanged += this.LoadCountChanged;

        }

		public static async Task<SpashScreen> Load() {
			var ss = new SpashScreen();
			ss.Show();
			await ss.ExecLoadables();
			return ss;
		}

		private async Task ExecLoadables() {
            return;
			this.LoadProgress.Value = 0;
			this.LoadProgress.Maximum = LoadQueue.Count;
			this.LoadDescription.Text = LoadQueue.Get(0).Description;
            while (!this.LoadProgress.Value.Equals(this.LoadProgress.Maximum)) {
				var li = LoadQueue.Get((int) this.LoadProgress.Value);
				this.LoadDescription.Text = LoadQueue.Get((int) this.LoadProgress.Value).Description;
				await li.LoadAction();
	            this.LoadProgress.Value++;
			}
		}

		private void LoadCountChanged(object sender, EventArgs e) {
			this.LoadProgress.Maximum = LoadQueue.Count;
		}


		private void SpashScreen_OnLoaded(object sender, RoutedEventArgs e) {
			Window wnd = sender as Window;
			GlassHelper.ExtendGlassFrame(wnd);
		}
	}
}

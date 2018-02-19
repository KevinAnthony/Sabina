using System.Windows;

namespace Noside.Wyvern.Common.Windows
{
	/// <summary>
	/// Interaction logic for SpashScreen.xaml
	/// </summary>
	public partial class SpashScreen : Window {
        
		public SpashScreen()
		{
			this.InitializeComponent();

        }

		private void SpashScreen_OnLoaded(object sender, RoutedEventArgs e) {
			Window wnd = sender as Window;
			GlassHelper.ExtendGlassFrame(wnd);
		}
	}
}

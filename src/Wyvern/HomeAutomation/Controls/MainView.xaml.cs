using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Noside.HomeAutomation.Model;
using Q42.HueApi;
using Q42.HueApi.ColorConverters.HSB;
using Noside.Common;
using Noside.Common.Ui;
using Noside.HomeAutomation.Types;
using Q42.HueApi.ColorConverters;

namespace Noside.HomeAutomation.Controls
{
	/// <summary>
	/// Interaction logic for MainView.xaml
	/// </summary>
	public partial class MainView : UserControl
	{
	    private bool _isOn;

	    public MainView()
		{
			this.InitializeComponent();
		}

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            if (!(sender is Expander expander)) return;
            foreach (Expander element in this.ExpanderDock.Children.OfType<Expander>())
            {
                if (element.Equals(expander)) continue;
                element.IsExpanded = false;
            }
        }

	    private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
	    {
	        HomeAutomationViewModel model = this.DataContext as HomeAutomationViewModel;
	        ListBox box = sender as ListBox;
	        if (model == null || !(box?.SelectedItem is Light light))
	        {
	            if (!this.ControlColumn.ActualWidth.Equals(0))
	                Animation.AnimationSlide(this.ControlColumn, null, this.ControlColumn.ActualWidth, 0);
	            return;
	        }


	        var type = light.Type.GetEnum<LightTypes>();
	        if (LightTypes.RGB.HasFlag(type))
	        {
	            model.IsRGB = true;
		        model.SelectedColor = light.State.ToRgb().GetColor();
	        }
	        if (LightTypes.Dimmable.HasFlag(type))
	        {
	            model.IsDimmable = true;
		        model.Brightness = light.State.Brightness;
	        }
            model.IsOn = light.State?.On ?? false;
            
	        if (this.ControlColumn.ActualWidth.Equals(0))
	            Animation.AnimationSlide(this.ControlColumn, null, 0, 300);

	    }

	}
}

#region Using

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Q42.HueApi;
using Q42.HueApi.ColorConverters.HSB;

#endregion

namespace Noside.Wyvern.HomeAutomation.Controls
{
    /// <summary>
    ///     Interaction logic for CoinView.xaml
    /// </summary>
    public partial class MainView
    {

        #region Constructors and Destructors

        public MainView()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

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
            Model.HomeAutomationViewModel model = this.DataContext as Model.HomeAutomationViewModel;
            ListBox box = sender as ListBox;
            if (model == null || !(box?.SelectedItem is Light light))
            {
                if (!this.ControlColumn.ActualWidth.Equals(0))
                    Animation.AnimationSlide(this.ControlColumn, null, this.ControlColumn.ActualWidth, 0);
                return;
            }


            Types.LightTypes type = Common.Extensions.GetEnum<Types.LightTypes>(light.Type);
            if (Types.LightTypes.RGB.HasFlag(type))
            {
                model.IsRGB = true;
                model.SelectedColor = light.State.ToRgb().GetColor();
            }
            if (Types.LightTypes.Dimmable.HasFlag(type))
            {
                model.IsDimmable = true;
                model.Brightness = light.State.Brightness;
            }
            model.IsOn = light.State?.On ?? false;

            if (this.ControlColumn.ActualWidth.Equals(0))
                Animation.AnimationSlide(this.ControlColumn, null, 0, 300);
        }

        #endregion
    }
}
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Noside.Wyvern.HomeAutomation.Controls
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl, INotifyPropertyChanged
    {
        public ColorPicker()
        {
            this.InitializeComponent();
			this.DataContextChanged += this.ColorPicker_DataContextChanged;
        }

		private void ColorPicker_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (!(this.DataContext is Model.HomeAutomationViewModel ha)) return;
		    ha.PropertyChanged += this.HomeAutomation_PropertyChanged;
		}

		private void HomeAutomation_PropertyChanged(object sender, PropertyChangedEventArgs e) {
			if (e.PropertyName == nameof(Model.HomeAutomationViewModel.SelectedColor)) {
				if (!(this.DataContext is Model.HomeAutomationViewModel ha)) return;
				this.R.ColorValue = ha.SelectedColor.R;
				this.G.ColorValue = ha.SelectedColor.G;
				this.B.ColorValue = ha.SelectedColor.B;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

        [Properties.NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

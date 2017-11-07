using Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Noside.Rgb.Model;

namespace Noside.Rgb.Controls
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl, INotifyPropertyChanged
    {
        public ColorPicker()
        {
            this.InitializeComponent();
			this.DataContextChanged += ColorPicker_DataContextChanged;
        }

		private void ColorPicker_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (!(this.DataContext is RgbViewModel rgb)) return;
			rgb.PropertyChanged += this.Rgb_PropertyChanged;
		}

		private void Rgb_PropertyChanged(object sender, PropertyChangedEventArgs e) {
			if (e.PropertyName == nameof(RgbViewModel.SelectedColor)) {
				if (!(this.DataContext is RgbViewModel rgb)) return;
				this.R.ColorValue = rgb.SelectedColor.R;
				this.G.ColorValue = rgb.SelectedColor.G;
				this.B.ColorValue = rgb.SelectedColor.B;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

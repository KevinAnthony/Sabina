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

namespace NoSide.Rgb.Controls
{
    /// <summary>
    /// Interaction logic for ColorSlider.xaml
    /// </summary>
    public partial class ColorSlider : UserControl, INotifyPropertyChanged
    {
        public ColorSlider()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ColorValueProperty = DependencyProperty.Register("ColorValue",
           typeof(int), typeof(ColorSlider));


        public int ColorOctate
        {
            get => (int)this.GetValue(ColorValueProperty);
            set
            {
                this.SetValue(ColorValueProperty, value);
                this.OnPropertyChanged();
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

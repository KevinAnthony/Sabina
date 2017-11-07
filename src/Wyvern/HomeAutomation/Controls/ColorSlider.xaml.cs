using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Noside.Properties;

namespace Noside.HomeAutomation.Controls
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


        public int ColorValue
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

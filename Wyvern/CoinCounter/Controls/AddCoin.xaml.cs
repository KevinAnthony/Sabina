#region Using

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Noside.CoinCounter.Models;

#endregion

namespace Noside.CoinCounter.Controls
{
    /// <summary>
    ///     Interaction logic for AddCoin.xaml
    /// </summary>
    public partial class AddCoin : INotifyPropertyChanged
    {
        private int _dimeValue;
        private int _dollarValue;
        private int _nickelValue;
        private int _pennyValue;
        private int _quarterValue;
        private object _viewModel;

        public void Reset()
        {
            DollarValue = 0;
            QuarterValue = 0;
            DimeValue = 0;
            NickelValue = 0;
            PennyValue = 0;
        }

        #region Constructors and Destructors

        public AddCoin()
        {
            InitializeComponent();
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            var control = Parent as FrameworkElement;
            var model = control?.DataContext as CoinViewModel;
            if (model == null) return;
            Dollar = model.CoinList[0];
            Quarter = model.CoinList[1];
            Dime = model.CoinList[2];
            Nickel = model.CoinList[3];
            Penny = model.CoinList[4];
        }

        #endregion

        #region Properties

        public Coin Dime { get; set; }

        public Coin Dollar { get; set; }

        public Coin Nickel { get; set; }

        public Coin Penny { get; set; }

        public Coin Quarter { get; set; }

        public int DimeValue
        {
            get { return _dimeValue; }
            set
            {
                Dime.Count = (uint) (Dime.Count + (value - _dimeValue));
                _dimeValue = value;
                this.OnPropertyChanged();
            }
        }

        public int DollarValue
        {
            get { return _dollarValue; }
            set
            {
                Dollar.Count = (uint) (Dollar.Count + (value - _dollarValue));
                _dollarValue = value;
                this.OnPropertyChanged();
            }
        }

        public int NickelValue
        {
            get { return _nickelValue; }
            set
            {
                Nickel.Count = (uint) (Nickel.Count + (value - _nickelValue));
                _nickelValue = value;
                this.OnPropertyChanged();
            }
        }

        public int PennyValue
        {
            get { return _pennyValue; }
            set
            {
                Penny.Count = (uint) (Penny.Count + (value - _pennyValue));
                _pennyValue = value;
                this.OnPropertyChanged();
            }
        }

        public int QuarterValue
        {
            get { return _quarterValue; }
            set
            {
                Quarter.Count = (uint) (Quarter.Count + (value - _quarterValue));
                _quarterValue = value;
                this.OnPropertyChanged();
            }
        }

        #endregion

        #region Methods

        private static bool IsTextAllowed(string text)
        {
            var regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        private void Close()
        {
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnGotFocus(object sender, EventArgs e)
        {
            var tb = sender as TextBox;
            tb?.SelectAll();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element == null) return;
            element.Focus();
            Keyboard.Focus(element);
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void TextBox_TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof (string)))
            {
                var text = (string) e.DataObject.GetData(typeof (string));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
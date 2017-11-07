#region Using

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Noside.CoinCounter.Models;

#endregion

namespace Noside.CoinCounter.Controls.Component
{
    /// <summary>
    ///     Interaction logic for AddBox.xaml
    /// </summary>
    public partial class AddBox : UserControl
    {
        public static readonly DependencyProperty WorkingCoinProperty = DependencyProperty.Register("WorkingCoin",
            typeof(Coin), typeof(AddBox));

        private uint _originalCount;

        public AddBox(Coin coin)
        {
	        this.InitializeComponent();
	        this.WorkingCoin = coin;
	        this._originalCount = this.WorkingCoin.Count;
	        this.WorkingCoin.PropertyChanged += this.WorkingCoinOnPropertyChanged;
        }

        public Coin WorkingCoin
        {
            get => (Coin) this.GetValue(WorkingCoinProperty);
	        set => this.SetValue(WorkingCoinProperty, value);
        }

        private void WorkingCoinOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName.Equals(nameof(this.WorkingCoin.Count)))
            {
	            this._originalCount = this.WorkingCoin.Count;
            }
	        if (args.PropertyName.Equals(nameof(this.WorkingCoin.Dirty)) && !this.WorkingCoin.Dirty)
	        {
		        this._originalCount = this.WorkingCoin.Count;
		        this.TextInput.Text = "0";
	        }
		}

        private void OnGotFocus(object sender, EventArgs e)
        {
            var tb = sender as TextBox;
            tb?.SelectAll();
        }

        private void TextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void TextBox_TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var text = (string) e.DataObject.GetData(typeof(string));
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

        private static bool IsTextAllowed(string text)
        {
            var regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            var raw = tb?.Text;
            if (this.WorkingCoin == null) return;
            if (string.IsNullOrWhiteSpace(raw))
            {
                if (tb != null) tb.Text = "0";
	            this.WorkingCoin.Count = this._originalCount;
            }
	        if (!uint.TryParse(raw, out uint count)) return;
	        this.WorkingCoin.Count = this._originalCount + count;
        }
    }
}
#region Using

using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Noside.Wyvern.Common.Interfaces;

#endregion

namespace Noside.Wyvern.CoinCounter.Controls
{
    /// <summary>
    ///     Interaction logic for CoinBox.xaml
    /// </summary>
    public partial class CoinBox : UserControl
    {


        #region Constructors and Destructors

	    public CoinBox() {
		    this.InitializeComponent();
	    }

	    #endregion

		#region Methods

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        private void OnGotFocus(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            tb?.SelectAll();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;
            element.Focus();
            Keyboard.Focus(element);
        }

        private void OnRollCoinsClicked(object sender, RoutedEventArgs e) {
	        var coin =this.DataContext as ICoin;
	        coin?.Roll();
        }

        private void TextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void TextBox_TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string) e.DataObject.GetData(typeof(string));
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
    }
}
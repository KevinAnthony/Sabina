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
    ///     Interaction logic for RollBox.xaml
    /// </summary>
    public partial class RollBox : UserControl
    {
        public RollBox()
        {
	        this.InitializeComponent();
        }

        /// <summary>
        ///     Determines whether [is text allowed] [the specified text].
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns><c>true</c> if [is text allowed] [the specified text]; otherwise, <c>false</c>.</returns>
        private static bool IsTextAllowed(string text)
        {
            var regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        /// <summary>
        ///     Handles the <see cref="E:GotFocus" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnGotFocus(object sender, EventArgs e)
        {
            var tb = sender as TextBox;
            tb?.SelectAll();
        }

        /// <summary>
        ///     Handles the OnPreviewTextInput event of the TextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextCompositionEventArgs" /> instance containing the event data.</param>
        private void TextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        /// <summary>
        ///     Handles the TextBoxPasting event of the TextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataObjectPastingEventArgs" /> instance containing the event data.</param>
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

        private void OnAddClick(object sender, RoutedEventArgs e)
        {
	        if (this.DataContext is ICoin coin)
		        coin.CashedRolls++;
        }

        private void OnSubClick(object sender, RoutedEventArgs e)
        {
			if (this.DataContext is ICoin coin)
				coin.CashedRolls--;
		}

		private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) {
			if (this.DataContext is ICoin coin)
				coin.CashedRolls = 0;
		}
	}
}
#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Noside.CoinCounter.Models;

#endregion

namespace Noside.CoinCounter.Controls
{
    /// <summary>
    ///     Interaction logic for RollBox.xaml
    /// </summary>
    public partial class RollBox : UserControl
    {
        /// <summary>
        ///     Dependency property registration for the wrapper <see cref="WorkingCoin" /> property.
        /// </summary>
        public static readonly DependencyProperty WorkingCoinProperty = DependencyProperty.Register("WorkingCoin",
            typeof (Coin), typeof (RollBox));

        public RollBox(Coin rollCoin)
        {
            InitializeComponent();
            this.WorkingCoin = rollCoin;
            this.AddButton.IsEnabled = false;
            this.SubButton.IsEnabled = WorkingCoin.CashedRolls != 0;
            this.WorkingCoin.PropertyChanged += WorkingCoinOnPropertyChanged;
        }

        private void WorkingCoinOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (!args.PropertyName.Equals(nameof(Coin.CashedRolls))) return;
            this.AddButton.IsEnabled = this.WorkingCoin.CashedRolls < this.WorkingCoin.RollsToCash;
            this.SubButton.IsEnabled = this.WorkingCoin.CashedRolls != 0;
        }

        public Coin WorkingCoin
        {
            get { return (Coin) GetValue(WorkingCoinProperty); }
            set { SetValue(WorkingCoinProperty, value); }
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

        private void OnAdddClick(object sender, RoutedEventArgs e)
        {
            WorkingCoin.CashedRolls++;
        }

        private void OnSubClick(object sender, RoutedEventArgs e)
        {
            WorkingCoin.CashedRolls--;
        }
    }
}
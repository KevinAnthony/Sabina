// ***********************************************************************
// Assembly         : Sabina
// Author           : Kevin Anthony
// Created          : 06-01-2017
//
// Last Modified By : Kevin Anthony
// Last Modified On : 05-31-2017
// ***********************************************************************
// <copyright file="Roll.xaml.cs" company="Activu Corporation">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

#region Using

#region Using

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Coin_Counter.Annotations;

#endregion

// ReSharper disable ExplicitCallerInfoArgument

#endregion

namespace NoSideDynamics.CoinCounter.Windows
{
    /// <summary>
    ///     Interaction logic for Roll.xaml
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public partial class Roll : INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        ///     The _dime cashed
        /// </summary>
        private uint _dimeCashed;

        /// <summary>
        ///     The _dime rolls
        /// </summary>
        private uint _dimeRolls;

        /// <summary>
        ///     The _dollar cashed
        /// </summary>
        private uint _dollarCashed;

        /// <summary>
        ///     The _dollar rolls
        /// </summary>
        private uint _dollarRolls;

        /// <summary>
        ///     The _nickel cashed
        /// </summary>
        private uint _nickelCashed;

        /// <summary>
        ///     The _nickel rolls
        /// </summary>
        private uint _nickelRolls;

        /// <summary>
        ///     The _penny cashed
        /// </summary>
        private uint _pennyCashed;

        /// <summary>
        ///     The _penny rolls
        /// </summary>
        private uint _pennyRolls;

        /// <summary>
        ///     The _quarter cashed
        /// </summary>
        private uint _quarterCashed;

        /// <summary>
        ///     The _quarter rolls
        /// </summary>
        private uint _quarterRolls;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Roll" /> class.
        /// </summary>
        /// <param name="dollars">The dollars.</param>
        /// <param name="quarters">The quarters.</param>
        /// <param name="dimes">The dimes.</param>
        /// <param name="nickels">The nickels.</param>
        /// <param name="pennies">The pennies.</param>
        public Roll(uint dollars, uint quarters, uint dimes, uint nickels, uint pennies)
        {
            this.InitializeComponent();
            this.DollarCashed = this.DollarRolls = dollars;
            this.QuarterCashed = this.QuarterRolls = quarters;
            this.DimeCashed = this.DimeRolls = dimes;
            this.NickelCashed = this.NickelRolls = nickels;
            this.PennyCashed = this.PennyRolls = pennies;
        }

        #endregion

        #region Events

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets a value indicating whether [dime at maximum].
        /// </summary>
        /// <value><c>true</c> if [dime at maximum]; otherwise, <c>false</c>.</value>
        public bool DimeAtMax => this.DimeCashed < this.DimeRolls;

        /// <summary>
        ///     Gets a value indicating whether [dime at minimum].
        /// </summary>
        /// <value><c>true</c> if [dime at minimum]; otherwise, <c>false</c>.</value>
        public bool DimeAtMin => this.DimeCashed > 0;

        /// <summary>
        ///     Gets or sets the dime cashed.
        /// </summary>
        /// <value>The dime cashed.</value>
        public uint DimeCashed {
            get { return this._dimeCashed; }
            set {
                if (value == this._dimeCashed) return;
                this._dimeCashed = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.DimeAtMin));
                this.OnPropertyChanged(nameof(this.DimeAtMax));
            }
        }

        /// <summary>
        ///     Gets or sets the dime rolls.
        /// </summary>
        /// <value>The dime rolls.</value>
        public uint DimeRolls {
            get { return this._dimeRolls; }
            set {
                if (value == this._dimeRolls) return;
                this._dimeRolls = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets a value indicating whether [dollar at maximum].
        /// </summary>
        /// <value><c>true</c> if [dollar at maximum]; otherwise, <c>false</c>.</value>
        public bool DollarAtMax => this.DollarCashed < this.DollarRolls;

        /// <summary>
        ///     Gets a value indicating whether [dollar at minimum].
        /// </summary>
        /// <value><c>true</c> if [dollar at minimum]; otherwise, <c>false</c>.</value>
        public bool DollarAtMin => this.DollarCashed > 0;

        /// <summary>
        ///     Gets or sets the dollar cashed.
        /// </summary>
        /// <value>The dollar cashed.</value>
        public uint DollarCashed {
            get { return this._dollarCashed; }
            set {
                if (value == this._dollarCashed) return;
                this._dollarCashed = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.DollarAtMin));
                this.OnPropertyChanged(nameof(this.DollarAtMax));
            }
        }

        /// <summary>
        ///     Gets or sets the dollar rolls.
        /// </summary>
        /// <value>The dollar rolls.</value>
        public uint DollarRolls {
            get { return this._dollarRolls; }
            set {
                if (value == this._dollarRolls) return;
                this._dollarRolls = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets a value indicating whether [nickel at maximum].
        /// </summary>
        /// <value><c>true</c> if [nickel at maximum]; otherwise, <c>false</c>.</value>
        public bool NickelAtMax => this.NickelCashed < this.NickelRolls;

        /// <summary>
        ///     Gets a value indicating whether [nickel at minimum].
        /// </summary>
        /// <value><c>true</c> if [nickel at minimum]; otherwise, <c>false</c>.</value>
        public bool NickelAtMin => this.NickelCashed > 0;

        /// <summary>
        ///     Gets or sets the nickel cashed.
        /// </summary>
        /// <value>The nickel cashed.</value>
        public uint NickelCashed {
            get { return this._nickelCashed; }
            set {
                if (value == this._nickelCashed) return;
                this._nickelCashed = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.NickelAtMin));
                this.OnPropertyChanged(nameof(this.NickelAtMax));
            }
        }

        /// <summary>
        ///     Gets or sets the nickel rolls.
        /// </summary>
        /// <value>The nickel rolls.</value>
        public uint NickelRolls {
            get { return this._nickelRolls; }
            set {
                if (value == this._nickelRolls) return;
                this._nickelRolls = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets a value indicating whether [penny at maximum].
        /// </summary>
        /// <value><c>true</c> if [penny at maximum]; otherwise, <c>false</c>.</value>
        public bool PennyAtMax => this.PennyCashed < this.PennyRolls;

        /// <summary>
        ///     Gets a value indicating whether [penny at minimum].
        /// </summary>
        /// <value><c>true</c> if [penny at minimum]; otherwise, <c>false</c>.</value>
        public bool PennyAtMin => this.PennyCashed > 0;

        /// <summary>
        ///     Gets or sets the penny cashed.
        /// </summary>
        /// <value>The penny cashed.</value>
        public uint PennyCashed {
            get { return this._pennyCashed; }
            set {
                if (value == this._pennyCashed) return;
                this._pennyCashed = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.PennyAtMin));
                this.OnPropertyChanged(nameof(this.PennyAtMax));
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether [penny maxed].
        /// </summary>
        /// <value><c>true</c> if [penny maxed]; otherwise, <c>false</c>.</value>
        public bool PennyMaxed { get; set; }

        /// <summary>
        ///     Gets or sets the penny rolls.
        /// </summary>
        /// <value>The penny rolls.</value>
        public uint PennyRolls {
            get { return this._pennyRolls; }
            set {
                if (value == this._pennyRolls) return;
                this._pennyRolls = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets a value indicating whether [quarter at maximum].
        /// </summary>
        /// <value><c>true</c> if [quarter at maximum]; otherwise, <c>false</c>.</value>
        public bool QuarterAtMax => this.QuarterCashed < this.QuarterRolls;

        /// <summary>
        ///     Gets a value indicating whether [quarter at minimum].
        /// </summary>
        /// <value><c>true</c> if [quarter at minimum]; otherwise, <c>false</c>.</value>
        public bool QuarterAtMin => this.QuarterCashed > 0;

        /// <summary>
        ///     Gets or sets the quarter cashed.
        /// </summary>
        /// <value>The quarter cashed.</value>
        public uint QuarterCashed {
            get { return this._quarterCashed; }
            set {
                if (value == this._quarterCashed) return;
                this._quarterCashed = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.QuarterAtMin));
                this.OnPropertyChanged(nameof(this.QuarterAtMax));
            }
        }

        /// <summary>
        ///     Gets or sets the quarter rolls.
        /// </summary>
        /// <value>The quarter rolls.</value>
        public uint QuarterRolls {
            get { return this._quarterRolls; }
            set {
                if (value == this._quarterRolls) return;
                this._quarterRolls = value;
                this.OnPropertyChanged();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     Determines whether [is text allowed] [the specified text].
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns><c>true</c> if [is text allowed] [the specified text]; otherwise, <c>false</c>.</returns>
        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
        ///     Handles the <see cref="E:AddDime" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnAddDime(object sender, RoutedEventArgs e)
        {
            this.DimeCashed = Math.Min(this.DimeCashed + 1, this.DimeRolls);
        }

        /// <summary>
        ///     Handles the <see cref="E:AddDollar" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnAddDollar(object sender, RoutedEventArgs e)
        {
            this.DollarCashed = Math.Min(this.DollarCashed + 1, this.DollarRolls);
        }

        /// <summary>
        ///     Handles the <see cref="E:AddNickel" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnAddNickel(object sender, RoutedEventArgs e)
        {
            this.NickelCashed = Math.Min(this.NickelCashed + 1, this.NickelRolls);
        }

        /// <summary>
        ///     Handles the <see cref="E:AddPenny" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnAddPenny(object sender, RoutedEventArgs e)
        {
            this.PennyCashed = Math.Min(this.PennyCashed + 1, this.PennyRolls);
        }

        /// <summary>
        ///     Handles the <see cref="E:AddQuarter" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnAddQuarter(object sender, RoutedEventArgs e)
        {
            this.QuarterCashed = Math.Min(this.QuarterCashed + 1, this.QuarterRolls);
        }

        /// <summary>
        ///     Handles the <see cref="E:Cancel" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnCancel(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        /// <summary>
        ///     Handles the <see cref="E:DragMouseDown" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs" /> instance containing the event data.</param>
        private void OnDragMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;
            this.DragMove();
        }

        /// <summary>
        ///     Handles the <see cref="E:GotFocus" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnGotFocus(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            tb?.SelectAll();
        }

        /// <summary>
        ///     Handles the <see cref="E:Save" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnSave(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        /// <summary>
        ///     Handles the <see cref="E:SubtractDime" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnSubtractDime(object sender, RoutedEventArgs e)
        {
            this.DimeCashed = Math.Max(this.DimeCashed - 1, 0);
        }

        /// <summary>
        ///     Handles the <see cref="E:SubtractDollar" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnSubtractDollar(object sender, RoutedEventArgs e)
        {
            this.DollarCashed = Math.Max(this.DollarCashed - 1, 0);
        }

        /// <summary>
        ///     Handles the <see cref="E:SubtractNickel" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnSubtractNickel(object sender, RoutedEventArgs e)
        {
            this.NickelCashed = Math.Max(this.NickelCashed - 1, 0);
        }

        /// <summary>
        ///     Handles the <see cref="E:SubtractPenny" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnSubtractPenny(object sender, RoutedEventArgs e)
        {
            this.PennyCashed = Math.Max(this.PennyCashed - 1, 0);
        }

        /// <summary>
        ///     Handles the <see cref="E:SubtractQuarter" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnSubtractQuarter(object sender, RoutedEventArgs e)
        {
            this.QuarterCashed = Math.Max(this.QuarterCashed - 1, 0);
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
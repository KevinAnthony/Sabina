#region Using

#region Using

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Coin_Counter.Annotations;
using Noside.CoinCounter.Models;

#endregion

// ReSharper disable ExplicitCallerInfoArgument

#endregion

namespace Noside.CoinCounter.Controls
{
    /// <summary>
    ///     Interaction logic for CashRolls.xaml
    /// </summary>
    /// <seealso cref="System.Windows.Controls.UserControl" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class CashRolls
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="CashRolls" /> class.
        /// </summary>
        public CashRolls()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            var control = Parent as FrameworkElement;
            var model = control?.DataContext as CoinViewModel;
            if (model == null) return;
            model.LoadDone += Model_LoadDone;
        }

        private void Model_LoadDone(object sender, EventArgs e)
        {
            var model = sender as CoinViewModel;
            if (model == null) return;
            this.TheGrid.Children.Clear();
            for (int index = 0; index < model.CoinList.Count; index++)
            {
                var coin = model.CoinList[index];
                this.TheGrid.RowDefinitions.Add(new RowDefinition());
                var box = new RollBox(coin);
                Grid.SetRow(box, index);
                this.TheGrid.Children.Add(box);
            }
        }

        #endregion

        private void OnCashRollsClick(object sender, RoutedEventArgs e)
        {
            var control = Parent as FrameworkElement;
            var model = control?.DataContext as CoinViewModel;
            if (model == null) return;
            foreach (var coin in model.CoinList)
            {
                coin.CashRolls();
            }
        }
    }
}
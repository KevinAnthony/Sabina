#region Using

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Noside.CoinCounter.Models;
using Noside.Common.Helpers.Ui;

#endregion

namespace Noside.CoinCounter.Controls
{
    /// <summary>
    ///     Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        #region Constructors and Destructors

        public MainView()
        {
            InitializeComponent();
            foreach (
                var box in
                    ((CoinViewModel) DataContext).CoinList.Select(
                        coin => new CoinBox {Coin = coin, Margin = new Thickness(3)}))
            {
                Panel.Children.Add(box);
                box.RollCoins += Box_RollCoins;
            }
        }

        #endregion

        #region Properties

        public bool Dirty => ((CoinViewModel) DataContext).Dirty;

        #endregion

        private void OnResetClicked(object sender, RoutedEventArgs e)
        {
            ((CoinViewModel) DataContext).Reset();
            ResetUI();
        }

        #region Methods

        private void Box_RollCoins(object sender, EventArgs args)
        {
            var box = sender as CoinBox;
            var coin = box?.Coin;
            ((CoinViewModel) DataContext).Roll(coin);
        }

        private void OnAddCoins(object sender, RoutedEventArgs e)
        {
            if (AddColumn.ActualWidth.Equals(0))
            {
                AddColumn.AnimateWidth(0, AddCoin.Width + AddCoin.Margin.Left + AddCoin.Margin.Right);
            }
            else
            {
                AddColumn.AnimateWidth(AddColumn.ActualWidth, 0);
            }
//
//            Add addWindow = new Add();
//            if (addWindow.ShowDialog() ?? false)
//            {
//                ((CoinViewModel) this.DataContext).AddCoins((uint) addWindow.DollarValue, (uint) addWindow.QuarterValue,
//                    (uint) addWindow.DimeValue, (uint) addWindow.NickelValue, (uint) addWindow.PennyValue);
//            }
        }

        private void OnSaveClicked(object sender, RoutedEventArgs e)
        {
            ((CoinViewModel) DataContext).Save();
            ResetUI();
        }

        private void ResetUI()
        {
            if (!AddColumn.ActualWidth.Equals(0))
            {
                AddColumn.AnimateWidth(AddColumn.ActualWidth, 0);
            }
            if (!RollColumn.ActualWidth.Equals(0))
            {
                RollColumn.AnimateWidth(RollColumn.ActualWidth, 0);
            }
            AddCoin.Reset();
            RollCoin.Reset();
        }


        private void OnRollCoins(object sender, RoutedEventArgs e)
        {
            if (RollColumn.ActualWidth.Equals(0))
            {
                RollColumn.AnimateWidth(0, RollCoin.Width + RollCoin.Margin.Left + RollCoin.Margin.Right);
            }
            else
            {
                RollColumn.AnimateWidth(RollCoin.ActualWidth, 0);
            }
//            CoinViewModel cvm = (CoinViewModel) this.DataContext;
//            Roll rollWindow = new Roll(cvm.CoinList[0].RollsToCash, cvm.CoinList[1].RollsToCash,
//                cvm.CoinList[2].RollsToCash, cvm.CoinList[3].RollsToCash, cvm.CoinList[4].RollsToCash);
//            if (rollWindow.ShowDialog() ?? false)
//            {
//                cvm.CashRolls(rollWindow.DollarCashed, rollWindow.QuarterCashed, rollWindow.DimeCashed,
//                    rollWindow.NickelCashed, rollWindow.PennyCashed);
//            }
        }

        #endregion
    }
}
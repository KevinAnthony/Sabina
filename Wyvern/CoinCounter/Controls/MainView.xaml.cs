#region Using

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
            var button = sender as Button;
            if (AddColumn.ActualWidth.Equals(0))
            {
                Animation.AnimationSlide(AddColumn,button, 0, AddCoin.Width + AddCoin.Margin.Left + AddCoin.Margin.Right);
            }
            else
            {
                Animation.AnimationSlide(AddColumn, button, AddColumn.ActualWidth, 0);
            }
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
                Animation.AnimationSlide(AddColumn, null, AddColumn.ActualWidth, 0);
            }
            if (!RollColumn.ActualWidth.Equals(0))
            {
                Animation.AnimationSlide(RollColumn, null, AddColumn.ActualWidth, 0);
            }
            AddCoin.Reset();
            RollCoin.Reset();
        }


        private void OnRollCoins(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (RollColumn.ActualWidth.Equals(0))
            {
                Animation.AnimationSlide(RollColumn, button, 0, RollCoin.Width + RollCoin.Margin.Left + RollCoin.Margin.Right);
            }
            else
            {
                Animation.AnimationSlide(RollColumn, button, RollCoin.ActualWidth, 0);
            }
        }

        #endregion
    }
}
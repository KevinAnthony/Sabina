#region Using

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Noside.CoinCounter.Models;
using Noside.Common.Ui;

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
            this.InitializeComponent();
            foreach (
                CoinBox box in
                ((CoinViewModel) this.DataContext).CoinList.Select(
                    coin => new CoinBox {Coin = coin, Margin = new Thickness(3)}))
            {
                this.Panel.Children.Add(box);
                box.RollCoins += this.Box_RollCoins;
            }
        }

        #endregion

        #region Properties

        public bool Dirty => ((CoinViewModel) this.DataContext).Dirty;

        #endregion

        #region Methods

        private void Box_RollCoins(object sender, EventArgs args)
        {
            CoinBox box = sender as CoinBox;
            Coin coin = box?.Coin;
            ((CoinViewModel) this.DataContext).Roll(coin);
        }

        private void OnAddCoins(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (this.AddColumn.ActualWidth.Equals(0))
            {
                Animation.AnimationSlide(this.AddColumn, button, 0, this.AddCoin.Width + this.AddCoin.Margin.Left + this.AddCoin.Margin.Right);
            }
            else
            {
                Animation.AnimationSlide(this.AddColumn, button, this.AddColumn.ActualWidth, 0);
            }
        }

        private void OnResetClicked(object sender, RoutedEventArgs e)
        {
            ((CoinViewModel) this.DataContext).Reset();
            this.ResetUI();
        }


        private void OnRollCoins(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (this.RollColumn.ActualWidth.Equals(0))
            {
                Animation.AnimationSlide(this.RollColumn, button, 0, this.RollCoin.Width + this.RollCoin.Margin.Left + this.RollCoin.Margin.Right);
            }
            else
            {
                Animation.AnimationSlide(this.RollColumn, button, this.RollCoin.ActualWidth, 0);
            }
        }

        private void OnSaveClicked(object sender, RoutedEventArgs e)
        {
            ((CoinViewModel) this.DataContext).Save();
            this.ResetUI();
        }

        private void ResetUI()
        {
            if (!this.AddColumn.ActualWidth.Equals(0))
            {
                Animation.AnimationSlide(this.AddColumn, null, this.AddColumn.ActualWidth, 0);
            }
            if (!this.RollColumn.ActualWidth.Equals(0))
            {
                Animation.AnimationSlide(this.RollColumn, null, this.AddColumn.ActualWidth, 0);
            }
        }

        #endregion
    }
}
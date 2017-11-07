#region Using

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Noside.CoinCounter.Models;
using Noside.Common.Ui;
using System.Collections.Generic;

#endregion

using System.Windows.Data;

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
	        ((CoinViewModel) this.DataContext).LoadDone += (sender, args) => {

		        foreach (
			        Component.CoinBox box in
			        ((CoinViewModel) this.DataContext).CoinList.Select(
				        coin => new Component.CoinBox {Coin = coin, Margin = new Thickness(3)})) {
			        this.Panel.Children.Add(box);
			        box.RollCoins += this.Box_RollCoins;
		        }
	        };
        }

        #endregion

        #region Properties

        public bool Dirty => ((CoinViewModel) this.DataContext).Dirty;

        #endregion

        #region Methods

        private void Box_RollCoins(object sender, EventArgs args)
        {
            Component.CoinBox box = sender as Component.CoinBox;
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

        public List<UIElement> GetToolbarButtons()
        {
            var list = new List<UIElement>();
            Button add = new Button();
            add.Width = 124;
            add.Click += this.OnAddCoins;
            add.Margin = new Thickness(3, 0, 0, 0);
            add.Content = Properties.Resources.Generic_Add;
            add.Style = (Style)App.Current.Resources["NsdButtonStyle"];
            

            Button cash = new Button();
            cash.Width = 124;
            cash.Click += this.OnRollCoins;
            cash.Margin = new Thickness(3, 0, 0, 0);
            cash.Content = Properties.Resources.CoinCounter_MainView_CashRolls;
            cash.Style = (Style)App.Current.Resources["NsdButtonStyle"];

            Button save = new Button();
            save.Width = 124;
            save.Click += this.OnSaveClicked;
            save.Margin = new Thickness(3, 0, 0, 0);
            save.Content = Properties.Resources.Generic_Save;
            save.Style = (Style)App.Current.Resources["NsdButtonStyle"];
            save.SetBinding(Button.IsEnabledProperty, new Binding { Path = new PropertyPath("Dirty"), UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

            Button reset = new Button();
            reset.Width = 124;
            reset.Click += this.OnResetClicked;
            reset.Margin = new Thickness(3, 0, 0, 0);
            reset.Content = Properties.Resources.Generic_Reset;
            reset.Style = (Style)App.Current.Resources["NsdButtonStyle"];
            reset.SetBinding(Button.IsEnabledProperty, new Binding { Path = new PropertyPath("Dirty"), UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

            list.Add(add);
            list.Add(cash);
            list.Add(save);
            list.Add(reset);

            return list;
         /*               < Button Content = "{x:Static properties:Resources.Generic_Add}" Click = "OnAddCoins" Style = "{StaticResource NsdButtonStyle}" Margin = "3,0,0,0"
                    Width = "124" />
            < Button Content = "{x:Static properties:Resources.CoinCounter_MainView_CashRolls}" Click = "OnRollCoins" Style = "{StaticResource NsdButtonStyle}" Margin = "3,0,0,0"
                    Width = "124" />
            < Button Content = "{x:Static properties:Resources.Generic_Save}" Click = "OnSaveClicked"
                    IsEnabled = "{Binding Dirty, UpdateSourceTrigger=PropertyChanged}"
                    Style = "{StaticResource NsdButtonStyle}" Width = "124" Margin = "3,0,0,0" />
    
                < Button Content = "{x:Static properties:Resources.Generic_Reset}" Click = "OnResetClicked"
                    IsEnabled = "{Binding Dirty, UpdateSourceTrigger=PropertyChanged}"
                    Style = "{StaticResource NsdButtonStyle}" Width = "124" Margin = "3,0,0,0" />
    
                < Rectangle />
                */
        }
    }
}
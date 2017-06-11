#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using NoSideDynamics.CoinCounter.Models;
using NoSideDynamics.CoinCounter.Windows;

#endregion

namespace NoSideDynamics.CoinCounter.Controls
{
    /// <summary>
    ///     Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            foreach (var box in ((CoinViewModel) DataContext).CoinList.Select(coin => new CoinBox
            {
                Coin = coin,
                Margin = new Thickness(3)
            }))
            {
                Panel.Children.Add(box);
                box.RollCoins += Box_RollCoins;
            }
            Panel.Loaded += Panel_Loaded;
        }

        public bool Dirty => ((CoinViewModel) DataContext).Dirty;

        private void OnAddCoins(object sender, RoutedEventArgs e)
        {
            var addWindow = new Add();
            if (addWindow.ShowDialog() ?? false)
            {
                ((CoinViewModel) DataContext).AddCoins((uint) addWindow.DollarValue, (uint) addWindow.QuarterValue,
                    (uint) addWindow.DimeValue, (uint) addWindow.NickelValue, (uint) addWindow.PennyValue);
            }
        }

        private void RollCoinsOnClick(object sender, RoutedEventArgs e)
        {
            var cvm = (CoinViewModel) DataContext;
            var rollWindow = new Roll(cvm.CoinList[0].RollsToCash, cvm.CoinList[1].RollsToCash,
                cvm.CoinList[2].RollsToCash, cvm.CoinList[3].RollsToCash, cvm.CoinList[4].RollsToCash);
            if (rollWindow.ShowDialog() ?? false)
            {
                cvm.CashRolls(rollWindow.DollarCashed, rollWindow.QuarterCashed, rollWindow.DimeCashed,
                    rollWindow.NickelCashed, rollWindow.PennyCashed);
            }
        }

        private void Box_RollCoins(object sender, EventArgs args)
        {
            var box = sender as CoinBox;
            var coin = box?.Coin;
            ((CoinViewModel) DataContext).Roll(coin);
        }

        private void Panel_Loaded(object sender, RoutedEventArgs e)
        {
            var stackPanel = sender as StackPanel;
            if (stackPanel != null) Width = stackPanel.ActualWidth + 4;
        }

        private void OnSaveClicked(object sender, RoutedEventArgs e)
        {
            ((CoinViewModel) DataContext).Save();
        }
    }
}
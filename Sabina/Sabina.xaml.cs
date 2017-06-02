using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using NoSideDynamics.CoinCounter.Controls;
using NoSideDynamics.CoinCounter.Models;
using NoSideDynamics.CoinCounter.Windows;

namespace NoSideDynamics
{
    /// <summary>
    /// Interaction logic for Sabina.xaml
    /// </summary>
    public partial class Sabina : Window
    {
        public Sabina()
        {
            this.InitializeComponent();
            ((CoinViewModel)this.DataContext).Init();
            foreach (Coin coin in ((CoinViewModel)this.DataContext).CoinList)
            {
                CoinBox box = new CoinBox
                {
                    Coin = coin,
                    Margin = new Thickness(3)
                };
                this.Panel.Children.Add(box);
                box.RollCoins += this.Box_RollCoins;
            }
            this.Panel.Loaded += this.Panel_Loaded;

        }

        private void Box_RollCoins(object sender, System.EventArgs args)
        {
            CoinBox box = sender as CoinBox;
            Coin coin = box?.Coin;
            ((CoinViewModel)this.DataContext).Roll(coin);
        }

        private void Panel_Loaded(object sender, RoutedEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;
            if (stackPanel != null) this.Width = stackPanel.ActualWidth + 4;
        }

        #region Methods

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (!((CoinViewModel)this.DataContext).Dirty) return;
            MessageBoxResult result = Common.Windows.MessageBox.Show("Save Results?", "Save", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Cancel) e.Cancel = true;
            if (result == MessageBoxResult.Yes) ((CoinViewModel)this.DataContext).Save();
        }

        private void OnAddCoins(object sender, RoutedEventArgs e)
        {
            Add addWindow = new Add();
            if (addWindow.ShowDialog() ?? false)
            {
                ((CoinViewModel)this.DataContext).AddCoins((uint)addWindow.DollarValue, (uint)addWindow.QuarterValue,
                    (uint)addWindow.DimeValue, (uint)addWindow.NickelValue, (uint)addWindow.PennyValue);
            }
        }

        private void RollCoinsOnClick(object sender, RoutedEventArgs e)
        {
            CoinViewModel cvm = (CoinViewModel)this.DataContext;
            Roll rollWindow = new Roll(cvm.CoinList[0].RollsToCash, cvm.CoinList[1].RollsToCash,
                cvm.CoinList[2].RollsToCash, cvm.CoinList[3].RollsToCash, cvm.CoinList[4].RollsToCash);
            if (rollWindow.ShowDialog() ?? false)
            {
                cvm.CashRolls(rollWindow.DollarCashed, rollWindow.QuarterCashed, rollWindow.DimeCashed,
                    rollWindow.NickelCashed, rollWindow.PennyCashed);
            }
        }

        #endregion

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void OnDragMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton != System.Windows.Input.MouseButtonState.Pressed) return;
            this.DragMove();
        }

        private void OnSaveClicked(object sender, RoutedEventArgs e)
        {
            ((CoinViewModel)this.DataContext).Save();
        }
    }
}

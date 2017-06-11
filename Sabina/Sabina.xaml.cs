#region Using

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NoSideDynamics.CoinCounter.Controls;
using NoSideDynamics.CoinCounter.Models;
using NoSideDynamics.CoinCounter.Windows;
using MessageBox = NoSideDynamics.Common.Windows.MessageBox;

#endregion

namespace NoSideDynamics
{
    /// <summary>
    ///     Interaction logic for Sabina.xaml
    /// </summary>
    public partial class Sabina
    {
        public Sabina()
        {
            InitializeComponent();
        }

        #region Methods

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (!CoinView.Dirty) return;
            var result = MessageBox.Show("Save Results?", "Save", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Cancel) e.Cancel = true;
            if (result == MessageBoxResult.Yes) ((CoinViewModel) DataContext).Save();
        }

        #endregion

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void OnDragMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;
            DragMove();
        }

        private void CoinView_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Width = (sender as Control).ActualWidth;
        }
    }
}
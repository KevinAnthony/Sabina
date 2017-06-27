#region Using

using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Noside.CoinCounter.Models;
using MessageBox = Noside.Common.Windows.MessageBox;

#endregion

namespace Noside
{
    /// <summary>
    ///     Interaction logic for Wyvern.xaml
    /// </summary>
    public partial class Wyvern
    {
        #region Constructors and Destructors

        public Wyvern()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (!CoinView.Dirty) return;
            var result = MessageBox.Show("Save Results?", "Save", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Cancel) e.Cancel = true;
            if (result == MessageBoxResult.Yes) ((CoinViewModel) DataContext).Save();
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

        #endregion
    }
}
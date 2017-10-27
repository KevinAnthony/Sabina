#region Using

using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Noside.CoinCounter.Models;
using MessageBox = Noside.Common.Windows.MessageBox;
using System.Windows.Media;
using System;
using System.Threading.Tasks;
using System.Windows.Interop;
using Noside.Common;
using Noside.Common.Windows;
using Color = System.Drawing.Color;

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
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (!this.CoinView.Dirty) return;
            MessageBoxResult result = MessageBox.Show(Properties.Resources.Wyvern_MainWindow_OnClosing_Save_Results_, Properties.Resources.Generic_Save, MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Cancel) e.Cancel = true;
            if (result == MessageBoxResult.Yes) ((CoinViewModel) this.DataContext).Save();
        }

        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void OnDragMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;
            this.DragMove();
        }

        #endregion

        private async void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            //this.Hide();
            //this.ShowInTaskbar = false;
            //var ss = await SpashScreen.Load();
            //ss.Close();
            //this.Show();
            //this.ShowInTaskbar = true;
            Window wnd = (Window)sender;
            GlassHelper.ExtendGlassFrame(wnd);
	        
        }
    }
}
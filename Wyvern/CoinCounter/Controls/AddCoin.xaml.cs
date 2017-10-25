#region Using

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Noside.CoinCounter.Controls.Component;
using Noside.CoinCounter.Models;

#endregion

namespace Noside.CoinCounter.Controls
{
    /// <summary>
    ///     Interaction logic for AddCoin.xaml
    /// </summary>
    public partial class AddCoin : INotifyPropertyChanged
    {
        
        #region Constructors and Destructors

        public AddCoin()
        {
	        this.InitializeComponent();
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            var control = this.Parent as FrameworkElement;
            var model = control?.DataContext as CoinViewModel;
            if (model == null) return;
            model.LoadDone += this.Model_LoadDone;
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
                var box = new AddBox(coin);
                Grid.SetRow(box, index);
                this.TheGrid.Children.Add(box);
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
	        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
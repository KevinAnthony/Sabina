#region Using

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Noside.Annotations;
using Noside.CoinCounter.Models;

#endregion

namespace Noside.CoinCounter.Controls
{
    internal class CountButton :Button, INotifyPropertyChanged
    {
        /// <summary>
        /// Dependency property registration for the wrapper <see cref="Coin"/> property.
        /// </summary>
        public static readonly DependencyProperty CoinProperty = DependencyProperty.Register("Coin", typeof (Coin), typeof (
            CountButton));

        public Coin Coin
        {
            get { return (Coin) GetValue(CoinProperty); }
            set
            {
                SetValue(CoinProperty, value);
                value.PropertyChanged += ValueOnPropertyChanged;
            }
        }

        private void ValueOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            this.OnPropertyChanged(nameof(Coin));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
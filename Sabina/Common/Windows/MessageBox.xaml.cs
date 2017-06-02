#region Using

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

#endregion

namespace NoSideDynamics.Common.Windows
{
    /// <summary>
    ///     Interaction logic for MessageBox.xaml
    /// </summary>
    public partial class MessageBox : INotifyPropertyChanged
    {
        #region Fields

        private string _text;

        #endregion

        #region Constructors and Destructors

        public MessageBox()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        public MessageBoxResult Result { get; set; }

        public string Text {
            get { return this._text; }
            set {
                if (value == this._text) return;
                this._text = value;
                this.OnPropertyChanged();
            }
        }

        #endregion

        #region Methods

        internal static MessageBoxResult Show(string text, string title, MessageBoxButton buttons)
        {
            MessageBox box = new MessageBox
            {
                Title = title,
                Text = text
            };
            switch (buttons)
            {
                case MessageBoxButton.OK:
                    box.OkColumn.Width = new GridLength(98);
                    box.YesColumn.Width = new GridLength(0);
                    box.NoColumn.Width = new GridLength(0);
                    box.CancelColumn.Width = new GridLength(0);
                    box.Width = 150;
                    box.CloseButton.IsCancel = true;
                    box.OkButton.IsDefault = true;
                    break;
                case MessageBoxButton.OKCancel:
                    box.OkColumn.Width = new GridLength(98);
                    box.YesColumn.Width = new GridLength(0);
                    box.NoColumn.Width = new GridLength(0);
                    box.CancelColumn.Width = new GridLength(98);
                    box.Width = 248;
                    box.CancelButton.IsCancel = true;
                    box.OkButton.IsDefault = true;
                    break;
                case MessageBoxButton.YesNoCancel:
                    box.OkColumn.Width = new GridLength(0);
                    box.YesColumn.Width = new GridLength(98);
                    box.NoColumn.Width = new GridLength(98);
                    box.CancelColumn.Width = new GridLength(98);
                    box.Width = 346;
                    box.CancelButton.IsCancel = true;
                    box.YesButton.IsDefault = true;
                    break;
                case MessageBoxButton.YesNo:
                    box.OkColumn.Width = new GridLength(0);
                    box.YesColumn.Width = new GridLength(98);
                    box.NoColumn.Width = new GridLength(98);
                    box.CancelColumn.Width = new GridLength(0);
                    box.Width = 248;
                    box.NoButton.IsCancel = true;
                    box.YesButton.IsDefault = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(buttons), buttons, null);
            }
            box.ShowDialog();
            return box.Result;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            this.Result = MessageBoxResult.Cancel;
            this.Close();
        }

        private void OnDragMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;
            this.DragMove();
        }

        private void OnNoClicked(object sender, RoutedEventArgs e)
        {
            this.Result = MessageBoxResult.No;
            this.Close();
        }

        private void OnOkClicked(object sender, RoutedEventArgs e)
        {
            this.Result = MessageBoxResult.OK;
            this.Close();
        }

        private void OnYesClicked(object sender, RoutedEventArgs e)
        {
            this.Result = MessageBoxResult.Yes;
            this.Close();
        }

        #endregion
    }
}
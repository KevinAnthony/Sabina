#region Using

using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Noside.Wyvern.Common.Interfaces;

#endregion

namespace Noside.Wyvern.CoinCounter.Controls {
	/// <summary>
	///     Interaction logic for AddBox.xaml
	/// </summary>
	public partial class AddBox : UserControl {
		#region Fields

		private uint _originalCount;

		#endregion

		#region Constructors and Destructors

		public AddBox() {
			this.InitializeComponent();
			this.DataContextChanged += this.AddBox_DataContextChanged;
		}

		#endregion

		#region Methods



		private static bool IsTextAllowed(string text) {
			var regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
			return !regex.IsMatch(text);
		}

		private void AddBox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
			ICoin coin = this.DataContext as ICoin;
			this._originalCount = coin?.Count ?? 0;
		}

		private void OnGotFocus(object sender, EventArgs e) {
			var tb = sender as TextBox;
			tb?.SelectAll();
		}

		private void TextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e) {
			e.Handled = !IsTextAllowed(e.Text);
		}

		private void TextBox_TextBoxPasting(object sender, DataObjectPastingEventArgs e) {
			if (e.DataObject.GetDataPresent(typeof(string))) {
				var text = (string) e.DataObject.GetData(typeof(string));
				if (!IsTextAllowed(text)) e.CancelCommand();
			}
			else {
				e.CancelCommand();
			}
		}

		private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e) {
			var tb = sender as TextBox;
			var raw = tb?.Text;
			if (!(this.DataContext is ICoin coin)) return;
			if (string.IsNullOrWhiteSpace(raw)) {
				if (tb != null) tb.Text = "0";
				coin.Count = this._originalCount;
			}

			if (!uint.TryParse(raw, out uint count)) return;
			coin.Count = this._originalCount + count;
		}

		#endregion

		private void AddBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			ICoin coin = this.DataContext as ICoin;
			if (this.IsVisible) {
				this._originalCount = coin?.Count ?? 0;
			}
			else {
				this._originalCount = coin?.Count ?? 0;
				this.TextInput.Text = string.Empty;
			}
		}
	}
}
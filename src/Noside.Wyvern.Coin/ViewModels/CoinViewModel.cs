#region Using

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Noside.Wyvern.CoinCounter.Properties;
using Noside.Wyvern.Common.Interfaces;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

#endregion

namespace Noside.Wyvern.CoinCounter.ViewModels {
	public class CoinViewModel : BindableBase {
		#region Fields

		private readonly ICoinDatabase _database;

		private ObservableCollection<ICoin> _coinList;
		private bool _dirty;
		private float _rolledValue;
		private float _totalValue;
		private float _unrolledValue;
		private bool _showAddCoins;
		private bool _showCashRolls;

		#endregion

		#region Constructors and Destructors

		public CoinViewModel(ICoinDatabase database) {

			this.AddCommand = new DelegateCommand(this.RaiseAdd);
			this.AddRequest = new InteractionRequest<INotification>();
			this.CashRollsCommand = new DelegateCommand(this.RaiseCashRolls);
			this.CashRollsRequest = new InteractionRequest<INotification>();
			this.SaveCommand = new DelegateCommand(this.OnSave);
			this.ResetCommand = new DelegateCommand(this.OnReset);

			this.CoinList = new ObservableCollection<ICoin>();
			this._database = database;
			this._database.Loaded += this.Database_Loaded;
			this._database.Load();
		}

		#endregion

		#region Events

		public event EventHandler LoadDone;
		
		#endregion

		#region Properties

		public ICommand AddCommand { get; set; }
		public InteractionRequest<INotification> AddRequest { get; set; }

		public ICommand CashRollsCommand { get; set; }
		public InteractionRequest<INotification> CashRollsRequest { get; set; }

		public ICommand SaveCashRollsCommand { get; set; }

		public ObservableCollection<ICoin> CoinList {
			get => this._coinList;
			set => this.SetProperty(ref this._coinList, value);
		}

		public bool Dirty {
			get => this._dirty;
			set => this.SetProperty(ref this._dirty, value);
		}

		public bool ShowAddCoins
		{
			get => this._showAddCoins;
			set => this.SetProperty(ref this._showAddCoins, value);
		}

		public bool ShowCashRolls
		{
			get => this._showCashRolls;
			set => this.SetProperty(ref this._showCashRolls, value);
		}

		public ICommand ResetCommand { get; set; }

		public float RolledValue {
			get => this._rolledValue;
			set => this.SetProperty(ref this._rolledValue, value);
		}

		public ICommand SaveCommand { get; set; }

		public float TotalValue {
			get => this._totalValue;
			set => this.SetProperty(ref this._totalValue, value);
		}

		public float UnrolledValue {
			get => this._unrolledValue;
			set => this.SetProperty(ref this._unrolledValue, value);
		}

		#endregion

		#region Public Methods

		public void AddCoins(params uint[] values) {
			if (values.Length != this.CoinList.Count)
				throw new ArgumentOutOfRangeException(nameof(values), Resources.CoinViewModel_AddCoins_Count_Mismatch);
			for (var index = 0; index < values.Length; index++)
				this.CoinList[index].Count += values[index];
		}

		public void CashRolls(params uint[] values) {
			if (values.Length != this.CoinList.Count)
				throw new ArgumentOutOfRangeException(nameof(values), Resources.CoinViewModel_AddCoins_Count_Mismatch);
			for (var index = 0; index < values.Length; index++) {
				this.CoinList[index].RollsToCash -= values[index];
				this.CoinList[index].Count -= values[index] * this.CoinList[index].CoinsPerRoll;
			}
		}

		public void Roll(Models.Coin coin) {
			if (coin == null) return;
			var coinsRolled = coin.CoinsPerRoll * coin.RollsToCash;
			if (coinsRolled > coin.Count) return;
			if (coin.Count - coinsRolled < coin.CoinsPerRoll)
				MessageBox.Show(string.Format(Resources.CoinViewModel_NotEnoughCoins, coin.Name, Environment.NewLine),
					Resources.Generic_Error, MessageBoxButton.OK);
			coin.RollsToCash++;
		}

		#endregion

		#region Methods

		private void RaiseCashRolls() {
			var notification = new Notification();
			notification.Title = "Cash Rolls";
			notification.Content = "Cash Some Rolls, Get some Cash";
			this.CashRollsRequest.Raise(notification);
		}

		private void RaiseAdd() {
			var notification = new Notification();
			notification.Title = "Add Coins";
			notification.Content = "Add Coins To Count";
			this.AddRequest.Raise(notification);
		}

		private void Database_Loaded(object sender, EventArgs e) {
			this.CoinList = new ObservableCollection<ICoin>(this._database.CoinList);
			foreach (var coin in this.CoinList) {
				coin.PropertyChanged += (s, a) => this.RecalcValues();
			}
			this._database.Clean();
			this.RecalcValues();
		}
		
		private void OnReset() {
			this._database.Load();
			this.HideSubControls();
		}

		private void HideSubControls() {
			this.ShowAddCoins = false;
			this.ShowCashRolls = false;
		}

		private void OnSave() {
			this._database.Save();
			this.HideSubControls();
		}

		private void RecalcValues() {
			this.Dirty = this.CoinList.Any(coin => coin.Dirty);
			this.RolledValue = this.CoinList.Sum(coin => coin.RolledValue);
			this.TotalValue = this.CoinList.Sum(coin => coin.TotalValue);
			this.UnrolledValue = this.CoinList.Sum(coin => coin.UnrolledValue);
		}

		#endregion
	}
}
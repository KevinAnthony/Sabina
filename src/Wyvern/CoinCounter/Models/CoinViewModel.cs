#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json.Linq;
using Noside.Common;
using Noside.Common.Load;
using Noside.Common.Source;
using Noside.Properties;
using MessageBox = Noside.Common.Windows.MessageBox;

#endregion

namespace Noside.CoinCounter.Models {

	public class CoinViewModel : Loadable, INotifyPropertyChanged {

		private readonly string _sheetName = Resources.Sheet_Name;
		private string _sheetId;
        private readonly LoadInfo _checkInfo;

        #region Constructors and Destructors

        public CoinViewModel() {
		
			if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
			{
                this._checkInfo = new LoadInfo(this.CheckId, "Checking ID");

                this.LoadList.Add(new LoadInfo(this.ParseCoinList, "Loading Coin List" ));
				this.LoadList.Add(new LoadInfo(GoogleApi.Login, "Logging into Google" ));
				this.LoadList.Add(new LoadInfo(this.FindSheet,"Finding Sheet" ));
				this.LoadList.Add(this._checkInfo);
				this.LoadList.Add(new LoadInfo(this.LoadFromSpreadsheet, "Loading Coin Data" ));
				this.LoadList.Add(new LoadInfo(this.RaiseLoadDone, "Finalizing Coin Data" ));
			}
			else {
				this.ParseCoinList();
				foreach (var coin in this.CoinList)
				{
					coin.Count = 100;
					coin.RollsToCash = 1;
				}
			}
			
		}

		public event EventHandler LoadDone;

		private async Task RaiseLoadDone() {
			this.LoadDone?.Invoke(this, new EventArgs());
		}

		private async Task FindSheet() {
			this._sheetId = await GoogleApi.FindSpreadsheetId(this._sheetName);
		}

		private async Task CheckId() {
			if (string.IsNullOrWhiteSpace(this._sheetId))
			{
                var index = this.LoadList.IndexOf(this._checkInfo);
                var newInfo = new LoadInfo(async () => {
                    IList<IList<object>> data = this.CreateNewSheet(out var range);
	                this._sheetId = await GoogleApi.CreateSpreadsheet(this._sheetName, range, data);
                }, "Creating Sheet");

				this.LoadList.Insert(index + 1, newInfo);

            }
		}

		#endregion

		#region Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		private IList<IList<object>> CreateNewSheet(out string range) {
			// Left Column, Coins, Space, Total value, Total Rolled
			var count = this.CoinList.Count + 4;

			range = $"A1:{(char) ('A' + count - 1)}6";
			var header = new object[count].Init("");
			var counts = new object[count].Init("");
			var rolls = new object[count].Init("");
			var values = new object[count].Init("");
			var spaces = new object[count].Init("");
			var wraps = new object[count].Init("");

			counts[0] = "Counts";
			rolls[0] = "Rolls To Cash";
			values[0] = "Value";
			values[this.CoinList.Count + 2] = "Total";
			spaces[this.CoinList.Count + 2] = $"=sum(B4:{(char) ('B' + (this.CoinList.Count - 1))}4)";
			values[this.CoinList.Count + 3] = "Total Rolled";
			spaces[this.CoinList.Count + 3] = "=";
			wraps[0] = "Required Wraps";
			for (var i = 0; i < this.CoinList.Count; i++) {
				var coin = this.CoinList[i];
				header[i + 1] = coin.Name;
				counts[i + 1] = coin.Count;
				rolls[i + 1] = coin.RollsToCash;
				var c = (char) ('B' + i);
				values[i + 1] = $"={c}2 * {coin.Value}";
				wraps[i + 1] = $"={c}2 / {coin.CoinsPerRoll} - {c}3";
				spaces[this.CoinList.Count + 3] += $"({c}3 * {coin.Value * coin.CoinsPerRoll}) + ";
			}

			spaces[this.CoinList.Count + 3] = ((string) spaces[this.CoinList.Count + 3]).Trim(' ', '+');

			return new List<IList<object>> {
				header,
				counts,
				rolls,
				values,
				spaces,
				wraps
			};
		}

		private async Task ParseCoinList() {
			foreach (var token in JArray.Parse(Resources.Coins)) {
				var jobj = (JObject) token;

				var name = ((JValue) jobj["name"]).Value as string;
				var value = Convert.ToSingle(((JValue) jobj["value"]).Value);
				var perRoll = Convert.ToUInt32(((JValue) jobj["perRoll"]).Value);
				this.CoinList.Add(new Coin(name, value, perRoll));
			}
			foreach (var coin in this.CoinList)
				coin.PropertyChanged += this.CoinOnPropertyChanged;
		}

		public async void Reset() {
			foreach (var coin in this.CoinList) {
				coin.Count = 0;
				coin.RollsToCash = 0;
				coin.Dirty = false;
			}
			await this.LoadFromSpreadsheet();
		}


		#region Properties

		public ObservableCollection<Coin> CoinList { get; set; } = new ObservableCollection<Coin>();

		public bool Dirty {
			get { return this.CoinList.Any(coin => coin.Dirty); }
		}

		public float RolledValue => this.CoinList.Where(coin => !coin.Value.Equals(1.00f)).Sum(coin => coin.RolledValue);

		public float TotalValue => this.CoinList.Sum(coin => coin.TotalValue);

		public float UnrolledValue => this.CoinList.Where(coin => !coin.Value.Equals(1.00f)).Sum(coin => coin.UnrolledValue);

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


		public async Task LoadFromSpreadsheet() {
			try {
				//Since B is inusive, Subtract one
				var range = $"B2:{(char) ('B' + (this.CoinList.Count - 1))}3";

				var values = await GoogleApi.ReadValuesFromSheet(this._sheetId, range);
				if (values != null && values.Count > 0)
					for (var c = 0; c < values[0].Count; c++) {
						var coin = this.CoinList[c];
						coin.Count = uint.Parse((string) values[0][c]);
						coin.RollsToCash = uint.Parse((string) values[1][c]);
						this.Clean();
					}
				else
					Debug.WriteLine("No data found.");
			} catch (HttpRequestException) { }
		}

		public void Roll(Coin coin) {
			if (coin == null) return;
			var coinsRolled = coin.CoinsPerRoll * coin.RollsToCash;
			if (coinsRolled > coin.Count) return;
			if (coin.Count - coinsRolled < coin.CoinsPerRoll)
				MessageBox.Show(string.Format(Resources.CoinViewModel_NotEnoughCoins, coin.Name, Environment.NewLine),
					Resources.Generic_Error, MessageBoxButton.OK);
			coin.RollsToCash++;
		}

		public async void Save() {
			//since b is inusive, Subtract one
			var range = $"B2:{(char) ('B' + (this.CoinList.Count - 1))}3";
			var data = new List<IList<object>> {
				this.CoinList.Select(coin => coin.Count).Cast<object>().ToList(),
				this.CoinList.Select(coin => coin.RollsToCash).Cast<object>().ToList()
			};
			GoogleApi.WriteValuesToSheet(this._sheetId, range, data);
			this.Clean();
		}

		#endregion

		#region Methods

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void Clean() {
			foreach (var coin in this.CoinList)
				coin.Dirty = false;
		}

		private void CoinOnPropertyChanged(object sender, PropertyChangedEventArgs args) {
			this.OnPropertyChanged(args.PropertyName);
		}

		#endregion

	}
}
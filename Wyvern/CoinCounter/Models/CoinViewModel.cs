#region Using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Coin_Counter.Annotations;
using Newtonsoft.Json.Linq;
using Noside.Common;
using Noside.Common.Source;
using Noside.Properties;
using MessageBox = Noside.Common.Windows.MessageBox;

#endregion

namespace Noside.CoinCounter.Models {
	public class CoinViewModel : INotifyPropertyChanged {
		private readonly string _sheetName = Resources.Sheet_Name;
		private string _sheetId;

		#region Constructors and Destructors

		public CoinViewModel() {
			Load();
			foreach (var coin in CoinList)
				coin.PropertyChanged += CoinOnPropertyChanged;
		}

		#endregion

		#region Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		private async void Load() {
			ParseCoinList();
			//Don't get real values if in desingner mode!
			if (!DesignerProperties.GetIsInDesignMode(new DependencyObject())) {
				await GoogleApi.Login();
				_sheetId = await GoogleApi.FindSpreadsheetId(_sheetName);
				if (string.IsNullOrWhiteSpace(_sheetId)) {
					string range;
					var data = CreateNewSheet(out range);
					_sheetId = await GoogleApi.CreateSpreadsheet(_sheetName, range, data);
				}
				await LoadFromSpreadsheet();
			} else {
				foreach (var coin in CoinList) {
					coin.Count = 100;
					coin.RollsToCash = 1;
				}
			}
		}

		private IList<IList<object>> CreateNewSheet(out string range) {
			// Left Column, Coins, Space, Total value, Total Rolled
			var count = CoinList.Count + 4;

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
			values[CoinList.Count + 2] = "Total";
			spaces[CoinList.Count + 2] = $"=sum(B4:{(char) ('B' + (CoinList.Count - 1))}4)";
			values[CoinList.Count + 3] = "Total Rolled";
			spaces[CoinList.Count + 3] = "=";
			wraps[0] = "Required Wraps";
			for (var i = 0; i < CoinList.Count; i++) {
				var coin = CoinList[i];
				header[i + 1] = coin.Name;
				counts[i + 1] = coin.Count;
				rolls[i + 1] = coin.RollsToCash;
				var c = (char) ('B' + i);
				values[i + 1] = $"={c}2 * {coin.Value}";
				wraps[i + 1] = $"={c}2 / {coin.CoinsPerRoll} - {c}3";
				spaces[CoinList.Count + 3] += $"({c}3 * {coin.Value * coin.CoinsPerRoll}) + ";
			}

			spaces[CoinList.Count + 3] = ((string) spaces[CoinList.Count + 3]).Trim(' ', '+');

			return new List<IList<object>> {
				header,
				counts,
				rolls,
				values,
				spaces,
				wraps
			};
		}

		private void ParseCoinList() {
			foreach (var token in JArray.Parse(Resources.Coins)) {
				var jobj = (JObject) token;

				var name = ((JValue) jobj["name"]).Value as string;
				var value = Convert.ToSingle(((JValue) jobj["value"]).Value);
				var perRoll = Convert.ToUInt32(((JValue) jobj["perRoll"]).Value);
				CoinList.Add(new Coin(name, value, perRoll));
			}
		}

		public async void Reset() {
			foreach (var coin in CoinList) {
				coin.Count = 0;
				coin.RollsToCash = 0;
				coin.Dirty = false;
			}
			await LoadFromSpreadsheet();
		}


		#region Properties

		public ObservableCollection<Coin> CoinList { get; set; } = new ObservableCollection<Coin>();

		public bool Dirty {
			get { return CoinList.Any(coin => coin.Dirty); }
		}

		public float RolledValue => CoinList.Where(coin => !coin.Value.Equals(1.00f)).Sum(coin => coin.RolledValue);

		public float TotalValue => CoinList.Sum(coin => coin.TotalValue);

		public float UnrolledValue => CoinList.Where(coin => !coin.Value.Equals(1.00f)).Sum(coin => coin.UnrolledValue);

		#endregion

		#region Public Methods

		public void AddCoins(params uint[] values) {
			if (values.Length != CoinList.Count)
				throw new ArgumentOutOfRangeException(nameof(values), Resources.CoinViewModel_AddCoins_Count_Mismatch);
			for (var index = 0; index < values.Length; index++)
				CoinList[index].Count += values[index];
		}

		public void CashRolls(params uint[] values) {
			if (values.Length != CoinList.Count)
				throw new ArgumentOutOfRangeException(nameof(values), Resources.CoinViewModel_AddCoins_Count_Mismatch);
			for (var index = 0; index < values.Length; index++) {
				CoinList[index].RollsToCash -= values[index];
				CoinList[index].Count -= values[index] * CoinList[index].CoinsPerRoll;
			}
		}


		public async Task LoadFromSpreadsheet() {
			try {
				//Since B is inusive, Subtract one
				var range = $"B2:{(char) ('B' + (CoinList.Count - 1))}3";

				var values = await GoogleApi.ReadValuesFromSheet(_sheetId, range);
				if (values != null && values.Count > 0)
					for (var c = 0; c < values[0].Count; c++) {
						var coin = CoinList[c];
						coin.Count = uint.Parse((string) values[0][c]);
						coin.RollsToCash = uint.Parse((string) values[1][c]);
						Clean();
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
			var range = $"B2:{(char) ('B' + (CoinList.Count - 1))}3";
			var data = new List<IList<object>> {
				CoinList.Select(coin => coin.Count).Cast<object>().ToList(),
				CoinList.Select(coin => coin.RollsToCash).Cast<object>().ToList()
			};
			GoogleApi.WriteValuesToSheet(_sheetId, range, data);
			Clean();
		}

		#endregion

		#region Methods

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void Clean() {
			foreach (var coin in CoinList)
				coin.Dirty = false;
		}

		private void CoinOnPropertyChanged(object sender, PropertyChangedEventArgs args) {
			OnPropertyChanged(args.PropertyName);
		}

		#endregion
	}
}
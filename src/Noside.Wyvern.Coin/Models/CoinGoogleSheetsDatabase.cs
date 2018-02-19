#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json.Linq;
using Noside.Wyvern.CoinCounter.Properties;
using Noside.Wyvern.Common.Interfaces;
using Prism.Mvvm;

#endregion

namespace Noside.Wyvern.CoinCounter.Models {
	public class CoinGoogleSheetsDatabase : BindableBase, ICoinDatabase {
		#region Fields

		private readonly string _sheetName = Resources.Sheet_Name;

		private bool _dirty;
		private float _rolledValue;

		private string _sheetId;
		private float _totalValue;
		private float _unrolledValue;
		private IGoogleApi _googleApiApi;

		#endregion

		#region Properties

		public CoinGoogleSheetsDatabase(IGoogleApi googleApi) {
			this._googleApiApi = googleApi;
		}
		public IList<ICoin> CoinList { get; set; }

		public bool Dirty {
			get => this._dirty;
			set => this.SetProperty(ref this._dirty, value);
		}

		public float RolledValue {
			get => this._rolledValue;
			set => this.SetProperty(ref this._rolledValue, value);
		}

		public float TotalValue {
			get => this._totalValue;
			set => this.SetProperty(ref this._totalValue, value);
		}

		public float UnrolledValue {
			get => this._unrolledValue;
			set => this.SetProperty(ref this._unrolledValue, value);
		}

		public event EventHandler<EventArgs> Loaded;

		public event EventHandler<EventArgs> Saved;

		#endregion

		#region Public Methods

		public void Clean() {
			foreach (var coin in this.CoinList)
				coin.Dirty = false;
		}

		public async Task Load() {
			this.CoinList = new List<ICoin>();
			if (!this._googleApiApi.Authenticated)
				await this._googleApiApi.Login();
			await this.ParseCoinList();
			await this.FindSheet();
			await this.CheckId();
			try {
				//Since B is inusive, Subtract one
				var range = $"B2:{(char) ('B' + (this.CoinList.Count - 1))}3";

				var values = await this._googleApiApi.ReadValuesFromSheet(this._sheetId, range);
				if (values != null && values.Count > 0)
					for (var c = 0; c < values[0].Count; c++) {
						var coin = this.CoinList[c];
						coin.Count = uint.Parse((string) values[0][c]);
						coin.RollsToCash = uint.Parse((string) values[1][c]);
						this.Clean();
					}
				else
					Debug.WriteLine("No data found.");

				this.Loaded?.Invoke(this, EventArgs.Empty);
			}
			catch (HttpRequestException) { }

		}

		public async void Reset() {
			foreach (var coin in this.CoinList) {
				coin.Count = 0;
				coin.RollsToCash = 0;
				coin.Dirty = false;
			}

			await this.Load();
		}

		public async void Save() {
			//since b is inusive, Subtract one
			var range = $"B2:{(char) ('B' + (this.CoinList.Count - 1))}3";
			var data = new List<IList<object>> {
				this.CoinList.Select(coin => coin.Count).Cast<object>().ToList(),
				this.CoinList.Select(coin => coin.RollsToCash).Cast<object>().ToList()
			};
			this._googleApiApi.WriteValuesToSheet(this._sheetId, range, data);
			this.Clean();
			this.Saved?.Invoke(this, EventArgs.Empty);
		}

		#endregion

		#region Methods

		private async Task CheckId() {
			if (string.IsNullOrWhiteSpace(this._sheetId)) {
				IList<IList<object>> data = this.CreateNewSheet(out var range);
				this._sheetId = await this._googleApiApi.CreateSpreadsheet(this._sheetName, range, data);
			}
		}

		private IList<IList<object>> CreateNewSheet(out string range) {
			// Left Column, Coins, Space, Total value, Total Rolled
			var count = this.CoinList.Count + 4;

			range = $"A1:{(char) ('A' + count - 1)}6";
			var header = Common.Extensions.Init(new object[count], "");
			var counts = Common.Extensions.Init(new object[count], "");
			var rolls = Common.Extensions.Init(new object[count], "");
			var values = Common.Extensions.Init(new object[count], "");
			var spaces = Common.Extensions.Init(new object[count], "");
			var wraps = Common.Extensions.Init(new object[count], "");

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

		private async Task FindSheet() {
			this._sheetId = await this._googleApiApi.FindSpreadsheetId(this._sheetName);
		}

		private async Task ParseCoinList() {
			foreach (var token in JArray.Parse(Resources.Coins)) {
				var jobj = (JObject) token;
				var coin = ServiceLocator.Current.GetInstance<ICoin>();
				coin.Name = ((JValue) jobj["name"]).Value as string;
				coin.Value = Convert.ToSingle(((JValue) jobj["value"]).Value);
				coin.CoinsPerRoll = Convert.ToUInt32(((JValue) jobj["perRoll"]).Value);
				this.CoinList.Add(coin);
			}

			foreach (var coin in this.CoinList)
				coin.PropertyChanged += this.RecalcValues;
		}

		private void RecalcValues(object sender, PropertyChangedEventArgs e) {
			this.Dirty = this.CoinList.Any(coin => coin.Dirty);
			this.RolledValue = this.CoinList.Where(coin => !coin.Value.Equals(1.00f)).Sum(coin => coin.RolledValue);
			this.TotalValue = this.CoinList.Sum(coin => coin.TotalValue);
			this.UnrolledValue = this.CoinList.Where(coin => !coin.Value.Equals(1.00f)).Sum(coin => coin.UnrolledValue);
		}

		#endregion
	}
}
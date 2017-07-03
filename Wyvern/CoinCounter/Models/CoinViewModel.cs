#region Using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Coin_Counter.Annotations;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using Noside.Common.Helpers;
using Noside.Properties;

#endregion

namespace Noside.CoinCounter.Models
{
    public class CoinViewModel : INotifyPropertyChanged
    {
        #region Constructors and Destructors

        public CoinViewModel()
        {
            Load();
            foreach (var coin in CoinList)
            {
//                if (coin.Value.Equals(1.00f)) continue;
                coin.PropertyChanged += CoinOnPropertyChanged;
            }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private async void Load()
        {
            await Login();
            await FindSpreadsheetId();
            await LoadFromSpreadsheet();
        }

        public async void Reset()
        {
            foreach (var coin in CoinList)
            {
                coin.Count = 0;
                coin.RollsToCash = 0;
                coin.Dirty = false;
            }
            await LoadFromSpreadsheet();
        }

        #region Fields

        private string ApplicationName = "Coin Counter Mk.4";
        private readonly string _sheetName = Resources.Sheet_Name;
        private readonly string[] _scopes = {SheetsService.Scope.Spreadsheets, DriveService.Scope.DriveReadonly};
        private UserCredential _credential;
        private string _spreadsheetId = "1FOnkhoTo_h3gs-T9jgAjPyUVyxOLdtc0PieGkKIfZtQ";
        private SheetsService _spreadSheetsService;

        #endregion

        #region Properties

        public ObservableCollection<Coin> CoinList { get; set; } = new ObservableCollection<Coin>
        {
            //TODO: L10n this (somehow)
            new Coin("Dollar", 1.00f, 25),
            new Coin("Quarter", 0.25f, 40),
            new Coin("Dime", 0.10f, 50),
            new Coin("Nickel", 0.05f, 40),
            new Coin("Penny", 0.01f, 50),

        };

        public bool Dirty
        {
            get { return CoinList.Any(coin => coin.Dirty); }
        }

        public float RolledValue => CoinList.Where(coin => !coin.Value.Equals(1.00f)).Sum(coin => coin.RolledValue);

        public float TotalValue => CoinList.Sum(coin => coin.Total);

        public float UnrolledValue => CoinList.Where(coin => !coin.Value.Equals(1.00f)).Sum(coin => coin.UnrolledValue);

        #endregion

        #region Public Methods

        public void AddCoins(params uint[] values)
        {
            if (values.Length != CoinList.Count) throw new ArgumentOutOfRangeException(nameof(values), Resources.CoinViewModel_AddCoins_Count_Mismatch);
            for (var index = 0; index < values.Length; index++)
            {
                CoinList[index].Count += values[index];
            }
        }

        public void CashRolls(params uint[] values)
        {
            if (values.Length != CoinList.Count) throw new ArgumentOutOfRangeException(nameof(values), Resources.CoinViewModel_AddCoins_Count_Mismatch);
            for (var index = 0; index < values.Length; index++)
            {
                CoinList[index].RollsToCash -= values[index];
                CoinList[index].Count -= values[index] * CoinList[index].CoinsPerRoll;
            }
        }

        public async Task FindSpreadsheetId()
        {
            var drive = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = _credential,
                ApplicationName = ApplicationName
            });
            var listRequest = drive.Files.List();
            var request = await listRequest.ExecuteAsync();
            var files = request.Files;
            var id =
                files.Where(file => file.Name.Equals(_sheetName, StringComparison.OrdinalIgnoreCase))
                    .Select(file => file.Id)
                    .FirstOrDefault();
            if (string.IsNullOrWhiteSpace(id))
            {
                id = await CreateSpreadsheet();
            }
            _spreadsheetId = id;
        }

        public async Task LoadFromSpreadsheet()
        {
            try
            {
                if (_spreadSheetsService == null)
                {
                    _spreadSheetsService = new SheetsService(new BaseClientService.Initializer
                    {
                        HttpClientInitializer = _credential,
                        ApplicationName = ApplicationName
                    });
                }
                //Since B is inusive, Subtract one
                string range = $"B2:{(char)('B' + (CoinList.Count - 1))}3";
                var response = await _spreadSheetsService.Spreadsheets.Values.Get(_spreadsheetId, range).ExecuteAsync();

                var values = response.Values;
                if (values != null && values.Count > 0)
                {
                    for (var c = 0; c < values[0].Count; c++)
                    {
                        var coin = CoinList[c];
                        coin.Count = uint.Parse((string) values[0][c]);
                        coin.RollsToCash = uint.Parse((string) values[1][c]);
                        Clean();
                    }
                }
                else
                {
                    Debug.WriteLine("No data found.");
                }
            }
            catch (HttpRequestException)
            {
            }
        }

        public async Task Login()
        {
            try
            {
                using (var stream =
                    new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
                {
                    var credPath = Environment.GetFolderPath(
                        Environment.SpecialFolder.Personal);
                    credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

                    _credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets, _scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true));
                }
            }
            catch (Exception)
            {
                
            }
        }

        public void Roll(Coin coin)
        {
            if (coin == null) return;
            var coinsRolled = coin.CoinsPerRoll*coin.RollsToCash;
            if (coinsRolled > coin.Count) return;
            if (coin.Count - coinsRolled < coin.CoinsPerRoll)
            {
                MessageBox.Show(
                    string.Format(Resources.CoinViewModel_Roll_Cannot_Roll__0__s_1_Not_Enough_Coins_To_Roll, coin.Name, Environment.NewLine), Resources.CoinViewModel_Roll_Error,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            coin.RollsToCash++;
        }

        public async void Save()
        {
            //since b is inusive, Subtract one
            char b = (char)('B' + (CoinList.Count - 1));  
            var r = $"B2:{b}3";
            var range = new ValueRange
            {
                Values = new List<IList<object>>
                {
                    CoinList.Select(coin => coin.Count).Cast<object>().ToList(),
                    CoinList.Select(coin => coin.RollsToCash).Cast<object>().ToList()
                },
                Range = r
            };

            var request = _spreadSheetsService.Spreadsheets.Values.Update(range, _spreadsheetId,r);
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            await request.ExecuteAsync();
            Clean();
        }

        #endregion

        #region Methods

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Clean()
        {
            foreach (var coin in CoinList)
            {
                coin.Dirty = false;
            }
        }

        private void CoinOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            OnPropertyChanged(args.PropertyName);
        }

        private async Task<string> CreateSpreadsheet()
        {
            _spreadSheetsService = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = _credential,
                ApplicationName = ApplicationName
            });
            var sheet = new Spreadsheet {Properties = new SpreadsheetProperties {Title = _sheetName}};
            var id = (await _spreadSheetsService.Spreadsheets.Create(sheet).ExecuteAsync()).SpreadsheetId;

            // Left Column, Coins, Space, Total value, Total Rolled
            var count = CoinList.Count + 4;

            string range = $"A1:{(char)('A' + count - 1)}6";
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
            spaces[CoinList.Count + 2] = $"=sum(B4:{(char)('B' + (CoinList.Count - 1))}4)";
            values[CoinList.Count + 3] = "Total Rolled";
            spaces[CoinList.Count + 3] = "=";
            wraps[0] = "Required Wraps";
            for (var i = 0; i < this.CoinList.Count; i++)
            {
                var coin = this.CoinList[i];
                header[i + 1] = coin.Name;
                counts[i + 1] = coin.Count;
                rolls[i + 1] = coin.RollsToCash;
                char c = (char) ('B' + i);
                values[i + 1] = $"={c}2 * {coin.Value}";
                wraps[i + 1] = $"={c}2 / {coin.CoinsPerRoll} - {c}3";
                spaces[CoinList.Count + 3] += $"({c}3 * {coin.Value * coin.CoinsPerRoll}) + ";
            }
            
            spaces[CoinList.Count + 3] = ((string)spaces[CoinList.Count + 3]).Trim(' ', '+');

            IList<IList<object>> list = new List<IList<object>>
            {
                header,
                counts,
                rolls,
                values,
                spaces,
                wraps
            };

            var valueRange = new ValueRange
            {
                Range = range,
                Values = list
            };

            var upd = _spreadSheetsService.Spreadsheets.Values.Update(valueRange, id, range);
            upd.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            await upd.ExecuteAsync();

            return id;
        }

        #endregion
    }
}
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
                if (coin.Value.Equals(1.00f)) continue;
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

        private const string ApplicationName = "Coin Counter Mk.4";
        private const string SheetName = "Coin Counts";
        private readonly string[] _scopes = {SheetsService.Scope.Spreadsheets, DriveService.Scope.DriveReadonly};
        private UserCredential _credential;
        private string _spreadsheetId = "1FOnkhoTo_h3gs-T9jgAjPyUVyxOLdtc0PieGkKIfZtQ";
        private SheetsService _spreadSheetsService;

        #endregion

        #region Properties

        public ObservableCollection<Coin> CoinList { get; set; } = new ObservableCollection<Coin>
        {
            new Coin("Dollar", 1.00f, 25),
            new Coin("Quarter", 0.25f, 40),
            new Coin("Dime", 0.10f, 50),
            new Coin("Nickel", 0.05f, 40),
            new Coin("Penny", 0.01f, 50)
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

        public void AddCoins(uint dollarValue, uint quarterValue, uint dimeValue, uint nickelValue, uint pennyValue)
        {
            CoinList[0].Count += dollarValue;
            CoinList[1].Count += quarterValue;
            CoinList[2].Count += dimeValue;
            CoinList[3].Count += nickelValue;
            CoinList[4].Count += pennyValue;
        }

        public void CashRolls(uint dollar, uint quarter, uint dime, uint nickel, uint penny)
        {
            CoinList[0].RollsToCash -= dollar;
            CoinList[0].Count -= dollar*CoinList[0].CoinsPerRoll;
            CoinList[1].RollsToCash -= quarter;
            CoinList[1].Count -= quarter*CoinList[1].CoinsPerRoll;
            CoinList[2].RollsToCash -= dime;
            CoinList[2].Count -= dime*CoinList[2].CoinsPerRoll;
            CoinList[3].RollsToCash -= nickel;
            CoinList[3].Count -= nickel*CoinList[3].CoinsPerRoll;
            CoinList[4].RollsToCash -= penny;
            CoinList[4].Count -= penny*CoinList[4].CoinsPerRoll;

            CoinList[0].Count += Convert.ToUInt32(quarter*CoinList[1].CoinsPerRoll*CoinList[1].Value);
            CoinList[0].Count += Convert.ToUInt32(dime*CoinList[2].CoinsPerRoll*CoinList[2].Value);
            CoinList[0].Count += Convert.ToUInt32(nickel*CoinList[3].CoinsPerRoll*CoinList[3].Value);
            CoinList[0].Count += Convert.ToUInt32(penny*CoinList[4].CoinsPerRoll*CoinList[4].Value);
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
                files.Where(file => file.Name.Equals(SheetName, StringComparison.OrdinalIgnoreCase))
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

                const string range = "B2:F3";
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

        public void Roll(Coin coin)
        {
            if (coin == null) return;
            var coinsRolled = coin.CoinsPerRoll*coin.RollsToCash;
            if (coinsRolled > coin.Count) return;
            if (coin.Count - coinsRolled < coin.CoinsPerRoll)
            {
                MessageBox.Show($"Cannot Roll {coin.Name}'s{Environment.NewLine}Not Enough Coins To Roll", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            coin.RollsToCash++;
        }

        public async void Save()
        {
            var range = new ValueRange
            {
                Values = new List<IList<object>>
                {
                    new List<object>
                    {
                        CoinList[0].Count,
                        CoinList[1].Count,
                        CoinList[2].Count,
                        CoinList[3].Count,
                        CoinList[4].Count
                    },
                    new List<object>
                    {
                        CoinList[0].RollsToCash,
                        CoinList[1].RollsToCash,
                        CoinList[2].RollsToCash,
                        CoinList[3].RollsToCash,
                        CoinList[4].RollsToCash
                    }
                },
                Range = "B2:F3"
            };

            var request = _spreadSheetsService.Spreadsheets.Values.Update(range, _spreadsheetId, "B2:F3");
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
            var sheet = new Spreadsheet {Properties = new SpreadsheetProperties {Title = SheetName}};
            var id = (await _spreadSheetsService.Spreadsheets.Create(sheet).ExecuteAsync()).SpreadsheetId;

            const string range = "A1:J6";

            IList<IList<object>> list = new List<IList<object>>
            {
                new List<object> {"", "Dollars", "Quarters", "Dimes", "Nickels", "Pennies", "", "", "", ""},
                new List<object> {"Counts", "0", "0", "0", "0", "0", "", "", "", ""},
                new List<object> {"Rolls To Cash", "0", "0", "0", "0", "0", "", "", "", ""},
                new List<object>
                {
                    "Value",
                    "=B2*1",
                    "=C2*0.25",
                    "=D2*0.10",
                    "=E2*0.05",
                    "=F2*0.01",
                    "",
                    "Total",
                    "",
                    "Total Rolled"
                },
                new List<object> {"", "", "", "", "", "", "", "=sum(B4:F4)", "", "=B3*25+C3*10+D3*5+E3*2+F3*0.5"},
                new List<object>
                {
                    "Required Wraps",
                    "=B2/25",
                    "=C2/40-C3",
                    "=D2/40-D3",
                    "=E2/40-E3",
                    "=F2/40-F3",
                    "",
                    "",
                    "",
                    ""
                }
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
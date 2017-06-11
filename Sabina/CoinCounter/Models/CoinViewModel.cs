#region Using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
using File = Google.Apis.Drive.v3.Data.File;

#endregion

namespace NoSideDynamics.CoinCounter.Models
{
    public class CoinViewModel : INotifyPropertyChanged
    {
        #region Fields

        private const string ApplicationName = "Coin Counter Mk.4";
        private const string SheetName = "Coin Counts";
        private readonly string[] _scopes = {SheetsService.Scope.Spreadsheets, DriveService.Scope.DriveReadonly};
        private UserCredential _credential;
        private string _spreadsheetId = "1FOnkhoTo_h3gs-T9jgAjPyUVyxOLdtc0PieGkKIfZtQ";
        private SheetsService _spreadSheetsService;

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

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

        public float RolledValue => this.CoinList.Where(coin => !coin.Value.Equals(1.00f)).Sum(coin => coin.RolledValue);

        public float TotalValue => this.CoinList.Sum(coin => coin.Total);

        public float UnrolledValue => this.CoinList.Where(coin => !coin.Value.Equals(1.00f)).Sum(coin => coin.UnrolledValue);

        #endregion

        #region Public Methods

        public void AddCoins(uint dollarValue, uint quarterValue, uint dimeValue, uint nickelValue, uint pennyValue)
        {
            this.CoinList[0].Count += dollarValue;
            this.CoinList[1].Count += quarterValue;
            this.CoinList[2].Count += dimeValue;
            this.CoinList[3].Count += nickelValue;
            this.CoinList[4].Count += pennyValue;
        }

        public void CashRolls(uint dollar, uint quarter, uint dime, uint nickel, uint penny)
        {
            this.CoinList[0].RollsToCash -= dollar;
            this.CoinList[0].Count -= dollar * this.CoinList[0].CoinsPerRoll;
            this.CoinList[1].RollsToCash -= quarter;
            this.CoinList[1].Count -= quarter * this.CoinList[1].CoinsPerRoll;
            this.CoinList[2].RollsToCash -= dime;
            this.CoinList[2].Count -= dime * this.CoinList[2].CoinsPerRoll;
            this.CoinList[3].RollsToCash -= nickel;
            this.CoinList[3].Count -= nickel * this.CoinList[3].CoinsPerRoll;
            this.CoinList[4].RollsToCash -= penny;
            this.CoinList[4].Count -= penny * this.CoinList[4].CoinsPerRoll;

            this.CoinList[0].Count += Convert.ToUInt32(quarter * this.CoinList[1].CoinsPerRoll * this.CoinList[1].Value);
            this.CoinList[0].Count += Convert.ToUInt32(dime * this.CoinList[2].CoinsPerRoll * this.CoinList[2].Value);
            this.CoinList[0].Count += Convert.ToUInt32(nickel * this.CoinList[3].CoinsPerRoll * this.CoinList[3].Value);
            this.CoinList[0].Count += Convert.ToUInt32(penny * this.CoinList[4].CoinsPerRoll * this.CoinList[4].Value);
        }

        public void FindSpreadsheetId()
        {
            DriveService drive = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = this._credential,
                ApplicationName = ApplicationName
            });
            FilesResource.ListRequest listRequest = drive.Files.List();
            IList<File> files = listRequest.Execute()
                                           .Files;
            string id = (files.Where(file => file.Name.Equals(SheetName, StringComparison.OrdinalIgnoreCase))
                .Select(file => file.Id)).FirstOrDefault();
            if (string.IsNullOrWhiteSpace(id))
            {
                id = this.CreateSpreadsheet();
            }
            this._spreadsheetId = id;
        }

        public CoinViewModel()
        {
            Task.Factory.StartNew(() =>
            {
                this.Login();
                this.FindSpreadsheetId();
                this.LoadFromSpreadsheet();
            });
            foreach (Coin coin in this.CoinList)
            {
                if (coin.Value.Equals(1.00f)) continue;
                coin.PropertyChanged += this.CoinOnPropertyChanged;
            }
        }
        
        public void LoadFromSpreadsheet()
        {
            if (this._spreadSheetsService == null)
            {
                this._spreadSheetsService = new SheetsService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = this._credential,
                    ApplicationName = ApplicationName
                });
            }

            const string range = "B2:F3";
            SpreadsheetsResource.ValuesResource.GetRequest request = this._spreadSheetsService.Spreadsheets.Values.Get(this._spreadsheetId, range);


            ValueRange response = request.Execute();
            IList<IList<object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                for (int c = 0; c < values[0].Count; c++)
                {
                    Coin coin = this.CoinList[c];
                    coin.Count = uint.Parse((string) values[0][c]);
                    coin.RollsToCash = uint.Parse((string) values[1][c]);
                    this.Clean();
                }
            }
            else
            {
                Debug.WriteLine("No data found.");
            }
        }

        public void Login()
        {
            using (FileStream stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = Environment.GetFolderPath(
                    Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

                this._credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets, this._scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }
        }

        public void Roll(Coin coin)
        {
            if (coin == null) return;
            uint coinsRolled = coin.CoinsPerRoll * coin.RollsToCash;
            if (coinsRolled > coin.Count) return;
            if (coin.Count - coinsRolled < coin.CoinsPerRoll)
            {
                MessageBox.Show($"Cannot Roll {coin.Name}'s{Environment.NewLine}Not Enough Coins To Roll", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            coin.RollsToCash++;
        }

        public bool Dirty {
            get { return this.CoinList.Any(coin => coin.Dirty); }
        }

        public void Save()
        {
            ValueRange range = new ValueRange
            {
                Values = new List<IList<object>>
                {
                    new List<object>
                    {
                        this.CoinList[0].Count,
                        this.CoinList[1].Count,
                        this.CoinList[2].Count,
                        this.CoinList[3].Count,
                        this.CoinList[4].Count
                    },
                    new List<object>
                    {
                        this.CoinList[0].RollsToCash,
                        this.CoinList[1].RollsToCash,
                        this.CoinList[2].RollsToCash,
                        this.CoinList[3].RollsToCash,
                        this.CoinList[4].RollsToCash
                    }
                },
                Range = "B2:F3"
            };

            SpreadsheetsResource.ValuesResource.UpdateRequest request = this._spreadSheetsService.Spreadsheets.Values.Update(range, this._spreadsheetId, "B2:F3");
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            UpdateValuesResponse exec = request.Execute();
            this.Clean();
        }

        private void Clean()
        {
            foreach (Coin coin in this.CoinList)
            {
                coin.Dirty = false;
            }
        }
        #endregion

        #region Methods

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CoinOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(this.RolledValue))
            {
                this.OnPropertyChanged(nameof(this.RolledValue));
                this.OnPropertyChanged(nameof(this.TotalValue));
            }
            if (args.PropertyName == nameof(this.UnrolledValue))
            {
                this.OnPropertyChanged(nameof(this.UnrolledValue));
                this.OnPropertyChanged(nameof(this.TotalValue));
            }
            if (args.PropertyName == nameof(this.Dirty))
            {
                this.OnPropertyChanged(nameof(this.Dirty));
            }
        }

        private string CreateSpreadsheet()
        {
            this._spreadSheetsService = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = this._credential,
                ApplicationName = ApplicationName
            });
            Spreadsheet sheet = new Spreadsheet {Properties = new SpreadsheetProperties {Title = SheetName}};
            SpreadsheetsResource.CreateRequest request = this._spreadSheetsService.Spreadsheets.Create(sheet);
            Spreadsheet response = request.Execute();
            string id = response.SpreadsheetId;
            string range = "A1:J6";

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

            ValueRange valueRange = new ValueRange
            {
                Range = range,
                Values = list
            };

            SpreadsheetsResource.ValuesResource.UpdateRequest upd = this._spreadSheetsService.Spreadsheets.Values.Update(valueRange, id, range);
            upd.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            upd.Execute();

            return id;
        }

        #endregion
    }
}
#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using Noside.Wyvern.Common.Interfaces;

#endregion

namespace Noside.Wyvern.Common.ThirdParty {
	public class GoogleApi : IGoogleApi
	{
		#region Fields

		private readonly string[] _scopes = {SheetsService.Scope.Spreadsheets, DriveService.Scope.DriveReadonly};

		private readonly string ApplicationName = "Coin Counter Mk.4";

		private UserCredential _credential;

		private SheetsService _spreadSheetsService;

		#endregion

		#region Properties

		public bool Authenticated { get; set; }

		#endregion

		#region Public Methods

		public async Task<string> CreateSpreadsheet(string sheetName, string range, IList<IList<object>> data) {
			this._spreadSheetsService = new SheetsService(new BaseClientService.Initializer {
				HttpClientInitializer = this._credential,
				ApplicationName = this.ApplicationName
			});
			var sheet = new Spreadsheet {Properties = new SpreadsheetProperties {Title = sheetName}};
			var id = (await this._spreadSheetsService.Spreadsheets.Create(sheet).ExecuteAsync()).SpreadsheetId;

			var valueRange = new ValueRange {
				Range = range,
				Values = data
			};

			var upd = this._spreadSheetsService.Spreadsheets.Values.Update(valueRange, id, range);
			upd.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
			await upd.ExecuteAsync();

			return id;
		}

		public async Task<string> FindSpreadsheetId(string sheetName) {
			var drive = new DriveService(new BaseClientService.Initializer {
				HttpClientInitializer = this._credential,
				ApplicationName = this.ApplicationName
			});
			var listRequest = drive.Files.List();
			var request = await listRequest.ExecuteAsync();
			var files = request.Files;
			var id =
				files.Where(file => file.Name.Equals(sheetName, StringComparison.OrdinalIgnoreCase))
				     .Select(file => file.Id)
				     .FirstOrDefault();
			return id;
		}

		public async Task Login() {
			using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read)) {
				var credPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

				this._credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
					GoogleClientSecrets.Load(stream).Secrets, this._scopes,
					"user",
					CancellationToken.None,
					new FileDataStore(credPath, true));
			}

			this.Authenticated = true;
		}

		public async Task<IList<IList<object>>> ReadValuesFromSheet(string spreadsheetId, string range) {
			if (this._spreadSheetsService == null)
				this._spreadSheetsService = new SheetsService(new BaseClientService.Initializer {
					HttpClientInitializer = this._credential,
					ApplicationName = this.ApplicationName
				});
			var response = await this._spreadSheetsService.Spreadsheets.Values.Get(spreadsheetId, range).ExecuteAsync();
			return response.Values;
		}

		public async Task WriteValuesToSheet(string spreadsheetId, string range, IList<IList<object>> data) {
			var valueRange = new ValueRange {
				Values = data,
				Range = range
			};

			var request = this._spreadSheetsService.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
			request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
			await request.ExecuteAsync();
		}

		#endregion
	}
}
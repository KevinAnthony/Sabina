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

#endregion

namespace Noside.Common.Source {
	internal class GoogleApi {
		public static async Task Login() {
			using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read)) {
				var credPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

				_credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
					GoogleClientSecrets.Load(stream).Secrets, Scopes,
					"user",
					CancellationToken.None,
					new FileDataStore(credPath, true));
			}
		}

		public static async Task<string> FindSpreadsheetId(string sheetName) {
			var drive = new DriveService(new BaseClientService.Initializer {
				HttpClientInitializer = _credential,
				ApplicationName = ApplicationName
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

		public static async Task<string> CreateSpreadsheet(string sheetName, string range, IList<IList<object>> data) {
			_spreadSheetsService = new SheetsService(new BaseClientService.Initializer {
				HttpClientInitializer = _credential,
				ApplicationName = ApplicationName
			});
			var sheet = new Spreadsheet {Properties = new SpreadsheetProperties {Title = sheetName}};
			var id = (await _spreadSheetsService.Spreadsheets.Create(sheet).ExecuteAsync()).SpreadsheetId;


			var valueRange = new ValueRange {
				Range = range,
				Values = data
			};

			var upd = _spreadSheetsService.Spreadsheets.Values.Update(valueRange, id, range);
			upd.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
			await upd.ExecuteAsync();

			return id;
		}

		public static async Task<IList<IList<object>>> ReadValuesFromSheet(string spreadsheetId, string range) {
			if (_spreadSheetsService == null)
				_spreadSheetsService = new SheetsService(new BaseClientService.Initializer {
					HttpClientInitializer = _credential,
					ApplicationName = ApplicationName
				});
			var response = await _spreadSheetsService.Spreadsheets.Values.Get(spreadsheetId, range).ExecuteAsync();
			return response.Values;
		}

		public static async Task WriteValuesToSheet(string spreadsheetId, string range, IList<IList<object>> data) {
			var valueRange = new ValueRange {
				Values = data,
				Range = range
			};

			var request = _spreadSheetsService.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
			request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
			await request.ExecuteAsync();
		}

		#region Fields

		private static readonly string ApplicationName = "Coin Counter Mk.4";

		private static readonly string[] Scopes = {SheetsService.Scope.Spreadsheets, DriveService.Scope.DriveReadonly};

		private static UserCredential _credential;

		private static SheetsService _spreadSheetsService;

		#endregion
	}
}
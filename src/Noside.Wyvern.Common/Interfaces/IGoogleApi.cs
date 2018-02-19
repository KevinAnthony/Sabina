using System.Collections.Generic;
using System.Threading.Tasks;

namespace Noside.Wyvern.Common.Interfaces
{
	public interface IGoogleApi
	{
		bool Authenticated { get; set; }

		Task<string> CreateSpreadsheet(string sheetName, string range, IList<IList<object>> data);
		Task<string> FindSpreadsheetId(string sheetName);
		Task Login();
		Task<IList<IList<object>>> ReadValuesFromSheet(string spreadsheetId, string range);
		Task WriteValuesToSheet(string spreadsheetId, string range, IList<IList<object>> data);
	}
}
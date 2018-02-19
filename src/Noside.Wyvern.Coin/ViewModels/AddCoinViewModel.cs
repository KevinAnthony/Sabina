#region Using

using System.Collections.ObjectModel;
using Noside.Wyvern.Common.Interfaces;

#endregion

namespace Noside.Wyvern.CoinCounter.ViewModels {
	internal class AddCoinViewModel {

		#region Constructors and Destructors

		public AddCoinViewModel(ICoinDatabase database) {
			this.CoinList = new ObservableCollection<ICoin>(database.CoinList);
		}

		#endregion

		#region Properties

		public ObservableCollection<ICoin> CoinList { get; set; }

		#endregion
	}
}
#region Using

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace Noside.Wyvern.Common.Interfaces {
	public interface ICoinDatabase {
		#region Properties

		IList<ICoin> CoinList { get; set; }

		bool Dirty { get; }

		float RolledValue { get; }

		float TotalValue { get; }

		float UnrolledValue { get; }

		event EventHandler<EventArgs> Loaded;

		event EventHandler<EventArgs> Saved;

		#endregion

		#region Public Methods

		void Clean();
		Task Load();
		void Reset();
		void Save();

		#endregion
	}
}
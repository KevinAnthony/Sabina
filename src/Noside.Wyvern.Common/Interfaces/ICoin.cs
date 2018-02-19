#region Using

using System.ComponentModel;

#endregion

namespace Noside.Wyvern.Common.Interfaces {
	public interface ICoin {
		#region Events

		event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Properties

		uint CashedRolls { get; set; }

		uint CoinsPerRoll { get; set; }

		uint Count { get; set; }

		bool Dirty { get; set; }

		string Name { get; set; }

		bool Rollable { get; }

		float RolledValue { get; }

		uint RollsToCash { get; set; }

		float TotalValue { get; }

		uint UnrolledCount { get; }

		float UnrolledValue { get; }

		float Value { get; set; }

		#endregion

		#region Public Methods

		void CashRolls();

		void Roll();

		#endregion
	}
}
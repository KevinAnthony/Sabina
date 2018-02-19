#region Using

#endregion

using Noside.Wyvern.Common.Interfaces;
using Prism.Mvvm;

namespace Noside.Wyvern.CoinCounter.Models {
	public class Coin : BindableBase, ICoin {
		#region Fields

		private uint _cashedRolls;
		private uint _coinsPerRoll;
		private uint _count;
		private bool _dirty;
		private string _name;
		private bool _rollable;
		private float _rolledValue;
		private uint _rollsToCash;
		private float _totalValue;
		private uint _unrolledCount;
		private float _unrolledValue;
		private float _value;

		#endregion

		#region Constructors and Destructors

		public Coin() {
			this.PropertyChanged += (s, a) => {
				if (a.PropertyName != nameof(this.Dirty) && a.PropertyName != nameof(this.CashedRolls) )
					this.RecalculateValues();
			};
		}

		#endregion

		#region Properties

		public uint CashedRolls {
			get => this._cashedRolls;
			set => this.SetProperty(ref this._cashedRolls, value);
		}

		public uint CoinsPerRoll {
			get => this._coinsPerRoll;
			set =>
				this.SetProperty(ref this._coinsPerRoll, value);
		}

		public uint Count {
			get => this._count;
			set =>
				this.SetProperty(ref this._count, value);
		}

		public bool Dirty {
			get => this._dirty;
			set =>
				this.SetProperty(ref this._dirty, value);
		}

		public string Name {
			get => this._name;
			set =>
				this.SetProperty(ref this._name, value);
		}

		public bool Rollable {
			get => this._rollable;
			set =>
				this.SetProperty(ref this._rollable, value);
		}

		public float RolledValue {
			get => this._rolledValue;
			set =>
				this.SetProperty(ref this._rolledValue, value);
		}

		public uint RollsToCash {
			get => this._rollsToCash;
			set =>
				this.SetProperty(ref this._rollsToCash, value);
		}

		public float TotalValue {
			get => this._totalValue;
			set =>
				this.SetProperty(ref this._totalValue, value);
		}

		public uint UnrolledCount {
			get => this._unrolledCount;
			set =>
				this.SetProperty(ref this._unrolledCount, value);
		}

		public float UnrolledValue {
			get => this._unrolledValue;
			set =>
				this.SetProperty(ref this._unrolledValue, value);
		}

		public float Value {
			get => this._value;
			set =>
				this.SetProperty(ref this._value, value);
		}

		#endregion

		#region Public Methods

		public void CashRolls() {
			if (this.CashedRolls == 0) return;
			this.Count -= this.CashedRolls * this.CoinsPerRoll;
			this.RollsToCash -= this.CashedRolls;
			this.Dirty = true;
		}

		public void Roll() {
			var coinsRolled = this.CoinsPerRoll * this.RollsToCash;
			if (coinsRolled > this.Count) return;
			if (this.Count - coinsRolled < this.CoinsPerRoll) return;
			this.RollsToCash += 1;
		}

		public override string ToString() {
			return $"{this.Name} - {this.Value:C}";
		}

		#endregion

		#region Methods

		private void RecalculateValues() {
			this.Dirty = true;
			this.Rollable = this.UnrolledCount >= this.CoinsPerRoll;
			this.TotalValue = this.Count * this.Value;
			this.RolledValue = this.RollsToCash * this.CoinsPerRoll * this.Value;
			this.UnrolledCount = this.Count - this.RollsToCash * this.CoinsPerRoll;
			this.UnrolledValue = this.UnrolledCount * this.Value;
		}

		#endregion
	}
}
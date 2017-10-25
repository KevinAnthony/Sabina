#region Using

#region Using

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Coin_Counter.Annotations;

#endregion

// ReSharper disable ExplicitCallerInfoArgument

#endregion

namespace Noside.CoinCounter.Models
{
    public class Coin : INotifyPropertyChanged
    {
        #region Constructors and Destructors

        public Coin(string name, float value, uint perRoll)
        {
	        this.Name = name;
	        this.Value = value;
	        this.CoinsPerRoll = perRoll;
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
	        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Fields

        private uint _count;
        private bool _dirty;
        private uint _rollsToCash;
        private uint _cashedRolls;

        #endregion

        #region Properties

        public uint CoinsPerRoll { get; }

        public uint Count
        {
            get => this._count;
            set
            {
                if (value == this._count) return;
	            this.Dirty = true;
	            this._count = value;
	            this.OnPropertyChanged();
	            this.OnPropertyChanged(nameof(this.UnrolledCount));
	            this.OnPropertyChanged(nameof(this.Rollable));
	            this.OnPropertyChanged(nameof(this.UnrolledValue));
	            this.OnPropertyChanged(nameof(this.TotalValue));
            }
        }

        public bool Dirty
        {
            get => this._dirty;
            set
            {
                if (value == this._dirty) return;
	            this._dirty = value;
	            this.OnPropertyChanged();
            }
        }

        public string Name { get; }

        public bool Rollable => this.UnrolledCount >= this.CoinsPerRoll;

        public float RolledValue => this.RollsToCash* this.CoinsPerRoll* this.Value;

        public uint RollsToCash
        {
            get => this._rollsToCash;
            set
            {
                if (value == this._rollsToCash) return;
	            this.Dirty = true;
	            this._rollsToCash = value;
	            this._cashedRolls = value;
	            this.OnPropertyChanged();
	            this.OnPropertyChanged(nameof(this.CashedRolls));
	            this.OnPropertyChanged(nameof(this.UnrolledCount));
	            this.OnPropertyChanged(nameof(this.Rollable));
	            this.OnPropertyChanged(nameof(this.UnrolledValue));
	            this.OnPropertyChanged(nameof(this.RolledValue));
	            this.OnPropertyChanged(nameof(this.TotalValue));
            }
        }

        public uint CashedRolls
        {
            get => this._cashedRolls;
            set
            {
                if (value == this._cashedRolls) return;
	            this._cashedRolls = value;
	            this.OnPropertyChanged();
            }
        }

        public float TotalValue => this.Count * this.Value;

        public uint UnrolledCount => this.Count - this.RollsToCash* this.CoinsPerRoll;

        public float UnrolledValue => this.UnrolledCount* this.Value;

        public float Value { get; }

        #endregion

        public void CashRolls()
        {
            if (this.CashedRolls == 0) return;
            this.Count -= (this.CashedRolls * this.CoinsPerRoll);
            this.RollsToCash -= this.CashedRolls;

            this.Dirty = true;
        }
    }
}
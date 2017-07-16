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
            Name = name;
            Value = value;
            CoinsPerRoll = perRoll;
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
            get => _count;
            set
            {
                if (value == _count) return;
                Dirty = true;
                _count = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(UnrolledCount));
                OnPropertyChanged(nameof(Rollable));
                OnPropertyChanged(nameof(UnrolledValue));
                OnPropertyChanged(nameof(TotalValue));
            }
        }

        public bool Dirty
        {
            get => _dirty;
            set
            {
                if (value == _dirty) return;
                _dirty = value;
                OnPropertyChanged();
            }
        }

        public string Name { get; }

        public bool Rollable => UnrolledCount >= CoinsPerRoll;

        public float RolledValue => RollsToCash*CoinsPerRoll*Value;

        public uint RollsToCash
        {
            get => _rollsToCash;
            set
            {
                if (value == _rollsToCash) return;
                Dirty = true;
                _rollsToCash = value;
                _cashedRolls = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CashedRolls));
                OnPropertyChanged(nameof(UnrolledCount));
                OnPropertyChanged(nameof(Rollable));
                OnPropertyChanged(nameof(UnrolledValue));
                OnPropertyChanged(nameof(RolledValue));
                OnPropertyChanged(nameof(TotalValue));
            }
        }

        public uint CashedRolls
        {
            get => _cashedRolls;
            set
            {
                if (value == _cashedRolls) return;
                _cashedRolls = value;
                OnPropertyChanged();
            }
        }

        public float TotalValue => Count * Value;

        public uint UnrolledCount => Count - RollsToCash*CoinsPerRoll;

        public float UnrolledValue => UnrolledCount*Value;

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
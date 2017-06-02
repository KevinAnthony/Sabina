#region Using

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Coin_Counter.Annotations;

#endregion

namespace NoSideDynamics.CoinCounter.Models
{
    public class Coin : INotifyPropertyChanged
    {
        #region Fields

        private uint _count;
        private uint _rollsToCash;
        private bool _dirty;

        #endregion

        #region Constructors and Destructors

        public Coin(string name, float value, uint perRoll)
        {
            this.Name = name;
            this.Value = value;
            this.CoinsPerRoll = perRoll;
        }

        public bool Dirty {
            get { return this._dirty; }
            set {
                if (value == this._dirty) return;
                this._dirty = value;
                this.OnPropertyChanged();
            }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        public uint CoinsPerRoll { get; }

        public uint Count {
            get { return this._count; }
            set {
                if (value == this._count) return;
                if (value == this._count) return;
                this.Dirty = true;
                this._count = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.Total));
                this.OnPropertyChanged(nameof(this.UnrolledCount));
                this.OnPropertyChanged(nameof(this.Rollable));
                this.OnPropertyChanged(nameof(this.UnrolledValue));
            }
        }

        public string Name { get; private set; }

        public bool Rollable => this.UnrolledCount >= this.CoinsPerRoll;

        public float RolledValue => this.RollsToCash * this.CoinsPerRoll * this.Value;

        public uint RollsToCash {
            get { return this._rollsToCash; }
            set {
                if (value == this._rollsToCash) return;
                if (value == this._rollsToCash) return;
                this.Dirty = true;
                this._rollsToCash = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.UnrolledCount));
                this.OnPropertyChanged(nameof(this.Rollable));
                this.OnPropertyChanged(nameof(this.UnrolledValue));
                this.OnPropertyChanged(nameof(this.RolledValue));
            }
        }

        public float Total => this.Count * this.Value;

        public uint UnrolledCount => this.Count - this.RollsToCash * this.CoinsPerRoll;

        public float UnrolledValue => this.UnrolledCount * this.Value;

        public float Value { get; }

        #endregion

        #region Methods

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
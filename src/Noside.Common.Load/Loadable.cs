using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Noside.Common.Load
{
    public abstract class Loadable
    {
		public ObservableCollection<LoadInfo> LoadList { get; } = new ObservableCollection<LoadInfo>();

        protected Loadable() {
		    LoadQueue.Register(this);
	    }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noside.Common.Load
{
    public class LoadQueue
    {
        private static List<Loadable> RegisteredLoadables { get; } = new List<Loadable>();

        private static List<LoadInfo> Actions { get
            {
                List<LoadInfo> allActions = new List<LoadInfo>();
                foreach (var loadable in RegisteredLoadables)
                {
                    allActions.AddRange(loadable.LoadList);
                }
                return allActions;
            }
        }

        public static int Count { get { return Actions.Count; } }

        public static LoadInfo Get(int index)
        {
            return Actions[index];
        }

        public static void Register(Loadable loadable)
        {
            RegisteredLoadables.Add(loadable);
            loadable.LoadList.CollectionChanged += Loadable_CollectionChanged;
            CountChanged?.Invoke(loadable, new EventArgs());
        }

        private static void Loadable_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CountChanged?.Invoke(sender, e);
        }

        public static event EventHandler CountChanged;
    }
}

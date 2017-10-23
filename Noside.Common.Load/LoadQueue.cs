using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noside.Common.Load
{
    public class LoadQueue
    {
        private static List<Loadable> RegisteredLoadables { get; } = new List<Loadable>();

        public static List<LoadInfo> Actions { get
            {
                List<LoadInfo> allActions = new List<LoadInfo>();
                foreach (var loadable in LoadQueue.RegisteredLoadables)
                {
                    allActions.AddRange(loadable.LoadList);
                }
                return allActions;
            }
        }

        public static void Register(Loadable loadable)
        {
            RegisteredLoadables.Add(loadable);
        }
    }
}

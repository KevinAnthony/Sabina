using System;
using System.Threading.Tasks;

namespace Noside.Common.Load {
	public class LoadInfo {
		public Func<Task> LoadAction { get; set; }
		public string Description { get; set; }
		public LoadInfo(Func<Task> loadAction, string description) {
			LoadAction = loadAction;
			Description = description;
		}
	}
}
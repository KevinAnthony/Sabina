using System;

namespace Noside.Common.Load {
	public class NewActionEventArgs : EventArgs {
		public LoadInfo Action { get; set; }
		public NewActionEventArgs(LoadInfo action) {
			Action = action;
		}
	}
}
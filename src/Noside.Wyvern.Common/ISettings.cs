using System;
using System.Windows;

namespace Noside.Wyvern.Common
{
	public interface ISettings
	{
		dynamic GetSetting(string key);
		Point GetWindowLocation(string id);
		void SetSetting(string key, dynamic value);
		void SetWindowLocation(string id, Point location);
		void Save();
	}
}
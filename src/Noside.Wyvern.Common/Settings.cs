#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace Noside.Wyvern.Common {
	public class Settings : ISettings {
		#region Fields

		private readonly Dictionary<string, dynamic> _otherSettings;

		private readonly string _saveFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NoSide", "Wyvern", "settings.json");
		private readonly Dictionary<string, Point> _windowLocations;

		#endregion

		#region Constructors and Destructors

		public Settings() {
			if (File.Exists(this._saveFile)) {
				using (StreamReader file = File.OpenText(this._saveFile))
				using (JsonTextReader reader = new JsonTextReader(file)) {
					JObject obj = (JObject) JToken.ReadFrom(reader);
					this._windowLocations = obj["windows"].ToObject<Dictionary<string, Point>>();
					this._otherSettings = obj["settings"].ToObject<Dictionary<string, dynamic>>();
				}
			} else {
				this._windowLocations = new Dictionary<string, Point>();
				this._otherSettings = new Dictionary<string, dynamic>();
			}
		}

		#endregion

		#region Public Methods

		public dynamic GetSetting(string key) {
			return !this._otherSettings.ContainsKey(key) ? null : this._otherSettings[key];
		}

		public Point GetWindowLocation(string id) {
			return this._windowLocations.ContainsKey(id) ? this._windowLocations[id] : new Point(-1, -1);
		}

		public void Save() {
			var dir = Path.GetDirectoryName(this._saveFile);
			if (dir != null && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
			using (StreamWriter file = File.CreateText(this._saveFile)) {
				JsonSerializer serializer = new JsonSerializer();
				JObject jobj = new JObject {
					["windows"] = JObject.FromObject(this._windowLocations),
					["settings"] = JObject.FromObject(this._otherSettings)
				};
				serializer.Serialize(file, jobj);
			}
		}

		public void SetSetting(string key, dynamic value) {
			this._otherSettings[key] = value;
		}

		public void SetWindowLocation(string id, Point location) {
			this._windowLocations[id] = location;
		}

		#endregion
	}
}
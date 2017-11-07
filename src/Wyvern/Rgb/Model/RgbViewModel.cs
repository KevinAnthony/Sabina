#region Using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Media;
using Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Noside.Common.Load;
using Q42.HueApi;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models.Bridge;
using Q42.HueApi.Models.Groups;

#endregion

namespace Noside.Rgb.Model {
	internal class RgbViewModel : Loadable, INotifyPropertyChanged {
		#region Fields

		private Color _selectedColor;
		private List<LocatedBridge> _bridgeIPs;
		private ILocalHueClient _client;
		#endregion

		public ObservableCollection<Light> Lights { get; set; } = new ObservableCollection<Light>();
		public ObservableCollection<Group> Groups { get; set; } = new ObservableCollection<Group>();

		#region Constructors and Destructors

		public RgbViewModel() {
		    for (int i = 0; i < 100; i++)
		    {
		        this.Lights.Add(new Light { Name = $"Light: {i}" });
		        this.Groups.Add(new Group { Name = $"Group: {i}" });
		    }
            this.LoadList.Add(new LoadInfo(this.FindBridge, "Finding HUE Bridge"));
			this.LoadList.Add(new LoadInfo(this.Init, "Initializing App"));
			this.LoadList.Add(new LoadInfo(this.FindLights, "Finding All Lights"));
			this.LoadList.Add(new LoadInfo(this.SetColor, "Setting Color"));
		}

	    private async Task FindLights()
	    {
	        foreach (var light in await this._client.GetLightsAsync())
	        {
	            this.Lights.Add(light);
	        }
	        foreach (var group in await this._client.GetGroupsAsync())
	        {
	            this.Groups.Add(group);
	        }
	}

		private async Task Init() {
			
			if (!File.Exists("hue_secret.json")) {
				ILocalHueClient client = new LocalHueClient(this._bridgeIPs.First().IpAddress);
				//TODO pop up message box, please press link button
				var appKey = await client.RegisterAsync(this._bridgeIPs.First().BridgeId, "wyvern");
				var dict = new Dictionary<string, string> {{"apiKey", appKey}};
				File.WriteAllText(@"hue_secret.json", JsonConvert.SerializeObject(dict));
				return;
			}
			string key = string.Empty;
			using (StreamReader file = File.OpenText(@"hue_secret.json"))
			using (JsonTextReader reader = new JsonTextReader(file)) {
				JObject obj = (JObject) JToken.ReadFrom(reader);
				key = (string)obj["apiKey"];
			}
			this._client = new LocalHueClient(this._bridgeIPs.First().IpAddress, key);
		}

		private async Task FindBridge() {
			IBridgeLocator locator = new HttpBridgeLocator();
			this._bridgeIPs = (await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5))).ToList();
		}

		private async Task SetColor() {
		}

		#endregion

		#region Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Properties

		public Color SelectedColor {
			get => this._selectedColor;
			set {
				if (this._selectedColor == value) return;
				this._selectedColor = value;
				this.OnPropertyChanged();
			}
		}

		#endregion

		#region Methods

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}
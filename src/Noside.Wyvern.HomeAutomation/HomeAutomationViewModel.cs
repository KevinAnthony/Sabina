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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Q42.HueApi;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models.Bridge;
using Q42.HueApi.Models.Groups;

#endregion

namespace Noside.Wyvern.HomeAutomation.Model {
	internal class HomeAutomationViewModel : INotifyPropertyChanged {
		#region Fields

		private List<LocatedBridge> _bridgeIPs;
		private ILocalHueClient _client;

		private Color _selectedColor;
		private bool _isRGB;
		private bool _isDimmable;
	    private bool _isOn;
		private double _brightness;

		#endregion

		#region Constructors and Destructors

		public HomeAutomationViewModel() {
//						for (int i = 0; i < 100; i++) {
//							this.Lights.Add(new Light {Name = $"Light: {i}", Type = "extended color light"});
////							this.Lights.Add(new Light {Name = $"Light: {i}", Type = "dimmable light" });
//							this.Groups.Add(new Group {Name = $"Group: {i}"});
//						}
//			this.LoadList.Add(new LoadInfo(this.FindBridge, "Finding HUE Bridge"));
//			this.LoadList.Add(new LoadInfo(this.Init, "Initializing App"));
//			this.LoadList.Add(new LoadInfo(this.FindLights, "Finding All Lights"));
//			this.LoadList.Add(new LoadInfo(this.SetColor, "Setting Color"));
            this.PropertyChanged += this.HomeAutomationViewModel_PropertyChanged;
		}

        private void HomeAutomationViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(this.IsOn):
                    
                    break;
                case nameof(this.SelectedColor):

                    break;
                case nameof(this.Brightness):

                    break;

            }
        }


        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Properties

		public ObservableCollection<Group> Groups { get; set; } = new ObservableCollection<Group>();

		public ObservableCollection<Light> Lights { get; set; } = new ObservableCollection<Light>();

		public Color SelectedColor {
			get => this._selectedColor;
			set {
				if (this._selectedColor == value) return;
				this._selectedColor = value;
				this.OnPropertyChanged();
			}
		}

		public bool IsRGB {
			get => this._isRGB;
			set {
				if (this.IsRGB == value) return;
				this._isRGB = value;
				this.OnPropertyChanged();
			}
		}

	    public bool IsOn
	    {
	        get => this._isOn;
	        set
	        {
	            if (value == this._isOn) return;
	            this._isOn = value;
	            this.OnPropertyChanged();
	        }
	    }

        public bool IsDimmable {
			get => this._isDimmable;
			set {
				if (this.IsDimmable == value) return;
				this._isDimmable = value;
				this.OnPropertyChanged();
			}
		}

		public double Brightness {
			get => this._brightness;
			set {
				if (value.Equals(this._brightness)) return;
				this._brightness = value;
				this.OnPropertyChanged();
			}
		}

		#endregion

		#region Methods

		[Properties.NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private async Task FindBridge() {
			IBridgeLocator locator = new HttpBridgeLocator();
			this._bridgeIPs = (await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5))).ToList();
		}

		private async Task FindLights() {
			foreach (Light light in await this._client.GetLightsAsync()) {
				this.Lights.Add(light);
			}
			foreach (Group group in await this._client.GetGroupsAsync()) {
				this.Groups.Add(group);
			}
		}

		private async Task Init() {
			if (!File.Exists("hue_secret.json")) {
				ILocalHueClient client = new LocalHueClient(this._bridgeIPs.First().IpAddress);
				//TODO pop up message box, please press link button
				string appKey = await client.RegisterAsync(this._bridgeIPs.First().BridgeId, "wyvern");
				Dictionary<string, string> dict = new Dictionary<string, string> {{"apiKey", appKey}};
				File.WriteAllText(@"hue_secret.json", JsonConvert.SerializeObject(dict));
				return;
			}
			string key = string.Empty;
			using (StreamReader file = File.OpenText(@"hue_secret.json"))
			using (JsonTextReader reader = new JsonTextReader(file)) {
				JObject obj = (JObject) JToken.ReadFrom(reader);
				key = (string) obj["apiKey"];
			}
			this._client = new LocalHueClient(this._bridgeIPs.First().IpAddress, key);
		}

		private async Task SetColor() { }

		#endregion
	}
}
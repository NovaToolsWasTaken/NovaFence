using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NovaFence.Settings.Models
{
    public class ApplicationSettings
    {
        [JsonProperty("location")]
        public Point Location { get; set; }

        [JsonProperty("size")]
        public Size Size { get; set; } = new Size(1013, 617);

        [JsonProperty("autoStart")]
        public bool AutoStart { get; set; } = true;

        [JsonProperty("stayMinimized")]
        public bool StayMinimized { get; set; } = false;

        [JsonProperty("checkUpdates")]
        public bool CheckUpdates { get; set; } = true;
    }
}
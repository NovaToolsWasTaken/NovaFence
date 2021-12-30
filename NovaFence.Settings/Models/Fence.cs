using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NovaFence.Settings.Models
{
    public class Fence
    {
        [JsonProperty("fenceID")]
        public string FenceID { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("location")]
        public Point Location { get; set; } = new Point(705, 359);

        [JsonProperty("backColor")]
        public Color BackColor { get; set; } = Color.FromArgb(20, 20, 20);

        [JsonProperty("bannerColor")]
        public Color BannerColor { get; set; } = Color.FromArgb(15, 15, 15);

        [JsonProperty("opacity")]
        public double Opacity { get; set; } = 0.80;

        [JsonProperty("roundedCorners")]
        public int RoundedCorners { get; set; } = 5;

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("size")]
        public Size Size { get; set; } = new Size(461, 309);

        [JsonProperty("font")]
        public Font Font { get; set; }

        [JsonProperty("isLocked")]
        public bool IsLocked { get; set; } = false;

        [JsonProperty("applications")]
        public List<Application> Applications { get; set; }
    }
}
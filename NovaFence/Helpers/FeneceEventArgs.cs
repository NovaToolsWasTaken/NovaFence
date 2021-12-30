using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaFence.Helpers
{
    public class FeneceEventArgs : EventArgs
    {
        public enum UpdateType
        {
            RoundedCorners,
            Opacity,
            BannerColor,
            BackColor,
            Font
        }
    }
}

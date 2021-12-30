using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NovaFence.UserControls
{
    public partial class AboutTab : UserControl
    {
        public AboutTab(string title, string content)
        {
            InitializeComponent();
            TitleLBL.Text = title;
            ContentLBL.Text = content;
        }
    }
}

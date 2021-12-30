using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NovaFence.SubForms
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void LoadAbout(UserControl userControl)
        {
            userControl.Dock = DockStyle.Top;
            MainPanel.Controls.Add(userControl);
        }

        private async void About_Load(object sender, EventArgs e)
        {
            if (Common.Globals.AboutList.Count == 0)
            {
                await Common.Classes.Information.LoadAboutAsync();
                await Task.Delay(3000);
            }

            MainPanel.Controls.Clear();
            foreach (var item in Common.Globals.AboutList)
            {
                LoadAbout(new UserControls.AboutTab(item.Title, item.Content));
            }
        }
    }
}

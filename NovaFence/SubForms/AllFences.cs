using NovaFence.UserControls;
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
    public partial class AllFences : Form
    {
        public AllFences()
        {
            InitializeComponent();
            Forms.Dashboard.UpdateFences += Dashboard_UpdateFences;
            MainPanel.ControlRemoved += MainPanel_ControlRemoved;
        }

        private void Dashboard_UpdateFences(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            var settings = Settings.FenceManager.LoadFences();
            foreach (var fence in settings)
            {
                OpenUserControl(new Fence(MainPanel, fence.FenceID));
            }

            label1.Text = $"All fences ({settings.Count})";
        }

        private void MainPanel_ControlRemoved(object sender, ControlEventArgs e)
        {
            label1.Text = $"All fences ({Settings.FenceManager.LoadFences().Count})";
        }

        private void OpenUserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Top;
            MainPanel.Controls.Add(userControl);
        }

        private void AllFences_Load(object sender, EventArgs e)
        {
            var settings = Settings.FenceManager.LoadFences();
            foreach (var fence in settings)
            {
                OpenUserControl(new Fence(MainPanel, fence.FenceID));
            }

            label1.Text = $"All fences ({settings.Count})";
        }
    }
}
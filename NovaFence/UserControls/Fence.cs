using NovaFence.Helpers;
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
    public partial class Fence : UserControl
    {
        public string ID { get; set; }
        public Panel MainPanel { get; set; }

        public Fence(Panel HostPanel, string id)
        {
            InitializeComponent();
            ID = id;
            MainPanel = HostPanel;

            Application.UpdateApplicationCount += Application_UpdateApplicationCount;
            Forms.FenceForm.FenceNewNameChnaged += FenceForm_FenceNewNameChnaged;
            Forms.FenceForm.ApplicationAdded += FenceForm_ApplicationAdded;

            var settings = Settings.FenceManager.LoadFences();
            var fence = settings.FirstOrDefault(x => x.FenceID == id);

            NameLBL.Text = fence.Title;
            appsLBL.Text = $"Applications: {fence.Applications.Count} | Opacity: {fence.Opacity}";
        }

        private void FenceForm_ApplicationAdded(object sender, string e)
        {
            if (this.ID == e)
            {
                var settings = Settings.FenceManager.LoadFences();
                var fence = settings.FirstOrDefault(x => x.FenceID == e);
                appsLBL.Text = $"Applications: {fence.Applications.Count} | Opacity: {fence.Opacity}";
            }
        }

        private void Application_UpdateApplicationCount(object sender, string e)
        {
            if (this.ID == e)
            {
                var settings = Settings.FenceManager.LoadFences();
                var fence = settings.FirstOrDefault(x => x.FenceID == e);
                appsLBL.Text = $"Applications: {fence.Applications.Count} | Opacity: {fence.Opacity}";
            }
        }

        private void FenceForm_FenceNewNameChnaged(object sender, FenceTitleChangedEventArgs e)
        {
            if (e.Id == ID)
            {
                this.NameLBL.Text = e.Text;
            }
        }

        private void DeleteFenceBTN_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show($"Are you sure you want to delete {NameLBL.Text} fence?", "NovaFence", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                var settings = Settings.FenceManager.LoadFences();
                var fence = settings.FirstOrDefault(x => x.FenceID == ID);

                settings.Remove(fence);
                Settings.FenceManager.SaveFences(settings);

                var fenceForm = Globals.Fences.FirstOrDefault(form => form.Name == ID);
                if (fenceForm != null || fenceForm != default)
                {
                    fenceForm.Close();
                    Globals.Fences.Remove(fenceForm);
                }

                ExtensionMethods.OpenAllFences(MainPanel);
            }
        }
    }
}
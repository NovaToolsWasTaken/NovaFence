using NovaFence.SubForms;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace NovaFence.Forms
{
    public partial class Dashboard : Form
    {
        public static event EventHandler UpdateFences;
        private readonly SettingsForm settingsForm = new SettingsForm();

        public Dashboard()
        {
            InitializeComponent();
            Elipse.BorderRadius = 13;

            var fences = Settings.FenceManager.LoadFences();
            foreach (var f in fences)
            {
                var form = new FenceForm(f.FenceID);
                form.Show();
                Globals.Fences.Add(form);
            }
        }

        private void OpenSubForm(Form form)
        {
            MainPanel.Controls.Clear();
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;

            MainPanel.Controls.Add(form);
            form.Show();
        }

        private async void LoadSettings()
        {
            var settings = Settings.Manager.LoadSettings();
            base.Size = settings.Size;

            if (settings.Location != default) 
                base.Location = settings.Location;

            if (settings.StayMinimized)
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                showDashboardToolStripMenuItem.Text = "Show Dashboard";
            }

            if (settings.CheckUpdates)
            {
                await Common.Classes.Update.CheckForUpdatesAsync();
            }
        }

        private async void Dashboard_Load(object sender, EventArgs e)
        {
            LoadSettings();
            OpenSubForm(new AllFences());

            await Common.Classes.Information.LoadAboutAsync();
        }

        private void ExitBTN_Click(object sender, EventArgs e)
        {
            var settings = Settings.Manager.LoadSettings();
            if (settings.Size != base.Size)
            {
                settings.Size = base.Size;
                settings.Location = base.Location;
            }

            Settings.Manager.SaveSettings(settings);
            Application.Exit();
        }

        private void AllFenechBTN_Click(object sender, EventArgs e)
        {
            OpenSubForm(new AllFences());
        }

        private void CreateNewBTN_Click(object sender, EventArgs e)
        {
            OpenSubForm(new CreateNew());
        }

        private void CustomizeBTN_Click(object sender, EventArgs e)
        {
            OpenSubForm(new Customize());
        }

        private void MaxBTN_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }
            else
            {
                WindowState = FormWindowState.Maximized;
            }
        }

        private void MinimzeBTN_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            IconClickMenu.Show(Control.MousePosition);
        }

        private void NewFenceBTN_Click(object sender, EventArgs e)
        {
            var settings = Settings.FenceManager.LoadFences();
            var fenceObj = new Settings.Models.Fence
            {
                Title = $"My fence ({settings.Count + 1})",
                Applications = new List<Settings.Models.Application>(),
                IsLocked = false,
                Opacity = 80,
                RoundedCorners = 5,
                Font = new Font("Segoe UI", 9)
            };

            Settings.FenceManager.AddFence(fenceObj);
            var fenceFrm = new FenceForm(fenceObj.FenceID);

            Globals.Fences.Add(fenceFrm);
            fenceFrm.Show();

            UpdateFences?.Invoke(this, EventArgs.Empty);
        }

        private async void CheckForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await Common.Classes.Update.CheckForUpdatesAsync(showResp: true);
        }

        private void ShowDashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                showDashboardToolStripMenuItem.Text = "Show Dashboard";
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
                showDashboardToolStripMenuItem.Text = "Hide Dashboard";
            }
        }

        private void OpenAboutBTN_Click(object sender, EventArgs e)
        {
            OpenSubForm(new About());
        }

        private void OpenSettignsBTN_Click(object sender, EventArgs e)
        {
            OpenSubForm(settingsForm);
        }
    }
}
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
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            var setting = Settings.Manager.LoadSettings();
            AutoStartSwitcvh.Checked = setting.AutoStart;
            UpdateCheck.Checked = setting.CheckUpdates;
            MinMewSwitch.Checked = setting.StayMinimized;
        }

        private void AutoStartSwitcvh_CheckedChanged(object sender, EventArgs e)
        {
            var settings = Settings.Manager.LoadSettings();
            settings.AutoStart = AutoStartSwitcvh.Checked;
            Settings.Manager.SaveSettings(settings);
            AutoStartStatusLBL.Text = AutoStartSwitcvh.Checked ? "On" : "Off";
            if (AutoStartSwitcvh.Checked)
            {
                Helpers.AutoRun.AddOnLoad();
            }
            else
            {
                Helpers.AutoRun.RemoveOnLoad();
            }
        }

        private void UpdateCheck_CheckedChanged(object sender, EventArgs e)
        {
            var settings = Settings.Manager.LoadSettings();
            settings.AutoStart = UpdateCheck.Checked;
            Settings.Manager.SaveSettings(settings);
            UpdateStatus.Text = UpdateCheck.Checked ? "On" : "Off";
        }

        private void MinMewSwitch_CheckedChanged(object sender, EventArgs e)
        {
            var settings = Settings.Manager.LoadSettings();
            settings.StayMinimized = MinMewSwitch.Checked;
            Settings.Manager.SaveSettings(settings);
            MinCheck.Text = MinMewSwitch.Checked ? "On" : "Off";
        }
    }
}
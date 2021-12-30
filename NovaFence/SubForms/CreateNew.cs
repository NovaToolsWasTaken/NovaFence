using NovaFence.Forms;
using NovaFence.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NovaFence.SubForms
{
    public partial class CreateNew : Form
    {
        private Color BannerColor = Color.FromArgb(15, 15, 15);
        private Color _BackColor = Color.FromArgb(20, 20, 20);
        private Font _Font = new Font("Segoe UI Semibold", 9, FontStyle.Bold);

        public CreateNew()
        {
            InitializeComponent();

            Globals.FenceApplications.Clear();
            MainPanel.Controls.Clear();
            MainPanel.ControlAdded += (_, e) =>
            {
                if (MainPanel.Controls.Count == 0) AppCountLBL.Text = "Applications";
                AppCountLBL.Text = $"Applications ({MainPanel.Controls.Count})";
            };
            MainPanel.ControlRemoved += (_, e) =>
            {
                if (MainPanel.Controls.Count == 0) AppCountLBL.Text = "Applications";
                else AppCountLBL.Text = $"Applications ({MainPanel.Controls.Count})";
            };
        }


        private void OpacitySilder_Scroll(object sender, EventArgs e)
        {
            OpacityValieLBL.Text = $"{OpacitySilder.Value}%";
        }

        private void CreateNew_Load(object sender, EventArgs e)
        {

        }

        private void AddAppBTN_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                ValidateNames = true,
                Title = "Add new application...",
                Multiselect = true,
                AutoUpgradeEnabled = true,
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foreach (var item in dialog.FileNames)
                {
                    if (Globals.FenceApplications.Any(x => x.Application.Link.Equals(item, StringComparison.OrdinalIgnoreCase)))
                    {
                        MessageBox.Show("This application already exists in this fence!", "NovaFence", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    MainPanel.OpenUserControl(new FenceApplication(MainPanel, Path.GetFileNameWithoutExtension(item), item, MainPanel.Controls.Count), true);
                }
            }
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FenceTitleTB.Text))
            {
                MessageBox.Show("Title cannot be nothing!", "NovaFence", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Globals.FenceApplications.Select(app => app.Application).Count() == 0)
            {
                MessageBox.Show("No applications added, min 1", "NovaFence", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var fenceObj = new Settings.Models.Fence
            {
                Title = FenceTitleTB.Text,
                Applications = Globals.FenceApplications.Select(app => app.Application).ToList(),
                BackColor = _BackColor,
                BannerColor = BannerColor,
                IsLocked = false,
                RoundedCorners = RounderSlider.Value,
                Font = _Font,
                Opacity = OpacitySilder.Value
            };

            Settings.FenceManager.AddFence(fenceObj);
            var fenceFrm = new FenceForm(fenceObj.FenceID);

            Globals.Fences.Add(fenceFrm);
            fenceFrm.Show();

            MessageBox.Show($"Fence ({FenceTitleTB.Text}) has been added", "NovaFence", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BannerColorBTN_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog()
            {
                FullOpen = true,
                AnyColor = true,
                AllowFullOpen = true,
                ShowHelp = true,
                Color = Color.FromArgb(15, 15, 15)
            };

            if (colorDialog.ShowDialog() == DialogResult.OK)
                BannerColor = colorDialog.Color;
        }

        private void BackColorBTN_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog()
            {
                FullOpen = true,
                AnyColor = true,
                AllowFullOpen = true,
                ShowHelp = true,
                Color = Color.FromArgb(20, 20, 20)
            };

            if (colorDialog.ShowDialog() == DialogResult.OK)
                _BackColor = colorDialog.Color;
        }

        private void RounderSlider_Scroll(object sender, EventArgs e)
        {
            RounderLBL.Text = RounderSlider.Value.ToString();
        }

        private void FontPickerBTN_Click(object sender, EventArgs e)
        {
            var fontPicker = new FontDialog();
            if (fontPicker.ShowDialog() == DialogResult.OK)
                _Font = fontPicker.Font;
        }
    }
}

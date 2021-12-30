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
    public partial class Customize : Form
    {
        //private bool IsLoading = true;
        public static event EventHandler<Helpers.FeneceEventArgs.UpdateType> UpdateFenceEvent;

        public Customize()
        {
            InitializeComponent();
        }

        private void Customize_Load(object sender, EventArgs e)
        {
            //StyleBox.DataSource = Enum.GetValues(typeof(Helpers.Styles));
            //IsLoading = false;
        }

        private void OpacitySilder_Scroll(object sender, EventArgs e)
        {
            OpacityValieLBL.Text = $"{OpacitySilder.Value}%";

            var fences = Settings.FenceManager.LoadFences();
            foreach (var fence in fences)
            {
                fence.Opacity = OpacitySilder.Value;
            }
            Settings.FenceManager.SaveFences(fences);

            UpdateFenceEvent?.Invoke(this, Helpers.FeneceEventArgs.UpdateType.Opacity);
        }

        private void RounderSlider_Scroll(object sender, EventArgs e)
        {
            RounderLBL.Text = $"{RounderSlider.Value}%";

            var fences = Settings.FenceManager.LoadFences();
            foreach (var fence in fences)
            {
                fence.RoundedCorners = RounderSlider.Value;
            }
            Settings.FenceManager.SaveFences(fences);

            UpdateFenceEvent?.Invoke(this, Helpers.FeneceEventArgs.UpdateType.RoundedCorners);
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
            {
                var fences = Settings.FenceManager.LoadFences();
                foreach (var fence in fences)
                {
                    fence.BannerColor = colorDialog.Color;
                }

                Settings.FenceManager.SaveFences(fences);
                UpdateFenceEvent?.Invoke(this, Helpers.FeneceEventArgs.UpdateType.BannerColor);
            }
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
            {
                var fences = Settings.FenceManager.LoadFences();
                foreach (var fence in fences)
                {
                    fence.BackColor = colorDialog.Color;
                }

                Settings.FenceManager.SaveFences(fences);
                UpdateFenceEvent?.Invoke(this, Helpers.FeneceEventArgs.UpdateType.BackColor);
            }
        }

        private void FontPickerBTN_Click(object sender, EventArgs e)
        {
            var fontPicker = new FontDialog();
            if (fontPicker.ShowDialog() == DialogResult.OK)
            {
                var fences = Settings.FenceManager.LoadFences();
                foreach (var fence in fences)
                {
                    fence.Font = fontPicker.Font;
                }

                Settings.FenceManager.SaveFences(fences);
                UpdateFenceEvent?.Invoke(this, Helpers.FeneceEventArgs.UpdateType.Font);
            }
        }

        private void StyleBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (IsLoading) return;


        }
    }
}

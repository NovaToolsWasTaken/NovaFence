using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web;
using System.Windows.Forms;

namespace NovaFence.UserControls
{
    public partial class FenceApplication : UserControl
    {
        public Settings.Models.Application Application { get; }
        public Panel Panel { get; }

        public FenceApplication(Panel hostPanel, string name, string path, int position)
        {
            InitializeComponent();

            Panel = hostPanel;
            Application = new Settings.Models.Application()
            {
                Name = name,
                Link = path,
                Position = position
            };

            // Check if file is an image
            if (MimeMapping.GetMimeMapping(path).StartsWith("image/"))
            {
                var bitmap = new Bitmap(path);
                pictureApp.Image = bitmap;
            }
            else
            {
                pictureApp.Image = Icon.ExtractAssociatedIcon(path).ToBitmap();
            }
            txtAppName.Text = name;
            txtAppPath.Text = path;
        }

        public void Swap<T>(IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;

            Panel.Controls.Clear();

            int position = 0;
            foreach (FenceApplication psc in Globals.FenceApplications)
            {
                psc.Application.Position = position;
                Panel.OpenUserControl(psc);
                position++;
            }
        }

        private void BtnUp_Click(object sender, EventArgs e)
        {
            try
            {
                Swap(Globals.FenceApplications, Application.Position, Application.Position + 1);
            }
            catch (Exception)
            {
                // ignored.
            }
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            try
            {
                Swap(Globals.FenceApplications, Application.Position, Application.Position - 1);
            }
            catch (Exception)
            {
                // ignored.
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Globals.FenceApplications.Remove(this);
                Panel.Controls.Remove(this);
            }
            catch (Exception)
            {

            }
        }
    }
}

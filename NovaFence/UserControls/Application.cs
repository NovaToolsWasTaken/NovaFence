using NovaFence.Forms;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Windows.Forms;

namespace NovaFence.UserControls
{
    public partial class Application : UserControl
    {
        public string Path { get; set; }
        public FenceForm Form { get; set; }
        internal static event EventHandler<string> UpdateApplicationCount;

        public Application(FenceForm form, string path)
        {
            InitializeComponent();
            Path = path;
            Form = form;
            label1.Text = System.IO.Path.GetFileNameWithoutExtension(path);

            if (MimeMapping.GetMimeMapping(path).StartsWith("image/"))
            {
                var bitmap = new Bitmap(path);
                this.guna2PictureBox1.Image = bitmap;
            }
            else
            {
                this.guna2PictureBox1.Image = Icon.ExtractAssociatedIcon(path).ToBitmap();
            }
        }

        private void Guna2PictureBox1_Click(object sender, System.EventArgs e)
        {
            try
            {
                Process.Start(Path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "NovaFence", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DeleteBTN_Click(object sender, EventArgs e)
        {
            var settings = Settings.FenceManager.LoadFences();
            var fence = settings.FirstOrDefault(x => x.FenceID == Form.Id);
            fence.Applications.Remove(fence.Applications.FirstOrDefault(y => y.Link == Path));

            Settings.FenceManager.SaveFences(settings);
            Form.LoadApps();

            UpdateApplicationCount?.Invoke(this, Form.Id);
        }
    }
}

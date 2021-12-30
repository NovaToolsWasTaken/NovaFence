using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;
using System;
using NovaFence.Helpers;
using System.IO;

namespace NovaFence.Forms
{
    public partial class FenceForm : Form
    {
        public string Id { get; set; }
        public int FormHeight { get; set; }
        internal static event EventHandler<FenceTitleChangedEventArgs> FenceNewNameChnaged;
        internal static event EventHandler<string> ApplicationAdded;

        public FenceForm(string id)
        {
            InitializeComponent();
            Id = id;
            this.ShowInTaskbar = false;
            this.Name = id;
            SubForms.Customize.UpdateFenceEvent += Customize_UpdateFenceEvent;


            var settings = Settings.FenceManager.LoadFences();
            var fence = settings.FirstOrDefault(x => x.FenceID == Id);
            LoadApps();

            if (fence.IsLocked)
            {
                this.DragControl.TargetControl = null;
                this.LockBTN.Text = "Unlock";
                this.ResizeBox.Visible = false;
            }
            else
            {
                this.DragControl.TargetControl = this.label1;
            }
            this.ResizeBox.BringToFront();

            this.label1.Text = fence.Title;
            this.label1.BackColor = fence.BannerColor;
            this.Size = fence.Size;
            this.NewNameTB.FillColor = fence.BannerColor;
            this.NewNameTB.Font = fence.Font;
            this.Location = fence.Location;
            this.BackColor = fence.BackColor;
            this.label1.Font = fence.Font;
            this.Elipse.BorderRadius = fence.RoundedCorners;

            if (fence.Opacity == 100 || fence.Opacity == 1) this.Opacity = 1.00;
            else this.Opacity = double.Parse($"0.{fence.Opacity}");
            this.FormHeight = fence.Size.Height;
        }

        public void LoadApps()
        {
            FlowPanel.Controls.Clear();
            var settings = Settings.FenceManager.LoadFences();
            var fence = settings.FirstOrDefault(x => x.FenceID == Id);
            foreach (var item in fence.Applications.OrderByDescending(x => x.Position))
            {
                FlowPanel.Controls.Add(new UserControls.Application(this, item.Link));
            }
        }

        private void Customize_UpdateFenceEvent(object sender, Helpers.FeneceEventArgs.UpdateType e)
        {
            var settings = Settings.FenceManager.LoadFences();
            var fence = settings.FirstOrDefault(x => x.FenceID == Id);

            switch (e)
            {
                case Helpers.FeneceEventArgs.UpdateType.RoundedCorners:
                    if (Elipse.TargetControl == null)
                    {
                        this.DragControl.TargetControl = this.label1;
                        this.Elipse.BorderRadius = fence.RoundedCorners;
                        this.DragControl.TargetControl = null;
                    }
                    else
                    {
                        this.Elipse.BorderRadius = fence.RoundedCorners;
                    }
                    break;

                case Helpers.FeneceEventArgs.UpdateType.Opacity:
                    if (fence.Opacity == 100 || fence.Opacity == 1) this.Opacity = 1.00;
                    else this.Opacity = double.Parse($"0.{fence.Opacity}");
                    break;

                case Helpers.FeneceEventArgs.UpdateType.BannerColor:
                    this.NewNameTB.FillColor = fence.BannerColor;
                    this.label1.BackColor = fence.BannerColor;
                    break;

                case Helpers.FeneceEventArgs.UpdateType.BackColor:
                    this.BackColor = fence.BackColor;
                    break;

                case Helpers.FeneceEventArgs.UpdateType.Font:
                    this.NewNameTB.Font = fence.Font;
                    this.label1.Font = fence.Font;
                    break;

                default:
                    MessageBox.Show($"Type {e} unknow, report this to support team", "NovaFence"); // Prob never happen lol
                    break;
            }
        }

        private void LockBTN_Click(object sender, System.EventArgs e)
        {
            var settings = Settings.FenceManager.LoadFences();
            var fence = settings.FirstOrDefault(x => x.FenceID == Id);

            if (this.LockBTN.Text == "Lock")
            {
                this.DragControl.TargetControl = null;
                this.LockBTN.Text = "Unlock";
                fence.IsLocked = true;
                this.ResizeBox.Visible = false;
            }
            else
            {
                this.DragControl.TargetControl = this.label1;
                this.LockBTN.Text = "Lock";
                fence.IsLocked = false;
                this.ResizeBox.Visible = true;
            }
            Settings.FenceManager.SaveFences(settings);
        }

        private void FenceForm_LocationChanged(object sender, System.EventArgs e)
        {
            var settings = Settings.FenceManager.LoadFences();
            var fence = settings.FirstOrDefault(x => x.FenceID == Id);

            fence.Size = this.Size;
            fence.Location = this.Location;
            Settings.FenceManager.SaveFences(settings);
        }

        private void FenceForm_SizeChanged(object sender, System.EventArgs e)
        {
            var settings = Settings.FenceManager.LoadFences();
            var fence = settings.FirstOrDefault(x => x.FenceID == Id);

            fence.Size = this.Size;
            fence.Location = this.Location;
            Settings.FenceManager.SaveFences(settings);
        }


        private void Label1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.NewNameTB.Text = this.label1.Text;
            this.NewNameTB.Visible = true;
            this.NewNameTB.SelectAll();
            this.NewNameTB.Focus(); 
        }

        private void NewNameTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(this.NewNameTB.Text))
                {
                    this.NewNameTB.Visible = false;
                    return;
                }

                this.NewNameTB.Visible = false;
                this.label1.Text = NewNameTB.Text;

                var settings = Settings.FenceManager.LoadFences();
                var fence = settings.FirstOrDefault(x => x.FenceID == Id);
                fence.Title = NewNameTB.Text;
                Settings.FenceManager.SaveFences(settings);

                var args = new FenceTitleChangedEventArgs
                {
                    Id = Id,
                    Text = NewNameTB.Text
                };
                FenceNewNameChnaged?.Invoke(this, args);
            }
        }

        private void NewNameTB_MouseLeave(object sender, EventArgs e)
        {
            this.NewNameTB.Visible = false;
        }

        private void FlowPanel_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                var data = e.Data.GetData(DataFormats.FileDrop);
                if (data != null)
                {
                    var fileNames = data as string[];
                    var settings = Settings.FenceManager.LoadFences();
                    var fence = settings.FirstOrDefault(x => x.FenceID == Id);

                    if (fence.Applications.Any(x => x.Link == fileNames.FirstOrDefault()))
                    {
                        MessageBox.Show("Application already exists in this fence!", "NovaFence", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    int pos = 0;
                    if (fence.Applications.Count != 0)
                    {
                        pos = fence.Applications.Last().Position + 1;
                    }

                    fence.Applications.Add(new Settings.Models.Application
                    {
                        Name = Path.GetFileNameWithoutExtension(fileNames[0]),
                        Link = fileNames[0],
                        Position = pos
                    });

                    FlowPanel.Controls.Add(new UserControls.Application(this, fileNames[0]));
                    Settings.FenceManager.SaveFences(settings);

                    ApplicationAdded?.Invoke(this, Id);
                }
                else
                {
                    MessageBox.Show("DragDrop file is null!", "NovaFence", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception: {ex.Message}", "NovaFence", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FlowPanel_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
    }
}
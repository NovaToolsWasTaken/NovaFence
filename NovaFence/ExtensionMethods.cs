using NovaFence.UserControls;
using System.Windows.Forms;

namespace NovaFence
{
    public static class ExtensionMethods
    {
        public static void OpenUserControl(this Panel panel, FenceApplication userControl, bool addToGlobal = false)
        {
            userControl.Dock = DockStyle.Top;
            panel.Controls.Add(userControl);
            if (addToGlobal) Globals.FenceApplications.Add(userControl);
        }

        public static void OpenAllFences(this Panel panel)
        {
            panel.Controls.Clear();

            foreach (var fence in Settings.FenceManager.LoadFences())
            {
                OpenUC(new Fence(panel, fence.FenceID), panel);
            }
        }

        private static void OpenUC(UserControl userControl, Panel panel)
        {
            userControl.Dock = DockStyle.Top;
            panel.Controls.Add(userControl);
        }
    }
}

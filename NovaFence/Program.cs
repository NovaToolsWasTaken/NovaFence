using System;
using System.Linq;
using System.Windows.Forms;

namespace NovaFence
{
    internal static class Program
    {
        private static readonly string ErrorMessage = "Oops, the application just crashed and threw an exception. Please report this to the support team.\n\nWould you like to copy an exception and exit?";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
            {
                MessageBox.Show("An instance of NovaFence is already running, more than one will cause some issues with managing fences.",
                    "NovaFence", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Environment.Exit(0);
            }

            Settings.FenceManager.CheckFenceFiles();
            Settings.Manager.CheckFiles();
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Application.Run(new Forms.Dashboard());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            DialogResult result = MessageBox.Show(ErrorMessage, "Nova Fences - Unhandled Exception", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (result == DialogResult.Yes)
            {
                var resp = e.ExceptionObject as Exception;
                Clipboard.SetText($"Message: {resp.Message}\nStackTrace: {resp.StackTrace}");
            }
            Application.Exit();
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            DialogResult result = MessageBox.Show(ErrorMessage, "Nova Fences - Thread Exception", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (result == DialogResult.Yes)
            {
                var resp = e.Exception;
                Clipboard.SetText($"Message: {resp.Message}\nStackTrace: {resp.StackTrace}");
            }
            Application.Exit();
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NovaFence.Common.Classes
{
    public static class Update
    {
        private static readonly string _version = "1.0.0.0";

        public static async Task CheckForUpdatesAsync(bool showResp = false)
        {
#if DEBUG
            return;
# else
            try
            {
                var resp = await Globals.client.GetAsync("https://novatools.xyz/api/fences/update");
                string content = await resp.Content.ReadAsStringAsync();

                if (!resp.IsSuccessStatusCode || string.IsNullOrWhiteSpace(content))
                {
                    MessageBox.Show($"Update API returned bad status code ({(int)resp.StatusCode}) or response was null" , "NovaFence", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var json = JsonConvert.DeserializeObject<Models.Update>(content);

                if (showResp && _version == json.Version)
                {
                    MessageBox.Show("Your up to date!", "NovaFence", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (_version != json.Version)
                {
                    DialogResult dialog = MessageBox.Show("New update found, do you want to download now?", "NovaFence", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dialog == DialogResult.Yes)
                    {
                        Process.Start("https://novatools.xyz/download");
                        Environment.Exit(0);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
#endif
        }
    }
}

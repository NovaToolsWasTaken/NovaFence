using Newtonsoft.Json;
using NovaFence.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NovaFence.Common.Classes
{
    public static class Information
    {
        public static async Task LoadAboutAsync()
        {
            try
            {
                var resp = await Globals.client.GetAsync("https://novatools.xyz/api/fences/about");
                string content = await resp.Content.ReadAsStringAsync();

                if (!resp.IsSuccessStatusCode || string.IsNullOrWhiteSpace(content))
                {
                    Globals.AboutList.Clear();
                    System.Windows.Forms.MessageBox.Show("About API returned bad status code or response was null", "NovaFence", 
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }

                if (Globals.AboutList.Count != 0)
                {
                    Globals.AboutList.Clear();
                }

                Globals.AboutList = JsonConvert.DeserializeObject<List<About>>(content);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
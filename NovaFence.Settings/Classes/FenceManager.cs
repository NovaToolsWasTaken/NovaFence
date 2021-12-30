using Newtonsoft.Json;
using NovaFence.Settings.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NovaFence.Settings
{
    public class FenceManager
    {
        private static readonly string dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "NovaTools", "NovaFence");
        private static readonly string settingsPath = Path.Combine(dataPath, "fences.txt");

        public static List<Fence> LoadFences()
        {
            var text = Base64.Decode(File.ReadAllText(settingsPath));
            return JsonConvert.DeserializeObject<List<Fence>>(text);
        }

        public static void SaveFences(List<Fence> fences)
        {
            var json = JsonConvert.SerializeObject(fences);
            File.WriteAllText(settingsPath, Base64.Encode(json));
        }

        public static void AddFence(Fence fence)
        {
            var text = Base64.Decode(File.ReadAllText(settingsPath));
            var all = JsonConvert.DeserializeObject<List<Fence>>(text);
            all.Add(fence);

            File.WriteAllText(settingsPath, Base64.Encode(JsonConvert.SerializeObject(all)));
        }

        public static void CheckFenceFiles()
        {
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

            if (!File.Exists(settingsPath))
            {
                var json = JsonConvert.SerializeObject(new List<Fence>());
                File.WriteAllText(settingsPath, Base64.Encode(json));
            }
            else
            {
                try
                {
                    var text = Base64.Decode(File.ReadAllText(settingsPath));
                    JsonConvert.DeserializeObject<List<Fence>>(text);
                }
                catch
                {
                    MessageBox.Show("Invalid fences settings, restating all!", "NovaFence", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    var json = JsonConvert.SerializeObject(new List<Fence>());
                    File.WriteAllText(settingsPath, Base64.Encode(json));
                }
            }
        }
    }
}
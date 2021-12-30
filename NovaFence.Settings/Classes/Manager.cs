using Newtonsoft.Json;
using NovaFence.Settings.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace NovaFence.Settings
{
    public static class Manager
    {
        private static readonly string dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "NovaTools", "NovaFence");
        private static readonly string settingsPath = Path.Combine(dataPath, "settings.txt");

        public static void SaveSettings(ApplicationSettings applicationSettings)
        {
            var json = JsonConvert.SerializeObject(applicationSettings);
            File.WriteAllText(settingsPath, Base64.Encode(json));
        }

        public static ApplicationSettings LoadSettings()
        {
            var text = Base64.Decode(File.ReadAllText(settingsPath));
            var models = JsonConvert.DeserializeObject<ApplicationSettings>(text);

            return models;
        }

        public static void CheckFiles()
        {
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

            if (!File.Exists(settingsPath))
            {
                var json = JsonConvert.SerializeObject(new ApplicationSettings());
                File.WriteAllText(settingsPath, Base64.Encode(json));
            }
            else
            {
                try
                {
                    var text = Base64.Decode(File.ReadAllText(settingsPath));
                    JsonConvert.DeserializeObject<ApplicationSettings>(text);
                }
                catch
                {
                    MessageBox.Show("Invalid application settings, restating all!", "NovaFence", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    var json = JsonConvert.SerializeObject(new ApplicationSettings());
                    File.WriteAllText(settingsPath, Base64.Encode(json));
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace CarrierJumpNotification
{
    static class GlobalSettings
    {
        public static string EliteFolderPath { get; set; }
        public static string NotificationPattern { get; set; }

        public static void InitDefault()
        {
            NotificationPattern = "FC <fc_name> (<fc_id>) is departing to <target_system> from <current_system> in <jump_time> minutes. \nLockdown in <lockdown_time> minutes.";
            EliteFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Saved Games\Frontier Developments\Elite Dangerous";

            if (!Directory.Exists(EliteFolderPath))
                EliteFolderPath = string.Empty;
        }

        public static void InitFromFile(string path)
        {
            if(!File.Exists(path))
            {
                InitDefault();
                return;
            }

            try
            {
                string config = File.ReadAllText(path);
                Settings newConfig = JsonConvert.DeserializeObject<Settings>(config);

                EliteFolderPath = newConfig.EliteFolderPath;
                NotificationPattern = newConfig.NotificationPattern;
            }
            catch 
            {
                InitDefault();
            }
        }

        public static void SaveSettings(string path)
        {
            Settings toSave = new Settings();
            toSave.EliteFolderPath = EliteFolderPath;
            toSave.NotificationPattern = NotificationPattern;

            try
            {
                string config = JsonConvert.SerializeObject(toSave);
                File.WriteAllText(path, config);
            }
            catch { }
        }

        private class Settings
        {
            public string EliteFolderPath { get; set; }
            public string NotificationPattern { get; set; }
        }
    }
}

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
        public static bool CutColSystem = true;
        public static bool ExtendedSearch = true;
        public static bool AutoClipboard = true;
        public static double UiColorIndex = 0;

        public static void InitDefault()
        {
            NotificationPattern = "<fc_name> (<fc_id>) is departing to <target_system> from <current_system>. Lockdown in <lockdown_time> minutes.";
            EliteFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Saved Games\Frontier Developments\Elite Dangerous";
            CutColSystem = true;
            ExtendedSearch = true;
            AutoClipboard = true;

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
                CutColSystem = newConfig.CutColSystem;
                ExtendedSearch = newConfig.ExtendedSearch;
                UiColorIndex = newConfig.UiColorIndex;
                AutoClipboard = newConfig.AutoClipboard;
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
            toSave.CutColSystem = CutColSystem;
            toSave.ExtendedSearch = ExtendedSearch;
            toSave.UiColorIndex = UiColorIndex;
            toSave.AutoClipboard = AutoClipboard;

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
            public bool CutColSystem { get; set; }
            public bool ExtendedSearch { get; set; }
            public bool AutoClipboard { get; set; }
            public double UiColorIndex { get; set; }
        }
    }
}

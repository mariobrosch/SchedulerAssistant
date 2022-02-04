using System.Collections.Generic;
using System.Linq;

namespace SchedulerAssistant.Data.Settings
{
    public static class Logic
    {
        private static readonly List<Model> Settings = new();
        public static List<Model> GetSettings()
        {
            return Settings;
        }

        public static string GetSetting(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return "";
            }
            return Settings.FirstOrDefault(s => s.Key == key)?.Value ?? "";
        }

        public static void WriteSettings(List<Model> newSettings)
        {
            Settings.Clear();
            foreach (Model setting in newSettings)
            {
                Settings.Add(setting);
            }
        }

        public static void AddSetting(Model setting)
        {
            Settings.Add(setting);
        }

        public static void UpdateSetting(Model newSetting)
        {
            foreach (Model setting in Settings)
            {
                if (setting.Key == newSetting.Key)
                {
                    setting.Value = newSetting.Value;
                }
            }
        }
    }
}

using SchedulerAssistant.Data.Data;
using SchedulerAssistant.Data.Models;
using SchedulerAssistant.Data.Requests;
using System.Collections.Generic;
using System.Linq;

namespace SchedulerAssistant.Data.Helpers
{
    public static class SettingsHelper
    {
        public static Setting? GetSettingByKey(string key)
        {
            FilterObject filterObject = new()
            {
                Column = "Key",
                Value = key
            };

            return SettingData.Get(filterObject).FirstOrDefault();
        }

        public static void Create(string key, string value)
        {
            Setting setting = new()
            {
                Key = key,
                Value = value
            };
            _ = SettingData.Create(setting);
        }

        public static List<Setting> GetDefaultSettings()
        {
            List<Setting> defaultSettings = new();
            Setting languageSetting = new()
            {
                Key = "Language",
                Value = "Dutch",
                PossibleValues = "Dutch"
            };
            defaultSettings.Add(languageSetting);

            return defaultSettings;
        }

        public static void CreateDefaultSettings()
        {
            foreach (Setting setting in GetDefaultSettings())
            {
                if (setting != null && setting.Key != null && setting.Value != null)
                {
                    SettingData.CreateDefault(setting.Key, setting.Value);
                }
            }
        }
    }
}

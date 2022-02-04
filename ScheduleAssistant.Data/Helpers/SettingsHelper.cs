using System.Collections.Generic;
using System.Linq;

namespace ScheduleAssistant.Data.Helpers
{
    public static class SettingsHelper
    {
        public static Setting GetSettingByKey(string key)
        {
            FilterObject filterObject = new FilterObject()
            {
                Column = "Key",
                Value = key
            };

            return SettingData.Get(filterObject).FirstOrDefault();
        }

        public static void Create(string key, string value)
        {
            Setting setting = new Setting()
            {
                Key = key,
                Value = value
            };
            _ = SettingData.Create(setting);
        }

        public static List<Setting> GetDefaultSettings()
        {
            List<Setting> defaultSettings = new List<Setting>();
            Setting languageSetting = new Setting()
            {
                Key = "Language",
                Value = "Dutch",
                PossibleValues = "Dutch, English"
            };
            defaultSettings.Add(languageSetting);

            return defaultSettings;
        }

        public static void CreateDefaultSettings()
        {
            foreach (Setting setting in GetDefaultSettings())
            {
                SettingData.CreateDefault(setting.Key, setting.Value);
            }
        }
    }
}

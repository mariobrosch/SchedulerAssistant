using Newtonsoft.Json;
using SchedulerAssistant.Data.Settings;
using System.Collections.Generic;
using System.IO;

namespace SchedulerAssistant.Helpers
{
    internal class Startup
    {
        internal static void Perform()
        {
            Logic.WriteSettings(GetLocalSettings());
        }

        private static List<Model> GetLocalSettings()
        {
            List<Model> settings = new();
            Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(".\\schedulerAssistant.json")) ?? new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> kv in dict)
            {
                Model setting = new()
                {
                    Key = kv.Key,
                    Value = kv.Value
                };
                settings.Add(setting);
            }
            return settings;
        }
    }
}

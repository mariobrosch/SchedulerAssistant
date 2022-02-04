using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using SchedulerAssistant.Data.Data;
using SchedulerAssistant.Data.Models;
using ProjectRegistration.Data.Data;
using SchedulerAssistant.Data.Enums;
using SchedulerAssistant.Data.Helpers;

namespace SchedulerAssistant.Data.Requests
{
    public static class SettingData
    {
        private const string tableName = "Settings";

        public static List<Setting> Get()
        {
            return Get("", "");
        }

        public static Setting Get(int key)
        {
            return Get(key.ToString()).First();
        }

        public static List<Setting> Get(FilterObject filter)
        {
            return Get("", Filter.CreateFilter(filter));
        }

        public static List<Setting> Get(string key, string filter = "")
        {
            string settings = GetData(key, filter);

            if (string.IsNullOrEmpty(settings))
            {
                return new List<Setting>();
            }

            return JsonConvert.DeserializeObject<List<Setting>>(settings) ?? new List<Setting>();
        }

        private static string GetData(int key, string filter = "")
        {
            return GetData(key.ToString(), filter);
        }

        private static string GetData(string key, string filter = "")
        {
            return RequestHandler.MakeRequest(HttpMethods.GET, tableName, key, filter);
        }

        public static int Delete(string key, string filter = "")
        {
            string numberOfRowsDeleted = RequestHandler.MakeRequest(HttpMethods.DELETE, tableName, key, filter);
            return int.Parse(numberOfRowsDeleted);
        }

        public static Setting Create(Setting settings)
        {
            string data = JsonConvert.SerializeObject(settings);
            string settingsId = RequestHandler.MakeRequest(HttpMethods.POST, tableName, "", "", data);
            string newSettings = GetData(settingsId);

            var returnValue = JsonConvert.DeserializeObject<List<Setting>>(newSettings) ?? new List<Setting>();
            return returnValue.First();
        }

        public static bool Update(Setting settings)
        {
            string data = JsonConvert.SerializeObject(settings);
            return RequestHandler.MakeRequest(HttpMethods.PUT, tableName, settings.Id, "", data) != "0";
        }

        public static bool Update(int settingsId, string property, string value)
        {
            var rawSettings = JsonConvert.DeserializeObject<List<Setting>>(GetData(settingsId)) ?? new List<Setting>();
            Setting settings = rawSettings.First();
            settings.SetProperty(property, value);
            return Update(settings);
        }

        public static void CreateDefault(string key, string value)
        {
            Setting? setting = SettingsHelper.GetSettingByKey(key);
            if (setting == null)
            {
                SettingsHelper.Create(key, value);
            }
        }
    }
}

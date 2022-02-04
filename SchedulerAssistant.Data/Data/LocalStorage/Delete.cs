using Newtonsoft.Json;
using System.Collections.Generic;
using SchedulerAssistant.Data.Models;

namespace SchedulerAssistant.Data.Data.LocalStorage
{
    internal class Delete
    {
        private const string defaultReturnValue = "";
        internal static string Perform(string fileContent, string table, string key, string filter, out string removeCount)
        {
            removeCount = "0";

            if (string.IsNullOrEmpty(key) && string.IsNullOrEmpty(filter))
            {
                return fileContent;
            }

            dynamic allEntries;

            switch (table)
            {
                case "Contacts":
                    allEntries = JsonConvert.DeserializeObject<List<Contact>>(fileContent) ?? new List<Contact>();
                    break;
                case "Settings":
                    allEntries = JsonConvert.DeserializeObject<List<Setting>>(fileContent) ?? new List<Setting>();
                    break;
                default:
                    return defaultReturnValue;
            }

            FilterObject filterObject = Filter.Deserialize(filter);
            List<dynamic> returnList = new();

            foreach (dynamic entry in allEntries)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    if (entry.Id.ToString() != key)
                    {
                        returnList.Add(entry);
                    }
                }
                else
                {
                    dynamic value = entry.GetType().GetProperty(filterObject.Column).GetValue(entry, null);
                    if (value.ToString() != filterObject.Value)
                    {
                        returnList.Add(entry);
                    }
                }
            }
            removeCount = (allEntries.Count - returnList.Count).ToString();
            return JsonConvert.SerializeObject(returnList);
        }

    }
}

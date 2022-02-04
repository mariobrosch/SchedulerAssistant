using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using SchedulerAssistant.Data.Models;

namespace SchedulerAssistant.Data.Data.LocalStorage
{
    internal class Get
    {
        private const string defaultReturnValue = "[]";

        internal static string Perform(string fileContent, string table, string key, string filter)
        {
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

            if (allEntries.Count == 0)
            {
                return defaultReturnValue;
            }

            if (!string.IsNullOrEmpty(key))
            {
                return table switch
                {
                    "Contacts" => DataParser.SaveListToJsonString(((List<Contact>)allEntries).Where(x => x.Id == int.Parse(key)).ToList()),
                    "Settings" => DataParser.SaveListToJsonString(((List<Setting>)allEntries).Where(x => x.Id == int.Parse(key)).ToList()),
                    _ => defaultReturnValue,
                };
            }

            FilterObject filterObject = Filter.Deserialize(filter);
            List<dynamic> entrySelection = new();

            foreach (dynamic entry in allEntries)
            {
                dynamic value = entry.GetType().GetProperty(filterObject.Column).GetValue(entry, null);
                if (filterObject.Value == "ISNULL" && value == null)
                {
                    entrySelection.Add(entry);
                }
                else if (value?.ToString() == filterObject.Value)
                {
                    entrySelection.Add(entry);
                }
            }

            return JsonConvert.SerializeObject(entrySelection);
        }

    }
}

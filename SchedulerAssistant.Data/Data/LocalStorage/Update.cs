using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using SchedulerAssistant.Data.Models;

namespace SchedulerAssistant.Data.Data.LocalStorage
{
    internal class Update
    {
        private const string defaultReturnValue = "";

        internal static string Perform(string fileContent, string table, string key, string data, out string isUpdated)
        {
            isUpdated = "0";
            dynamic allEntries;

            if (string.IsNullOrEmpty(key))
            {
                return defaultReturnValue;
            }

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

            dynamic? entryToUpdate;

            if (allEntries.Count == 0)
            {
                return defaultReturnValue;
            }

            switch (table)
            {
                case "Contacts":
                    entryToUpdate = ((List<Contact>)allEntries).FirstOrDefault(x => x.Id == int.Parse(key));
                    break;
                case "Settings":
                    entryToUpdate = ((List<Setting>)allEntries).FirstOrDefault(x => x.Id == int.Parse(key));
                    break;
                default:
                    return defaultReturnValue;
            }

            if (entryToUpdate == null || entryToUpdate?.Id == null)
            {
                return defaultReturnValue;
            }

            dynamic index = allEntries.IndexOf(entryToUpdate);

            if (index != -1)
            {
                isUpdated = "1";
                dynamic? updatedEntry;
                switch (table)
                {
                    case "Contacts":
                        updatedEntry = JsonConvert.DeserializeObject<Contact>(data);
                        break;
                    case "Settings":
                        updatedEntry = JsonConvert.DeserializeObject<Setting>(data);
                        break;
                    default:
                        return defaultReturnValue;
                }
                allEntries[index] = updatedEntry;
                return DataParser.SaveListToJsonString(allEntries);
            }
            return defaultReturnValue;
        }
    }
}

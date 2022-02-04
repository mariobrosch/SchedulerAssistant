using Newtonsoft.Json;
using SchedulerAssistant.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace SchedulerAssistant.Data.Data.LocalStorage
{
    internal class Create
    {
        private const string defaultReturnValue = "";

        internal static string Perform(string fileContent, string table, string data, out string newId)
        {
            newId = "0";
            dynamic? allEntries;

            if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty(fileContent))
            {
                return defaultReturnValue;
            }

            switch (table)
            {
                case "Contacts":
                    allEntries = JsonConvert.DeserializeObject<List<Contact>>(fileContent);
                    break;
                case "Settings":
                    allEntries = JsonConvert.DeserializeObject<List<Setting>>(fileContent);
                    break;
                default:
                    return defaultReturnValue;
            }

            int newEntryId = 1;
            if (allEntries?.Count > 0)
            {
                switch (table)
                {
                    case "Contacts":
                        newEntryId = ((List<Contact>)allEntries).OrderBy(x => x.Id).Last().Id + 1;
                        break;
                    case "Settings":
                        newEntryId = ((List<Setting>)allEntries).OrderBy(x => x.Id).Last().Id + 1;
                        break;
                    default:
                        return defaultReturnValue;
                }
            }

            dynamic? newEntry;
            switch (table)
            {
                case "Contacts":
                    newEntry = JsonConvert.DeserializeObject<Contact>(data);
                    if (newEntry != null)
                    {
                        newEntry.Id = newEntryId;
                    }
                    break;
                case "Settings":
                    newEntry = JsonConvert.DeserializeObject<Setting>(data);
                    if (newEntry != null)
                    {
                        newEntry.Id = newEntryId;
                    }
                    break;
                default:
                    return defaultReturnValue;
            }
            if (allEntries != null)
            {
                allEntries.Add(newEntry);
            }
            newId = newEntryId.ToString();
            return JsonConvert.SerializeObject(allEntries);
        }
    }
}

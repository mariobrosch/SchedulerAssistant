using SchedulerAssistant.Data.Enums;
using System.IO;

namespace SchedulerAssistant.Data.Data.LocalStorage
{
    internal static class LocalStorage
    {
        private static readonly string localStoragePath = Settings.Logic.GetSetting("localStoragePath");

        internal static string MakeRequest(HttpMethods method, string table, string key, string filter, string data)
        {

            string filePath = Path.Combine(localStoragePath, table + ".json");
            if (!File.Exists(filePath))
            {
                if (!Directory.Exists(localStoragePath))
                {
                    _ = Directory.CreateDirectory(localStoragePath);
                }
                File.WriteAllText(filePath, "[]");
            }
            string fileContent = File.ReadAllText(filePath);

            switch (method)
            {
                case HttpMethods.GET:
                    return Get.Perform(fileContent, table, key, filter);
                case HttpMethods.POST:
                    string newId;
                    string newPostContent = Create.Perform(fileContent, table, data, out newId);
                    File.WriteAllText(filePath, newPostContent);
                    return newId;
                case HttpMethods.PUT:
                    string isUpdated;
                    string newPutContent = Update.Perform(fileContent, table, key, data, out isUpdated);
                    File.WriteAllText(filePath, newPutContent);
                    return isUpdated;
                case HttpMethods.DELETE:
                    string removeCount;
                    string newDeleteContent = Delete.Perform(fileContent, table, key, filter, out removeCount);
                    File.WriteAllText(filePath, newDeleteContent);
                    return removeCount;
                default:
                    break;
            }
            return "";
        }
    }
}

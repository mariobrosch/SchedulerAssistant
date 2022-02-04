using SchedulerAssistant.Data.Data.LocalStorage;
using SchedulerAssistant.Data.Enums;
using SchedulerAssistant.Data.Settings;

namespace ProjectRegistration.Data.Data
{
    public static class RequestHandler
    {
        private static readonly string dataLocation = Logic.GetSetting("dataLocation");

        public static string MakeRequest(HttpMethods method, string table, string key, string filter, string data = "")
        {
            switch (dataLocation)
            {
                case "local":
                    return LocalStorage.MakeRequest(method, table, key, filter, data);
                default:
                    break;
            }
            return "";
        }

        public static string MakeRequest(HttpMethods method, string table, int key, string filter, string data = "")
        {
            return MakeRequest(method, table, key.ToString(), filter, data);
        }
    }
}

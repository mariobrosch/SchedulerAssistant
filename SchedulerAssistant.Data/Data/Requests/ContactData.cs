using Newtonsoft.Json;
using ProjectRegistration.Data.Data;
using SchedulerAssistant.Data.Enums;
using SchedulerAssistant.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace SchedulerAssistant.Data.Data.Requests
{
    public static class ContactData
    {
        private const string tableName = "Contacts";

        public static List<Contact> Get(bool displayRemoved = false)
        {
            return Get("", "", displayRemoved);
        }

        public static Contact Get(int key)
        {
            return Get(key.ToString(), "", true).First();
        }

        public static List<Contact> Get(FilterObject filter)
        {
            return Get("", Filter.CreateFilter(filter), true);
        }

        public static List<Contact> Get(string key, string filter = "", bool displayRemoved = false)
        {
            string players = GetData(key, filter);
            var playerList = JsonConvert.DeserializeObject<List<Contact>>(players);
            
            if (playerList == null)
            {
                return new List<Contact>();
            }
            return FilterRemoved(playerList, displayRemoved);
        }

        private static string GetData(int key, string filter = "")
        {
            return GetData(key.ToString(), filter);
        }

        private static string GetData(string key, string filter = "")
        {
            return RequestHandler.MakeRequest(HttpMethods.GET, tableName, key, filter);
        }

        public static int Delete(FilterObject filter)
        {
            return Delete("", Filter.CreateFilter(filter));
        }

        public static int Delete(string key, string filter = "")
        {
            string numberOfRowsDeleted = RequestHandler.MakeRequest(HttpMethods.DELETE, tableName, key, filter);
            return int.Parse(numberOfRowsDeleted);
        }

        public static Contact? Create(Contact player)
        {
            string data = JsonConvert.SerializeObject(player);
            string playerId = RequestHandler.MakeRequest(HttpMethods.POST, tableName, "", "", data);
            string newPlayer = GetData(playerId);
            return JsonConvert.DeserializeObject<List<Contact>>(newPlayer)?.First();
        }

        public static bool Update(Contact player)
        {
            string data = JsonConvert.SerializeObject(player);
            return RequestHandler.MakeRequest(HttpMethods.PUT, tableName, player.Id, "", data) != "0";
        }

        public static bool Update(int playerId, string property, string value)
        {
            var rawPlayer = GetData(playerId);
            if (rawPlayer == null)
            {
                return false;
            }
            Contact player = JsonConvert.DeserializeObject<List<Contact>>(rawPlayer)?.First() ?? new Contact();
            player.SetProperty(property, value);
            return Update(player);
        }

        private static List<Contact> FilterRemoved(List<Contact> players, bool displayRemoved)
        {
            if (players == null || players.Count == 0 || displayRemoved)
            {
                return players ?? new List<Contact>();
            }
            return players.Where(p => p.IsRemoved == false).ToList();
        }
    }
}

using Newtonsoft.Json;
using ProjectRegistration.Data.Data;
using SchedulerAssistant.Data.Enums;
using SchedulerAssistant.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace SchedulerAssistant.Data.Data.Requests
{
    public static class EventData
    {
        private const string tableName = "Events";

        public static List<Event> Get(bool displayRemoved = false)
        {
            return Get("", "", displayRemoved);
        }

        public static Event Get(int key)
        {
            return Get(key.ToString(), "", true).First();
        }

        public static List<Event> Get(FilterObject filter)
        {
            return Get("", Filter.CreateFilter(filter), true);
        }

        public static List<Event> Get(string key, string filter = "", bool displayRemoved = false)
        {
            string events = GetData(key, filter);
            var eventList = JsonConvert.DeserializeObject<List<Event>>(events);
            
            if (eventList == null)
            {
                return new List<Event>();
            }
            return FilterRemoved(eventList, displayRemoved);
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

        public static Event? Create(Event @event)
        {
            string data = JsonConvert.SerializeObject(@event);
            string eventId = RequestHandler.MakeRequest(HttpMethods.POST, tableName, "", "", data);
            string newEvent = GetData(eventId);
            return JsonConvert.DeserializeObject<List<Event>>(newEvent)?.First();
        }

        public static bool Update(Event @event)
        {
            string data = JsonConvert.SerializeObject(@event);
            return RequestHandler.MakeRequest(HttpMethods.PUT, tableName, @event.Id, "", data) != "0";
        }

        public static bool Update(int eventId, string property, string value)
        {
            var rawEvent = GetData(eventId);
            if (rawEvent == null)
            {
                return false;
            }
            Event @event = JsonConvert.DeserializeObject<List<Event>>(rawEvent)?.First() ?? new Event();
            @event.SetProperty(property, value);
            return Update(@event);
        }

        private static List<Event> FilterRemoved(List<Event> events, bool displayRemoved)
        {
            if (events == null || events.Count == 0 || displayRemoved)
            {
                return events ?? new List<Event>();
            }
            return events.Where(p => p.IsRemoved == false).ToList();
        }
    }
}

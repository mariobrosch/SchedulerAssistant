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
            string contacts = GetData(key, filter);
            var contactList = JsonConvert.DeserializeObject<List<Contact>>(contacts);
            
            if (contactList == null)
            {
                return new List<Contact>();
            }
            return FilterRemoved(contactList, displayRemoved);
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

        public static Contact? Create(Contact contact)
        {
            string data = JsonConvert.SerializeObject(contact);
            string contactId = RequestHandler.MakeRequest(HttpMethods.POST, tableName, "", "", data);
            string newContact = GetData(contactId);
            return JsonConvert.DeserializeObject<List<Contact>>(newContact)?.First();
        }

        public static bool Update(Contact contact)
        {
            string data = JsonConvert.SerializeObject(contact);
            return RequestHandler.MakeRequest(HttpMethods.PUT, tableName, contact.Id, "", data) != "0";
        }

        public static bool Update(int contactId, string property, string value)
        {
            var rawContact = GetData(contactId);
            if (rawContact == null)
            {
                return false;
            }
            Contact contact = JsonConvert.DeserializeObject<List<Contact>>(rawContact)?.First() ?? new Contact();
            contact.SetProperty(property, value);
            return Update(contact);
        }

        private static List<Contact> FilterRemoved(List<Contact> contacts, bool displayRemoved)
        {
            if (contacts == null || contacts.Count == 0 || displayRemoved)
            {
                return contacts ?? new List<Contact>();
            }
            return contacts.Where(p => p.IsRemoved == false).ToList();
        }
    }
}

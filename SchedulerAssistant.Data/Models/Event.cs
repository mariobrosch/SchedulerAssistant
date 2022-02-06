using Newtonsoft.Json;
using SchedulerAssistant.Data.Enums;
using System.Collections.Generic;

namespace SchedulerAssistant.Data.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Date { get; set; }
        public string? Time { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public List<Contact>? InvitedContacts { get; set; }
        public List<ContactType>? InvitedContactTypes { get; set; }
        public bool ModeramenOnly { get; set; }
    }
}

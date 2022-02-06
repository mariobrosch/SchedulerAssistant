using Newtonsoft.Json;
using SchedulerAssistant.Data.Enums;
using System;
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
        public List<int>? InvitedContacts { get; set; }
        public List<string>? InvitedContactTypes { get; set; }
        public bool ModeramenOnly { get; set; }
        public DateTimeOffset? LastModifiedDateTime { get; set; }

        public string? Removed { get; set; }
        [JsonIgnore]
        public bool IsRemoved
        {
            get
            {
                return Removed == "1";
            }
            set
            {
                Removed = value ? "1" : "0";
            }
        }
        [JsonIgnore]
        public string? DisplayValue
        {
            get
            {
                return Name + " " + "(" + Date?.ToString() + ")";
            }
        }
    }
}

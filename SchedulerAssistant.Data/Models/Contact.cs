using Newtonsoft.Json;
using SchedulerAssistant.Data.Enums;

namespace SchedulerAssistant.Data.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Lastname { get; set; }
        public string? Abbreviation { get; set; }
        public string? EmailAddress { get; set; }
        public string? Enabled { get; set; }
        public string? Type { get; set; }
        [JsonIgnore]
        public bool IsEnabled
        {
            get
            {
                return Enabled == "1";
            }
            set
            {
                Enabled = value ? "1" : "0";
            }
        }
        public string? ModeramenMember { get; set; }
        [JsonIgnore]
        public bool IsModeramenMember
        {
            get
            {
                return ModeramenMember == "1";
            }
            set
            {
                ModeramenMember = value ? "1" : "0";
            }
        } 
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
                return Name + " " + Lastname + " " + "(" + Abbreviation + ")";
            }
        }
    }
}

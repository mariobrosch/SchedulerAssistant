using Newtonsoft.Json;

namespace SchedulerAssistant.Data.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Abbreviation { get; set; }
        public string? EmailAddress { get; set; }
        public string? Enabled { get; set; }
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
    }
}

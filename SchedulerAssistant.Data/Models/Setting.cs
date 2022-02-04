using Newtonsoft.Json;

namespace SchedulerAssistant.Data.Models
{
    public class Setting
    {
        public int Id { get; set; }
        public string? Key { get; set; }
        public string? Value { get; set; }
        [JsonIgnore]
        public string? PossibleValues { get; set; }
    }
}

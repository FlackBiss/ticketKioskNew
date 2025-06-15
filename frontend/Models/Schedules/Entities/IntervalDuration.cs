using Newtonsoft.Json;

namespace Lastik.Models.Schedules.Entities
{
    public class IntervalDuration
    {
        [JsonProperty("interval")]
        public Duration? Interval { get; set; }
    }
}

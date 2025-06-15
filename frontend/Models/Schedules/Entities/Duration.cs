using Newtonsoft.Json;

namespace Lastik.Models.Schedules.Entities
{
    public class Duration
    {
        [JsonProperty("years")]
        public int Years { get; set; }

        [JsonProperty("months")]
        public int Months { get; set; }

        [JsonProperty("days")]
        public int Days { get; set; }

        [JsonProperty("hours")]
        public int Hours { get; set; }

        [JsonProperty("minutes")]
        public int Minutes { get; set; }

        [JsonProperty("seconds")]
        public int Seconds { get; set; }

        [JsonProperty("milliseconds")]
        public int Milliseconds { get; set; }
    }
}

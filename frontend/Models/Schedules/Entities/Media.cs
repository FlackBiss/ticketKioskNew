using Newtonsoft.Json;

namespace Lastik.Models.Schedules.Entities
{
    public class Media
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("image")]
        public string ImageUrl { get; set; }

        [JsonIgnore]
        public string ImagePath { get; set; }
    }
}

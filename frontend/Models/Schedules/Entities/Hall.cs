using Newtonsoft.Json;

namespace Lastik.Models.Schedules.Entities
{
    public class Hall
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("image")]
        public string? ImageUri { get; set; }

        [JsonIgnore]
        public string? ImagePath { get; set; }

        [JsonProperty("imageDimensionX")]
        public int ImageDimensionX { get; set; }

        [JsonProperty("imageDimensionY")]
        public int ImageDimensionY { get; set; }
    }
}

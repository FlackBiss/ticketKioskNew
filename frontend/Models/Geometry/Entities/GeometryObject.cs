using Newtonsoft.Json;

namespace Lastik.Models.Geometry.Entities
{
    public class GeometryObject
    {
        [JsonProperty("bb")]
        public SizeCoordinates? Bb { get; set; }

        [JsonProperty("text")]
        public GeometryText? Text { get; set; }

        [JsonProperty("uuid")]
        public string? Uuid { get; set; }

        [JsonProperty("path")]
        public GeometryPath? Path { get; set; }
    }
}

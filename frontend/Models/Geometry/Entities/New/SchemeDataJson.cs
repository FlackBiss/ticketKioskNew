using Newtonsoft.Json;

namespace Lastik.Models.Geometry.Entities.New
{
    public class SchemeDataJson
    {
        [JsonProperty("placeId")]
        public int? PlaceId { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("cords")]
        public List<Cord> Cords { get; set; }

        [JsonProperty("left")]
        public double? Left { get; set; }

        [JsonProperty("top")]
        public double? Top { get; set; }

        [JsonProperty("width")]
        public double? Width { get; set; }

        [JsonProperty("height")]
        public double? Height { get; set; }

        [JsonProperty("booked")]
        public bool? Booked { get; set; }

        [JsonProperty("section")]
        public object Section { get; set; }

        [JsonProperty("row")]
        public object Row { get; set; }

        [JsonProperty("seatNumber")]
        public object SeatNumber { get; set; }
    }
}

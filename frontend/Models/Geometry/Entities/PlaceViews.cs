using Newtonsoft.Json;

namespace Lastik.Models.Geometry.Entities
{
    public class PlaceViews
    {
        [JsonProperty("place_views")]
        public Dictionary<string,PlaceView>? PlaceViewsDetails { get; set; }

        [JsonProperty("place_map")]
        public Dictionary<string,List<string>>? PlaceMap { get; set; }

    }
}

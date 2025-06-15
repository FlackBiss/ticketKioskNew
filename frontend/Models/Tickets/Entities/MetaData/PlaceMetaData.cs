using Newtonsoft.Json;

namespace Lastik.Models.Tickets.Entities.MetaData
{
    public class PlaceMetaData
    {
        [JsonProperty("row")]
        public RowMetaData? Row { get; set; }

        [JsonProperty("seat")]
        public string? Seat { get; set; }

        [JsonProperty("sector")]
        public SectorMetaData? Sector { get; set; }

        [JsonProperty("fragment")]
        public FragmentMetaData? Fragment { get; set; }

    }
}

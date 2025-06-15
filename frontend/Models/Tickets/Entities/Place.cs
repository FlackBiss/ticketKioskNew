using Lastik.Models.Geometry.Entities;
using Lastik.Models.Tickets.Entities.MetaData;
using Newtonsoft.Json;

namespace Lastik.Models.Tickets.Entities;

public class Place
{
    [JsonProperty("uuid")]
    public string? Uuid { get; set; }

    [JsonProperty("is_active")]
    public bool IsActive { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("geometry")]
    public PlaceGeometry? Geometry { get; set; }

    [JsonProperty("sector_uuid")]
    public string? SectorUuid { get; set; }

    [JsonProperty("fragment_uuid")]
    public string? FragmentUuid { get; set; }

    [JsonProperty("row_uuid")]
    public string? RowUuid { get; set; }

    [JsonProperty("meta")]
    public PlaceMetaData? Meta { get; set; }
}

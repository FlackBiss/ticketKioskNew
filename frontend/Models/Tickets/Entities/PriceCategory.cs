using Lastik.Models.Schedules.Entities;
using Newtonsoft.Json;

namespace Lastik.Models.Tickets.Entities;

public class PriceCategory
{
    [JsonProperty("uuid")]
    public string? Uuid { get; set; }

    [JsonProperty("is_active")]
    public bool IsActive { get; set; }

    [JsonProperty("name")]
    public MultiLanguageText? Name { get; set; }

    [JsonProperty("description")]
    public object? Description { get; set; }

    [JsonProperty("ticket_type_uuid")]
    public string? TicketTypeUuid { get; set; }

    [JsonProperty("event_uuid")]
    public string? EventUuid { get; set; }

    [JsonProperty("spot_uuid")]
    public string? SpotUuid { get; set; }
}
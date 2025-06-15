using Lastik.Models.Geometry.Entities;
using Lastik.Models.Geometry.Entities.New;
using Newtonsoft.Json;
using static Lastik.Models.Schedules.Stores.ScheduleStore;

namespace Lastik.Models.Schedules.Entities;

public class Schedule
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("scheme")]
    public Hall? Hall { get; set; }

    [JsonProperty("Spot")]
    public Spot? Spot { get; set; }

    [JsonProperty("title")]
    public string? Title { get; set; }

    [JsonProperty("event")]
    public Schedule? Event { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }

    [JsonIgnore]
    public ScheduleTicketType TypeEnum { get; set; }

    [JsonProperty("image")]
    public string? ImageUri { get; set; }

    [JsonIgnore]
    public string? ImagePath { get; set; }

    [JsonProperty("startPrice")]
    public int? StartPrice { get; set; }

    [JsonProperty("placesHave")]
    public int? PlacesHave { get; set; }

    [JsonIgnore]
    public bool IsHaveTickets => StartPrice != null;

    [JsonProperty("shortDescription")]
    public string? ShortDescription { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("age")]
    public string? Age { get; set; }

    [JsonProperty("duration")]
    public DateTime Duration { get; set; }

    [JsonProperty("uuid")] 
    public string Uuid { get; set; } = string.Empty;

    [JsonProperty("dateTimeAt")]
    public DateTime DateTimeAt { get; set; }

    [JsonProperty("is_sold_out")]
    public bool IsSoldOut { get; set; }

    [JsonProperty("is_season_head")]
    public bool IsSeasonHead { get; set; }

    [JsonProperty("images")]
    public List<Media>? Images { get; set; }

    [JsonProperty("schemeDataJson")]
    public List<SchemeDataJson> SchemeDataJson { get; set; }

    [JsonProperty("typesPlaces")]
    public List<TypesPlace> TypesPlaces { get; set; }
    [JsonProperty("LinkedSchedules")] public List<Schedule> LinkedSchedules { get; set; } = [];
}
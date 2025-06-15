using Lastik.Models.Schedules.Entities;
using Lastik.ViewModels.HallGeometry;
using Newtonsoft.Json;

namespace Lastik.Models.Tickets.Entities;

public class Ticket
{
    [JsonProperty("uuid")]
    public string Uuid { get; set; } = string.Empty;

    [JsonProperty("is_active")]
    public bool IsActive { get; set; }

    [JsonProperty("price_values")] 
    public List<string> PriceValues { get; set; } = [];

    [JsonProperty("is_for_sale")]
    public bool IsForSale { get; set; }

    [JsonProperty("Place")] 
    public Place Place { get; set; } = null!;

    [JsonProperty("processed")]
    public bool Processed { get; set; }

}

public class PriceTicket(
    string uuid,
    Place place, 
    string priceId,
    Dictionary<string, PriceValue> prices)
{
    public string Uuid { get; } = uuid;
    public PriceValue Price { get; } = prices[priceId];
    public Place Place { get; } = place;
}

public static class PriceTicketExtensions
{
    public static List<PlaceViewModel> ToPlaceViewModels(
        this IEnumerable<PriceTicket> tickets,
        Models.Geometry.Entities.HallGeometry hallGeometry,
        Schedule schedule, List<PriceValue> priceValues)
        => tickets
            .Select(t => new PlaceViewModel(t, hallGeometry, schedule, priceValues))
            .ToList();
}
using Lastik.Models.Schedules.Entities;
using Lastik.Models.Tickets.Entities;
using Lastik.Models.Tickets.Entities.MetaData;
using Newtonsoft.Json;

namespace Lastik.Models.Cart.Entities;


public class CartTicket
{
    [JsonProperty("uuid")]
    public string? Uuid { get; set; }

    [JsonProperty("item_uuid")]
    public string? ItemUuid { get; set; }

    [JsonProperty("quantity")]
    public int Quantity { get; set; }

    [JsonProperty("currency")]
    public string? Currency { get; set; }

    [JsonProperty("nominal_price")]
    public int NominalPrice { get; set; }

    [JsonProperty("price")]
    public PriceValue? Price { get; set; }

    [JsonProperty("price_uuid")]
    public string? PriceUuid { get; set; }

    [JsonProperty("total_cost")]
    public int TotalCost { get; set; }

    [JsonProperty("expired_at")]
    public DateTime ExpiredAt { get; set; }

    [JsonProperty("is_expired")]
    public bool IsExpired { get; set; }

    [JsonProperty("is_reserved")]
    public bool IsReserved { get; set; }

    [JsonProperty("service_fee")]
    public int ServiceFee { get; set; }

    [JsonProperty("pushkin_card_approved")]
    public bool PushkinCardApproved { get; set; }

    [JsonProperty("place")]
    public PlaceMetaData? Place { get; set; }

    [JsonProperty("price_values")]
    public List<PriceValue>? PriceValues { get; set; }
}
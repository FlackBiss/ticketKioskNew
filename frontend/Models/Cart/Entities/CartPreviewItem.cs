using Lastik.Models.Schedules.Entities;
using Newtonsoft.Json;

namespace Lastik.Models.Cart.Entities
{
    public class CartPreviewItem
    {
        [JsonProperty("item_uuid")]
        public string? ItemUuid { get; set; }

        [JsonProperty("price_uuid")]
        public string? PriceUuid { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("priceValues")]
        public List<PriceValue>? PriceValues { get; set; }

        [JsonProperty("schedule")]
        public string? Schedule { get; set; }

        [JsonProperty("price")]
        public PriceValue? Price { get; set; }

    }
}

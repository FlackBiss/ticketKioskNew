using FreeKassaPayOnline.Enums;
using Newtonsoft.Json;

namespace Lastik.Models.Cart.Entities
{
    public class Cart
    {
        [JsonProperty("total_price")]
        public int TotalPrice { get; set; }

        [JsonProperty("items")]
        public List<CartItem>? Items { get; set; }

        [JsonProperty("items")]
        public PaymentType PaymentType { get; set; }
    }
}

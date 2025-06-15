using Newtonsoft.Json;

namespace Lastik.Models.Cart.Entities;

public class CartPreview
{
    [JsonProperty("items")] public List<CartPreviewItem> Items { get; set; } = [];

    [JsonProperty("lang")] public string Language => "ru";
}
using Lastik.Models.Tickets.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Lastik.Models.Schedules.Entities;

public class PriceValue
{
    [JsonIgnore]
    private int _price;

    [JsonProperty("amount")]
    public int Price
    {
        get => _price / 100;
        set => _price = value;
    }
    [JsonProperty("uuid")]
    public string? Uuid { get; set; }


    [JsonIgnore]
    public List<int>? Color { get; set; }

}

public static class PriceValueExtensions
{

    public static List<List<int>> GeneratePriceColors(this IEnumerable<PriceValue> prices)
    {
        var random = new Random();
        List<List<int>> res = [];
        var priceList = prices.ToList();
        var step = 255 / priceList.Count;
        var count = 1;
        var currentColor = 0;
        var i = 0;
        while (i < priceList.Count)
        {
            currentColor += step * count;
            count = 0;
            List<int> color = [currentColor];
            var rand = random.Next(255 - currentColor, 255);
            color.Add(rand);
            color.Add(510 - rand - currentColor);
            do
            {
                res.Add(color);
                i++;
                if (i >= priceList.Count) break;
                count++;
            } while (priceList[i - 1].Price == priceList[i].Price);
        }
        return res;
    }

    public static Dictionary<string, PriceValue> ColorPriceValues(this Dictionary<string,PriceValue> prices)
    {
        var ordered = prices.OrderBy(p => p.Value.Price).ToDictionary();
        var colors = ordered.Values.GeneratePriceColors();

        var i = 0;

        foreach (var priceValue in ordered)
        {
            priceValue.Value.Color = colors[i];
            i++;
        }

        return ordered;
    }
}
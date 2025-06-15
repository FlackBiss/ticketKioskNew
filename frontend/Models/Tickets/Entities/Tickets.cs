using Lastik.Models.Schedules.Entities;
using Newtonsoft.Json;

namespace Lastik.Models.Tickets.Entities;

public class Tickets
{
    [JsonProperty("prices")] public Dictionary<string, PriceValue> Prices { get; set; } = new();
    [JsonProperty("tickets")] public List<Ticket> TicketsList { get; set; } = [];

    public Tickets FillPrices()
    {
        FilterInactive();
        ColorPrices();
        return this;
    }

    private void FilterInactive()
        =>TicketsList = 
            TicketsList
                .Where(item => item is { IsActive: true, IsForSale: true })
                .ToList();

    private void ColorPrices() =>
        Prices = Prices.ColorPriceValues();
}

public static class TicketsExtensions
{
    public static List<PriceTicket> ToPriceTickets(this Tickets tickets) => 
        tickets
            .FillPrices()
            .TicketsList
            .Select(t => new PriceTicket(t.Uuid,t.Place, t.PriceValues.First(), tickets.Prices))
            .ToList();

    public static List<PriceValue> ToPrices(this IEnumerable<PriceTicket> tickets) =>
        tickets.Select(t => t.Price).Distinct().OrderBy(t=>t.Price).ToList();
}
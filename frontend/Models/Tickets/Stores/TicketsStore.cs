using Lastik.Helpers.Logging;
using Lastik.Models.Schedules.Entities;
using Lastik.Models.Tickets.Entities;
using Lastik.Utilities;

namespace Lastik.Models.Tickets.Stores;

public class TicketsStore(IApiHttpClient httpClient,ILoggingService logger)
{
    public async Task<List<PriceTicket>> GetAsync(string scheduleId)=>
        (await httpClient.
            GetTickets(scheduleId))
        .GetContent(logger)
        .ToPriceTickets();
}
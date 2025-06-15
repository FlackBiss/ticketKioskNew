using Lastik.Helpers.Logging;
using Refit;

namespace Lastik.Models.EventCalendar;

public class EventCalendarStore(IApiHttpClient apiHttpClient, ILoggingService logger)
{
    public async Task<EventCalendar> GetAsync()
        => (await apiHttpClient.GetEventCalendar()).GetContent(logger);
}
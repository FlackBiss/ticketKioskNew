using Lastik.Helpers.Logging;
using Lastik.Models.Schedules.Entities;
using Lastik.Utilities;
using Refit;

namespace Lastik.Models.Schedules.Stores;

public class ScheduleStore(IApiHttpClient httpClient, ILoggingService logger)
{
    public async Task<List<Schedule>> GetEvents(string searchText, DateTime date, int? schemeId, string? ticketType)
    {
        if (!string.IsNullOrEmpty(searchText))
        {
            return await LoadImages((await httpClient.GetEvents(new EventQueryParameters
            {
                SearchText = searchText
            })).GetContent(logger));
        }

        if (date != DateTime.MinValue)
        {
            return await LoadImages((await httpClient.GetEvents(new EventQueryParameters
            {
                DateBefore = date.Date.AddDays(1),
                DateAfter = date.Date,
            })).GetContent(logger));
        }

        var query = new EventQueryParameters();
        
        if (schemeId is not null && schemeId != 0)
        {
            query.SchemeId = schemeId;
        }

        if (!string.IsNullOrEmpty(ticketType))
        {
            query.ticketType = ticketType;
        }

        return await LoadImages((await httpClient.GetEvents(query)).GetContent(logger));
    }

    public async Task<List<Schedule>> GetNews()
        => await LoadImages((await httpClient.GetNews()).GetContent(logger));

    public async Task<List<Schedule>> GetHelp()
        => await LoadImages([(await httpClient.GetHelp()).GetContent(logger)]);

    private async Task<List<Schedule>> LoadImages(List<Schedule> schedules)
    {
        foreach (var schedule in schedules)
        {
            if (schedule.ImageUri != null)
                schedule.ImagePath = await httpClient.LoadImageAndGetPath(logger, schedule.ImageUri);

            if (schedule.Images != null)
                foreach (var scheduleImage in schedule.Images)
                {
                    scheduleImage.ImagePath = await httpClient.LoadImageAndGetPath(logger, scheduleImage.ImageUrl);
                }

            schedule.TypeEnum = schedule.Type switch
            {
                "Места согласно билетам" => ScheduleTicketType.SeatsByTickets,
                "Ограниченное количество мест" => ScheduleTicketType.TicketsLimited,
                "Неограниченное количество мест" => ScheduleTicketType.TicketsUnlimited,
                _ => schedule.TypeEnum
            };

            if (schedule.PlacesHave == 0)
            {
                schedule.PlacesHave = null;
            }

            //if (schedule.Event.Id == 0)
            //    schedule.Event = null;
        }

        return schedules;
    }

    public enum ScheduleTicketType
    {
        SeatsByTickets,
        TicketsLimited,
        TicketsUnlimited
    }
}
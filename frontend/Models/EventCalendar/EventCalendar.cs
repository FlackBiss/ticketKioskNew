using Newtonsoft.Json;

namespace Lastik.Models.EventCalendar;

public class EventCalendar
{
    [JsonProperty("min")] public DateTime? Min => Dates?.Min();
    [JsonProperty("max")] public DateTime? Max => Dates?.Max();

    [JsonProperty("dates")]
    public List<DateTime>? Dates { get; set; }

    public IEnumerable<DateTime> TryGetWeek(DateTime? startDate)
    {
        
        List<DateTime> week = [];
        var dates = Dates?.ToList();
        if (dates is null) return [];
        startDate ??= dates.Min();

        var ind = 0;
        var weekFound = false;
        while (ind < dates.Count)
        {
            if (weekFound) return week;
            week.Clear();
            for (var i = 0; i < 7; i++)
            {
                if (ind >= dates.Count) break;
                if (dates[ind] == startDate) weekFound = true;
                week.Add(dates[ind]);
                ind++;
            }
        }

        return week;
    }
}
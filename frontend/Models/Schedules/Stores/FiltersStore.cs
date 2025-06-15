using Lastik.Helpers.Logging;
using Lastik.Models.Schedules.Entities;
using Lastik.ViewModels.Popups;

namespace Lastik.Models.Schedules.Stores;

public class FiltersStore(IApiHttpClient httpClient, ILoggingService logger)
{
    private List<Hall>? _filters;
    public List<Hall> SelectedFilterHalls { get; set; } = [];

    public async Task<List<Hall>> GetAllFilters() => _filters ??= await LoadFilters();

    private async Task<List<Hall>> LoadFilters()
        => (await httpClient.GetFilters()).GetContent(logger);

    
    
    public List<Hall> SelectedEventTicketType { get; set; } = [];

    public readonly List<Hall> EventTicketType =
    [
        new()
        {
            Id = 1,
            Name = "Места согласно билетам"
        },

        new()
        {
            Id = 2,
            Name = "Неограниченное количество мест"
        },

        new()
        {
            Id = 3,
            Name = "Ограниченное количество мест"
        }

    ];
}


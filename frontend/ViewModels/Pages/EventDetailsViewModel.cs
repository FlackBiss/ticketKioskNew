using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lastik.Models.Schedules.Entities;
using MvvmNavigationLib.Services;
using System.Text;
using Lastik.Models.Loggining;
using Lastik.Models.Schedules.Stores;
using MvvmNavigationLib.Stores;
using Lastik.Models.Geometry.Entities.New;

namespace Lastik.ViewModels.Pages;

public partial class EventDetailsViewModel(
        IParameterNavigationService<Schedule> toEventDetailsNavigationService,
        IParameterNavigationService<Schedule> toPlaceSelectionNavigationService,
        IParameterNavigationService<Schedule> toCartPageNavigationService,
        GoBackNavigationService<NavigationStore> goBack,
        Schedule @event,
        UserSessionStore sessionStore) 
    : BasePageViewModel(@event.IsHaveTickets ? "МЕРОПРИЯТИЕ" : "НОВОСТЬ")
{

    [ObservableProperty] private Schedule _event = @event;

    public string EventDuration => GetEventTime(Event.Duration);

    [RelayCommand]
    private void ToEventDetails()
    {
        if (Event.Event == null) return;
        sessionStore.AddAction($"Переход к мероприятию: {Event.Event.Title}", Event.Event.Id);
        toEventDetailsNavigationService.Navigate(Event.Event);
    }

    [RelayCommand]
    private void ToPlaceSelection()
    {
        if (@event.TypeEnum == ScheduleStore.ScheduleTicketType.SeatsByTickets)
        {
            sessionStore.AddAction($"Переход к покупке мест по мероприятию: {Event.Title}", Event.Id);
            toPlaceSelectionNavigationService.Navigate(Event);
        }
        else
        {
            sessionStore.AddAction($"Переход к покупке билетов по мероприятию: {Event.Title}", Event.Id);
            toCartPageNavigationService.Navigate((Event));
        }
    }

    [RelayCommand]
    private void GoBack() => goBack.Navigate();

    private string GetEventTime(DateTime? duration)
    {
        if (duration is null) return string.Empty;
        var res = new StringBuilder();
        res.Append(GetTimePart(duration.Value.Hour, "час", "часа", "часов"));
        res.Append(GetTimePart(duration.Value.Minute, "минута", "минуты", "минут"));
        res.Append(GetTimePart(duration.Value.Second, "секунда", "секунды", "секунд"));
        return res.ToString();
    }

    private string GetTimePart(int timePart, string first, string second, string third)
        => timePart switch
        {
            1 => $"{timePart} {first} ",
            > 1 and <= 4 => $"{timePart} {second} ",
            > 4 => $"{timePart} {third} ",
            _ => string.Empty
        };
}


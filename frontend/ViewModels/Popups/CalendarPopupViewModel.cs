using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Lastik.Models.EventCalendar;
using MvvmNavigationLib.Services;
using MvvmNavigationLib.Stores;

namespace Lastik.ViewModels.Popups;

public partial class CalendarPopupViewModel(
    DateTime selectedDate,
    IMessenger messenger,
    CloseNavigationService<ModalNavigationStore> closeModalNavigationService,
    EventCalendarStore eventCalendarStore)
    : BasePopupViewModel(closeModalNavigationService)
{
    [ObservableProperty] private EventCalendar? _eventCalendar;
    [ObservableProperty] private DateTime _selectedDate = selectedDate;

    private readonly List<DateTime> _availableDates = [];

    [RelayCommand]
    private async Task Loaded(Calendar calendarControl)
    {
        EventCalendar = await eventCalendarStore.GetAsync();
        if (EventCalendar.Dates is null) return;
        if (EventCalendar.Min == null) return;
        var date = (DateTime)EventCalendar.Min;
        while (date <= EventCalendar.Max)
        {
            if (EventCalendar.Dates.All(d => d.Date != date.Date))
                calendarControl.BlackoutDates.Add(new CalendarDateRange(date));
            else
                _availableDates.Add(date);
            date = date.AddDays(1);
        }
    }

    [RelayCommand]
    private void SelectDay()
    {
        messenger.Send(new CalendarDateSelectedMessage(SelectedDate));
        CloseContainerCommand.Execute(false);
    }
}
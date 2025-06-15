using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lastik.Models.Schedules.Entities;
using Lastik.Models.Schedules.Stores;
using Lastik.ViewModels.Controllers;
using Lastik.ViewModels.Pages;
using MvvmNavigationLib.Services;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Messaging;
using CustomKeyboard.Helpers;
using Lastik.Models;
using Lastik.Models.EventCalendar;
using Lastik.ViewModels.Popups;
using System.Globalization;
using Lastik.Models.Loggining;
using Lastik.Models.Schedules.Messages;

namespace Lastik.ViewModels.Controls;

public enum PageTypes
{
    Events,
    News,
    Help
}


public delegate IEnumerable<ItemViewModel> CreateScheduleItem(IEnumerable<Schedule> schedules, PageTypes page);

public partial class ScheduleListViewModel 
    : BasePageViewModel, IRecipient<CalendarDateSelectedMessage>, IRecipient<FiltersChangedMessage>, IRecipient<FiltersResetMessage>
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasEvents))]
    private ObservableCollection<ItemViewModel> _allEvents = [];

    

    [ObservableProperty,NotifyPropertyChangedFor(nameof(HasCalendar))] private NavigationBarItem? _curPage;

    [ObservableProperty] private string _searchText = string.Empty;

    [ObservableProperty] private bool _isKeyboardOpen;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasEvents))]
    private bool _loading;

    [ObservableProperty] private List<DateTime> _eventDays = [];
    [ObservableProperty] private DateTime? _selectedEventDay;
    private List<int> FilterHalls { get; set; } = [];
    private List<string> FilterTicketTypes { get; set; } = [];

    public bool HasCalendar => CurPage?.PageType is PageTypes.Events;
    private ScrollViewer? _scrollViewer;

    public NavigationBarItem[] NavigationItems { get; } =
    [
        new(PageTypes.Events,"МЕРОПРИЯТИЯ"),
        new(PageTypes.News,"НОВОСТИ"),
        new(PageTypes.Help,"СПРАВКА")
    ];


    public bool HasEvents => AllEvents.Count > 0 || !Loading;

    private List<ItemViewModel> _events = [];
    private readonly LoaderController _loaderController;
    private readonly ScheduleStore _itemStore;
    private readonly CreateScheduleItem _eventItemFactory;
    private readonly EventCalendarStore _eventCalendar;
    private readonly ParameterNavigationService<CalendarPopupViewModel, DateTime> _toCalendar;
    private readonly NavigationService<FiltersPopupViewModel> _toFilters;
    private readonly UserSessionStore _sessionStore;
    private EventCalendar? _calendar;

    public ScheduleListViewModel(
        LoaderController loaderController,
        ScheduleStore itemStore,
        CreateScheduleItem eventItemFactory,
        EventCalendarStore eventCalendar,
        ParameterNavigationService<CalendarPopupViewModel,DateTime> toCalendar,
        NavigationService<FiltersPopupViewModel> toFilters,
        UserSessionStore sessionStore,
        IMessenger messenger)
        : base("МЕРОПРИЯТИЯ")
    {
        _loaderController = loaderController;
        _itemStore = itemStore;
        _eventItemFactory = eventItemFactory;
        _eventCalendar = eventCalendar;
        _toCalendar = toCalendar;
        _toFilters = toFilters;
        _sessionStore = sessionStore;
        messenger.RegisterAll(this);
    }

    async partial void OnCurPageChanged(NavigationBarItem? value)
    {
        if(value is null) return;
        Loading = false;
        SearchText = string.Empty;
        Title = value.PageTitle;
        AllEvents = [];
        _sessionStore.AddAction($"Переход в раздел: {value.PageTitle}");
        await _loaderController.RunLoadingTask(() => LoadSchedules(value));
        Loading = true;
    }

    [RelayCommand]
    private async Task Loaded(ScrollViewer scroll)
    {
        if (CurPage is not null) return;
        CurPage = NavigationItems.First();
        _scrollViewer = scroll;
        _calendar = await _eventCalendar.GetAsync();
        InitCalendar();
    }

    [RelayCommand]
    private async Task Search()
    {
        FilterHalls.Clear();
        FilterTicketTypes.Clear();
        SelectedEventDay = null;
        _sessionStore.AddAction($"Поиск мероприятия по строке: {SearchText}");
        await _loaderController.RunLoadingTask(() => LoadSchedules(CurPage));
        CloseKeyboard();
    }

    [RelayCommand]
    private void CloseKeyboard()
    {
        IsKeyboardOpen = false;
        KeyboardManager.CurrentViewModel.SetLayout(KeyboardLayoutNames.Russian);
        SearchText = string.Empty;
    }

    [RelayCommand]
    private void GotSearchFocus() => IsKeyboardOpen = true;

    private async Task LoadSchedules(NavigationBarItem? page)
    {
        if(page is null) return;
        _events = page.PageType switch
        {
            PageTypes.Events => [.. await LoadEvents()],
            PageTypes.News => [.. await LoadNews()],
            PageTypes.Help => [.. await LoadHelp()],
            _ => throw new ArgumentOutOfRangeException()
        };
        AllEvents = [];
        AllEvents.LoadNextToObservable(_events,10);
        OnPropertyChanged(nameof(HasEvents));
        _scrollViewer?.ScrollToHome();
    }

    [RelayCommand]
    private void ScrollChanged(ScrollChangedEventArgs args)
    {
        if(args.CheckScrollOverflow())
            AllEvents.LoadNextToObservable(_events,10);
    }

    [RelayCommand]
    private void OpenCalendar() => _toCalendar.Navigate(SelectedEventDay??DateTime.MinValue);

    [RelayCommand]
    private void OpenFilters() => _toFilters.Navigate();

    [RelayCommand]
    private async Task DaySelected() => await LoadSchedules(CurPage);

    private async Task<List<ItemViewModel>> LoadEvents()
    {
        return [.._eventItemFactory(await _itemStore.GetEvents(
            SearchText,
            SelectedEventDay ?? DateTime.MinValue, 
            FilterHalls.FirstOrDefault(),
            FilterTicketTypes.FirstOrDefault()),
            PageTypes.Events)];
    }

    private async Task<List<ItemViewModel>> LoadNews()
    {
        return [.._eventItemFactory(await _itemStore.GetNews(), PageTypes.News)];
    }

    private async Task<List<ItemViewModel>> LoadHelp()
    {
        return [.. _eventItemFactory(await _itemStore.GetHelp(), PageTypes.Help)];
    }

    public async void Receive(CalendarDateSelectedMessage message)
    {
        await Task.Delay(500);
        SearchText = string.Empty;
        SelectedEventDay = message.Value;
        InitCalendar();
        _sessionStore.AddAction($"Поиск мероприятия по дате: {message.Value}");
        await LoadSchedules(CurPage);
    }

    private void InitCalendar()
    {
        EventDays = [.. _calendar?.TryGetWeek(SelectedEventDay)??[]];
        if (SelectedEventDay is not null
            || SelectedEventDay == EventDays.FirstOrDefault()
            || EventDays.Count < 1)
        {
            SelectedEventDay = SelectedEventDay == DateTime.MinValue ? null : SelectedEventDay;
        }
        else
        {
            SelectedEventDay = EventDays.First();
        }
    }

    public async void Receive(FiltersChangedMessage message)
    {
        await Task.Delay(500);
        FilterHalls = message.Value.Item1;
        FilterTicketTypes = message.Value.Item2;
        SelectedEventDay = null;
        _sessionStore.AddAction($"Филтрация мероприятий: {message.Value.Item1} {message.Value.Item2}");
        await LoadSchedules(CurPage);
    }

    public async void Receive(FiltersResetMessage message)
    {
        await Task.Delay(500);
        FilterHalls.Clear();
        FilterTicketTypes.Clear();
        SelectedEventDay = _calendar?.Min;
        await LoadSchedules(CurPage);
    }
}

public record NavigationBarItem(PageTypes PageType, string PageTitle);
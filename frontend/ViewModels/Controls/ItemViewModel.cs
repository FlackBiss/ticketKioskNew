using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lastik.Models.Loggining;
using Lastik.Models.Schedules.Entities;
using MvvmNavigationLib.Services;

namespace Lastik.ViewModels.Controls;

public partial class ItemViewModel : ObservableObject
{
    private readonly IParameterNavigationService<Schedule>? _toDetailsNavigationService;
    [ObservableProperty] private Schedule _item;
    [ObservableProperty] private UserSessionStore _sessionStore;

    protected ItemViewModel(IParameterNavigationService<Schedule>? toDetailsNavigationService, Schedule item, UserSessionStore sessionStore)
    {
        _toDetailsNavigationService = toDetailsNavigationService;
        _item = item;
        _sessionStore = sessionStore;
    }

    [RelayCommand]
    private void ToDetails()
    {
        _sessionStore.AddAction($"Переход к элементу: {Item.Title}", Item.Id);
        _toDetailsNavigationService?.Navigate(Item);
    }

}

public class NewsItemViewModel(IParameterNavigationService<Schedule>? toDetailsNavigationService, Schedule news, UserSessionStore sessionStore)
    : ItemViewModel(toDetailsNavigationService, news, sessionStore);

public class EventItemViewModel(IParameterNavigationService<Schedule>? toDetailsNavigationService, Schedule @event, UserSessionStore sessionStore)
    : ItemViewModel(toDetailsNavigationService, @event, sessionStore);

public class HelpViewModel(Schedule help) : ItemViewModel(toDetailsNavigationService: null, help, null);
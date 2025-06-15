using System.IO;
using System.Net.Http;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Lastik.Helpers;
using Lastik.Helpers.Logging;
using Lastik.Models;
using Lastik.Models.Loggining;
using Lastik.Models.Schedules.Entities;
using Lastik.Models.Schedules.Stores;
using Lastik.ViewModels.Controls;
using Lastik.ViewModels.Pages;
using Lastik.ViewModels.Popups;
using Microsoft.Extensions.Logging;
using MvvmNavigationLib.Services;
using MvvmNavigationLib.Stores;
using Newtonsoft.Json;

namespace Lastik.ViewModels;

public class InactivityManager<TInitialViewModel>:IDisposable
where TInitialViewModel:ObservableObject
{
    private readonly NavigationStore _mainStore;
    private readonly ModalNavigationStore _modalStore;
    private readonly FiltersStore _filtersStore;
    private readonly INavigationService _closePopupNavigationService;
    private readonly ParameterNavigationService<StandByPageViewModel, List<MainAd>> _standByParameterNavigationService;
    private readonly IApiHttpClient _client;
    private readonly ILoggingService _logger;
    private readonly BaseInactivityHelper _inactivity;
    private readonly BaseInactivityHelper _passwordInactivity;
    private readonly UserSessionStore _sessionStore;
    public InactivityManager(
        NavigationStore mainStore,
        ModalNavigationStore modalStore,
        FiltersStore filtersStore,
        INavigationService initialNavigationService,
        INavigationService closePopupNavigationService,
        ParameterNavigationService<StandByPageViewModel, List<MainAd>> standByParameterNavigationService,
        IApiHttpClient client,
        ILoggingService logger,
        UserSessionStore sessionStore,
        int inactivityTime,
        int passwordInactivityTime)
    {
        _mainStore = mainStore;
        _modalStore = modalStore;
        _filtersStore = filtersStore;
        _sessionStore = sessionStore;
        _standByParameterNavigationService = standByParameterNavigationService;
        _client = client;
        _logger = logger;
        _closePopupNavigationService = closePopupNavigationService;
        _inactivity = new BaseInactivityHelper(inactivityTime);
        _passwordInactivity = new BaseInactivityHelper(passwordInactivityTime);
    }

    public void Activate()
    {
        _inactivity.OnInactivity += _inactivity_OnInactivity;
        _passwordInactivity.OnInactivity += _passwordInactivity_OnInactivity;
    }

    public void Dispose()
    {
        _inactivity.OnInactivity -= _inactivity_OnInactivity;
        _passwordInactivity.OnInactivity -= _passwordInactivity_OnInactivity;
    }

    private void _passwordInactivity_OnInactivity(int inactivityTime)
    {
        if(_modalStore.CurrentViewModel is not PasswordPopupViewModel) return;
        _closePopupNavigationService.Navigate();
    }

    private async void _inactivity_OnInactivity(int inactivityTime)
    {
        _filtersStore.SelectedFilterHalls.Clear();
        if (_mainStore.CurrentViewModel is StandByPageViewModel) return;
        _standByParameterNavigationService.Navigate(await LoadImages((await _client.GetStandByModels()).GetContent(_logger)));
        _sessionStore.SendAsync();
        //if(_mainStore.CurrentViewModel is ScheduleListViewModel) return;
        //_initialNavigationService.Navigate();
    }
    private async Task<List<MainAd>> LoadImages(List<MainAd> ads)
    {
        foreach (var ad in ads)
        {
            if (ad.MediaUrl!= null)
                ad.MediaPath = await _client.LoadImageAndGetPath(_logger, ad.MediaUrl);
        }

        return ads;
    }
}
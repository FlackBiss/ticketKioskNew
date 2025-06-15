using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core;
using Lastik.Helpers;
using Lastik.ViewModels.Controls;
using Lastik.ViewModels.Pages;
using Lastik.ViewModels.Popups;
using MvvmNavigationLib.Services;
using MvvmNavigationLib.Stores;
using System.Windows.Threading;
using Lastik.Models;
using Lastik.Models.Loggining;
using Lastik.Models.Schedules.Entities;
using ObservableObject = CommunityToolkit.Mvvm.ComponentModel.ObservableObject;


namespace Lastik.ViewModels.Windows;

public partial class MainWindowViewModel:ObservableObject,
    IRecipient<ViewModelChangedMessage>,
    IRecipient<ModalViewModelChangedMessage>, 
    IRecipient<StandByChangedMessage>,
    IRecipient<ServerErrorChanged>
{
    private readonly DispatcherTimer _timer = new();
    private int _sec;

    private readonly ServerErrorModalStore _serverErrorStore;
    private readonly IApiHttpClient _iApiHttpClient;
    private readonly NavigationStore _navigationStore;
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly InactivityManager<ScheduleListViewModel> _inactivityManager;
    private readonly ParameterNavigationService<PasswordPopupViewModel,string> _passwordNavigationService;
    private readonly UserSessionStore _sessionStore;
    private readonly string _password;

    public BasePageViewModel? CurrentViewModel => _navigationStore.CurrentViewModel as BasePageViewModel;
    public ObservableObject? CurrentModalViewModel => _modalNavigationStore.CurrentViewModel;

    public ObservableObject? ServerError => _serverErrorStore.CurrentViewModel;

    public bool IsModalOpen => CurrentModalViewModel is not null;
    public bool IsServerError => ServerError is not null;

    [ObservableProperty] private bool _isStandBy = true;

    public MainWindowViewModel(IMessenger messenger,
        ServerErrorModalStore serverErrorStore,
        NavigationStore navigationStore, 
        ModalNavigationStore modalNavigationStore, 
        InactivityManager<ScheduleListViewModel> inactivityManager,
        IApiHttpClient httpClient,
        ParameterNavigationService<PasswordPopupViewModel, string> passwordNavigationService,
        UserSessionStore sessionStore, 
        string password)
    {
        _sessionStore = sessionStore;
        _sessionStore = sessionStore;
        _iApiHttpClient = httpClient;
        _serverErrorStore = serverErrorStore;
        _navigationStore = navigationStore;
        _modalNavigationStore = modalNavigationStore;
        _inactivityManager = inactivityManager;
        _passwordNavigationService = passwordNavigationService;
        messenger.RegisterAll(this);
        _inactivityManager.Activate();
        _password = password;
    }

    private void Timer(object? sender, EventArgs eventArgs)
    {
        _sec++;
        var test = 2 / int.Parse("0");
        if (_sec < 7) return;
        _passwordNavigationService.Navigate(_password);
    }

    [RelayCommand]
    private void Loaded()
    {
        _sessionStore.InitTrackTouch();
        ExplorerHelper.KillExplorer();
    } 

    [RelayCommand] private void Closing() => ExplorerHelper.RunExplorer();

    [RelayCommand]
    private void StopTimer()
    {
        _timer.Tick -= Timer;
        _timer.Stop();
        _sec = 0;
    }

    [RelayCommand]
    private void StartTimer()
    {
        _timer.Stop();
        _sec = 0;
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer;
        _timer.Start();
    }

    public void Receive(ViewModelChangedMessage message)=>OnPropertyChanged(nameof(CurrentViewModel));   

    public void Receive(ModalViewModelChangedMessage message)
    {
        OnPropertyChanged(nameof(CurrentModalViewModel));
        OnPropertyChanged(nameof(IsModalOpen));
    }

    public void Receive(StandByChangedMessage message)=>IsStandBy = message.Value;
    public void Receive(ServerErrorChanged message)
    {
        OnPropertyChanged(nameof(IsServerError));
        OnPropertyChanged(nameof(ServerError));
    }
}
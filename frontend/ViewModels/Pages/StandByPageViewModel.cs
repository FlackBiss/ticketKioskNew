using System.Net.Http;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Lastik.Models;
using Lastik.Models.Loggining;
using Lastik.Models.Schedules.Entities;
using Lastik.ViewModels.Controls;
using MvvmNavigationLib.Services;

namespace Lastik.ViewModels.Pages;

public partial class StandByPageViewModel : BasePageViewModel
{
    private readonly DispatcherTimer _timer = new(DispatcherPriority.Normal);

    [ObservableProperty] private Uri? _media1;
    [ObservableProperty] private Uri? _media2;
    [ObservableProperty] private bool _switchMedia;

    private List<MainAd> _allMedia = [];
    private NavigationService<ScheduleListViewModel> _navigationmainPageNavigationService;
    private UserSessionStore _userSessionStore;
    private int _currentIndex = -1;

    public StandByPageViewModel(NavigationService<ScheduleListViewModel> mainPageNavigationService, UserSessionStore sessionStore, List<MainAd> ads) : base("РЕЖИМ ОЖИДАНИЯ")
    {
        _navigationmainPageNavigationService = mainPageNavigationService;
        _userSessionStore = sessionStore;
        _allMedia = ads;
        _timer.Tick += _timer_Tick;
    }

    [RelayCommand]
    private void Close()
    {
        _userSessionStore.AddAction("Выход из режима ожидания");
        _navigationmainPageNavigationService.Navigate();
    }

    [RelayCommand]
    private async Task Loaded()
    {

        if (_allMedia.Any(media => !media.MediaPath.EndsWith(".mp4")))
        {
            Media2 = new Uri(_allMedia.Last(media => !media.MediaPath.EndsWith(".mp4")).MediaPath);
            await Task.Delay(500);//Anim
        }
        await SwitchContent();
    }
    [RelayCommand]
    private void Unloaded()
    { 
        _timer.Stop();
        _timer.Tick -= _timer_Tick;
        Media1 = null;
        Media2 = null;
    }

    private async Task SwitchContent()
    {
        if (_allMedia.Count == 0) return;
        _timer.Stop();
        _currentIndex++;
        if (_currentIndex > _allMedia.Count - 1) _currentIndex = 0;
        SwitchMedia = !SwitchMedia;
        if (SwitchMedia)
        {
            Media1 = new Uri(_allMedia[_currentIndex].MediaPath);
            await Task.Delay(1000);//Anim
            Media2 = null;
        }
        else
        {
            Media2 = new Uri(_allMedia[_currentIndex].MediaPath);
            await Task.Delay(1000);//Anim
            Media1 = null;
        }
        _timer.Interval = TimeSpan.FromSeconds(10);
        _timer.Start();
    }
    private async void _timer_Tick(object? sender, EventArgs e)
    {
        try
        {
            await SwitchContent();
        }
        catch
        {
            // ignored
        }
    }
}
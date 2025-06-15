using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lastik.Models.Terminal;
using MvvmNavigationLib.Services;
using Application = System.Windows.Application;

namespace Lastik.ViewModels.Popups;

public partial class PasswordPopupViewModel(INavigationService closeModalNavigationService, TerminalStore terminalStore, string defaultPassword)
    : BasePopupViewModel(closeModalNavigationService)
{
    [ObservableProperty] private bool _isPinPadOpen = true;

    [RelayCommand]
    private async void Loaded()
    {
        try
        {
            _password = defaultPassword;
            var password = (await terminalStore.GetAsync()).Password;
            _password = password == 0 ? defaultPassword : password.ToString();
        }
        catch (Exception e)
        {
            // ignored
        }
    }

    private string _currentPassword = string.Empty;
    private string _password = string.Empty;
    public string CurrentPassword
    {
        get => _currentPassword;
        set
        {   
            SetProperty(ref _currentPassword, value);
            OnPropertyChanged(nameof(IsValid));
        }
    }

    public bool IsValid => CurrentPassword == _password;


    [RelayCommand]
    private void Exit() => Application.Current.Shutdown();

    [RelayCommand]
    private void RemoveSymbol()
    {
        if (CurrentPassword.Length > 0) CurrentPassword = CurrentPassword[..^1];
        OnPropertyChanged(nameof(IsValid));
    }

    [RelayCommand]
    private void AddSymbol(string symbol)
    {
        CurrentPassword += symbol;
        OnPropertyChanged(nameof(IsValid));
    }

    [RelayCommand]
    private void OpenPinPad()=>IsPinPadOpen = true;

    [RelayCommand]
    private void ClosePinPad()=>IsPinPadOpen = false;

}
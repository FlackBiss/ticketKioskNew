using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lastik.Models.Order;
using Lastik.Models.Terminal;
using MvvmNavigationLib.Services;

namespace Lastik.ViewModels.Popups;

public partial class ServerErrorPopupViewModel
    (CloseNavigationService<ServerErrorModalStore> closeModalNavigationService, TerminalStore terminalStore, string defaultText) 
    :BasePopupViewModel(closeModalNavigationService)
{
    [ObservableProperty] private string _contacts = defaultText;

    [RelayCommand]
    private async void Loaded()
    {
        try
        {
            var terminal = await terminalStore.GetAsync();
            if (!string.IsNullOrEmpty(terminal.Contacts))
            {
                Contacts = terminal.Contacts;
            }
        }
        catch (Exception e)
        {
            // ignored
        }
    }
}
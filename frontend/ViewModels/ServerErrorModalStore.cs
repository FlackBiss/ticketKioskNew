using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Lastik.Models;
using Lastik.Models.Terminal;
using Lastik.Utilities;
using Lastik.ViewModels.Popups;
using MvvmNavigationLib.Services;
using MvvmNavigationLib.Stores;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace Lastik.ViewModels;

public class ServerErrorChanged(ObservableObject? vm) : ValueChangedMessage<ObservableObject?>(vm);

public class ServerErrorModalStore : INavigationStore,IRecipient<ServerErrorMessage>
{
    private readonly CloseNavigationService<ServerErrorModalStore> _close;
    private readonly IMessenger _messenger;
    private readonly TerminalStore _terminalStore;
    private readonly string _defaultText;

    public ServerErrorModalStore(IMessenger messenger, TerminalStore terminalStore, string defaultText)
    {
        _close = new CloseNavigationService<ServerErrorModalStore>(this);
        _terminalStore = terminalStore;
        _defaultText = defaultText;
        _messenger = messenger;
        _messenger.RegisterAll(this);
    }

    public void GoBack()
    {
    }

    private ObservableObject? _currentViewModel;
    public ObservableObject? CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            
            if(_currentViewModel is not null && value is not null) return;
            _currentViewModel = value;
            _messenger.Send(new ServerErrorChanged(value));
        }
    }

    public MvvmNavigationLib.Utilities.StackQueue<ObservableObject> RecentViewModels { get; set; }
    public void Receive(ServerErrorMessage message)
    {
        CurrentViewModel = new ServerErrorPopupViewModel(_close, _terminalStore, _defaultText);
    }
}
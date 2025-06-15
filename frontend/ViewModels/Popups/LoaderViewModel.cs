using CommunityToolkit.Mvvm.Messaging;
using MvvmNavigationLib.Services;

namespace Lastik.ViewModels.Popups;

public class LoaderViewModel:BasePopupViewModel,IRecipient<CloseLoaderMessage>
{
    public LoaderViewModel(IMessenger messenger, INavigationService closeModalNavigationService) 
        : base(closeModalNavigationService)=>messenger.RegisterAll(this);

    public void Receive(CloseLoaderMessage message)=>CloseContainerCommand.Execute(false);
}
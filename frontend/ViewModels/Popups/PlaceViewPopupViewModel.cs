using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Lastik.Models.Geometry.Entities.New;
using Lastik.Models.Loggining;
using Lastik.ViewModels.HallGeometry;
using MvvmNavigationLib.Services;

namespace Lastik.ViewModels.Popups;

public class PlaceChosenMessage(SchemeDataJson place) : ValueChangedMessage<SchemeDataJson>(place);
public class PlaceDeclinedMessage(SchemeDataJson place) : ValueChangedMessage<SchemeDataJson>(place);

public partial class PlaceViewPopupViewModel(
    INavigationService closeModalNavigationService, 
    IMessenger messenger,
    SchemeDataJson place,
    UserSessionStore sessionStore) 
    :BasePopupViewModel(closeModalNavigationService)
{
    [ObservableProperty] private SchemeDataJson _place = place;

    [RelayCommand]
    private void ChoosePlace()
    {
        sessionStore.AddAction($"Выбрано место: секция - {Place.Section}, ряд - {Place.Row}, место - {Place.SeatNumber}");
        messenger.Send(new PlaceChosenMessage(Place));
        CloseContainerCommand.Execute(false);
    }

    [RelayCommand]
    private void DeclinePlace()
    {
        messenger.Send(new PlaceDeclinedMessage(Place));
        CloseContainerCommand.Execute(false);
    }
}
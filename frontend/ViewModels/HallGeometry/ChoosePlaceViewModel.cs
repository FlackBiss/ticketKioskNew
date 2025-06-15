
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lastik.Models.Cart.Entities;
using Lastik.Models.Geometry.Entities;
using Lastik.Models.Geometry.Stores;
using Lastik.Models.Schedules.Entities;
using Lastik.Models.Tickets.Entities;
using Lastik.Models.Tickets.Stores;
using Lastik.ViewModels.Controllers;
using Lastik.ViewModels.Pages;
using MvvmNavigationLib.Services;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CommunityToolkit.Mvvm.Messaging;
using Lastik.ViewModels.Popups;
using MvvmNavigationLib.Stores;
using Lastik.Models.Geometry.Entities.New;
using Lastik.Models.Loggining;

namespace Lastik.ViewModels.HallGeometry;

public partial class ChoosePlaceViewModel 
    : BasePageViewModel, IRecipient<PlaceDeclinedMessage>,IRecipient<PlaceChosenMessage>
{
    [ObservableProperty] private Schedule _schedule;
    [ObservableProperty] private Schedule? _hallGeometry;
    [ObservableProperty]
    private ObservableCollection<SchemeDataJson> _selectedPlaces= [];
    [ObservableProperty] private List<SchemeDataJson> _places = [];
    [ObservableProperty] private List<PriceValue> _prices = [];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TotalPrice))]
    [NotifyPropertyChangedFor(nameof(IsPlacesNotEmpty))]
    private ObservableCollection<SchemeDataJson> _confirmedPlaces = [];

    public int TotalPrice => ConfirmedPlaces.Sum(p => p.Price);
    public bool IsPlacesNotEmpty => ConfirmedPlaces.Count > 0;

    private readonly LoaderController _loaderController;
    private readonly GeometryStore _geometryStore;
    private readonly GoBackNavigationService<NavigationStore> _goBack;
    private readonly ParameterNavigationService<CartPageViewModel,(Schedule, IEnumerable<SchemeDataJson>)> _toMakeOrderNavigationService;
    private readonly ParameterNavigationService<PlaceViewPopupViewModel,SchemeDataJson> _toPlaceView;
    private readonly UserSessionStore _sessionStore;

    public ChoosePlaceViewModel(LoaderController loaderController,
        Schedule schedule,
        GeometryStore geometryStore,
        GoBackNavigationService<NavigationStore> goBack,
        ParameterNavigationService<CartPageViewModel,(Schedule, IEnumerable<SchemeDataJson>)> toMakeOrderNavigationService,
        ParameterNavigationService<PlaceViewPopupViewModel, SchemeDataJson> toPlaceView,
        UserSessionStore sessionStore,
        IMessenger messenger) : base("ВЫБОР МЕСТА")
    {
        _loaderController = loaderController;
        _geometryStore = geometryStore;
        _goBack = goBack;
        _toMakeOrderNavigationService = toMakeOrderNavigationService;
        _toPlaceView = toPlaceView;
        _schedule = schedule;
        _sessionStore = sessionStore;
        messenger.RegisterAll(this);
    }



    [RelayCommand]
    private async Task Load()
    {
            
        _loaderController.ShowLoader();

        SelectedPlaces.Clear();
        OnPropertyChanged(nameof(IsPlacesNotEmpty));
        OnPropertyChanged(nameof(TotalPrice));

        if (Schedule.Hall?.Id is null) return;

        HallGeometry = await _geometryStore.GetAsync(Schedule.Id);

        //_tickets = await _ticketsStore.GetAsync(Schedule.Uuid);
        //Prices = _tickets.ToPrices();
        Places = HallGeometry.SchemeDataJson;
        SelectedPlaces.CollectionChanged += SelectedPlaces_CollectionChanged;
        ConfirmedPlaces.CollectionChanged += ConfirmedPlaces_CollectionChanged;

        _loaderController.CloseLoader();
    }

    private void ConfirmedPlaces_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(TotalPrice));
        OnPropertyChanged(nameof(IsPlacesNotEmpty));
    }

    [RelayCommand]
    private void GoBack() => _goBack.Navigate();

    [RelayCommand] private void Unload()
    {
        SelectedPlaces.CollectionChanged -= SelectedPlaces_CollectionChanged;
        ConfirmedPlaces.CollectionChanged -= ConfirmedPlaces_CollectionChanged;
    }

    [RelayCommand] private void RemovePlace(SchemeDataJson selectedPlace)
        => SelectedPlaces.Remove(selectedPlace);

    [RelayCommand] private void ToMakeOrder()
    {
        _sessionStore.AddAction("Переход к оформлению");
        _toMakeOrderNavigationService.Navigate((Schedule, ConfirmedPlaces));
    }

    private void SelectedPlaces_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add 
            when e.NewItems?.Count > 0
                 && e.NewItems[0] is SchemeDataJson place:
                _toPlaceView.Navigate(place);
                break;
            case NotifyCollectionChangedAction.Remove 
            when e.OldItems?.Count > 0
                 && e.OldItems[0] is SchemeDataJson declinedPlace:
                ConfirmedPlaces.Remove(declinedPlace);
                break;
        }

        
    }

    public void Receive(PlaceChosenMessage message)
    {
        ConfirmedPlaces.Add(message.Value);
    }

    public void Receive(PlaceDeclinedMessage message)
    {
        SelectedPlaces.Remove(message.Value);
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CustomKeyboard.Helpers;
using Lastik.Models.Cart.Entities;
using Lastik.Models.Cart.Stores;
using Lastik.ViewModels.Controls;
using Lastik.ViewModels.HallGeometry;
using Lastik.ViewModels.Popups;
using MainComponents.Components;
using MvvmNavigationLib.Services;
using MvvmNavigationLib.Stores;
using System.Windows.Input;
using System.Windows.Threading;
using FreeKassaPayOnline.Enums;
using Lastik.Models.Order;
using Lastik.Models.Geometry.Entities.New;
using Lastik.Models.Schedules.Entities;
using Lastik.Models.Schedules.Stores;

namespace Lastik.ViewModels.Pages;


public enum PaymentStage
{
    Cart,
    UserData,
    Terms
}
public static class MapPaymentStageExtensions
{
    public static void MapPaymentStage(this PaymentStage paymentStage, Action promoCode, Action userData, Action terms)
    {
        switch (paymentStage)
        {
            case PaymentStage.Cart:
                promoCode();
                break;
            case PaymentStage.UserData:
                userData();
                break;
            case PaymentStage.Terms:
                terms();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(paymentStage), paymentStage, null);
        }
    }

    public static T MapPaymentStage<T>(this PaymentStage paymentStage, Func<T> promoCode, Func<T> userData, Func<T> terms) => paymentStage switch
    {
        PaymentStage.Cart => promoCode(),
        PaymentStage.UserData => userData(),
        PaymentStage.Terms => terms(),
        _ => throw new ArgumentOutOfRangeException(nameof(paymentStage), paymentStage, null)
    };

}

public partial class CartPageViewModel : BasePopupViewModel
{
    public CartPageViewModel(
        CloseNavigationService<ModalNavigationStore> close,
        IParameterNavigationService<Cart> openModalNavigationService,
        CartStore cartStore, 
        Schedule cartPreview,
        IEnumerable<SchemeDataJson> placeViewModels,
        INavigationService initialNavigationService,
        OrderStore orderStore) : base(close)
    {
        _initialNavigationService = initialNavigationService;
        _openModalNavigationService = openModalNavigationService;
        _cartPreview = cartPreview;
        _placeViewModels = placeViewModels.ToList();
        TotalPrice = placeViewModels.Sum(pv => pv.Price);
    }

    public CartPageViewModel(
        CloseNavigationService<ModalNavigationStore> close,
        IParameterNavigationService<Cart> openModalNavigationService,
        CartStore cartStore, 
        Schedule cartPreview,
        INavigationService initialNavigationService,
        OrderStore orderStore) : base(close)
    {
        _initialNavigationService = initialNavigationService;
        _openModalNavigationService = openModalNavigationService;
        _cartPreview = cartPreview;
        _placeViewModels = new List<CartItem>
        {
            new()
            {
                Price = cartPreview.StartPrice ?? 0,
                Name = cartPreview.Title,
                EventId = cartPreview.Id,
                Count = 1,
                AvailableCount = CartPreview.PlacesHave ?? 0,
                Place = "Нет",
                Type = cartPreview.Type
            }
        };
        
        TotalPrice = cartPreview.StartPrice;
    }


    private INavigationService _initialNavigationService;
    private IParameterNavigationService<Cart> _openModalNavigationService;
    private readonly DispatcherTimer _timer = new(DispatcherPriority.Normal);

    [ObservableProperty] private TimeSpan _remainingTime = new(0, 0, 20, 0);
    [ObservableProperty, NotifyPropertyChangedFor(nameof(TotalPrice))] private static IEnumerable<object>? _placeViewModels;
    [ObservableProperty] private Schedule _cartPreview;
    [ObservableProperty] private Cart? _cart;
    [ObservableProperty, NotifyPropertyChangedFor(nameof(ForwardButtonText))] private int? _totalPrice = _placeViewModels?.Sum(vm =>
    {
        return vm switch
        {
            CartItem cartItem => cartItem.Price * cartItem.Count,
            SchemeDataJson dataJson => dataJson.Price,
            _ => 0
        };
    }) ?? 0;

    [ObservableProperty] private bool _isKeyboardOpen;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(GoForwardCommand))]
    [NotifyPropertyChangedFor(nameof(ForwardButtonText))]
    [NotifyPropertyChangedFor(nameof(HeaderText))]
    private PaymentStage _paymentStage = PaymentStage.Cart;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(GoForwardCommand))]
    private bool _hasValidationErrors;
    
    [ObservableProperty] private UserFormViewModel _userForm = new();
    
    public string HeaderText => PaymentStage.MapPaymentStage(
        () => "ОФОРМЛЕНИЕ",
        () => "ВВЕДИТЕ ПЕРСОНАЛЬНЫЕ ДАННЫЕ",
        () => "ОБРАБОТКА ПЕРСОНАЛЬНЫХ ДАННЫХ");
    public string ForwardButtonText => PaymentStage is PaymentStage.UserData ? $"ОПЛАТИТЬ ПО КАРТЕ" : "ДАЛЕЕ";

    public bool CanGoForward => PaymentStage is PaymentStage.Cart || 
                                PaymentStage is PaymentStage.UserData && 
                                !HasValidationErrors;


    partial void OnPaymentStageChanged(PaymentStage value) => CloseKeyboard();

    [RelayCommand]
    private void CloseKeyboard()
    {
        IsKeyboardOpen = false;
        KeyboardManager.CurrentViewModel.SetLayout(KeyboardLayoutNames.Russian);
    }

    [RelayCommand]
    private void GotFocus(KeyboardFocusChangedEventArgs args)
    {
        if (args.NewFocus is not PlaceholderTextBox placeholder) return;
        IsKeyboardOpen = true;
        var layout = placeholder.Tag switch
        {
            "ru" => "Russian",
            "en" => "English",
            "smb" => "Symbol",
            _ => string.Empty
        };
        if(string.IsNullOrEmpty(layout)) return;
        KeyboardManager.SetLayout(layout);

    }



    [RelayCommand]
    private async Task Load()
    {
        UserForm.ErrorsChanged += (sender, args) =>
        {
            HasValidationErrors = UserForm.HasErrors;
        };

        Cart = CreateCart();
        
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += _timer_Tick;
        _timer.Start();
    }

    [RelayCommand]
    private void Increase(object item)
    {
        if (item is CartItem cartItems)
        {
            if ((cartItems.Count < cartItems.AvailableCount) || 
                (_cartPreview.TypeEnum == ScheduleStore.ScheduleTicketType.TicketsUnlimited))
                cartItems.Count++;
            TotalPrice = cartItems.Price * cartItems.Count;
        }
    }

    [RelayCommand]
    private void Decrease(object item)
    {
        if (item is CartItem { Count: > 1 } cartItems)
        {
            cartItems.Count--;
            TotalPrice = cartItems.Price * cartItems.Count;
        }
    }
    private Cart CreateCart()
    {
        if (PlaceViewModels is List<CartItem> cartItems)
        {
            return new Cart
            {
                Items = cartItems
            };
        }

        if (PlaceViewModels is not List<SchemeDataJson> dataJsons)
            return new Cart();

        var cart = new Cart
        {
            TotalPrice = dataJsons.Sum(i => i.Price),
            Items = []
        };

        foreach (var placeViewModel in dataJsons)
        {
            var cartItem = new CartItem
            {
                Price = placeViewModel.Price,
                Uuid = placeViewModel.Uuid,
                PlaceToPrint = $"{CartPreview.Title} – {CartPreview.DateTimeAt:g} – Секция {placeViewModel.Section}, ряд {placeViewModel.Row}, место {placeViewModel.SeatNumber}",
                Place = $"Секция {placeViewModel.Section}, ряд {placeViewModel.Row}, место {placeViewModel.SeatNumber}",
                EventId = _cartPreview.Id,
                Type = CartPreview.Type
            };

            if (_cartPreview.TypeEnum == ScheduleStore.ScheduleTicketType.SeatsByTickets)
            {
                cartItem.Count = 1;
            }
            else
            {
                //cartItem.Quantity 
            }

            cart.Items.Add(cartItem);
        }

        

        return cart;
    }

    private void _timer_Tick(object? sender, EventArgs e)
    {
        if (RemainingTime == TimeSpan.Zero) _initialNavigationService.Navigate();
        else RemainingTime = RemainingTime.Subtract(TimeSpan.FromSeconds(1));
    }

    [RelayCommand]
    private void GoBack()
    {
        PaymentStage.MapPaymentStage(
            () => CloseContainerCommand.Execute(null),
            () => PaymentStage = PaymentStage.Cart,
            () => PaymentStage = PaymentStage.UserData);
    }

    [RelayCommand(CanExecute = nameof(CanGoForward))]
    private void GoForward(PaymentType type)
    {
        if (Cart is null) return;
        Cart.PaymentType = type;
        PaymentStage.MapPaymentStage(
            () => PaymentStage = PaymentStage.UserData, 
            ToPayment, 
            () => { });
    }

    [RelayCommand]
    private async Task UnLoad()
    {
        _timer.Stop();
        _timer.Tick -= _timer_Tick;
        //cartPreview.Items.ForEach(c => c.Quantity = 0);
        //await cartStore.SendAsync(cartPreview);
    }

    protected override void OnClosed()=>CloseKeyboard();

    private async void ToPayment()
    {
        if (Cart?.TotalPrice == 0)
            Cart.TotalPrice = TotalPrice ?? 0;

        if(!UserForm.Validate()) return;
        if (Cart is null) return;
        CloseKeyboard();
        _openModalNavigationService.Navigate(Cart);

        if (Cart.Items != null)
            foreach (var cartItem in Cart.Items)
            {
                cartItem.Email = UserForm.Email;
                cartItem.Name = UserForm.Name;
                cartItem.Surname = UserForm.Surname;
            }
    }
}
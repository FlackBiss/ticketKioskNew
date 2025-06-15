using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FreeKassaPayOnline.Model;
using Lastik.Models;
using Lastik.Models.Cart.Entities;
using Lastik.Models.Cart.Stores;
using Lastik.Models.Kassa;
using MvvmNavigationLib.Services;
using System.Net.Http;
using FreeKassaPayOnline.Enums;
using Lastik.Models.Loggining;

namespace Lastik.ViewModels.Popups
{
    public enum PaymentState
    {
        Processing,
        Success,
        Fail
    }
    public partial class PaymentPopupViewModel : BasePopupViewModel
    {
        [ObservableProperty] private Cart _cart;
        [ObservableProperty] private int _partPayment;
        [ObservableProperty] private PaymentState _state = PaymentState.Processing;
        private readonly IKassaService _paymentService;
        private readonly INavigationService _toMain;
        private CartStore _cartStore;
        private UserSessionStore _sessionStore;

        
        public PaymentPopupViewModel(INavigationService closeModalNavigationService, Cart cart,
            IKassaService paymentService,INavigationService toMain, CartStore cartStore, UserSessionStore sessionStore) : base(closeModalNavigationService)
        {
            _paymentService = paymentService;
            _cartStore = cartStore;
            _toMain = toMain;
            _cart = cart;
            _sessionStore = sessionStore;
            _paymentService.OnError += PaymentServiceOnOnError;
            _paymentService.OnSuccess += PaymentService_OnSuccess;
            _paymentService.OnPartPayment += PaymentServiceOnOnPartPayment;
        }

        private void PaymentServiceOnOnPartPayment(int denomination)
        {
            PartPayment += denomination;
        }

        private async void PaymentService_OnSuccess()
        {
            _sessionStore.AddAction("Успешная оплата");
            try
            {
                var res = await _cartStore.SendAsync(Cart, _paymentService.GetPaperStatus());
                State = res.Contains(false) ? PaymentState.Fail : PaymentState.Success;
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        private void PaymentServiceOnOnError(string errordescription)
        {
            _sessionStore.AddAction($"Ошибка оплаты: {errordescription}");
            State = PaymentState.Fail;
        }

        [RelayCommand]
        private void Loaded()
        {
            List<BasketModel> basket = [];
            foreach (var cartItem in Cart.Items ?? [])
            {
                basket.Add(new BasketModel
                {
                    Name = cartItem.PlaceToPrint,
                    Cost = cartItem.Price,
                    Quantity = cartItem.Count,
                    TaxType = Tax1Value.Default,
                    PaymentItemSign = PaymentItemSign.Default
                });
            }
            _paymentService.StartPayment(basket, Cart.PaymentType);
        }

        protected override void OnClosed()
        {
            _paymentService.OnError -= PaymentServiceOnOnError;
            _paymentService.OnSuccess -= PaymentService_OnSuccess;
            if (State == PaymentState.Success)
                _toMain.Navigate();
            base.OnClosed();
        }
    }
}

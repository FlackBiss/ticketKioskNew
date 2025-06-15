using FreeKassaPayOnline.Enums;
using FreeKassaPayOnline.Model;

namespace Lastik.Models.Kassa;

public interface IKassaService
{
    void StartPayment(List<BasketModel> basketModels, PaymentType paymentType = PaymentType.Sberbank);
    bool GetPaperStatus();

    delegate void Error(string errorDescription);
    delegate void PartPayment(int denomination);
    delegate void Success();

    event Error? OnError;
    event Success? OnSuccess;
    event PartPayment? OnPartPayment;

}
using FreeKassaPayOnline.Enums;
using FreeKassaPayOnline.Model;

namespace Lastik.Models.Kassa;

public class FakeKassaService:IKassaService
{
    public async void StartPayment(List<BasketModel> basketModels, PaymentType paymentType = PaymentType.Sberbank)
    {
        await Task.Delay(1000);
        //var rnd = new Random();
        //var res =rnd.NextDouble();
        //if(res>0.2) 
            OnSuccess?.Invoke();
        //else OnError?.Invoke("Что-то пошло не так. Попробуйте ещё раз");
    }

    public bool GetPaperStatus()
    {
        return true;
    }

    public event IKassaService.Error? OnError;
    public event IKassaService.Success? OnSuccess;
    public event IKassaService.PartPayment? OnPartPayment;
}
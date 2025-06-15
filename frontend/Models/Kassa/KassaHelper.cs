using FreeKassaPayOnline.Enums;
using FreeKassaPayOnline.Model;
using FreeKassaPayOnline.Models;
using FreeKassaPayOnlineFramework;
using Lastik.Helpers.Logging;


namespace Lastik.Models.Kassa;

public class KassaHelper : IKassaService
{
    public event IKassaService.Error? OnError;
    public event IKassaService.Success? OnSuccess;
    public event IKassaService.PartPayment? OnPartPayment;
    private KktManager? _kassaManager;
    private PayModel _payModel;
    private List<BasketModel> _basketModels;

    private readonly ILoggingService _logger;

    public KassaHelper(ILoggingService logger)
    {
        _logger = logger;
        InitKassa();
    }

    private void InitKassa()
    {
        _kassaManager = new KktManager();

        _kassaManager.Error += KassaOnError;
        _kassaManager.ResultPayment += KassaManagerOnSuccessfullyPayment;
        _kassaManager.PartPayment += KassaManagerOnPartPayment;

        _kassaManager.ConnectKassa();
    }

    private void KassaManagerOnPartPayment(int sum)
    {
        OnPartPayment?.Invoke(sum);
    }

    public void StartPayment(List<BasketModel> basketModels, PaymentType paymentType = PaymentType.Sberbank)
    {
        var payModel = new PayModel();

        var sum = basketModels.Sum(f => (decimal)f.Quantity * f.Cost);

        if (basketModels.Count == 0)
        {
            const string err = "Basket is empty";
            OnError?.Invoke(err);
            _logger.Log(err);
            return;
        }

        if (sum == 0)
        {
            const string err = "Sum is 0";
            OnError?.Invoke(err);
            _logger.Log(err);
            return;
        }

        payModel.PaymentType = paymentType switch
        {
            PaymentType.Sberbank => SummType.ElectronicallyMir,
            PaymentType.CashValidator => SummType.Cash,
            _ => payModel.PaymentType
        };
        payModel.Sum = sum;

        _payModel = payModel;
        _basketModels = basketModels;

        _kassaManager?.StartPayment(paymentType, (long)sum * 100);
    }

    public bool GetPaperStatus()
    {
        return _kassaManager != null && _kassaManager.GetPrinterPaperStatus();
    }

    private void KassaManagerOnSuccessfullyPayment(int errorCode)
    {
        if (errorCode != 4403)
        {
            _logger.Log("Операция оплаты не завершена с кодом ошибки " + errorCode);
            OnError?.Invoke($"Код ошибки - {errorCode}");
            return;
        }

        _logger.Log("Оплата прошла успешно");

        var res = _kassaManager != null && _kassaManager.RegisterReceipt(_payModel, _basketModels);
        _logger.Log(res ? "Чек зарегистрирован" : "Чек не зарегистрирован");

        OnSuccess?.Invoke();
    }

    private void KassaOnError(int errorCode, string? errorDescription, bool isShow)
    {
        OnError?.Invoke(errorDescription);
        _logger.Log($"{errorDescription} - [{errorCode}]");
    }
}
using System.Net.Http;
using Lastik.Helpers.Logging;
using Lastik.Models.Cart.Entities;
using Lastik.Models.Terminal;

namespace Lastik.Models.Cart.Stores;

public class CartStore(IApiHttpClient httpClient,ILoggingService loggingService, int terminalId)
{
    public async Task<List<bool?>> SendAsync(Entities.Cart cartPreview, bool paperStatus)
    {
        var result = new List<bool?>();
        if (cartPreview.Items == null) return result;

        foreach (var item in cartPreview.Items)
        {
            var response = await httpClient.SendCart(item);
            result.Add(response.IsSuccessful);
        }

        result.Add((await httpClient.SendKktState(new TerminalEdit
        {
            Id = terminalId,
            Kkt = paperStatus
        })).IsSuccessful);

        return result;
    }
    //public async Task<Entities.Cart> GetAsync() => (await httpClient.GetCart()).GetContent(loggingService);
}
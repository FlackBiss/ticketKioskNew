using System.Net.Http;
using Lastik.Models.Token;

namespace Lastik.Utilities
{
    public class AuthHeaderHandler(TokenStore tokenStore) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = tokenStore.GetToken();
            request.Headers.Add("X-API-KEY", token);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using Lastik.Helpers.Logging;

namespace Lastik.Utilities
{
    public class ServerErrorMessage;

    public class LoggingHttpHandler(ILoggingService logger,IMessenger messenger):DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                return await base.SendAsync(request, cancellationToken);
            }
            catch (Exception e)
            {
                logger.Log(e);
                messenger.Send(new ServerErrorMessage());
                return new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    RequestMessage = request,
                    Content = new StringContent("{}")
                };
            }
        }
    }
}

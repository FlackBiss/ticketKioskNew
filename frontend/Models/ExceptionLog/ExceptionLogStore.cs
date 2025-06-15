using Lastik.Helpers.Logging;
using Lastik.Models.Terminal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lastik.Models.ExceptionLog
{
    internal class ExceptionLogStore(IApiHttpClient apiHttpClient, ILoggingService logger, int terminalId)
    {
        public async Task<ExceptionLogResponse> SendAsync(Exception exception)
        {
            return (await apiHttpClient.SendExceptionLogs(new ExceptionLog
            {
                Name = exception.Message,
                Code = "",
                Comment = exception.Source ?? "",
                TerminalId = terminalId,
                Log = exception.StackTrace ?? ""
            })).GetContent(logger);
        }
    }
}

using Lastik.Helpers.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lastik.Models.Terminal
{
    public class TerminalStore(IApiHttpClient apiHttpClient, ILoggingService logger, int id)
    {
        public async Task<Terminal> GetAsync() => (await apiHttpClient.GetTerminal(id)).GetContent(logger);
    }
}

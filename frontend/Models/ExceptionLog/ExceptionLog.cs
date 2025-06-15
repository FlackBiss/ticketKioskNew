using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lastik.Models.ExceptionLog
{
    public class ExceptionLog
    {
        [JsonProperty("terminalId")]
        public int TerminalId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("log")]
        public string Log { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }
    }
}

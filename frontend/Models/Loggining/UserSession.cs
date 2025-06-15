using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lastik.Models.Loggining
{
    public class UserSession
    {
        [JsonProperty("startAt")]
        public DateTime StartAt { get; set; }

        [JsonProperty("endAt")]
        public DateTime EndAt { get; set; }

        [JsonProperty("terminalId")]
        public int TerminalId { get; set; }

        [JsonProperty("allEvent")]
        public List<UserAction> AllEvent { get; set; }
    }
}

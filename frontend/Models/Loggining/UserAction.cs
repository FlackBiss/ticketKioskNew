using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lastik.Models.Loggining
{
    public class UserAction
    {
        [JsonProperty("objectName")]
        public string? ObjectName { get; set; }

        [JsonProperty("dateAt")]
        public DateTime DateAt { get; set; }

        [JsonProperty("coordinates")]
        public string? Coordinates { get; set; }

        [JsonProperty("response")]
        public string? Response { get; set; }
    }
}

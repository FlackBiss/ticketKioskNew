using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lastik.Models.Terminal
{
    public class TerminalEdit
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("kkt")]
        public bool Kkt { get; set; }
    }
}

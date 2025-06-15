using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lastik.Models.Terminal
{
    public class Terminal
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("network")]
        public bool Network { get; set; }

        [JsonProperty("kkt")]
        public bool Kkt { get; set; }

        [JsonProperty("ipAddress")]
        public string IpAddress { get; set; }

        [JsonProperty("contacts")]
        public string Contacts { get; set; }

        [JsonProperty("password")]
        public int Password { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lastik.Models.Schedules.Entities
{
    public class MainAd
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("media")]
        public string? MediaUrl { get; set; }

        [JsonIgnore]
        public string? MediaPath { get; set; }
    }
}

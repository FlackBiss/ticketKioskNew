using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lastik.Models.Schedules.Entities
{
    public class MultiLanguageText
    {
        [JsonProperty("ru")]
        public string? Ru { get; set; }

        [JsonProperty("en")]
        public string? En { get; set; }
    }
}

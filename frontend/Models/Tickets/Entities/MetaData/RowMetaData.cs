using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lastik.Models.Tickets.Entities.MetaData
{
    public class RowMetaData
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("uuid")]
        public string? Uuid { get; set; }
    }
}

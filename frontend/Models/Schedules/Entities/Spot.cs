using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lastik.Models.Schedules.Entities
{
    public class Spot
    {
        [JsonProperty("uuid")]
        public string? Uuid { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("is_deleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("name")]
        public MultiLanguageText? Name { get; set; }

        [JsonProperty("short_name")]
        public MultiLanguageText? ShortName { get; set; }

        [JsonProperty("description")]
        public MultiLanguageText? Description { get; set; }
    }
}

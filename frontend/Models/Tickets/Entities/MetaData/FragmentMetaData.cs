using Lastik.Models.Schedules.Entities;
using Newtonsoft.Json;

namespace Lastik.Models.Tickets.Entities.MetaData
{
    public class FragmentMetaData
    {
        [JsonProperty("name")]
        public MultiLanguageText? Name { get; set; }

        [JsonProperty("uuid")]
        public string? Uuid { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lastik.Models.Geometry.Entities
{
    public class RowGeometry
    {
        [JsonProperty("bb")]
        public SizeCoordinates? Bb { get; set; }

        [JsonProperty("uuid")]
        public string? Uuid { get; set; }

        [JsonProperty("places")]
        public List<PlaceGeometry>? Places { get; set; }
    }
}

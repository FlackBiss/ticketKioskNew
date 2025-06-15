using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lastik.Models.Geometry.Entities
{
    public class SectorGeometry
    {
        [JsonProperty("bb")]
        public SizeCoordinates? Bb { get; set; }

        [JsonProperty("path")]
        public string? Path { get; set; }

        [JsonProperty("uuid")]
        public string? Uuid { get; set; }

        [JsonProperty("fragments")]
        public List<FragmentGeometry>? Fragments { get; set; }
    }
}

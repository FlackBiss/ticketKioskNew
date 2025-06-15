using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lastik.Models.Geometry.Entities
{
    public class FragmentGeometry
    {
        [JsonProperty("bb")]
        public SizeCoordinates? Bb { get; set; }

        [JsonProperty("rows")]
        public List<RowGeometry>? Rows { get; set; }

        [JsonProperty("uuid")]
        public string? Uuid { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lastik.Models.Geometry.Entities
{
    public class Geometry
    {
        [JsonProperty("bb")]
        public SizeCoordinates? SizeCoordinates { get; set; }

        [JsonProperty("objects")]
        public List<GeometryObject>? Objects { get; set; }

        [JsonProperty("sectors")]
        public List<SectorGeometry>? Sectors { get; set; }
    }
}

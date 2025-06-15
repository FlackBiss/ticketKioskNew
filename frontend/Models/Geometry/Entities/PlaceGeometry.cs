using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lastik.Models.Geometry.Entities
{
    public class PlaceGeometry
    {
        [JsonProperty("h")]
        public double H { get; set; }

        [JsonProperty("w")]
        public double W { get; set; }

        [JsonProperty("cx")]
        public double Cx { get; set; }

        [JsonProperty("cy")]
        public double Cy { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("uuid")]
        public string? Uuid { get; set; }
    }
}

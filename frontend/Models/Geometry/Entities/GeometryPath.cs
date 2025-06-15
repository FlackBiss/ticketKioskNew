using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lastik.Models.Geometry.Entities
{
    public class GeometryPath
    {
        [JsonProperty("style")]
        public string? Style { get; set; }

        [JsonProperty("value")]
        public string? Value { get; set; }

        [JsonIgnore]
        public double StrokeThickness
        {
            get
            {
                var match = Regex.Match(Style, @"stroke-width:(\d+);");
                if (!match.Success || !double.TryParse(match.Groups[1].Value, out var res)) return 8;
                return res;
            }
        }
    }
}

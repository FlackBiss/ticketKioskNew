using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lastik.Models.Schedules.Entities;

namespace Lastik.Models.Geometry.Entities
{
    public class GeometryText
    {
        [JsonProperty("style")]
        public string? Style { get; set; }

        [JsonProperty("value")]
        public MultiLanguageText? Value { get; set; }

        [JsonIgnore]
        public double FontSize
        {
            get
            {
                var match = Regex.Match(Style, @"font-size:(\d+)px;");
                if (!match.Success || !double.TryParse(match.Groups[1].Value,out var res)) return 50;
                return res;
            }
        }
    }
}

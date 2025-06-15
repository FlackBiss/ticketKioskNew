using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lastik.Models.Geometry.Entities
{
    public class PlaceView
    {
        [JsonIgnore]
        private Random rnd = new Random();
        [JsonIgnore] 
        private List<int>? _color;
        [JsonProperty("color")]
        public List<int> Color { 
            get=>_color;
            set
            {
                if (value.Count != 3)
                {
                    _color = value;
                    return;
                }
                
                _color = GenerateColor(value);

            }
        }

        [JsonProperty("path")] 
        public string? UrlPath { get; set; }

        [JsonIgnore]
        public string? Path { get; set; }

        private List<int> GenerateColor(List<int> initialColor)
        {
            if (initialColor[0] < 120)
            {
                initialColor[0] += rnd.Next(60, 100);
            }
            else if (initialColor[1] < 120)
            {
                initialColor[1] += rnd.Next(60, 100);
                
            }
            else if (initialColor[2] < 120)
            {
                initialColor[2] += rnd.Next(60, 100);
            }
            return initialColor;
        }
    }
}

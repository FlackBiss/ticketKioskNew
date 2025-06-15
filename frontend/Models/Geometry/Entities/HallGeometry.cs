using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lastik.Models.Geometry.Entities;

public class HallGeometry
{
    [JsonProperty("geometry")]
    public Geometry? Geometry { get; set; }

    [JsonProperty("place_view")]
    public PlaceViews? PlaceViews { get; set; }



    public List<GeometryObject> PathGeometryObjects => Geometry?.Objects?.Where(o => o.Path is not null).ToList() ?? [];

    public List<GeometryObject> TextGeometryObjects => Geometry?.Objects?.Where(o => o.Text is not null && o.Path is null).ToList() ?? [];

}
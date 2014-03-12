using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Boxer.Data;
using Newtonsoft.Json;

namespace SpriteUtility.Data.Export
{
    [Serializable]
    public class PolygonGroupExport
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("polygons")]
        public List<PolygonExport> Polygons { get; set; }

        public PolygonGroupExport(PolygonGroup poly)
        {
            Name = poly.Name;
            Polygons = new List<PolygonExport>(poly.Children.Count);
            foreach (Polygon polygon in poly.Children)
            {
                Polygons.Add(new PolygonExport(polygon));
            }
        }
    }
}

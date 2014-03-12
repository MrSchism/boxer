using System;
using System.Collections.Generic;
using Boxer.Data;
using Newtonsoft.Json;

namespace SpriteUtility.Data.Export
{
    [Serializable]
    public class PolygonExport
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("points")]
        public List<PolyPointExport> Points { get; set; }

        public PolygonExport(Polygon poly)
        {
            Name = poly.Name;
            Points = new List<PolyPointExport>(poly.Children.Count);
            foreach (PolyPoint point in poly.Children)
            {
                Points.Add(new PolyPointExport(point));           
            }
        }
    }
}
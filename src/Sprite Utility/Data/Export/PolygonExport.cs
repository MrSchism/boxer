using System;
using System.Collections.Generic;
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
            Points = new List<PolyPointExport>(poly.Points.Count);
            foreach (var point in poly.Points)
            {
                Points.Add(new PolyPointExport(point));           
            }
        }
    }
}
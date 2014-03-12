using System;
using System.Collections.Generic;
using Boxer.Data;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using SpriteUtility.Data;
using SpriteUtility.Data.Export;

namespace SpriteUtility
{
    [Serializable]
    public class ImageFrameExport
    {
        [JsonProperty("center_point")]
        public Vector2 CenterPoint { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("is_open")]
        public bool IsOpen { get; set; }

        [JsonProperty("polygonGroups")]
        public List<PolygonGroupExport> PolygonGroups { get; set; }

        public ImageFrameExport(ImageFrame frame)
        {
            CenterPoint = new Vector2(frame.MappedCenterPointX, frame.MappedCenterPointY);
            PolygonGroups = new List<PolygonGroupExport>(frame.Children.Count);
            foreach (PolygonGroup polyGroup in frame.Children)
            {
                PolygonGroups.Add(new PolygonGroupExport(polyGroup));
            }
            Duration = frame.Duration;
            IsOpen = frame.IsOpen;
        }
    }
}
using System;
using System.Collections.Generic;
using Boxer.Data;
using Newtonsoft.Json;

namespace SpriteUtility.Data.Export
{
    [Serializable]
    public class ImageDataExport
    {
        [JsonProperty("image_file")]
        public string ImageFile { get; set; }

        [JsonProperty("frames")]
        public List<ImageFrameExport> Frames { get; set; }

        public ImageDataExport(ImageData data)
        {
            ImageFile = data.Filename;
            Frames = new List<ImageFrameExport>(data.Children.Count);
            foreach (ImageFrame frame in data.Children)
            {
                Frames.Add(new ImageFrameExport(frame));
            }
        }
    }
}
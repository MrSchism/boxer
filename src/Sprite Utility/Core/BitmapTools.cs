using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Boxer.Core
{
    public class BitmapTools
    {
        public static Rectangle TrimRect(Bitmap source, int minWidth = 0, int minHeight = 0)
        {
            Rectangle srcRect;
            BitmapData data = null;
            try
            {
                data = source.LockBits(new Rectangle(0, 0, source.Width, source.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                var buffer = new byte[data.Height * data.Stride];
                Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);

                int xMin = int.MaxValue,
                    xMax = int.MinValue,
                    yMin = int.MaxValue,
                    yMax = int.MinValue;

                var foundPixel = false;

                // Find xMin
                for (var x = 0; x < data.Width; x++)
                {
                    var stop = false;
                    for (var y = 0; y < data.Height; y++)
                    {
                        var alpha = buffer[y * data.Stride + 4 * x + 3];
                        if (alpha == 0) continue;
                        xMin = x;
                        stop = true;
                        foundPixel = true;
                        break;
                    }
                    if (stop)
                    {
                        break;
                    }
                }

                // Image is empty...
                if (!foundPixel)
                {
                    return Rectangle.Empty;
                }

                // Find yMin
                for (var y = 0; y < data.Height; y++)
                {
                    var stop = false;
                    for (var x = xMin; x < data.Width; x++)
                    {
                        var alpha = buffer[y * data.Stride + 4 * x + 3];
                        if (alpha == 0) continue;
                        yMin = y;
                        stop = true;
                        break;
                    }
                    if (stop)
                    {
                        break;
                    }
                }

                // Find xMax
                for (var x = data.Width - 1; x >= xMin; x--)
                {
                    var stop = false;
                    for (var y = yMin; y < data.Height; y++)
                    {
                        var alpha = buffer[y * data.Stride + 4 * x + 3];
                        if (alpha == 0) continue;
                        xMax = x;
                        stop = true;
                        break;
                    }
                    if (stop)
                    {
                        break;
                    }
                }

                // Find yMax
                for (var y = data.Height - 1; y >= yMin; y--)
                {
                    var stop = false;
                    for (var x = xMin; x <= xMax; x++)
                    {
                        var alpha = buffer[y * data.Stride + 4 * x + 3];
                        if (alpha == 0) continue;
                        yMax = y;
                        stop = true;
                        break;
                    }
                    if (stop)
                    {
                        break;
                    }
                }
                srcRect = Rectangle.FromLTRB(xMin, yMin, xMax + 1, yMax + 1);
            }
            finally
            {
                if (data != null)
                {
                    source.UnlockBits(data);
                }
            }

            return srcRect;
        }

        // Original Source: http://stackoverflow.com/a/4821100
        public static Bitmap Trim(Bitmap source, int minWidth = 0, int minHeight = 0)
        {
            var srcRect = TrimRect(source, minWidth, minHeight);
            var width = srcRect.Width < minWidth ? minWidth : srcRect.Width;
            var height = srcRect.Height < minHeight ? minHeight : srcRect.Height;
            if (width == 0 && height == 0)
            {
                // Empty frame
                return new Bitmap(1, 1);
            }

            var dest = new Bitmap(width, height);
            var destRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);
            using (var graphics = Graphics.FromImage(dest))
            {
                graphics.DrawImage(source, destRect, srcRect, GraphicsUnit.Pixel);
            }
            return dest;
        }
    }
}

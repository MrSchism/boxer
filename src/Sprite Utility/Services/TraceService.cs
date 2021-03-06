﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Linq;
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;
using fwd;

namespace Boxer.Services
{
    public static class TraceService
    {
        public static void SetDisplayUnitToSimUnitRatio(float simulationRatio)
        {
            ConvertUnits.SetDisplayUnitToSimUnitRatio(simulationRatio);
        }

        public static Shape CreateSimpleShape(Image imageBitmap, int timeout, StringBuilder spool, bool holeDetection = true, bool strict = false, TriangulationAlgorithm algorithm = TriangulationAlgorithm.Bayazit)
        {
            Shape shape = null;
            Action action = () =>
            {
                try
                {
                    shape = BuildShape(imageBitmap, spool, holeDetection, strict, algorithm);
                }
                catch
                {
                    spool.AppendFormat("Error creating shape for image");
                    spool.AppendLine();
                }
            };

            var result = action.BeginInvoke(null, null);
            if (!result.AsyncWaitHandle.WaitOne(timeout))
            {
                spool.AppendFormat("Timed out attempting to create shape");
                spool.AppendLine();
            }

            return shape;
        }

        public static Shape CreateComplexShape(Image imageBitmap, int timeout, StringBuilder spool, float hullTolerance, byte alphaTolerance, bool multiPartDetection, bool holeDetection = true, bool strict = false, TriangulationAlgorithm algorithm = TriangulationAlgorithm.Bayazit)
        {
            Shape shape = null;
            Action action = () =>
            {
                try
                {
                    shape = BuildShape(imageBitmap, spool, hullTolerance, alphaTolerance, multiPartDetection, holeDetection, strict, algorithm);
                }
                catch
                {
                    spool.AppendFormat("Error creating shape for image");
                    spool.AppendLine();
                }
            };

            var result = action.BeginInvoke(null, null);
            if (!result.AsyncWaitHandle.WaitOne(timeout))
            {
                spool.AppendFormat("Timed out attempting to create shape");
                spool.AppendLine();
            }

            return shape;
        }

        private static Shape BuildShape(Image imageBitmap, StringBuilder spool, bool holeDetection, bool strict, TriangulationAlgorithm algorithm)
        {
            var data = LoadImageData(imageBitmap);
            var polygon = PolygonTools.CreatePolygon(data, imageBitmap.Width, holeDetection);

            var polygons = new List<Vertices> { polygon };
            return ScaleConvertAndPartition(spool, strict, imageBitmap, polygons, algorithm);
        }

        private static Shape BuildShape(Image imageBitmap, StringBuilder spool, float hullTolerance, byte alphaTolerance, bool multiPartDetection, bool holeDetection = true, bool strict = false, TriangulationAlgorithm algorithm = TriangulationAlgorithm.Bayazit)
        {
            var data = LoadImageData(imageBitmap);
            var polygons = PolygonTools.CreatePolygon(data, imageBitmap.Width, hullTolerance, alphaTolerance, multiPartDetection, holeDetection);

            return ScaleConvertAndPartition(spool, strict, imageBitmap, polygons, algorithm);
        }


        private static Shape ScaleConvertAndPartition(StringBuilder spool, bool strict, Image image, IEnumerable<Vertices> polygons, TriangulationAlgorithm algorithm)
        {
            var scale = ConvertUnits.ToSimUnits(1, 1);
            var width = ConvertUnits.ToSimUnits(image.Width);
            var height = ConvertUnits.ToSimUnits(image.Height);
            var translation = new Vector2(-width, -height)*0.5f;

            var final = new List<List<Vector2>>();

            foreach (var polygon in polygons)
            {
                polygon.Scale(scale);
                polygon.Translate(translation);

                var thisPolygon = SimplifyTools.CollinearSimplify(polygon);

                if (strict)
                {
                    var errors = thisPolygon.CheckPolygon();
                    if (errors != PolygonError.NoError)
                    {
                        spool.AppendFormat("Invalid shape ({0})", errors);
                        return null;
                    }
                }
                
                try
                {
                    var partition = Triangulate.ConvexPartition(thisPolygon, algorithm);
                    var vertices = partition.Select(verts => verts.Select(v => new Vector2(v.X, v.Y)).ToList());
                    final.AddRange(vertices);
                }
                catch
                {
                    spool.AppendFormat("Cannot triangulate polygon");
                    spool.AppendLine();
                }
            }
            
            var shape = new Shape
            {
                Vertices = final
            };
            return shape;
        }

        public static unsafe uint[] LoadImageData(Image image)
        {
            var bitmap = (Bitmap)image;
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Size.Width, bitmap.Size.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            var data = new uint[image.Width * image.Height];
            var ptr = (uint*)bitmapData.Scan0.ToPointer();
            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var index = x + y * image.Width;
                    data[index] = ptr[index];
                }
            }
            bitmap.UnlockBits(bitmapData);
            return data;
        }
    }
}

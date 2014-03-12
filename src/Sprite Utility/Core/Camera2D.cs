using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boxer.Core
{
    public class Camera2D
    {
        protected int _zoomIndex; // Camera Zoom
        public Matrix _transform; // Matrix Transform
        public Vector2 _pos; // Camera Position
        protected float _rotation; // Camera Rotation
        protected float[] _zoomValues = new float[] { 0.001f, 0.002f, 0.003f, 0.004f, 0.005f, 0.007f, 0.01f, 0.015f, 0.02f, 0.03f, 0.04f, 0.05f, 0.0625f, 0.0833f, 0.125f, 0.1667f, 0.25f, 0.3333f, 0.5f, 0.6667f, 1, 2, 3, 4, 5, 6, 7, 8, 12, 16, 32 };
        protected GraphicsDevice _graphicsDevice;
        public Camera2D(GraphicsDevice graphicsDevice)
        {
            _zoomIndex = Properties.Settings.Default.ZoomIndex;
            _rotation = 0.0f;
            _pos = Properties.Settings.Default.CameraPos;
            _graphicsDevice = graphicsDevice;
        }

        public float Zoom
        {
            get { return _zoomValues[_zoomIndex]; }
        }

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public Vector2 GetWorldCoordinates(Vector2 vector2)
        {
            var matrix = get_transformation();
            matrix = Matrix.Invert(matrix);
            return Vector2.Transform(vector2, matrix);
        }

        public Vector2 GetScreenCoordinates(Vector2 vector2)
        {
            var matrix = get_transformation();
            return Vector2.Transform(vector2, matrix);
        }

        public void ResetZoom()
        {
            SetZoom(20);
        }

        public void SetZoom(int zoom, Point? mouseLocation = null)
        {
            if (mouseLocation != null)
            {
                var cursor = new Vector2(mouseLocation.Value.X, mouseLocation.Value.Y);

                var transformMatrix = get_transformation();
                transformMatrix = Matrix.Invert(transformMatrix);

                _zoomIndex = zoom;

                var worldCursorBeforeZoom = Vector2.Transform(cursor, transformMatrix);

                transformMatrix = get_transformation();
                transformMatrix = Matrix.Invert(transformMatrix);

                var worldCursorAfterZoom = Vector2.Transform(cursor, transformMatrix);

                var pan = worldCursorBeforeZoom - worldCursorAfterZoom;

                _pos += pan;
            }
            else
            {
                _zoomIndex = zoom;
            }
            SaveCamera();
        }

        public void SaveCamera()
        {
            Properties.Settings.Default.ZoomIndex = _zoomIndex;
            Properties.Settings.Default.CameraPos = _pos;
            Properties.Settings.Default.Save();
        }

        public void DoZoom(int howMany, Point? mouseLocation = null)
        {
            var zoom = _zoomIndex + howMany;
            if (zoom >= _zoomValues.Length)
                zoom = _zoomValues.Length - 1;

            if (zoom < 0)
                zoom = 0;

            SetZoom(zoom, mouseLocation);
        }

        // Auxiliary function to move the camera
        public void Move(Vector2 amount)
        {
            _pos += (amount / Zoom);
            // _pos += amount;
            SaveCamera();
        }
        // Get set position
        public Vector2 Pos
        {
            get { return _pos; }
            set
            {
                _pos = value;
                SaveCamera();
            }
        }

        public Matrix get_transformation()
        {
            _transform =       // Thanks to o KB o for this solution
              Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
                                         Matrix.CreateRotationZ(Rotation) *
                                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(_graphicsDevice.Viewport.Width * 0.5f, _graphicsDevice.Viewport.Height * 0.5f, 0));
            return _transform;
        }
    }
}

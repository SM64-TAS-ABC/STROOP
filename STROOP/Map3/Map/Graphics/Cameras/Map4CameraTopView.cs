using OpenTK;
using STROOP.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Controls.Map.Graphics
{
    class Map4CameraTopView : IMap4Camera
    {
        public Vector2 Origin = new Vector2(0, 0);
        public float RotationAngle = 0.0f;
        public float Scale = 1.0f / 0x4000;

        public Matrix4 _view;
        public Matrix4 _projection;

        public Matrix4 Matrix => GetCameraMatrix();

        public Map4CameraTopView(Map4Graphics graphics)
        {
            graphics.OnSizeChanged += (sender, e) => UpdateProjection(graphics);
            UpdateProjection(graphics);
        }

        private void UpdateProjection(Map4Graphics graphics)
        {
            _projection = Matrix4.CreateOrthographic(graphics.NormalizedWidth / Scale, graphics.NormalizedHeight / Scale, 0.0001f, 0x8000);
        }

        private Matrix4 GetCameraMatrix()
        {
            _view = Matrix4.LookAt(new Vector3(Origin.X, 0x4000, Origin.Y),
                new Vector3(Origin.X, 0, Origin.Y),
                new Vector3((float)Math.Cos(RotationAngle - Math.PI / 2), 0, (float)Math.Sin(RotationAngle - Math.PI / 2)));

            return _view * _projection;
        }
    }
}

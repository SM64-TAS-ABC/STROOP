using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Controls.Map.Graphics
{
    class MapCameraTopView : IMapCamera
    {
        public Vector2 Origin;
        public float RotationAngle;
        public float Scale;

        public Matrix4 _view;
        public Matrix4 _projection;

        public Matrix4 CameraMatrix
        {
            get
            {
                /*matrix *= Matrix4.CreateRotationY(RotationAngle);
                matrix *= Matrix4.CreateScale(Scale);
                Matrix4 matrix = Matrix4.CreateTranslation(new Vector3(Origin));
                return matrix;*/
                return Matrix4.Identity;
            }
        }

        public void SetProjectionMatrix()
        {
            // _projection = Matrix4.CreateOrthographicOffCenter()
        }
    }
}

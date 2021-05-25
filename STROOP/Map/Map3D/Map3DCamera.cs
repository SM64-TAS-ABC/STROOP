﻿using OpenTK;
using STROOP.Extensions;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Map.Map3D
{
    public class Map3DCamera
    {
        public Vector3 Position { get; set; }

        public float ZNear = 0.1f;
        public float ZFar = 0x8000;

        private float _fov = (float)Math.PI / 4.0f;
        public float FOV
        {
            get => _fov;
            set
            {
                _fov = value;
                UpdateProjection();
            }
        }

        public Matrix4 _viewRot = Matrix4.Identity;
        public Matrix4 _viewPos = Matrix4.Zero;
        public Matrix4 _projection;

        public Matrix4 Matrix => GetCameraMatrix();

        public Map3DCamera()
        {
            Config.Map3DGraphics.OnSizeChanged += (sender, e) => UpdateProjection();
            UpdateProjection();
        }

        private void UpdateProjection()
        {
            try
            {
                _projection = Matrix4.CreatePerspectiveFieldOfView(FOV, Config.Map3DGraphics.AspectRatio, ZNear, ZFar);
            }
            catch (Exception)
            {

            }
        }

        public void SetLookTarget(Vector3 target, Vector3 up)
        {
            _viewPos = Matrix4.LookAt(target, Vector3.Zero, up);
        }

        public void SetRotation(float yaw, float pitch, float roll)
        {
            _viewRot = (Matrix4.CreateRotationZ(roll)
                * Matrix4.CreateRotationX(pitch)
                * Matrix4.CreateRotationY(yaw + (float)Math.PI)).Inverted();
        }

        private Matrix4 GetCameraMatrix()
        {
            _viewPos = Matrix4.CreateTranslation(-Position);
            return _viewPos * _viewRot * _projection;
        }
    }
}

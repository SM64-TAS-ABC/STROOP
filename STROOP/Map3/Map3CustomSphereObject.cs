using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;
using System.Drawing.Imaging;

namespace STROOP.Map3
{
    public class Map3CustomSphereObject : Map3SphereObject
    {
        private readonly PositionAngle _posAngle;

        public Map3CustomSphereObject(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;

            Size = 1000;
        }

        protected override (float centerX, float centerY, float centerZ, float radius3D) Get3DDimensions()
        {
            return ((float)_posAngle.X, (float)_posAngle.Y, (float)_posAngle.Z, Size);
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.SphereImage;
        }

        public override string GetName()
        {
            return "Sphere for " + _posAngle.GetMapName();
        }

        public override float GetY()
        {
            return (float)_posAngle.Y;
        }
    }
}

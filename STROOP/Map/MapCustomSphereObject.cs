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

namespace STROOP.Map
{
    public class MapCustomSphereObject : MapSphereObject
    {
        private readonly PositionAngle _posAngle;

        public MapCustomSphereObject(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;

            Size = 1000;
        }

        protected override List<(float centerX, float centerY, float centerZ, float radius3D)> Get3DDimensions()
        {
            return new List<(float centerX, float centerY, float centerZ, float radius3D)>()
            {
                ((float)_posAngle.X, (float)_posAngle.Y, (float)_posAngle.Z, Size)
            };
        }

        public override Image GetInternalImage()
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

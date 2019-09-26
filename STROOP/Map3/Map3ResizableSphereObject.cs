using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using STROOP.Controls.Map;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;
using System.Drawing.Imaging;

namespace STROOP.Map3
{
    public class Map3ResizableSphereObject : Map3CircleObject
    {
        private readonly PositionAngle _posAngle;

        public Map3ResizableSphereObject(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;

            Size = 1000;
            Opacity = 0.5;
            Color = Color.Red;
        }

        protected override (float centerX, float centerZ, float radius) GetDimensions()
        {
            float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            float thisY = (float)_posAngle.Y;
            float yDiff = marioY - thisY;
            float radiusSquared = Size * Size - yDiff * yDiff;
            float radius = radiusSquared >= 0 ? (float)Math.Sqrt(radiusSquared) : 0;
            return ((float)_posAngle.X, (float)_posAngle.Z, radius);
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.TriangleWallImage;
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

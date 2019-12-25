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
    public class Map3DrawDistanceSphereObject : Map3SphereObject
    {
        private readonly PositionAngle _posAngle;

        public Map3DrawDistanceSphereObject(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;

            Opacity = 0.5;
            Color = Color.Red;
        }

        protected override (float centerX, float centerZ, float radius) GetDimensions()
        {
            uint objAddress = _posAngle.GetObjAddress();
            float tangibleDist = Config.Stream.GetSingle(objAddress + ObjectConfig.DrawDistOffset);
            float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            float thisY = (float)_posAngle.Y;
            float yDiff = marioY - thisY;
            float radiusSquared = tangibleDist * tangibleDist - yDiff * yDiff;
            float radius = radiusSquared >= 0 ? (float)Math.Sqrt(radiusSquared) : 0;
            return ((float)_posAngle.X, (float)_posAngle.Z, radius);
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.SphereImage;
        }

        public override string GetName()
        {
            return "Draw Distance Sphere for " + _posAngle.GetMapName();
        }

        public override float GetY()
        {
            return (float)_posAngle.Y;
        }
    }
}

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
    public class Map3TangibilitySphereObject : Map3CircleObject
    {
        private readonly PositionAngle _posAngle;

        public Map3TangibilitySphereObject(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;

            Color = Color.Red;
            Opacity = 0.5;
        }

        protected override (float centerX, float centerZ, float radius) GetDimensions()
        {
            uint objAddress = _posAngle.GetObjAddress().Value;
            float tangibleDist = Config.Stream.GetSingle(objAddress + ObjectConfig.TangibleDistOffset);
            float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            float thisY = (float)_posAngle.Y;
            float yDiff = marioY - thisY;
            float radiusSquared = tangibleDist * tangibleDist - yDiff * yDiff;
            float radius = radiusSquared >= 0 ? (float)Math.Sqrt(radiusSquared) : 0;
            return ((float)_posAngle.X, (float)_posAngle.Z, radius);
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.TriangleWallImage;
        }

        public override string GetName()
        {
            return "Tangibility Sphere for " + _posAngle.GetMapName();
        }

        public override float GetY()
        {
            return (float)_posAngle.Y;
        }
    }
}

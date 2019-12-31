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
    public class Map3TangibilitySphereObject : Map3SphereObject
    {
        private readonly PositionAngle _posAngle;

        public Map3TangibilitySphereObject(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;
        }

        protected override (float centerX, float centerY, float centerZ, float radius3D) Get3DDimensions()
        {
            uint objAddress = _posAngle.GetObjAddress();
            float tangibleDist = Config.Stream.GetSingle(objAddress + ObjectConfig.TangibleDistOffset);
            return ((float)_posAngle.X, (float)_posAngle.Y, (float)_posAngle.Z, tangibleDist);
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.SphereImage;
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

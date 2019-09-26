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
    public class Map3ResizableCylinderObject : Map3CircleObject
    {
        private readonly PositionAngle _posAngle;

        public Map3ResizableCylinderObject(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;

            Color = Color.Red;
            Opacity = 0.5;
        }

        protected override (float centerX, float centerZ, float radius) GetDimensions()
        {
            return ((float)_posAngle.X, (float)_posAngle.Z, Size);
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.TriangleWallImage;
        }

        public override string GetName()
        {
            return "Cylinder for " + _posAngle.GetMapName();
        }

        public override float GetY()
        {
            return (float)_posAngle.Y;
        }
    }
}

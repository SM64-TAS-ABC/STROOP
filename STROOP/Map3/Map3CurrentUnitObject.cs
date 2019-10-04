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
    public class Map3CurrentUnitObject : Map3QuadObject
    {
        private readonly PositionAngle _posAngle;

        public Map3CurrentUnitObject(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;

            Opacity = 0.5;
            Color = Color.Purple;
        }

        protected override List<List<(float x, float z)>> GetQuadList()
        {
            float posAngleX = (float)_posAngle.X;
            float posAngleZ = (float)_posAngle.Z;
            int xMin = (short)posAngleX;
            int xMax = xMin + (posAngleX >= 0 ? 1 : -1);
            int zMin = (short)posAngleZ;
            int zMax = zMin + (posAngleZ >= 0 ? 1 : -1);

            List<(float x, float z)> quad =
                new List<(float x, float z)>()
                {
                    (xMin, zMin),
                    (xMin, zMax),
                    (xMax, zMax),
                    (xMax, zMin),
                };
            return new List<List<(float x, float z)>>() { quad };
        }

        public override string GetName()
        {
            return "Current Unit for " + _posAngle.GetMapName();
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.CurrentUnitImage;
        }
    }
}

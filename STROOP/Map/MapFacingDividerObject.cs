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
using System.Windows.Forms;

namespace STROOP.Map
{
    public class MapFacingDividerObject : MapLineObject
    {
        private readonly PositionAngle _posAngle;

        public MapFacingDividerObject(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;

            Size = 1000;
            OutlineWidth = 3;
            OutlineColor = Color.Red;
        }

        protected override List<(float x, float y, float z)> GetVertices()
        {
            (float x, float y, float z, float angle) = ((float, float, float, float))_posAngle.GetValues();

            (float x1, float z1) =
                ((float, float))MoreMath.AddVectorToPoint(Size, angle - 16384, x, z);
            (float x2, float z2) =
                ((float, float))MoreMath.AddVectorToPoint(Size, angle + 16384, x, z);

            return new List<(float x, float y, float z)>()
            {
                (x1, y, z1),
                (x2, y, z2),
            };
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.ArrowImage;
        }

        public override string GetName()
        {
            return "Facing Divider for " + _posAngle.GetMapName();
        }

        public override PositionAngle GetPositionAngle()
        {
            return _posAngle;
        }
    }
}

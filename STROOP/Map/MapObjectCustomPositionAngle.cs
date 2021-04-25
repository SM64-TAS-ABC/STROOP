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

namespace STROOP.Map
{
    public class MapObjectCustomPositionAngle : MapObjectIconPoint
    {
        private readonly PositionAngle _posAngle;

        public MapObjectCustomPositionAngle(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;

            InternalRotates = true;
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.GreenMarioMapImage;
        }

        public override PositionAngle GetPositionAngle()
        {
            return _posAngle;
        }

        public override string GetName()
        {
            return _posAngle.GetMapName();
        }
    }
}

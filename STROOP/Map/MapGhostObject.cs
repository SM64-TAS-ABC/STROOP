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
    public class MapGhostObject : MapIconPointObject
    {
        public MapGhostObject()
            : base()
        {
            InternalRotates = true;
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.GreenMarioMapImage;
        }

        public override PositionAngle GetPositionAngle()
        {
            return PositionAngle.Ghost;
        }

        public override string GetName()
        {
            return "Ghost";
        }

        public override float GetY()
        {
            return (float)PositionAngle.Ghost.Y;
        }
    }
}

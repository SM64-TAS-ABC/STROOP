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
    public class MapSelfObject : MapIconPointObject
    {
        public MapSelfObject()
            : base()
        {
            InternalRotates = true;
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.PurpleMarioMapImage;
        }

        public override PositionAngle GetPositionAngle()
        {
            return PositionAngle.Self;
        }

        public override string GetName()
        {
            return "Self";
        }
    }
}

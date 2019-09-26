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

namespace STROOP.Map3
{
    public class Map3HolpObject : Map3IconPointObject
    {
        public Map3HolpObject()
            : base()
        {
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.HolpImage;
        }

        public override PositionAngle GetPositionAngle()
        {
            return PositionAngle.Holp;
        }

        public override string GetName()
        {
            return "HOLP";
        }

        public override float GetY()
        {
            return (float)PositionAngle.Holp.Y;
        }
    }
}

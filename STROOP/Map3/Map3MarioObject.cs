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
    public class Map3MarioObject : Map3IconPointObject
    {
        public Map3MarioObject()
            : base(() => Config.ObjectAssociations.MarioMapImage)
        {
        }

        protected override (double x, double y, double z, double angle) GetPositionAngle()
        {
            return PositionAngle.Mario.GetValues();
        }
    }
}

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
    public class MapMarioSlidingArrowObject : MapArrowObject
    {
        public MapMarioSlidingArrowObject()
            : base()
        {
        }

        public override PositionAngle GetPositionAngle()
        {
            return PositionAngle.Mario;
        }

        protected override double GetYaw()
        {
            return WatchVariableSpecialUtilities.GetMarioSlidingAngle();
        }

        protected override double GetRecommendedSize()
        {
            return WatchVariableSpecialUtilities.GetMarioSlidingSpeed();
        }

        public override string GetName()
        {
            return "Mario Sliding Arrow";
        }
    }
}

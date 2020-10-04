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
        private readonly PositionAngle _posAngle;

        public MapMarioSlidingArrowObject(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;
        }

        public override PositionAngle GetPositionAngle()
        {
            return _posAngle;
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

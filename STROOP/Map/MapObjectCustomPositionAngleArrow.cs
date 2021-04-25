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
    public class MapObjectCustomPositionAngleArrow : MapObjectArrow
    {
        private readonly PositionAngle _posPA;
        private readonly PositionAngle _anglePA;

        public MapObjectCustomPositionAngleArrow(PositionAngle posPA, PositionAngle anglePA)
            : base()
        {
            _posPA = posPA;
            _anglePA = anglePA;
        }

        public override PositionAngle GetPositionAngle()
        {
            return _posPA;
        }

        protected override double GetYaw()
        {
            return _anglePA.Angle;
        }

        protected override double GetRecommendedSize()
        {
            return 100;
        }

        public override string GetName()
        {
            return "Custom Position Angle Arrow";
        }
    }
}

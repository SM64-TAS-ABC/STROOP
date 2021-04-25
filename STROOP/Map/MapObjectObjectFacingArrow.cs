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
    public class MapObjectObjectFacingArrow : MapObjectArrow
    {
        private readonly PositionAngle _posAngle;
        private readonly uint _objAddress;

        public MapObjectObjectFacingArrow(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;
            _objAddress = posAngle.GetObjAddress();
        }

        public override PositionAngle GetPositionAngle()
        {
            return _posAngle;
        }

        protected override double GetYaw()
        {
            return Config.Stream.GetUShort(_objAddress + ObjectConfig.YawFacingOffset);
        }

        protected override double GetRecommendedSize()
        {
            return Config.Stream.GetFloat(_objAddress + ObjectConfig.HSpeedOffset);
        }

        public override string GetName()
        {
            return "Object Facing Arrow for " + _posAngle.GetMapName();
        }
    }
}

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
using System.Xml.Linq;

namespace STROOP.Map
{
    public class MapObjectMarioMovingArrow : MapObjectArrow
    {
        private readonly PositionAngle _posAngle;

        public MapObjectMarioMovingArrow(PositionAngle posAngle)
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
            return Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.MovingYawOffset);
        }

        protected override double GetPitch()
        {
            return Config.Stream.GetShort(MarioConfig.StructAddress + MarioConfig.FacingPitchOffset);
        }

        protected override double GetRecommendedSize()
        {
            return Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
        }

        protected override void SetRecommendedSize(double size)
        {
            Config.Stream.SetValue((float)size, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
        }

        protected override void SetYaw(double yaw)
        {
            Config.Stream.SetValue(MoreMath.NormalizeAngleUshort(yaw), MarioConfig.StructAddress + MarioConfig.MovingYawOffset);
        }

        public override string GetName()
        {
            return "Mario Moving Arrow";
        }

        public override List<XAttribute> GetXAttributes()
        {
            return new List<XAttribute>()
            {
                new XAttribute("positionAngle", _posAngle),
            };
        }
    }
}

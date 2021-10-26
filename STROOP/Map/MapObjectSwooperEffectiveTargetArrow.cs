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
    public class MapObjectSwooperEffectiveTargetArrow : MapObjectArrow
    {
        private readonly PositionAngle _posAngle;

        public MapObjectSwooperEffectiveTargetArrow(PositionAngle posAngle)
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
            uint globalTimer = Config.Stream.GetUInt(MiscConfig.GlobalTimerAddress);
            int targetAngle = Config.Stream.GetInt(_posAngle.GetObjAddress() + ObjectConfig.SwooperTargetYawOffset);
            return targetAngle + (short)(3000 * InGameTrigUtilities.InGameCosine(4000 * (int)globalTimer));
        }

        protected override double GetPitch()
        {
            return -1 * Config.Stream.GetShort(_posAngle.GetObjAddress() + ObjectConfig.PitchFacingOffset);
        }

        protected override double GetRecommendedSize()
        {
            return Config.Stream.GetFloat(_posAngle.GetObjAddress() + ObjectConfig.HSpeedOffset);
        }

        public override string GetName()
        {
            return "Swooper Target Arrow for " + _posAngle.GetMapName();
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

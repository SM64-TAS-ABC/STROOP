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
    public class MapMarioIntendedArrowObject : MapArrowObject
    {
        public MapMarioIntendedArrowObject()
            : base()
        {
        }

        public override PositionAngle GetPositionAngle()
        {
            return PositionAngle.Mario;
        }

        protected override double GetYaw()
        {
            return Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.IntendedYawOffset);
        }

        protected override double GetRecommendedSize()
        {
            return Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
        }

        public override string GetName()
        {
            return "Mario Intended Arrow";
        }
    }
}

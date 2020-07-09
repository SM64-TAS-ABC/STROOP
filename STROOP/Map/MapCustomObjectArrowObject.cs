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
    public class MapCustomObjectArrowObject : MapArrowObject
    {
        private readonly PositionAngle _posAngle;
        private readonly uint _objAddress;
        private readonly uint _yawOffset;
        private readonly int _numBytes;

        public MapCustomObjectArrowObject(PositionAngle posAngle, uint yawOffset, int numBytes)
            : base()
        {
            _posAngle = posAngle;
            _objAddress = posAngle.GetObjAddress();
            _yawOffset = yawOffset;
            _numBytes = numBytes;
        }

        public override PositionAngle GetPositionAngle()
        {
            return _posAngle;
        }

        protected override double GetYaw()
        {
            return _numBytes == 2 ?
                Config.Stream.GetUInt16(_objAddress + _yawOffset) :
                Config.Stream.GetUInt32(_objAddress + _yawOffset);
        }

        protected override double GetRecommendedSize()
        {
            return Config.Stream.GetSingle(_objAddress + ObjectConfig.HSpeedOffset);
        }

        public override string GetName()
        {
            return "Custom Object Arrow";
        }
    }
}

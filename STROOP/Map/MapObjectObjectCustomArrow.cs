﻿using System;
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
    public class MapObjectObjectCustomArrow : MapObjectArrow
    {
        private readonly PositionAngle _posAngle;
        private readonly uint _yawOffset;
        private readonly int _numBytes;

        public MapObjectObjectCustomArrow(PositionAngle posAngle, uint yawOffset, int numBytes)
            : base()
        {
            _posAngle = posAngle;
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
                Config.Stream.GetUShort(_posAngle.GetObjAddress() + _yawOffset) :
                Config.Stream.GetUInt(_posAngle.GetObjAddress() + _yawOffset);
        }

        protected override double GetRecommendedSize()
        {
            return Config.Stream.GetFloat(_posAngle.GetObjAddress() + ObjectConfig.HSpeedOffset);
        }

        public override string GetName()
        {
            return "Object Custom Arrow for " + _posAngle.GetMapName();
        }

        public override List<XAttribute> GetXAttributes()
        {
            return new List<XAttribute>()
            {
                new XAttribute("positionAngle", _posAngle),
                new XAttribute("yawOffset", HexUtilities.FormatValue(_yawOffset)),
                new XAttribute("numBytes", _numBytes),
            };
        }
    }
}

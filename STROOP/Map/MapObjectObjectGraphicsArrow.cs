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
    public class MapObjectObjectGraphicsArrow : MapObjectArrow
    {
        private readonly PositionAngle _posAngle;
        private readonly uint _objAddress;

        public MapObjectObjectGraphicsArrow(PositionAngle posAngle)
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
            return Config.Stream.GetUShort(_objAddress + ObjectConfig.GraphicsYawOffset);
        }

        protected override double GetRecommendedSize()
        {
            return Config.Stream.GetFloat(_objAddress + ObjectConfig.HSpeedOffset);
        }

        public override string GetName()
        {
            return "Object Graphics Arrow for " + _posAngle.GetMapName();
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

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

        protected override double GetPitch()
        {
            return 0;
        }

        protected override double GetRecommendedSize()
        {
            return Size;
        }

        protected override void SetRecommendedSize(double size)
        {
            GetParentMapTracker().SetSize((float)(Scales ? size : size * Config.CurrentMapGraphics.MapViewScaleValue));
        }

        protected override void SetYaw(double yaw)
        {
            _anglePA.SetAngle(yaw);
        }

        public override string GetName()
        {
            return _anglePA.GetMapName() + " for " + _posPA.GetMapName();
        }

        public override List<XAttribute> GetXAttributes()
        {
            return new List<XAttribute>()
            {
                new XAttribute("positionAngle1", _posPA),
                new XAttribute("positionAngle2", _anglePA),
            };
        }
    }
}

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
using System.Xml.Linq;

namespace STROOP.Map
{
    public class MapObjectPyramidNormalTarget : MapObjectIconPoint
    {
        private readonly PositionAngle _posAngle;

        public MapObjectPyramidNormalTarget(PositionAngle posAngle)
            : base()
        {
            _posAngle = PositionAngle.PyramidNormalTarget(posAngle.GetObjAddress());
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.BlueMarioMapImage;
        }

        public override PositionAngle GetPositionAngle()
        {
            return _posAngle;
        }

        public override string GetName()
        {
            return _posAngle.GetMapName();
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

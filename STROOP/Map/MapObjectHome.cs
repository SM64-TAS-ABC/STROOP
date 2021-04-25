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
using STROOP.Models;
using System.Xml.Linq;

namespace STROOP.Map
{
    public class MapObjectHome : MapObjectIconPoint
    {
        private readonly PositionAngle _posAngle;

        public MapObjectHome(uint objAddress)
            : base()
        {
            _posAngle = PositionAngle.ObjHome(objAddress);
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.HomeImage;
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
                new XAttribute("objectAddress", HexUtilities.FormatValue(_posAngle.GetObjAddress())),
            };
        }
    }
}

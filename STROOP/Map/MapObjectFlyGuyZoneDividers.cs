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
    public class MapObjectFlyGuyZoneDividers : MapObjectLine
    {
        private readonly PositionAngle _posAngle;

        public MapObjectFlyGuyZoneDividers(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;

            LineWidth = 3;
            LineColor = Color.Blue;
        }

        protected override List<(float x, float y, float z)> GetVerticesTopDownView()
        {
            return new List<(float x, float y, float z)>();
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.FacingDividerImage;
        }

        public override string GetName()
        {
            return "Fly Guy Zone Divider for " + _posAngle.GetMapName();
        }

        public override PositionAngle GetPositionAngle()
        {
            return _posAngle;
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

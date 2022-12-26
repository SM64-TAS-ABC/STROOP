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
using System.Xml.Linq;

namespace STROOP.Map
{
    public class MapObjectHomeLine : MapObjectLine
    {
        private readonly PositionAngle _objPosAngle;
        private readonly PositionAngle _homePosAngle;

        public MapObjectHomeLine(PositionAngle posAngle)
            : base()
        {
            _objPosAngle = PositionAngle.Obj(posAngle.GetObjAddress());
            _homePosAngle = PositionAngle.ObjHome(posAngle.GetObjAddress());

            LineWidth = 3;
            LineColor = Color.Black;
        }

        protected override List<(float x, float y, float z)> GetVerticesTopDownView()
        {
            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
            vertices.Add(((float)_homePosAngle.X, (float)_homePosAngle.Y, (float)_homePosAngle.Z));
            vertices.Add(((float)_objPosAngle.X, (float)_objPosAngle.Y, (float)_objPosAngle.Z));
            return vertices;
        }

        public override string GetName()
        {
            return "Home Line for " + _objPosAngle.GetMapName();
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.HomeLineImage;
        }

        public override PositionAngle GetPositionAngle()
        {
            return _objPosAngle;
        }

        public override List<XAttribute> GetXAttributes()
        {
            return new List<XAttribute>()
            {
                new XAttribute("positionAngle", _objPosAngle),
            };
        }
    }
}

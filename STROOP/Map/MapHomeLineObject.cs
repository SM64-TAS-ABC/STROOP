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

namespace STROOP.Map
{
    public class MapHomeLineObject : MapLineObject
    {
        private readonly PositionAngle _objPosAngle;
        private readonly PositionAngle _homePosAngle;

        public MapHomeLineObject(uint objAddress)
            : base()
        {
            _objPosAngle = PositionAngle.Obj(objAddress);
            _homePosAngle = PositionAngle.ObjHome(objAddress);

            OutlineWidth = 3;
            OutlineColor = Color.Black;
        }

        protected override List<(float x, float y, float z)> GetVertices()
        {
            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
            vertices.Add(((float)_homePosAngle.X, (float)_homePosAngle.Y, (float)_homePosAngle.Z));
            vertices.Add(((float)_objPosAngle.X, (float)_objPosAngle.Y, (float)_objPosAngle.Z));
            return vertices;
        }

        public override string GetName()
        {
            return "Home Line";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.ArrowImage;
        }

        public override PositionAngle GetPositionAngle()
        {
            return _objPosAngle;
        }
    }
}

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
using System.Drawing.Imaging;
using System.Xml.Linq;

namespace STROOP.Map
{
    public class MapObjectTangibilitySphere : MapObjectSphere
    {
        private readonly PositionAngle _posAngle;

        public MapObjectTangibilitySphere(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;
        }

        protected override List<(float centerX, float centerY, float centerZ, float radius3D)> Get3DDimensions()
        {
            uint objAddress = _posAngle.GetObjAddress();
            float tangibleDist = Config.Stream.GetFloat(objAddress + ObjectConfig.TangibleDistOffset);
            return new List<(float centerX, float centerY, float centerZ, float radius3D)>()
            {
                ((float)_posAngle.X, (float)_posAngle.Y, (float)_posAngle.Z, tangibleDist)
            };
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.SphereImage;
        }

        public override string GetName()
        {
            return "Tangibility Sphere for " + _posAngle.GetMapName();
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

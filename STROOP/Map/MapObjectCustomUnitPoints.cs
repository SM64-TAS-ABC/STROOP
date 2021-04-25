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
    public class MapObjectCustomUnitPoints : MapObjectQuad
    {
        private readonly List<(int x, int z)> _unitPoints;

        public MapObjectCustomUnitPoints(List<(int x, int z)> unitPoints)
            : base()
        {
            _unitPoints = unitPoints;

            Opacity = 0.5;
            Color = Color.Orange;
        }

        public static MapObjectCustomUnitPoints Create(string text, bool useTriplets)
        {
            List<(double x, double y, double z)> points = MapUtilities.ParsePoints(text, useTriplets);
            if (points == null) return null;
            List<(int x, int z)> unitPoints = points.ConvertAll(
                point => ((int)point.x, (int)point.z));
            return new MapObjectCustomUnitPoints(unitPoints);
        }

        protected override List<List<(float x, float y, float z)>> GetQuadList()
        {
            return MapUtilities.ConvertUnitPointsToQuads(_unitPoints);
        }

        public override string GetName()
        {
            return "Custom Unit Points";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.CustomPointsImage;
        }

        public override List<XAttribute> GetXAttributes()
        {
            List<string> pointList = _unitPoints.ConvertAll(
                p => string.Format("({0},{1})", (double)p.x, (double)p.z));
            return new List<XAttribute>()
            {
                new XAttribute("points", string.Join(",", pointList)),
            };
        }
    }
}

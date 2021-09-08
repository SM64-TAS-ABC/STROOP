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
using System.Windows.Forms;

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

        protected override List<List<(float x, float y, float z, bool isHovered)>> GetQuadList(MapObjectHoverData hoverData)
        {
            List<List<(float x, float y, float z, bool isHovered)>> output =
                new List<List<(float x, float y, float z, bool isHovered)>>();
            for (int i = 0; i < _unitPoints.Count; i++)
            {
                bool isHovered = this == hoverData?.MapObject && i == hoverData?.Index;
                List<(int x, int z)> unit = new List<(int x, int z)>() { _unitPoints[i] };
                List<List<(float x, float y, float z)>> quadList =
                    MapUtilities.ConvertUnitPointsToQuads(unit);
                List<List<(float x, float y, float z, bool isHovered)>> quadListHovered =
                    quadList.ConvertAll(quad => quad.ConvertAll(p => (p.x, p.y, p.z, isHovered)));
                output.AddRange(quadListHovered);
            }
            return output;
        }

        public override string GetName()
        {
            return "Custom Unit Points";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.CustomPointsImage;
        }

        public override MapObjectHoverData GetHoverDataTopDownView()
        {
            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe();
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;
            (float inGameX, float inGameZ) = MapUtilities.ConvertCoordsForInGame(relPos.X, relPos.Y);

            int inGameXTruncated = (int)inGameX;
            int inGameZTruncated = (int)inGameZ;
            for (int i = 0; i < _unitPoints.Count; i++)
            {
                var unitPoint = _unitPoints[i];
                if (unitPoint.x == inGameXTruncated && unitPoint.z == inGameZTruncated)
                {
                    return new MapObjectHoverData(this, unitPoint.x, 0, unitPoint.z, index: i);
                }
            }
            return null;
        }

        public override MapObjectHoverData GetHoverDataOrthographicView()
        {
            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe();
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;
            for (int i = 0; i < _unitPoints.Count; i++)
            {
                var unitPoint = _unitPoints[i];
                List<(int x, int z)> unitPointList = new List<(int x, int z)>() { _unitPoints[i] };
                List<List<(float x, float y, float z)>> quadList =
                    MapUtilities.ConvertUnitPointsToQuads(unitPointList);
                List<List<(float x, float z)>> quadListForControl =
                    quadList.ConvertAll(quad => quad.ConvertAll(p => MapUtilities.ConvertCoordsForControlOrthographicView(p.x, p.y, p.z)));
                if (quadListForControl.Any(quad => MapUtilities.IsWithinShapeForControl(quad, relPos.X, relPos.Y)))
                {
                    return new MapObjectHoverData(this, unitPoint.x, 0, unitPoint.z, index: i);
                }
            }
            return null;
        }

        public override List<ToolStripItem> GetHoverContextMenuStripItems(MapObjectHoverData hoverData)
        {
            List<ToolStripItem> output = base.GetHoverContextMenuStripItems(hoverData);

            var unitPoint = _unitPoints[hoverData.Index.Value];
            List<double> posValues = new List<double>() { unitPoint.x, unitPoint.z };
            ToolStripMenuItem copyPositionItem = MapUtilities.CreateCopyItem(posValues, "Position");
            output.Insert(0, copyPositionItem);

            return output;
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

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

        public override MapObjectHoverData GetHoverData()
        {
            Point relPos = Config.MapGui.CurrentControl.PointToClient(MapObjectHoverData.GetCurrentPoint());
            (float inGameX, float inGameZ) = MapUtilities.ConvertCoordsForInGame(relPos.X, relPos.Y);

            int inGameXTruncated = (int)inGameX;
            int inGameZTruncated = (int)inGameZ;
            int? hoverIndex = null;
            for (int i = 0; i < _unitPoints.Count; i++)
            {
                var unitPoint = _unitPoints[i];
                if (unitPoint.x == inGameXTruncated && unitPoint.z == inGameZTruncated)
                {
                    hoverIndex = i;
                    break;
                }
            }
            return hoverIndex.HasValue ? new MapObjectHoverData(this, index: hoverIndex) : null;
        }

        public override List<ToolStripItem> GetHoverContextMenuStripItems(MapObjectHoverData hoverData)
        {
            List<ToolStripItem> output = base.GetHoverContextMenuStripItems(hoverData);

            var unitPoint = _unitPoints[hoverData.Index.Value];
            List<object> posObjs = new List<object>() { unitPoint.x, unitPoint.z };
            ToolStripMenuItem copyPositionItem = MapUtilities.CreateCopyItem(posObjs, "Position");
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

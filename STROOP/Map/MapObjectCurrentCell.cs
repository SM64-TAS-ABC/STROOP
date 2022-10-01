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
    public class MapObjectCurrentCell : MapObjectQuad
    {
        private readonly PositionAngle _posAngle;

        public MapObjectCurrentCell(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;

            Opacity = 0.5;
            Color = Color.Yellow;
        }

        protected override List<List<(float x, float y, float z, Color color, bool isHovered)>> GetQuadList(MapObjectHoverData hoverData)
        {
            (float posAngleX, float posAngleY, float posAngleZ, float posAngleAngle) =
                ((float, float, float, float))_posAngle.GetValues();

            (int cellX, int cellZ) = CellUtilities.GetCell(posAngleX, posAngleZ);
            int xMin = (cellX - 8) * 1024;
            int xMax = xMin + 1024;
            int zMin = (cellZ - 8) * 1024;
            int zMax = zMin + 1024;

            bool isHovered = this == hoverData?.MapObject;

            List<(float x, float y, float z, Color color, bool isHovered)> quad =
                new List<(float x, float y, float z, Color color, bool isHovered)>()
                {
                    (xMin, posAngleY, zMin, Color, isHovered),
                    (xMin, posAngleY, zMax, Color, isHovered),
                    (xMax, posAngleY, zMax, Color, isHovered),
                    (xMax, posAngleY, zMin, Color, isHovered),
                };
            return new List<List<(float x, float y, float z, Color color, bool isHovered)>>() { quad };
        }

        public override string GetName()
        {
            return "Current Cell for " + _posAngle.GetMapName();
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.CurrentCellImage;
        }

        public override PositionAngle GetPositionAngle()
        {
            return _posAngle;
        }

        public override MapObjectHoverData GetHoverDataTopDownView(bool isForObjectDrag, bool forceCursorPosition)
        {
            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe(isForObjectDrag, forceCursorPosition);
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;
            (float inGameX, float inGameZ) = MapUtilities.ConvertCoordsForInGameTopDownView(relPos.X, relPos.Y);
            var quadList = GetQuadList(null);
            for (int i = quadList.Count - 1; i >= 0; i--)
            {
                var quad = quadList[i];
                var simpleQuad = quad.ConvertAll(q => (q.x, q.y, q.z));
                if (MapUtilities.IsWithinRectangularQuad(simpleQuad, inGameX, inGameZ) || forceCursorPosition)
                {
                    double xMin = quad.Min(p => p.x);
                    double xMax = quad.Max(p => p.x);
                    double zMin = quad.Min(p => p.z);
                    double zMax = quad.Max(p => p.z);
                    string info = string.Format("{0}<x<{1} {2}<z<{3}", xMin, xMax, zMin, zMax);
                    return new MapObjectHoverData(this, 0, 0, 0, index: i, info: info);
                }
            }
            return null;
        }

        public override MapObjectHoverData GetHoverDataOrthographicView(bool isForObjectDrag, bool forceCursorPosition)
        {
            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe(isForObjectDrag, forceCursorPosition);
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;

            var quadList = GetQuadList(null);
            for (int i = quadList.Count - 1; i >= 0; i--)
            {
                var quad = quadList[i];
                var quadForControl = quad.ConvertAll(p => MapUtilities.ConvertCoordsForControlOrthographicView(p.x, p.y, p.z, UseRelativeCoordinates));
                if (MapUtilities.IsWithinShapeForControl(quadForControl, relPos.X, relPos.Y, forceCursorPosition))
                {
                    double xMin = quad.Min(p => p.x);
                    double xMax = quad.Max(p => p.x);
                    double zMin = quad.Min(p => p.z);
                    double zMax = quad.Max(p => p.z);
                    string info = string.Format("{0}<x<{1} {2}<z<{3}", xMin, xMax, zMin, zMax);
                    return new MapObjectHoverData(this, 0, 0, 0, index: i, info: info);
                }
            }
            return null;
        }

        public override List<ToolStripItem> GetHoverContextMenuStripItems(MapObjectHoverData hoverData)
        {
            List<ToolStripItem> output = base.GetHoverContextMenuStripItems(hoverData);

            var quadList = GetQuadList(null);
            var quad = quadList[hoverData.Index.Value];

            double xMin = quad.Min(p => p.x);
            double xMax = quad.Max(p => p.x);
            double zMin = quad.Min(p => p.z);
            double zMax = quad.Max(p => p.z);

            ToolStripMenuItem copyXMin = new ToolStripMenuItem(string.Format("Copy X Min ({0})", xMin));
            ToolStripMenuItem copyXMax = new ToolStripMenuItem(string.Format("Copy X Max ({0})", xMax));
            ToolStripMenuItem copyZMin = new ToolStripMenuItem(string.Format("Copy Z Min ({0})", zMin));
            ToolStripMenuItem copyZMax = new ToolStripMenuItem(string.Format("Copy Z Max ({0})", zMax));

            copyXMin.Click += (sender, e) => Clipboard.SetText(xMin.ToString());
            copyXMax.Click += (sender, e) => Clipboard.SetText(xMax.ToString());
            copyZMin.Click += (sender, e) => Clipboard.SetText(zMin.ToString());
            copyZMax.Click += (sender, e) => Clipboard.SetText(zMax.ToString());

            output.Insert(0, copyXMin);
            output.Insert(1, copyXMax);
            output.Insert(2, copyZMin);
            output.Insert(3, copyZMax);

            return output;
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

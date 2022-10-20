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
    public class MapObjectCurrentUnit : MapObjectQuad
    {
        private readonly PositionAngle _posAngle;

        public MapObjectCurrentUnit(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;

            Opacity = 0.5;
            Color = Color.Purple;
        }

        protected override List<List<(float x, float y, float z, Color color, bool isHovered)>> GetQuadList(MapObjectHoverData hoverData)
        {
            (float posAngleX, float posAngleY, float posAngleZ, float posAngleAngle) =
                ((float, float, float, float))_posAngle.GetValues();

            int xMin = (short)posAngleX;
            int xMax = xMin + (posAngleX >= 0 ? 1 : -1);
            int zMin = (short)posAngleZ;
            int zMax = zMin + (posAngleZ >= 0 ? 1 : -1);

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
            return "Current Unit for " + _posAngle.GetMapName();
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.CurrentUnitImage;
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
            int inGameXTruncated = (int)inGameX;
            int inGameZTruncated = (int)inGameZ;
            (double x, double y, double z, double angle) = _posAngle.GetValues();
            int xTruncated = (int)x;
            int yTruncated = (int)y;
            int zTruncated = (int)z;
            if (xTruncated == inGameXTruncated && zTruncated == inGameZTruncated || forceCursorPosition)
            {
                return new MapObjectHoverData(this, MapObjectHoverDataEnum.Rectangle, xTruncated, yTruncated, zTruncated);
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
                    return new MapObjectHoverData(this, MapObjectHoverDataEnum.Rectangle, 0, 0, 0, index: i);
                }
            }
            return null;
        }

        public override List<ToolStripItem> GetHoverContextMenuStripItems(MapObjectHoverData hoverData)
        {
            List<ToolStripItem> output = base.GetHoverContextMenuStripItems(hoverData);

            ToolStripMenuItem copyPositionItem = MapUtilities.CreateCopyItem((int)_posAngle.X, (int)_posAngle.Y, (int)_posAngle.Z, "Position");
            output.Insert(0, copyPositionItem);

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

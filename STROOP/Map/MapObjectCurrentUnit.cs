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

        protected override List<List<(float x, float y, float z, bool isHovered)>> GetQuadList(MapObjectHoverData hoverData)
        {
            (float posAngleX, float posAngleY, float posAngleZ, float posAngleAngle) =
                ((float, float, float, float))_posAngle.GetValues();

            int xMin = (short)posAngleX;
            int xMax = xMin + (posAngleX >= 0 ? 1 : -1);
            int zMin = (short)posAngleZ;
            int zMax = zMin + (posAngleZ >= 0 ? 1 : -1);

            bool isHovered = this == hoverData?.MapObject;
            List<(float x, float y, float z, bool isHovered)> quad =
                new List<(float x, float y, float z, bool isHovered)>()
                {
                    (xMin, posAngleY, zMin, isHovered),
                    (xMin, posAngleY, zMax, isHovered),
                    (xMax, posAngleY, zMax, isHovered),
                    (xMax, posAngleY, zMin, isHovered),
                };
            return new List<List<(float x, float y, float z, bool isHovered)>>() { quad };
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

        public override MapObjectHoverData GetHoverDataTopDownView()
        {
            Point relPos = Config.MapGui.CurrentControl.PointToClient(MapObjectHoverData.GetCurrentPoint());
            (float inGameX, float inGameZ) = MapUtilities.ConvertCoordsForInGame(relPos.X, relPos.Y);
            int inGameXTruncated = (int)inGameX;
            int inGameZTruncated = (int)inGameZ;
            (double x, double y, double z, double angle) = _posAngle.GetValues();
            int xTruncated = (int)x;
            int yTruncated = (int)y;
            int zTruncated = (int)z;
            if (xTruncated == inGameXTruncated && zTruncated == inGameZTruncated)
            {
                return new MapObjectHoverData(this, xTruncated, yTruncated, zTruncated);
            }
            return null;
        }

        public override MapObjectHoverData GetHoverDataOrthographicView()
        {
            Point relPos = Config.MapGui.CurrentControl.PointToClient(MapObjectHoverData.GetCurrentPoint());
            var quadList = GetQuadList(null);
            for (int i = quadList.Count - 1; i >= 0; i--)
            {
                var quad = quadList[i];
                var quadForControl = quad.ConvertAll(p => MapUtilities.ConvertCoordsForControlOrthographicView(p.x, p.y, p.z));
                if (MapUtilities.IsWithinParallelogramQuadControl(quadForControl, relPos.X, relPos.Y))
                {
                    return new MapObjectHoverData(this, 0, 0, 0, index: i);
                }
            }
            return null;
        }

        public override List<ToolStripItem> GetHoverContextMenuStripItems(MapObjectHoverData hoverData)
        {
            List<ToolStripItem> output = base.GetHoverContextMenuStripItems(hoverData);

            List<double> posValues = new List<double>() { (int)_posAngle.X, (int)_posAngle.Z };
            ToolStripMenuItem copyPositionItem = MapUtilities.CreateCopyItem(posValues, "Position");
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

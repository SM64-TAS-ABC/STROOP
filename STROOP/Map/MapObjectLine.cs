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
using STROOP.Map.Map3D;
using System.Windows.Forms;

namespace STROOP.Map
{
    public abstract class MapObjectLine : MapObject
    {
        public MapObjectLine()
            : base()
        {
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            MapUtilities.DrawLinesOn2DControlTopDownView(GetVerticesTopDownView(), LineWidth, LineColor, OpacityByte);

            if (_customImage != null)
            {
                List<(float x, float z)> positions = GetCustomImagePositions();
                for (int i = 0; i < positions.Count; i++)
                {
                    (float x, float z) = positions[i];
                    (float controlX, float controlZ) = MapUtilities.ConvertCoordsForControlTopDownView(x, z);
                    SizeF size = MapUtilities.ScaleImageSizeForControl(_customImage.Size, 8, Scales);
                    MapUtilities.DrawTexture(_customImageTex.Value, new PointF(controlX, controlZ), size, 0, 1);
                }
            }
        }

        public override void DrawOn2DControlOrthographicView()
        {
            MapUtilities.DrawLinesOn2DControlOrthographicView(GetVerticesOrthographicView(), LineWidth, LineColor, OpacityByte);
        }

        public override void DrawOn3DControl()
        {
            MapUtilities.DrawLinesOn3DControl(GetVertices3D(), LineWidth, LineColor, OpacityByte, GetModelMatrix());
        }

        protected abstract List<(float x, float y, float z)> GetVerticesTopDownView();

        protected virtual List<(float x, float y, float z)> GetVerticesOrthographicView()
        {
            return GetVerticesTopDownView();
        }

        protected virtual List<(float x, float y, float z)> GetVertices3D()
        {
            return GetVerticesTopDownView();
        }

        protected virtual List<(float x, float z)> GetCustomImagePositions()
        {
            return new List<(float x, float z)>();
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Perspective;
        }

        public override MapObjectHoverData GetHoverData()
        {
            Point relPos = Config.MapGui.CurrentControl.PointToClient(MapObjectHoverData.GetCurrentPoint());
            (float inGameX, float inGameZ) = MapUtilities.ConvertCoordsForInGame(relPos.X, relPos.Y);

            var positions = GetCustomImagePositions();
            for (int i = 0; i < positions.Count; i++)
            {
                var position = positions[i];
                double dist = MoreMath.GetDistanceBetween(position.x, position.z, inGameX, inGameZ);
                double radius = Scales ? Size : Size / Config.CurrentMapGraphics.MapViewScaleValue;
                if (dist <= radius)
                {
                    return new MapObjectHoverData(this, index: i);
                }
            }
            return null;
        }

        public override List<ToolStripItem> GetHoverContextMenuStripItems(MapObjectHoverData hoverData)
        {
            List<ToolStripItem> output = base.GetHoverContextMenuStripItems(hoverData);

            var positions = GetCustomImagePositions();
            var position = positions[hoverData.Index.Value];
            List<double> posValues = new List<double>() { position.x, position.z };
            ToolStripMenuItem copyPositionItem = MapUtilities.CreateCopyItem(posValues, "Position");
            output.Insert(0, copyPositionItem);

            return output;
        }
    }
}

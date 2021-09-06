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
using System.Windows.Forms;

namespace STROOP.Map
{
    public class MapObjectWaters : MapObjectQuad
    {
        public MapObjectWaters()
            : base()
        {
            Opacity = 0.5;
            Color = Color.Purple;
        }

        protected override List<List<(float x, float y, float z, bool isHovered)>> GetQuadList(MapObjectHoverData hoverData)
        {
            List<(int y, int xMin, int xMax, int zMin, int zMax)> waters = WaterUtilities.GetWaterLevels();
            List<List<(float x, float y, float z, bool isHovered)>> quads =
                new List<List<(float x, float y, float z, bool isHovered)>>();
            for (int i = 0; i < waters.Count; i++)
            {
                bool isHovered = this == hoverData?.MapObject && i == hoverData?.Index;
                var water = waters[i];
                List<(float x, float y, float z, bool isHovered)> quad =
                    new List<(float x, float y, float z, bool isHovered)>();
                quad.Add((water.xMin, water.y, water.zMin, isHovered));
                quad.Add((water.xMin, water.y, water.zMax, isHovered));
                quad.Add((water.xMax, water.y, water.zMax, isHovered));
                quad.Add((water.xMax, water.y, water.zMin, isHovered));
                quads.Add(quad);
            }
            return quads;
        }

        public override string GetName()
        {
            return "Waters";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.WatersImage;
        }

        public override MapObjectHoverData GetHoverDataTopDownView()
        {
            Point relPos = Config.MapGui.CurrentControl.PointToClient(MapObjectHoverData.GetCurrentPoint());
            (float inGameX, float inGameZ) = MapUtilities.ConvertCoordsForInGame(relPos.X, relPos.Y);
            var quadList = GetQuadList(null);
            for (int i = quadList.Count - 1; i >= 0; i--)
            {
                var quad = quadList[i];
                var simpleQuad = quad.ConvertAll(q => (q.x, q.y, q.z));
                if (MapUtilities.IsWithinRectangularQuad(simpleQuad, inGameX, inGameZ))
                {
                    return new MapObjectHoverData(this, 0, 0, 0, index: i);
                }
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
                if (MapUtilities.IsWithinShapeForControl(quadForControl, relPos.X, relPos.Y))
                {
                    return new MapObjectHoverData(this, 0, 0, 0, index: i);
                }
            }
            return null;
        }

        public override List<ToolStripItem> GetHoverContextMenuStripItems(MapObjectHoverData hoverData)
        {
            List<ToolStripItem> output = base.GetHoverContextMenuStripItems(hoverData);

            var quadList = GetQuadList(null);
            var quad = quadList[hoverData.Index.Value];
            if (quad.Count == 0) return output;

            double xMin = quad.Min(p => p.x);
            double xMax = quad.Max(p => p.x);
            double zMin = quad.Min(p => p.z);
            double zMax = quad.Max(p => p.z);
            double y = quad.Max(p => p.y);

            ToolStripMenuItem copyXMin = new ToolStripMenuItem(string.Format("Copy X Min ({0})", xMin));
            ToolStripMenuItem copyXMax = new ToolStripMenuItem(string.Format("Copy X Max ({0})", xMax));
            ToolStripMenuItem copyZMin = new ToolStripMenuItem(string.Format("Copy Z Min ({0})", zMin));
            ToolStripMenuItem copyZMax = new ToolStripMenuItem(string.Format("Copy Z Max ({0})", zMax));
            ToolStripMenuItem copyY = new ToolStripMenuItem(string.Format("Copy Y ({0})", y));

            copyXMin.Click += (sender, e) => Clipboard.SetText(xMin.ToString());
            copyXMax.Click += (sender, e) => Clipboard.SetText(xMax.ToString());
            copyZMin.Click += (sender, e) => Clipboard.SetText(zMin.ToString());
            copyZMax.Click += (sender, e) => Clipboard.SetText(zMax.ToString());
            copyY.Click += (sender, e) => Clipboard.SetText(y.ToString());

            output.Insert(0, copyXMin);
            output.Insert(1, copyXMax);
            output.Insert(2, copyZMin);
            output.Insert(3, copyZMax);
            output.Insert(4, copyY);

            return output;
        }
    }
}

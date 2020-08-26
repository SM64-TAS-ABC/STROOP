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
using STROOP.Models;
using System.Windows.Forms;

namespace STROOP.Map
{
    public abstract class MapTriangleObject : MapObject
    {
        private float? _withinDist;
        private float? _withinCenter;
        protected bool _excludeDeathBarriers;

        public MapTriangleObject()
            : base()
        {
            _withinDist = null;
            _withinCenter = null;
            _excludeDeathBarriers = false;
        }

        protected List<List<(float x, float y, float z)>> GetVertexLists()
        {
            return GetTrianglesWithinDist().ConvertAll(tri => tri.Get3DVertices());
        }

        protected List<TriangleDataModel> GetTrianglesWithinDist()
        {
            float centerY = _withinCenter ?? Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            List<TriangleDataModel> tris = GetTrianglesOfAnyDist()
                .FindAll(tri => tri.IsTriWithinVerticalDistOfCenter(_withinDist, centerY));
            if (_excludeDeathBarriers)
            {
                tris = tris.FindAll(tri => tri.SurfaceType != 0x0A);
            }
            return tris;
        }

        protected abstract List<TriangleDataModel> GetTrianglesOfAnyDist();

        protected static (float x, float y, float z) OffsetVertex(
            (float x, float y, float z) vertex, float xOffset, float yOffset, float zOffset)
        {
            return (vertex.x + xOffset, vertex.y + yOffset, vertex.z + zOffset);
        }

        protected List<ToolStripMenuItem> GetTriangleToolStripMenuItems()
        {
            ToolStripMenuItem itemSetWithinDist = new ToolStripMenuItem("Set Within Dist");
            itemSetWithinDist.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter the vertical distance from the center (default: Mario) within which to show tris.");
                float? withinDistNullable = ParsingUtilities.ParseFloatNullable(text);
                if (!withinDistNullable.HasValue) return;
                _withinDist = withinDistNullable.Value;
            };

            ToolStripMenuItem itemClearWithinDist = new ToolStripMenuItem("Clear Within Dist");
            itemClearWithinDist.Click += (sender, e) =>
            {
                _withinDist = null;
            };

            ToolStripMenuItem itemSetWithinCenter = new ToolStripMenuItem("Set Within Center");
            itemSetWithinCenter.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter the center y of the within-dist range.");
                float? withinCenterNullable =
                    text == "" ?
                    Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset) :
                    ParsingUtilities.ParseFloatNullable(text);
                if (!withinCenterNullable.HasValue) return;
                _withinCenter = withinCenterNullable.Value;
            };

            ToolStripMenuItem itemClearWithinCenter = new ToolStripMenuItem("Clear Within Center");
            itemClearWithinCenter.Click += (sender, e) =>
            {
                _withinCenter = null;
            };

            return new List<ToolStripMenuItem>()
            {
                itemSetWithinDist,
                itemClearWithinDist,
                itemSetWithinCenter,
                itemClearWithinCenter,
            };
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Perspective;
        }
    }
}

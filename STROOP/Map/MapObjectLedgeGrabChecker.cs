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
using System.Windows.Forms;

namespace STROOP.Map
{
    public class MapObjectLedgeGrabChecker : MapObjectLine
    {
        private uint? _customWallTri;

        private ToolStripMenuItem _itemSetCustomWallTriangle;

        private static readonly string SET_CUSTOM_WALL_TRIANGLE_TEXT = "Set Custom Wall Triangle";

        public MapObjectLedgeGrabChecker()
            : base()
        {
            OutlineWidth = 5;
            OutlineColor = Color.Purple;

            _customWallTri = null;
        }

        protected override List<(float x, float y, float z)> GetVerticesTopDownView()
        {
            float marioX = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
            float marioZ = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZOffset);

            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();

            vertices.Add((marioX, marioY + 30, marioZ));
            vertices.Add((marioX, marioY + 150, marioZ));

            uint wallTriangle = _customWallTri ?? Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.WallTriangleOffset);
            if (wallTriangle != 0)
            {
                double wallUphillAngle = WatchVariableSpecialUtilities.GetTriangleUphillAngle(wallTriangle);
                (float x2, float z2) = ((float, float))MoreMath.AddVectorToPoint(60, wallUphillAngle, marioX, marioZ);
                vertices.Add((x2, marioY + 100, z2));
                vertices.Add((x2, marioY + 238, z2));
            }

            return vertices;
        }


        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                _itemSetCustomWallTriangle = new ToolStripMenuItem(SET_CUSTOM_WALL_TRIANGLE_TEXT);
                _itemSetCustomWallTriangle.Click += (sender, e) =>
                {
                    string text = DialogUtilities.GetStringFromDialog(labelText: "Enter wall triangle as hex uint.");
                    uint? wallTriangleNullable = ParsingUtilities.ParseHexNullable(text);
                    if (!wallTriangleNullable.HasValue) return;
                    uint wallTriangle = wallTriangleNullable.Value;
                    MapObjectSettings settings = new MapObjectSettings(
                        changeTriangle: true, newTriangle: wallTriangle);
                    GetParentMapTracker().ApplySettings(settings);
                };

                ToolStripMenuItem itemClearCustomWallTriangle = new ToolStripMenuItem("Clear Custom Wall Triangle");
                itemClearCustomWallTriangle.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        changeTriangle: true, newTriangle: null);
                    GetParentMapTracker().ApplySettings(settings);
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(_itemSetCustomWallTriangle);
                _contextMenuStrip.Items.Add(itemClearCustomWallTriangle);
            }

            return _contextMenuStrip;
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            base.ApplySettings(settings);

            if (settings.ChangeTriangle)
            {
                _customWallTri = settings.NewTriangle;
                string suffix = _customWallTri.HasValue ? string.Format(" ({0})", HexUtilities.FormatValue(_customWallTri)) : "";
                _itemSetCustomWallTriangle.Text = SET_CUSTOM_WALL_TRIANGLE_TEXT + suffix;
            }
        }

        public override string GetName()
        {
            return "Ledge Grab Checker";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.CellGridlinesImage;
        }
    }
}

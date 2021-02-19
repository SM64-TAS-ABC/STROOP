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
    public class MapLedgeGrabCheckerObject : MapLineObject
    {
        private uint? _customWallTri;

        public MapLedgeGrabCheckerObject()
            : base()
        {
            OutlineWidth = 5;
            OutlineColor = Color.Purple;

            _customWallTri = null;
        }

        protected override List<(float x, float y, float z)> GetVerticesTopDownView()
        {
            float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);

            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();

            vertices.Add((marioX, marioY + 30, marioZ));
            vertices.Add((marioX, marioY + 150, marioZ));

            uint wallTriangle = _customWallTri ?? Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.WallTriangleOffset);
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
                ToolStripMenuItem itemSetCustomWallTriangle = new ToolStripMenuItem("Set Custom Wall Triangle...");
                itemSetCustomWallTriangle.Click += (sender, e) =>
                {
                    string text = DialogUtilities.GetStringFromDialog(labelText: "Enter wall triangle as hex uint.");
                    uint? wallTriangleNullable = ParsingUtilities.ParseHexNullable(text);
                    if (!wallTriangleNullable.HasValue) return;
                    uint wallTriangle = wallTriangleNullable.Value;
                    _customWallTri = wallTriangle;
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(itemSetCustomWallTriangle);
            }

            return _contextMenuStrip;
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

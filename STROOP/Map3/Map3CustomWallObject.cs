using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using STROOP.Controls.Map;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;
using System.Drawing.Imaging;
using STROOP.Models;
using System.Windows.Forms;

namespace STROOP.Map3
{
    public class Map3CustomWallObject : Map3WallObject
    {
        private readonly List<uint> _triAddressList;
        private float? _relativeHeight;

        public Map3CustomWallObject(List<uint> triAddressList)
            : base()
        {
            _triAddressList = triAddressList;
            _relativeHeight = null;

            Opacity = 0.5;
            Color = Color.Green;
        }

        public static Map3CustomWallObject Create(string text)
        {
            if (text == null) return null;
            List<uint?> nullableUIntList = ParsingUtilities.ParseStringList(text)
                .ConvertAll(word => ParsingUtilities.ParseHexNullable(word));
            if (nullableUIntList.Any(nullableUInt => !nullableUInt.HasValue))
            {
                return null;
            }
            List<uint> uintList = nullableUIntList.ConvertAll(nullableUInt => nullableUInt.Value);
            return new Map3CustomWallObject(uintList);
        }

        protected override List<(float x1, float z1, float x2, float z2, bool xProjection)> GetWallData()
        {
            float marioHeight = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            float? height = _relativeHeight.HasValue ? marioHeight - _relativeHeight.Value : (float?)null;
            return _triAddressList.ConvertAll(address => new TriangleDataModel(address))
                .ConvertAll(tri => Map3Utilities.GetWallDataFromTri(tri, height))
                .FindAll(wallDataNullable => wallDataNullable.HasValue)
                .ConvertAll(wallDataNullable => wallDataNullable.Value);
        }

        public override string GetName()
        {
            return "Custom Wall Tris";
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.TriangleWallImage;
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                ToolStripMenuItem itemSetRelativeHeight = new ToolStripMenuItem("Set Relative Height");
                itemSetRelativeHeight.Click += (sender, e) =>
                {
                    string text = DialogUtilities.GetStringFromDialog(labelText: "Enter relative height of wall hitbox compared to wall triangle.");
                    float? relativeHeightNullable = ParsingUtilities.ParseFloatNullable(text);
                    if (!relativeHeightNullable.HasValue) return;
                    _relativeHeight = relativeHeightNullable.Value;
                };

                ToolStripMenuItem itemClearRelativeHeight = new ToolStripMenuItem("Clear Relative Height");
                itemClearRelativeHeight.Click += (sender, e) =>
                {
                    _relativeHeight = null;
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(itemSetRelativeHeight);
                _contextMenuStrip.Items.Add(itemClearRelativeHeight);
            }

            return _contextMenuStrip;
        }
    }
}

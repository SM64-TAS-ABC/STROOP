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
using STROOP.Forms;

namespace STROOP.Map3
{
    public class Map3LevelTriangleObject : Map3TriangleObject
    {
        private readonly List<uint> _triAddressList;

        public Map3LevelTriangleObject()
            : base()
        {
            _triAddressList = TriangleUtilities.GetLevelTriangles()
                .ConvertAll(tri => tri.Address);

            Opacity = 0.5;
        }

        protected override bool UseAutomaticColoring()
        {
            return true;
        }

        public override void DrawOn2DControl()
        {
            // do nothing
        }

        protected override List<TriangleDataModel> GetTriangles()
        {
            return Map3Utilities.GetTriangles(_triAddressList);
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                ToolStripMenuItem itemReset = new ToolStripMenuItem("Reset");
                itemReset.Click += (sender, e) =>
                {
                    _triAddressList.Clear();
                    _triAddressList.AddRange(TriangleUtilities.GetLevelTriangles()
                        .ConvertAll(tri => tri.Address));
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(itemReset);
            }

            return _contextMenuStrip;
        }

        public override string GetName()
        {
            return "Level Tris";
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.HolpImage;
        }
    }
}

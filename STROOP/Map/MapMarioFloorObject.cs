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
    public class MapMarioFloorObject : MapFloorObject
    {
        public MapMarioFloorObject()
            : base()
        {
        }

        protected override List<TriangleDataModel> GetTrianglesOfAnyDist()
        {
            uint triAddress = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
            return MapUtilities.GetTriangles(triAddress);
        }

        public override string GetName()
        {
            return "Floor Tri";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.TriangleFloorImage;
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                _contextMenuStrip = new ContextMenuStrip();
                GetFloorToolStripMenuItems().ForEach(item => _contextMenuStrip.Items.Add(item));
                _contextMenuStrip.Items.Add(new ToolStripSeparator());
                GetHorizontalTriangleToolStripMenuItems().ForEach(item => _contextMenuStrip.Items.Add(item));
                _contextMenuStrip.Items.Add(new ToolStripSeparator());
                GetTriangleToolStripMenuItems().ForEach(item => _contextMenuStrip.Items.Add(item));
            }

            return _contextMenuStrip;
        }
    }
}

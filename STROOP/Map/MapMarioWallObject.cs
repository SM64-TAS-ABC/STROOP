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
    public class MapMarioWallObject : MapWallObject
    {
        private readonly List<uint> _triAddressList;

        public MapMarioWallObject()
            : base()
        {
        }

        protected override List<TriangleDataModel> GetTriangles()
        {
            uint triAddress = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.WallTriangleOffset);
            return MapUtilities.GetTriangles(triAddress);
        }

        public override string GetName()
        {
            return "Wall Tri";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.TriangleWallImage;
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                _contextMenuStrip = CreateWallContextMenuStrip();
            }

            return _contextMenuStrip;
        }
    }
}

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
    public class MapObjectCellGridlines : MapObjectLine
    {
        public MapObjectCellGridlines()
            : base()
        {
            LineWidth = 3;
            LineColor = Color.Black;
        }

        protected override List<(float x, float y, float z)> GetVerticesTopDownView()
        {
            float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);

            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
            for (int x = -8192; x <= 8192; x += 1024)
            {
                vertices.Add((x, marioY, - 8192));
                vertices.Add((x, marioY, 8192));
            }
            for (int z = -8192; z <= 8192; z += 1024)
            {
                vertices.Add((-8192, marioY, z));
                vertices.Add((8192, marioY, z));
            }
            return vertices;
        }

        public override string GetName()
        {
            return "Cell Gridlines";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.CellGridlinesImage;
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                _contextMenuStrip = new ContextMenuStrip();
                GetLineToolStripMenuItems().ForEach(item => _contextMenuStrip.Items.Add(item));
            }

            return _contextMenuStrip;
        }
    }
}

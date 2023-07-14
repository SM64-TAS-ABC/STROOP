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
    public class MapObjectCellGridlines : MapObjectGridlines
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

            int min = -8192;
            int max = 8192;
            int gap = 1024;
            List<int> values = ExtendedLevelBoundariesUtilities.GetValuesInRange(min, max, gap, false, true, true);

            int convertedMin = ExtendedLevelBoundariesUtilities.Convert(min, false);
            int convertedMax = ExtendedLevelBoundariesUtilities.Convert(max, false);

            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
            foreach (int x in values)
            {
                vertices.Add((x, marioY, convertedMin));
                vertices.Add((x, marioY, convertedMax));
            }
            foreach (int z in values)
            {
                vertices.Add((convertedMin, marioY, z));
                vertices.Add((convertedMax, marioY, z));
            }
            return vertices;
        }

        protected override List<(float x, float y, float z)> GetGridlineIntersectionPositionsTopDownView()
        {
            float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);

            int min = -8192;
            int max = 8192;
            int gap = 1024;
            List<int> values = ExtendedLevelBoundariesUtilities.GetValuesInRange(min, max, gap, false, true, true);

            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
            foreach (int x in values)
            {
                foreach (int z in values)
                {
                    vertices.Add((x, marioY, z));
                }
            }
            return vertices;
        }

        protected override List<(float x, float y, float z)> GetGridlineIntersectionPositionsOrthographicView()
        {
            return GetGridlineIntersectionPositionsTopDownView();
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
                GetGridlinesToolStripMenuItems().ForEach(item => _contextMenuStrip.Items.Add(item));
            }

            return _contextMenuStrip;
        }
    }
}

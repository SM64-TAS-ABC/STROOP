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
    public class MapObjectCustomGridlines : MapObjectGridlines
    {
        public MapObjectCustomGridlines()
            : base()
        {
            Size = 2;
            LineWidth = 3;
            LineColor = Color.Black;
        }

        protected override List<(float x, float y, float z)> GetVerticesTopDownView()
        {
            float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);

            int min = -8192;
            int max = 8192;
            int size = (int)Size;
            if (size < 1) size = 1;
            List<float> values = ExtendedLevelBoundariesUtilities.GetValuesInRangeFloat(min, max, size);

            int convertedMin = ExtendedLevelBoundariesUtilities.Convert(min, false);
            int convertedMax = ExtendedLevelBoundariesUtilities.Convert(max, false);

            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
            foreach (float x in values)
            {
                vertices.Add((x, marioY, convertedMin));
                vertices.Add((x, marioY, convertedMax));
            }
            foreach (float z in values)
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
            int size = (int)Size;
            if (size < 1) size = 1;
            List<float> values = ExtendedLevelBoundariesUtilities.GetValuesInRangeFloat(min, max, size);

            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
            foreach (float x in values)
            {
                foreach (float z in values)
                {
                    vertices.Add((x, marioY, z));
                }
            }
            return vertices;
        }

        protected override List<(float x, float y, float z)> GetVerticesOrthographicView()
        {
            int min = -8192;
            int max = 8192;
            int size = (int)Size;
            if (size < 1) size = 1;
            List<float> values = ExtendedLevelBoundariesUtilities.GetValuesInRangeFloat(min, max, size);

            int convertedMin = ExtendedLevelBoundariesUtilities.Convert(min, false);
            int convertedMax = ExtendedLevelBoundariesUtilities.Convert(max, false);

            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
            foreach (float x in values)
            {
                foreach (float y in values)
                {
                    vertices.Add((x, y, convertedMin));
                    vertices.Add((x, y, convertedMax));
                }
            }
            foreach (float x in values)
            {
                foreach (float z in values)
                {
                    vertices.Add((x, convertedMin, z));
                    vertices.Add((x, convertedMax, z));
                }
            }
            foreach (float z in values)
            {
                foreach (float y in values)
                {
                    vertices.Add((convertedMin, y, z));
                    vertices.Add((convertedMax, y, z));
                }
            }
            return vertices;
        }

        protected override List<(float x, float y, float z)> GetGridlineIntersectionPositionsOrthographicView()
        {
            float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);

            int min = -8192;
            int max = 8192;
            int size = (int)Size;
            if (size < 1) size = 1;
            List<float> values = ExtendedLevelBoundariesUtilities.GetValuesInRangeFloat(min, max, size);

            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
            foreach (float x in values)
            {
                foreach (float y in values)
                {
                    foreach (float z in values)
                    {
                        vertices.Add((x, y, z));
                    }
                }
            }
            return vertices;
        }

        public override string GetName()
        {
            return "Custom Gridlines";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.CustomGridlinesImage;
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

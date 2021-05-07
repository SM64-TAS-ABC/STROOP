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

namespace STROOP.Map
{
    public class MapObjectWaters : MapObjectQuad
    {
        public MapObjectWaters()
            : base()
        {
            Opacity = 0.5;
            Color = Color.Purple;
        }

        protected override List<List<(float x, float y, float z)>> GetQuadList()
        {
            List<(int y, int xMin, int xMax, int zMin, int zMax)> waters = WaterUtilities.GetWaterLevels();
            List<List<(float x, float y, float z)>> quads = new List<List<(float x, float y, float z)>>();
            foreach (var water in waters)
            {
                List<(float x, float y, float z)> quad = new List<(float x, float y, float z)>();
                quad.Add((water.xMin, water.y, water.zMin));
                quad.Add((water.xMin, water.y, water.zMax));
                quad.Add((water.xMax, water.y, water.zMax));
                quad.Add((water.xMax, water.y, water.zMin));
                quads.Add(quad);
            }
            return quads;
        }

        public override string GetName()
        {
            return "Waters";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.WatersImage;
        }
    }
}

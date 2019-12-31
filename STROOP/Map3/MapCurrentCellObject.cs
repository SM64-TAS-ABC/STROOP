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

namespace STROOP.Map3
{
    public class MapCurrentCellObject : MapQuadObject
    {
        public MapCurrentCellObject()
            : base()
        {
            Opacity = 0.5;
            Color = Color.Yellow;
        }

        protected override List<List<(float x, float y, float z)>> GetQuadList()
        {
            float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            (int cellX, int cellZ) = WatchVariableSpecialUtilities.GetMarioCell();
            int xMin = (cellX - 8) * 1024;
            int xMax = xMin + 1024;
            int zMin = (cellZ - 8) * 1024;
            int zMax = zMin + 1024;
            List<(float x, float y, float z)> quad =
                new List<(float x, float y, float z)>()
                {
                    (xMin, marioY, zMin),
                    (xMin, marioY, zMax),
                    (xMax, marioY, zMax),
                    (xMax, marioY, zMin),
                };
            return new List<List<(float x, float y, float z)>>() { quad };
        }

        public override string GetName()
        {
            return "Current Cell";
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.CurrentCellImage;
        }
    }
}

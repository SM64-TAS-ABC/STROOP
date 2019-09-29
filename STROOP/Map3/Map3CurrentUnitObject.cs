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

namespace STROOP.Map3
{
    public class Map3CurrentUnitObject : Map3QuadObject
    {
        public Map3CurrentUnitObject()
            : base()
        {
            Opacity = 0.5;
            Color = Color.Purple;
        }

        protected override List<List<(float x, float z)>> GetQuadList()
        {
            float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
            int xMin = (short)marioX;
            int xMax = xMin + (marioX >= 0 ? 1 : -1);
            int zMin = (short)marioZ;
            int zMax = zMin + (marioZ >= 0 ? 1 : -1);

            List<(float x, float z)> quad =
                new List<(float x, float z)>()
                {
                    (xMin, zMin),
                    (xMin, zMax),
                    (xMax, zMax),
                    (xMax, zMin),
                };
            return new List<List<(float x, float z)>>() { quad };
        }

        public override string GetName()
        {
            return "Current Unit";
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.CurrentUnitImage;
        }
    }
}

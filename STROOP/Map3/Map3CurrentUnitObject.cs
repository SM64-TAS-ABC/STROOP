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
            Color = Color.Purple;
            Opacity = 0.5;
        }

        protected override List<(float x, float z)> GetVertices()
        {
            float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
            int xMin = (short)marioX;
            int xMax = xMin + (marioX >= 0 ? 1 : -1);
            int zMin = (short)marioZ;
            int zMax = zMin + (marioZ >= 0 ? 1 : -1);

            return new List<(float x, float z)>()
            {
                (xMin, zMin),
                (xMin, zMax),
                (xMax, zMax),
                (xMax, zMin),
            };
        }

        public override string GetName()
        {
            return "Current Unit";
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.EmptyImage;
        }
    }
}

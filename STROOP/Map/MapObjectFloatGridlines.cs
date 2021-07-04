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
    public class MapObjectFloatGridlines : MapObjectLine
    {
        public MapObjectFloatGridlines()
            : base()
        {
            LineWidth = 1;
            LineColor = Color.Black;
        }

        protected override List<(float x, float y, float z)> GetVerticesTopDownView()
        {
            float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);

            float xMin = Config.CurrentMapGraphics.MapViewXMin;
            float xMax = Config.CurrentMapGraphics.MapViewXMax;
            float zMin = Config.CurrentMapGraphics.MapViewZMin;
            float zMax = Config.CurrentMapGraphics.MapViewZMax;

            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
            int xCounter = 0;
            for (float x = xMin; x <= xMax; x = MoreMath.GetNextFloat(x))
            {
                vertices.Add((x, marioY, zMin));
                vertices.Add((x, marioY, zMax));
                xCounter++;
                if (xCounter > 4000) break;
            }
            int zCounter = 0;
            for (float z = zMin; z <= zMax; z = MoreMath.GetNextFloat(z))
            {
                vertices.Add((xMin, marioY, z));
                vertices.Add((xMax, marioY, z));
                zCounter++;
                if (zCounter > 4000) break;
            }

            // failsafe to prevent filling the whole screen
            if (xCounter > Config.MapGui.CurrentControl.Width ||
                zCounter > Config.MapGui.CurrentControl.Height)
            {
                return new List<(float x, float y, float z)>();
            }

            return vertices;
        }

        protected override List<(float x, float y, float z)> GetVerticesOrthographicView()
        {
            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();

            if (Config.CurrentMapGraphics.MapViewPitchValue == 0)
            {
                float xMin = Config.CurrentMapGraphics.MapViewXMin;
                float xMax = Config.CurrentMapGraphics.MapViewXMax;
                float yMin = Config.CurrentMapGraphics.MapViewYMin;
                float yMax = Config.CurrentMapGraphics.MapViewYMax;
                float zMin = Config.CurrentMapGraphics.MapViewZMin;
                float zMax = Config.CurrentMapGraphics.MapViewZMax;

                int yCounter = 0;
                for (float y = yMin; y <= yMax; y = MoreMath.GetNextFloat(y))
                {
                    vertices.Add((float.NegativeInfinity, y, float.NegativeInfinity));
                    vertices.Add((float.PositiveInfinity, y, float.PositiveInfinity));
                    yCounter++;
                    if (yCounter > 4000) break;
                }
                //int zCounter = 0;
                //for (float z = zMin; z <= zMax; z = MoreMath.GetNextFloat(z))
                //{
                //    vertices.Add((xMin, marioY, z));
                //    vertices.Add((xMax, marioY, z));
                //    zCounter++;
                //    if (zCounter > 4000) break;
                //}

                //// failsafe to prevent filling the whole screen
                //if (xCounter > Config.MapGui.CurrentControl.Width ||
                //    zCounter > Config.MapGui.CurrentControl.Height)
                //{
                //    return new List<(float x, float y, float z)>();
                //}
            }

            return vertices;
        }

        public override string GetName()
        {
            return "Float Gridlines";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.UnitGridlinesImage;
        }
    }
}

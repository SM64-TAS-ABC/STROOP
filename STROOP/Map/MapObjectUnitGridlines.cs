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
    public class MapObjectUnitGridlines : MapObjectLine
    {
        public MapObjectUnitGridlines()
            : base()
        {
            LineWidth = 1;
            LineColor = Color.Black;
        }

        protected override List<(float x, float y, float z)> GetVerticesTopDownView()
        {
            // failsafe to prevent filling the whole screen
            if (!MapUtilities.IsAbleToShowUnitPrecision())
            {
                return new List<(float x, float y, float z)>();
            }

            float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);

            int xMin = (int)Config.CurrentMapGraphics.MapViewXMin - 1;
            int xMax = (int)Config.CurrentMapGraphics.MapViewXMax + 1;
            int zMin = (int)Config.CurrentMapGraphics.MapViewZMin - 1;
            int zMax = (int)Config.CurrentMapGraphics.MapViewZMax + 1;

            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
            for (int x = xMin; x <= xMax; x++)
            {
                vertices.Add((x, marioY, zMin));
                vertices.Add((x, marioY, zMax));
            }
            for (int z = zMin; z <= zMax; z++)
            {
                vertices.Add((xMin, marioY, z));
                vertices.Add((xMax, marioY, z));
            }
            return vertices;
        }

        protected override List<(float x, float z)> GetCustomImagePositions()
        {
            // failsafe to prevent filling the whole screen
            if (!MapUtilities.IsAbleToShowUnitPrecision())
            {
                return new List<(float x, float z)>();
            }

            int xMin = (int)Config.CurrentMapGraphics.MapViewXMin - 1;
            int xMax = (int)Config.CurrentMapGraphics.MapViewXMax + 1;
            int zMin = (int)Config.CurrentMapGraphics.MapViewZMin - 1;
            int zMax = (int)Config.CurrentMapGraphics.MapViewZMax + 1;

            List<(float x, float z)> vertices = new List<(float x, float z)>();
            for (int x = xMin; x <= xMax; x++)
            {
                for (int z = zMin; z <= zMax; z++)
                {
                    vertices.Add((x, z));
                }
            }
            return vertices;
        }

        protected override List<(float x, float y, float z)> GetVerticesOrthographicView()
        {
            // failsafe to prevent filling the whole screen
            if (!MapUtilities.IsAbleToShowUnitPrecision())
            {
                return new List<(float x, float y, float z)>();
            }

            float xCenter = Config.CurrentMapGraphics.MapViewCenterXValue;
            float zCenter = Config.CurrentMapGraphics.MapViewCenterZValue;
            int xMin = (int)Config.CurrentMapGraphics.MapViewXMin - 1;
            int xMax = (int)Config.CurrentMapGraphics.MapViewXMax + 1;
            int yMin = (int)Config.CurrentMapGraphics.MapViewYMin - 1;
            int yMax = (int)Config.CurrentMapGraphics.MapViewYMax + 1;
            int zMin = (int)Config.CurrentMapGraphics.MapViewZMin - 1;
            int zMax = (int)Config.CurrentMapGraphics.MapViewZMax + 1;

            if (Config.CurrentMapGraphics.MapViewPitchValue == 0 &&
                (Config.CurrentMapGraphics.MapViewYawValue == 0 ||
                Config.CurrentMapGraphics.MapViewYawValue == 32768))
            {
                List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
                for (int x = xMin; x <= xMax; x++)
                {
                    vertices.Add((x, yMin, zCenter));
                    vertices.Add((x, yMax, zCenter));
                }
                for (int y = yMin; y <= yMax; y++)
                {
                    vertices.Add((xMin, y, zCenter));
                    vertices.Add((xMax, y, zCenter));
                }
                return vertices;
            }
            else if (Config.CurrentMapGraphics.MapViewPitchValue == 0 &&
                (Config.CurrentMapGraphics.MapViewYawValue == 16384 ||
                Config.CurrentMapGraphics.MapViewYawValue == 49152))
            {
                List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
                for (int z = zMin; z <= zMax; z++)
                {
                    vertices.Add((xCenter, yMin, z));
                    vertices.Add((xCenter, yMax, z));
                }
                for (int y = yMin; y <= yMax; y++)
                {
                    vertices.Add((zCenter, y, zMin));
                    vertices.Add((xCenter, y, zMax));
                }
                return vertices;
            }
            else if (Config.CurrentMapGraphics.MapViewPitchValue == 0)
            {
                List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
                for (int y = yMin; y <= yMax; y++)
                {
                    vertices.Add((float.NegativeInfinity, y, float.NegativeInfinity));
                    vertices.Add((float.PositiveInfinity, y, float.PositiveInfinity));
                }
                return vertices;
            }
            else
            {
                return new List<(float x, float y, float z)>();
            }
        }

        public override string GetName()
        {
            return "Unit Gridlines";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.UnitGridlinesImage;
        }
    }
}

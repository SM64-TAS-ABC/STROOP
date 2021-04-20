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

namespace STROOP.Map
{
    public class MapCustomGridlinesObject : MapLineObject
    {
        public MapCustomGridlinesObject()
            : base()
        {
            Size = 2;
            OutlineWidth = 3;
            OutlineColor = Color.Black;
        }

        protected override List<(float x, float y, float z)> GetVerticesTopDownView()
        {
            float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);

            int gridlineMin = -8192;
            int gridlineMax = 8192;

            double size = Size;
            if (size < 1) size = 1;
            double increment = 16384 / size;

            double viewXMin = Config.CurrentMapGraphics.MapViewXMin;
            double viewXMax = Config.CurrentMapGraphics.MapViewXMax;
            double viewXDiff = viewXMax - viewXMin;
            double viewXDiffPixels = viewXDiff * Config.CurrentMapGraphics.MapViewScaleValue;

            double viewZMin = Config.CurrentMapGraphics.MapViewZMin;
            double viewZMax = Config.CurrentMapGraphics.MapViewZMax;
            double viewZDiff = viewZMax - viewZMin;
            double viewZDiffPixels = viewZDiff * Config.CurrentMapGraphics.MapViewScaleValue;

            int xMinMultiple = Math.Max((int)((viewXMin - gridlineMin) / increment) - 1, 0);
            int xMaxMultiple = Math.Min((int)((viewXMax - gridlineMin) / increment) + 1, (int)size);
            int numXLines = xMaxMultiple - xMinMultiple + 1;

            int zMinMultiple = Math.Max((int)((viewZMin - gridlineMin) / increment) - 1, 0);
            int zMaxMultiple = Math.Min((int)((viewZMax - gridlineMin) / increment) + 1, (int)size);
            int numZLines = zMaxMultiple - zMinMultiple + 1;

            if (numXLines > viewXDiffPixels || numZLines > viewZDiffPixels)
                return new List<(float x, float y, float z)>();
            
            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
            for (int multiple = xMinMultiple; multiple <= xMaxMultiple; multiple++)
            {
                float x = (float)(multiple * increment + gridlineMin);
                vertices.Add((x, marioY, gridlineMin));
                vertices.Add((x, marioY, gridlineMax));
            }
            for (int multiple = zMinMultiple; multiple <= zMaxMultiple; multiple++)
            {
                float z = (float)(multiple * increment + gridlineMin);
                vertices.Add((gridlineMin, marioY, z));
                vertices.Add((gridlineMax, marioY, z));
            }
            return vertices;
        }

        protected override List<(float x, float y, float z)> GetVerticesOrthographicView()
        {
            int gridlineMin = -8192;
            int gridlineMax = 8192;

            double size = Size;
            if (size < 1) size = 1;
            double increment = 16384 / size;

            double viewXMin = Config.CurrentMapGraphics.MapViewXMin;
            double viewXMax = Config.CurrentMapGraphics.MapViewXMax;
            double viewXDiff = viewXMax - viewXMin;
            double viewXDiffPixels = viewXDiff * Config.CurrentMapGraphics.MapViewScaleValue;

            double viewYMin = Config.CurrentMapGraphics.MapViewYMin;
            double viewYMax = Config.CurrentMapGraphics.MapViewYMax;
            double viewYDiff = viewYMax - viewYMin;
            double viewYDiffPixels = viewYDiff * Config.CurrentMapGraphics.MapViewScaleValue;

            double viewZMin = Config.CurrentMapGraphics.MapViewZMin;
            double viewZMax = Config.CurrentMapGraphics.MapViewZMax;
            double viewZDiff = viewZMax - viewZMin;
            double viewZDiffPixels = viewZDiff * Config.CurrentMapGraphics.MapViewScaleValue;

            int xMinMultiple = Math.Max((int)((viewXMin - gridlineMin) / increment) - 1, 0);
            int xMaxMultiple = Math.Min((int)((viewXMax - gridlineMin) / increment) + 1, (int)size);
            int numXLines = xMaxMultiple - xMinMultiple + 1;

            int yMinMultiple = Math.Max((int)((viewYMin - gridlineMin) / increment) - 1, 0);
            int yMaxMultiple = Math.Min((int)((viewYMax - gridlineMin) / increment) + 1, (int)size);
            int numYLines = yMaxMultiple - yMinMultiple + 1;

            int zMinMultiple = Math.Max((int)((viewZMin - gridlineMin) / increment) - 1, 0);
            int zMaxMultiple = Math.Min((int)((viewZMax - gridlineMin) / increment) + 1, (int)size);
            int numZLines = zMaxMultiple - zMinMultiple + 1;

            if (numXLines > viewXDiffPixels || numYLines > viewYDiffPixels || numZLines > viewZDiffPixels)
                return new List<(float x, float y, float z)>();

            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
            for (int xMultiple = xMinMultiple; xMultiple <= xMaxMultiple; xMultiple++)
            {
                float x = (float)(xMultiple * increment + gridlineMin);
                for (int yMultiple = yMinMultiple; yMultiple <= yMaxMultiple; yMultiple++)
                {
                    float y = (float)(yMultiple * increment + gridlineMin);
                    vertices.Add((x, y, gridlineMin));
                    vertices.Add((x, y, gridlineMax));
                }
            }
            for (int xMultiple = xMinMultiple; xMultiple <= xMaxMultiple; xMultiple++)
            {
                float x = (float)(xMultiple * increment + gridlineMin);
                for (int zMultiple = zMinMultiple; zMultiple <= zMaxMultiple; zMultiple++)
                {
                    float z = (float)(zMultiple * increment + gridlineMin);
                    vertices.Add((x, gridlineMin, z));
                    vertices.Add((x, gridlineMax, z));
                }
            }
            for (int zMultiple = zMinMultiple; zMultiple <= zMaxMultiple; zMultiple++)
            {
                float z = (float)(zMultiple * increment + gridlineMin);
                for (int yMultiple = yMinMultiple; yMultiple <= yMaxMultiple; yMultiple++)
                {
                    float y = (float)(yMultiple * increment + gridlineMin);
                    vertices.Add((gridlineMin, y, z));
                    vertices.Add((gridlineMax, y, z));
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
    }
}

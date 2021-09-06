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
using System.Windows.Forms;

namespace STROOP.Map
{
    public class MapObjectFloatGridlines : MapObjectGridlines
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

        protected override List<(float x, float y, float z)> GetGridlineIntersectionPositionsTopDownView()
        {
            float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);

            float xMin = Config.CurrentMapGraphics.MapViewXMin;
            float xMax = Config.CurrentMapGraphics.MapViewXMax;
            float zMin = Config.CurrentMapGraphics.MapViewZMin;
            float zMax = Config.CurrentMapGraphics.MapViewZMax;

            int xCounter = 0;
            for (float x = xMin; x <= xMax; x = MoreMath.GetNextFloat(x))
            {
                xCounter++;
                if (xCounter > 4000) break;
            }
            int zCounter = 0;
            for (float z = zMin; z <= zMax; z = MoreMath.GetNextFloat(z))
            {
                zCounter++;
                if (zCounter > 4000) break;
            }

            // failsafe to prevent filling the whole screen
            if (xCounter > Config.MapGui.CurrentControl.Width ||
                zCounter > Config.MapGui.CurrentControl.Height)
            {
                return new List<(float x, float y, float z)>();
            }

            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
            for (float x = xMin; x <= xMax; x = MoreMath.GetNextFloat(x))
            {
                for (float z = zMin; z <= zMax; z = MoreMath.GetNextFloat(z))
                {
                    vertices.Add((x, marioY, z));
                }
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

                int xCounter = 0;
                if (Config.CurrentMapGraphics.MapViewYawValue == 0 ||
                    Config.CurrentMapGraphics.MapViewYawValue == 32768)
                {
                    for (float x = xMin; x <= xMax; x = MoreMath.GetNextFloat(x))
                    {
                        vertices.Add((x, yMin, Config.CurrentMapGraphics.MapViewCenterZValue));
                        vertices.Add((x, yMax, Config.CurrentMapGraphics.MapViewCenterZValue));
                        xCounter++;
                        if (xCounter > 4000) break;
                    }
                }

                int zCounter = 0;
                if (Config.CurrentMapGraphics.MapViewYawValue == 16384 ||
                    Config.CurrentMapGraphics.MapViewYawValue == 49152)
                {
                    for (float z = zMin; z <= zMax; z = MoreMath.GetNextFloat(z))
                    {
                        vertices.Add((Config.CurrentMapGraphics.MapViewCenterXValue, yMin, z));
                        vertices.Add((Config.CurrentMapGraphics.MapViewCenterXValue, yMax, z));
                        zCounter++;
                        if (zCounter > 4000) break;
                    }
                }

                // failsafe to prevent filling the whole screen
                if (xCounter > Config.MapGui.CurrentControl.Width ||
                    yCounter > Config.MapGui.CurrentControl.Height ||
                    zCounter > Config.MapGui.CurrentControl.Width)
                {
                    return new List<(float x, float y, float z)>();
                }
            }

            return vertices;
        }

        protected override List<(float x, float y, float z)> GetGridlineIntersectionPositionsOrthographicView()
        {
            if (Config.CurrentMapGraphics.MapViewPitchValue == 0)
            {
                float xMin = Config.CurrentMapGraphics.MapViewXMin;
                float xMax = Config.CurrentMapGraphics.MapViewXMax;
                float yMin = Config.CurrentMapGraphics.MapViewYMin;
                float yMax = Config.CurrentMapGraphics.MapViewYMax;
                float zMin = Config.CurrentMapGraphics.MapViewZMin;
                float zMax = Config.CurrentMapGraphics.MapViewZMax;

                List<float> yValues = new List<float>();
                for (float y = yMin; y <= yMax; y = MoreMath.GetNextFloat(y))
                {
                    yValues.Add(y);
                    if (yValues.Count > 4000) break;
                }

                List<float> xValues = new List<float>();
                if (Config.CurrentMapGraphics.MapViewYawValue == 0 ||
                    Config.CurrentMapGraphics.MapViewYawValue == 32768)
                {
                    for (float x = xMin; x <= xMax; x = MoreMath.GetNextFloat(x))
                    {
                        xValues.Add(x);
                        if (xValues.Count > 4000) break;
                    }
                }

                List<float> zValues = new List<float>();
                if (Config.CurrentMapGraphics.MapViewYawValue == 16384 ||
                    Config.CurrentMapGraphics.MapViewYawValue == 49152)
                {
                    for (float z = zMin; z <= zMax; z = MoreMath.GetNextFloat(z))
                    {
                        zValues.Add(z);
                        if (zValues.Count > 4000) break;
                    }
                }

                // failsafe to prevent filling the whole screen
                if (xValues.Count > Config.MapGui.CurrentControl.Width ||
                    yValues.Count > Config.MapGui.CurrentControl.Height ||
                    zValues.Count > Config.MapGui.CurrentControl.Width)
                {
                    return new List<(float x, float y, float z)>();
                }


                if (Config.CurrentMapGraphics.MapViewYawValue == 0 ||
                    Config.CurrentMapGraphics.MapViewYawValue == 32768)
                {
                    List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
                    foreach (float x in xValues)
                    {
                        foreach (float y in yValues)
                        {
                            vertices.Add((x, y, Config.CurrentMapGraphics.MapViewCenterZValue));
                        }
                    }
                    return vertices;
                }

                if (Config.CurrentMapGraphics.MapViewYawValue == 16384 ||
                    Config.CurrentMapGraphics.MapViewYawValue == 49152)
                {
                    List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
                    foreach (float z in zValues)
                    {
                        foreach (float y in yValues)
                        {
                            vertices.Add((Config.CurrentMapGraphics.MapViewCenterXValue, y, z));
                        }
                    }
                    return vertices;
                }
            }

            return new List<(float x, float y, float z)>();
        }

        public override string GetName()
        {
            return "Float Gridlines";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.UnitGridlinesImage;
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

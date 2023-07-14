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
    public class MapObjectUnitGridlines : MapObjectGridlines
    {
        public MapObjectUnitGridlines()
            : base()
        {
            LineWidth = 1;
            LineColor = Color.Black;
        }

        private int Normalize(float number)
        {
            return (int)number / _multiplier * _multiplier;
        }

        protected override List<(float x, float y, float z)> GetVerticesTopDownView()
        {
            // failsafe to prevent filling the whole screen
            if (!MapUtilities.IsAbleToShowUnitPrecision())
            {
                return new List<(float x, float y, float z)>();
            }

            float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);

            List<long> xValues = ExtendedLevelBoundariesUtilities.GetValuesInRange(
                (long)Config.CurrentMapGraphics.MapViewXMin, (long)Config.CurrentMapGraphics.MapViewXMax,
                1, false, ExtendedLevelBoundariesUtilities.ValueOffsetType.GO_THROUGH_VALUE, 0, false, true, true);
            List<long> zValues = ExtendedLevelBoundariesUtilities.GetValuesInRange(
                (long)Config.CurrentMapGraphics.MapViewZMin, (long)Config.CurrentMapGraphics.MapViewZMax,
                1, false, ExtendedLevelBoundariesUtilities.ValueOffsetType.GO_THROUGH_VALUE, 0, false, true, true);

            float xMin = Config.CurrentMapGraphics.MapViewXMin;
            float xMax = Config.CurrentMapGraphics.MapViewXMax;
            float zMin = Config.CurrentMapGraphics.MapViewZMin;
            float zMax = Config.CurrentMapGraphics.MapViewZMax;

            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
            foreach (long x in xValues)
            {
                vertices.Add((x, marioY, zMin));
                vertices.Add((x, marioY, zMax));
            }
            foreach (long z in zValues)
            {
                vertices.Add((xMin, marioY, z));
                vertices.Add((xMax, marioY, z));
            }
            return vertices;
        }

        protected override List<(float x, float y, float z)> GetGridlineIntersectionPositionsTopDownView()
        {
            // failsafe to prevent filling the whole screen
            if (!MapUtilities.IsAbleToShowUnitPrecision())
            {
                return new List<(float x, float y, float z)>();
            }

            float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);

            List<long> xValues = ExtendedLevelBoundariesUtilities.GetValuesInRange(
                (long)Config.CurrentMapGraphics.MapViewXMin, (long)Config.CurrentMapGraphics.MapViewXMax,
                1, false, ExtendedLevelBoundariesUtilities.ValueOffsetType.GO_THROUGH_VALUE, 0, false, true, true);
            List<long> zValues = ExtendedLevelBoundariesUtilities.GetValuesInRange(
                (long)Config.CurrentMapGraphics.MapViewZMin, (long)Config.CurrentMapGraphics.MapViewZMax,
                1, false, ExtendedLevelBoundariesUtilities.ValueOffsetType.GO_THROUGH_VALUE, 0, false, true, true);

            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
            foreach (long x in xValues)
            {
                foreach (long z in zValues)
                {
                    vertices.Add((x, marioY, z));
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
            int xMin = Normalize(Config.CurrentMapGraphics.MapViewXMin) - _multiplier;
            int xMax = Normalize(Config.CurrentMapGraphics.MapViewXMax) + _multiplier;
            int yMin = Normalize(Config.CurrentMapGraphics.MapViewYMin) - _multiplier;
            int yMax = Normalize(Config.CurrentMapGraphics.MapViewYMax) + _multiplier;
            int zMin = Normalize(Config.CurrentMapGraphics.MapViewZMin) - _multiplier;
            int zMax = Normalize(Config.CurrentMapGraphics.MapViewZMax) + _multiplier;

            if (Config.CurrentMapGraphics.MapViewPitchValue == 0 &&
                (Config.CurrentMapGraphics.MapViewYawValue == 0 ||
                Config.CurrentMapGraphics.MapViewYawValue == 32768))
            {
                List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
                for (int x = xMin; x <= xMax; x += _multiplier)
                {
                    vertices.Add((x, yMin, zCenter));
                    vertices.Add((x, yMax, zCenter));
                }
                for (int y = yMin; y <= yMax; y += _multiplier)
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
                for (int z = zMin; z <= zMax; z += _multiplier)
                {
                    vertices.Add((xCenter, yMin, z));
                    vertices.Add((xCenter, yMax, z));
                }
                for (int y = yMin; y <= yMax; y += _multiplier)
                {
                    vertices.Add((xCenter, y, zMin));
                    vertices.Add((xCenter, y, zMax));
                }
                return vertices;
            }
            else if (Config.CurrentMapGraphics.MapViewPitchValue == 0)
            {
                List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
                for (int y = yMin; y <= yMax; y += _multiplier)
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

        protected override List<(float x, float y, float z)> GetGridlineIntersectionPositionsOrthographicView()
        {
            // failsafe to prevent filling the whole screen
            if (!MapUtilities.IsAbleToShowUnitPrecision())
            {
                return new List<(float x, float y, float z)>();
            }

            float xCenter = Config.CurrentMapGraphics.MapViewCenterXValue;
            float zCenter = Config.CurrentMapGraphics.MapViewCenterZValue;
            int xMin = Normalize(Config.CurrentMapGraphics.MapViewXMin) - _multiplier;
            int xMax = Normalize(Config.CurrentMapGraphics.MapViewXMax) + _multiplier;
            int yMin = Normalize(Config.CurrentMapGraphics.MapViewYMin) - _multiplier;
            int yMax = Normalize(Config.CurrentMapGraphics.MapViewYMax) + _multiplier;
            int zMin = Normalize(Config.CurrentMapGraphics.MapViewZMin) - _multiplier;
            int zMax = Normalize(Config.CurrentMapGraphics.MapViewZMax) + _multiplier;

            if (Config.CurrentMapGraphics.MapViewPitchValue == 0 &&
                (Config.CurrentMapGraphics.MapViewYawValue == 0 ||
                Config.CurrentMapGraphics.MapViewYawValue == 32768))
            {
                List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
                for (int x = xMin; x <= xMax; x += _multiplier)
                {
                    for (int y = yMin; y <= yMax; y += _multiplier)
                    {
                        vertices.Add((x, y, zCenter));
                    }
                }
                return vertices;
            }
            else if (Config.CurrentMapGraphics.MapViewPitchValue == 0 &&
                (Config.CurrentMapGraphics.MapViewYawValue == 16384 ||
                Config.CurrentMapGraphics.MapViewYawValue == 49152))
            {
                List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
                for (int z = zMin; z <= zMax; z += _multiplier)
                {
                    for (int y = yMin; y <= yMax; y += _multiplier)
                    {
                        vertices.Add((xCenter, y, z));
                    }
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

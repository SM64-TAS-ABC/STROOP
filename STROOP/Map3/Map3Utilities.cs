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
    public static class Map3Utilities
    {
        /** Takes in in-game coordinates, outputs control coordinates. */
        public static (float x, float z) ConvertCoordsForControl(float x, float z)
        {
            x = Config.Map3Graphics.MapViewEnablePuView ? x : (float)PuUtilities.GetRelativeCoordinate(x);
            z = Config.Map3Graphics.MapViewEnablePuView ? z : (float)PuUtilities.GetRelativeCoordinate(z);
            float xOffset = x - Config.Map3Graphics.MapViewCenterXValue;
            float zOffset = z - Config.Map3Graphics.MapViewCenterZValue;
            (float xOffsetRotated, float zOffsetRotated) =
                ((float, float))MoreMath.RotatePointAboutPointAnAngularDistance(
                    xOffset,
                    zOffset,
                    0,
                    0,
                    -1 * Config.Map3Graphics.MapViewAngleValue);
            float xOffsetPixels = xOffsetRotated * Config.Map3Graphics.MapViewScaleValue;
            float zOffsetPixels = zOffsetRotated * Config.Map3Graphics.MapViewScaleValue;
            float centerX = Config.Map3Gui.GLControl.Width / 2 + xOffsetPixels;
            float centerZ = Config.Map3Gui.GLControl.Height / 2 + zOffsetPixels;
            return (centerX, centerZ);
        }

        /** Takes in in-game angle, outputs control angle. */
        public static float ConvertAngleForControl(double angle)
        {
            angle += 32768 - Config.Map3Graphics.MapViewAngleValue;
            if (double.IsNaN(angle)) angle = 0;
            return (float)MoreMath.AngleUnitsToDegrees(angle);
        }

        public static SizeF ScaleImageSize(Size imageSize, float desiredSize)
        {
            float scale = Math.Max(imageSize.Height / desiredSize, imageSize.Width / desiredSize);
            return new SizeF(imageSize.Width / scale, imageSize.Height / scale);
        }

        public static MapLayout GetMapLayout()
        {
            object mapLayoutChoice = Config.Map3Gui.comboBoxMap3OptionsLevel.SelectedItem;
            if (mapLayoutChoice is MapLayout mapLayout)
            {
                return mapLayout;
            }
            else
            {
                return Config.MapAssociations.GetBestMap();
            }
        }

        public static List<(float x, float z)> GetPuCenters()
        {
            int xMin = ((((int)Config.Map3Graphics.MapViewXMin) / 65536) - 1) * 65536;
            int xMax = ((((int)Config.Map3Graphics.MapViewXMax) / 65536) + 1) * 65536;
            int zMin = ((((int)Config.Map3Graphics.MapViewZMin) / 65536) - 1) * 65536;
            int zMax = ((((int)Config.Map3Graphics.MapViewZMax) / 65536) + 1) * 65536;
            List<(float x, float z)> centers = new List<(float x, float z)>();
            for (int x = xMin; x <= xMax; x += 65536)
            {
                for (int z = zMin; z <= zMax; z += 65536)
                {
                    centers.Add((x, z));
                }
            }
            return centers;
        }

        public static List<(float x, float z)> GetPuCoordinates(float relX, float relZ)
        {
            return GetPuCenters().ConvertAll(center => (center.x + relX, center.z + relZ));
        }
    }
}

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
        public static (float x, float z) ConvertCoordsForControl(float x, float z)
        {
            float relX = (float)PuUtilities.GetRelativeCoordinate(x);
            float relZ = (float)PuUtilities.GetRelativeCoordinate(z);

            float xOffsetInGameUnits = relX - Config.Map3Graphics.XMin;
            float xOffsetPixels = xOffsetInGameUnits * Config.Map3Graphics.ConversionScale;
            float xPosPixels = Config.Map3Graphics.MapView.X + xOffsetPixels;

            float zOffsetInGameUnits = relZ - Config.Map3Graphics.ZMin;
            float zOffsetPixels = zOffsetInGameUnits * Config.Map3Graphics.ConversionScale;
            float zPosPixels = Config.Map3Graphics.MapView.Y + zOffsetPixels;

            return (xPosPixels, zPosPixels);
        }

        public static SizeF ScaleImageSize(Size imageSize, float desiredSize)
        {
            float scale = Math.Max(imageSize.Height / desiredSize, imageSize.Width / desiredSize);
            return new SizeF(imageSize.Width / scale, imageSize.Height / scale);
        }
    }
}

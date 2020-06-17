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
using STROOP.Map.Map3D;

namespace STROOP.Map
{
    public class MapCompassObject : MapObject
    {
        public MapCompassObject()
            : base()
        {
        }

        public override void DrawOn2DControl()
        {
            float centerX = 0;
            float centerZ = 0;
            float arrowLineHeight = 200;
            float arrowLineWidth = 20;
            float arrowHeadHeight = 50;
            float arrowHeadWidth = 50;

            List<(float x, float z)> points = new List<(float x, float z)>();
            for (int angle = 0; angle < 65536; angle += 16384)
            {
                double angleUp = angle;
                double angleDown = angle + 32768;
                double angleLeft = angle + 16384;
                double angleRight = angle - 16384;
                double angleUpLeft = angle + 8192;
                double angleUpRight = angle - 8192;
                double angleDownLeft = angle + 24576;
                double angleDownRight = angle - 24576;

                (float x, float z) arrowBaseLeft = ((float, float))MoreMath.AddVectorToPoint(arrowLineWidth / Math.Sqrt(2), angleUpLeft, centerX, centerZ);
                (float x, float z) arrowBaseRight = ((float, float))MoreMath.AddVectorToPoint(arrowLineWidth / Math.Sqrt(2), angleUpRight, centerX, centerZ);
                (float x, float z) arrowHeadInnerCornerLeft = ((float, float))MoreMath.AddVectorToPoint(arrowLineHeight, angleUp, arrowBaseLeft.x, arrowBaseLeft.z);
                (float x, float z) arrowHeadInnerCornerRight = ((float, float))MoreMath.AddVectorToPoint(arrowLineHeight, angleUp, arrowBaseRight.x, arrowBaseRight.z);
                (float x, float z) arrowHeadCornerLeft = ((float, float))MoreMath.AddVectorToPoint((arrowHeadWidth - arrowLineWidth) / 2, angleLeft, arrowHeadInnerCornerLeft.x, arrowHeadInnerCornerLeft.z);
                (float x, float z) arrowHeadCornerRight = ((float, float))MoreMath.AddVectorToPoint((arrowHeadWidth - arrowLineWidth) / 2, angleRight, arrowHeadInnerCornerRight.x, arrowHeadInnerCornerRight.z);
                (float x, float z) arrowHeadPoint = ((float, float))MoreMath.AddVectorToPoint(arrowLineHeight + arrowHeadHeight, angleUp, centerX, centerZ);

                points.AddRange(
                    new List<(float x, float z)>()
                    {
                        arrowHeadInnerCornerRight,
                        arrowHeadCornerRight,
                        arrowHeadPoint,
                        arrowHeadCornerLeft,
                        arrowHeadInnerCornerLeft,
                        arrowBaseLeft,
                    });
            }

            List<(float x, float z)> pointsForControl =
                points.ConvertAll(point => MapUtilities.ConvertCoordsForControl(point.x, point.z));

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw outline
            if (OutlineWidth != 0)
            {
                GL.Color4(OutlineColor.R, OutlineColor.G, OutlineColor.B, (byte)255);
                GL.LineWidth(OutlineWidth);
                GL.Begin(PrimitiveType.LineLoop);
                foreach ((float x, float z) in pointsForControl)
                {
                    GL.Vertex2(x, z);
                }
                GL.End();
            }

            GL.Color4(1, 1, 1, 1.0f);

            /*
            List<List<(float x, float y, float z)>> quadList = new List<List<(float x, float y, float z)>>()
            {
                new List<(float x, float y, float z)>()
                {
                    (1000, 0, 1000),
                    (1000, 0, -1000),
                    (-1000, 0, -1000),
                    (-1000, 0, 1000),
                }
            };
            List<List<(float x, float z)>> quadListForControl =
                quadList.ConvertAll(quad => quad.ConvertAll(
                    vertex => MapUtilities.ConvertCoordsForControl(vertex.x, vertex.z)));

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw quad
            GL.Color4(Color.R, Color.G, Color.B, OpacityByte);
            GL.Begin(PrimitiveType.Quads);
            foreach (List<(float x, float z)> quad in quadListForControl)
            {
                foreach ((float x, float z) in quad)
                {
                    GL.Vertex2(x, z);
                }
            }
            GL.End();

            // Draw outline
            if (OutlineWidth != 0)
            {
                GL.Color4(OutlineColor.R, OutlineColor.G, OutlineColor.B, (byte)255);
                GL.LineWidth(OutlineWidth);
                foreach (List<(float x, float z)> quad in quadListForControl)
                {
                    GL.Begin(PrimitiveType.LineLoop);
                    foreach ((float x, float z) in quad)
                    {
                        GL.Vertex2(x, z);
                    }
                    GL.End();
                }
            }

            GL.Color4(1, 1, 1, 1.0f);
            */
        }

        public override void DrawOn3DControl()
        {
            // do nothing
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Perspective;
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.ArrowImage;
        }

        public override string GetName()
        {
            return "Compass";
        }
    }
}

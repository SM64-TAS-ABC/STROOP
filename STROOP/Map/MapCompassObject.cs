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
            List<CompassArrow> arrows = Enumerable.Range(0, 4).ToList().ConvertAll(index => new CompassArrow(16384 * index));

            List<List<(float x, float z)>> triPoints = new List<List<(float x, float z)>>();
            for (int i = 0; i < arrows.Count; i++)
            {
                CompassArrow arrow1 = arrows[i];
                CompassArrow arrow2 = arrows[(i + 2) % 4];
                triPoints.Add(new List<(float x, float z)>() { arrow1.ArrowHeadPoint, arrow1.ArrowHeadCornerLeft, arrow1.ArrowHeadCornerRight });
                triPoints.Add(new List<(float x, float z)>() { arrow1.ArrowHeadInnerCornerRight, arrow1.ArrowHeadInnerCornerLeft, arrow2.ArrowHeadInnerCornerRight });
            }

            List<(float x, float z)> outlinePoints = arrows.ConvertAll(arrow => arrow.GetOutlinePoints()).SelectMany(points => points).ToList();

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw polygon
            GL.Color4(Color.R, Color.G, Color.B, OpacityByte);
            GL.Begin(PrimitiveType.Triangles);
            foreach (List<(float x, float z)> tri in triPoints)
            {
                foreach ((float x, float z) in tri)
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
                GL.Begin(PrimitiveType.LineLoop);
                foreach ((float x, float z) in outlinePoints)
                {
                    GL.Vertex2(x, z);
                }
                GL.End();
            }

            GL.Color4(1, 1, 1, 1.0f);
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

        public class CompassArrow
        {
            private static float arrowLineHeight = 200;
            private static float arrowLineWidth = 20;
            private static float arrowHeadHeight = 50;
            private static float arrowHeadWidth = 50;
            private static float margin = 10;

            private static float centerX = margin + arrowHeadHeight + arrowLineHeight + arrowLineWidth / 2;
            private static float centerZ = margin + arrowHeadHeight + arrowLineHeight + arrowLineWidth / 2;

            public (float x, float z) ArrowBaseRight;
            public (float x, float z) ArrowHeadInnerCornerRight;
            public (float x, float z) ArrowHeadCornerRight;
            public (float x, float z) ArrowHeadPoint;
            public (float x, float z) ArrowHeadCornerLeft;
            public (float x, float z) ArrowHeadInnerCornerLeft;
            public (float x, float z) ArrowBaseLeft;

            public CompassArrow(int angle)
            {
                double angleUp = angle;
                double angleDown = angle + 32768;
                double angleLeft = angle + 16384;
                double angleRight = angle - 16384;
                double angleUpLeft = angle + 8192;
                double angleUpRight = angle - 8192;
                double angleDownLeft = angle + 24576;
                double angleDownRight = angle - 24576;

                ArrowBaseLeft = ((float, float))MoreMath.AddVectorToPoint(arrowLineWidth / Math.Sqrt(2), angleUpLeft, centerX, centerZ);
                ArrowBaseRight = ((float, float))MoreMath.AddVectorToPoint(arrowLineWidth / Math.Sqrt(2), angleUpRight, centerX, centerZ);
                ArrowHeadInnerCornerLeft = ((float, float))MoreMath.AddVectorToPoint(arrowLineHeight, angleUp, ArrowBaseLeft.x, ArrowBaseLeft.z);
                ArrowHeadInnerCornerRight = ((float, float))MoreMath.AddVectorToPoint(arrowLineHeight, angleUp, ArrowBaseRight.x, ArrowBaseRight.z);
                ArrowHeadCornerLeft = ((float, float))MoreMath.AddVectorToPoint((arrowHeadWidth - arrowLineWidth) / 2, angleLeft, ArrowHeadInnerCornerLeft.x, ArrowHeadInnerCornerLeft.z);
                ArrowHeadCornerRight = ((float, float))MoreMath.AddVectorToPoint((arrowHeadWidth - arrowLineWidth) / 2, angleRight, ArrowHeadInnerCornerRight.x, ArrowHeadInnerCornerRight.z);
                ArrowHeadPoint = ((float, float))MoreMath.AddVectorToPoint(arrowLineHeight + arrowHeadHeight, angleUp, centerX, centerZ);
            }

            public List<(float x, float z)> GetOutlinePoints()
            {
                return new List<(float x, float z)>()
                {
                    ArrowHeadInnerCornerRight,
                    ArrowHeadCornerRight,
                    ArrowHeadPoint,
                    ArrowHeadCornerLeft,
                    ArrowHeadInnerCornerLeft,
                    ArrowBaseLeft,
                };
            }
        }
    }
}

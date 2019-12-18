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
    public class Map3IwerlipseObject : Map3Object
    {
        private readonly static int NUM_POINTS = 256;

        public Map3IwerlipseObject()
            : base()
        {
            Opacity = 0.5;
            Color = Color.Red;
        }

        public override void DrawOnControl()
        {
            float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
            ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
            (double x1, double z1) = MoreMath.AddVectorToPoint(100, marioAngle + 0, marioX, marioZ);
            (double x2, double z2) = MoreMath.AddVectorToPoint(100, marioAngle + 16384, marioX, marioZ);
            (double x3, double z3) = MoreMath.AddVectorToPoint(100, marioAngle + 32768, marioX, marioZ);
            (double x4, double z4) = MoreMath.AddVectorToPoint(100, marioAngle + 49152, marioX, marioZ);
            DrawCircles((float)x1, (float)z1, 20);
            DrawCircles((float)x2, (float)z2, 20);
            DrawCircles((float)x3, (float)z3, 20);
            DrawCircles((float)x4, (float)z4, 20);
        }

        private void DrawCircles(float centerX, float centerZ, float radius)
        {
            (float controlCenterX, float controlCenterZ) = Map3Utilities.ConvertCoordsForControl(centerX, centerZ);
            float controlRadius = radius * Config.Map3Graphics.MapViewScaleValue;
            List <(float pointX, float pointZ)> controlPoints = Enumerable.Range(0, NUM_POINTS).ToList()
                .ConvertAll(index => (index / (float)NUM_POINTS) * 65536)
                .ConvertAll(angle => ((float, float))MoreMath.AddVectorToPoint(controlRadius, angle, controlCenterX, controlCenterZ));

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw circle
            GL.Color4(Color.R, Color.G, Color.B, OpacityByte);
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Vertex2(controlCenterX, controlCenterZ);
            foreach ((float x, float z) in controlPoints)
            {
                GL.Vertex2(x, z);
            }
            GL.Vertex2(controlPoints[0].pointX, controlPoints[0].pointZ);
            GL.End();

            // Draw outline
            if (OutlineWidth != 0)
            {
                GL.Color4(OutlineColor.R, OutlineColor.G, OutlineColor.B, (byte)255);
                GL.LineWidth(OutlineWidth);
                GL.Begin(PrimitiveType.LineLoop);
                foreach ((float x, float z) in controlPoints)
                {
                    GL.Vertex2(x, z);
                }
                GL.End();
            }

            GL.Color4(1, 1, 1, 1.0f);
        }

        public override string GetName()
        {
            return "Iwerlipses";
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.PathImage;
        }
    }
}

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

namespace STROOP.Map3
{
    public abstract class MapCircleObject : MapObject
    {
        protected readonly static int NUM_POINTS_2D = 256;
        protected readonly static int NUM_POINTS_3D = 64;

        public MapCircleObject()
            : base()
        {
            Opacity = 0.5;
            Color = Color.Red;
        }

        public override void DrawOn2DControl()
        {
            (float centerX, float centerZ, float radius) = Get2DDimensions();
            (float controlCenterX, float controlCenterZ) = MapUtilities.ConvertCoordsForControl(centerX, centerZ);
            float controlRadius = radius * Config.Map3Graphics.MapViewScaleValue;
            List <(float pointX, float pointZ)> controlPoints = Enumerable.Range(0, NUM_POINTS_2D).ToList()
                .ConvertAll(index => (index / (float)NUM_POINTS_2D) * 65536)
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

        protected abstract (float centerX, float centerZ, float radius) Get2DDimensions();

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Perspective;
        }
    }
}

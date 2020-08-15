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
    public class MapSectorObject : MapObject
    {
        protected readonly static int NUM_POINTS_2D = 257;

        private readonly PositionAngle _posAngle;

        public MapSectorObject(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;

            Size = 1000;
            Opacity = 0.5;
            Color = Color.Yellow;
        }

        public override void DrawOn2DControl()
        {
            List<(float centerX, float centerZ, float radius, float angle, float angleRadius)> dimenstionList = GetDimensions();

            foreach ((float centerX, float centerZ, float radius, float angle, float angleRadius) in dimenstionList)
            {
                (float controlCenterX, float controlCenterZ) = MapUtilities.ConvertCoordsForControl(centerX, centerZ);
                float controlRadius = radius * Config.MapGraphics.MapViewScaleValue;
                List <(float pointX, float pointZ)> outerPoints = Enumerable.Range(0, NUM_POINTS_2D).ToList()
                    .ConvertAll(index => (index - NUM_POINTS_2D / 2) / (float)(NUM_POINTS_2D / 2))
                    .ConvertAll(proportion => angle + proportion * angleRadius)
                    .ConvertAll(ang => ((float, float))MoreMath.AddVectorToPoint(controlRadius, ang, controlCenterX, controlCenterZ));

                GL.BindTexture(TextureTarget.Texture2D, -1);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadIdentity();

                // Draw circle
                GL.Color4(Color.R, Color.G, Color.B, OpacityByte);
                GL.Begin(PrimitiveType.TriangleFan);
                GL.Vertex2(controlCenterX, controlCenterZ);
                foreach ((float x, float z) in outerPoints)
                {
                    GL.Vertex2(x, z);
                }
                GL.End();

                // Draw outline
                if (OutlineWidth != 0)
                {
                    GL.Color4(OutlineColor.R, OutlineColor.G, OutlineColor.B, (byte)255);
                    GL.LineWidth(OutlineWidth);
                    GL.Begin(PrimitiveType.LineLoop);
                    GL.Vertex2(controlCenterX, controlCenterZ);
                    foreach ((float x, float z) in outerPoints)
                    {
                        GL.Vertex2(x, z);
                    }
                    GL.Vertex2(controlCenterX, controlCenterZ);
                    GL.End();
                }
            }

            GL.Color4(1, 1, 1, 1.0f);
        }

        public override void DrawOn3DControl()
        {
            // do nothing
        }

        protected List<(float centerX, float centerZ, float radius, float angle, float angleRadius)> GetDimensions()
        {
            (double x, double y, double z, double angle) = _posAngle.GetValues();
            return new List<(float centerX, float centerZ, float radius, float angle, float angleRadius)>()
            {
                ((float)x, (float)z, Size, (float)angle, 1024f)
            };
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
            return "Sector for " + _posAngle.GetMapName();
        }
    }
}

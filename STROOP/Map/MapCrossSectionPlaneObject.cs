using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;
using OpenTK.Graphics;
using System.Drawing;
using STROOP.Map.Map3D;

namespace STROOP.Map
{
    public class MapCrossSectionPlaneObject : MapObject
    {
        public MapCrossSectionPlaneObject()
            : base()
        {
            Color = Color.Red;
            Opacity = 0.5;
            OutlineWidth = 3;
        }

        public override void DrawOn2DControlTopDownView()
        {
            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw outline
            if (OutlineWidth != 0)
            {
                GL.Color4(Color.R, Color.G, Color.B, (byte)255);
                GL.LineWidth(OutlineWidth);
                GL.Begin(PrimitiveType.Lines);
                float width = Config.MapGui.GLControlMap2D.Width;
                float height = Config.MapGui.GLControlMap2D.Height;
                GL.Vertex2(0, height / 2);
                GL.Vertex2(width, height / 2);
                GL.End();
            }

            GL.Color4(1, 1, 1, 1.0f);
        }

        public override void DrawOn2DControlOrthographicView()
        {
            // do nothing
        }

        public override void DrawOn3DControl()
        {
            float width = Config.MapGui.GLControlMap2D.Width;
            float height = Config.MapGui.GLControlMap2D.Height;
            (float x1, float z1) = MapUtilities.ConvertCoordsForInGame(0, height / 2);
            (float x2, float z2) = MapUtilities.ConvertCoordsForInGame(width, height / 2);

            Map3DVertex[] vertexes = new Map3DVertex[]
            {
                new Map3DVertex(new Vector3(x1, -16384, z1), Color4),
                new Map3DVertex(new Vector3(x2, -16384, z2), Color4),
                new Map3DVertex(new Vector3(x2, 16384, z2), Color4),
                new Map3DVertex(new Vector3(x1, 16384, z1), Color4),
            };

            Matrix4 viewMatrix = GetModelMatrix() * Config.Map3DCamera.Matrix;
            GL.UniformMatrix4(Config.Map3DGraphics.GLUniformView, false, ref viewMatrix);

            // draw plane
            {
                int buffer = GL.GenBuffer();
                GL.BindTexture(TextureTarget.Texture2D, MapUtilities.WhiteTexture);
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexes.Length * Map3DVertex.Size), vertexes, BufferUsageHint.DynamicDraw);
                Config.Map3DGraphics.BindVertices();
                GL.DrawArrays(PrimitiveType.Polygon, 0, vertexes.Length);
                GL.DeleteBuffer(buffer);
            }

            // draw outline
            if (OutlineWidth != 0)
            {
                int buffer = GL.GenBuffer();
                GL.BindTexture(TextureTarget.Texture2D, MapUtilities.WhiteTexture);
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexes.Length * Map3DVertex.Size), vertexes, BufferUsageHint.DynamicDraw);
                GL.LineWidth(OutlineWidth);
                Config.Map3DGraphics.BindVertices();
                GL.DrawArrays(PrimitiveType.LineLoop, 0, vertexes.Length);
                GL.DeleteBuffer(buffer);
            }
        }

        public override string GetName()
        {
            return "Cross Section Plane";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.ArrowImage;
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Perspective;
        }
    }
}

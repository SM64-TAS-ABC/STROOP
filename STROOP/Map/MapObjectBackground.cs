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
using STROOP.Map.Map3D;

namespace STROOP.Map
{
    public abstract class MapObjectBackground : MapObjectIconRectangle
    {
        public MapObjectBackground()
            : base()
        {
            InternalRotates = false;
        }

        protected override List<(PointF loc, SizeF size)> GetDimensions()
        {
            float xCenter = Config.MapGui.CurrentControl.Width / 2;
            float yCenter = Config.MapGui.CurrentControl.Height / 2;
            float length = Math.Max(Config.MapGui.CurrentControl.Width, Config.MapGui.CurrentControl.Height) + 2;
            (PointF loc, SizeF size) dimension = (new PointF(xCenter, yCenter), new SizeF(length, length));
            return new List<(PointF loc, SizeF size)>() { dimension };
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Background;
        }

        private Map3DVertex[] GetVertices()
        {
            float width = Config.MapGui.GLControlMap3D.Width;
            float height = Config.MapGui.GLControlMap3D.Height;
            bool widthIsMin = width <= height;
            float ratio = Math.Max(width, height) / Math.Min(width, height);
            float widthMultiplier = widthIsMin ? ratio : 1;
            float heightMultiplier = widthIsMin ? 1 : ratio;

            float leftBound = -1 * widthMultiplier;
            float rightBound = 1 * widthMultiplier;
            float upperBound = 1 * heightMultiplier;
            float lowerBound = -1 * heightMultiplier;

            return new Map3DVertex[]
            {
                new Map3DVertex(new Vector3(leftBound, lowerBound, 0), Color4, new Vector2(0, 1)),
                new Map3DVertex(new Vector3(rightBound, lowerBound, 0), Color4, new Vector2(1, 1)),
                new Map3DVertex(new Vector3(leftBound, upperBound, 0), Color4, new Vector2(0, 0)),
                new Map3DVertex(new Vector3(rightBound, upperBound, 0), Color4, new Vector2(1, 0)),
                new Map3DVertex(new Vector3(leftBound, upperBound, 0), Color4, new Vector2(0, 0)),
                new Map3DVertex(new Vector3(rightBound, lowerBound, 0), Color4, new Vector2(1, 1)),
            };
        }

        public override void DrawOn3DControl()
        {
            Map3DVertex[] vertices = GetVertices();

            Matrix4 viewMatrix = GetModelMatrix();
            GL.UniformMatrix4(Config.Map3DGraphics.GLUniformView, false, ref viewMatrix);

            int buffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Map3DVertex.Size),
                vertices, BufferUsageHint.StaticDraw);
            GL.BindTexture(TextureTarget.Texture2D, TextureId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
            Config.Map3DGraphics.BindVertices();
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
            GL.DeleteBuffer(buffer);
        }
    }
}

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
using STROOP.Map3.Map.Graphics;

namespace STROOP.Map3
{
    public abstract class Map3BackgroundObject : Map3IconRectangleObject
    {
        public Map3BackgroundObject()
            : base()
        {
            InternalRotates = false;
        }

        protected override List<(PointF loc, SizeF size)> GetDimensions()
        {
            float xCenter = Config.Map3Gui.GLControl2D.Width / 2;
            float yCenter = Config.Map3Gui.GLControl2D.Height / 2;
            float length = Math.Max(Config.Map3Gui.GLControl2D.Width, Config.Map3Gui.GLControl2D.Height);
            (PointF loc, SizeF size) dimension = (new PointF(xCenter, yCenter), new SizeF(length, length));
            return new List<(PointF loc, SizeF size)>() { dimension };
        }

        public override Map3DrawType GetDrawType()
        {
            return Map3DrawType.Background;
        }

        private Map4Vertex[] GetVertices()
        {
            float width = Config.Map3Gui.GLControl3D.Width;
            float height = Config.Map3Gui.GLControl3D.Height;
            bool widthIsMin = width <= height;
            float ratio = Math.Max(width, height) / Math.Min(width, height);
            float widthMultiplier = widthIsMin ? ratio : 1;
            float heightMultiplier = widthIsMin ? 1 : ratio;

            float leftBound = -1 * widthMultiplier;
            float rightBound = 1 * widthMultiplier;
            float upperBound = 1 * heightMultiplier;
            float lowerBound = -1 * heightMultiplier;

            return new Map4Vertex[]
            {
                new Map4Vertex(new Vector3(leftBound, lowerBound, 0), Color4, new Vector2(0, 1)),
                new Map4Vertex(new Vector3(rightBound, lowerBound, 0), Color4, new Vector2(1, 1)),
                new Map4Vertex(new Vector3(leftBound, upperBound, 0), Color4, new Vector2(0, 0)),
                new Map4Vertex(new Vector3(rightBound, upperBound, 0), Color4, new Vector2(1, 0)),
                new Map4Vertex(new Vector3(leftBound, upperBound, 0), Color4, new Vector2(0, 0)),
                new Map4Vertex(new Vector3(rightBound, lowerBound, 0), Color4, new Vector2(1, 1)),
            };
        }

        public override void DrawOn3DControl()
        {
            Map4Vertex[] vertices = GetVertices();

            int buffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Map4Vertex.Size),
                vertices, BufferUsageHint.StaticDraw);
            GL.BindTexture(TextureTarget.Texture2D, TextureId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
            Config.Map4Graphics.BindVertices();
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
            GL.DeleteBuffer(buffer);
        }
    }
}

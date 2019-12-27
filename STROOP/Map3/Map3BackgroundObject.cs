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

        static readonly Map4Vertex[] _vertices = new Map4Vertex[]
        {
            new Map4Vertex(new Vector3(-1, -1, 0), new Vector2(0, 1)),
            new Map4Vertex(new Vector3(1, -1, 0),  new Vector2(1, 1)),
            new Map4Vertex(new Vector3(-1, 1, 0),  new Vector2(0, 0)),
            new Map4Vertex(new Vector3(1, 1, 0),   new Vector2(1, 0)),
            new Map4Vertex(new Vector3(-1, 1, 0),  new Vector2(0, 0)),
            new Map4Vertex(new Vector3(1, -1, 0),  new Vector2(1, 1)),
        };

        public override void DrawOn3DControl()
        {

            int buffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(_vertices.Length * Map4Vertex.Size),
                _vertices, BufferUsageHint.StaticDraw);
            GL.BindTexture(TextureTarget.Texture2D, TextureId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
            Config.Map4Graphics.BindVertices();
            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Length);
            GL.DeleteBuffer(buffer);
        }
    }
}

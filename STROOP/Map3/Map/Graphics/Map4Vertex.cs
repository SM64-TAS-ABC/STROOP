using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Map3.Map.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Map4Vertex
    {
        public Vector3 Position;
        public Color4 Color;
        public Vector2 TexCoord;

        public static int Size { get => Marshal.SizeOf(typeof(Map4Vertex)); }
        public static int IndexPosition { get => 0; }
        public static int IndexColor { get => IndexPosition + Marshal.SizeOf(typeof(Vector3)); }
        public static int IndexTexCoord { get => IndexColor + Marshal.SizeOf(typeof(Color4)); }

        public Map4Vertex(Vector3 position, Color4 color, Vector2 texCoord)
        {
            Position = position;
            Color = color;
            TexCoord = texCoord;
        }

        public Map4Vertex(Vector3 position, Color4 color)
            : this(position, color, Vector2.Zero) { }

        public Map4Vertex(Vector3 position, Vector2 texCoord)
            : this(position, Color4.White, texCoord) { }
    }
}

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using STROOP.Extensions;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Map3.Map.Graphics
{
    public class Map4GraphicsUtilities
    {
        public static int WhiteTexture { get; }
        private static readonly byte[] _whiteTexData = new byte[] { 0xFF };

        static Map4GraphicsUtilities()
        {
            WhiteTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, WhiteTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, 1, 1, 0, OpenTK.Graphics.OpenGL.PixelFormat.Luminance, PixelType.UnsignedByte, _whiteTexData);
        }

        public static Vector3 GetPositionOnViewFromCoordinate(Vector3 pos)
        {
            Vector4 vec = Vector4.Transform(new Vector4(pos, 1), Config.Map4Camera.Matrix);
            vec.X /= vec.W;
            vec.Y /= vec.W;
            vec.Z = 0;
            return vec.Xyz;
        }
    }
}

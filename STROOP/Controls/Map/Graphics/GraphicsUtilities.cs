using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using STROOP.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Controls.Map.Graphics
{
    public class GraphicsUtilities
    {
        MapGraphics _graphics;

        public int WhiteTexture { get; }
        private readonly byte[] _whiteTexData = new byte[] { 0xFF };

        public GraphicsUtilities(MapGraphics graphics)
        {
            _graphics = graphics;

            WhiteTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, WhiteTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, 1, 1, 0, OpenTK.Graphics.OpenGL.PixelFormat.Luminance, PixelType.UnsignedByte, _whiteTexData);
        }

        public Vector3 GetPositionOnViewFromCoordinate(Vector3 pos)
        {
            return Vector4.Transform(new Vector4(pos, 1), _graphics.Camera.Matrix).Xyz;
        }

        public int LoadTexture(Bitmap bmp)
        {
            // Create texture and id
            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            // Set Bi-Linear Texture Filtering
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            // Get data from bitmap image
            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.GenerateMipmap, 1);

            // Store bitmap data as OpenGl texture
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, bmp.Width, bmp.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

            bmp.UnlockBits(bmp_data);

            // Generate mipmaps for texture filtering

            return id;
        }
    }
}

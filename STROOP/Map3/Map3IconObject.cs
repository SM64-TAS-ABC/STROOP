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

namespace STROOP.Map3
{
    public abstract class Map3IconObject : Map3Object
    {
        private readonly Func<Image> ImageFunction;
        protected Image Image;
        protected int TextureId;

        public Map3IconObject(Map3Graphics graphics, Func<Image> imageFunction)
            : base(graphics)
        {
            ImageFunction = imageFunction;
            Image = null;
            TextureId = -1;
        }

        protected void UpdateImage()
        {
            Image image = ImageFunction();
            if (image != Image)
            {
                Image = image;
                GL.DeleteTexture(TextureId);
                TextureId = Graphics.LoadTexture(image as Bitmap);
            }
        }

        public override void Dispose()
        {
            GL.DeleteTexture(TextureId);
        }
    }
}

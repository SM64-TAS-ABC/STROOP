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
using System.Drawing.Imaging;

namespace STROOP.Map3
{
    public abstract class Map3IconObject : Map3Object
    {
        protected Image Image;
        protected int TextureId;

        public Map3IconObject()
            : base()
        {
            Image = null;
            TextureId = -1;
        }

        protected void UpdateImage()
        {
            Image image = GetImage() ?? Config.ObjectAssociations.EmptyImage;
            if (image != Image)
            {
                Image = image;
                GL.DeleteTexture(TextureId);
                TextureId = Map3Utilities.LoadTexture(image as Bitmap);
            }
        }
    }
}

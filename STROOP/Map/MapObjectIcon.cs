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
    public abstract class MapObjectIcon : MapObject
    {
        protected Image Image;
        protected int TextureId;

        public MapObjectIcon()
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
                TextureId = MapUtilities.LoadTexture(image as Bitmap);
            }
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Overlay;
        }

        public override void Update()
        {
            UpdateImage();
        }
    }
}

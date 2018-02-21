using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK;

namespace STROOP.Controls.Map.Graphics.Items
{
    class MapGraphicsIconItem : IMapGraphicsItem
    {
        int _imageID;
        private Bitmap _image;

        public Vector3 Position { get; set; }

        public bool Visible { get; set; }

        public bool DrawOnTopDown { get; set; }

        public bool DrawOnPerspective { get; set; }

        public float TopDownPriority { get; set; }

        public MapGraphicsIconItem(Bitmap image)
        {
            _image = (Bitmap) image.Clone();
        }

        public void Load(MapGraphics graphics)
        {
            _imageID = graphics.LoadTexture(_image);
        }

        public void Draw(MapGraphics graphics)
        {
            
        }

        public void Unload()
        {
            GL.DeleteTexture(_imageID);
        }
    }
}

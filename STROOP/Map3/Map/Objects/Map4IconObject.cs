using STROOP.Controls.Map.Graphics.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace STROOP.Controls.Map.Objects
{
    public abstract class Map4IconObject : Map4Object
    {
        public Map4IconObject(string name, Bitmap bitmapImage, Color? backColor, bool rotates)
            : base(name, bitmapImage, backColor, rotates, null)
        {
        }

        public override IEnumerable<Map4GraphicsItem> GraphicsItems => new List<Map4GraphicsItem>() { _iconGraphics };
        protected abstract Map4GraphicsIconItem _iconGraphics { get; set; }

        public override float Opacity { get => _iconGraphics.Opacity; set => _iconGraphics.Opacity = value; }
        public override float Size { get => _iconGraphics.Size; set => _iconGraphics.Size = value; }
        public override int DisplayLayer { get => _iconGraphics.DisplayLayer; set => _iconGraphics.DisplayLayer = value; }
    }
}

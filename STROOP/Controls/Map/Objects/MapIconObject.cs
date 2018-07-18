using STROOP.Controls.Map.Graphics.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace STROOP.Controls.Map.Objects
{
    public abstract class MapIconObject : MapObject
    {
        public MapIconObject(string name, Bitmap bitmapImage, Color? backColor, bool rotates)
            : base(name, bitmapImage, backColor, rotates, null)
        {
        }

        public override IEnumerable<MapGraphicsItem> GraphicsItems => new List<MapGraphicsItem>() { _iconGraphics };
        protected abstract MapGraphicsIconItem _iconGraphics { get; set; }

        public override float Opacity { get => _iconGraphics.Opacity; set => _iconGraphics.Opacity = value; }
        public override float Size { get => _iconGraphics.Size; set => _iconGraphics.Size = value; }
        public override int DisplayLayer { get => _iconGraphics.DisplayLayer; set => DisplayLayer = value; }
    }
}

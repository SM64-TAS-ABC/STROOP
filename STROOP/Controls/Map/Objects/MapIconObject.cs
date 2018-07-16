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
        public MapIconObject(string name) : base(name)
        {
        }

        public override Bitmap BitmapImage
        {
            get => GraphicsItems.FirstOrDefault()?.BitmapImage;
        }

        public override IEnumerable<MapGraphicsItem> GraphicsItems => new List<MapGraphicsItem>() { _iconGraphics };
        protected abstract MapGraphicsIconItem _iconGraphics { get; set; }

        public override float Opacity { get => _iconGraphics.Opacity; set => _iconGraphics.Opacity = value; }
        public override float Size { get => _iconGraphics.Size; set => _iconGraphics.Size = value; }
        public override bool Rotates { get; set; } = true;
        public override int DisplayLayer { get => _iconGraphics.DisplayLayer; set => DisplayLayer = value; }
    }
}

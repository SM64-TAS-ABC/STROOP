using STROOP.Controls.Map.Graphics.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Controls.Map.Objects
{
    public abstract class MapIconObject : MapObject
    {
        public override IEnumerable<MapGraphicsItem> GraphicsItems => new List<MapGraphicsItem>() { _iconGraphics };
        protected abstract MapGraphicsIconItem _iconGraphics { get; set; }

        public float Size { get => _iconGraphics.Size; set => _iconGraphics.Size = value; }
        public int DisplayLayer { get => _iconGraphics.DisplayLayer; set => DisplayLayer = value; }
        public float Opacity { get => _iconGraphics.Opacity; set => _iconGraphics.Opacity = value; }
        public bool Rotates { get; set; } = true;
    }
}

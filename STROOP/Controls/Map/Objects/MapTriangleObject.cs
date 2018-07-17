using OpenTK;
using OpenTK.Graphics;
using STROOP.Controls.Map.Graphics;
using STROOP.Controls.Map.Graphics.Items;
using STROOP.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Controls.Map.Objects
{
    abstract class MapTriangleObject : MapObject
    {
        private MapGraphicsTrianglesItem _triGraphics = new MapGraphicsTrianglesItem();

        public override float Opacity { get; set; } = 0.5f;
        public Color4 Color { get; set; }

        protected abstract TriangleDataModel _triangleData { get; }

        public override IEnumerable<MapGraphicsItem> GraphicsItems => new MapGraphicsItem[] { _triGraphics };

        public MapTriangleObject(string name, Bitmap bitmapImage) : base(name, bitmapImage, null, false)
        {
        }

        public override void Update()
        {
            TriangleDataModel tri = _triangleData;
            Shown = tri != null;
            if (tri == null)
                return;

            Color4 color = Color;
            color.A = Opacity;

            _triGraphics.SetTriangles(new Vertex[]
            {
                new Vertex(new Vector3(tri.X1, tri.Y1, tri.Z1), Color),
                new Vertex(new Vector3(tri.X2, tri.Y2, tri.Z2), Color),
                new Vertex(new Vector3(tri.X3, tri.Y3, tri.Z3), Color),
            });
        }
    }
}

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

        protected abstract TriangleDataModel _triangleData { get; }

        public override IEnumerable<MapGraphicsItem> GraphicsItems => new MapGraphicsItem[] { _triGraphics };

        public MapTriangleObject(string name, Bitmap bitmapImage, Color color) : base(name, bitmapImage, null, false, color)
        {
        }

        public override void Update()
        {
            TriangleDataModel tri = _triangleData;
            Displayed = tri != null;
            if (tri == null)
                return;

            Color4 color = new Color4(
                MyColor.Value.R / 255f,
                MyColor.Value.G / 255f,
                MyColor.Value.B / 255f,
                Opacity);

            _triGraphics.SetTriangles(new Vertex[]
            {
                new Vertex(new Vector3(tri.X1, tri.Y1, tri.Z1), color),
                new Vertex(new Vector3(tri.X2, tri.Y2, tri.Z2), color),
                new Vertex(new Vector3(tri.X3, tri.Y3, tri.Z3), color),
            });
        }
    }
}

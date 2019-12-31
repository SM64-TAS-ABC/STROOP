using OpenTK;
using OpenTK.Graphics;
using STROOP.Map3.Map.Graphics;
using STROOP.Map3.Map.Graphics.Items;
using STROOP.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Map3.Map.Objects
{
    abstract class Map4TriangleObject : Map4Object
    {
        private Map4GraphicsTrianglesItem _triGraphics = new Map4GraphicsTrianglesItem();

        public override float Opacity { get; set; } = 0.5f;

        protected abstract TriangleDataModel _triangleData { get; }

        public override IEnumerable<Map4GraphicsItem> GraphicsItems => new Map4GraphicsItem[] { _triGraphics };

        public override float DefaultOpacity { get; } = 0.5f;

        public Map4TriangleObject(string name, Bitmap bitmapImage, Color color) : base(name, bitmapImage, null, false, color)
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

            _triGraphics.SetTriangles(new Map4Vertex[]
            {
                new Map4Vertex(new Vector3(tri.X1, tri.Y1, tri.Z1), color),
                new Map4Vertex(new Vector3(tri.X2, tri.Y2, tri.Z2), color),
                new Map4Vertex(new Vector3(tri.X3, tri.Y3, tri.Z3), color),
            });
        }
    }
}

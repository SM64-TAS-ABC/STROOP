using OpenTK;
using OpenTK.Graphics;
using STROOP.Models;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STROOP.Map
{
    public class MapObjectHoverData
    {
        public static long HoverStartTime = 0;

        public readonly MapObject MapObject;
        public readonly TriangleDataModel Tri;
        public readonly float? MidUnitX;
        public readonly float? MidUnitZ;

        public MapObjectHoverData(
            MapObject mapObject,
            TriangleDataModel tri = null,
            float? midUnitX = null,
            float? midUnitZ = null)
        {
            MapObject = mapObject;
            Tri = tri;
            MidUnitX = midUnitX;
            MidUnitZ = midUnitZ;
        }

        public List<ToolStripItem> GetContextMenuStripItems()
        {
            return MapObject.GetHoverContextMenuStripItems(this);
        }

        public override string ToString()
        {
            List<object> parts = new List<object>();
            parts.Add(MapObject);
            if (Tri != null) parts.Add(HexUtilities.FormatValue(Tri.Address));
            if (MidUnitX.HasValue) parts.Add(MidUnitX.Value);
            if (MidUnitZ.HasValue) parts.Add(MidUnitZ.Value);
            return string.Join(" ", parts);
        }

        public override bool Equals(object obj)
        {
            if (obj is MapObjectHoverData other)
            {
                return MapObject == other.MapObject &&
                    Tri?.Address == other.Tri?.Address &&
                    MidUnitX == other.MidUnitX &&
                    MidUnitZ == other.MidUnitZ;
            }
            return false;
        }
    }
}

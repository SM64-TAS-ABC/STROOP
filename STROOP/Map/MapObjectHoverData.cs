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

namespace STROOP.Map
{
    public class MapObjectHoverData
    {
        public static long HoverStartTime = 0;

        public readonly MapObject MapObject;
        public readonly TriangleDataModel Tri;

        public MapObjectHoverData(MapObject mapObject, TriangleDataModel tri = null)
        {
            MapObject = mapObject;
            Tri = tri;
        }

        public override string ToString()
        {
            List<object> parts = new List<object>();
            parts.Add(MapObject);
            if (Tri != null) parts.Add(HexUtilities.FormatValue(Tri.Address));
            return string.Join(",", parts);
        }

        public override bool Equals(object obj)
        {
            if (obj is MapObjectHoverData other)
            {
                return MapObject == other.MapObject &&
                    Tri?.Address == other.Tri?.Address;
            }
            return false;
        }
    }
}

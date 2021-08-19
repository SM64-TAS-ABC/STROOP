using OpenTK;
using OpenTK.Graphics;
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

        public MapObjectHoverData(MapObject mapObject)
        {
            MapObject = mapObject;
        }

        public override string ToString()
        {
            return MapObject.ToString();
        }
    }
}

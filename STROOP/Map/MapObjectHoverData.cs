﻿using OpenTK;
using OpenTK.Graphics;
using STROOP.Models;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        public static bool ContextMenuStripIsOpen = false;
        public static Point ContextMenuStripPoint = new Point();

        public readonly MapObject MapObject;
        public readonly uint? ObjAddress;
        public readonly TriangleDataModel Tri;
        public readonly float? MidUnitX;
        public readonly float? MidUnitZ;
        public readonly int? Index;
        public readonly int? Index2;

        public MapObjectHoverData(
            MapObject mapObject,
            uint? objAddress = null,
            TriangleDataModel tri = null,
            float? midUnitX = null,
            float? midUnitZ = null,
            int? index = null,
            int? index2 = null)
        {
            MapObject = mapObject;
            ObjAddress = objAddress;
            Tri = tri;
            MidUnitX = midUnitX;
            MidUnitZ = midUnitZ;
            Index = index;
            Index2 = index2;
        }

        public static Point GetCurrentPoint()
        {
            return ContextMenuStripIsOpen ? ContextMenuStripPoint : Cursor.Position;
        }

        public List<ToolStripItem> GetContextMenuStripItems()
        {
            return MapObject.GetHoverContextMenuStripItems(this);
        }

        public override string ToString()
        {
            List<object> parts = new List<object>();
            parts.Add(MapObject);
            if (ObjAddress != null) parts.Add(HexUtilities.FormatValue(ObjAddress));
            if (Tri != null) parts.Add(HexUtilities.FormatValue(Tri.Address));
            if (MidUnitX.HasValue) parts.Add(MidUnitX.Value);
            if (MidUnitZ.HasValue) parts.Add(MidUnitZ.Value);
            if (Index.HasValue) parts.Add(Index);
            if (Index2.HasValue) parts.Add(Index2);
            return string.Join(" ", parts);
        }

        public override bool Equals(object obj)
        {
            if (obj is MapObjectHoverData other)
            {
                return MapObject == other.MapObject &&
                    ObjAddress == other.ObjAddress &&
                    Tri?.Address == other.Tri?.Address &&
                    MidUnitX == other.MidUnitX &&
                    MidUnitZ == other.MidUnitZ &&
                    Index == other.Index &&
                    Index2 == other.Index2;
            }
            return false;
        }
    }
}

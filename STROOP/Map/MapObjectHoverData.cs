﻿using OpenTK;
using OpenTK.Graphics;
using STROOP.Models;
using STROOP.Structs.Configurations;
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
        public static uint LastTriangleAddress = 0;

        public readonly MapObject MapObject;
        public readonly double X;
        public readonly double Y;
        public readonly double Z;
        public readonly uint? ObjAddress;
        public readonly TriangleDataModel Tri;
        public readonly bool IsTriUnit;
        public readonly int? Index;
        public readonly int? Index2;
        public readonly string Info;

        public MapObjectHoverData(
            MapObject mapObject,
            double x,
            double y,
            double z,
            uint? objAddress = null,
            TriangleDataModel tri = null,
            bool isTriUnit = false,
            int? index = null,
            int? index2 = null,
            string info = null)
        {
            MapObject = mapObject;
            X = x;
            Y = y;
            Z = z;
            ObjAddress = objAddress;
            Tri = tri;
            IsTriUnit = isTriUnit;
            Index = index;
            Index2 = index2;
            Info = info;
        }

        public static Point? GetPositionMaybe()
        {
            Point cursorPos = ContextMenuStripIsOpen ? ContextMenuStripPoint : Cursor.Position;
            Point controlPos = Config.MapGui.CurrentControl.PointToClient(cursorPos);
            if (controlPos.X < 0 || controlPos.X >= Config.MapGui.GLControlMap2D.Width ||
                controlPos.Y < 0 || controlPos.Y >= Config.MapGui.GLControlMap2D.Height)
            {
                return null;
            }
            return controlPos;
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
            if (IsTriUnit) parts.Add("Unit");
            if (Index.HasValue) parts.Add(Index);
            if (Index2.HasValue) parts.Add(Index2);
            if (Info != null)
            {
                parts.Add(Info);
            }
            else if (Config.MapGui.checkBoxMapOptionsEnableOrthographicView.Checked)
            {
                parts.Add(string.Format("({0},{1},{2})", X, Y, Z));
            }
            else
            {
                parts.Add(string.Format("({0},{1})", X, Z));
            }
            return string.Join(" ", parts);
        }

        public override bool Equals(object obj)
        {
            if (obj is MapObjectHoverData other)
            {
                return MapObject == other.MapObject &&
                    X == other.X &&
                    Y == other.Y &&
                    Z == other.Z &&
                    ObjAddress == other.ObjAddress &&
                    Tri?.Address == other.Tri?.Address &&
                    IsTriUnit == other.IsTriUnit &&
                    Index == other.Index &&
                    Index2 == other.Index2 &&
                    Info == other.Info;
            }
            return false;
        }
    }
}
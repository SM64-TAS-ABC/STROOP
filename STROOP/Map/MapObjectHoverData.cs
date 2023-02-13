using OpenTK;
using OpenTK.Graphics;
using STROOP.Models;
using STROOP.Structs;
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
        public readonly MapObjectHoverDataEnum Type;
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
            MapObjectHoverDataEnum type,
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
            Type = type;
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

        public static Point? GetPositionMaybe(bool isForObjectDrag, bool forceCursorPosition)
        {
            Point cursorPos = !isForObjectDrag && ContextMenuStripIsOpen ? ContextMenuStripPoint : Cursor.Position;
            Point controlPos = Config.MapGui.CurrentControl.PointToClient(cursorPos);
            if (!forceCursorPosition)
            {
                if (controlPos.X < 0 || controlPos.X >= Config.MapGui.GLControlMap2D.Width ||
                    controlPos.Y < 0 || controlPos.Y >= Config.MapGui.GLControlMap2D.Height)
                {
                    return null;
                }
            }
            return controlPos;
        }

        public static MapObjectHoverData GetMapObjectHoverDataForCursor(bool isForObjectDrag)
        {
            (float x, float y, float z)? cursorPosition = GetCursorPosition(isForObjectDrag);
            if (!cursorPosition.HasValue) return null;
            return new MapObjectHoverData(null, MapObjectHoverDataEnum.None, cursorPosition.Value.x, cursorPosition.Value.y, cursorPosition.Value.z);
        }

        public static (float x, float y, float z)? GetCursorPosition(bool isForObjectDrag)
        {
            Point? relPosMaybe = GetPositionMaybe(isForObjectDrag, false);
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;
            float inGameX, inGameY, inGameZ;
            if (Config.CurrentMapGraphics.IsOrthographicViewEnabled)
            {
                (inGameX, inGameY, inGameZ) = MapUtilities.ConvertCoordsForInGameOrthographicView(relPos.X, relPos.Y);
            }
            else
            {
                (inGameX, inGameZ) = MapUtilities.ConvertCoordsForInGameTopDownView(relPos.X, relPos.Y);
                inGameY = Config.CurrentMapGraphics.MapViewCenterYValue;
            }
            return (inGameX, inGameY, inGameZ);
        }

        public List<ToolStripItem> GetContextMenuStripItems()
        {
            List<ToolStripItem> items;
            if (MapObject != null)
            {
                items = MapObject.GetHoverContextMenuStripItems(this);
            }
            else
            {
                items = new List<ToolStripItem>();
            }
            (float x, float y, float z)? cursorPosition = GetCursorPosition(false);
            if (cursorPosition.HasValue)
            {
                if (items.Count > 0)
                {
                    items.Add(new ToolStripSeparator());
                }

                ToolStripMenuItem goToClickedPositionItem = new ToolStripMenuItem("Go to Clicked Position");
                goToClickedPositionItem.Click += (sender, e) =>
                {
                    if (!Config.MapGraphics.IsOrthographicViewEnabled)
                    {
                        Config.Stream.SetValue(cursorPosition.Value.x, MarioConfig.StructAddress + MarioConfig.XOffset);
                        Config.Stream.SetValue(cursorPosition.Value.z, MarioConfig.StructAddress + MarioConfig.ZOffset);
                    }
                    else
                    {
                        if (Config.MapGraphics.MapViewYawValue == 0 || Config.MapGraphics.MapViewYawValue == 32768)
                        {
                            Config.Stream.SetValue(cursorPosition.Value.x, MarioConfig.StructAddress + MarioConfig.XOffset);
                            Config.Stream.SetValue(cursorPosition.Value.y, MarioConfig.StructAddress + MarioConfig.YOffset);
                        }
                        else if (Config.MapGraphics.MapViewYawValue == 16384 || Config.MapGraphics.MapViewYawValue == 49152)
                        {
                            Config.Stream.SetValue(cursorPosition.Value.y, MarioConfig.StructAddress + MarioConfig.YOffset);
                            Config.Stream.SetValue(cursorPosition.Value.z, MarioConfig.StructAddress + MarioConfig.ZOffset);
                        }
                        else
                        {
                            Config.Stream.SetValue(cursorPosition.Value.x, MarioConfig.StructAddress + MarioConfig.XOffset);
                            Config.Stream.SetValue(cursorPosition.Value.y, MarioConfig.StructAddress + MarioConfig.YOffset);
                            Config.Stream.SetValue(cursorPosition.Value.z, MarioConfig.StructAddress + MarioConfig.ZOffset);
                        }
                    }
                };
                items.Add(goToClickedPositionItem);

                ToolStripMenuItem copyPositionItem = MapUtilities.CreateCopyItem(
                    cursorPosition.Value.x, cursorPosition.Value.y, cursorPosition.Value.z, "Clicked Position");
                items.Add(copyPositionItem);
            }
            return items;
        }

        public override string ToString()
        {
            List<object> parts = new List<object>();
            if (MapObject != null) parts.Add(MapObject);
            if (ObjAddress != null) parts.Add(HexUtilities.FormatValue(ObjAddress));
            if (Tri != null) parts.Add(HexUtilities.FormatValue(Tri.Address));
            if (IsTriUnit) parts.Add("Unit");
            if (Index.HasValue) parts.Add(Index);
            if (Index2.HasValue) parts.Add(Index2);
            parts.Add(string.Format("({0}, {1}, {2})", HandleRounding(X), HandleRounding(Y), HandleRounding(Z)));
            if (Info != null) parts.Add(Info);
            return string.Join(" ", parts);
        }

        private double HandleRounding(double value)
        {
            if (MapObject is MapObjectFloatGridlines) return value;
            return Math.Round(value, 3);
        }

        public override bool Equals(object obj)
        {
            if (obj is MapObjectHoverData other)
            {
                return MapObject == other.MapObject &&
                    ObjAddress == other.ObjAddress &&
                    Tri?.Address == other.Tri?.Address &&
                    IsTriUnit == other.IsTriUnit &&
                    Index == other.Index &&
                    Index2 == other.Index2 &&
                    Info == other.Info;
            }
            return false;
        }

        public override int GetHashCode()
        {
            List<object> fields = new List<object>()
            {
                MapObject,
                ObjAddress,
                Tri?.Address,
                IsTriUnit,
                Index,
                Index2,
                Info,
            };
            return string.Join(",", fields).GetHashCode();
        }

        public (double x, double y, double z)? GetDragPosition()
        {
            return MapObject?.GetDragPosition();
        }

        public void SetDragPositionTopDownView(double? x = null, double? y = null, double? z = null)
        {
            MapObject?.SetDragPositionTopDownView(x, y, z);
        }

        public void SetDragPositionOrthographicView(double? x = null, double? y = null, double? z = null)
        {
            MapObject?.SetDragPositionOrthographicView(x, y, z);
        }
    }
}

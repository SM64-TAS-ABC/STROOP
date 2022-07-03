using STROOP.Controls;
using STROOP.Forms;
using STROOP.Interfaces;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace STROOP.Map
{
    public class MapTrackerFlowLayoutPanel : NoTearFlowLayoutPanel
    {
        public MapObjectHoverData PreviousHoverData = null;
        public MapObjectHoverData ObjectDragData = null;

        private readonly object _objectLock = new object();

        private MapObject _mapObjMap;
        private MapObject _mapObjBackground;
        private MapObject _mapObjHitboxTris;

        public void Initialize(MapObject mapObjMap, MapObject mapObjBackground, MapObject mapObjHitboxTris)
        {
            _mapObjMap = mapObjMap;
            _mapObjBackground = mapObjBackground;
            _mapObjHitboxTris = mapObjHitboxTris;
        }

        public void MoveUpControl(MapTracker mapTracker, int numMoves)
        {
            lock (_objectLock)
            {
                int index = Controls.IndexOf(mapTracker);
                int newIndex = numMoves == 0 ? 0 : Math.Max(index - numMoves, 0);
                Controls.SetChildIndex(mapTracker, newIndex);
            }
        }

        public void MoveDownControl(MapTracker mapTracker, int numMoves)
        {
            lock (_objectLock)
            {
                int index = Controls.IndexOf(mapTracker);
                int newIndex = numMoves == 0 ? Controls.Count - 1 : Math.Min(index + numMoves, Controls.Count - 1);
                Controls.SetChildIndex(mapTracker, newIndex);
            }
        }

        public MapTracker GetTrackerAtIndex(int index)
        {
            if (index < 0 || index >= Controls.Count) return null;
            return Controls[index] as MapTracker;
        }

        public void RemoveControl(MapTracker mapTracker)
        {
            lock (_objectLock)
            {
                mapTracker.CleanUp();
                Controls.Remove(mapTracker);
            }
        }

        public void AddNewControl(MapTracker mapTracker)
        {
            lock (_objectLock)
            {
                Controls.Add(mapTracker);
            }
        }
        
        public void ClearControls()
        {
            lock (_objectLock)
            {
                while (Controls.Count > 0)
                {
                    RemoveControl(Controls[0] as MapTracker);
                }
            }
        }

        public void UpdateControl()
        {
            (int num2DTopDown, int num2DOrthographic, int num3D) = FormManager.GetMapCounts();
            if (!Config.MapGui.checkBoxMapOptionsDisableHitboxTris.Checked && (num2DOrthographic > 0 || num3D > 0))
            {
                _mapObjHitboxTris.Update();
            }
            if (num2DTopDown > 0)
            {
                _mapObjMap.Update();
            }
            _mapObjBackground.Update();

            lock (_objectLock)
            {
                foreach (MapTracker tracker in Controls)
                {
                    tracker.UpdateControl();
                }
            }
        }

        public void DrawOn2DControl(bool isMainGraphics)
        {
            List<MapObject> listOrderOnTop = new List<MapObject>();
            List<MapObject> listOrderOnBottom = new List<MapObject>();
            List<MapObject> listOrderByY = new List<MapObject>();

            MapType mapType = Config.CurrentMapGraphics.IsOrthographicViewEnabled ? MapType.Map2DOrthographic : MapType.Map2DTopDown;

            lock (_objectLock)
            {
                foreach (MapTracker mapTracker in Controls)
                {
                    List<MapObject> mapObjects = mapTracker.GetMapObjectsToDisplay(mapType);
                    switch (mapTracker.GetOrderType())
                    {
                        case MapTrackerOrderType.OrderOnTop:
                            listOrderOnTop.AddRange(mapObjects);
                            break;
                        case MapTrackerOrderType.OrderOnBottom:
                            listOrderOnBottom.AddRange(mapObjects);
                            break;
                        case MapTrackerOrderType.OrderByY:
                            listOrderByY.AddRange(mapObjects);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            listOrderOnTop.Reverse();
            listOrderOnBottom.Reverse();
            listOrderByY.Reverse();
            listOrderByY = listOrderByY.OrderBy(obj => obj.GetY()).ToList();
            List<MapObject> listCombined = listOrderOnBottom.Concat(listOrderByY).Concat(listOrderOnTop).ToList();

            if (!Config.MapGui.checkBoxMapOptionsDisableHitboxTris.Checked)
            {
                listCombined.Insert(0, _mapObjHitboxTris);
            }
            listCombined.Insert(0, _mapObjMap);
            listCombined.Insert(0, _mapObjBackground);

            MapObjectHoverData hoverData = null;
            if (isMainGraphics &&
                !Config.MapGui.checkBoxMapOptionsEnable3D.Checked &&
                Config.MapGui.checkBoxMapOptionsEnableObjectHover.Checked)
            {
                for (int i = listCombined.Count - 1; i >= 0; i--)
                {
                    MapObject mapObject = listCombined[i];
                    hoverData = mapObject.GetHoverData(false, false);
                    if (hoverData != null) break;
                }
                if (hoverData == null || KeyboardUtilities.IsCtrlHeld())
                {
                    hoverData = MapObjectHoverData.GetMapObjectHoverDataForCursor(false);
                }
                if (Config.CurrentMapGraphics.DraggedObject != null)
                {
                    hoverData = Config.CurrentMapGraphics.DraggedObject;
                }
                if (!Equals(PreviousHoverData, hoverData))
                {
                    PreviousHoverData = hoverData;
                    MapObjectHoverData.HoverStartTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                    if (hoverData != null && hoverData.Tri != null)
                    {
                        MapObjectHoverData.LastTriangleAddress = hoverData.Tri.Address;
                    }
                }
                if (hoverData == null)
                {
                    Config.HideDebugText();
                }
                else
                {
                    Config.SetDebugText(hoverData);
                }
            }

            if (isMainGraphics)
            {
                ObjectDragData = null;
                if (!Config.MapGui.checkBoxMapOptionsEnable3D.Checked &&
                    Config.MapGui.checkBoxMapOptionsEnableObjectDrag.Checked)
                {
                    for (int i = listCombined.Count - 1; i >= 0; i--)
                    {
                        MapObject mapObject = listCombined[i];
                        if (mapObject.GetDragPosition().HasValue)
                        {
                            ObjectDragData = mapObject.GetHoverData(true, false);
                        }
                        if (ObjectDragData != null) break;
                    }
                }
            }

            foreach (MapObject obj in listCombined)
            {
                obj.DrawOn2DControl(hoverData);
            }
        }

        public void DrawOn3DControl(MapDrawType drawType)
        {
            List<MapObject> listOrderOnTop = new List<MapObject>();
            List<MapObject> listOrderOnBottom = new List<MapObject>();
            List<MapObject> listOrderByY = new List<MapObject>();

            lock (_objectLock)
            {
                foreach (MapTracker mapTracker in Controls)
                {
                    List<MapObject> mapObjects = mapTracker.GetMapObjectsToDisplay(MapType.Map3D);
                    switch (mapTracker.GetOrderType())
                    {
                        case MapTrackerOrderType.OrderOnTop:
                            listOrderOnTop.AddRange(mapObjects);
                            break;
                        case MapTrackerOrderType.OrderOnBottom:
                            listOrderOnBottom.AddRange(mapObjects);
                            break;
                        case MapTrackerOrderType.OrderByY:
                            listOrderByY.AddRange(mapObjects);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            listOrderOnTop.Reverse();
            listOrderOnBottom.Reverse();
            listOrderByY.Reverse();
            listOrderByY = listOrderByY.OrderBy(obj => obj.GetY()).ToList();

            List<MapObject> listCombined = listOrderOnBottom.Concat(listOrderByY).Concat(listOrderOnTop).ToList();
            listCombined.Insert(0, _mapObjBackground);
            if (!Config.MapGui.checkBoxMapOptionsDisableHitboxTris.Checked)
            {
                listCombined.Insert(0, _mapObjHitboxTris);
            }

            for (int i = 0; i < Config.Map3DCamera.ZRegionCount; i++)
            {
                Config.Map3DCamera.ZRegionCurrentIndex = i;
                List<MapObject> listDrawType = listCombined.FindAll(obj => obj.GetDrawType() == drawType);
                foreach (MapObject obj in listDrawType)
                {
                    obj.DrawOn3DControl();
                }
            }
        }

        public void SetGlobalIconSize(float size)
        {
            lock (_objectLock)
            {
                foreach (MapTracker tracker in Controls)
                {
                    tracker.SetGlobalIconSize(size);
                }
            }
        }

        public void NotifyMouseEvent(MouseEvent mouseEvent, bool isLeftButton, int mouseX, int mouseY, GLControl control)
        {
            lock (_objectLock)
            {
                foreach (MapTracker mapTracker in Controls)
                {
                    mapTracker.NotifyMouseEvent(mouseEvent, isLeftButton, mouseX, mouseY, control);
                }
            }
        }

        public List<XElement> ToXElements()
        {
            List<XElement> xElements = new List<XElement>();
            lock (_objectLock)
            {
                foreach (Control control in Controls)
                {
                    MapTracker mapTracker = control as MapTracker;
                    XElement xElement = mapTracker.ToXElement();
                    xElements.Add(xElement);
                }
            }
            return xElements;
        }
    }
}

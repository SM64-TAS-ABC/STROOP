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
        private readonly object _objectLock = new object();

        private MapObject _mapObjMap;
        private MapObject _mapObjBackground;
        private MapObject _mapObjHitboxHackTris;

        public void Initialize(MapObject mapObjMap, MapObject mapObjBackground, MapObject mapObjHitboxHackTris)
        {
            _mapObjMap = mapObjMap;
            _mapObjBackground = mapObjBackground;
            _mapObjHitboxHackTris = mapObjHitboxHackTris;
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
            if (Config.MapGui.checkBoxMapOptionsEnable3D.Checked ||
                Config.MapGui.checkBoxMapOptionsEnableOrthographicView.Checked)
            {            
                _mapObjHitboxHackTris.Update();
            }
            if (!Config.MapGui.checkBoxMapOptionsEnable3D.Checked)
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

        public void DrawOn2DControl()
        {
            _mapObjBackground.DrawOn2DControl();
            _mapObjMap.DrawOn2DControl();
            
            List<MapObject> listOrderOnTop = new List<MapObject>();
            List<MapObject> listOrderOnBottom = new List<MapObject>();
            List<MapObject> listOrderByY = new List<MapObject>();

            lock (_objectLock)
            {
                foreach (MapTracker mapTracker in Controls)
                {
                    switch (mapTracker.GetOrderType())
                    {
                        case MapTrackerOrderType.OrderOnTop:
                            listOrderOnTop.AddRange(mapTracker.GetMapObjectsToDisplay());
                            break;
                        case MapTrackerOrderType.OrderOnBottom:
                            listOrderOnBottom.AddRange(mapTracker.GetMapObjectsToDisplay());
                            break;
                        case MapTrackerOrderType.OrderByY:
                            listOrderByY.AddRange(mapTracker.GetMapObjectsToDisplay());
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

            if (!Config.MapGui.checkBoxMapOptionsDisableHitboxHackTris.Checked)
            {
                listCombined.Insert(0, _mapObjHitboxHackTris);
            }

            foreach (MapObject obj in listCombined)
            {
                obj.DrawOn2DControl();
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
                    switch (mapTracker.GetOrderType())
                    {
                        case MapTrackerOrderType.OrderOnTop:
                            listOrderOnTop.AddRange(mapTracker.GetMapObjectsToDisplay());
                            break;
                        case MapTrackerOrderType.OrderOnBottom:
                            listOrderOnBottom.AddRange(mapTracker.GetMapObjectsToDisplay());
                            break;
                        case MapTrackerOrderType.OrderByY:
                            listOrderByY.AddRange(mapTracker.GetMapObjectsToDisplay());
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
            if (!Config.MapGui.checkBoxMapOptionsDisableHitboxHackTris.Checked)
            {
                listCombined.Insert(0, _mapObjHitboxHackTris);
            }

            List<MapObject> listDrawType = listCombined.FindAll(obj => obj.GetDrawType() == drawType);
            foreach (MapObject obj in listDrawType)
            {
                obj.DrawOn3DControl();
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

        public void NotifyMouseEvent(MouseEvent mouseEvent, bool isLeftButton, int mouseX, int mouseY)
        {
            lock (_objectLock)
            {
                foreach (MapTracker mapTracker in Controls)
                {
                    mapTracker.NotifyMouseEvent(mouseEvent, isLeftButton, mouseX, mouseY);
                }
            }
        }
    }
}

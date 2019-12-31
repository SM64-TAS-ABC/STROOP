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

namespace STROOP.Map3
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

        public void MoveUpControl(MapTracker mapTracker)
        {
            lock (_objectLock)
            {
                int index = Controls.IndexOf(mapTracker);
                if (index == 0) return;
                int newIndex = index - 1;
                Controls.SetChildIndex(mapTracker, newIndex);
            }
        }

        public void MoveDownControl(MapTracker mapTracker)
        {
            lock (_objectLock)
            {
                int index = Controls.IndexOf(mapTracker);
                if (index == Controls.Count - 1) return;
                int newIndex = index + 1;
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
            _mapObjMap.Update();
            _mapObjBackground.Update();
            _mapObjHitboxHackTris.Update();

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

            foreach (MapObject obj in listOrderOnBottom)
            {
                obj.DrawOn2DControl();
            }
            foreach (MapObject obj in listOrderByY)
            {
                obj.DrawOn2DControl();
            }
            foreach (MapObject obj in listOrderOnTop)
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
            if (!Config.Map3Gui.checkBoxMap3OptionsDisable3DHitboxHackTris.Checked)
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
    }
}

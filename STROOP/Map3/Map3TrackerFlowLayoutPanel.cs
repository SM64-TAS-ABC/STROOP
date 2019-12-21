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

namespace STROOP.Map3
{
    public class Map3TrackerFlowLayoutPanel : NoTearFlowLayoutPanel
    {
        private readonly Object _objectLock = new Object();

        private Map3Object _mapObjMap;
        private Map3Object _mapObjBackground;

        public void Initialize(Map3Object mapObjMap, Map3Object mapObjBackground)
        {
            _mapObjMap = mapObjMap;
            _mapObjBackground = mapObjBackground;
        }

        public void MoveUpControl(Map3Tracker mapTracker)
        {
            lock (_objectLock)
            {
                int index = Controls.IndexOf(mapTracker);
                if (index == 0) return;
                int newIndex = index - 1;
                Controls.SetChildIndex(mapTracker, newIndex);
            }
        }

        public void MoveDownControl(Map3Tracker mapTracker)
        {
            lock (_objectLock)
            {
                int index = Controls.IndexOf(mapTracker);
                if (index == Controls.Count - 1) return;
                int newIndex = index + 1;
                Controls.SetChildIndex(mapTracker, newIndex);
            }
        }

        public void RemoveControl(Map3Tracker mapTracker)
        {
            lock (_objectLock)
            {
                mapTracker.CleanUp();
                Controls.Remove(mapTracker);
            }
        }

        public void AddNewControl(Map3Tracker mapTracker)
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
                    RemoveControl(Controls[0] as Map3Tracker);
                }
            }
        }

        public void UpdateControl()
        {
            lock (_objectLock)
            {
                foreach (Map3Tracker tracker in Controls)
                {
                    tracker.UpdateControl();
                }
            }
        }

        public void DrawOnControl()
        {
            _mapObjBackground.DrawOn2DControl();
            _mapObjMap.DrawOn2DControl();
            
            List<Map3Object> listOrderOnTop = new List<Map3Object>();
            List<Map3Object> listOrderOnBottom = new List<Map3Object>();
            List<Map3Object> listOrderByY = new List<Map3Object>();

            lock (_objectLock)
            {
                foreach (Map3Tracker mapTracker in Controls)
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

            foreach (Map3Object obj in listOrderOnBottom)
            {
                obj.DrawOn2DControl();
            }
            foreach (Map3Object obj in listOrderByY)
            {
                obj.DrawOn2DControl();
            }
            foreach (Map3Object obj in listOrderOnTop)
            {
                obj.DrawOn2DControl();
            }
        }

        public void SetGlobalIconSize(float size)
        {
            lock (_objectLock)
            {
                foreach (Map3Tracker tracker in Controls)
                {
                    tracker.SetGlobalIconSize(size);
                }
            }
        }
    }
}

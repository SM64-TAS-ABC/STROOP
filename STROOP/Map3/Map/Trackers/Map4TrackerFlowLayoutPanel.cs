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

namespace STROOP.Map3.Map.Trackers
{
    public class Map4TrackerFlowLayoutPanel : NoTearFlowLayoutPanel
    {
        private readonly Object _objectLock = new Object();

        public void MoveUpControl(Map4Tracker mapTracker)
        {
            lock (_objectLock)
            {
                int index = Controls.IndexOf(mapTracker);
                if (index == 0) return;
                int newIndex = index - 1;
                Controls.SetChildIndex(mapTracker, newIndex);
            }
        }

        public void MoveDownControl(Map4Tracker mapTracker)
        {
            lock (_objectLock)
            {
                int index = Controls.IndexOf(mapTracker);
                if (index == Controls.Count - 1) return;
                int newIndex = index + 1;
                Controls.SetChildIndex(mapTracker, newIndex);
            }
        }

        public void RemoveControl(Map4Tracker mapTracker)
        {
            lock (_objectLock)
            {
                mapTracker.CleanUp();
                Controls.Remove(mapTracker);
            }
        }

        public void AddNewControl(Map4Tracker mapTracker)
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
                    RemoveControl(Controls[0] as Map4Tracker);
                }
            }
        }
        
        public void UpdateControls()
        {
            foreach (Map4Tracker tracker in Controls)
            {
                tracker.UpdateTracker();
            }

            List<Map4Object> listOrderOnTop = new List<Map4Object>();
            List<Map4Object> listOrderOnBottom = new List<Map4Object>();
            List<Map4Object> listOrderByDepth = new List<Map4Object>();

            lock (_objectLock)
            {
                foreach (Map4Tracker mapTracker in Controls)
                {
                    if (!mapTracker.Visible) continue;
                    switch (mapTracker.GetOrderType())
                    {
                        case MapTrackerOrderType.OrderOnTop:
                            listOrderOnTop.AddRange(mapTracker.MapObjectList);
                            break;
                        case MapTrackerOrderType.OrderOnBottom:
                            listOrderOnBottom.AddRange(mapTracker.MapObjectList);
                            break;
                        case MapTrackerOrderType.OrderByY:
                            listOrderByDepth.AddRange(mapTracker.MapObjectList);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            listOrderOnTop.Reverse();

            for (int i = 0; i < listOrderByDepth.Count; i++)
            {
                listOrderByDepth[i].DisplayLayer = 0;
            }

            for (int i = 0; i < listOrderOnTop.Count; i++)
            {
                listOrderOnTop[i].DisplayLayer = i + 1;
            }

            for (int i = 0; i < listOrderOnBottom.Count; i++)
            {
                listOrderOnBottom[i].DisplayLayer = -1 * (i + 1);
            }
        }

    }
}

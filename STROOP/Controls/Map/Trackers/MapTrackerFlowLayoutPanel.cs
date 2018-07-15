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

namespace STROOP.Controls.Map.Trackers
{
    public class MapTrackerFlowLayoutPanel : NoTearFlowLayoutPanel
    {
        private readonly Object _objectLock = new Object();

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
                Controls.Clear();
            }
        }
        
        public void UpdateControls()
        {
            foreach (MapTracker tracker in Controls)
            {
                tracker.UpdateTracker();
            }

            List<MapObject> listOrderOnTop = new List<MapObject>();
            List<MapObject> listOrderOnBottom = new List<MapObject>();
            List<MapObject> listOrderByDepth = new List<MapObject>();

            lock (_objectLock)
            {
                foreach (MapTracker mapTracker in Controls)
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
                        case MapTrackerOrderType.OrderByDepth:
                            listOrderByDepth.AddRange(mapTracker.MapObjectList);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            listOrderOnTop.Reverse();
            listOrderOnBottom.Reverse();
            listOrderByDepth.OrderBy(mapObj => mapObj.GetDepth());

            List<MapObject> listMapObjects = new List<MapObject>();
            listMapObjects.AddRange(listOrderOnBottom);
            listMapObjects.AddRange(listOrderByDepth);
            listMapObjects.AddRange(listOrderOnTop);
            listMapObjects.ForEach(mapObject => mapObject.Update());
        }

    }
}

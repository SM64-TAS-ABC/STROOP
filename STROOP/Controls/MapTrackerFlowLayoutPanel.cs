using STROOP.Forms;
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

namespace STROOP.Controls
{
    public class MapTrackerFlowLayoutPanel : NoTearFlowLayoutPanel
    {
        private readonly Object _objectLock;

        public MapTrackerFlowLayoutPanel()
        {
            _objectLock = new Object();
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
                Controls.Remove(mapTracker);
            }
        }

        public void AddNewControl()
        {
            MapTracker mapTracker = new MapTracker();
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
            lock (_objectLock)
            {
                for (int i = 0; i < Controls.Count; i++)
                {
                    MapTracker mapTracker = Controls[i] as MapTracker;
                    mapTracker.UpdateControl();
                }
            }
        }

    }
}

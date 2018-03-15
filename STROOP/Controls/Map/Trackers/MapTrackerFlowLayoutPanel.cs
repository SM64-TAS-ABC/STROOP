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

        public void MoveUpControl(Control mapTracker)
        {
            lock (_objectLock)
            {
                int index = Controls.IndexOf(mapTracker);
                if (index == 0) return;
                int newIndex = index - 1;
                Controls.SetChildIndex(mapTracker, newIndex);
            }
        }

        public void MoveDownControl(Control mapTracker)
        {
            lock (_objectLock)
            {
                int index = Controls.IndexOf(mapTracker);
                if (index == Controls.Count - 1) return;
                int newIndex = index + 1;
                Controls.SetChildIndex(mapTracker, newIndex);
            }
        }

        public void RemoveControl(Control mapTracker)
        {
            lock (_objectLock)
            {
                Controls.Remove(mapTracker);
            }
        }

        public void AddNewControl(Control mapTracker)
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
            lock (_objectLock)
            {
                foreach(IUpdatable updatable in Controls.OfType<IUpdatable>())
                    updatable?.Update();
            }
        }

    }
}

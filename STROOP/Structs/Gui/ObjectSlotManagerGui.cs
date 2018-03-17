using STROOP.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STROOP.Structs
{
    public class ObjectSlotManagerGui
    {
        public Image SelectedObjectOverlayImage;
        public Image TrackedAndShownObjectOverlayImage;
        public Image TrackedNotShownObjectOverlayImage;
        public Image StoodOnObjectOverlayImage;
        public Image HeldObjectOverlayImage;
        public Image InteractionObjectOverlayImage;
        public Image UsedObjectOverlayImage;
        public Image ClosestObjectOverlayImage;
        public Image CameraObjectOverlayImage;
        public Image CameraHackObjectOverlayImage;
        public Image ModelObjectOverlayImage;
        public Image FloorObjectOverlayImage;
        public Image WallObjectOverlayImage;
        public Image CeilingObjectOverlayImage;
        public Image ParentObjectOverlayImage;
        public Image ParentUnusedObjectOverlayImage;
        public Image ParentNoneObjectOverlayImage;
        public Image ChildObjectOverlayImage;
        public Image MarkedObjectOverlayImage;

        public CheckBox LockLabelsCheckbox;
        public TabControl TabControl;
        public ComboBox SortMethodComboBox;
        public ComboBox LabelMethodComboBox;
        public WatchVariableFlowLayoutPanel FlowLayoutContainer;

        ~ObjectSlotManagerGui()
        {
            SelectedObjectOverlayImage?.Dispose();
            TrackedAndShownObjectOverlayImage?.Dispose();
            TrackedNotShownObjectOverlayImage?.Dispose();
            StoodOnObjectOverlayImage?.Dispose();
            HeldObjectOverlayImage?.Dispose();
            InteractionObjectOverlayImage?.Dispose();
            UsedObjectOverlayImage?.Dispose();
            ClosestObjectOverlayImage?.Dispose();
            CameraObjectOverlayImage?.Dispose();
            CameraHackObjectOverlayImage?.Dispose();
            ModelObjectOverlayImage?.Dispose();
            FloorObjectOverlayImage?.Dispose();
            WallObjectOverlayImage?.Dispose();
            CeilingObjectOverlayImage?.Dispose();
            ParentObjectOverlayImage?.Dispose();
            ParentUnusedObjectOverlayImage?.Dispose();
            ParentNoneObjectOverlayImage?.Dispose();
            ChildObjectOverlayImage?.Dispose();
            MarkedObjectOverlayImage?.Dispose();
        }
    }
}

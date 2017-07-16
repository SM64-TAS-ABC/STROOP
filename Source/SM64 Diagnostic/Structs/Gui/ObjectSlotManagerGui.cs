using SM64_Diagnostic.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM64_Diagnostic.Structs
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
        public Image FloorObjectOverlayImage;
        public Image WallObjectOverlayImage;
        public Image CeilingObjectOverlayImage;

        public CheckBox LockLabelsCheckbox;
        public TabControl TabControl;
        public ComboBox SortMethodComboBox;
        public ComboBox LabelMethodComboBox;
        public NoTearFlowLayoutPanel FlowLayoutContainer;

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
            FloorObjectOverlayImage?.Dispose();
            WallObjectOverlayImage?.Dispose();
            CeilingObjectOverlayImage?.Dispose();
        }
    }
}

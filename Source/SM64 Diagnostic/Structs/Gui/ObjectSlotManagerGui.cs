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
        public Image StandingOnObjectOverlayImage;
        public Image HoldingObjectOverlayImage;
        public Image InteractingObjectOverlayImage;
        public Image UsingObjectOverlayImage;

        public CheckBox LockLabelsCheckbox;
        public TabControl TabControl;
        public ComboBox MapObjectToggleModeComboBox;
        public FlowLayoutPanel FlowLayoutContainer;

        ~ObjectSlotManagerGui()
        {
            SelectedObjectOverlayImage?.Dispose();
            StandingOnObjectOverlayImage?.Dispose();
            HoldingObjectOverlayImage?.Dispose();
            InteractingObjectOverlayImage?.Dispose();
            UsingObjectOverlayImage?.Dispose();
        }
    }
}

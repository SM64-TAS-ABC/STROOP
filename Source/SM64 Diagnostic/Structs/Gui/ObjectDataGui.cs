using SM64_Diagnostic.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM64_Diagnostic.Structs
{
    public struct ObjectDataGui
    {
        public Panel ObjectBorderPanel;
        public IntPictureBox ObjectImagePictureBox;
        public NoTearFlowLayoutPanel ObjectFlowLayout;
        public Button CloneButton;
        public Button UnloadButton;
        public Button RetrieveButton;
        public Button GoToButton;
        public Button RetrieveHomeButton;
        public Button GoToHomeButton;
        public TextBox ObjectNameTextBox;
        public Label ObjBehaviorLabel;
        public Label ObjSlotIndexLabel;
        public Label ObjSlotPositionLabel;
        public Label ObjAddressLabel;
        public Label ObjAddressLabelValue;
    }
}

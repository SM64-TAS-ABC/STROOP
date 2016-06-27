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
        public PictureBox ObjectImagePictureBox;
        public FlowLayoutPanel ObjectFlowLayout;
        public Button CloneButton;
        public Button UnloadButton;
        public Button MoveToMarioButton;
        public Button MoveMarioToButton;
        public TextBox ObjectNameTextBox;
        public Label ObjBehaviorLabel;
        public Label ObjSlotIndexLabel;
        public Label ObjSlotPositionLabel;
        public Label ObjAddressLabel;
    }
}

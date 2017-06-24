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

        public Button PosXnZnButton;
        public Button PosXnButton;
        public Button PosXnZpButton;
        public Button PosZnButton;
        public Button PosZpButton;
        public Button PosXpZnButton;
        public Button PosXpButton;
        public Button PosXpZpButton;
        public Button PosYnButton;
        public Button PosYpButton;
        public TextBox PosXZTextbox;
        public TextBox PosYTextbox;

        public Button AngleYawPButton;
        public Button AngleYawNButton;
        public Button AnglePitchPButton;
        public Button AnglePitchNButton;
        public Button AngleRollPButton;
        public Button AngleRollNButton;
        public TextBox AngleYawTextbox;
        public TextBox AnglePitchTextbox;
        public TextBox AngleRollTextbox;

        public Button HomeXnZnButton;
        public Button HomeXnButton;
        public Button HomeXnZpButton;
        public Button HomeZnButton;
        public Button HomeZpButton;
        public Button HomeXpZnButton;
        public Button HomeXpButton;
        public Button HomeXpZpButton;
        public Button HomeYnButton;
        public Button HomeYpButton;
        public TextBox HomeXZTextbox;
        public TextBox HomeYTextbox;
    }
}

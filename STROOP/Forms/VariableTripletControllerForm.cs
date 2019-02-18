using STROOP.Controls;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Linq;

namespace STROOP.Forms
{
    public partial class VariableTripletControllerForm : Form
    {
        public VariableTripletControllerForm()
        {
            InitializeComponent();
        }

        public void Initialize(List<WatchVariableControl> controls)
        {
            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Euler,
                true,
                groupBoxVar,
                groupBoxVar.Controls["buttonVarXn"] as Button,
                groupBoxVar.Controls["buttonVarXp"] as Button,
                groupBoxVar.Controls["buttonVarZn"] as Button,
                groupBoxVar.Controls["buttonVarZp"] as Button,
                groupBoxVar.Controls["buttonVarXnZn"] as Button,
                groupBoxVar.Controls["buttonVarXnZp"] as Button,
                groupBoxVar.Controls["buttonVarXpZn"] as Button,
                groupBoxVar.Controls["buttonVarXpZp"] as Button,
                groupBoxVar.Controls["buttonVarYp"] as Button,
                groupBoxVar.Controls["buttonVarYn"] as Button,
                groupBoxVar.Controls["textBoxVarXZ"] as TextBox,
                groupBoxVar.Controls["textBoxVarY"] as TextBox,
                groupBoxVar.Controls["checkBoxVarRelative"] as CheckBox,
                (float hOffset, float vOffset, float nOffset, bool useRelative) =>
                {
                    ButtonUtilities.TranslateMario(
                        hOffset,
                        nOffset,
                        -1 * vOffset,
                        useRelative);
                });
        }

        public void ShowForm()
        {
            Show();
        }
    }
}

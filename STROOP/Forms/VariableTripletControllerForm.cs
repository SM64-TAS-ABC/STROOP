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
            if (controls.Count < 3) return;

            List<Func<double>> getters =
                new List<Func<double>>()
                {
                    () => ParsingUtilities.ParseDouble(controls[0].GetValue(false)),
                    () => ParsingUtilities.ParseDouble(controls[1].GetValue(false)),
                    () => ParsingUtilities.ParseDouble(controls[2].GetValue(false)),
                };
            if (controls.Count >= 4)
                getters.Add(() => ParsingUtilities.ParseDouble(controls[3].GetValue(false)));

            List<Func<double, bool>> setters =
                new List<Func<double, bool>>()
                {
                    (double value) => controls[0].SetValue(value),
                    (double value) => controls[1].SetValue(value),
                    (double value) => controls[2].SetValue(value),
                };
            if (controls.Count >= 4)
                setters.Add((double value) => controls[3].SetValue(value));

            PositionAngle posAngle = PositionAngle.Functions(getters, setters);
                    
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
                    ButtonUtilities.TranslatePosAngle(
                        posAngle,
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

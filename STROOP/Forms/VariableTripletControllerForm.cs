using STROOP.Controls;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    if (controls.Count < 3) return;

                    List<List<object>> valueLists = new List<List<object>>()
                    {
                        controls[0].GetValues(handleFormatting: false),
                        controls[1].GetValues(handleFormatting: false),
                        controls[2].GetValues(handleFormatting: false),
                    };
                    if (controls.Count >= 4)
                    {
                        valueLists.Add(controls[3].GetValues(handleFormatting: false));
                    }
                    int minCount = valueLists.Min(valueList => valueList.Count);

                    List<PositionAngle> posAngles = new List<PositionAngle>();
                    for (int i = 0; i < minCount; i++)
                    {
                        int index = i;
                        List<Func<double>> getters = new List<Func<double>>()
                        {
                            () => ParsingUtilities.ParseDouble(valueLists[0][index]),
                            () => ParsingUtilities.ParseDouble(valueLists[1][index]),
                            () => ParsingUtilities.ParseDouble(valueLists[2][index]),
                            () => controls.Count >= 4 ? ParsingUtilities.ParseDouble(valueLists[3][index]) : double.NaN,
                        };
                        List<Func<double, bool, bool>> setters = new List<Func<double, bool, bool>>()
                        {
                            (double value, bool setManually) => controls[0].SetValueOfValues(value, setManually, index),
                            (double value, bool setManually) => controls[1].SetValueOfValues(value, setManually, index),
                            (double value, bool setManually) => controls[2].SetValueOfValues(value, setManually, index),
                            (double value, bool setManually) => controls.Count >= 4 ? controls[3].SetValueOfValues(value, setManually, index) : true,
                        };
                        PositionAngle posAngle = PositionAngle.Functions(getters, setters);
                        posAngles.Add(posAngle);
                    }
                    ButtonUtilities.TranslatePosAngle(
                        posAngles,
                        hOffset,
                        nOffset,
                        -1 * vOffset,
                        useRelative);
                });
        }

        public void Initialize(
            string text,
            CoordinateSystem coordinateSystem,
            bool allowRelativeOptions,
            Action<float, float, float, bool> actionMove)
        {
            Text = text;
            ControlUtilities.InitializeThreeDimensionController(
                coordinateSystem,
                allowRelativeOptions,
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
                actionMove);
        }

        public void ShowForm()
        {
            Show();
        }
    }
}

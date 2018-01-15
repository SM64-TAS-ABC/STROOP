using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using System.Windows.Forms;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Controls;
using SM64_Diagnostic.Extensions;
using SM64_Diagnostic.Structs.Configurations;
using static SM64_Diagnostic.Utilities.ControlUtilities;

namespace SM64_Diagnostic.Managers
{
    public class CustomManager : DataManager
    {
        public static CustomManager Instance;

        public CustomManager(List<WatchVariableControl> variables, Control customControl, WatchVariablePanel variableTable)
            : base(variables, variableTable)
        {
            Instance = this;

            variables.ForEach(variable => variable.NotifyInCustomTab());

            SplitContainer splitContainerCustom = customControl.Controls["splitContainerCustom"] as SplitContainer;

            Button buttonClearVariables = splitContainerCustom.Panel1.Controls["buttonClearVariables"] as Button;
            buttonClearVariables.Click += (sender, e) => ClearVariables();

            GroupBox groupBoxVarNameWidth = splitContainerCustom.Panel1.Controls["groupBoxVarNameWidth"] as GroupBox;
            InitializeAddSubtractGetSetFuncionality(
                groupBoxVarNameWidth.Controls["buttonVarNameWidthSubtract"] as Button,
                groupBoxVarNameWidth.Controls["buttonVarNameWidthAdd"] as Button,
                groupBoxVarNameWidth.Controls["buttonVarNameWidthGet"] as Button,
                groupBoxVarNameWidth.Controls["buttonVarNameWidthSet"] as Button,
                groupBoxVarNameWidth.Controls["betterTextboxVarNameWidthAddSubtract"] as TextBox,
                groupBoxVarNameWidth.Controls["betterTextboxVarNameWidthGetSet"] as TextBox,
                (int value) => { WatchVariableControl.VariableNameWidth = value; },
                () => WatchVariableControl.VariableNameWidth);

            GroupBox groupBoxVarValueWidth = splitContainerCustom.Panel1.Controls["groupBoxVarValueWidth"] as GroupBox;
            InitializeAddSubtractGetSetFuncionality(
                groupBoxVarValueWidth.Controls["buttonVarValueWidthSubtract"] as Button,
                groupBoxVarValueWidth.Controls["buttonVarValueWidthAdd"] as Button,
                groupBoxVarValueWidth.Controls["buttonVarValueWidthGet"] as Button,
                groupBoxVarValueWidth.Controls["buttonVarValueWidthSet"] as Button,
                groupBoxVarValueWidth.Controls["betterTextboxVarValueWidthAddSubtract"] as TextBox,
                groupBoxVarValueWidth.Controls["betterTextboxVarValueWidthGetSet"] as TextBox,
                (int value) => { WatchVariableControl.VariableValueWidth = value; },
                () => WatchVariableControl.VariableValueWidth);

            GroupBox groupBoxVarHeight = splitContainerCustom.Panel1.Controls["groupBoxVarHeight"] as GroupBox;
            InitializeAddSubtractGetSetFuncionality(
                groupBoxVarHeight.Controls["buttonVarHeightSubtract"] as Button,
                groupBoxVarHeight.Controls["buttonVarHeightAdd"] as Button,
                groupBoxVarHeight.Controls["buttonVarHeightGet"] as Button,
                groupBoxVarHeight.Controls["buttonVarHeightSet"] as Button,
                groupBoxVarHeight.Controls["betterTextboxVarHeightAddSubtract"] as TextBox,
                groupBoxVarHeight.Controls["betterTextboxVarHeightGetSet"] as TextBox,
                (int value) => { WatchVariableControl.VariableHeight = value; },
                () => WatchVariableControl.VariableHeight);
        }

        public static void InitializeAddSubtractGetSetFuncionality(
            Button buttonSubtract,
            Button buttonAdd,
            Button buttonGet,
            Button buttonSet,
            TextBox textboxAddSubtract,
            TextBox textboxGetSet,
            Action<int> setterFunction,
            Func<int> getterFunction)
        {
            buttonSubtract.Click += (sender, e) =>
            {
                int? intValueNullable = ParsingUtilities.ParseIntNullable(textboxAddSubtract.Text);
                if (!intValueNullable.HasValue) return;
                int intValue = intValueNullable.Value;
                int newValue = getterFunction() - intValue;
                setterFunction(newValue);
            };

            buttonAdd.Click += (sender, e) =>
            {
                int? intValueNullable = ParsingUtilities.ParseIntNullable(textboxAddSubtract.Text);
                if (!intValueNullable.HasValue) return;
                int intValue = intValueNullable.Value;
                int newValue = getterFunction() + intValue;
                setterFunction(newValue);
            };

            buttonGet.Click += (sender, e) =>
            {
                textboxGetSet.Text = getterFunction().ToString();
            };

            buttonSet.Click += (sender, e) =>
            {
                int? intValueNullable = ParsingUtilities.ParseIntNullable(textboxGetSet.Text);
                if (!intValueNullable.HasValue) return;
                int intValue = intValueNullable.Value;
                setterFunction(intValue);
            };
        }

        public override void AddVariable(WatchVariableControl watchVarControl)
        {
            base.AddVariable(watchVarControl);
            watchVarControl.NotifyInCustomTab();
        }

        public override void Update(bool updateView)
        {
            if (!updateView)
                return;

            base.Update();
        }
    }
}

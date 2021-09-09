using STROOP.Controls;
using STROOP.Forms;
using STROOP.Managers;
using STROOP.Models;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace STROOP.Structs
{
    public static class WatchVariableSelectionUtilities
    {
        public static List<ToolStripItem> CreateSelectionToolStripItems(
            Func<List<WatchVariableControl>> getVars,
            WatchVariableFlowLayoutPanel panel)
        {
            Action<WatchVariableControlSettings, List<WatchVariableControl>> apply2 =
                (WatchVariableControlSettings settings, List<WatchVariableControl> vars) =>
            {
                if (KeyboardUtilities.IsCtrlHeld())
                    WatchVariableControlSettingsManager.AddSettings(settings);
                else
                    vars.ForEach(control => control.ApplySettings(settings));
            };

            Action<WatchVariableControlSettings> apply = (WatchVariableControlSettings settings) => apply2(settings, getVars());

            ToolStripMenuItem itemHighlight = new ToolStripMenuItem("Highlight...");
            ControlUtilities.AddDropDownItems(
                itemHighlight,
                new List<string>() { "Highlight", "Don't Highlight" },
                new List<Action>()
                {
                    () => apply(new WatchVariableControlSettings(changeHighlighted: true, newHighlighted: true)),
                    () => apply(new WatchVariableControlSettings(changeHighlighted: true, newHighlighted: false)),
                });
            ToolStripMenuItem itemHighlightColor = new ToolStripMenuItem("Color...");
            ControlUtilities.AddDropDownItems(
                itemHighlightColor,
                new List<string>()
                {
                    "Red",
                    "Orange",
                    "Yellow",
                    "Green",
                    "Blue",
                    "Purple",
                    "Pink",
                    "Brown",
                    "Black",
                    "White",
                },
                new List<Action>()
                {
                    () => apply(new WatchVariableControlSettings(changeHighlightColor: true, newHighlightColor: Color.Red)),
                    () => apply(new WatchVariableControlSettings(changeHighlightColor: true, newHighlightColor: Color.Orange)),
                    () => apply(new WatchVariableControlSettings(changeHighlightColor: true, newHighlightColor: Color.Yellow)),
                    () => apply(new WatchVariableControlSettings(changeHighlightColor: true, newHighlightColor: Color.Green)),
                    () => apply(new WatchVariableControlSettings(changeHighlightColor: true, newHighlightColor: Color.Blue)),
                    () => apply(new WatchVariableControlSettings(changeHighlightColor: true, newHighlightColor: Color.Purple)),
                    () => apply(new WatchVariableControlSettings(changeHighlightColor: true, newHighlightColor: Color.Pink)),
                    () => apply(new WatchVariableControlSettings(changeHighlightColor: true, newHighlightColor: Color.Brown)),
                    () => apply(new WatchVariableControlSettings(changeHighlightColor: true, newHighlightColor: Color.Black)),
                    () => apply(new WatchVariableControlSettings(changeHighlightColor: true, newHighlightColor: Color.White)),
                });
            itemHighlight.DropDownItems.Add(itemHighlightColor);

            ToolStripMenuItem itemLock = new ToolStripMenuItem("Lock...");
            ControlUtilities.AddDropDownItems(
                itemLock,
                new List<string>() { "Lock", "Don't Lock" },
                new List<Action>()
                {
                    () => apply(new WatchVariableControlSettings(changeLocked: true, newLocked: true)),
                    () => apply(new WatchVariableControlSettings(changeLocked: true, newLocked: false)),
                });

            ToolStripMenuItem itemFixAddress = new ToolStripMenuItem("Fix Address...");
            ControlUtilities.AddDropDownItems(
                itemFixAddress,
                new List<string>() { "Default", "Fix Address", "Fix Address Special", "Don't Fix Address" },
                new List<Action>()
                {
                    () => apply(new WatchVariableControlSettings(changeFixedAddress: true, changeFixedAddressToDefault: true)),
                    () => apply(new WatchVariableControlSettings(changeFixedAddress: true, newFixedAddress: true)),
                    () => apply(new WatchVariableControlSettings(doFixAddressSpecial: true)),
                    () => apply(new WatchVariableControlSettings(changeFixedAddress: true, newFixedAddress: false)),
                });

            ToolStripMenuItem itemCopy = new ToolStripMenuItem("Copy...");
            CopyUtilities.AddDropDownItems(itemCopy, getVars);

            ToolStripMenuItem itemPaste = new ToolStripMenuItem("Paste");
            itemPaste.Click += (sender, e) => PasteUtilities.Paste(getVars());

            ToolStripMenuItem itemRoundTo = new ToolStripMenuItem("Round to...");
            ToolStripMenuItem itemRoundToDefault = new ToolStripMenuItem("Default");
            itemRoundToDefault.Click += (sender, e) =>
                apply(new WatchVariableControlSettings(
                    changeRoundingLimit: true, changeRoundingLimitToDefault: true));
            ToolStripMenuItem itemRoundToNoRounding = new ToolStripMenuItem("No Rounding");
            itemRoundToNoRounding.Click += (sender, e) =>
                apply(new WatchVariableControlSettings(
                    changeRoundingLimit: true, newRoundingLimit: -1));
            List<ToolStripMenuItem> itemsRoundToNumDecimalPlaces = new List<ToolStripMenuItem>();
            for (int i = 0; i <= 10; i++)
            {
                int index = i;
                itemsRoundToNumDecimalPlaces.Add(new ToolStripMenuItem(index + " decimal place(s)"));
                itemsRoundToNumDecimalPlaces[index].Click += (sender, e) =>
                    apply(new WatchVariableControlSettings(
                        changeRoundingLimit: true, newRoundingLimit: index));
            }
            itemRoundTo.DropDownItems.Add(itemRoundToDefault);
            itemRoundTo.DropDownItems.Add(itemRoundToNoRounding);
            itemsRoundToNumDecimalPlaces.ForEach(setAllRoundingLimitsNumberItem =>
            {
                itemRoundTo.DropDownItems.Add(setAllRoundingLimitsNumberItem);
            });

            ToolStripMenuItem itemDisplayAsHex = new ToolStripMenuItem("Display as Hex...");
            ControlUtilities.AddDropDownItems(
                itemDisplayAsHex,
                new List<string>() { "Default", "Hex", "Decimal" },
                new List<Action>()
                {
                    () => apply(new WatchVariableControlSettings(changeDisplayAsHex: true, changeDisplayAsHexToDefault: true)),
                    () => apply(new WatchVariableControlSettings(changeDisplayAsHex: true, newDisplayAsHex: true)),
                    () => apply(new WatchVariableControlSettings(changeDisplayAsHex: true, newDisplayAsHex: false)),
                });

            ToolStripMenuItem itemAngleSigned = new ToolStripMenuItem("Angle: Signed...");
            ControlUtilities.AddDropDownItems(
                itemAngleSigned,
                new List<string>() { "Default", "Unsigned", "Signed" },
                new List<Action>()
                {
                    () => apply(new WatchVariableControlSettings(changeAngleSigned: true, changeAngleSignedToDefault: true)),
                    () => apply(new WatchVariableControlSettings(changeAngleSigned: true, newAngleSigned: false)),
                    () => apply(new WatchVariableControlSettings(changeAngleSigned: true, newAngleSigned: true)),
                });

            ToolStripMenuItem itemAngleUnits = new ToolStripMenuItem("Angle: Units...");
            ToolStripMenuItem itemAngleUnitsDefault = new ToolStripMenuItem("Default");
            itemAngleUnitsDefault.Click += (sender, e) =>
                apply(new WatchVariableControlSettings(
                    changeAngleUnits: true, changeAngleUnitsToDefault: true));
            List<ToolStripMenuItem> itemsAngleUnitsValue = new List<ToolStripMenuItem>();
            foreach (AngleUnitType angleUnitType in Enum.GetValues(typeof(AngleUnitType)))
            {
                AngleUnitType angleUnitTypeFixed = angleUnitType;
                string stringValue = angleUnitTypeFixed.ToString();
                if (stringValue == AngleUnitType.InGameUnits.ToString()) stringValue = "In-Game Units";
                ToolStripMenuItem itemAngleUnitsValue = new ToolStripMenuItem(stringValue);
                itemAngleUnitsValue.Click += (sender, e) =>
                    apply(new WatchVariableControlSettings(
                        changeAngleUnits: true, newAngleUnits: angleUnitTypeFixed));
                itemsAngleUnitsValue.Add(itemAngleUnitsValue);
            }
            itemAngleUnits.DropDownItems.Add(itemAngleUnitsDefault);
            itemsAngleUnitsValue.ForEach(setAllAngleUnitsValuesItem =>
            {
                itemAngleUnits.DropDownItems.Add(setAllAngleUnitsValuesItem);
            });

            ToolStripMenuItem itemAngleTruncateToMultipleOf16 = new ToolStripMenuItem("Angle: Truncate to Multiple of 16...");
            ControlUtilities.AddDropDownItems(
                itemAngleTruncateToMultipleOf16,
                new List<string>() { "Default", "Truncate to Multiple of 16", "Don't Truncate to Multiple of 16" },
                new List<Action>()
                {
                    () => apply(new WatchVariableControlSettings(changeAngleTruncateToMultipleOf16: true, changeAngleTruncateToMultipleOf16ToDefault: true)),
                    () => apply(new WatchVariableControlSettings(changeAngleTruncateToMultipleOf16: true, newAngleTruncateToMultipleOf16: true)),
                    () => apply(new WatchVariableControlSettings(changeAngleTruncateToMultipleOf16: true, newAngleTruncateToMultipleOf16: false)),
                });

            ToolStripMenuItem itemAngleConstrainToOneRevolution = new ToolStripMenuItem("Angle: Constrain to One Revolution...");
            ControlUtilities.AddDropDownItems(
                itemAngleConstrainToOneRevolution,
                new List<string>() { "Default", "Constrain to One Revolution", "Don't Constrain to One Revolution" },
                new List<Action>()
                {
                    () => apply(new WatchVariableControlSettings(changeAngleConstrainToOneRevolution: true, changeAngleConstrainToOneRevolutionToDefault: true)),
                    () => apply(new WatchVariableControlSettings(changeAngleConstrainToOneRevolution: true, newAngleConstrainToOneRevolution: true)),
                    () => apply(new WatchVariableControlSettings(changeAngleConstrainToOneRevolution: true, newAngleConstrainToOneRevolution: false)),
                });

            ToolStripMenuItem itemAngleReverse = new ToolStripMenuItem("Angle: Reverse...");
            ControlUtilities.AddDropDownItems(
                itemAngleReverse,
                new List<string>() { "Default", "Reverse", "Don't Reverse" },
                new List<Action>()
                {
                    () => apply(new WatchVariableControlSettings(changeAngleReverse: true, changeAngleReverseToDefault: true)),
                    () => apply(new WatchVariableControlSettings(changeAngleReverse: true, newAngleReverse: true)),
                    () => apply(new WatchVariableControlSettings(changeAngleReverse: true, newAngleReverse: false)),
                });

            ToolStripMenuItem itemAngleDisplayAsHex = new ToolStripMenuItem("Angle: Display as Hex...");
            ControlUtilities.AddDropDownItems(
                itemAngleDisplayAsHex,
                new List<string>() { "Default", "Hex", "Decimal" },
                new List<Action>()
                {
                    () => apply(new WatchVariableControlSettings(changeAngleDisplayAsHex: true, changeAngleDisplayAsHexToDefault: true)),
                    () => apply(new WatchVariableControlSettings(changeAngleDisplayAsHex: true, newAngleDisplayAsHex: true)),
                    () => apply(new WatchVariableControlSettings(changeAngleDisplayAsHex: true, newAngleDisplayAsHex: false)),
                });

            ToolStripMenuItem itemShowVariableXml = new ToolStripMenuItem("Show Variable XML");
            itemShowVariableXml.Click += (sender, e) =>
            {
                InfoForm infoForm = new InfoForm();
                infoForm.SetText(
                    "Variable Info",
                    "Variable XML",
                    String.Join("\r\n", getVars().ConvertAll(control => control.ToXml(true))));
                infoForm.Show();
            };

            ToolStripMenuItem itemShowVariableInfo = new ToolStripMenuItem("Show Variable Info");
            itemShowVariableInfo.Click += (sender, e) =>
            {
                InfoForm infoForm = new InfoForm();
                infoForm.SetText(
                    "Variable Info",
                    "Variable Info",
                    String.Join("\t",
                        WatchVariableWrapper.GetVarInfoLabels()) +
                        "\r\n" +
                        String.Join(
                            "\r\n",
                            getVars().ConvertAll(control => control.GetVarInfo())
                                .ConvertAll(infoList => String.Join("\t", infoList))));
                infoForm.Show();
            };

            void createBinaryMathOperationVariable(BinaryMathOperation operation)
            {
                List<WatchVariableControl> controls = getVars();
                if (controls.Count % 2 == 1) controls.RemoveAt(controls.Count - 1);

                for (int i = 0; i < controls.Count / 2; i++)
                {
                    WatchVariableControl control1 = controls[i];
                    WatchVariableControl control2 = controls[i + controls.Count / 2];
                    string specialType = WatchVariableSpecialUtilities.AddBinaryMathOperationEntry(control1, control2, operation);
                    string name = string.Format("{0} {1} {2}", control1.VarName, MathOperationUtilities.GetSymbol(operation), control2.VarName);

                    WatchVariable watchVariable =
                        new WatchVariable(
                            name: name,
                            memoryTypeName: null,
                            specialType: specialType,
                            baseAddressType: BaseAddressTypeEnum.None,
                            offsetUS: null,
                            offsetJP: null,
                            offsetSH: null,
                            offsetEU: null,
                            offsetDefault: null,
                            mask: null,
                            shift: null,
                            handleMapping: true);
                    WatchVariableControlPrecursor precursor =
                        new WatchVariableControlPrecursor(
                            name: name,
                            watchVar: watchVariable,
                            subclass: WatchVariableSubclass.Number,
                            backgroundColor: null,
                            displayType: null,
                            roundingLimit: null,
                            useHex: null,
                            invertBool: null,
                            isYaw: null,
                            coordinate: null,
                            groupList: new List<VariableGroup>() { VariableGroup.Custom });
                    WatchVariableControl control = precursor.CreateWatchVariableControl();
                    panel.AddVariable(control);
                }
            }
            void createAggregateMathOperationVariable(AggregateMathOperation operation)
            {
                List<WatchVariableControl> controls = getVars();
                if (controls.Count == 0) return;
                string specialType = WatchVariableSpecialUtilities.AddAggregateMathOperationEntry(controls, operation);
                string name = operation.ToString();
                WatchVariable watchVariable =
                    new WatchVariable(
                        name: name,
                        memoryTypeName: null,
                        specialType: specialType,
                        baseAddressType: BaseAddressTypeEnum.None,
                        offsetUS: null,
                        offsetJP: null,
                        offsetSH: null,
                        offsetEU: null,
                        offsetDefault: null,
                        mask: null,
                        shift: null,
                        handleMapping: true);
                WatchVariableControlPrecursor precursor =
                    new WatchVariableControlPrecursor(
                        name: name,
                        watchVar: watchVariable,
                        subclass: WatchVariableSubclass.Number,
                        backgroundColor: null,
                        displayType: null,
                        roundingLimit: null,
                        useHex: null,
                        invertBool: null,
                        isYaw: null,
                        coordinate: null,
                        groupList: new List<VariableGroup>() { VariableGroup.Custom });
                WatchVariableControl control = precursor.CreateWatchVariableControl();
                panel.AddVariable(control);
            }
            void createDistanceMathOperationVariable(bool use3D)
            {
                List<WatchVariableControl> controls = getVars();
                bool satisfies2D = !use3D && controls.Count >= 4;
                bool satisfies3D = use3D && controls.Count >= 6;
                if (!satisfies2D && !satisfies3D) return;
                string specialType = WatchVariableSpecialUtilities.AddDistanceMathOperationEntry(controls, use3D);
                string name = use3D ?
                    string.Format(
                        "({0},{1},{2}) to ({3},{4},{5})",
                        controls[0].VarName,
                        controls[1].VarName,
                        controls[2].VarName,
                        controls[3].VarName,
                        controls[4].VarName,
                        controls[5].VarName) :
                    string.Format(
                        "({0},{1}) to ({2},{3})",
                        controls[0].VarName,
                        controls[1].VarName,
                        controls[2].VarName,
                        controls[3].VarName);
                WatchVariable watchVariable =
                    new WatchVariable(
                        name: name,
                        memoryTypeName: null,
                        specialType: specialType,
                        baseAddressType: BaseAddressTypeEnum.None,
                        offsetUS: null,
                        offsetJP: null,
                        offsetSH: null,
                        offsetEU: null,
                        offsetDefault: null,
                        mask: null,
                        shift: null,
                        handleMapping: true);
                WatchVariableControlPrecursor precursor =
                    new WatchVariableControlPrecursor(
                        name: name,
                        watchVar: watchVariable,
                        subclass: WatchVariableSubclass.Number,
                        backgroundColor: null,
                        displayType: null,
                        roundingLimit: null,
                        useHex: null,
                        invertBool: null,
                        isYaw: null,
                        coordinate: null,
                        groupList: new List<VariableGroup>() { VariableGroup.Custom });
                WatchVariableControl control = precursor.CreateWatchVariableControl();
                panel.AddVariable(control);
            }
            void createRealTimeVariable()
            {
                List<WatchVariableControl> controls = getVars();
                for (int i = 0; i < controls.Count; i++)
                {
                    WatchVariableControl control = controls[i];
                    string specialType = WatchVariableSpecialUtilities.AddRealTimeEntry(control);
                    string name = string.Format("{0} Real Time", control.VarName);

                    WatchVariable watchVariable =
                        new WatchVariable(
                            name: name,
                            memoryTypeName: null,
                            specialType: specialType,
                            baseAddressType: BaseAddressTypeEnum.None,
                            offsetUS: null,
                            offsetJP: null,
                            offsetSH: null,
                            offsetEU: null,
                            offsetDefault: null,
                            mask: null,
                            shift: null,
                            handleMapping: true);
                    WatchVariableControlPrecursor precursor =
                        new WatchVariableControlPrecursor(
                            name: name,
                            watchVar: watchVariable,
                            subclass: WatchVariableSubclass.String,
                            backgroundColor: null,
                            displayType: null,
                            roundingLimit: null,
                            useHex: null,
                            invertBool: null,
                            isYaw: null,
                            coordinate: null,
                            groupList: new List<VariableGroup>() { VariableGroup.Custom });
                    WatchVariableControl control2 = precursor.CreateWatchVariableControl();
                    panel.AddVariable(control2);
                }
            }
            void createDereferencedVariable(string typeString)
            {
                List<WatchVariableControl> controls = getVars();
                uint? offset = null;
                if (KeyboardUtilities.IsCtrlHeld())
                {
                    string offsetString = DialogUtilities.GetStringFromDialog(labelText: "Enter Offset in Hex:");
                    if (offsetString != null)
                    {
                        offset = ParsingUtilities.ParseHexNullable(offsetString);
                    }
                }
                for (int i = 0; i < controls.Count; i++)
                {
                    WatchVariableControl control = controls[i];
                    string specialType = WatchVariableSpecialUtilities.AddDereferencedEntry(control, typeString, offset);
                    string name = string.Format("{0}{1} Deref", control.VarName, offset.HasValue ? " + " + HexUtilities.FormatValue(offset.Value) : "");

                    WatchVariable watchVariable =
                        new WatchVariable(
                            name: name,
                            memoryTypeName: null,
                            specialType: specialType,
                            baseAddressType: BaseAddressTypeEnum.None,
                            offsetUS: null,
                            offsetJP: null,
                            offsetSH: null,
                            offsetEU: null,
                            offsetDefault: null,
                            mask: null,
                            shift: null,
                            handleMapping: true);
                    WatchVariableControlPrecursor precursor =
                        new WatchVariableControlPrecursor(
                            name: name,
                            watchVar: watchVariable,
                            subclass: WatchVariableSubclass.Number,
                            backgroundColor: null,
                            displayType: null,
                            roundingLimit: null,
                            useHex: null,
                            invertBool: null,
                            isYaw: null,
                            coordinate: null,
                            groupList: new List<VariableGroup>() { VariableGroup.Custom });
                    WatchVariableControl control2 = precursor.CreateWatchVariableControl();
                    panel.AddVariable(control2);
                }
            }
            ToolStripMenuItem itemAddDereferencedVariable = new ToolStripMenuItem("Dereferenced...");
            foreach (string typeString in TypeUtilities.InGameTypeList)
            {
                ToolStripMenuItem typeItem = new ToolStripMenuItem(typeString);
                itemAddDereferencedVariable.DropDownItems.Add(typeItem);
                typeItem.Click += (sender, e) =>
                {
                    createDereferencedVariable(typeString);
                };
            }
            ToolStripMenuItem itemAddVariables = new ToolStripMenuItem("Add Variable(s)...");
            ControlUtilities.AddDropDownItems(
                itemAddVariables,
                new List<string>()
                {
                    "Addition",
                    "Subtraction",
                    "Multiplication",
                    "Division",
                    "Modulo",
                    "Non-Negative Modulo",
                    "Exponent",
                    null,
                    "Mean",
                    "Median",
                    "Min",
                    "Max",
                    "Sum",
                    null,
                    "2D Distance",
                    "3D Distance",
                    null,
                    "Real Time",
                },
                new List<Action>()
                {
                    () => createBinaryMathOperationVariable(BinaryMathOperation.Add),
                    () => createBinaryMathOperationVariable(BinaryMathOperation.Subtract),
                    () => createBinaryMathOperationVariable(BinaryMathOperation.Multiply),
                    () => createBinaryMathOperationVariable(BinaryMathOperation.Divide),
                    () => createBinaryMathOperationVariable(BinaryMathOperation.Modulo),
                    () => createBinaryMathOperationVariable(BinaryMathOperation.NonNegativeModulo),
                    () => createBinaryMathOperationVariable(BinaryMathOperation.Exponent),
                    () => { },
                    () => createAggregateMathOperationVariable(AggregateMathOperation.Mean),
                    () => createAggregateMathOperationVariable(AggregateMathOperation.Median),
                    () => createAggregateMathOperationVariable(AggregateMathOperation.Min),
                    () => createAggregateMathOperationVariable(AggregateMathOperation.Max),
                    () => createAggregateMathOperationVariable(AggregateMathOperation.Sum),
                    () => { },
                    () => createDistanceMathOperationVariable(use3D: false),
                    () => createDistanceMathOperationVariable(use3D: true),
                    () => { },
                    () => createRealTimeVariable(),
                });
            itemAddVariables.DropDownItems.Add(itemAddDereferencedVariable);

            ToolStripMenuItem itemSetCascadingValues = new ToolStripMenuItem("Set Cascading Values");
            itemSetCascadingValues.Click += (sender, e) =>
            {
                List<WatchVariableControl> controls = getVars();
                object value1 = DialogUtilities.GetStringFromDialog(labelText: "Base Value:");
                object value2 = DialogUtilities.GetStringFromDialog(labelText: "Offset Value:");
                if (value1 == null || value2 == null) return;
                double? number1 = ParsingUtilities.ParseDoubleNullable(value1);
                double? number2 = ParsingUtilities.ParseDoubleNullable(value2);
                if (!number1.HasValue || !number2.HasValue) return;
                List<Func<object, bool>> setters = controls.SelectMany(control => control.GetSetters()).ToList();
                for (int i = 0; i < setters.Count; i++)
                {
                    setters[i](number1.Value + i * number2.Value);
                }
            };

            List<string> backgroundColorStringList = new List<string>();
            List<Action> backgroundColorActionList = new List<Action>();
            backgroundColorStringList.Add("Default");
            backgroundColorActionList.Add(
                () => apply(new WatchVariableControlSettings(changeBackgroundColor: true, changeBackgroundColorToDefault: true)));
            foreach (KeyValuePair<string, string> pair in ColorUtilities.ColorToParamsDictionary)
            {
                Color color = ColorTranslator.FromHtml(pair.Value);
                string colorString = pair.Key;
                if (colorString == "LightBlue") colorString = "Light Blue";
                backgroundColorStringList.Add(colorString);
                backgroundColorActionList.Add(
                    () => apply(new WatchVariableControlSettings(changeBackgroundColor: true, newBackgroundColor: color)));
            }
            backgroundColorStringList.Add("Control (No Color)");
            backgroundColorActionList.Add(
                () => apply(new WatchVariableControlSettings(changeBackgroundColor: true, newBackgroundColor: SystemColors.Control)));
            backgroundColorStringList.Add("Custom Color");
            backgroundColorActionList.Add(
                () =>
                {
                    List<WatchVariableControl> vars = getVars();
                    Color? newColor = ColorUtilities.GetColorFromDialog(SystemColors.Control);
                    if (newColor.HasValue)
                    {
                        apply2(new WatchVariableControlSettings(changeBackgroundColor: true, newBackgroundColor: newColor.Value), vars);
                        ColorUtilities.LastCustomColor = newColor.Value;
                    }
                });
            backgroundColorStringList.Add("Last Custom Color");
            backgroundColorActionList.Add(
                () => apply(new WatchVariableControlSettings(changeBackgroundColor: true, newBackgroundColor: ColorUtilities.LastCustomColor)));
            ToolStripMenuItem itemBackgroundColor = new ToolStripMenuItem("Background Color...");
            ControlUtilities.AddDropDownItems(
                itemBackgroundColor,
                backgroundColorStringList,
                backgroundColorActionList);

            ToolStripMenuItem itemMove = new ToolStripMenuItem("Move...");
            ControlUtilities.AddDropDownItems(
                itemMove,
                new List<string>() { "Start Move", "End Move", "Clear Move" },
                new List<Action>()
                {
                    () => panel.NotifyOfReorderingStart(getVars()),
                    () => panel.NotifyOfReorderingEnd(getVars()),
                    () => panel.NotifyOfReorderingClear(),
                });

            ToolStripMenuItem itemRemove = new ToolStripMenuItem("Remove");
            itemRemove.Click += (sender, e) => panel.RemoveVariables(getVars());

            ToolStripMenuItem itemRename = new ToolStripMenuItem("Rename...");
            itemRename.Click += (sender, e) =>
            {
                List<WatchVariableControl> watchVars = getVars();
                string template = DialogUtilities.GetStringFromDialog("$");
                if (template == null) return;
                foreach (WatchVariableControl control in watchVars)
                {
                    control.VarName = template.Replace("$", control.VarName);
                }
            };

            ToolStripMenuItem itemOpenController = new ToolStripMenuItem("Open Controller");
            itemOpenController.Click += (sender, e) =>
            {
                List<WatchVariableControl> vars = getVars();
                VariableControllerForm varController =
                    new VariableControllerForm(
                        vars.ConvertAll(control => control.VarName),
                        vars.ConvertAll(control => control.WatchVarWrapper),
                        vars.ConvertAll(control => control.FixedAddressListGetter()));
                varController.Show();
            };

            ToolStripMenuItem itemOpenTripletController = new ToolStripMenuItem("Open Triplet Controller");
            itemOpenTripletController.Click += (sender, e) =>
            {
                VariableTripletControllerForm form = new VariableTripletControllerForm();
                form.Initialize(getVars().ConvertAll(control => control.CreateCopy()));
                form.ShowForm();
            };

            ToolStripMenuItem itemOpenPopOut = new ToolStripMenuItem("Open Pop Out");
            itemOpenPopOut.Click += (sender, e) =>
            {
                VariablePopOutForm form = new VariablePopOutForm();
                form.Initialize(getVars().ConvertAll(control => control.CreateCopy()));
                form.ShowForm();
            };

            ToolStripMenuItem itemAddToTab = new ToolStripMenuItem("Add to Tab...");
            ControlUtilities.AddDropDownItems(
                itemAddToTab,
                new List<string>() { "Regular", "Fixed", "Grouped by Base Address", "Grouped by Variable"},
                new List<Action>()
                {
                    () => SelectionForm.ShowDataManagerSelectionForm(getVars(), AddToTabTypeEnum.Regular),
                    () => SelectionForm.ShowDataManagerSelectionForm(getVars(), AddToTabTypeEnum.Fixed),
                    () => SelectionForm.ShowDataManagerSelectionForm(getVars(), AddToTabTypeEnum.GroupedByBaseAddress),
                    () => SelectionForm.ShowDataManagerSelectionForm(getVars(), AddToTabTypeEnum.GroupedByVariable),
                });

            ToolStripMenuItem itemAddToCustomTab = new ToolStripMenuItem("Add to Custom Tab...");
            ControlUtilities.AddDropDownItems(
                itemAddToCustomTab,
                new List<string>() { "Regular", "Fixed", "Grouped by Base Address", "Grouped by Variable" },
                new List<Action>()
                {
                    () => WatchVariableControl.AddVarsToTab(getVars(), Config.CustomManager, AddToTabTypeEnum.Regular),
                    () => WatchVariableControl.AddVarsToTab(getVars(), Config.CustomManager, AddToTabTypeEnum.Fixed),
                    () => WatchVariableControl.AddVarsToTab(getVars(), Config.CustomManager, AddToTabTypeEnum.GroupedByBaseAddress),
                    () => WatchVariableControl.AddVarsToTab(getVars(), Config.CustomManager, AddToTabTypeEnum.GroupedByVariable),
                });

            return new List<ToolStripItem>()
            {
                itemHighlight,
                itemLock,
                itemFixAddress,
                itemCopy,
                itemPaste,
                new ToolStripSeparator(),
                itemRoundTo,
                itemDisplayAsHex,
                new ToolStripSeparator(),
                itemAngleSigned,
                itemAngleUnits,
                itemAngleTruncateToMultipleOf16,
                itemAngleConstrainToOneRevolution,
                itemAngleReverse,
                itemAngleDisplayAsHex,
                new ToolStripSeparator(),
                itemShowVariableXml,
                itemShowVariableInfo,
                new ToolStripSeparator(),
                itemAddVariables,
                itemSetCascadingValues,
                new ToolStripSeparator(),
                itemBackgroundColor,
                itemMove,
                itemRemove,
                itemRename,
                itemOpenController,
                itemOpenTripletController,
                itemOpenPopOut,
                itemAddToTab,
                itemAddToCustomTab,
            };
        }
        
    }
}
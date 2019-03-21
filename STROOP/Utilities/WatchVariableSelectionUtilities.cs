using STROOP.Controls;
using STROOP.Forms;
using STROOP.Managers;
using STROOP.Models;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
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

            ToolStripMenuItem itemFix = new ToolStripMenuItem("Fix...");
            ControlUtilities.AddDropDownItems(
                itemFix,
                new List<string>() { "Default", "Fix", "Don't Fix" },
                new List<Action>()
                {
                    () => apply(new WatchVariableControlSettings(changeFixed: true, changeFixedToDefault: true)),
                    () => apply(new WatchVariableControlSettings(changeFixed: true, newFixed: true)),
                    () => apply(new WatchVariableControlSettings(changeFixed: true, newFixed: false)),
                });

            ToolStripMenuItem itemCopy = new ToolStripMenuItem("Copy...");
            Action<List<WatchVariableControl>, string> copyValues =
                (List<WatchVariableControl> controls, string separator) =>
            {
                if (controls.Count == 0) return;
                Clipboard.SetText(
                    String.Join(separator, controls.ConvertAll(
                        control => control.GetValue(false))));
            };
            ControlUtilities.AddDropDownItems(
                itemCopy,
                new List<string>()
                {
                    "Copy with Commas",
                    "Copy with Tabs",
                    "Copy with Line Breaks",
                    "Copy for Code",
                },
                new List<Action>()
                {
                    () => copyValues(getVars(), ","),
                    () => copyValues(getVars(), "\t"),
                    () => copyValues(getVars(), "\r\n"),
                    () =>
                    {
                        List<WatchVariableControl> watchVars = getVars();
                        string prefix = KeyboardUtilities.IsCtrlHeld() ? DialogUtilities.GetStringFromDialog() : "";
                        List<string> lines = new List<string>();
                        foreach (WatchVariableControl watchVar in watchVars)
                        {
                            Type type = watchVar.GetMemoryType();
                            string line = String.Format(
                                "{0} {1}{2} = {3}{4};",
                                type != null ? TypeUtilities.TypeToString[watchVar.GetMemoryType()] : "double",
                                prefix,
                                watchVar.VarName.Replace(" ", ""),
                                watchVar.GetValue(false),
                                type == typeof(float) ? "f" : "");
                            lines.Add(line);
                        }
                        if (lines.Count > 0)
                        {
                            Clipboard.SetText(String.Join("\r\n", lines));
                        }
                    },
                });

            ToolStripMenuItem itemPaste = new ToolStripMenuItem("Paste");
            itemPaste.Click += (sender, e) =>
            {
                List<string> stringList = ParsingUtilities.ParseStringList(Clipboard.GetText());
                List<WatchVariableControl> varList = getVars();
                if (stringList.Count != varList.Count) return;

                Config.Stream.Suspend();
                for (int i = 0; i < stringList.Count; i++)
                {
                    varList[i].SetValue(stringList[i]);
                }
                Config.Stream.Resume();
            };

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

            Action<MathOperation> createVariable = (MathOperation operation) =>
            {
                List<WatchVariableControl> controls = getVars();
                if (controls.Count % 2 == 1) controls.RemoveAt(controls.Count - 1);

                for (int i = 0; i < controls.Count / 2; i++)
                {
                    WatchVariableControl control1 = controls[i];
                    WatchVariableControl control2 = controls[i + controls.Count / 2];
                    string specialType = WatchVariableSpecialUtilities.AddCalculatedEntry(control1, control2, operation);

                    WatchVariable watchVariable =
                        new WatchVariable(
                            memoryTypeName: null,
                            specialType: specialType,
                            baseAddressType: BaseAddressTypeEnum.None,
                            offsetUS: null,
                            offsetJP: null,
                            offsetPAL: null,
                            offsetDefault: null,
                            mask: null,
                            shift: null);
                    WatchVariableControlPrecursor precursor =
                        new WatchVariableControlPrecursor(
                            name: string.Format("{0} {1} {2}", control1.VarName, MathOperationUtilities.GetSymbol(operation), control2.VarName),
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
            };
            ToolStripMenuItem itemAddVariables = new ToolStripMenuItem("Add Variables...");
            ControlUtilities.AddDropDownItems(
                itemAddVariables,
                new List<string>()
                {
                    "Addition",
                    "Subtraction",
                    "Multiplication",
                    "Division",
                },
                new List<Action>()
                {
                    () => createVariable(MathOperation.Add),
                    () => createVariable(MathOperation.Subtract),
                    () => createVariable(MathOperation.Multiply),
                    () => createVariable(MathOperation.Divide),
                });

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

            ToolStripMenuItem itemEnableCustomization = new ToolStripMenuItem("Enable Customization");
            itemEnableCustomization.Click += (sender, e) => apply(new WatchVariableControlSettings(enableCustomization: true));

            ToolStripMenuItem itemOpenController = new ToolStripMenuItem("Open Controller");
            itemOpenController.Click += (sender, e) =>
            {
                List<WatchVariableControl> vars = getVars();
                VariableControllerForm varController =
                    new VariableControllerForm(
                        vars.ConvertAll(control => control.VarName),
                        vars.ConvertAll(control => control.WatchVarWrapper),
                        vars.ConvertAll(control => control.FixedAddressList));
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
            itemAddToTab.Click += (sender, e) => SelectionForm.ShowDataManagerSelectionForm(getVars());

            ToolStripMenuItem itemAddToCustomTab = new ToolStripMenuItem("Add to Custom Tab");
            itemAddToCustomTab.Click += (sender, e) => WatchVariableControl.AddVarsToTab(getVars(), Config.CustomManager);

            return new List<ToolStripItem>()
            {
                itemHighlight,
                itemLock,
                itemFix,
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
                new ToolStripSeparator(),
                itemBackgroundColor,
                itemMove,
                itemRemove,
                itemEnableCustomization,
                itemOpenController,
                itemOpenTripletController,
                itemOpenPopOut,
                itemAddToTab,
                itemAddToCustomTab,
            };
        }
        
    }
}
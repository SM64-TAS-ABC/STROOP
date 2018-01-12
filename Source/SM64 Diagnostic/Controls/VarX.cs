using SM64_Diagnostic.Extensions;
using SM64_Diagnostic.Managers;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Structs.Configurations;
using SM64_Diagnostic.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SM64_Diagnostic.Controls
{
    public class VarX
    {
        protected const int DEFAULT_ROUNDING_LIMIT = 3;
        protected const bool DEFAULT_DISPLAY_AS_HEX = false;
        protected const bool DEFAULT_USE_CHECKBOX = false;

        protected readonly AddressHolder _addressHolder;
        protected readonly VarXControl _varXControl;
        protected readonly ContextMenuStrip _contextMenuStrip;

        private ToolStripMenuItem _itemLock;
        private ToolStripMenuItem _itemRemoveAllLocks;

        private readonly bool _startsAsCheckbox;

        public static VarX CreateVarX(
            AddressHolder addressHolder,
            VarXControl varXControl,
            VarXSubclass varXSubclcass,
            bool? useHex,
            bool? invertBool,
            VarXCoordinate? coordinate)
        {
            switch (varXSubclcass)
            {
                case VarXSubclass.String:
                    return new VarX(addressHolder, varXControl);

                case VarXSubclass.Number:
                    return new VarXNumber(
                        addressHolder,
                        varXControl,
                        DEFAULT_ROUNDING_LIMIT,
                        useHex,
                        DEFAULT_USE_CHECKBOX,
                        coordinate);

                case VarXSubclass.Angle:
                    return new VarXAngle(addressHolder, varXControl);

                case VarXSubclass.Object:
                    return new VarXObject(addressHolder, varXControl);

                case VarXSubclass.Boolean:
                    return new VarXBoolean(addressHolder, varXControl, invertBool);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected VarX(AddressHolder addressHolder, VarXControl varXControl, bool useCheckbox = false)
        {
            _addressHolder = addressHolder;
            _varXControl = varXControl;

            _startsAsCheckbox = useCheckbox;
            _contextMenuStrip = new ContextMenuStrip();
            AddContextMenuStripItems();
        }

        public bool StartsAsCheckbox()
        {
            return _startsAsCheckbox;
        }

        public ContextMenuStrip GetContextMenuStrip()
        {
            return _contextMenuStrip;
        }

        protected void AddContextMenuStripItems()
        {
            ToolStripMenuItem itemHighlight = new ToolStripMenuItem("Highlight");
            itemHighlight.Click += (sender, e) =>
            {
                _varXControl.ShowBorder = !_varXControl.ShowBorder;
                itemHighlight.Checked = _varXControl.ShowBorder;
            };
            itemHighlight.Checked = _varXControl.ShowBorder;

            _itemLock = new ToolStripMenuItem("Lock");
            _itemLock.Click += (sender, e) =>
            {
                if (VarXLockManager.ContainsLocksBool(_addressHolder))
                {
                    VarXLockManager.RemoveLocks(_addressHolder);
                }
                else
                {
                    VarXLockManager.AddLocks(_addressHolder);
                }
            };

            _itemRemoveAllLocks = new ToolStripMenuItem("Remove All Locks");
            _itemRemoveAllLocks.Click += (sender, e) => { VarXLockManager.RemoveAllLocks(); };

            ToolStripMenuItem itemEdit = new ToolStripMenuItem("Edit");
            itemEdit.Click += (sender, e) => { _varXControl.EditMode = true; };

            ToolStripMenuItem itemCopyAsIs = new ToolStripMenuItem("Copy (As Is)");
            itemCopyAsIs.Click += (sender, e) => { Clipboard.SetText(_varXControl.TextBoxValue); };

            ToolStripMenuItem itemCopyUnrounded = new ToolStripMenuItem("Copy (Unrounded)");
            itemCopyUnrounded.Click += (sender, e) => { Clipboard.SetText(GetValueForTextbox(false)); };

            ToolStripMenuItem itemPaste = new ToolStripMenuItem("Paste");
            itemPaste.Click += (sender, e) => { SetValueFromTextbox(Clipboard.GetText()); };

            _contextMenuStrip.Items.Add(itemHighlight);
            _contextMenuStrip.Items.Add(_itemLock);
            _contextMenuStrip.Items.Add(_itemRemoveAllLocks);
            _contextMenuStrip.Items.Add(itemEdit);
            _contextMenuStrip.Items.Add(itemCopyAsIs);
            _contextMenuStrip.Items.Add(itemCopyUnrounded);
            _contextMenuStrip.Items.Add(itemPaste);
        }

        public void ShowVarInfo()
        {
            VariableViewerForm varInfo;
            varInfo = new VariableViewerForm(
                _varXControl.VarName,
                _addressHolder.GetTypeDescription(),
                _addressHolder.GetRamAddressString(),
                _addressHolder.GetProcessAddressString());
            varInfo.ShowDialog();
        }

        public CheckState GetLockedCheckState()
        {
            return VarXLockManager.ContainsLocksCheckState(_addressHolder);
        }

        public bool GetLockedBool()
        {
            return VarXLockManager.ContainsLocksBool(_addressHolder);
        }

        public void UpdateItemCheckStates()
        {
            _itemLock.Checked = GetLockedBool();
            _itemRemoveAllLocks.Visible = VarXLockManager.ContainsAnyLocks();
        }



        public string GetValueForTextbox(
            bool handleRounding = true,
            bool handleFormatting = true)
        {
            List<string> values = _addressHolder.GetValues();
            (bool meaningfulValue, string value) = CombineValues(values);
            if (!meaningfulValue) return value;

            value = HandleAngleConverting(value);
            if (handleRounding) value = HandleRounding(value);
            value = HandleAngleRoundingOut(value);
            value = HandleNegating(value);
            if (handleFormatting) value = HandleHexDisplaying(value);
            if (handleFormatting) value = HandleObjectDisplaying(value);

            return value;
        }

        public bool SetValueFromTextbox(string value)
        {
            value = HandleObjectUndisplaying(value);
            value = HandleHexUndisplaying(value);
            value = HandleUnnegating(value);
            value = HandleAngleUnconverting(value);

            bool success = _addressHolder.SetValue(value);
            if (success && GetLockedBool()) VarXLockManager.UpdateLockValues(_addressHolder, value);
            return success;
        }

        public CheckState GetValueForCheckbox()
        {
            List<string> values = _addressHolder.GetValues();
            List<CheckState> checkStates = values.ConvertAll(value => ConvertValueToCheckState(value));
            CheckState checkState = CombineCheckStates(checkStates);
            return checkState;
        }

        public bool SetValueFromCheckbox(CheckState checkState)
        {
            string value = ConvertCheckStateToValue(checkState);
            bool success = _addressHolder.SetValue(value);
            if (success && GetLockedBool()) VarXLockManager.UpdateLockValues(_addressHolder, value);
            return success;
        }



        protected (bool meaningfulValue, string stringValue) CombineValues(List<string> values)
        {
            if (values.Count == 0) return (false, "(none)");
            string firstValue = values[0];
            for (int i = 1; i < values.Count; i++)
            {
                if (values[i] != firstValue) return (false, "(multiple values)");
            }
            return (true, firstValue);
        }

        protected CheckState CombineCheckStates(List<CheckState> checkStates)
        {
            if (checkStates.Count == 0) return CheckState.Unchecked;
            CheckState firstCheckState = checkStates[0];
            for (int i = 1; i < checkStates.Count; i++)
            {
                if (checkStates[i] != firstCheckState) return CheckState.Indeterminate;
            }
            return firstCheckState;
        }



        // Number methods

        protected virtual string HandleRounding(string value)
        {
            return value;
        }

        protected virtual string HandleNegating(string value)
        {
            return value;
        }

        protected virtual string HandleUnnegating(string value)
        {
            return value;
        }

        protected virtual string HandleHexDisplaying(string value)
        {
            return value;
        }

        protected virtual string HandleHexUndisplaying(string value)
        {
            return value;
        }

        // Angle methods

        protected virtual string HandleAngleConverting(string value)
        {
            return value;
        }

        protected virtual string HandleAngleUnconverting(string value)
        {
            return value;
        }

        protected virtual string HandleAngleRoundingOut(string value)
        {
            return value;
        }

        // Object methods

        protected virtual string HandleObjectDisplaying(string value)
        {
            return value;
        }

        protected virtual string HandleObjectUndisplaying(string value)
        {
            return value;
        }

        // Boolean methods

        protected virtual CheckState ConvertValueToCheckState(string value)
        {
            return CheckState.Unchecked;
        }

        protected virtual string ConvertCheckStateToValue(CheckState checkState)
        {
            return "";
        }


    }
}

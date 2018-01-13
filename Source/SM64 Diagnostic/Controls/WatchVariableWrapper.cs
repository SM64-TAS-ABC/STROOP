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
    public class WatchVariableWrapper
    {
        protected const int DEFAULT_ROUNDING_LIMIT = 3;
        protected const bool DEFAULT_DISPLAY_AS_HEX = false;
        protected const bool DEFAULT_USE_CHECKBOX = false;

        protected readonly WatchVariable _watchVar;
        protected readonly WatchVariableControl _watchVarControl;
        protected readonly BetterContextMenuStrip _contextMenuStrip;

        private ToolStripMenuItem _itemLock;
        private ToolStripMenuItem _itemRemoveAllLocks;

        private readonly bool _startsAsCheckbox;

        public static WatchVariableWrapper CreateWatchVariableWrapper(
            WatchVariable watchVar,
            WatchVariableControl watchVarControl,
            WatchVariableSubclass subclass,
            bool? useHex,
            bool? invertBool,
            WatchVariableCoordinate? coordinate)
        {
            switch (subclass)
            {
                case WatchVariableSubclass.String:
                    return new WatchVariableWrapper(watchVar, watchVarControl);

                case WatchVariableSubclass.Number:
                    return new WatchVariableNumberWrapper(
                        watchVar,
                        watchVarControl,
                        DEFAULT_ROUNDING_LIMIT,
                        useHex,
                        DEFAULT_USE_CHECKBOX,
                        coordinate);

                case WatchVariableSubclass.Angle:
                    return new WatchVariableAngleWrapper(watchVar, watchVarControl);

                case WatchVariableSubclass.Object:
                    return new WatchVariableObjectWrapper(watchVar, watchVarControl);

                case WatchVariableSubclass.Boolean:
                    return new WatchVariableBooleanWrapper(watchVar, watchVarControl, invertBool);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected WatchVariableWrapper(WatchVariable watchVar, WatchVariableControl watchVarControl, bool useCheckbox = false)
        {
            _watchVar = watchVar;
            _watchVarControl = watchVarControl;

            _startsAsCheckbox = useCheckbox;
            _contextMenuStrip = new BetterContextMenuStrip();
            AddContextMenuStripItems();
            AddExternalContextMenuStripItems();
        }

        public bool StartsAsCheckbox()
        {
            return _startsAsCheckbox;
        }

        public ContextMenuStrip GetContextMenuStrip()
        {
            return _contextMenuStrip;
        }

        private void AddContextMenuStripItems()
        {
            ToolStripMenuItem itemHighlight = new ToolStripMenuItem("Highlight");
            itemHighlight.Click += (sender, e) =>
            {
                _watchVarControl.ShowBorder = !_watchVarControl.ShowBorder;
                itemHighlight.Checked = _watchVarControl.ShowBorder;
            };
            itemHighlight.Checked = _watchVarControl.ShowBorder;

            _itemLock = new ToolStripMenuItem("Lock");
            _itemLock.Click += (sender, e) =>
            {
                if (WatchVariableLockManager.ContainsLocksBool(_watchVar))
                {
                    WatchVariableLockManager.RemoveLocks(_watchVar);
                }
                else
                {
                    WatchVariableLockManager.AddLocks(_watchVar);
                }
            };

            _itemRemoveAllLocks = new ToolStripMenuItem("Remove All Locks");
            _itemRemoveAllLocks.Click += (sender, e) => { WatchVariableLockManager.RemoveAllLocks(); };

            ToolStripMenuItem itemEdit = new ToolStripMenuItem("Edit");
            itemEdit.Click += (sender, e) => { _watchVarControl.EditMode = true; };

            ToolStripMenuItem itemCopyAsIs = new ToolStripMenuItem("Copy (As Is)");
            itemCopyAsIs.Click += (sender, e) => { Clipboard.SetText(_watchVarControl.TextBoxValue); };

            ToolStripMenuItem itemCopyUnrounded = new ToolStripMenuItem("Copy (Unrounded)");
            itemCopyUnrounded.Click += (sender, e) => { Clipboard.SetText(GetStringValue(false)); };

            ToolStripMenuItem itemPaste = new ToolStripMenuItem("Paste");
            itemPaste.Click += (sender, e) => { SetStringValue(Clipboard.GetText()); };

            _contextMenuStrip.AddToBeginningList(itemHighlight);
            _contextMenuStrip.AddToBeginningList(_itemLock);
            _contextMenuStrip.AddToBeginningList(_itemRemoveAllLocks);
            _contextMenuStrip.AddToBeginningList(itemEdit);
            _contextMenuStrip.AddToBeginningList(itemCopyAsIs);
            _contextMenuStrip.AddToBeginningList(itemCopyUnrounded);
            _contextMenuStrip.AddToBeginningList(itemPaste);
        }

        private void AddExternalContextMenuStripItems()
        {
            ToolStripMenuItem itemAddToCustomTab = new ToolStripMenuItem("Add to Custom Tab");
            itemAddToCustomTab.Click += (sender, e) => { CustomManager.Instance.AddVariable(_watchVarControl.CreateCopy()); };

            ToolStripMenuItem itemOpenController = new ToolStripMenuItem("Open Controller");
            itemOpenController.Click += (sender, e) => { ShowVarController(); };

            _contextMenuStrip.AddToEndingList(new ToolStripSeparator());
            _contextMenuStrip.AddToEndingList(itemAddToCustomTab);
            _contextMenuStrip.AddToEndingList(itemOpenController);
        }

        public void ShowVarInfo()
        {
            VariableViewerForm varInfo =
                new VariableViewerForm(
                    _watchVarControl.VarName,
                    _watchVar.GetTypeDescription(),
                    _watchVar.GetRamAddressString(),
                    _watchVar.GetProcessAddressString());
            varInfo.ShowDialog();
        }

        public void ShowVarController()
        {
            VariableControllerForm varController =
                new VariableControllerForm(
                    _watchVarControl.VarName,
                    this);
            varController.Show();
        }

        public CheckState GetLockedCheckState()
        {
            return WatchVariableLockManager.ContainsLocksCheckState(_watchVar);
        }

        public bool GetLockedBool()
        {
            return WatchVariableLockManager.ContainsLocksBool(_watchVar);
        }

        public void UpdateItemCheckStates()
        {
            _itemLock.Checked = GetLockedBool();
            _itemRemoveAllLocks.Visible = WatchVariableLockManager.ContainsAnyLocks();
        }



        public string GetStringValue(
            bool handleRounding = true,
            bool handleFormatting = true,
            List<uint> addresses = null)
        {
            List<string> values = _watchVar.GetValues(addresses);
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

        public bool SetStringValue(string value, List<uint> addresses = null)
        {
            value = HandleObjectUndisplaying(value);
            value = HandleHexUndisplaying(value);
            value = HandleUnnegating(value);
            value = HandleAngleUnconverting(value);

            bool success = _watchVar.SetValue(value, addresses);
            if (success && GetLockedBool()) WatchVariableLockManager.UpdateLockValues(_watchVar, value);
            return success;
        }

        public CheckState GetCheckStateValue(List<uint> addresses = null)
        {
            List<string> values = _watchVar.GetValues(addresses);
            List<CheckState> checkStates = values.ConvertAll(value => ConvertValueToCheckState(value));
            CheckState checkState = CombineCheckStates(checkStates);
            return checkState;
        }

        public bool SetCheckStateValue(CheckState checkState, List<uint> addresses = null)
        {
            string value = ConvertCheckStateToValue(checkState);
            bool success = _watchVar.SetValue(value, addresses);
            if (success && GetLockedBool()) WatchVariableLockManager.UpdateLockValues(_watchVar, value);
            return success;
        }




        public bool AddValue(string stringValue, bool add, List<uint> addresses = null)
        {
            double? doubleValueNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
            if (!doubleValueNullable.HasValue) return false;
            double doubleValue = doubleValueNullable.Value;

            string currentValueString = GetStringValue(false, false, addresses);
            double? currentValueNullable = ParsingUtilities.ParseDoubleNullable(currentValueString);
            if (!currentValueNullable.HasValue) return false;
            double currentValue = currentValueNullable.Value;

            double newValue = currentValue + doubleValue * (add ? +1 : -1);
            return SetStringValue(newValue.ToString(), addresses);
        }

        public List<uint> GetCurrentAddresses()
        {
            return _watchVar.AddressList;
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

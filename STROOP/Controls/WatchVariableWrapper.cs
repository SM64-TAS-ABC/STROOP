using STROOP.Extensions;
using STROOP.Forms;
using STROOP.Managers;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace STROOP.Controls
{
    public abstract class WatchVariableWrapper
    {
        // Defaults
        protected const int DEFAULT_ROUNDING_LIMIT = 3;
        protected const bool DEFAULT_DISPLAY_AS_HEX = false;
        protected const bool DEFAULT_USE_CHECKBOX = false;

        // Main objects
        protected readonly WatchVariable _watchVar;
        protected readonly WatchVariableControl _watchVarControl;
        protected readonly BetterContextMenuStrip _contextMenuStrip;

        // Main items
        private ToolStripMenuItem _itemHighlight;
        private ToolStripMenuItem _itemLock;
        private ToolStripMenuItem _itemRemoveAllLocks;

        // Custom items
        private ToolStripSeparator _separatorCustom;
        private ToolStripMenuItem _itemFixAddress;
        private ToolStripMenuItem _itemRename;
        private ToolStripMenuItem _itemDelete;

        // Fields
        private readonly bool _startsAsCheckbox;

        public static WatchVariableWrapper CreateWatchVariableWrapper(
            WatchVariable watchVar,
            WatchVariableControl watchVarControl,
            WatchVariableSubclass subclass,
            int? roundingLimit,
            bool? useHex,
            bool? invertBool,
            WatchVariableCoordinate? coordinate)
        {
            switch (subclass)
            {
                case WatchVariableSubclass.String:
                    return new WatchVariableStringWrapper(watchVar, watchVarControl);

                case WatchVariableSubclass.Number:
                    return new WatchVariableNumberWrapper(
                        watchVar,
                        watchVarControl,
                        roundingLimit,
                        useHex,
                        DEFAULT_USE_CHECKBOX,
                        coordinate);

                case WatchVariableSubclass.Angle:
                    return new WatchVariableAngleWrapper(watchVar, watchVarControl);

                case WatchVariableSubclass.Object:
                    return new WatchVariableObjectWrapper(watchVar, watchVarControl);

                case WatchVariableSubclass.Triangle:
                    return new WatchVariableTriangleWrapper(watchVar, watchVarControl);

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
            AddCustomContextMenuStripItems();
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
            _itemHighlight = new ToolStripMenuItem("Highlight");
            _itemHighlight.Click += (sender, e) => _watchVarControl.ToggleHighlighted();
            _itemHighlight.Checked = _watchVarControl.Highlighted;

            _itemLock = new ToolStripMenuItem("Lock");
            _itemLock.Click += (sender, e) => ToggleLocked(_watchVarControl.FixedAddressList);

            _itemRemoveAllLocks = new ToolStripMenuItem("Remove All Locks");
            _itemRemoveAllLocks.Click += (sender, e) => WatchVariableLockManager.RemoveAllLocks();

            ToolStripMenuItem itemCopyUnrounded = new ToolStripMenuItem("Copy");
            itemCopyUnrounded.Click += (sender, e) => Clipboard.SetText(GetValue(false).ToString());

            ToolStripMenuItem itemPaste = new ToolStripMenuItem("Paste");
            itemPaste.Click += (sender, e) => _watchVarControl.SetValue(Clipboard.GetText());

            _contextMenuStrip.AddToBeginningList(_itemHighlight);
            _contextMenuStrip.AddToBeginningList(_itemLock);
            _contextMenuStrip.AddToBeginningList(_itemRemoveAllLocks);
            _contextMenuStrip.AddToBeginningList(itemCopyUnrounded);
            _contextMenuStrip.AddToBeginningList(itemPaste);
        }

        private void AddExternalContextMenuStripItems()
        {
            ToolStripMenuItem itemPanelOptions = new ToolStripMenuItem("Panel Options");
            itemPanelOptions.Click += (sender, e) =>
            {
                _watchVarControl.OpenPanelOptions(_contextMenuStrip.Bounds.Location);
            };

            ToolStripMenuItem itemOpenController = new ToolStripMenuItem("Open Controller");
            itemOpenController.Click += (sender, e) => ShowVarController();

            ToolStripMenuItem itemAddToCustomTab = new ToolStripMenuItem("Add to Custom Tab");
            itemAddToCustomTab.Click += (sender, e) =>
                _watchVarControl.AddToTab(Config.CustomManager, false, false);

            _contextMenuStrip.AddToEndingList(new ToolStripSeparator());
            _contextMenuStrip.AddToEndingList(itemPanelOptions);
            _contextMenuStrip.AddToEndingList(itemOpenController);
            _contextMenuStrip.AddToEndingList(itemAddToCustomTab);
        }

        private void AddCustomContextMenuStripItems()
        {
            _separatorCustom = new ToolStripSeparator();
            _separatorCustom.Visible = false;

            _itemFixAddress = new ToolStripMenuItem("Fix Address");
            _itemFixAddress.Click += (sender, e) =>
            {
                _watchVarControl.ToggleFixedAddress();
                _itemFixAddress.Checked = _watchVarControl.FixedAddressList != null;
            };
            _itemFixAddress.Visible = false;

            _itemRename = new ToolStripMenuItem("Rename");
            _itemRename.Click += (sender, e) => { _watchVarControl.RenameMode = true; };
            _itemRename.Visible = false;

            _itemDelete = new ToolStripMenuItem("Delete");
            _itemDelete.Click += (sender, e) => { _watchVarControl.DeleteFromPanel(); };
            _itemDelete.Visible = false;

            _contextMenuStrip.AddToEndingList(_separatorCustom);
            _contextMenuStrip.AddToEndingList(_itemFixAddress);
            _contextMenuStrip.AddToEndingList(_itemRename);
            _contextMenuStrip.AddToEndingList(_itemDelete);
        }

        public void ShowVarInfo()
        {
            VariableViewerForm varInfo =
                new VariableViewerForm(
                    _watchVarControl.VarName,
                    _watchVar.GetTypeDescription(),
                    _watchVar.GetBaseOffsetDescription(),
                    _watchVar.GetRamAddressString(true, _watchVarControl.FixedAddressList),
                    _watchVar.GetProcessAddressString(_watchVarControl.FixedAddressList));
            varInfo.Show();
        }

        public List<string> GetVarInfo()
        {
            return new List<string>()
            {
                _watchVarControl.VarName,
                _watchVar.GetTypeDescription(),
                _watchVar.GetBaseOffsetDescription(),
                _watchVar.GetRamAddressString(true, _watchVarControl.FixedAddressList),
                _watchVar.GetProcessAddressString(_watchVarControl.FixedAddressList),
            };
        }

        public static List<string> GetVarInfoLabels()
        {
            return new List<string>()
            {
                "Name",
                "Type",
                "Base + Offset",
                "N64 Address",
                "Emulator Address",
            };
        }

        public void ShowVarController()
        {
            VariableControllerForm varController =
                new VariableControllerForm(
                    _watchVarControl.VarName,
                    this,
                    _watchVarControl.FixedAddressList);
            varController.Show();
        }

        public CheckState GetLockedCheckState(List<uint> addresses = null)
        {
            return WatchVariableLockManager.ContainsLocksCheckState(_watchVar, addresses);
        }

        public bool GetLockedBool(List<uint> addresses = null)
        {
            return WatchVariableLockManager.ContainsLocksBool(_watchVar, addresses);
        }

        public void UpdateItemCheckStates(List<uint> addresses = null)
        {
            _itemHighlight.Checked = _watchVarControl.Highlighted;
            _itemLock.Checked = GetLockedBool(addresses);
            _itemRemoveAllLocks.Visible = WatchVariableLockManager.ContainsAnyLocks();
            _itemFixAddress.Checked = _watchVarControl.FixedAddressList != null;
        }

        public void ToggleLocked(List<uint> addresses = null)
        {
            if (WatchVariableLockManager.ContainsLocksBool(_watchVar, addresses))
            {
                WatchVariableLockManager.RemoveLocks(_watchVar, addresses);
            }
            else
            {
                WatchVariableLockManager.AddLocks(_watchVar, addresses);
            }
        }




        public object GetValue(
            bool handleRounding = true,
            bool handleFormatting = true,
            List<uint> addresses = null)
        {
            List<object> values = _watchVar.GetValues(addresses);
            (bool meaningfulValue, object value) = CombineValues(values);
            if (!meaningfulValue) return value;

            value = ConvertValue(value, handleRounding, handleFormatting);
            return value;
        }

        private object ConvertValue(
            object value,
            bool handleRounding = true,
            bool handleFormatting = true)
        {
            HandleVerification(value);
            value = HandleAngleConverting(value);
            value = HandleRounding(value, handleRounding);
            value = HandleAngleRoundingOut(value);
            if (handleFormatting) value = HandleHexDisplaying(value);
            if (handleFormatting) value = HandleObjectDisplaying(value);
            return value;
        }

        public bool SetValue(object value, List<uint> addresses = null)
        {
            value = UnconvertValue(value);
            bool success = _watchVar.SetValue(value, addresses);
            if (success && GetLockedBool(addresses))
                WatchVariableLockManager.UpdateLockValues(_watchVar, value, addresses);
            return success;
        }

        public object UnconvertValue(object value)
        {
            value = HandleObjectUndisplaying(value);
            value = HandleHexUndisplaying(value);
            value = HandleAngleUnconverting(value);
            return value;
        }

        public CheckState GetCheckStateValue(List<uint> addresses = null)
        {
            List<object> values = _watchVar.GetValues(addresses);
            List<CheckState> checkStates = values.ConvertAll(value => ConvertValueToCheckState(value));
            CheckState checkState = CombineCheckStates(checkStates);
            return checkState;
        }

        public bool SetCheckStateValue(CheckState checkState, List<uint> addresses = null)
        {
            object value = ConvertCheckStateToValue(checkState);
            bool success = _watchVar.SetValue(value, addresses);
            if (success && GetLockedBool(addresses))
                WatchVariableLockManager.UpdateLockValues(_watchVar, value, addresses);
            return success;
        }

        public bool AddValue(object objectValue, bool add, List<uint> addresses = null)
        {
            double? changeValueNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
            if (!changeValueNullable.HasValue) return false;
            double changeValue = changeValueNullable.Value;

            List<object> currentValues = _watchVar.GetValues(addresses);
            List<double?> currentValuesDoubleNullable =
                currentValues.ConvertAll(
                    currentValue => ParsingUtilities.ParseDoubleNullable(currentValue));
            List<object> newValues = currentValuesDoubleNullable.ConvertAll(currentValueDoubleNullable =>
            {
                if (!currentValueDoubleNullable.HasValue) return null;
                double currentValueDouble = currentValueDoubleNullable.Value;
                object convertedValue = ConvertValue(currentValueDouble, false, false);
                // TODO tyler fix this for float logic
                double convertedValueDouble = ParsingUtilities.ParseDouble(convertedValue);
                double modifiedValue = convertedValueDouble + changeValue * (add ? +1 : -1);
                object unconvertedValue = UnconvertValue(modifiedValue);
                return unconvertedValue;
            });

            bool success = _watchVar.SetValues(newValues, addresses);
            if (success && GetLockedBool(addresses))
                WatchVariableLockManager.UpdateLockValues(_watchVar, newValues, addresses);
            return success;
        }

        public List<uint> GetCurrentAddresses()
        {
            return _watchVar.AddressList;
        }

        public void EnableCustomFunctionality()
        {
            _separatorCustom.Visible = true;
            _itemFixAddress.Visible = true;
            _itemRename.Visible = true;
            _itemDelete.Visible = true;
        }

        public void AddToVarHackTab(List<uint> addresses = null)
        {
            if (_watchVar.BaseAddressType == BaseAddressTypeEnum.Triangle)
            {
                Config.VarHackManager.AddVariable(
                    _watchVarControl.VarName + " ",
                    Config.TriangleManager.TrianglePointerAddress,
                    _watchVar.MemoryType,
                    GetUseHex(),
                    _watchVar.Offset);
            }
            else
            {
                List<uint> addressList = addresses ?? _watchVar.AddressList;
                for (int i = 0; i < addressList.Count; i++)
                {
                    string indexSuffix = addressList.Count > 1 ? (i + 1).ToString() : "";
                    Config.VarHackManager.AddVariable(
                        _watchVarControl.VarName + indexSuffix + " ",
                        addressList[i],
                        _watchVar.MemoryType,
                        GetUseHex(),
                        null);
                }
            }
        }





        protected (bool meaningfulValue, object value) CombineValues(List<object> values)
        {
            if (values.Count == 0) return (false, "(none)");
            object firstValue = values[0];
            for (int i = 1; i < values.Count; i++)
            {
                if (!Object.Equals(values[i], firstValue)) return (false, "(multiple values)");
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



        // Generic methods

        protected virtual void HandleVerification(object value)
        {
            if (value == null)
                throw new ArgumentOutOfRangeException("value cannot be null");
        }

        // Number methods

        protected virtual object HandleRounding(object value, bool handleRounding)
        {
            return value;
        }

        protected virtual object HandleHexDisplaying(object value)
        {
            return value;
        }

        protected virtual object HandleHexUndisplaying(object value)
        {
            return value;
        }

        // Angle methods

        protected virtual object HandleAngleConverting(object value)
        {
            return value;
        }

        protected virtual object HandleAngleUnconverting(object value)
        {
            return value;
        }

        protected virtual object HandleAngleRoundingOut(object value)
        {
            return value;
        }

        // Object methods

        protected virtual object HandleObjectDisplaying(object value)
        {
            return value;
        }

        protected virtual object HandleObjectUndisplaying(object value)
        {
            return value;
        }

        // Boolean methods

        protected virtual CheckState ConvertValueToCheckState(object value)
        {
            return CheckState.Unchecked;
        }

        protected virtual object ConvertCheckStateToValue(CheckState checkState)
        {
            return "";
        }



        // Virtual methods

        protected virtual bool GetUseHex()
        {
            return false;
        }

        public virtual void ApplySettings(WatchVariableControlSettings settings)
        {

        }
    }
}

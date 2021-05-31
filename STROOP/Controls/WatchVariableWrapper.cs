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
        protected const Type DEFAULT_DISPLAY_TYPE = null;
        protected const int DEFAULT_ROUNDING_LIMIT = 3;
        protected const bool DEFAULT_DISPLAY_AS_HEX = false;
        protected const bool DEFAULT_USE_CHECKBOX = false;
        protected const bool DEFAULT_IS_YAW = false;

        // Main objects
        public readonly WatchVariable WatchVar;
        protected readonly WatchVariableControl _watchVarControl;
        protected readonly BetterContextMenuStrip _contextMenuStrip;

        // Main items
        private ToolStripMenuItem _itemHighlight;
        private ToolStripMenuItem _itemLock;

        // Custom items
        private ToolStripSeparator _separatorCustom;
        private ToolStripMenuItem _itemFixAddress;
        private ToolStripMenuItem _itemRename;
        private ToolStripMenuItem _itemRemove;

        // Fields
        private readonly bool _startsAsCheckbox;

        public static WatchVariableWrapper CreateWatchVariableWrapper(
            WatchVariable watchVar,
            WatchVariableControl watchVarControl,
            WatchVariableSubclass subclass,
            Type displayType,
            int? roundingLimit,
            bool? useHex,
            bool? invertBool,
            bool? isYaw,
            Coordinate? coordinate)
        {
            switch (subclass)
            {
                case WatchVariableSubclass.String:
                    return new WatchVariableStringWrapper(watchVar, watchVarControl);

                case WatchVariableSubclass.Number:
                    return new WatchVariableNumberWrapper(
                        watchVar,
                        watchVarControl,
                        displayType,
                        roundingLimit,
                        useHex,
                        DEFAULT_USE_CHECKBOX,
                        coordinate);

                case WatchVariableSubclass.Angle:
                    return new WatchVariableAngleWrapper(watchVar, watchVarControl, displayType, isYaw);

                case WatchVariableSubclass.Object:
                    return new WatchVariableObjectWrapper(watchVar, watchVarControl);

                case WatchVariableSubclass.Triangle:
                    return new WatchVariableTriangleWrapper(watchVar, watchVarControl);

                case WatchVariableSubclass.Address:
                    return new WatchVariableAddressWrapper(watchVar, watchVarControl);

                case WatchVariableSubclass.Boolean:
                    return new WatchVariableBooleanWrapper(watchVar, watchVarControl, invertBool);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected WatchVariableWrapper(WatchVariable watchVar, WatchVariableControl watchVarControl, bool useCheckbox = false)
        {
            WatchVar = watchVar;
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
            _itemLock.Click += (sender, e) => ToggleLocked(null, _watchVarControl.FixedAddressListGetter());

            ToolStripMenuItem itemCopyUnrounded = new ToolStripMenuItem("Copy");
            itemCopyUnrounded.Click += (sender, e) =>
            {
                Clipboard.SetText(GetValue(false, true, _watchVarControl.FixedAddressListGetter()).ToString());
                _watchVarControl.FlashColor(WatchVariableControl.COPY_COLOR);
            };

            ToolStripMenuItem itemPaste = new ToolStripMenuItem("Paste");
            itemPaste.Click += (sender, e) => _watchVarControl.SetValue(Clipboard.GetText());

            _contextMenuStrip.AddToBeginningList(_itemHighlight);
            _contextMenuStrip.AddToBeginningList(_itemLock);
            _contextMenuStrip.AddToBeginningList(itemCopyUnrounded);
            _contextMenuStrip.AddToBeginningList(itemPaste);
        }

        private void AddExternalContextMenuStripItems()
        {
            ToolStripMenuItem itemOpenController = new ToolStripMenuItem("Open Controller");
            itemOpenController.Click += (sender, e) => ShowControllerForm();

            ToolStripMenuItem itemOpenBitController = new ToolStripMenuItem("Open Bit Controller");
            itemOpenBitController.Click += (sender, e) => ShowBitForm();

            ToolStripMenuItem itemAddToCustomTab = new ToolStripMenuItem("Add to Custom Tab");
            itemAddToCustomTab.Click += (sender, e) =>
                _watchVarControl.AddToTab(Config.CustomManager);

            _contextMenuStrip.AddToEndingList(new ToolStripSeparator());
            _contextMenuStrip.AddToEndingList(itemOpenController);
            _contextMenuStrip.AddToEndingList(itemOpenBitController);
            _contextMenuStrip.AddToEndingList(itemAddToCustomTab);
        }

        private void AddCustomContextMenuStripItems()
        {
            _separatorCustom = new ToolStripSeparator();

            _itemFixAddress = new ToolStripMenuItem("Fix Address");
            _itemFixAddress.Click += (sender, e) =>
            {
                _watchVarControl.ToggleFixedAddress();
                _itemFixAddress.Checked = _watchVarControl.FixedAddressListGetter() != null;
            };

            _itemRename = new ToolStripMenuItem("Rename");
            _itemRename.Click += (sender, e) => { _watchVarControl.RenameMode = true; };

            _itemRemove = new ToolStripMenuItem("Remove");
            _itemRemove.Click += (sender, e) => { _watchVarControl.RemoveFromPanel(); };

            _contextMenuStrip.AddToEndingList(_separatorCustom);
            _contextMenuStrip.AddToEndingList(_itemFixAddress);
            _contextMenuStrip.AddToEndingList(_itemRename);
            _contextMenuStrip.AddToEndingList(_itemRemove);
        }

        public void ShowVarInfo()
        {
            VariableViewerForm varInfo =
                new VariableViewerForm(
                    name: _watchVarControl.VarName,
                    clazz: GetClass(),
                    type: WatchVar.GetTypeDescription(),
                    baseTypeOffset: WatchVar.GetBaseTypeOffsetDescription(),
                    n64BaseAddress: WatchVar.GetBaseAddressListString(_watchVarControl.FixedAddressListGetter()),
                    emulatorBaseAddress: WatchVar.GetProcessBaseAddressListString(_watchVarControl.FixedAddressListGetter()),
                    n64Address: WatchVar.GetRamAddressListString(true, _watchVarControl.FixedAddressListGetter()),
                    emulatorAddress: WatchVar.GetProcessAddressListString(_watchVarControl.FixedAddressListGetter()));
            varInfo.Show();
        }

        public List<string> GetVarInfo()
        {
            return new List<string>()
            {
                _watchVarControl.VarName,
                GetClass(),
                WatchVar.GetTypeDescription(),
                WatchVar.GetBaseTypeOffsetDescription(),
                WatchVar.GetBaseAddressListString(_watchVarControl.FixedAddressListGetter()),
                WatchVar.GetProcessBaseAddressListString(_watchVarControl.FixedAddressListGetter()),
                WatchVar.GetRamAddressListString(true, _watchVarControl.FixedAddressListGetter()),
                WatchVar.GetProcessAddressListString(_watchVarControl.FixedAddressListGetter()),
            };
        }

        public static List<string> GetVarInfoLabels()
        {
            return new List<string>()
            {
                "Name",
                "Class",
                "Type",
                "BaseType + Offset",
                "N64 Base Address",
                "Emulator Base Address",
                "N64 Address",
                "Emulator Address",
            };
        }

        public List<Func<object, bool>> GetSetters(List<uint> addresses = null)
        {
            return WatchVar.GetSetters(addresses);
        }

        public void ShowControllerForm()
        {
            VariableControllerForm varController =
                new VariableControllerForm(
                    _watchVarControl.VarName,
                    this,
                    _watchVarControl.FixedAddressListGetter());
            varController.Show();
        }

        public void ShowBitForm()
        {
            if (WatchVar.IsSpecial) return;
            VariableBitForm varController =
                new VariableBitForm(
                    _watchVarControl.VarName,
                    WatchVar,
                    _watchVarControl.FixedAddressListGetter());
            varController.Show();
        }

        public void ViewInMemoryTab()
        {
            if (WatchVar.IsSpecial) return;
            List<uint> addressList = WatchVar.GetAddressList(_watchVarControl.FixedAddressListGetter());
            if (addressList.Count == 0) return;
            uint address = addressList[0];
            Config.TabControlMain.SelectedTab = Config.TabControlMain.TabPages["tabPageMemory"];
            Config.MemoryManager.SetCustomAddress(address);
            Config.MemoryManager.UpdateHexDisplay();
        }

        public CheckState GetLockedCheckState(List<uint> addresses = null)
        {
            return Config.LockManager.ContainsLocksCheckState(WatchVar, addresses);
        }

        public bool GetLockedBool(List<uint> addresses = null)
        {
            return Config.LockManager.ContainsLocksBool(WatchVar, addresses);
        }

        public void UpdateItemCheckStates(List<uint> addresses = null)
        {
            _itemHighlight.Checked = _watchVarControl.Highlighted;
            _itemLock.Checked = GetLockedBool(addresses);
            _itemFixAddress.Checked = _watchVarControl.FixedAddressListGetter() != null;
        }

        public void ToggleLocked(bool? newLockedValueNullable, List<uint> addresses = null)
        {
            bool currentLockedValue = Config.LockManager.ContainsLocksBool(WatchVar, addresses);
            bool newLockedValue = newLockedValueNullable ?? !currentLockedValue;
            if (newLockedValue == currentLockedValue) return;

            if (newLockedValue)
            {
                Config.LockManager.AddLocks(WatchVar, addresses);
            }
            else
            {
                Config.LockManager.RemoveLocks(WatchVar, addresses);
            }
        }



        public Type GetMemoryType()
        {
            return WatchVar.MemoryType;
        }

        public List<uint> GetBaseAddresses(List<uint> addresses = null)
        {
            return addresses ?? WatchVar.GetBaseAddressList();
        }

        private List<object> GetVerifiedValues(List<uint> addresses = null)
        {
            List<object> values = WatchVar.GetValues(addresses);
            values.ForEach(value => HandleVerification(value));
            return values;
        }

        public List<object> GetValues(
            bool handleRounding = true,
            bool handleFormatting = true,
            List<uint> addresses = null)
        {
            List<object> values = GetVerifiedValues(addresses);
            values = values.ConvertAll(value => ConvertValue(value, handleRounding, handleFormatting));
            return values;
        }

        public object GetValue(
            bool handleRounding = true,
            bool handleFormatting = true,
            List<uint> addresses = null)
        {
            List<object> values = GetVerifiedValues(addresses);
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
            if (handleFormatting && GetUseHexExactly() && SavedSettingsConfig.DisplayAsHexUsesMemory)
            {
                return HandleHexDisplaying(value);
            }
            value = HandleAngleConverting(value);
            value = HandleRounding(value, handleRounding);
            value = HandleAngleRoundingOut(value);
            if (handleFormatting) value = HandleHexDisplaying(value);
            if (handleFormatting) value = HandleObjectDisplaying(value);
            return value;
        }

        public bool SetValues(List<object> values, List<uint> addresses = null)
        {
            values = values.ConvertAll(value => UnconvertValue(value));
            return WatchVar.SetValues(values, addresses);
        }

        public bool SetValue(object value, List<uint> addresses = null)
        {
            value = UnconvertValue(value);
            return WatchVar.SetValue(value, addresses);
        }

        public object UnconvertValue(object value)
        {
            value = HandleObjectUndisplaying(value);
            value = HandleHexUndisplaying(value);
            value = HandleAngleUnconverting(value);
            value = HandleNumberConversion(value);
            return value;
        }

        public CheckState GetCheckStateValue(List<uint> addresses = null)
        {
            List<object> values = GetVerifiedValues(addresses);
            List<CheckState> checkStates = values.ConvertAll(value => ConvertValueToCheckState(value));
            CheckState checkState = CombineCheckStates(checkStates);
            return checkState;
        }

        public bool SetCheckStateValue(CheckState checkState, List<uint> addresses = null)
        {
            object value = ConvertCheckStateToValue(checkState);
            return WatchVar.SetValue(value, addresses);
        }

        public bool AddValue(object objectValue, bool add, List<uint> addresses = null)
        {
            double? changeValueNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
            if (!changeValueNullable.HasValue) return false;
            double changeValue = changeValueNullable.Value;

            List<object> currentValues = GetVerifiedValues(addresses);
            List<object> convertedValues = currentValues.ConvertAll(
                currentValue => ConvertValue(currentValue, false, false));
            List<double?> convertedValuesDoubleNullable =
                convertedValues.ConvertAll(
                    convertedValue => ParsingUtilities.ParseDoubleNullable(convertedValue));
            List<object> newValues = convertedValuesDoubleNullable.ConvertAll(convertedValueDoubleNullable =>
            {
                if (!convertedValueDoubleNullable.HasValue) return null;
                double convertedValueDouble = convertedValueDoubleNullable.Value;
                double modifiedValue = convertedValueDouble + changeValue * (add ? +1 : -1);
                object unconvertedValue = UnconvertValue(modifiedValue);
                return unconvertedValue;
            });

            return WatchVar.SetValues(newValues, addresses);
        }

        public List<uint> GetCurrentAddressesToFix()
        {
            return new List<uint>(WatchVar.GetBaseAddressList());
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

        public abstract string GetClass();

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

        protected virtual object HandleNumberConversion(object value)
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

        public virtual bool GetUseHex()
        {
            return false;
        }

        protected virtual bool GetUseHexExactly()
        {
            return false;
        }

        public virtual void ApplySettings(WatchVariableControlSettings settings)
        {
            if (settings.ChangeLocked)
            {
                ToggleLocked(settings.NewLocked, _watchVarControl.FixedAddressListGetter());
            }
        }

        public virtual void ToggleDisplayAsHex(bool? displayAsHexNullable = null)
        {

        }
    }
}

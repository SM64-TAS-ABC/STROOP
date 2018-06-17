using STROOP.Controls;
using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace STROOP.Managers
{
    public class MemoryManager : DataManager
    {
        private readonly BetterTextbox _textBoxMemoryBaseAddress;
        private readonly BetterTextbox _textBoxMemoryMemorySize;

        private readonly CheckBox _checkBoxMemoryLittleEndian;
        private readonly CheckBox _checkBoxMemoryRelativeAddresses;
        private readonly CheckBox _checkBoxMemoryUpdateContinuously;
        private readonly CheckBox _checkBoxMemoryUseObjAddress;
        private readonly CheckBox _checkBoxMemoryHighlightObjVars;

        private readonly ComboBox _comboBoxMemoryTypes;
        private readonly CheckBox _checkBoxMemoryHex;
        private readonly CheckBox _checkBoxMemoryObj;

        private readonly Button _buttonMemoryMoveUpOnce;
        private readonly Button _buttonMemoryMoveDownOnce;
        private readonly Button _buttonMemoryMoveUpContinuously;
        private readonly Button _buttonMemoryMoveDownContinuously;

        private readonly RichTextBoxEx _richTextBoxMemoryAddresses;
        private readonly RichTextBoxEx _richTextBoxMemoryValues;

        private readonly List<ValueText> _currentValueTexts;
        private readonly List<WatchVariableControlPrecursor> _objectPrecursors;
        private readonly List<WatchVariableControlPrecursor> _objectSpecificPrecursors;
        private List<WatchVariableControlPrecursor> _memTabPrecursors
        {
            get
            {
                return _variablePanel.GetCurrentVariablePrecursors();
            }
        }

        private uint? _address;
        public uint? Address
        {
            get => _address;
            set
            {
                _address = value;
                RefreshAddressTextbox();
            }
        }

        private uint _memorySize;
        private uint MemorySize
        {
            get => _memorySize;
            set
            {
                _memorySize = value;
                RefreshMemorySizeTextbox();
            }
        }

        private BehaviorCriteria? _behavior;
        private BehaviorCriteria? Behavior
        {
            get
            {
                return _behavior;
            }
            set
            {
                if (value == _behavior) return;
                _behavior = value;
                _objectSpecificPrecursors.Clear();
                if (_behavior.HasValue)
                {
                    List<WatchVariableControlPrecursor> precursors =
                        Config.ObjectAssociations.GetWatchVarControls(_behavior.Value)
                            .ConvertAll(control => control.WatchVarPrecursor);
                    _objectSpecificPrecursors.AddRange(precursors);
                }
            }
        }

        public MemoryManager(TabPage tabControl, WatchVariableFlowLayoutPanel watchVariablePanel, string varFilePath)
            : base(null, watchVariablePanel)
        {
            // Initialize fields
            _address = null;
            _memorySize = ObjectConfig.StructSize;
            _behavior = null;

            _currentValueTexts = new List<ValueText>();
            _objectPrecursors = XmlConfigParser.OpenWatchVariableControlPrecursors(varFilePath);
            _objectSpecificPrecursors = new List<WatchVariableControlPrecursor>();

            // Get controls
            SplitContainer splitContainer = tabControl.Controls["splitContainerMemory"] as SplitContainer;
            SplitContainer splitContainerMemoryControls =
                splitContainer.Panel1.Controls["splitContainerMemoryControls"] as SplitContainer;
            SplitContainer splitContainerMemoryControlsDisplays =
                splitContainerMemoryControls.Panel2.Controls["splitContainerMemoryControlsDisplays"] as SplitContainer;

            _textBoxMemoryBaseAddress = splitContainerMemoryControls.Panel1.Controls["textBoxMemoryBaseAddress"] as BetterTextbox;
            _textBoxMemoryMemorySize = splitContainerMemoryControls.Panel1.Controls["textBoxMemoryMemorySize"] as BetterTextbox;
            
            _checkBoxMemoryLittleEndian = splitContainerMemoryControls.Panel1.Controls["checkBoxMemoryLittleEndian"] as CheckBox;
            _checkBoxMemoryRelativeAddresses = splitContainerMemoryControls.Panel1.Controls["checkBoxMemoryRelativeAddresses"] as CheckBox;
            _checkBoxMemoryUpdateContinuously = splitContainerMemoryControls.Panel1.Controls["checkBoxMemoryUpdateContinuously"] as CheckBox;
            _checkBoxMemoryUseObjAddress = splitContainerMemoryControls.Panel1.Controls["checkBoxMemoryUseObjAddress"] as CheckBox;
            _checkBoxMemoryHighlightObjVars = splitContainerMemoryControls.Panel1.Controls["checkBoxMemoryHighlightObjVars"] as CheckBox;

            _comboBoxMemoryTypes = splitContainerMemoryControls.Panel1.Controls["comboBoxMemoryTypes"] as ComboBox;
            _checkBoxMemoryHex = splitContainerMemoryControls.Panel1.Controls["checkBoxMemoryHex"] as CheckBox;
            _checkBoxMemoryObj = splitContainerMemoryControls.Panel1.Controls["checkBoxMemoryObj"] as CheckBox;

            _buttonMemoryMoveUpOnce = splitContainerMemoryControls.Panel1.Controls["buttonMemoryMoveUpOnce"] as Button;
            _buttonMemoryMoveDownOnce = splitContainerMemoryControls.Panel1.Controls["buttonMemoryMoveDownOnce"] as Button;
            _buttonMemoryMoveUpContinuously = splitContainerMemoryControls.Panel1.Controls["buttonMemoryMoveUpContinuously"] as Button;
            _buttonMemoryMoveDownContinuously = splitContainerMemoryControls.Panel1.Controls["buttonMemoryMoveDownContinuously"] as Button;

            _richTextBoxMemoryAddresses = splitContainerMemoryControlsDisplays.Panel1.Controls["richTextBoxMemoryAddresses"] as RichTextBoxEx;
            _richTextBoxMemoryValues = splitContainerMemoryControlsDisplays.Panel2.Controls["richTextBoxMemoryValues"] as RichTextBoxEx;

            // Set up controls
            _comboBoxMemoryTypes.DataSource = TypeUtilities.SimpleTypeList;

            _checkBoxMemoryLittleEndian.Click += (sender, e) => UpdateHexDisplay();
            _comboBoxMemoryTypes.SelectedValueChanged += (sender, e) => UpdateHexDisplay();

            _richTextBoxMemoryValues.Click += (sender, e) => MemoryValueClick();

            _textBoxMemoryBaseAddress.AddEnterAction(() =>
                SetCustomAddress(ParsingUtilities.ParseHexNullable(_textBoxMemoryBaseAddress.Text)));
            _textBoxMemoryMemorySize.AddEnterAction(() =>
                SetCustomMemorySize(ParsingUtilities.ParseHexNullable(_textBoxMemoryMemorySize.Text)));

            _buttonMemoryMoveUpOnce.Click += (sender, e) => ScrollMemory(-1);
            _buttonMemoryMoveDownOnce.Click += (sender, e) => ScrollMemory(1);

            int scrollSpeed = 60;

            Timer moveUpContinuouslyTimer = new Timer { Interval = scrollSpeed };
            moveUpContinuouslyTimer.Tick += (s, e) => ScrollMemory(-1);
            _buttonMemoryMoveUpContinuously.MouseDown += (sender, e) => moveUpContinuouslyTimer.Start();
            _buttonMemoryMoveUpContinuously.MouseUp += (sender, e) => moveUpContinuouslyTimer.Stop();

            Timer moveDownContinuouslyTimer = new Timer { Interval = scrollSpeed };
            moveDownContinuouslyTimer.Tick += (s, e) => ScrollMemory(1);
            _buttonMemoryMoveDownContinuously.MouseDown += (sender, e) => moveDownContinuouslyTimer.Start();
            _buttonMemoryMoveDownContinuously.MouseUp += (sender, e) => moveDownContinuouslyTimer.Stop();
        }

        private void ScrollMemory(int numLines)
        {
            uint? address = Address;
            if (!address.HasValue) return;

            int addressOffset = numLines * 0x10;
            uint newAddress = (uint)(address.Value + addressOffset);
            SetCustomAddress(newAddress);
        }

        private void SetCustomAddress(uint? address)
        {
            if (!address.HasValue) 
            {
                RefreshAddressTextbox();
                return;
            }
            if (address < 0x80000000 || address + MemorySize >= 0x80000000 + Config.RamSize)
            {
                RefreshAddressTextbox();
                return;
            }
            _checkBoxMemoryUseObjAddress.Checked = false;
            Address = address.Value;
        }

        private void SetCustomMemorySize(uint? memorySize)
        {
            if (!memorySize.HasValue)
            {
                RefreshMemorySizeTextbox();
                return;
            }
            memorySize = memorySize.Value / 16 * 16;
            if (memorySize.Value == 0)
            {
                RefreshMemorySizeTextbox();
                return;
            }
            if (Address + memorySize.Value >= 0x80000000 + Config.RamSize)
            {
                RefreshMemorySizeTextbox();
                return;
            }
            _checkBoxMemoryUseObjAddress.Checked = false;
            MemorySize = memorySize.Value;
        }

        public void SetObjectAddress(uint? address)
        {
            if (!address.HasValue) return;
            _checkBoxMemoryUseObjAddress.Checked = true;
            Address = address.Value;
            MemorySize = ObjectConfig.StructSize;
        }

        private void RefreshAddressTextbox()
        {
            _textBoxMemoryBaseAddress.Text =
                _address.HasValue ? HexUtilities.FormatValue(_address.Value, 8) : "";
        }

        private void RefreshMemorySizeTextbox()
        {
            _textBoxMemoryMemorySize.Text = HexUtilities.FormatValue(_memorySize);
        }

        private void MemoryValueClick()
        {
            bool isCtrlKeyHeld = KeyboardUtilities.IsCtrlHeld();
            bool isAltKeyHeld = KeyboardUtilities.IsAltHeld();
            if (!isCtrlKeyHeld) return;
            int index = _richTextBoxMemoryValues.SelectionStart;
            bool useObjAddress = _checkBoxMemoryUseObjAddress.Checked;
            bool useHex = _checkBoxMemoryHex.Checked;
            bool useObj = _checkBoxMemoryObj.Checked;
            if (isAltKeyHeld)
            {
                List<List<WatchVariableControlPrecursor>> precursorLists =
                    new List<List<WatchVariableControlPrecursor>>()
                        { _objectPrecursors, _objectSpecificPrecursors };
                _currentValueTexts.ForEach(valueText =>
                    valueText.AddOverlappedIfSelected(index, precursorLists));
            }
            else
            {
                _currentValueTexts.ForEach(valueText =>
                    valueText.AddVariableIfSelected(index, useObjAddress, useHex, useObj));
            }
            _richTextBoxMemoryValues.Parent.Focus();
        }

        private class ValueText
        {
            public readonly int ByteIndex;
            public readonly int ByteSize;
            public readonly int StringIndex;
            public readonly int StringSize;
            private readonly uint MemoryAddress;
            private readonly Type MemoryType;
            
            public ValueText(
                int byteIndex,
                int byteSize,
                int stringIndex,
                int stringSize,
                uint memoryAddress,
                Type memoryType)
            {
                ByteIndex = byteIndex;
                ByteSize = byteSize;
                StringIndex = stringIndex;
                StringSize = stringSize;
                MemoryAddress = memoryAddress;
                MemoryType = memoryType;
            }

            public bool OverlapsData(List<WatchVariableControlPrecursor> precursors)
            {
                return GetOverlapped(precursors).Count > 0;
            }

            private List<WatchVariableControlPrecursor> GetOverlapped(
                List<WatchVariableControlPrecursor> precursors)
            {
                uint minOffset = MemoryAddress;
                uint maxOffset = MemoryAddress + (uint)ByteSize - 1;
                uint? minObjOffset = ObjectConfig.GetObjectRelativeAddress(minOffset);
                uint? maxObjOffset = ObjectConfig.GetObjectRelativeAddress(maxOffset);

                return precursors.FindAll(precursor =>
                {
                    WatchVariable watchVar = precursor.WatchVar;
                    if (watchVar.IsSpecial) return false;
                    if (watchVar.Mask != null) return false;

                    uint minPrecursorOffset = watchVar.Offset;
                    uint maxPrecursorOffset = watchVar.Offset + (uint)watchVar.ByteCount.Value - 1;

                    if (watchVar.BaseAddressType == BaseAddressTypeEnum.Object)
                    {
                        if (!minObjOffset.HasValue || !maxObjOffset.HasValue) return false;
                        return minObjOffset <= maxPrecursorOffset && maxObjOffset >= minPrecursorOffset;
                    }
                    if (watchVar.BaseAddressType == BaseAddressTypeEnum.Relative)
                    {
                        return minOffset <= maxPrecursorOffset && maxOffset >= minPrecursorOffset;
                    }
                    return false;
                });
            }

            public void AddOverlappedIfSelected(int selectedIndex, List<List<WatchVariableControlPrecursor>> precursorLists)
            {
                if (selectedIndex >= StringIndex && selectedIndex <= StringIndex + StringSize)
                {
                    AddOverlapped(precursorLists);
                }
            }

            private void AddOverlapped(List<List<WatchVariableControlPrecursor>> precursorLists)
            {
                precursorLists.ForEach(precursors =>
                {
                    List<WatchVariableControlPrecursor> overlapped = GetOverlapped(precursors);
                    overlapped.ForEach(precursor => Config.MemoryManager.AddVariable(
                        precursor.CreateWatchVariableControl(
                            newVariableGroupList: new List<VariableGroup>() { VariableGroup.Custom })));
                });
            }

            public void AddVariableIfSelected(int selectedIndex, bool useObjAddress, bool useHex, bool useObj)
            {
                if (selectedIndex >= StringIndex && selectedIndex <= StringIndex + StringSize)
                {
                    AddVariable(useObjAddress, useHex, useObj);
                }
            }

            private void AddVariable(bool useObjAddress, bool useHex, bool useObj)
            {
                WatchVariableControlPrecursor precursor = CreatePrecursor(useObjAddress, useHex, useObj);
                Config.MemoryManager.AddVariable(precursor.CreateWatchVariableControl());
            }

            private WatchVariableControlPrecursor CreatePrecursor(bool useObjAddress, bool useHex, bool useObj)
            {
                WatchVariableSubclass subclass = useObj
                    ? WatchVariableSubclass.Object
                    : WatchVariableSubclass.Number;
                if (Keyboard.IsKeyDown(Key.A)) subclass = WatchVariableSubclass.Angle;
                if (Keyboard.IsKeyDown(Key.B)) subclass = WatchVariableSubclass.Boolean;
                if (Keyboard.IsKeyDown(Key.Q)) subclass = WatchVariableSubclass.Object;
                if (Keyboard.IsKeyDown(Key.T)) subclass = WatchVariableSubclass.Triangle;

                bool isObjectOrTriangle =
                    subclass == WatchVariableSubclass.Object ||
                    subclass == WatchVariableSubclass.Triangle;

                Type effectiveType = isObjectOrTriangle
                    ? typeof(uint)
                    : MemoryType;
                string typeString = TypeUtilities.TypeToString[effectiveType];

                bool? hexValue = null;
                if (useHex) hexValue = true;
                if (isObjectOrTriangle) hexValue = null;

                BaseAddressTypeEnum baseAddressType =
                    useObjAddress ? BaseAddressTypeEnum.Object : BaseAddressTypeEnum.Relative;
                uint offset = useObjAddress ? (uint)ByteIndex : MemoryAddress; 

                WatchVariable watchVar = new WatchVariable(
                    typeString,
                    null /* specialType */,
                    baseAddressType,
                    null /* offsetUS */,
                    null /* offsetJP */,
                    null /* offsetPAL */,
                    offset,
                    null /* mask */);
                return new WatchVariableControlPrecursor(
                    typeString + " " + HexUtilities.FormatValue(offset),
                    watchVar,
                    subclass,
                    null /* backgroundColor */,
                    null /* displayType */,
                    null /* roundingLimit */,
                    hexValue,
                    null /* invertBool */,
                    null /* isYaw */,
                    null /* coordinate */,
                    new List<VariableGroup>() { VariableGroup.Custom });
            }
        }

        public void UpdateHexDisplay()
        {
            uint? address = Address;
            if (!address.HasValue)
            {
                _richTextBoxMemoryAddresses.Text = "";
                _richTextBoxMemoryValues.Text = "";
                return;
            }

            // read from memory
            if (ObjectConfig.GetObjectRelativeAddress(address.Value) == 0)
            {
                Behavior = new ObjectDataModel(address.Value).BehaviorCriteria;
            }
            else
            {
                Behavior = null;
            }
            byte[] bytes = Config.Stream.ReadRam(address.Value, (int)MemorySize, EndiannessType.Big);

            // read settings from controls
            bool littleEndian = _checkBoxMemoryLittleEndian.Checked;
            bool relativeAddresses = _checkBoxMemoryRelativeAddresses.Checked;
            uint startAddress = relativeAddresses ? 0 : address.Value;
            bool highlightObjVars = _checkBoxMemoryHighlightObjVars.Checked;
            Type type = TypeUtilities.StringToType[(string)_comboBoxMemoryTypes.SelectedItem];
            bool useHex = _checkBoxMemoryHex.Checked;
            bool useObj = _checkBoxMemoryObj.Checked;

            // update memory addresses
            _richTextBoxMemoryAddresses.Text = FormatAddresses(startAddress, (int)MemorySize);

            // update memory values + highlighting
            int initialSelectionStart = _richTextBoxMemoryValues.SelectionStart;
            int initialSelectionLength = _richTextBoxMemoryValues.SelectionLength;
            _richTextBoxMemoryValues.Text = FormatValues(bytes, address.Value, type, littleEndian, useHex, useObj);
            _currentValueTexts.ForEach(valueText =>
            {
                // Mem tab var
                if (valueText.OverlapsData(_memTabPrecursors))
                {
                    _richTextBoxMemoryValues.SetBackColor(
                        valueText.StringIndex, valueText.StringSize, Color.LightBlue);
                }
                // Specific object var
                else if (highlightObjVars && valueText.OverlapsData(_objectSpecificPrecursors))
                {
                    _richTextBoxMemoryValues.SetBackColor(
                        valueText.StringIndex, valueText.StringSize, Color.LightGreen);
                }
                // Generic object var
                else if (highlightObjVars && valueText.OverlapsData(_objectPrecursors))
                {
                    _richTextBoxMemoryValues.SetBackColor(
                        valueText.StringIndex, valueText.StringSize, Color.LightPink);
                }
            });
            _richTextBoxMemoryValues.SelectionStart = initialSelectionStart;
            _richTextBoxMemoryValues.SelectionLength = initialSelectionLength;
        }

        private static string FormatAddresses(uint startAddress, int totalMemorySize)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < totalMemorySize; i += 16)
            {
                string whiteSpace = "\n";
                if (i == 0) whiteSpace = "";
                builder.Append(whiteSpace);

                uint address = startAddress + (uint)i;
                builder.Append(HexUtilities.FormatValue(address, 8));
            }
            return builder.ToString();
        }

        private static string FormatBytes(byte[] bytes, bool littleEndian)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                string whiteSpace = " ";
                if (i % 4 == 0) whiteSpace = "  ";
                if (i % 16 == 0) whiteSpace = "\n";
                if (i == 0) whiteSpace = "";
                builder.Append(whiteSpace);

                int byteIndex = i;
                if (littleEndian)
                {
                    int mod = i % 4;
                    int antiMod = 3 - mod;
                    byteIndex = byteIndex - mod + antiMod;
                }
                builder.Append(HexUtilities.FormatValue(bytes[byteIndex], 2, false));
            }
            return builder.ToString();
        }

        private string FormatValues(
            byte[] bytes, uint baseAddress, Type type, bool isLittleEndian, bool useHex, bool useObj)
        {
            int typeSize = TypeUtilities.TypeSize[type];
            List<string> stringList = new List<string>();
            for (int i = 0; i < bytes.Length; i += typeSize)
            {
                string whiteSpace = " ";
                if (i % 4 == 0) whiteSpace = "  ";
                if (i % 16 == 0) whiteSpace = "\n ";
                if (i == 0) whiteSpace = " ";
                stringList.Add(whiteSpace);

                object value = TypeUtilities.ConvertBytes(type, bytes, i, isLittleEndian);
                if (useObj)
                {
                    uint uintValue = ParsingUtilities.ParseUInt(value);
                    value = Config.ObjectSlotsManager.GetDescriptiveSlotLabelFromAddress(uintValue, true);
                }
                else if (useHex)
                {
                    value = HexUtilities.FormatMemory(value, typeSize * 2, false);
                }
                stringList.Add(value.ToString());
            }

            List<int> indexList = Enumerable.Range(0, stringList.Count / 2).ToList()
                .ConvertAll(index => index * 2 + 1);
            int maxLength = indexList.Max(index => stringList[index].Length);
            indexList.ForEach(index =>
            {
                string oldString = stringList[index];
                string newString = oldString.PadLeft(maxLength, ' ');
                stringList[index] = newString;
            });

            _currentValueTexts.Clear();
            int totalLength = 0;
            for (int i = 0; i < stringList.Count; i++)
            {
                string stringValue = stringList[i];
                int stringLength = stringValue.Length;
                totalLength += stringLength;
                if (i % 2 == 1)
                {
                    int trimmedLength = stringValue.Trim().Length;
                    int valueIndex = (i - 1) / 2;
                    int byteIndex = valueIndex * typeSize;
                    int byteIndexEndian = isLittleEndian
                        ? EndianUtilities.SwapEndianness(byteIndex, typeSize)
                        : byteIndex;
                    ValueText valueText =
                        new ValueText(
                            byteIndexEndian,
                            typeSize,
                            totalLength - trimmedLength,
                            trimmedLength,
                            baseAddress + (uint)byteIndexEndian,
                            type);
                    _currentValueTexts.Add(valueText);
                }
            }

            StringBuilder builder = new StringBuilder();
            stringList.ForEach(stringValue => builder.Append(stringValue));
            return builder.ToString();
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;

            base.Update(updateView);

            if (_checkBoxMemoryUpdateContinuously.Checked)
            {
                UpdateHexDisplay();
            }
        }
    }
}

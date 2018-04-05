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
        private readonly BetterTextbox _textBoxMemoryObjAddress;
        private readonly CheckBox _checkBoxMemoryUpdateContinuously;
        private readonly CheckBox _checkBoxMemoryLittleEndian;
        private readonly CheckBox _checkBoxMemoryRelativeAddresses;
        private readonly ComboBox _comboBoxMemoryTypes;
        private readonly CheckBox _checkBoxMemoryHex;
        private readonly CheckBox _checkBoxMemoryObj;

        private readonly RichTextBoxEx _richTextBoxMemoryAddresses;
        private readonly RichTextBoxEx _richTextBoxMemoryValues;

        private readonly List<ValueText> _currentValueTexts;
        private readonly List<WatchVariableControlPrecursor> _objectPrecursors;
        private readonly List<WatchVariableControlPrecursor> _objectSpecificPrecursors;

        public uint? Address
        {
            get
            {
                HashSet<uint> addresses = Config.ObjectSlotsManager.SelectedSlotsAddresses;
                if (addresses.Count != 1) return null;
                return addresses.First();
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

        private static readonly int _memorySize = (int)ObjectConfig.StructSize;

        public MemoryManager(TabPage tabControl, WatchVariableFlowLayoutPanel watchVariablePanel, List<WatchVariableControlPrecursor> objectData)
            : base(new List<WatchVariableControlPrecursor>(), watchVariablePanel)
        {
            _behavior = null;
            _currentValueTexts = new List<ValueText>();
            _objectPrecursors = new List<WatchVariableControlPrecursor>(objectData);
            _objectSpecificPrecursors = new List<WatchVariableControlPrecursor>();

            SplitContainer splitContainer = tabControl.Controls["splitContainerMemory"] as SplitContainer;

            _textBoxMemoryObjAddress = splitContainer.Panel1.Controls["textBoxMemoryObjAddress"] as BetterTextbox;
            _checkBoxMemoryUpdateContinuously = splitContainer.Panel1.Controls["checkBoxMemoryUpdateContinuously"] as CheckBox;
            _checkBoxMemoryLittleEndian = splitContainer.Panel1.Controls["checkBoxMemoryLittleEndian"] as CheckBox;
            _checkBoxMemoryRelativeAddresses = splitContainer.Panel1.Controls["checkBoxMemoryRelativeAddresses"] as CheckBox;
            _comboBoxMemoryTypes = splitContainer.Panel1.Controls["comboBoxMemoryTypes"] as ComboBox;
            _checkBoxMemoryHex = splitContainer.Panel1.Controls["checkBoxMemoryHex"] as CheckBox;
            _checkBoxMemoryObj = splitContainer.Panel1.Controls["checkBoxMemoryObj"] as CheckBox;

            _richTextBoxMemoryAddresses = splitContainer.Panel1.Controls["richTextBoxMemoryAddresses"] as RichTextBoxEx;
            _richTextBoxMemoryValues = splitContainer.Panel1.Controls["richTextBoxMemoryValues"] as RichTextBoxEx;

            _comboBoxMemoryTypes.DataSource = TypeUtilities.SimpleTypeList;

            _checkBoxMemoryLittleEndian.Click += (sender, e) => UpdateDisplay();
            _comboBoxMemoryTypes.SelectedValueChanged += (sender, e) => UpdateDisplay();

            _richTextBoxMemoryValues.Click += (sender, e) =>
            {
                bool isCtrlKeyHeld = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
                bool isAltKeyHeld = Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt);
                if (!isCtrlKeyHeld) return;
                int index = _richTextBoxMemoryValues.SelectionStart;
                bool isLittleEndian = _checkBoxMemoryLittleEndian.Checked;
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
                        valueText.AddVariableIfSelected(index, useHex, useObj));
                }
                _richTextBoxMemoryValues.Parent.Focus();
            };
        }

        private class ValueText
        {
            public readonly int ByteIndex;
            public readonly int ByteSize;
            public readonly int StringIndex;
            public readonly int StringSize;
            private readonly Type MemoryType;
            
            public ValueText(int byteIndex, int byteSize, int stringIndex, int stringSize, Type memoryType)
            {
                ByteIndex = byteIndex;
                ByteSize = byteSize;
                StringIndex = stringIndex;
                StringSize = stringSize;
                MemoryType = memoryType;
            }

            public bool OverlapsData(List<WatchVariableControlPrecursor> precursors)
            {
                return GetOverlapped(precursors).Count > 0;
            }

            private List<WatchVariableControlPrecursor> GetOverlapped(
                List<WatchVariableControlPrecursor> precursors)
            {
                int minIndex = ByteIndex;
                int maxIndex = ByteIndex + ByteSize - 1;

                return precursors.FindAll(precursor =>
                {
                    WatchVariable watchVar = precursor.WatchVar;
                    if (watchVar.BaseAddressType != BaseAddressTypeEnum.Object) return false;
                    if (watchVar.IsSpecial) return false;
                    if (watchVar.Mask != null) return false;

                    int minPrecursorIndex = (int)watchVar.Offset;
                    int maxPrecursorIndex = (int)watchVar.Offset + watchVar.ByteCount.Value - 1;

                    return minIndex <= maxPrecursorIndex && maxIndex >= minPrecursorIndex;
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
                    overlapped.ForEach(precursor => Config.MemoryManager.AddVariable(precursor.CreateWatchVariableControl()));
                });
            }

            public void AddVariableIfSelected(int selectedIndex, bool useHex, bool useObj)
            {
                if (selectedIndex >= StringIndex && selectedIndex <= StringIndex + StringSize)
                {
                    AddVariable(useHex, useObj);
                }
            }

            private void AddVariable(bool useHex, bool useObj)
            {
                WatchVariableControlPrecursor precursor = CreatePrecursor(useHex, useObj);
                Config.MemoryManager.AddVariable(precursor.CreateWatchVariableControl());
            }

            private WatchVariableControlPrecursor CreatePrecursor(bool useHex, bool useObj)
            {
                WatchVariableSubclass subclass = useObj
                    ? WatchVariableSubclass.Object
                    : WatchVariableSubclass.Number;
                if (Keyboard.IsKeyDown(Key.A)) subclass = WatchVariableSubclass.Angle;
                if (Keyboard.IsKeyDown(Key.B)) subclass = WatchVariableSubclass.Boolean;
                if (Keyboard.IsKeyDown(Key.Q)) subclass = WatchVariableSubclass.Object;

                Type effectiveType = subclass == WatchVariableSubclass.Object
                    ? typeof(uint)
                    : MemoryType;
                string typeString = TypeUtilities.TypeToString[effectiveType];

                WatchVariable watchVar = new WatchVariable(
                    typeString,
                    null /* specialType */,
                    BaseAddressTypeEnum.Object,
                    null /* offsetUS */,
                    null /* offsetJP */,
                    null /* offsetPAL */,
                    (uint) ByteIndex,
                    null /* mask */);
                return new WatchVariableControlPrecursor(
                    typeString + " " + HexUtilities.Format(ByteIndex),
                    watchVar,
                    subclass,
                    null /* backgroundColor */,
                    null /* roundingLimit */,
                    subclass == WatchVariableSubclass.Object
                        ? (bool?)null
                        : useHex && MemoryType != typeof(float),
                    null /* invertBool */,
                    null /* coordinate */,
                    new List<VariableGroup>());
            }
        }

        public void UpdateDisplay()
        {
            uint? address = Address;
            if (!address.HasValue)
            {
                _textBoxMemoryObjAddress.Text = HexUtilities.Format(0, 8);
                _richTextBoxMemoryAddresses.Text = "";
                _richTextBoxMemoryValues.Text = "";
                return;
            }

            // read from memory
            Behavior = new ObjectDataModel(address.Value).BehaviorCriteria;
            byte[] bytes = Config.Stream.ReadRam(address.Value, _memorySize);

            // read settings from controls
            bool littleEndian = _checkBoxMemoryLittleEndian.Checked;
            bool relativeAddresses = _checkBoxMemoryRelativeAddresses.Checked;
            uint startAddress = relativeAddresses ? 0 : address.Value;
            Type type = TypeUtilities.StringToType[(string)_comboBoxMemoryTypes.SelectedItem];
            bool useHex = _checkBoxMemoryHex.Checked;
            bool useObj = _checkBoxMemoryObj.Checked;

            // update control text
            _textBoxMemoryObjAddress.Text = HexUtilities.Format(address.Value, 8);
            _richTextBoxMemoryAddresses.Text = FormatAddresses(startAddress, _memorySize);

            // highlight value texts
            int initialSelectionStart = _richTextBoxMemoryValues.SelectionStart;
            int initialSelectionLength = _richTextBoxMemoryValues.SelectionLength;
            _richTextBoxMemoryValues.Text = FormatValues(bytes, type, littleEndian, useHex, useObj);
            _currentValueTexts.ForEach(valueText =>
            {
                if (valueText.OverlapsData(_objectPrecursors))
                {
                    _richTextBoxMemoryValues.SetBackColor(
                        valueText.StringIndex, valueText.StringSize, Color.LightPink);
                }
                else if (valueText.OverlapsData(_objectSpecificPrecursors))
                {
                    _richTextBoxMemoryValues.SetBackColor(
                        valueText.StringIndex, valueText.StringSize, Color.LightGreen);
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
                builder.Append(HexUtilities.Format(address, 8));
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
                builder.Append(HexUtilities.Format(bytes[byteIndex], 2, false));
            }
            return builder.ToString();
        }

        private string FormatValues(byte[] bytes, Type type, bool isLittleEndian, bool useHex, bool useObj)
        {
            int typeSize = TypeUtilities.TypeSize[type];
            List<string> stringList = new List<string>();
            for (int i = 0; i < bytes.Length; i += typeSize)
            {
                string whiteSpace = " ";
                if (i % 4 == 0) whiteSpace = "  ";
                if (i % 16 == 0) whiteSpace = "\n";
                if (i == 0) whiteSpace = "";
                stringList.Add(whiteSpace);

                object value = TypeUtilities.ConvertBytes(type, bytes, i, isLittleEndian);
                if (useObj)
                {
                    uint uintValue = ParsingUtilities.ParseUInt(value);
                    value = Config.ObjectSlotsManager.GetDescriptiveSlotLabelFromAddress(uintValue, true);
                }
                else if (useHex && type != typeof(float))
                {
                    value = HexUtilities.Format(value, typeSize * 2, false);
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
                UpdateDisplay();
            }
        }
    }
}

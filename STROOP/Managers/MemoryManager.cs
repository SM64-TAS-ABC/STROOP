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
        private readonly ComboBox _comboBoxMemoryTypes;
        private readonly CheckBox _checkBoxMemoryHex;
        private readonly CheckBox _checkBoxMemoryObj;

        private readonly RichTextBoxEx _richTextBoxMemoryAddresses;
        private readonly RichTextBoxEx _richTextBoxMemoryBytes;
        private readonly RichTextBoxEx _richTextBoxMemoryValues;

        private readonly bool[] _objectDataBools;
        private bool[] _objectSpecificDataBools;

        public uint? Address { get; private set; }

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
                if (_behavior.HasValue)
                {
                    List<WatchVariableControlPrecursor> precursors =
                        Config.ObjectAssociations.GetWatchVarControls(_behavior.Value)
                            .ConvertAll(control => control.WatchVarPrecursor);
                    _objectSpecificDataBools = ConvertPrecursorsToBoolArray(precursors);
                }
                else
                {
                    _objectSpecificDataBools = ConvertPrecursorsToBoolArray(null);
                }
            }
        }

        private static readonly int _memorySize = (int)ObjectConfig.StructSize;

        private List<ValueText> _currentValueTexts;

        public MemoryManager(TabPage tabControl, WatchVariableFlowLayoutPanel watchVariablePanel, List<WatchVariableControlPrecursor> objectData)
            : base(new List<WatchVariableControlPrecursor>(), watchVariablePanel)
        {
            Address = null;
            _behavior = null;
            _currentValueTexts = new List<ValueText>();

            SplitContainer splitContainer = tabControl.Controls["splitContainerMemory"] as SplitContainer;

            _textBoxMemoryObjAddress = splitContainer.Panel1.Controls["textBoxMemoryObjAddress"] as BetterTextbox;
            _checkBoxMemoryUpdateContinuously = splitContainer.Panel1.Controls["checkBoxMemoryUpdateContinuously"] as CheckBox;
            _checkBoxMemoryLittleEndian = splitContainer.Panel1.Controls["checkBoxMemoryLittleEndian"] as CheckBox;
            _comboBoxMemoryTypes = splitContainer.Panel1.Controls["comboBoxMemoryTypes"] as ComboBox;
            _checkBoxMemoryHex = splitContainer.Panel1.Controls["checkBoxMemoryHex"] as CheckBox;
            _checkBoxMemoryObj = splitContainer.Panel1.Controls["checkBoxMemoryObj"] as CheckBox;

            _richTextBoxMemoryAddresses = splitContainer.Panel1.Controls["richTextBoxMemoryAddresses"] as RichTextBoxEx;
            _richTextBoxMemoryBytes = splitContainer.Panel1.Controls["richTextBoxMemoryBytes"] as RichTextBoxEx;
            _richTextBoxMemoryValues = splitContainer.Panel1.Controls["richTextBoxMemoryValues"] as RichTextBoxEx;

            _comboBoxMemoryTypes.DataSource = TypeUtilities.SimpleTypeList;

            _objectDataBools = ConvertPrecursorsToBoolArray(objectData);

            _checkBoxMemoryLittleEndian.Click += (sender, e) => UpdateMemory();
            _comboBoxMemoryTypes.SelectedValueChanged += (sender, e) => UpdateMemory();

            _richTextBoxMemoryValues.Click += (sender, e) =>
            {
                bool isCtrlKeyHeld = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
                if (!isCtrlKeyHeld) return;
                int index = _richTextBoxMemoryValues.SelectionStart;
                bool isLittleEndian = _checkBoxMemoryLittleEndian.Checked;
                bool useHex = _checkBoxMemoryHex.Checked;
                bool useObj = _checkBoxMemoryObj.Checked;
                _currentValueTexts.ForEach(valueText =>
                    valueText.AddToVariablePanelIfSelected(index, isLittleEndian, useHex, useObj));
                _richTextBoxMemoryValues.Parent.Focus();
            };
        }

        private bool[] ConvertPrecursorsToBoolArray(List<WatchVariableControlPrecursor> precursors)
        {
            bool[] boolArray = new bool[ObjectConfig.StructSize];
            if (precursors == null) return boolArray;
            foreach (WatchVariableControlPrecursor precursor in precursors)
            {
                WatchVariable watchVar = precursor.WatchVar;
                if (watchVar.BaseAddressType != BaseAddressTypeEnum.Object) continue;
                if (watchVar.IsSpecial) continue;
                if (watchVar.Mask != null) continue;

                uint offset = watchVar.Offset;
                int size = watchVar.ByteCount.Value;

                for (int i = 0; i < size; i++)
                {
                    boolArray[offset + i] = true;
                }
            }
            return boolArray;
        }

        public void SetAddressAndUpdateMemory(uint address)
        {
            _textBoxMemoryObjAddress.Text = HexUtilities.Format(address, 8);
            Address = address;
            UpdateMemory();
        }

        private class ValueText
        {
            public readonly int ByteIndex;
            public readonly int ByteSize;
            public readonly int StringIndex;
            public readonly int StringSize;
            private readonly Type MemoryType;
            private readonly List<int> _byteIndexes;
            private readonly List<int> _byteIndexesLittleEndian;
            
            public ValueText(int byteIndex, int byteSize, int stringIndex, int stringSize, Type memoryType)
            {
                ByteIndex = byteIndex;
                ByteSize = byteSize;
                StringIndex = stringIndex;
                StringSize = stringSize;
                MemoryType = memoryType;
                _byteIndexes = Enumerable.Range(byteIndex, byteSize).ToList();
                _byteIndexesLittleEndian = _byteIndexes.ConvertAll(
                    index => EndianUtilities.SwapEndianness(index));
            }

            public bool OverlapsData(bool[] dataBools, bool littleEndian)
            {
                List<int> byteIndexes = littleEndian ? _byteIndexesLittleEndian : _byteIndexes;
                return byteIndexes.Any(byteIndex => dataBools[byteIndex]);
            }

            public void AddToVariablePanelIfSelected(int selectedIndex, bool isLittleEndian, bool useHex, bool useObj)
            {
                if (selectedIndex >= StringIndex && selectedIndex <= StringIndex + StringSize)
                {
                    AddToVariablePanel(isLittleEndian, useHex, useObj);
                }
            }

            private void AddToVariablePanel(bool isLittleEndian, bool useHex, bool useObj)
            {
                WatchVariableControlPrecursor precursor = CreatePrecursor(isLittleEndian, useHex, useObj);
                Config.MemoryManager.AddVariable(precursor.CreateWatchVariableControl());
            }

            private WatchVariableControlPrecursor CreatePrecursor(bool isLittleEndian, bool useHex, bool useObj)
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
                uint address = isLittleEndian
                        ? (uint)EndianUtilities.SwapEndianness(ByteIndex, ByteSize)
                        : (uint)ByteIndex;

                WatchVariable watchVar = new WatchVariable(
                    typeString,
                    null /* specialType */,
                    BaseAddressTypeEnum.Memory,
                    null /* offsetUS */,
                    null /* offsetJP */,
                    null /* offsetPAL */,
                    address,
                    null /* mask */);
                return new WatchVariableControlPrecursor(
                    typeString + " " + HexUtilities.Format(address),
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

        private void UpdateMemory()
        {
            if (!Address.HasValue) return;

            Behavior = new ObjectDataModel(Address.Value).BehaviorCriteria;

            byte[] bytes = Config.Stream.ReadRam(Address.Value, _memorySize);
            bool littleEndian = _checkBoxMemoryLittleEndian.Checked;
            Type type = TypeUtilities.StringToType[(string)_comboBoxMemoryTypes.SelectedItem];
            bool useHex = _checkBoxMemoryHex.Checked;
            bool useObj = _checkBoxMemoryObj.Checked;
            _richTextBoxMemoryAddresses.Text = FormatAddresses(Address.Value, _memorySize);
            _richTextBoxMemoryBytes.Text = FormatBytes(bytes, littleEndian);

            _richTextBoxMemoryValues.Text = FormatValues(bytes, type, littleEndian, useHex, useObj);
            _currentValueTexts.ForEach(valueText =>
            {
                if (valueText.OverlapsData(_objectDataBools, littleEndian))
                {
                    _richTextBoxMemoryValues.SetBackColor(
                        valueText.StringIndex, valueText.StringSize, Color.LightPink);
                }
                else if (valueText.OverlapsData(_objectSpecificDataBools, littleEndian))
                {
                    _richTextBoxMemoryValues.SetBackColor(
                        valueText.StringIndex, valueText.StringSize, Color.LightGreen);
                }
            });
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

        private string FormatValues(byte[] bytes, Type type, bool littleEndian, bool useHex, bool useObj)
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

                object value = TypeUtilities.ConvertBytes(type, bytes, i, littleEndian);
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
                    ValueText valueText =
                        new ValueText(
                            valueIndex * typeSize,
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

            bool isCtrlKeyHeld = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            if (_checkBoxMemoryUpdateContinuously.Checked && !isCtrlKeyHeld)
            {
                UpdateMemory();
            }
        }
    }
}

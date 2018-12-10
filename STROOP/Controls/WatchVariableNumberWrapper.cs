using STROOP.Extensions;
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
    public class WatchVariableNumberWrapper : WatchVariableWrapper
    {
        private ToolStripSeparator _separatorCoordinates;
        private ToolStripMenuItem _itemCopyCoordinates;
        private ToolStripMenuItem _itemPasteCoordinates;

        private static readonly int MAX_ROUNDING_LIMIT = 10;

        private readonly Type _displayType;

        private readonly int _defaultRoundingLimit;
        private int _roundingLimit;
        private Action<int> _setRoundingLimit;

        protected readonly bool _defaultDisplayAsHex;
        protected bool _displayAsHex;
        protected Action<bool> _setDisplayAsHex;

        public WatchVariableNumberWrapper(
            WatchVariable watchVar,
            WatchVariableControl watchVarControl,
            Type displayType = DEFAULT_DISPLAY_TYPE,
            int? roundingLimit = DEFAULT_ROUNDING_LIMIT,
            bool? displayAsHex = DEFAULT_DISPLAY_AS_HEX,
            bool useCheckbox = DEFAULT_USE_CHECKBOX,
            Coordinate? coordinate = null)
            : base(watchVar, watchVarControl, useCheckbox)
        {
            _displayType = displayType;

            _defaultRoundingLimit = roundingLimit ?? DEFAULT_ROUNDING_LIMIT;
            _roundingLimit = _defaultRoundingLimit;
            if (_roundingLimit < -1 || _roundingLimit > MAX_ROUNDING_LIMIT)
                throw new ArgumentOutOfRangeException();

            _defaultDisplayAsHex = displayAsHex ?? DEFAULT_DISPLAY_AS_HEX;
            _displayAsHex = _defaultDisplayAsHex;

            AddCoordinateContextMenuStripItems();
            AddNumberContextMenuStripItems();

            if (coordinate != null) WatchVariableCoordinateManager.NotifyCoordinate(coordinate.Value, this);
        }

        private void AddNumberContextMenuStripItems()
        {
            ToolStripMenuItem itemRoundTo = new ToolStripMenuItem("Round to ...");
            List<int> roundingLimitNumbers = Enumerable.Range(-1, MAX_ROUNDING_LIMIT + 2).ToList();
            _setRoundingLimit = ControlUtilities.AddCheckableDropDownItems(
                itemRoundTo,
                roundingLimitNumbers.ConvertAll(i => i == -1 ? "No Rounding" : i + " decimal place(s)"),
                roundingLimitNumbers,
                (int roundingLimit) => { _roundingLimit = roundingLimit; },
                _roundingLimit);

            ToolStripMenuItem itemDisplayAsHex = new ToolStripMenuItem("Display as Hex");
            _setDisplayAsHex = (bool displayAsHex) =>
            {
                _displayAsHex = displayAsHex;
                itemDisplayAsHex.Checked = displayAsHex;
            };
            itemDisplayAsHex.Click += (sender, e) => _setDisplayAsHex(!_displayAsHex);
            itemDisplayAsHex.Checked = _displayAsHex;

            _contextMenuStrip.AddToBeginningList(new ToolStripSeparator());
            _contextMenuStrip.AddToBeginningList(itemRoundTo);
            _contextMenuStrip.AddToBeginningList(itemDisplayAsHex);
        }

        private void AddCoordinateContextMenuStripItems()
        {
            _separatorCoordinates = new ToolStripSeparator();
            _separatorCoordinates.Visible = false;

            _itemCopyCoordinates = new ToolStripMenuItem("Copy Coordinates");
            _itemCopyCoordinates.Visible = false;

            _itemPasteCoordinates = new ToolStripMenuItem("Paste Coordinates");
            _itemPasteCoordinates.Visible = false;

            _contextMenuStrip.AddToBeginningList(_separatorCoordinates);
            _contextMenuStrip.AddToBeginningList(_itemCopyCoordinates);
            _contextMenuStrip.AddToBeginningList(_itemPasteCoordinates);
        }

        public void EnableCoordinateContextMenuStripItemFunctionality(List<WatchVariableNumberWrapper> coordinateVarList)
        {
            int coordinateCount = coordinateVarList.Count;
            if (coordinateCount != 2 && coordinateCount != 3)
                throw new ArgumentOutOfRangeException();

            Action<string> copyCoordinatesWithSeparator = (string separator) =>
            {
                Clipboard.SetText(
                    String.Join(separator, coordinateVarList.ConvertAll(
                        coord => coord.GetValue(false))));
            };

            ToolStripMenuItem itemCopyCoordinatesCommas = new ToolStripMenuItem("Copy Coordinates with Commas");
            itemCopyCoordinatesCommas.Click += (sender, e) => copyCoordinatesWithSeparator(",");

            ToolStripMenuItem itemCopyCoordinatesTabs = new ToolStripMenuItem("Copy Coordinates with Tabs");
            itemCopyCoordinatesTabs.Click += (sender, e) => copyCoordinatesWithSeparator("\t");

            ToolStripMenuItem itemCopyCoordinatesLineBreaks = new ToolStripMenuItem("Copy Coordinates with Line Breaks");
            itemCopyCoordinatesLineBreaks.Click += (sender, e) => copyCoordinatesWithSeparator("\r\n");

            _itemCopyCoordinates.DropDownItems.Add(itemCopyCoordinatesCommas);
            _itemCopyCoordinates.DropDownItems.Add(itemCopyCoordinatesTabs);
            _itemCopyCoordinates.DropDownItems.Add(itemCopyCoordinatesLineBreaks);

            _itemPasteCoordinates.Click += (sender, e) =>
            {
                List<string> stringList = ParsingUtilities.ParseStringList(Clipboard.GetText());
                int stringCount = stringList.Count;
                if (stringCount != 2 && stringCount != 3) return;

                Config.Stream.Suspend();
                coordinateVarList[0]._watchVarControl.SetValue(stringList[0]);
                if (coordinateCount == 3 && stringCount == 3)
                    coordinateVarList[1]._watchVarControl.SetValue(stringList[1]);
                coordinateVarList[coordinateCount - 1]._watchVarControl.SetValue(stringList[stringCount - 1]);
                Config.Stream.Resume();
            };

            _separatorCoordinates.Visible = true;
            _itemCopyCoordinates.Visible = true;
            _itemPasteCoordinates.Visible = true;
        }



        protected override void HandleVerification(object value)
        {
            base.HandleVerification(value);
            if (!TypeUtilities.IsNumber(value))
                throw new ArgumentOutOfRangeException(value + " is not a number");
        }

        protected override object HandleRounding(object value, bool handleRounding)
        {
            if (_displayAsHex) return value;
            int? roundingLimit = handleRounding && _roundingLimit >= 0 ? _roundingLimit : (int?)null;
            double doubleValue = Convert.ToDouble(value);
            double roundedValue = roundingLimit.HasValue
                ? Math.Round(doubleValue, roundingLimit.Value)
                : doubleValue;
            if (SavedSettingsConfig.DontRoundValuesToZero &&
                roundedValue == 0 && doubleValue != 0)
            {
                // Specially print values near zero
                string digitsString = roundingLimit?.ToString() ?? "";
                return doubleValue.ToString("E" + digitsString);
            }
            return roundedValue;
        }

        protected override object HandleHexDisplaying(object value)
        {
            if (!_displayAsHex) return value;
            return SavedSettingsConfig.DisplayAsHexUsesMemory
                ? HexUtilities.FormatMemory(value, GetHexDigitCount() ?? 8, true)
                : HexUtilities.FormatValue(value, GetHexDigitCount() ?? 8, true);
        }

        protected override object HandleHexUndisplaying(object value)
        {
            string stringValue = value.ToString();
            if (stringValue.Length < 2 || stringValue.Substring(0, 2) != "0x") return value;

            if (SavedSettingsConfig.DisplayAsHexUsesMemory)
            {
                if (_watchVar.MemoryType == null) return value;
                object obj = TypeUtilities.ConvertBytes(_watchVar.MemoryType, stringValue, false);
                if (obj != null) return obj;
            }
            else
            {
                uint? parsed = ParsingUtilities.ParseHexNullable(stringValue);
                if (parsed != null) return parsed.Value;
            }
            return value;
        }

        protected virtual int? GetHexDigitCount()
        {
            if (_displayType != null) return TypeUtilities.TypeSize[_displayType] * 2;
            return _watchVar.NibbleCount;
        }

        protected override bool GetUseHex()
        {
            return _displayAsHex;
        }

        protected override bool GetUseHexExactly()
        {
            return _displayAsHex;
        }

        public override void ApplySettings(WatchVariableControlSettings settings)
        {
            base.ApplySettings(settings);
            if (settings.ChangeRoundingLimit && _roundingLimit != 0)
            {
                if (settings.ChangeRoundingLimitToDefault)
                    _setRoundingLimit(_defaultRoundingLimit);
                else
                    _setRoundingLimit(settings.NewRoundingLimit);
            }
            if (settings.ChangeDisplayAsHex)
            {
                if (settings.ChangeDisplayAsHexToDefault)
                    _setDisplayAsHex(_defaultDisplayAsHex);
                else
                    _setDisplayAsHex(settings.NewDisplayAsHex);
            }
        }

        public override void ToggleDisplayAsHex(bool? displayAsHexNullable = null)
        {
            bool displayAsHex = displayAsHexNullable ?? !_displayAsHex;
            _setDisplayAsHex(displayAsHex);
        }
    }
}

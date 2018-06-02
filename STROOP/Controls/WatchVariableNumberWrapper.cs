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

        private readonly int? _defaultRoundingLimit;
        private int? _roundingLimit;

        protected readonly bool _defaultDisplayAsHex;
        protected bool _displayAsHex;

        public WatchVariableNumberWrapper(
            WatchVariable watchVar,
            WatchVariableControl watchVarControl,
            int? roundingLimit = DEFAULT_ROUNDING_LIMIT,
            bool? displayAsHex = DEFAULT_DISPLAY_AS_HEX,
            bool useCheckbox = DEFAULT_USE_CHECKBOX,
            WatchVariableCoordinate? coordinate = null)
            : base(watchVar, watchVarControl, useCheckbox)
        {
            // Null input translates to default rounding, not null as in no rounding
            roundingLimit = roundingLimit ?? DEFAULT_ROUNDING_LIMIT;
            if (roundingLimit.HasValue)
            {
                roundingLimit = MoreMath.Clamp(roundingLimit.Value, 0, MAX_ROUNDING_LIMIT);
            }

            _defaultRoundingLimit = roundingLimit;
            _roundingLimit = _defaultRoundingLimit;

            _defaultDisplayAsHex = displayAsHex ?? DEFAULT_DISPLAY_AS_HEX;
            _displayAsHex = _defaultDisplayAsHex;

            AddCoordinateContextMenuStripItems();
            AddNumberContextMenuStripItems();

            if (coordinate != null) WatchVariableCoordinateManager.NotifyCoordinate(coordinate.Value, this);
        }

        private void AddNumberContextMenuStripItems()
        {
            ToolStripMenuItem itemRoundTo = new ToolStripMenuItem("Round to ...");
            List<int> roundingLimitNumbers = Enumerable.Range(0, MAX_ROUNDING_LIMIT + 1).ToList();
            ControlUtilities.AddCheckableDropDownItems(
                itemRoundTo,
                new List<string>() { "No rounding" }.Concat(roundingLimitNumbers.ConvertAll(i => i + " decimal place(s)")).ToList(),
                new List<object>() { null }.Concat(roundingLimitNumbers.ConvertAll(i => (object)i)).ToList(),
                (object obj) => { _roundingLimit = (int?)obj; },
                _roundingLimit);

            ToolStripMenuItem itemDisplayAsHex = new ToolStripMenuItem("Display as Hex");
            itemDisplayAsHex.Click += (sender, e) =>
            {
                _displayAsHex = !_displayAsHex;
                itemDisplayAsHex.Checked = _displayAsHex;
            };
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
            int? roundingLimit = handleRounding ? _roundingLimit : null;
            double doubleValue = Convert.ToDouble(value);
            double roundedValue = roundingLimit.HasValue
                ? Math.Round(doubleValue, roundingLimit.Value)
                : doubleValue;
            if (OptionsConfig.DontRoundValuesToZero &&
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
            return HexUtilities.FormatByValueIfInteger(value, GetHexDigitCount(), true);
        }

        protected override object HandleHexUndisplaying(object value)
        {
            string stringValue = value.ToString();
            if (stringValue.Length >= 2 && stringValue.Substring(0,2) == "0x")
            {
                uint? parsed = ParsingUtilities.ParseHexNullable(stringValue);
                if (parsed != null) return parsed.Value;
            }
            return value;
        }

        protected virtual int? GetHexDigitCount()
        {
            return _watchVar.NibbleCount;
        }

        protected override bool GetUseHex()
        {
            return _displayAsHex;
        }

        public override void ApplySettings(WatchVariableControlSettings settings)
        {
            base.ApplySettings(settings);
            if (settings.ChangeRoundingLimit && _defaultRoundingLimit != 0)
            {
                if (settings.ChangeRoundingLimitToDefault)
                    _roundingLimit = _defaultRoundingLimit;
                else
                    _roundingLimit = settings.NewRoundingLimit;
            }
        }
    }
}

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

        private int? _roundingLimit;
        private bool _displayAsHex;
        private bool _displayAsNegated;

        public WatchVariableNumberWrapper(
            WatchVariable watchVar,
            WatchVariableControl watchVarControl,
            int? roundingLimit = DEFAULT_ROUNDING_LIMIT,
            bool? displayAsHex = DEFAULT_DISPLAY_AS_HEX,
            bool useCheckbox = DEFAULT_USE_CHECKBOX,
            WatchVariableCoordinate? coordinate = null)
            : base(watchVar, watchVarControl, useCheckbox)
        {
            if (roundingLimit.HasValue)
            {
                roundingLimit = MoreMath.Clamp(roundingLimit.Value, 0, MAX_ROUNDING_LIMIT);
            }

            _roundingLimit = roundingLimit;
            _displayAsHex = displayAsHex ?? DEFAULT_DISPLAY_AS_HEX;
            _displayAsNegated = false;

            AddCoordinateContextMenuStripItems();
            AddNumberContextMenuStripItems();

            if (coordinate != null) WatchVariableCoordinateManager.NotifyCoordinate(coordinate.Value, this);
        }

        private void AddNumberContextMenuStripItems()
        {
            ToolStripMenuItem itemRoundTo = new ToolStripMenuItem("Round to ...");
            List<int> roundingLimitNumbers = Enumerable.Range(0, MAX_ROUNDING_LIMIT + 1).ToList();
            ControlUtilities.AddDropDownItems(
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

            ToolStripMenuItem itemDisplayAsNegated = new ToolStripMenuItem("Display as Negated");
            itemDisplayAsNegated.Click += (sender, e) =>
            {
                _displayAsNegated = !_displayAsNegated;
                itemDisplayAsNegated.Checked = _displayAsNegated;
            };
            itemDisplayAsNegated.Checked = _displayAsNegated;

            _contextMenuStrip.AddToBeginningList(new ToolStripSeparator());
            _contextMenuStrip.AddToBeginningList(itemRoundTo);
            _contextMenuStrip.AddToBeginningList(itemDisplayAsHex);
            _contextMenuStrip.AddToBeginningList(itemDisplayAsNegated);
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
            if (coordinateVarList.Count != 3) throw new ArgumentOutOfRangeException();

            Action<string> copyCoordinatesWithSeparator = (string separator) =>
            {
                Clipboard.SetText(
                    String.Join(separator, coordinateVarList.ConvertAll(
                        coord => coord.GetStringValue(false))));
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
                List<string> stringList = ParsingUtilities.ParseTextIntoStrings(Clipboard.GetText());
                if (stringList.Count < 3) return;

                Config.Stream.Suspend();
                for (int i = 0; i < 3; i++)
                {
                    coordinateVarList[i].SetStringValue(stringList[i]);
                }
                Config.Stream.Resume();
            };

            _separatorCoordinates.Visible = true;
            _itemCopyCoordinates.Visible = true;
            _itemPasteCoordinates.Visible = true;
        }



        protected override string HandleRounding(string stringValue)
        {
            double? doubleValueNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
            if (!doubleValueNullable.HasValue) return stringValue;
            double doubleValue = doubleValueNullable.Value;
            if (_roundingLimit.HasValue) doubleValue = Math.Round(doubleValue, _roundingLimit.Value);
            return doubleValue.ToString();
        }

        protected override string HandleNegating(string stringValue)
        {
            double? doubleValueNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
            if (!doubleValueNullable.HasValue) return stringValue;
            double doubleValue = doubleValueNullable.Value;
            if (_displayAsNegated) doubleValue = -1 * doubleValue;
            return doubleValue.ToString();
        }

        protected override string HandleUnnegating(string stringValue)
        {
            return HandleNegating(stringValue);
        }

        protected override string HandleHexDisplaying(string stringValue)
        {
            if (!_displayAsHex) return stringValue;

            int? numHexDigits = GetHexDigitCount();
            string stringHexDigits = numHexDigits?.ToString() ?? "";

            int? intValueNullable = ParsingUtilities.ParseIntNullable(stringValue);
            if (intValueNullable.HasValue)
            {
                string hexFormat = String.Format("{0:X" + stringHexDigits + "}", intValueNullable.Value);
                if (numHexDigits.HasValue && hexFormat.Length > numHexDigits.Value)
                {
                    hexFormat = hexFormat.Substring(hexFormat.Length - numHexDigits.Value);
                }
                return "0x" + hexFormat;
            }

            uint? uintValueNullable = ParsingUtilities.ParseUIntNullable(stringValue);
            if (uintValueNullable.HasValue)
            {
                string hexFormat = String.Format("{0:X" + stringHexDigits + "}", uintValueNullable.Value);
                if (numHexDigits.HasValue && hexFormat.Length > numHexDigits.Value)
                {
                    hexFormat = hexFormat.Substring(hexFormat.Length - numHexDigits.Value);
                }
                return "0x" + hexFormat;
            }

            return stringValue;
        }

        protected override string HandleHexUndisplaying(string value)
        {
            if (value != null && value.Length >= 2 && value.Substring(0,2) == "0x")
            {
                uint? parsed = ParsingUtilities.ParseHexNullable(value);
                if (parsed != null) return parsed.ToString();
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
    }
}

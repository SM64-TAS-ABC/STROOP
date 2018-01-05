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
    public class VarXNumber : VarX
    {
        private static readonly int MAX_ROUNDING_LIMIT = 10;

        private int? _roundingLimit;
        private bool _displayAsHex;
        private bool _displayAsNegated;

        public VarXNumber(string name, AddressHolder addressHolder, Color? backgroundColor, int? roundingLimit = 3, bool displayAsHex = false)
            : base(name, addressHolder, backgroundColor)
        {
            if (roundingLimit.HasValue)
            {
                roundingLimit = MoreMath.Clamp(roundingLimit.Value, 0, MAX_ROUNDING_LIMIT);
            }

            _roundingLimit = roundingLimit;
            _displayAsHex = displayAsHex;
            _displayAsNegated = false;

            AddNumberContextMenuStrip();
        }

        private void AddNumberContextMenuStrip()
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

            Control.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            Control.ContextMenuStrip.Items.Add(itemRoundTo);
            Control.ContextMenuStrip.Items.Add(itemDisplayAsHex);
            Control.ContextMenuStrip.Items.Add(itemDisplayAsNegated);
        }

        public override string HandleRounding(string stringValue)
        {
            double? doubleValueNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
            if (!doubleValueNullable.HasValue) return stringValue;
            double doubleValue = doubleValueNullable.Value;
            if (_roundingLimit.HasValue) doubleValue = Math.Round(doubleValue, _roundingLimit.Value);
            return doubleValue.ToString();
        }

        public override string HandleNegating(string stringValue)
        {
            double? doubleValueNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
            if (!doubleValueNullable.HasValue) return stringValue;
            double doubleValue = doubleValueNullable.Value;
            if (_displayAsNegated) doubleValue = -1 * doubleValue;
            return doubleValue.ToString();
        }

        public override string HandleUnnegating(string stringValue)
        {
            return HandleNegating(stringValue);
        }

        public override string HandleHexDisplaying(string stringValue)
        {
            if (!_displayAsHex) return stringValue;

            string numHexDigits = AddressHolder.ByteCount > 0 ? (AddressHolder.ByteCount * 2).ToString() : "";

            int ? intValueNullable = ParsingUtilities.ParseIntNullable(stringValue);
            if (intValueNullable.HasValue)
            {
                return String.Format("0x{0:X" + numHexDigits + "}", intValueNullable.Value);
            }

            uint? uintValueNullable = ParsingUtilities.ParseUIntNullable(stringValue);
            if (uintValueNullable.HasValue)
            {
                return String.Format("0x{0:X" + numHexDigits + "}", uintValueNullable.Value);
            }

            return stringValue;
        }

        public override string HandleHexUndisplaying(string value)
        {
            if (value != null && value.Length >= 2 && value.Substring(0,2) == "0x")
            {
                uint? parsed = ParsingUtilities.ParseHexNullable(value);
                if (parsed != null) return parsed.ToString();
            }
            return value;
        }
    }
}

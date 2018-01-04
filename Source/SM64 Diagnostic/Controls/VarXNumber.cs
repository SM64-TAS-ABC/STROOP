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
        private bool _negate = false;

        public VarXNumber(string name, AddressHolder addressHolder, int? roundingLimit = 3)
            : base(name, addressHolder)
        {
            if (roundingLimit.HasValue && (roundingLimit.Value < 0 || roundingLimit.Value > MAX_ROUNDING_LIMIT))
            {
                throw new ArgumentOutOfRangeException();
            }
            _roundingLimit = roundingLimit;
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

            ToolStripMenuItem itemNegate = new ToolStripMenuItem("Negate");
            itemNegate.Click += (sender, e) =>
            {
                _negate = !_negate;
                itemNegate.Checked = _negate;
            };

            Control.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            Control.ContextMenuStrip.Items.Add(itemRoundTo);
            Control.ContextMenuStrip.Items.Add(itemNegate);
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
            if (_negate) doubleValue = -1 * doubleValue;
            return doubleValue.ToString();
        }

        public override string HandleUnnegating(string stringValue)
        {
            return HandleNegating(stringValue);
        }
    }
}

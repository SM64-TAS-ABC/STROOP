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
    public class VarXAngle : VarXNumber
    {
        private bool _signed;
        private AngleUnitType _angleUnitType;
        private bool _truncateToMultipleOf16;
        private bool _constrainToOneRevolution;

        public VarXAngle(
            AddressHolder addressHolder,
            VarXControl varXControl,
            bool? signed)
            : base(addressHolder, varXControl, 0)
        {
            _signed = _addressHolder.IsSpecial ?
                signed ?? false :
                _addressHolder.SignedType.Value;
            _angleUnitType = AngleUnitType.InGameUnits;
            _truncateToMultipleOf16 = false;
            _constrainToOneRevolution = false;

            AddAngleContextMenuStripItems();
        }

        private void AddAngleContextMenuStripItems()
        {
            ToolStripMenuItem itemSigned = new ToolStripMenuItem("Signed");
            itemSigned.Click += (sender, e) =>
            {
                _signed = !_signed;
                itemSigned.Checked = _signed;
            };
            itemSigned.Checked = _signed;

            ToolStripMenuItem itemUnits = new ToolStripMenuItem("Units...");
            ControlUtilities.AddDropDownItems(
                itemUnits,
                new List<string> { "In-Game Units", "Degrees", "Radians", "Revolutions" },
                new List<object> { AngleUnitType.InGameUnits, AngleUnitType.Degrees, AngleUnitType.Radians, AngleUnitType.Revolutions },
                (object obj) => { _angleUnitType = (AngleUnitType)obj; },
                _angleUnitType);

            ToolStripMenuItem itemTruncateToMultipleOf16 = new ToolStripMenuItem("Truncate to Multiple of 16");
            itemTruncateToMultipleOf16.Click += (sender, e) =>
            {
                _truncateToMultipleOf16 = !_truncateToMultipleOf16;
                itemTruncateToMultipleOf16.Checked = _truncateToMultipleOf16;
            };
            itemTruncateToMultipleOf16.Checked = _truncateToMultipleOf16;

            ToolStripMenuItem itemConstrainToOneRevolution = new ToolStripMenuItem("Constrain to One Revolution");
            itemConstrainToOneRevolution.Click += (sender, e) =>
            {
                _constrainToOneRevolution = !_constrainToOneRevolution;
                itemConstrainToOneRevolution.Checked = _constrainToOneRevolution;
            };
            itemConstrainToOneRevolution.Checked = _constrainToOneRevolution;

            _contextMenuStrip.Items.Add(new ToolStripSeparator());
            _contextMenuStrip.Items.Add(itemSigned);
            _contextMenuStrip.Items.Add(itemUnits);
            _contextMenuStrip.Items.Add(itemTruncateToMultipleOf16);
            _contextMenuStrip.Items.Add(itemConstrainToOneRevolution);
        }

        private double GetAngleUnitTypeMaxValue(AngleUnitType? angleUnitTypeNullable = null)
        {
            AngleUnitType angleUnitType = angleUnitTypeNullable ?? _angleUnitType;
            switch (angleUnitType)
            {
                case AngleUnitType.InGameUnits:
                    return 65536;
                case AngleUnitType.Degrees:
                    return 360;
                case AngleUnitType.Radians:
                    return 2 * Math.PI;
                case AngleUnitType.Revolutions:
                    return 1;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private double GetAngleUnitTypeAndMaybeSignedMaxValue(AngleUnitType? angleUnitTypeNullable = null, bool? signedNullable = null)
        {
            AngleUnitType angleUnitType = angleUnitTypeNullable ?? _angleUnitType;
            bool signed = signedNullable ?? _signed;
            double maxValue = GetAngleUnitTypeMaxValue(angleUnitType);
            return signed ? maxValue / 2 : maxValue;
        }

        private double GetAngleUnitTypeAndMaybeSignedMinValue(AngleUnitType? angleUnitTypeNullable = null, bool? signedNullable = null)
        {
            AngleUnitType angleUnitType = angleUnitTypeNullable ?? _angleUnitType;
            bool signed = signedNullable ?? _signed;
            double maxValue = GetAngleUnitTypeMaxValue(angleUnitType);
            return signed ? -1 * maxValue / 2 : 0;
        }

        protected override string HandleAngleConverting(string stringValue)
        {
            double? doubleValueNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
            if (!doubleValueNullable.HasValue) return stringValue;
            double doubleValue = doubleValueNullable.Value;

            if (_constrainToOneRevolution)
            {
                doubleValue = _signed ?
                    MoreMath.MaybeNegativeModulus(doubleValue, 65536) :
                    MoreMath.NonNegativeModulus(doubleValue, 65536);
            }
            doubleValue = (doubleValue / 65536) * GetAngleUnitTypeMaxValue();

            return doubleValue.ToString();
        }

        protected override string HandleAngleUnconverting(string stringValue)
        {
            double? doubleValueNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
            if (!doubleValueNullable.HasValue) return stringValue;
            double doubleValue = doubleValueNullable.Value;

            doubleValue = (doubleValue / GetAngleUnitTypeMaxValue()) * 65536;
            if (_constrainToOneRevolution)
            {
                doubleValue = _signed ?
                    MoreMath.MaybeNegativeModulus(doubleValue, 65536) :
                    MoreMath.NonNegativeModulus(doubleValue, 65536);
            }

            return doubleValue.ToString();
        }

        protected override string HandleAngleRoundingOut(string stringValue)
        {
            double? doubleValueNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
            if (!doubleValueNullable.HasValue) return stringValue;
            double doubleValue = doubleValueNullable.Value;

            if (doubleValue == GetAngleUnitTypeAndMaybeSignedMaxValue()) doubleValue = GetAngleUnitTypeAndMaybeSignedMinValue();

            return doubleValue.ToString();
        }
    }
}

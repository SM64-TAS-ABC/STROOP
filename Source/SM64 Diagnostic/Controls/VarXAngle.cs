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
        private readonly bool _recommendedSigned;
        private bool? _currentSigned;
        private bool _effectiveSigned { get { return _currentSigned ?? _recommendedSigned; } }

        private AngleUnitType _angleUnitType;

        public VarXAngle(
            string name,
            AddressHolder addressHolder,
            Color? backgroundColor,
            bool recommendedSigned,
            AngleUnitType angleUnitType = AngleUnitType.InGameUnits)
            : base(name, addressHolder, backgroundColor, 0)
        {
            _recommendedSigned = recommendedSigned;
            _currentSigned = null;
            _angleUnitType = angleUnitType;

            AddAngleContextMenuStripItems();
        }

        protected void AddAngleContextMenuStripItems()
        {
            ToolStripMenuItem itemSign = new ToolStripMenuItem("Sign...");
            ControlUtilities.AddDropDownItems(
                itemSign,
                new List<string> { "Recommended", "Signed", "Unsigned" },
                new List<object> { null, true, false },
                (object obj) => { _currentSigned = (bool?)obj; },
                null);

            ToolStripMenuItem itemUnits = new ToolStripMenuItem("Units...");
            ControlUtilities.AddDropDownItems(
                itemUnits,
                new List<string> { "In-Game Units", "Degrees", "Radians", "Revolutions" },
                new List<object> { AngleUnitType.InGameUnits, AngleUnitType.Degrees, AngleUnitType.Radians, AngleUnitType.Revolutions },
                (object obj) => { _angleUnitType = (AngleUnitType)obj; },
                _angleUnitType);

            _contextMenuStrip.Items.Add(new ToolStripSeparator());
            _contextMenuStrip.Items.Add(itemSign);
            _contextMenuStrip.Items.Add(itemUnits);
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
            bool signed = signedNullable ?? _effectiveSigned;
            double maxValue = GetAngleUnitTypeMaxValue(angleUnitType);
            return signed ? maxValue / 2 : maxValue;
        }

        private double GetAngleUnitTypeAndMaybeSignedMinValue(AngleUnitType? angleUnitTypeNullable = null, bool? signedNullable = null)
        {
            AngleUnitType angleUnitType = angleUnitTypeNullable ?? _angleUnitType;
            bool signed = signedNullable ?? _effectiveSigned;
            double maxValue = GetAngleUnitTypeMaxValue(angleUnitType);
            return signed ? -1 * maxValue / 2 : 0;
        }

        public override string HandleAngleConverting(string stringValue)
        {
            double? doubleValueNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
            if (!doubleValueNullable.HasValue) return stringValue;
            double doubleValue = doubleValueNullable.Value;

            doubleValue = _effectiveSigned ?
                    MoreMath.MaybeNegativeModulus(doubleValue, 65536) :
                    MoreMath.NonNegativeModulus(doubleValue, 65536);
            doubleValue = (doubleValue / 65536) * GetAngleUnitTypeMaxValue();

            return doubleValue.ToString();
        }

        public override string HandleAngleUnconverting(string stringValue)
        {
            double? doubleValueNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
            if (!doubleValueNullable.HasValue) return stringValue;
            double doubleValue = doubleValueNullable.Value;

            doubleValue = (doubleValue / GetAngleUnitTypeMaxValue()) * 65536;
            doubleValue = _effectiveSigned ?
                MoreMath.MaybeNegativeModulus(doubleValue, 65536) :
                MoreMath.NonNegativeModulus(doubleValue, 65536);

            return doubleValue.ToString();
        }

        public override string HandleAngleRoundingOut(string stringValue)
        {
            double? doubleValueNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
            if (!doubleValueNullable.HasValue) return stringValue;
            double doubleValue = doubleValueNullable.Value;

            if (doubleValue == GetAngleUnitTypeAndMaybeSignedMaxValue()) doubleValue = GetAngleUnitTypeAndMaybeSignedMinValue();

            return doubleValue.ToString();
        }
    }
}

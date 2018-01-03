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
            bool recommendedSigned,
            AngleUnitType angleUnitType = AngleUnitType.InGameUnits)
            : base(name, addressHolder, 0)
        {
            _recommendedSigned = recommendedSigned;
            _currentSigned = null;
            _angleUnitType = angleUnitType;
            AddAngleContextMenuStrip();
        }

        private void AddAngleContextMenuStrip()
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

            Control.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            Control.ContextMenuStrip.Items.Add(itemSign);
            Control.ContextMenuStrip.Items.Add(itemUnits);
        }

        public override List<object> GetValue()
        {
            return base.GetValue().ConvertAll(objValue =>
            {
                double? newValueNullable = ParsingUtilities.ParseDoubleNullable(objValue.ToString());
                if (!newValueNullable.HasValue) return objValue;
                double newValue = newValueNullable.Value;

                newValue = _effectiveSigned ?
                    MoreMath.MaybeNegativeModulus(newValue, 65536) :
                    MoreMath.NonNegativeModulus(newValue, 65536);

                newValue = (newValue / 65536) * GetAngleUnitTypeMaxValue();

                return (object)newValue;
            });
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

        public override string GetDisplayedValue(string stringValue)
        {
            stringValue = base.GetDisplayedValue(stringValue);
            double? newValueNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
            if (!newValueNullable.HasValue) return stringValue;
            double newValue = newValueNullable.Value;

            // Handle the case of the variable rounding to outside the accepted interval
            if (newValue == GetAngleUnitTypeAndMaybeSignedMaxValue()) newValue = GetAngleUnitTypeAndMaybeSignedMinValue();

            return newValue.ToString();
        }

        public override void SetValue(string stringValue)
        {
            double? newValueNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
            if (!newValueNullable.HasValue)
            {
                base.SetValue(stringValue);
                return;
            }
            double newValue = newValueNullable.Value;

            newValue = (newValue / GetAngleUnitTypeMaxValue()) * 65536;

            newValue = _effectiveSigned ?
                MoreMath.MaybeNegativeModulus(newValue, 65536) :
                MoreMath.NonNegativeModulus(newValue, 65536);

            base.SetValue(newValue.ToString());
        }
    }
}

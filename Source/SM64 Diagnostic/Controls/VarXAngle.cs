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
        private bool? _signed;
        private AngleUnitType _angleUnitType;

        public VarXAngle(
            string name,
            AddressHolder addressHolder,
            bool? signed = null,
            AngleUnitType angleUnitType = AngleUnitType.InGameUnits)
            : base(name, addressHolder, 0)
        {
            _signed = signed;
            AddAngleContextMenuStrip();
        }

        private void AddAngleContextMenuStrip()
        {
            ToolStripMenuItem itemSign = new ToolStripMenuItem("Sign...");
            ControlUtilities.AddDropDownItems(
                itemSign,
                new List<string> { "Recommended", "Signed", "Unsigned" },
                new List<object> { null, true, false },
                (object obj) => { _signed = (bool?)obj; },
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
            return base.GetValue();
        }

        public override void SetValue(string stringValue)
        {
            base.SetValue(stringValue);
        }
    }
}

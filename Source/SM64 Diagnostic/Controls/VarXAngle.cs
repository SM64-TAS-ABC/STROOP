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
        public VarXAngle(string name, AddressHolder addressHolder)
            : base(name, addressHolder, 0)
        {
            AddAngleContextMenuStrip();
        }

        private void AddAngleContextMenuStrip()
        {

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

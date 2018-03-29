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
    public class WatchVariableStringWrapper : WatchVariableWrapper
    {
        public WatchVariableStringWrapper(
            WatchVariable watchVar,
            WatchVariableControl watchVarControl)
            : base(watchVar, watchVarControl, DEFAULT_USE_CHECKBOX)
        {
            
        }
        
        protected override void HandleVerification(object value)
        {
            base.HandleVerification(value);
            if (!(value is string))
                throw new ArgumentOutOfRangeException(value + " is not a string");
        }
    }
}

using SM64_Diagnostic.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SM64_Diagnostic.Controls.AngleDataContainer;

namespace SM64_Diagnostic.Structs
{
    public class SpecialWatchVariable
    {
        public string Name;
        public bool IsAngle;
        public AngleViewModeType AngleViewMode;

        public SpecialWatchVariable(string name, bool isAngle = false, AngleViewModeType angleViewMode = AngleViewModeType.Unsigned)
        {
            Name = name;
            IsAngle = isAngle;
            AngleViewMode = angleViewMode;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using STROOP.Utilities;
using STROOP.Structs;
using STROOP.Extensions;
using System.Reflection;
using STROOP.Managers;
using STROOP.Structs.Configurations;

namespace STROOP.Controls
{
    public class WatchVariableControlSettings
    {
        public readonly bool ChangeAngleSigned;
        public readonly bool ChangeAngleSignedToDefault;
        public readonly bool NewAngleSigned;

        public WatchVariableControlSettings(
            bool changeAngleSigned = false,
            bool changeAngleSignedToDefault = false,
            bool newAngleSigned = false)
        {
            ChangeAngleSigned = changeAngleSigned;
            ChangeAngleSignedToDefault = changeAngleSignedToDefault;
            NewAngleSigned = newAngleSigned;
        }

    }
}

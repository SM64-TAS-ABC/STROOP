using STROOP.Controls;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using STROOP.Utilities;

namespace STROOP.Exceptions
{
    public class DolphinNotRunningGameException : Exception
    {
        public DolphinNotRunningGameException()
            : base("Dolphin running, but emulator hasn't started")
        {
        }
    }
}

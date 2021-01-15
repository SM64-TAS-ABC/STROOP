using STROOP.Controls;
using STROOP.Managers;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public static class WatchVariableData
    {
        public static Dictionary<string, List<WatchVariableControlPrecursor>> Dictionary =
            new Dictionary<string, List<WatchVariableControlPrecursor>>()
            {
                [@"Config/MarioData.xml"] = new List<WatchVariableControlPrecursor>()
                {

                },
            };
    }
}

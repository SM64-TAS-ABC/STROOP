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
    public static class HelpfulHintUtilities
    {
        public static string GetRandomHelpfulHint()
        {
            return System.DateTime.Now.ToString();
        }

    }
}

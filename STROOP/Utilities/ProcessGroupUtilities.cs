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
    public static class ProcessGroupUtilities
    {

        public static string GetProcessGroupDescription(uint processGroup)
        {
            switch (processGroup)
            {
                case 0:
                    return "Player";
                case 2:
                    return "Respawning";
                case 4:
                    return "Actor";
                case 5:
                    return "Pushable";
                case 6:
                    return "Level";
                case 8:
                    return "Default";
                case 9:
                    return "Surface";
                case 10:
                    return "Usable";
                case 11:
                    return "Spawner";
                case 12:
                    return "Unimportant";
                case uint.MaxValue:
                    return "Vacant";
                default:
                    return "Unknown";
            }
        }

    }
}

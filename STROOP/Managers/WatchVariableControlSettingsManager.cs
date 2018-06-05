using STROOP.Controls;
using STROOP.Managers;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STROOP.Structs
{
    public static class WatchVariableControlSettingsManager
    {
        private static List<WatchVariableControlSettings> settingsList =
            new List<WatchVariableControlSettings>();

        public static void AddSettings(WatchVariableControlSettings settings)
        {
            settingsList.Add(settings);
        }

        public static int GetSettingsLevel()
        {
            return settingsList.Count;
        }

        public static List<WatchVariableControlSettings> GetSettingsToApply(int currentLevel)
        {
            return settingsList.Skip(currentLevel).ToList();
        }
    };
}

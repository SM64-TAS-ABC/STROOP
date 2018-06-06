using STROOP.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs.Configurations
{
    public static class SavedSettingsConfig
    {
        private static bool _yawSigned;
        public static bool YawSigned
        {
            get => _yawSigned;
            set
            {
                _yawSigned = value;
                WatchVariableControlSettingsManager.AddSettings(
                    new WatchVariableControlSettings(
                        changeYawSigned: true, newYawSigned: value));
            }
        }

        public static void SaveSettings()
        {
            
        }
    }
}

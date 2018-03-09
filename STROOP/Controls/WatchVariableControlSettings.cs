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
    /**
     * Class for applying settings to a watch var control and wrapper.
     * For each setting, there are 3 variables:
     * (1) ChangeSetting: a boolean whether this setting should be changed
     * (2) ChangeSettingToDefault: a boolean whether the change should be to the default value
     * (3) NewSetting: the new value if we're not using the default value
     * 
     * When constructing this class, for each setting, either leave all 3 variables out, or:
     * (1) Set changeSetting to true and changeSettingToDefault to true
     * (2) Set changeSetting to true and newSetting to the new value
     */
    public class WatchVariableControlSettings
    {
        public readonly bool ChangeRoundingLimit;
        public readonly bool ChangeRoundingLimitToDefault;
        public readonly int? NewRoundingLimit;

        public readonly bool ChangeAngleSigned;
        public readonly bool ChangeAngleSignedToDefault;
        public readonly bool NewAngleSigned;

        public readonly bool ChangeAngleUnits;
        public readonly bool ChangeAngleUnitsToDefault;
        public readonly AngleUnitType NewAngleUnits;

        public readonly bool ChangeAngleHex;
        public readonly bool ChangeAngleHexToDefault;
        public readonly bool NewAngleHex;

        public WatchVariableControlSettings(
            bool changeRoundingLimit = false,
            bool changeRoundingLimitToDefault = false,
            int? newRoundingLimit = null,
            bool changeAngleSigned = false,
            bool changeAngleSignedToDefault = false,
            bool newAngleSigned = false,
            bool changeAngleUnits = false,
            bool changeAngleUnitsToDefault = false,
            AngleUnitType newAngleUnits = AngleUnitType.InGameUnits,
            bool changeAngleHex = false,
            bool changeAngleHexToDefault = false,
            bool newAngleHex = false)
        {
            ChangeRoundingLimit = changeRoundingLimit;
            ChangeRoundingLimitToDefault = changeRoundingLimitToDefault;
            NewRoundingLimit = newRoundingLimit;

            ChangeAngleSigned = changeAngleSigned;
            ChangeAngleSignedToDefault = changeAngleSignedToDefault;
            NewAngleSigned = newAngleSigned;

            ChangeAngleUnits = changeAngleUnits;
            ChangeAngleUnitsToDefault = changeAngleUnitsToDefault;
            NewAngleUnits = newAngleUnits;

            ChangeAngleHex = changeAngleHex;
            ChangeAngleHexToDefault = changeAngleHexToDefault;
            NewAngleHex = newAngleHex;
        }
    }
}

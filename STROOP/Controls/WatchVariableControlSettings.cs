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
        public readonly int NewRoundingLimit;

        public readonly bool ChangeDisplayAsHex;
        public readonly bool ChangeDisplayAsHexToDefault;
        public readonly bool NewDisplayAsHex;

        public readonly bool ChangeAngleSigned;
        public readonly bool ChangeAngleSignedToDefault;
        public readonly bool NewAngleSigned;

        public readonly bool ChangeYawSigned;
        public readonly bool ChangeYawSignedToDefault;
        public readonly bool NewYawSigned;

        public readonly bool ChangeAngleUnits;
        public readonly bool ChangeAngleUnitsToDefault;
        public readonly AngleUnitType NewAngleUnits;

        public readonly bool ChangeAngleTruncateToMultipleOf16;
        public readonly bool ChangeAngleTruncateToMultipleOf16ToDefault;
        public readonly bool NewAngleTruncateToMultipleOf16;

        public readonly bool ChangeAngleConstrainToOneRevolution;
        public readonly bool ChangeAngleConstrainToOneRevolutionToDefault;
        public readonly bool NewAngleConstrainToOneRevolution;

        public readonly bool ChangeAngleReverse;
        public readonly bool ChangeAngleReverseToDefault;
        public readonly bool NewAngleReverse;

        public readonly bool ChangeAngleDisplayAsHex;
        public readonly bool ChangeAngleDisplayAsHexToDefault;
        public readonly bool NewAngleDisplayAsHex;

        public readonly bool ChangeHighlighted;
        public readonly bool NewHighlighted;

        public readonly bool ChangeHighlightColor;
        public readonly Color? NewHighlightColor;

        public readonly bool ChangeBackgroundColor;
        public readonly bool ChangeBackgroundColorToDefault;
        public readonly Color? NewBackgroundColor;

        public readonly bool ChangeLocked;
        public readonly bool NewLocked;

        public readonly bool ChangeFixedAddress;
        public readonly bool ChangeFixedAddressToDefault;
        public readonly bool NewFixedAddress;

        public WatchVariableControlSettings(
            bool changeRoundingLimit = false,
            bool changeRoundingLimitToDefault = false,
            int newRoundingLimit = 0,

            bool changeDisplayAsHex = false,
            bool changeDisplayAsHexToDefault = false,
            bool newDisplayAsHex = false,

            bool changeAngleSigned = false,
            bool changeAngleSignedToDefault = false,
            bool newAngleSigned = false,

            bool changeYawSigned = false,
            bool changeYawSignedToDefault = false,
            bool newYawSigned = false,

            bool changeAngleUnits = false,
            bool changeAngleUnitsToDefault = false,
            AngleUnitType newAngleUnits = AngleUnitType.InGameUnits,

            bool changeAngleTruncateToMultipleOf16 = false,
            bool changeAngleTruncateToMultipleOf16ToDefault = false,
            bool newAngleTruncateToMultipleOf16 = false,

            bool changeAngleConstrainToOneRevolution = false,
            bool changeAngleConstrainToOneRevolutionToDefault = false,
            bool newAngleConstrainToOneRevolution = false,

            bool changeAngleReverse = false,
            bool changeAngleReverseToDefault = false,
            bool newAngleReverse = false,

            bool changeAngleDisplayAsHex = false,
            bool changeAngleDisplayAsHexToDefault = false,
            bool newAngleDisplayAsHex = false,

            bool changeHighlighted = false,
            bool newHighlighted = false,

            bool changeHighlightColor = false,
            Color? newHighlightColor = null,

            bool changeBackgroundColor = false,
            bool changeBackgroundColorToDefault = false,
            Color? newBackgroundColor = null,

            bool changeLocked = false,
            bool newLocked = false,

            bool changeFixedAddress = false,
            bool changeFixedAddressToDefault = false,
            bool newFixedAddress = false)
        {
            ChangeRoundingLimit = changeRoundingLimit;
            ChangeRoundingLimitToDefault = changeRoundingLimitToDefault;
            NewRoundingLimit = newRoundingLimit;

            ChangeDisplayAsHex = changeDisplayAsHex;
            ChangeDisplayAsHexToDefault = changeDisplayAsHexToDefault;
            NewDisplayAsHex = newDisplayAsHex;

            ChangeAngleSigned = changeAngleSigned;
            ChangeAngleSignedToDefault = changeAngleSignedToDefault;
            NewAngleSigned = newAngleSigned;

            ChangeYawSigned = changeYawSigned;
            ChangeYawSignedToDefault = changeYawSignedToDefault;
            NewYawSigned = newYawSigned;

            ChangeAngleUnits = changeAngleUnits;
            ChangeAngleUnitsToDefault = changeAngleUnitsToDefault;
            NewAngleUnits = newAngleUnits;

            ChangeAngleTruncateToMultipleOf16 = changeAngleTruncateToMultipleOf16;
            ChangeAngleTruncateToMultipleOf16ToDefault = changeAngleTruncateToMultipleOf16ToDefault;
            NewAngleTruncateToMultipleOf16 = newAngleTruncateToMultipleOf16;

            ChangeAngleConstrainToOneRevolution = changeAngleConstrainToOneRevolution;
            ChangeAngleConstrainToOneRevolutionToDefault = changeAngleConstrainToOneRevolutionToDefault;
            NewAngleConstrainToOneRevolution = newAngleConstrainToOneRevolution;

            ChangeAngleReverse = changeAngleReverse;
            ChangeAngleReverseToDefault = changeAngleReverseToDefault;
            NewAngleReverse = newAngleReverse;

            ChangeAngleDisplayAsHex = changeAngleDisplayAsHex;
            ChangeAngleDisplayAsHexToDefault = changeAngleDisplayAsHexToDefault;
            NewAngleDisplayAsHex = newAngleDisplayAsHex;

            ChangeHighlighted = changeHighlighted;
            NewHighlighted = newHighlighted;

            ChangeHighlightColor = changeHighlightColor;
            NewHighlightColor = newHighlightColor;

            ChangeBackgroundColor = changeBackgroundColor;
            ChangeBackgroundColorToDefault = changeBackgroundColorToDefault;
            NewBackgroundColor = newBackgroundColor;

            ChangeLocked = changeLocked;
            NewLocked = newLocked;

            ChangeFixedAddress = changeFixedAddress;
            ChangeFixedAddressToDefault = changeFixedAddressToDefault;
            NewFixedAddress = newFixedAddress;
        }
    }
}

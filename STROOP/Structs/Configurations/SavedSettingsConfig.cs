using STROOP.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace STROOP.Structs.Configurations
{
    public static class SavedSettingsConfig
    {
        public static bool IsLoaded = false;

        private static bool _yawSigned;
        public static bool YawSigned
        {
            get => _yawSigned;
            set
            {
                if (_yawSigned == value) return;
                _yawSigned = value;
                WatchVariableControlSettingsManager.AddSettings(
                    new WatchVariableControlSettings(
                        changeYawSigned: true, newYawSigned: value));
                if (IsLoaded) Save();
            }
        }

        /*
        private static bool _slotIndexsFromOne;
        public static bool SlotIndexsFromOne
        {
            get => _slotIndexsFromOne;
            set
            {
                if (_slotIndexsFromOne == value) return;
                _slotIndexsFromOne = value;
                if (IsLoaded) Save();
            }
        }
        */

        public static bool SlotIndexsFromOne = true;
        public static bool MoveCameraWithPu = true;
        public static bool ScaleDiagonalPositionControllerButtons = false;
        public static bool ExcludeDustForClosestObject = true;
        public static bool UseMisalignmentOffsetForDistanceToLine = true;
        public static bool DontRoundValuesToZero = true;

        public static bool NeutralizeTrianglesWith21 = true;
        public static short NeutralizeTriangleValue(bool? use21Nullable = null)
        {
            bool use21 = use21Nullable ?? NeutralizeTrianglesWith21;
            return (short)(use21 ? 21 : 0);
        }






        public static List<XElement> ToXML()
        {
            return new List<XElement>
            {
                new XElement("YawSigned", _yawSigned)
            };
        }

        public static void Save()
        {
            DialogUtilities.SaveXmlElements(
                FileType.Xml, "SavedSettings", ToXML(), @"Config/SavedSettings.xml");
        }
    }
}

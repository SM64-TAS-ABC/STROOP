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

        private static bool _slotIndexsFromOne = true;
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

        private static bool _moveCameraWithPu = true;
        public static bool MoveCameraWithPu
        {
            get => _moveCameraWithPu;
            set
            {
                if (_moveCameraWithPu == value) return;
                _moveCameraWithPu = value;
                if (IsLoaded) Save();
            }
        }

        public static bool _scaleDiagonalPositionControllerButtons = false;
        public static bool ScaleDiagonalPositionControllerButtons
        {
            get => _scaleDiagonalPositionControllerButtons;
            set
            {
                if (_scaleDiagonalPositionControllerButtons == value) return;
                _scaleDiagonalPositionControllerButtons = value;
                if (IsLoaded) Save();
            }
        }

        public static bool _excludeDustForClosestObject = true;
        public static bool ExcludeDustForClosestObject
        {
            get => _excludeDustForClosestObject;
            set
            {
                if (_excludeDustForClosestObject == value) return;
                _excludeDustForClosestObject = value;
                if (IsLoaded) Save();
            }
        }

        public static bool _useMisalignmentOffsetForDistanceToLine = true;
        public static bool UseMisalignmentOffsetForDistanceToLine
        {
            get => _useMisalignmentOffsetForDistanceToLine;
            set
            {
                if (_useMisalignmentOffsetForDistanceToLine == value) return;
                _useMisalignmentOffsetForDistanceToLine = value;
                if (IsLoaded) Save();
            }
        }

        public static bool _dontRoundValuesToZero = true;
        public static bool DontRoundValuesToZero
        {
            get => _dontRoundValuesToZero;
            set
            {
                if (_dontRoundValuesToZero == value) return;
                _dontRoundValuesToZero = value;
                if (IsLoaded) Save();
            }
        }

        public static bool _neutralizeTrianglesWith21 = true;
        public static bool NeutralizeTrianglesWith21
        {
            get => _neutralizeTrianglesWith21;
            set
            {
                if (_neutralizeTrianglesWith21 == value) return;
                _neutralizeTrianglesWith21 = value;
                if (IsLoaded) Save();
            }
        }

        public static short NeutralizeTriangleValue(bool? use21Nullable = null)
        {
            bool use21 = use21Nullable ?? NeutralizeTrianglesWith21;
            return (short)(use21 ? 21 : 0);
        }






        public static List<XElement> ToXML()
        {
            return new List<XElement>
            {
                new XElement("YawSigned", _yawSigned),
                new XElement("SlotIndexsFromOne", _slotIndexsFromOne),
                new XElement("MoveCameraWithPu", _moveCameraWithPu),
                new XElement("ScaleDiagonalPositionControllerButtons", _scaleDiagonalPositionControllerButtons),
                new XElement("ExcludeDustForClosestObject", _excludeDustForClosestObject),
                new XElement("UseMisalignmentOffsetForDistanceToLine", _useMisalignmentOffsetForDistanceToLine),
                new XElement("DontRoundValuesToZero", _dontRoundValuesToZero),
                new XElement("NeutralizeTrianglesWith21", _neutralizeTrianglesWith21),
            };
        }

        public static void Save()
        {
            DialogUtilities.SaveXmlElements(
                FileType.Xml, "SavedSettings", ToXML(), @"Config/SavedSettings.xml");
        }
    }
}

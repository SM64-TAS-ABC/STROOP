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

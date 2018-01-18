using SM64_Diagnostic.Extensions;
using SM64_Diagnostic.Managers;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Structs.Configurations;
using SM64_Diagnostic.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SM64_Diagnostic.Controls
{
    public class WatchVariableControlPrecursor
    {
        private readonly string _name;
        private readonly WatchVariable _watchVar;
        private readonly WatchVariableSubclass _subclass;
        private readonly Color? _backgroundColor;
        private readonly bool? _useHex;
        private readonly bool? _invertBool;
        private readonly WatchVariableCoordinate? _coordinate;
        private readonly List<VariableGroup> _groupList;

        public WatchVariableControlPrecursor(
            string name,
            WatchVariable watchVar,
            WatchVariableSubclass subclass,
            Color? backgroundColor,
            bool? useHex,
            bool? invertBool,
            WatchVariableCoordinate? coordinate,
            List<VariableGroup> groupList)
        {
            _name = name;
            _watchVar = watchVar;
            _subclass = subclass;
            _backgroundColor = backgroundColor;
            _useHex = useHex;
            _invertBool = invertBool;
            _coordinate = coordinate;
            _groupList = groupList;
        }

        public WatchVariableControl CreateWatchVariableControl(Color? newColor = null)
        {
            return new WatchVariableControl(
                this,
                _name,
                _watchVar,
                _subclass,
                newColor ?? _backgroundColor,
                _useHex,
                _invertBool,
                _coordinate,
                _groupList);
        }

        public override string ToString()
        {
            XmlBuilder xmlBuilder = new XmlBuilder();
            xmlBuilder.SetRootElement("Data");
            xmlBuilder.SetValue(_name);

            if (_groupList.Count > 0)
                xmlBuilder.SetElement("groupList", String.Join(",", _groupList));

            xmlBuilder.SetElement("baseAddressType", _watchVar.BaseAddressType.ToString());

            if (_watchVar.OffsetDefault != null)
                xmlBuilder.SetElement(
                    "offset",
                    String.Format("0x{0:X}", _watchVar.OffsetDefault.Value));

            if (_watchVar.OffsetUS != null)
                xmlBuilder.SetElement(
                    "offsetUS",
                    String.Format("0x{0:X}", _watchVar.OffsetUS.Value));

            if (_watchVar.OffsetJP != null)
                xmlBuilder.SetElement(
                    "offsetJP",
                    String.Format("0x{0:X}", _watchVar.OffsetJP.Value));

            if (_watchVar.OffsetPAL != null)
                xmlBuilder.SetElement(
                    "offsetPAL",
                    String.Format("0x{0:X}", _watchVar.OffsetPAL.Value));

            if (_watchVar.MemoryTypeName != null)
                xmlBuilder.SetElement("type", _watchVar.MemoryTypeName);

            if (_watchVar.SpecialType != null)
                xmlBuilder.SetElement("specialType", _watchVar.SpecialType);

            if (_watchVar.Mask != null)
                xmlBuilder.SetElement(
                    "mask",
                    String.Format("0x{0:X" + _watchVar.NibbleCount + "}", _watchVar.Mask.Value));

            if (_subclass != WatchVariableSubclass.Number)
                xmlBuilder.SetElement("subclass", _subclass.ToString());

            if (_invertBool.HasValue)
                xmlBuilder.SetElement("invertBool", _invertBool.Value.ToString().ToLower());

            if (_useHex.HasValue)
                xmlBuilder.SetElement("useHex", _useHex.Value.ToString().ToLower());

            if (_coordinate.HasValue)
                xmlBuilder.SetElement("coord", _coordinate.Value.ToString());

            if (_backgroundColor.HasValue)
                xmlBuilder.SetElement(
                    "color",
                    "#" + ColorUtilities.ToString(_backgroundColor.Value));

            return xmlBuilder.ToString();
        }

    }
}

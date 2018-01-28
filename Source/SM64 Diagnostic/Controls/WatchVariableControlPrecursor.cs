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
using System.Xml.Linq;

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
            XDocument xmlBuild = new XDocument();
            XElement root = new XElement("Data", _name);

            if (_groupList.Count > 0)
                root.Add(new XElement("groupList", String.Join(",", _groupList)));

            root.Add(new XElement("base", _watchVar.BaseAddressType.ToString()));

            if (_watchVar.OffsetDefault != null)
                root.Add(new XElement(
                    "offset",
                    String.Format("0x{0:X}", _watchVar.OffsetDefault.Value)));

            if (_watchVar.OffsetUS != null)
                root.Add(new XElement(
                    "offsetUS",
                    String.Format("0x{0:X}", _watchVar.OffsetUS.Value)));

            if (_watchVar.OffsetJP != null)
                root.Add(new XElement(
                    "offsetJP",
                    String.Format("0x{0:X}", _watchVar.OffsetJP.Value)));

            if (_watchVar.OffsetPAL != null)
                root.Add(new XElement(
                    "offsetPAL",
                    String.Format("0x{0:X}", _watchVar.OffsetPAL.Value)));

            if (_watchVar.MemoryTypeName != null)
                root.Add(new XElement("type", _watchVar.MemoryTypeName));

            if (_watchVar.SpecialType != null)
                root.Add(new XElement("specialType", _watchVar.SpecialType));

            if (_watchVar.Mask != null)
                root.Add(new XElement(
                    "mask",
                    String.Format("0x{0:X" + _watchVar.NibbleCount + "}", _watchVar.Mask.Value)));

            if (_subclass != WatchVariableSubclass.Number)
                root.Add(new XElement("subclass", _subclass.ToString()));

            if (_invertBool.HasValue)
                root.Add(new XElement("invertBool", _invertBool.Value.ToString().ToLower()));

            if (_useHex.HasValue)
                root.Add(new XElement("useHex", _useHex.Value.ToString().ToLower()));

            if (_coordinate.HasValue)
                root.Add(new XElement("coord", _coordinate.Value.ToString()));

            if (_backgroundColor.HasValue)
                root.Add(new XElement(
                    "color",
                    "#" + ColorUtilities.ToString(_backgroundColor.Value)));

            xmlBuild.Add(root);
            return root.ToString();
        }

    }
}

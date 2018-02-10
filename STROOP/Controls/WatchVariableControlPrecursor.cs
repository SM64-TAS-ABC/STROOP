using STROOP.Extensions;
using STROOP.Managers;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;

namespace STROOP.Controls
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
            xmlBuild.Add(root);

            if (_groupList.Count > 0)
                root.Add(new XAttribute("groupList", String.Join(",", _groupList)));

            root.Add(new XAttribute("base", _watchVar.BaseAddressType.ToString()));

            if (_watchVar.OffsetDefault != null)
                root.Add(new XAttribute(
                    "offset",
                    String.Format("0x{0:X}", _watchVar.OffsetDefault.Value)));

            if (_watchVar.OffsetUS != null)
                root.Add(new XAttribute(
                    "offsetUS",
                    String.Format("0x{0:X}", _watchVar.OffsetUS.Value)));

            if (_watchVar.OffsetJP != null)
                root.Add(new XAttribute(
                    "offsetJP",
                    String.Format("0x{0:X}", _watchVar.OffsetJP.Value)));

            if (_watchVar.OffsetPAL != null)
                root.Add(new XAttribute(
                    "offsetPAL",
                    String.Format("0x{0:X}", _watchVar.OffsetPAL.Value)));

            if (_watchVar.MemoryTypeName != null)
                root.Add(new XAttribute("type", _watchVar.MemoryTypeName));

            if (_watchVar.SpecialType != null)
                root.Add(new XAttribute("specialType", _watchVar.SpecialType));

            if (_watchVar.Mask != null)
                root.Add(new XAttribute(
                    "mask",
                    String.Format("0x{0:X" + _watchVar.NibbleCount + "}", _watchVar.Mask.Value)));

            if (_subclass != WatchVariableSubclass.Number)
                root.Add(new XAttribute("subclass", _subclass.ToString()));

            if (_invertBool.HasValue)
                root.Add(new XAttribute("invertBool", _invertBool.Value.ToString().ToLower()));

            if (_useHex.HasValue)
                root.Add(new XAttribute("useHex", _useHex.Value.ToString().ToLower()));

            if (_coordinate.HasValue)
                root.Add(new XAttribute("coord", _coordinate.Value.ToString()));

            if (_backgroundColor.HasValue)
                root.Add(new XAttribute(
                    "color",
                    "#" + ColorUtilities.ToString(_backgroundColor.Value)));

            return root.ToString();
        }

    }
}

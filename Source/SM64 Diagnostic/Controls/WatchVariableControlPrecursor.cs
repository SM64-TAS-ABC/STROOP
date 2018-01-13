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

        public WatchVariableControl CreateVarXControl(Color? newColor = null)
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

    }
}

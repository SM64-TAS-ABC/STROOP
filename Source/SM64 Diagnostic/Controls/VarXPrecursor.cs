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
    public class VarXPrecursor
    {
        private readonly string _name;
        private readonly AddressHolder _addressHolder;
        private readonly VarXSubclass _varXSubclass;
        private readonly Color? _backgroundColor;
        private readonly bool? _useHex;
        private readonly bool? _invertBool;
        private readonly VarXCoordinate? _coordinate;
        private readonly List<VariableGroup> _groupList;

        public VarXPrecursor(
            string name,
            AddressHolder addressHolder,
            VarXSubclass varXSubclass,
            Color? backgroundColor,
            bool? useHex,
            bool? invertBool,
            VarXCoordinate? coordinate,
            List<VariableGroup> groupList)
        {
            _name = name;
            _addressHolder = addressHolder;
            _varXSubclass = varXSubclass;
            _backgroundColor = backgroundColor;
            _useHex = useHex;
            _invertBool = invertBool;
            _coordinate = coordinate;
            _groupList = groupList;
        }

        public VarXControl CreateVarXControl(Color? newColor = null)
        {
            return new VarXControl(
                this,
                _name,
                _addressHolder,
                _varXSubclass,
                newColor ?? _backgroundColor,
                _useHex,
                _invertBool,
                _coordinate,
                _groupList);
        }

    }
}

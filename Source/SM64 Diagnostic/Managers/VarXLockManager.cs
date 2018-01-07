using SM64_Diagnostic.Controls;
using SM64_Diagnostic.Managers;
using SM64_Diagnostic.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public static class VarXLockManager
    {
        private class VarXLock
        {
            public readonly AddressHolder Variable;
            private string _value;
            public string Value { get { return _value; } }

            public VarXLock(AddressHolder variable, string value)
            {
                Variable = variable;
                _value = value;
            }

            public void Update()
            {
                Variable.SetValue(_value);
            }

            public void UpdateLockValue(string value)
            {
                _value = value;
            }
        }

        private static List<VarXLock> _lockList = new List<VarXLock>();
        private static Dictionary<AddressHolder, VarXLock> _lockDict = new Dictionary<AddressHolder, VarXLock>();

        public static void AddLock(AddressHolder variable, string value)
        {
            VarXLock varXLock = new VarXLock(variable, value);
            _lockList.Add(varXLock);
            _lockDict.Add(variable, varXLock);
        }

        public static void RemoveLock(AddressHolder variable)
        {
            if (!_lockDict.ContainsKey(variable)) return;

            VarXLock varLock = _lockDict[variable];
            _lockList.Remove(varLock);
            _lockDict.Remove(variable);
        }

        public static bool ContainsLock(AddressHolder variable)
        {
            return _lockDict.ContainsKey(variable);
        }

        public static void UpdateLockValue(AddressHolder variable, string value)
        {
            if (!_lockDict.ContainsKey(variable)) return;

            VarXLock varLock = _lockDict[variable];
            varLock.UpdateLockValue(value);
        }

        public static void Update()
        {
            _lockList.ForEach(varLock => varLock.Update());
        }

    };
}

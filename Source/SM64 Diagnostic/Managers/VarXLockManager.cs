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
            public readonly string Value;

            public VarXLock(AddressHolder variable, string value)
            {
                Variable = variable;
                Value = value;
            }

            public void Update()
            {
                Variable.SetValue(Value);
            }
        }

        private static List<VarXLock> _lockList = new List<VarXLock>();

        public static void AddLock(AddressHolder variable, string value)
        {
            VarXLock varXLock = new VarXLock(variable, value);
            _lockList.Add(varXLock);
        }

        public static void RemoveLock(AddressHolder variable)
        {
            _lockList.RemoveAll(varLock => varLock.Variable == variable);
        }

        public static bool ContainsLock(AddressHolder variable)
        {
            return _lockList.Any(varLock => varLock.Variable == variable);
        }

        public static void Update()
        {
            _lockList.ForEach(varLock => varLock.Update());
        }

    };
}

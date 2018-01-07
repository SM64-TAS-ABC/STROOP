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
            public readonly VarX Variable;
            public readonly string Value;

            public VarXLock(VarX variable, string value)
            {
                Variable = variable;
                Value = value;
            }
        }

        private static List<VarXLock> _lockList = new List<VarXLock>();

        public static void AddLock(VarX varX, string value)
        {
            VarXLock varXLock = new VarXLock(varX, value);
            _lockList.Add(varXLock);
        }

        public static void RemoveLock(VarX varX)
        {
            _lockList.RemoveAll(varLock => varLock.Variable == varX);
        }

        public static bool ContainsLock(VarX varX)
        {
            return _lockList.Any(varLock => varLock.Variable == varX);
        }

    };
}

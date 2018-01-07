using SM64_Diagnostic.Controls;
using SM64_Diagnostic.Managers;
using SM64_Diagnostic.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM64_Diagnostic.Structs
{
    public static class VarXLockManager
    {
        private static List<AddressHolderLock> _lockList = new List<AddressHolderLock>();

        public static void AddLocks(AddressHolder variable)
        {
            List<AddressHolderLock> newLocks = variable.GetLocks();
            foreach (AddressHolderLock newLock in newLocks)
            {
                if (!_lockList.Contains(newLock)) _lockList.Add(newLock);
            }
        }

        public static void RemoveLocks(AddressHolder variable)
        {
            List<AddressHolderLock> newLocks = variable.GetLocks();
            foreach (AddressHolderLock newLock in newLocks)
            {
                _lockList.Remove(newLock);
            }
        }

        public static bool ContainsLocksBool(AddressHolder variable)
        {
            return ContainsLocksCheckState(variable) != CheckState.Unchecked;
        }

        public static CheckState ContainsLocksCheckState(AddressHolder variable)
        {
            if (!ContainsAnyLocks()) return CheckState.Unchecked;
            List<AddressHolderLock> newLocks = variable.GetLocks();

            if (newLocks.Count == 0) return CheckState.Unchecked;
            CheckState firstCheckState =
                _lockList.Contains(newLocks[0]) ? CheckState.Checked : CheckState.Unchecked;
            for (int i = 1; i < newLocks.Count; i++)
            {
                CheckState checkState =
                    _lockList.Contains(newLocks[i]) ? CheckState.Checked : CheckState.Unchecked;
                if (checkState != firstCheckState) return CheckState.Indeterminate;
            }
            return firstCheckState;
        }

        public static void UpdateLockValues(AddressHolder variable, string newValue)
        {
            List<AddressHolderLock> newLocks = variable.GetLocks();
            foreach (AddressHolderLock newLock in newLocks)
            {
                AddressHolderLock currentLock = _lockList.FirstOrDefault(current => current.Equals(newLock));
                if (currentLock == null) continue;
                currentLock.UpdateLockValue(newValue);
            }
        }

        public static void RemoveAllLocks()
        {
            _lockList.Clear();
        }

        public static bool ContainsAnyLocks()
        {
            return _lockList.Count > 0;
        }

        public static void Update()
        {
            _lockList.ForEach(varLock => varLock.Invoke());
        }

    };
}

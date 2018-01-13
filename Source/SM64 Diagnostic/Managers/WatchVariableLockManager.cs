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
    public static class WatchVariableLockManager
    {
        private static List<WatchVariableLock> _lockList = new List<WatchVariableLock>();

        public static void AddLocks(WatchVariable variable)
        {
            List<WatchVariableLock> newLocks = variable.GetLocks();
            foreach (WatchVariableLock newLock in newLocks)
            {
                if (!_lockList.Contains(newLock)) _lockList.Add(newLock);
            }
        }

        public static void RemoveLocks(WatchVariable variable)
        {
            List<WatchVariableLock> newLocks = variable.GetLocks();
            foreach (WatchVariableLock newLock in newLocks)
            {
                _lockList.Remove(newLock);
            }
        }

        public static bool ContainsLocksBool(WatchVariable variable)
        {
            return ContainsLocksCheckState(variable) != CheckState.Unchecked;
        }

        public static CheckState ContainsLocksCheckState(WatchVariable variable)
        {
            if (!ContainsAnyLocks()) return CheckState.Unchecked;
            List<WatchVariableLock> newLocks = variable.GetLocks();

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

        public static void UpdateLockValues(WatchVariable variable, string newValue)
        {
            List<WatchVariableLock> newLocks = variable.GetLocks();
            foreach (WatchVariableLock newLock in newLocks)
            {
                WatchVariableLock currentLock = _lockList.FirstOrDefault(current => current.Equals(newLock));
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

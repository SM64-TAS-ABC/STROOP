using STROOP.Controls;
using STROOP.Forms;
using STROOP.Managers;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STROOP.Structs
{
    public class WatchVariableLockManager
    {
        private static readonly Image LockBlackClosed = Properties.Resources.lock_black;
        private static readonly Image LockBlackOpen = Properties.Resources.lock_black_open;
        private static readonly Image LockBlueClosed = Properties.Resources.lock_blue;
        private static readonly Image LockBlueOpen = Properties.Resources.lock_blue_open;
        private static readonly Image LockRedClosed = Properties.Resources.lock_red;
        private static readonly Image LockRedOpen = Properties.Resources.lock_red_open;

        private readonly PictureBox _pictureBoxLock;
        private readonly List<WatchVariableLock> _lockList;

        private readonly ToolStripMenuItem _itemRemoveAllLocks;
        private readonly ToolStripMenuItem _itemSpecificLock;
        private readonly ToolStripMenuItem _itemDisableLocking;
        private readonly ToolStripMenuItem _itemSeeLockInfo;

        public bool IsInvokingLocks = false;

        public WatchVariableLockManager(PictureBox pictureBoxLock)
        {
            _lockList = new List<WatchVariableLock>();

            _pictureBoxLock = pictureBoxLock;
            _pictureBoxLock.ContextMenuStrip = new ContextMenuStrip();
            _pictureBoxLock.Click += (sender, e) => _pictureBoxLock.ContextMenuStrip.Show(Cursor.Position);
            _pictureBoxLock.ContextMenuStrip.Opening += (sender, e) => UpdateSpecificLocks();

            _itemRemoveAllLocks = new ToolStripMenuItem("Remove All Locks");
            _itemRemoveAllLocks.Click += (sender, e) => Config.LockManager.RemoveAllLocks();
            _pictureBoxLock.ContextMenuStrip.Items.Add(_itemRemoveAllLocks);

            _itemSpecificLock = new ToolStripMenuItem("Remove Specific Lock...");
            _pictureBoxLock.ContextMenuStrip.Items.Add(_itemSpecificLock);

            _itemDisableLocking = new ToolStripMenuItem("Disable Locking");
            _itemDisableLocking.Click += (sender, e) => LockConfig.LockingDisabled = !LockConfig.LockingDisabled;
            _pictureBoxLock.ContextMenuStrip.Items.Add(_itemDisableLocking);

            _itemSeeLockInfo = new ToolStripMenuItem("See Lock Info");
            _itemSeeLockInfo.Click += (sender, e) => SeeLockInfo();
            _pictureBoxLock.ContextMenuStrip.Items.Add(_itemSeeLockInfo);
        }

        // for tests
        public WatchVariableLockManager()
        {
            _lockList = new List<WatchVariableLock>();
        }

        public void AddLocks(WatchVariable variable, List<uint> addresses = null)
        {
            List<WatchVariableLock> newLocks = variable.GetLocks(addresses);
            foreach (WatchVariableLock newLock in newLocks)
            {
                if (!_lockList.Contains(newLock)) _lockList.Add(newLock);
            }
        }

        public void RemoveLocks(WatchVariable variable, List<uint> addresses = null)
        {
            List<WatchVariableLock> newLocks = variable.GetLocks(addresses);
            foreach (WatchVariableLock newLock in newLocks)
            {
                _lockList.Remove(newLock);
            }
        }

        public bool ContainsLocksBool(WatchVariable variable, List<uint> addresses = null)
        {
            return ContainsLocksCheckState(variable, addresses) != CheckState.Unchecked;
        }

        public CheckState ContainsLocksCheckState(
            WatchVariable variable, List<uint> addresses = null)
        {
            if (!ContainsAnyLocks()) return CheckState.Unchecked;
            List<WatchVariableLock> newLocks = variable.GetLocks(addresses);

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

        public List<object> GetExistingLockValues(
            WatchVariable variable, List<uint> addresses = null)
        {
            if (LockConfig.LockingDisabled) return null;
            // don't get the locks with values, or there's a stack overflow error
            List<WatchVariableLock> locks = variable.GetLocksWithoutValues(addresses);
            List<object> returnValues = new List<object>();
            foreach (WatchVariableLock lok in locks)
            {
                WatchVariableLock existingLock = _lockList.FirstOrDefault(l => l.Equals(lok));
                object value = existingLock?.Value;
                returnValues.Add(value);
            }
            return returnValues;
        }

        public void UpdateLockValues(
            WatchVariable variable, object newValue, List<uint> addresses = null)
        {
            if (LockConfig.LockingDisabled) return;
            if (!ContainsAnyLocks()) return;
            List<WatchVariableLock> newLocks = variable.GetLocks(addresses);
            foreach (WatchVariableLock newLock in newLocks)
            {
                foreach (WatchVariableLock currentLock in _lockList)
                {
                    if (currentLock.Equals(newLock))
                    {
                        currentLock.UpdateLockValue(newValue);
                    }
                }
            }
        }

        public void UpdateLockValues(
            WatchVariable variable, List<object> newValues, List<uint> addresses = null)
        {
            if (LockConfig.LockingDisabled) return;
            if (!ContainsAnyLocks()) return;
            List<WatchVariableLock> newLocks = variable.GetLocks(addresses);
            int minCount = Math.Min(newValues.Count, newLocks.Count);
            for (int i = 0; i < minCount; i++)
            {
                if (newValues[i] == null) continue;
                foreach (WatchVariableLock currentLock in _lockList)
                {
                    if (currentLock.Equals(newLocks[i]))
                    {
                        currentLock.UpdateLockValue(newValues[i]);
                    }
                }
            }
        }

        public object GetMemoryLockValue(
            uint address, Type type, uint? mask, int? shift)
        {
            if (LockConfig.LockingDisabled) return null;
            if (!ContainsAnyLocks()) return null;
            foreach (WatchVariableLock currentLock in _lockList)
            {
                if (currentLock.EqualsMemorySignature(address, type, mask, shift))
                {
                    return currentLock.Value;
                }
            }
            return null;
        }

        public void UpdateMemoryLockValue(
            object newValue, uint address, Type type, uint? mask, int? shift)
        {
            if (LockConfig.LockingDisabled) return;
            if (!ContainsAnyLocks()) return;
            foreach (WatchVariableLock currentLock in _lockList)
            {
                if (currentLock.EqualsMemorySignature(address, type, mask, shift))
                {
                    currentLock.UpdateLockValue(newValue);
                }
            }
        }

        public void RemoveAllLocks()
        {
            _lockList.Clear();
        }

        public bool ContainsAnyLocks()
        {
            return _lockList.Count > 0;
        }

        public bool ContainsAnyLocksForObject(uint objAddress)
        {
            return _lockList.Any(lok => lok.BaseAddress == objAddress);
        }

        private Image GetImageForLock()
        {
            if (_lockList.Count > 0)
            {
                if (Config.Stream.Readonly) return LockRedClosed;
                if (LockConfig.LockingDisabled) return LockBlueClosed;
                return LockBlackClosed;
            }
            else
            {
                if (Config.Stream.Readonly) return LockRedOpen;
                if (LockConfig.LockingDisabled) return LockBlueOpen;
                return LockBlackOpen;
            }
        }

        private void SeeLockInfo()
        {
            List<string> lines = new List<string>();
            lines.Add(WatchVariableLock.GetHeaderLine());
            foreach (WatchVariableLock lok in _lockList)
            {
                lines.Add(lok.ToString());
            }
            InfoForm.ShowValue(string.Join("\r\n", lines), "Lock Info", "Lock Info");
        }

        private void UpdateSpecificLocks()
        {
            List<ToolStripMenuItem> items = _lockList.ConvertAll(lok =>
            {
                ToolStripMenuItem item = new ToolStripMenuItem(lok.GetCondensedInfo());
                item.Click += (sender, e) => _lockList.Remove(lok);
                return item;
            });
            _itemSpecificLock.DropDownItems.Clear();
            _itemSpecificLock.DropDownItems.AddRange(items.ToArray());
        }

        public void Update()
        {
            // FOR DEBUGGING LOCKS
            //List<string> stringValues = _lockList.ConvertAll(l => HexUtilities.FormatValue(l.Address) + "=" + HexUtilities.FormatValue(l.Value));
            //string stringValue = string.Join(" ", stringValues);
            //Config.SetDebugText(stringValue);

            _itemRemoveAllLocks.Text = string.Format(
                "Remove All Locks ({0})",
                _lockList.Count);
            _itemDisableLocking.Checked = LockConfig.LockingDisabled;
            Image lockImage = GetImageForLock();
            if (_pictureBoxLock.BackgroundImage != lockImage)
            {
                _pictureBoxLock.BackgroundImage = lockImage;
            }

            if (LockConfig.LockingDisabled) return;
            bool shouldSuspend = _lockList.Count >= 2;
            if (shouldSuspend) Config.Stream.Suspend();
            InvokeLocks();
            if (shouldSuspend) Config.Stream.Resume();
        }

        private void InvokeLocks()
        {
            IsInvokingLocks = true;
            _lockList.ForEach(lok => lok.Invoke());
            IsInvokingLocks = false;
        }
    };
}

using STROOP.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static STROOP.Utilities.Kernal32NativeMethods;

namespace STROOP.Utilities
{
    class DolphinProcessIO : WindowsProcessRamIO
    {
        public DolphinProcessIO(Process process, Emulator emulator, uint ramSize)
            : base(process, emulator, ramSize) { }

        protected override void CalculateOffset()
        {
            MemoryBasicInformation info;
            IntPtr infoSize = (IntPtr)Marshal.SizeOf(typeof(MemoryBasicInformation));
            uint setInfoSize = (uint)Marshal.SizeOf(typeof(PsapiWorkingSetExInformation));

            _baseOffset = (UIntPtr)0;
            bool mem1Found = false;
            for (IntPtr p = new IntPtr(); VQueryEx(_processHandle, p, out info, infoSize) == infoSize; 
                p = (IntPtr) (p.ToInt64() + info.RegionSize.ToInt64()))
            {
                if (mem1Found)
                {
                    if (info.BaseAddress == _baseOffset + 0x10000000)
                    {
                        break;
                    }
                    else if (info.BaseAddress.ToUInt64() > _baseOffset.ToUInt64() + 0x10000000)
                    {
                        break;
                    }
                    continue;
                }

                if (info.RegionSize == (IntPtr)0x2000000 && info.Type == MemoryType.MEM_MAPPED)
                {
                    // Here, it's likely the right page, but it can happen that multiple pages with these criteria
                    // exists and have nothing to do with the emulated memory. Only the right page has valid
                    // working set information so an additional check is required that it is backed by physical
                    // memory.
                    PsapiWorkingSetExInformation wsInfo;
                    wsInfo.VirtualAddress = (IntPtr)info.BaseAddress.ToUInt64();
                    if (QWorkingSetEx(_processHandle, out wsInfo, setInfoSize))
                    {
                        if ((wsInfo.VirtualAttributes & 0x01) != 0)
                        {
                            _baseOffset = info.BaseAddress;
                            mem1Found = true;
                        }
                    }
                }
            }
            if (_baseOffset.ToUInt64() == 0)
                throw new ArgumentNullException("Dolphin running, but emulator hasn't started");

            _baseOffset = (UIntPtr)(_baseOffset.ToUInt64() + _emulator.RamStart);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;

namespace STROOP.Utilities
{
    interface IEmuRamIO : IDisposable
    {
        bool Suspend();
        bool Resume();

        bool IsSuspended { get; }

        bool ReadRelative(uint address, byte[] buffer, EndianessType endianess);
        bool ReadAbsolute(UIntPtr address, byte[] buffer, EndianessType endianess);
        bool WriteRelative(uint address, byte[] buffer, EndianessType endianess);
        bool WriteAbsolute(UIntPtr address, byte[] buffer, EndianessType endianess);

        UIntPtr GetAbsoluteAddress(uint n64Address, int size);
        uint GetRelativeAddress(UIntPtr absoluteAddress, int size);

        event EventHandler OnClose;
    }
}

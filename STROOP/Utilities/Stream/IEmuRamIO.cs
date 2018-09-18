using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;

namespace STROOP.Utilities
{
    interface IEmuRamIO
    {
        string Name { get; }
        bool Suspend();
        bool Resume();

        bool IsSuspended { get; }

        bool ReadRelative(uint address, byte[] buffer, EndiannessType endianness);
        bool ReadAbsolute(UIntPtr address, byte[] buffer, EndiannessType endianness);
        bool WriteRelative(uint address, byte[] buffer, EndiannessType endianness);
        bool WriteAbsolute(UIntPtr address, byte[] buffer, EndiannessType endianness);

        UIntPtr GetAbsoluteAddress(uint n64Address, int size);
        uint GetRelativeAddress(UIntPtr absoluteAddress, int size);

        event EventHandler OnClose;
    }
}

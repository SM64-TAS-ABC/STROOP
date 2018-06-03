using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Utilities
{
    interface IEmuRamIO : IDisposable
    {
        bool Suspend();
        bool Resume();

        bool IsSuspended { get; }

        bool ReadRelative(uint address, byte[] buffer);
        bool ReadAbsolute(UIntPtr address, byte[] buffer);
        bool WriteRelative(uint address, byte[] buffer);
        bool WriteAbsolute(UIntPtr address, byte[] buffer);

        UIntPtr GetAbsoluteAddress(uint n64Address);
        uint GetRelativeAddress(UIntPtr absoluteAddress);

        event EventHandler OnClose;
    }
}

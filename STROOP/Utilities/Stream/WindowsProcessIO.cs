using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static STROOP.Utilities.Kernal32NativeMethods;

namespace STROOP.Utilities
{
    class WindowsProcessRamIO : BaseProcessIO, IDisposable
    {
        protected IntPtr _processHandle;
        protected Process _process;
        protected bool _isSuspended = false;
        protected UIntPtr _baseOffset;
        protected Emulator _emulator;

        public override bool IsSuspended => _isSuspended;

        protected override EndiannessType Endianness => _emulator.Endianness;
        protected override UIntPtr BaseOffset => _baseOffset;

        public override string Name => _process.ProcessName;
        public override Process Process => _process;

        public override event EventHandler OnClose;

        public WindowsProcessRamIO(Process process, Emulator emulator) : base()
        {
            _process = process;
            _emulator = emulator;

            _process.EnableRaisingEvents = true;

            ProcessAccess accessFlags = ProcessAccess.PROCESS_QUERY_LIMITED_INFORMATION | ProcessAccess.SUSPEND_RESUME 
                | ProcessAccess.VM_OPERATION | ProcessAccess.VM_READ | ProcessAccess.VM_WRITE;
            _processHandle = ProcessGetHandleFromId(accessFlags, false, _process.Id);
            try
            {
                CalculateOffset();
            }
            catch (Exception e)
            {
                CloseProcess(_processHandle);
                throw e;
            }

            _process.Exited += _process_Exited;
        }

        private void _process_Exited(object sender, EventArgs e)
        {
            Dispose();
            OnClose.Invoke(sender, e);
        }

        protected override bool ReadFunc(UIntPtr address, byte[] buffer)
        {
            int numOfBytes = 0;
            return ProcessReadMemory(_processHandle, address, buffer, (IntPtr)buffer.Length, ref numOfBytes);
        }

        protected override bool WriteFunc(UIntPtr address, byte[] buffer)
        {
            int numOfBytes = 0;
            return ProcessWriteMemory(_processHandle, address, buffer, (IntPtr)buffer.Length, ref numOfBytes);
        }

        public override byte[] ReadAllMemory()
        {
            List<byte> output = new List<byte>();
            byte[] buffer = new byte[1];
            int numBytes = 1;

            for (uint address = 0; true; address++)
            {
                bool success = ProcessReadMemory(_processHandle, (UIntPtr)address, buffer, (IntPtr)buffer.Length, ref numBytes);
                if (!success) break;
                output.Add(buffer[0]);
            }

            return output.ToArray();
        }

        protected class MemoryRegion
        {
            public IntPtr StartAddress;
            public IntPtr EndAddress;
            public IntPtr Size;

            public MemoryRegion(IntPtr start, IntPtr size)
            {
                StartAddress = start;
                Size = size;
                EndAddress = (IntPtr)(start.ToInt64() + size.ToInt64());
            }

            public static bool TryMerge(MemoryRegion a, MemoryRegion b, out MemoryRegion merged)
            {
                if ((Int64)a.StartAddress > (Int64)b.StartAddress)
                {
                    MemoryRegion tmp = a;
                    a = b;
                    b = tmp;
                }

                if ((Int64)a.EndAddress < (Int64)b.StartAddress)
                {
                    merged = new MemoryRegion((IntPtr)0, (IntPtr)0);
                    return false;
                }

                IntPtr endAddress = (Int64)a.EndAddress > (Int64)b.EndAddress ? a.EndAddress : b.EndAddress;
                IntPtr size = (IntPtr)((Int64)endAddress - (Int64)(a.StartAddress));
                merged = new MemoryRegion(a.StartAddress, size);
                return true;
            }
        };

        static private bool ProtectIsReadable(uint protect)
        {
            // Includes PAGE_READONLY, PAGE_READWRITE, PAGE_EXECUTE_READ, and PAGE_EXECUTE_READWRITE
            return (protect & 0x02) != 0 || // PAGE_READONLY
                   (protect & 0x04) != 0 || // PAGE_READWRITE
                   (protect & 0x20) != 0 || // PAGE_EXECUTE_READ
                   (protect & 0x40) != 0;   // PAGE_EXECUTE_READWRITE
        }

        protected IEnumerable<UIntPtr> FindBytesInProcess(byte[] searchBytes, MemoryRegion limit)
        {
            byte[] buffer = new byte[1024 * 1024]; // Adjust buffer size as needed

            MemoryBasicInformation info;
            IntPtr infoSize = (IntPtr)Marshal.SizeOf(typeof(MemoryBasicInformation));
            uint setInfoSize = (uint)Marshal.SizeOf(typeof(PsapiWorkingSetExInformation));

            List<MemoryRegion> regions = new List<MemoryRegion>();
            MemoryBasicInformation mbi;
            for (IntPtr p = new IntPtr(); VQueryEx(_processHandle, p, out info, infoSize) == infoSize;
                p = (IntPtr)(p.ToInt64() + info.RegionSize.ToInt64()))
            {
                if (info.State != 0x1000 || !ProtectIsReadable(info.Protect) || (info.Protect & 0x100) != 0)
                    continue;
                //if (info.Type != MemoryType.MEM_MAPPED)
                //    continue;
                if ((Int64)info.RegionSize < Config.RamSize)
                    continue;

                MemoryRegion region = new MemoryRegion((IntPtr)(Int64)info.BaseAddress, info.RegionSize);

                if (limit != null)
                {
                    if ((Int64)region.StartAddress < (Int64)limit.StartAddress)
                        continue;
                    if ((Int64)region.EndAddress > (Int64)limit.EndAddress)
                        continue;
                }

                regions.Add(region);
            }
            bool changed = true;
            while (changed)
            {
                changed = false;
                for (int i = 0; i < regions.Count && !changed; i++)
                {
                    for (int j = i+1; j < regions.Count && !changed; j++)
                    {
                        MemoryRegion a = regions[i];
                        MemoryRegion b = regions[j];
                        if (MemoryRegion.TryMerge(a, b, out MemoryRegion merged))
                        {
                            regions.Remove(a);
                            regions.Remove(b);
                            regions.Add(merged);
                            changed = true;
                        }
                    }
                }
            }

            foreach (MemoryRegion region in regions) { 
                UIntPtr baseAddress = (UIntPtr)(UInt64)region.StartAddress; // Start address for reading
                Int64 bytesRemain = (Int64)region.Size;
                while (true)
                {
                    int bytesRead = 0;
                    IntPtr maxRead = (IntPtr)buffer.Length;
                    if ((Int64)maxRead > bytesRemain)
                        maxRead = (IntPtr)bytesRemain;
                    bool rpm = ProcessReadMemory(_processHandle, baseAddress, buffer, maxRead, ref bytesRead);
                    if (!rpm || bytesRead == 0)
                        break;

                    for (int i = 0; i < bytesRead - searchBytes.Length; i++)
                    {
                        bool found = true;
                        for (int j = 0; j < searchBytes.Length; j++)
                        {
                            if (buffer[i + j] != searchBytes[j])
                            {
                                found = false;
                                break;
                            }
                        }
                        if (found)
                        {
                            yield return baseAddress + i;
                            break;
                        }
                    }
                    baseAddress = (UIntPtr)((UInt64)baseAddress + (UInt64)bytesRead);
                    bytesRemain -= bytesRead;
                }
            }
        }

        static byte[] _arcTableBytes;
        static byte[] ArcTableBytes
        {
            get
            {
                if (_arcTableBytes == null)
                {
                    int arcLength = (TrigTable.gArctanTable.Length / 2) * 2;
                    _arcTableBytes = new byte[arcLength * 2];
                    for (int i = 0; i < arcLength; i++)
                    {
                        short value = TrigTable.gArctanTable[i];
                        int pos = (i / 2) * 4;
                        if (i % 2 == 0)
                        {
                            pos += 2;
                        }
                        _arcTableBytes[pos] = ((byte)value);
                        _arcTableBytes[pos + 1] = ((byte)(value >> 8));
                    }
                }
                return _arcTableBytes;
            }
        }

        static readonly uint[] ArcTableOffsets = { 0x38B000, 0x385CC0 /* EU */, 0x387CF0 /* Shindou */ };

        protected bool StartOfRamAutoDetectValid(UIntPtr startOfRam, bool checkArcTable = true)
        {
            byte[] buffer = new byte[4];
            int bytesRead = 0;
            UIntPtr tellAddress = (UIntPtr)((UInt64)startOfRam + (RomVersionConfig.RomVersionTellAddress & ~0x80000000u));
            ProcessReadMemory(_processHandle, tellAddress, buffer, (IntPtr)buffer.Length, ref bytesRead);
            uint[] tells = { RomVersionConfig.RomVersionTellValueJP, RomVersionConfig.RomVersionTellValueSH, RomVersionConfig.RomVersionTellValueUS, RomVersionConfig.RomVersionTellValueEU };
            uint tell = BitConverter.ToUInt32(buffer, 0);
            if (!tells.Contains(tell))
                return false;

            if (checkArcTable)
            {
                uint offset = 0;
                if (tell == RomVersionConfig.RomVersionTellValueJP)
                    offset = ArcTableOffsets[0];
                else if (tell == RomVersionConfig.RomVersionTellValueUS)
                    offset = ArcTableOffsets[0];
                else if (tell == RomVersionConfig.RomVersionTellValueEU)
                    offset = ArcTableOffsets[1];
                else if (tell == RomVersionConfig.RomVersionTellValueSH)
                    offset = ArcTableOffsets[2];

                UIntPtr arcTableAddress = (UIntPtr)((UInt64)startOfRam + offset);
                buffer = new byte[ArcTableBytes.Length];
                ProcessReadMemory(_processHandle, arcTableAddress, buffer, (IntPtr)buffer.Length, ref bytesRead);
                if (!buffer.SequenceEqual(ArcTableBytes))
                    return false;
            }

            return true;
        }

        protected virtual void CalculateOffset()
        {
            // Find DLL offset if needed
            IntPtr dllOffset = new IntPtr();
            bool useDll = _emulator.Dll != null;
            MemoryRegion dllRegion = null;

            if (_emulator != null && useDll)
            {
                ProcessModule dll = _process.Modules.Cast<ProcessModule>()
                    ?.FirstOrDefault(d => d.ModuleName == _emulator.Dll);

                if (dll == null)
                    throw new ArgumentNullException($"Could not find dll {_emulator.Dll} in process");

                dllOffset = dll.BaseAddress;
                dllRegion = new MemoryRegion(dllOffset, (IntPtr)dll.ModuleMemorySize);
            }

            UIntPtr configRamStart = (UIntPtr)(_emulator.RamStart + (UInt64)dllOffset.ToInt64());

            if (_emulator.AutoDetect)
            {
                // Start by assuming the config offset is good
                _baseOffset = configRamStart;

                // Check if this assumption was good
                if (!StartOfRamAutoDetectValid(configRamStart))
                {
                    // Look through memory to find arc table
                    List<UIntPtr> validAddress = new List<UIntPtr>();
                    foreach (UIntPtr address in FindBytesInProcess(ArcTableBytes, dllRegion))
                    {
                        // Check different Sm64 offsets
                        foreach (uint offset in ArcTableOffsets)
                        {
                            // Check for start of ram signature
                            UIntPtr startOfRam = (UIntPtr)((UInt64)address - offset);
                            if (!StartOfRamAutoDetectValid(startOfRam))
                                continue;

                            _baseOffset = startOfRam;
                            return;
                        }
                    }
                }
            }
            else
            {
                _baseOffset = configRamStart;
            }
        }

        public override bool Suspend()
        {
            SuspendProcess(_process);
            _isSuspended = true;
            return true;
        }

        public override bool Resume()
        {
            // Resume all threads
            ResumeProcess(_process);
            _isSuspended = false;
            return true;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (IsSuspended)
                        Resume();
                    _process.Exited -= _process_Exited;
                }

                // Close old process
                CloseProcess(_processHandle);

                disposedValue = true;
            }
        }
        
        ~WindowsProcessRamIO()
        {
            Dispose(false);
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}

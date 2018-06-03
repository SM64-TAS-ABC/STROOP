using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STROOP.Utilities
{
    public class ProcessStream : IDisposable
    {
        Emulator _emulator;
        IEmuRamIO _io;
        ConcurrentQueue<double> _fpsTimes = new ConcurrentQueue<double>();
        byte[] _ram;
        bool _lastUpdateBeforePausing = false;
        object _enableLocker = new object();
        object _mStreamProcess = new object();

        public event EventHandler OnUpdate;
        public event EventHandler OnDisconnect;
        public event EventHandler FpsUpdated;
        public event EventHandler WarnReadonlyOff;

        public bool Readonly { get; set; } = false;
        public bool ShowWarning { get; set; } = false;
        public bool IsEnabled { get; set; } = true;
        public bool IsRunning { get; private set; } = false;

        public byte[] Ram => _ram;
        public string ProcessName => _emulator == null ? "(No Emulator)" : _emulator.ProcessName;
        public bool IsSuspended => _io?.IsSuspended ?? false;
        public double FpsInPractice => _fpsTimes.Count == 0 ? 0 : 1000 / _fpsTimes.Average();

        public ProcessStream()
        {
            _ram = new byte[Config.RamSize];
            Task.Run(() => ProcessUpdate());
        }

        private void LogException(Exception e)
        {
            try
            {
                var log = String.Format("{0}\n{1}\n{2}\n", e.Message, e.TargetSite.ToString(), e.StackTrace);
                File.AppendAllText("error.txt", log);
            }
            catch (Exception) { }
        }

        private void ExceptionHandler(Task obj)
        {
            LogException(obj.Exception);
            throw obj.Exception;
        }

        public bool SwitchProcess(Process newProcess, Emulator emulator)
        {
            Monitor.Enter(_mStreamProcess);
            IsRunning = false;

            // Dipose of old process
            _io?.Dispose();
            if (_io != null)
                _io.OnClose -= ProcessClosed;

            // Check for no process
            if (newProcess == null)
                goto Error;

            try
            {
                // Open and set new process
                _io = new WindowsProcessRamIO(newProcess, emulator, Config.RamSize);
                _io.OnClose += ProcessClosed;
                _emulator = emulator;
            }
            catch (Exception) // Failed to create process
            {
                goto Error;
            }
                
            IsEnabled = true;
            IsRunning = true;
            Monitor.Exit(_mStreamProcess);

            return true;

            Error:
            _io = null;
            _emulator = null;
            Monitor.Exit(_mStreamProcess);
            return false;
        }

        public void Suspend()
        {
            _lastUpdateBeforePausing = true;
            _io?.Suspend();
        }
        
        public void Resume()
        {
            _io?.Resume();
        }

        private void ProcessClosed(object sender, EventArgs e)
        {
            IsEnabled = false;
            OnDisconnect?.Invoke(this, new EventArgs());
        }

        public UIntPtr GetAbsoluteAddress(uint relativeAddress)
        {
            return _io?.GetAbsoluteAddress(relativeAddress) ?? new UIntPtr(0);
        }

        public uint GetRelativeAddress(UIntPtr relativeAddress)
        {
            return _io?.GetRelativeAddress(relativeAddress) ?? 0;
        }

        public object GetValue(Type type, uint address, bool absoluteAddress = false, uint? mask = null)
        {
            if (type == typeof(byte)) return GetByte(address, absoluteAddress, mask);
            if (type == typeof(sbyte)) return GetSByte(address, absoluteAddress, mask);
            if (type == typeof(short)) return GetInt16(address, absoluteAddress, mask);
            if (type == typeof(ushort)) return GetUInt16(address, absoluteAddress, mask);
            if (type == typeof(int)) return GetInt32(address, absoluteAddress, mask);
            if (type == typeof(uint)) return GetUInt32(address, absoluteAddress, mask);
            if (type == typeof(float)) return GetSingle(address, absoluteAddress, mask);

            throw new ArgumentOutOfRangeException("Cannot call ProcessStream.GetValue with type " + type);
        }

        public byte GetByte(uint address, bool absoluteAddress = false, uint? mask = null)
        {
            byte value = ReadRamLittleEndian(new UIntPtr(address), 1, absoluteAddress)[0];
            if (mask.HasValue) value = (byte)(value & mask.Value);
            return value;
        }

        public sbyte GetSByte(uint address, bool absoluteAddress = false, uint? mask = null)
        {
            sbyte value = (sbyte)ReadRamLittleEndian(new UIntPtr(address), 1, absoluteAddress)[0];
            if (mask.HasValue) value = (sbyte)(value & mask.Value);
            return value;
        }

        public short GetInt16(uint address, bool absoluteAddress = false, uint? mask = null)
        { 
            short value = BitConverter.ToInt16(ReadRamLittleEndian(new UIntPtr(address), 2, absoluteAddress), 0);
            if (mask.HasValue) value = (short)(value & mask.Value);
            return value;
        }

        public ushort GetUInt16(uint address, bool absoluteAddress = false, uint? mask = null)
        {
            ushort value = BitConverter.ToUInt16(ReadRamLittleEndian(new UIntPtr(address), 2, absoluteAddress), 0);
            if (mask.HasValue) value = (ushort)(value & mask.Value);
            return value;
        }

        public int GetInt32(uint address, bool absoluteAddress = false, uint? mask = null)
        {
            int value = BitConverter.ToInt32(ReadRamLittleEndian(new UIntPtr(address), 4, absoluteAddress), 0);
            if (mask.HasValue) value = (int)(value & mask.Value);
            return value;
        }

        public uint GetUInt32(uint address, bool absoluteAddress = false, uint? mask = null)
        {
            uint value = BitConverter.ToUInt32(ReadRamLittleEndian(new UIntPtr(address), 4, absoluteAddress), 0);
            if (mask.HasValue) value = (uint)(value & mask.Value);
            return value;
        }

        public float GetSingle(uint address, bool absoluteAddress = false, uint? mask = null)
        {
            return BitConverter.ToSingle(ReadRamLittleEndian(new UIntPtr(address), 4, absoluteAddress), 0);
        }

        public byte[] ReadRamLittleEndian(UIntPtr address, int length, bool absoluteAddress = false)
        {
            byte[] readBytes = new byte[length];
            uint localAddress;

            if (absoluteAddress)
                localAddress = _io?.GetRelativeAddress(address) ?? 0;
            else
                localAddress = EndianessUtilitiies.SwapAddressEndianess(address.ToUInt32(), length);

            localAddress &= ~0x80000000;

            if (localAddress + length > _ram.Length)
                return new byte[length];

            Buffer.BlockCopy(_ram, (int)localAddress, readBytes, 0, length);
            return readBytes;
        }

        readonly byte[] _fixAddress = { 0x03, 0x02, 0x01, 0x00 };
        public byte[] ReadRam(uint address, int length, bool littleEndian = false)
        {
            byte[] readBytes = new byte[length];
            address &= ~0x80000000U;

            if (address + length > _ram.Length)
                return new byte[length];

            for (uint i = 0; i < length; i++, address++)
            {
                readBytes[i] = _ram[address & ~0x03 | _fixAddress[address & 0x03]];
            }

            return readBytes;
        }

        public bool ReadProcessMemory(UIntPtr address, byte[] buffer)
        {
            return _io?.ReadAbsolute(address, buffer) ?? false;
        }

        public bool CheckReadonlyOff()
        {
            if (ShowWarning)
                WarnReadonlyOff?.Invoke(this, new EventArgs());

            return Readonly;
        }

        public bool SetValueRoundingWrapping (
            Type type, object value, uint address, bool absoluteAddress = false, uint? mask = null)
        {
            // Allow short circuiting if object is already of type
            if (type == typeof(byte) && value is byte byteValue) return SetValue(byteValue, address, absoluteAddress, mask);
            if (type == typeof(sbyte) && value is sbyte sbyteValue) return SetValue(sbyteValue, address, absoluteAddress, mask);
            if (type == typeof(short) && value is short shortValue) return SetValue(shortValue, address, absoluteAddress, mask);
            if (type == typeof(ushort) && value is ushort ushortValue) return SetValue(ushortValue, address, absoluteAddress, mask);
            if (type == typeof(int) && value is int intValue) return SetValue(intValue, address, absoluteAddress, mask);
            if (type == typeof(uint) && value is uint uintValue) return SetValue(uintValue, address, absoluteAddress, mask);
            if (type == typeof(float) && value is float floatValue) return SetValue(floatValue, address, absoluteAddress, mask);

            value = ParsingUtilities.ParseDoubleNullable(value);
            if (value == null) return false;

            if (type == typeof(byte)) value = ParsingUtilities.ParseByteRoundingWrapping(value);
            if (type == typeof(sbyte)) value = ParsingUtilities.ParseSByteRoundingWrapping(value);
            if (type == typeof(short)) value = ParsingUtilities.ParseShortRoundingWrapping(value);
            if (type == typeof(ushort)) value = ParsingUtilities.ParseUShortRoundingWrapping(value);
            if (type == typeof(int)) value = ParsingUtilities.ParseIntRoundingWrapping(value);
            if (type == typeof(uint)) value = ParsingUtilities.ParseUIntRoundingWrapping(value);

            return SetValue(type, value.ToString(), address, absoluteAddress, mask);
        }

        public bool SetValue(Type type, object value, uint address, bool absoluteAddress = false, uint? mask = null)
        {
            if (value is string)
            {
                if (type == typeof(byte)) value = ParsingUtilities.ParseByteNullable(value);
                if (type == typeof(sbyte)) value = ParsingUtilities.ParseSByteNullable(value);
                if (type == typeof(short)) value = ParsingUtilities.ParseShortNullable(value);
                if (type == typeof(ushort)) value = ParsingUtilities.ParseUShortNullable(value);
                if (type == typeof(int)) value = ParsingUtilities.ParseIntNullable(value);
                if (type == typeof(uint)) value = ParsingUtilities.ParseUIntNullable(value);
                if (type == typeof(float)) value = ParsingUtilities.ParseFloatNullable(value);
            }

            if (value == null) return false;

            if (type == typeof(byte)) return SetValue((byte)value, address, absoluteAddress, mask);
            if (type == typeof(sbyte)) return SetValue((sbyte)value, address, absoluteAddress, mask);
            if (type == typeof(short)) return SetValue((short)value, address, absoluteAddress, mask);
            if (type == typeof(ushort)) return SetValue((ushort)value, address, absoluteAddress, mask);
            if (type == typeof(int)) return SetValue((int)value, address, absoluteAddress, mask);
            if (type == typeof(uint)) return SetValue((uint)value, address, absoluteAddress, mask);
            if (type == typeof(float)) return SetValue((float)value, address, absoluteAddress, mask);

            throw new ArgumentOutOfRangeException("Cannot call ProcessStream.SetValue with type " + type);
        }

        public bool SetValue(byte value, uint address, bool absoluteAddress = false, uint? mask = null)
        {
            if (mask.HasValue)
            {
                byte oldValue = GetByte(address, absoluteAddress);
                value = (byte)((oldValue & ~mask.Value) | (value & mask.Value));
            }
            bool returnValue = WriteRamLittleEndian(new byte[] { value }, address, absoluteAddress);
            if (returnValue) WatchVariableLockManager.UpdateMemoryLockValue(value, address, typeof(byte), mask);
            return returnValue;
        }

        public bool SetValue(sbyte value, uint address, bool absoluteAddress = false, uint? mask = null)
        {
            if (mask.HasValue)
            {
                sbyte oldValue = GetSByte(address, absoluteAddress);
                value = (sbyte)((oldValue & ~mask.Value) | (value & mask.Value));
            }
            bool returnValue = WriteRamLittleEndian(new byte[] { (byte)value }, address, absoluteAddress);
            if (returnValue) WatchVariableLockManager.UpdateMemoryLockValue(value, address, typeof(sbyte), mask);
            return returnValue;
        }

        public bool SetValue(Int16 value, uint address, bool absoluteAddress = false, uint? mask = null)
        {
            if (mask.HasValue)
            {
                short oldValue = GetInt16(address, absoluteAddress);
                value = (short)((oldValue & ~mask.Value) | (value & mask.Value));
            }
            bool returnValue = WriteRamLittleEndian(BitConverter.GetBytes(value), address, absoluteAddress);
            if (returnValue) WatchVariableLockManager.UpdateMemoryLockValue(value, address, typeof(short), mask);
            return returnValue;
        }

        public bool SetValue(UInt16 value, uint address, bool absoluteAddress = false, uint? mask = null)
        {
            if (mask.HasValue)
            {
                ushort oldValue = GetUInt16(address, absoluteAddress);
                value = (ushort)((oldValue & ~mask.Value) | (value & mask.Value));
            }
            bool returnValue = WriteRamLittleEndian(BitConverter.GetBytes(value), address, absoluteAddress);
            if (returnValue) WatchVariableLockManager.UpdateMemoryLockValue(value, address, typeof(ushort), mask);
            return returnValue;
        }

        public bool SetValue(Int32 value, uint address, bool absoluteAddress = false, uint? mask = null)
        {
            if (mask.HasValue)
            {
                int oldValue = GetInt32(address, absoluteAddress);
                value = (int)((oldValue & ~mask.Value) | (value & mask.Value));
            }
            bool returnValue = WriteRamLittleEndian(BitConverter.GetBytes(value), address, absoluteAddress);
            if (returnValue) WatchVariableLockManager.UpdateMemoryLockValue(value, address, typeof(int), mask);
            return returnValue;
        }

        public bool SetValue(UInt32 value, uint address, bool absoluteAddress = false, uint? mask = null)
        {
            if (mask.HasValue)
            {
                uint oldValue = GetUInt32(address, absoluteAddress);
                value = (uint)((oldValue & ~mask.Value) | (value & mask.Value));
            }
            bool returnValue = WriteRamLittleEndian(BitConverter.GetBytes(value), address, absoluteAddress);
            if (returnValue) WatchVariableLockManager.UpdateMemoryLockValue(value, address, typeof(uint), mask);
            return returnValue;
        }

        public bool SetValue(float value, uint address, bool absoluteAddress = false, uint? mask = null)
        {
            bool returnValue = WriteRamLittleEndian(BitConverter.GetBytes(value), address, absoluteAddress);
            if (returnValue) WatchVariableLockManager.UpdateMemoryLockValue(value, address, typeof(float), mask);
            return returnValue;
        }

        public bool WriteRamLittleEndian(byte[] buffer, uint address, bool absoluteAddress = false, 
            int bufferStart = 0, int? length = null, bool safeWrite = true)
        {
            if (length == null)
                length = buffer.Length - bufferStart;

            if (CheckReadonlyOff())
                return false;

            byte[] writeBytes = new byte[length.Value];
            Array.Copy(buffer, bufferStart, writeBytes, 0, length.Value);

            // Attempt to pause the game before writing 
            bool preSuspended = _io?.IsSuspended ?? false;
            if (safeWrite)
                _io?.Suspend();

            // Write memory to game/process
            bool result;
            if (absoluteAddress)
                result = _io?.WriteAbsolute((UIntPtr)address, writeBytes) ?? false;
            else
                result = _io?.WriteRelative(address, writeBytes) ?? false;

            // Resume stream 
            if (safeWrite && !preSuspended)
                _io?.Resume();

            return result;
        }

        public bool WriteRam(byte[] buffer, uint address, int bufferStart = 0, int? length = null, bool safeWrite = true)
        {
            if (length == null)
                length = buffer.Length - bufferStart;

            if (CheckReadonlyOff())
                return false;

            bool success = true;

            // Attempt to pause the game before writing 
            bool preSuspended = _io?.IsSuspended ?? false;
            if (safeWrite)
                _io?.Suspend();

            // Take care of first alignment
            int bufPos = bufferStart;
            uint alignment = _fixAddress[address & 0x03] + 1U;
            if (alignment < 4)
            {
                byte[] writeBytes = new byte[Math.Min(alignment, length.Value)];
                Array.Copy(buffer, bufPos, writeBytes, 0, writeBytes.Length);
                success &= _io?.WriteRelative(address, writeBytes.Reverse().ToArray()) ?? false;
                length -= writeBytes.Length;
                bufPos += writeBytes.Length;
                address += alignment;
            }

            // Take care of middle
            if (length >= 4)
            {
                byte[] writeBytes = new byte[length.Value & ~0x03];
                for (int i = 0; i < writeBytes.Length; bufPos += 4, i += 4)
                {
                    writeBytes[i] = buffer[bufPos + 3];
                    writeBytes[i + 1] = buffer[bufPos + 2];
                    writeBytes[i + 2] = buffer[bufPos + 1];
                    writeBytes[i + 3] = buffer[bufPos];
                }
                success &= _io?.WriteRelative(address, writeBytes) ?? false;
                address += (uint)writeBytes.Length;
                length -= writeBytes.Length;
            }

            // Take care of last
            if (length > 0)
            {
                byte[] writeBytes = new byte[length.Value];
                Array.Copy(buffer, bufPos, writeBytes, 0, writeBytes.Length);
                success &= _io?.WriteRelative(address, writeBytes.Reverse().ToArray()) ?? false;
            }

            // Resume stream 
            if (safeWrite && !preSuspended)
                _io?.Resume();

            return success;
        }

        public bool RefreshRam()
        {
            try
            {
                // Read whole ram value to buffer
                if (_ram.Length != Config.RamSize)
                    _ram = new byte[Config.RamSize];

                return _io?.ReadRelative(0, _ram) ?? false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void ProcessUpdate()
        {
            Stopwatch frameStopwatch = Stopwatch.StartNew();

            for (; ; )
            {
                try {
                    Monitor.Enter(_mStreamProcess);

                    frameStopwatch.Restart();
                    if ((!IsEnabled || !IsRunning) && !_lastUpdateBeforePausing)
                        goto FrameLimitStreamUpdate;

                    _lastUpdateBeforePausing = false;

                    if (!RefreshRam())
                        goto FrameLimitStreamUpdate;

                    OnUpdate?.Invoke(this, new EventArgs());

                    FrameLimitStreamUpdate:

                    // Calculate delay to match correct FPS
                    frameStopwatch.Stop();
                    int timeToWait = (int)RefreshRateConfig.RefreshRateInterval - (int)frameStopwatch.ElapsedMilliseconds;
                    timeToWait = Math.Max(timeToWait, 0);

                    // Calculate Fps
                    if (_fpsTimes.Count() >= 10)
                    {
                        double garbage;
                        _fpsTimes.TryDequeue(out garbage);
                    }
                    _fpsTimes.Enqueue(frameStopwatch.ElapsedMilliseconds + timeToWait);
                    FpsUpdated?.Invoke(this, new EventArgs());

                    Monitor.Exit(_mStreamProcess);

                    if (timeToWait > 0)
                        Thread.Sleep(timeToWait);
                    else
                        Thread.Yield();
                }
                catch (Exception e)
                {
                    Debugger.Break();
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_io != null)
                    {
                        _io.OnClose -= ProcessClosed;
                        _io.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}

using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOPUnitTests.Mocks
{
    class MockEmuIO : BaseProcessIO
    {
        public override bool IsSuspended => false;

        public override string Name => nameof(MockEmuIO);

        protected override UIntPtr BaseOffset => new UIntPtr(Offset);

        protected override EndiannessType Endianness => _endianness;

        public override event EventHandler OnClose;

        public const uint Offset = 0x1000000;

        private EndiannessType _endianness;
        private byte[] _buffer;


        public MockEmuIO(EndiannessType endianness)
            : base(Config.RamSize)
        {
            _endianness = endianness;
            Clear();
        }

        public void SetEndianness(EndiannessType endianness)
        {
            _endianness = endianness;
        }

        public void Clear()
        {
            _buffer = new byte[Config.RamSize];
        }

        public byte[] GetBytes(uint address, int count)
        {
            byte[] outBytes = new byte[count];
            ReadFunc(new UIntPtr(address + Offset), outBytes);
            return outBytes;
        }

        public override bool Resume() { return true; }

        public override bool Suspend() { return true; }

        protected override bool ReadFunc(UIntPtr address, byte[] buffer)
        {
            Array.Copy(_buffer, address.ToUInt32() - Offset, buffer, 0, buffer.Length);
            return true;
        }

        protected override bool WriteFunc(UIntPtr address, byte[] buffer)
        {
            Array.Copy(buffer, 0, _buffer, address.ToUInt32() - Offset, buffer.Length);
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;
using ICSharpCode.SharpZipLib.GZip;
using System.IO;
using STROOP.Structs.Configurations;

namespace STROOP.Utilities
{
    class StFileIO : BaseProcessIO
    {

        public override bool IsSuspended => false;

        public override event EventHandler OnClose;

        private string _path;
        public string Path => _path;

        protected override UIntPtr BaseOffset => new UIntPtr(0x1B0);
        protected override EndiannessType Endianness => EndiannessType.Little;

        public override string Name => System.IO.Path.GetFileName(_path);

        private byte[] _data;

        public StFileIO(string path, uint ramSize) : base(ramSize)
        {
            _path = path;

            LoadMemory();
        }

        public void LoadMemory()
        {
            using (var fileStream = new FileStream(_path, FileMode.Open))
            {
                using (var gzipStream = new GZipInputStream(fileStream))
                {
                    using (MemoryStream unzip = new MemoryStream())
                    {
                        gzipStream.CopyTo(unzip);
                        _data = unzip.GetBuffer();
                    }
                }
            }
        }

        public void SaveMemory(string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                using (var gzipStream = new GZipOutputStream(fileStream))
                {
                    gzipStream.Write(_data, 0, _data.Length);
                }
            }
        }

        public override bool Resume() { return true; }

        public override bool Suspend() { return true; }

        protected override bool WriteFunc(UIntPtr address, byte[] buffer)
        {
            if ((uint)address + buffer.Length > _data.Length)
                return false;

            Array.Copy(buffer, 0, _data, (uint)address, buffer.Length);
            return true;
        }

        protected override bool ReadFunc(UIntPtr address, byte[] buffer)
        {
            if ((uint)address + buffer.Length > _data.Length)
                return false;

            Array.Copy(_data, (uint)address, buffer, 0, buffer.Length);
            return true;
        }
    }
}

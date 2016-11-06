using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Utilities
{
    public class WatchVariableLock
    {
        uint _address;
        byte[] _data;
        ProcessStream _stream;

        public WatchVariableLock(ProcessStream stream, uint address, byte[] data)
        {
            _stream = stream;
            _data = data;
            _address = address;
        }

        public bool Update()
        {
            return _stream.WriteRam(_data, _address);
        }

        public override int GetHashCode()
        {
            return new Tuple<uint, int>(_address, _data.Length).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is WatchVariableLock))
                return false;

            var other = obj as WatchVariableLock;
            return (other == this);
        }

        public static bool operator ==(WatchVariableLock a, WatchVariableLock b)
        {
            if (object.ReferenceEquals(a, null))
            {
                return object.ReferenceEquals(b, null);
            }
            else if (object.ReferenceEquals(b, null))
            {
                return false;
            }

            return (a._address == b._address && a._data.Length == b._data.Length);
        }

        public static bool operator !=(WatchVariableLock a, WatchVariableLock b)
        {
            return !(a == b);
        }
    }
}

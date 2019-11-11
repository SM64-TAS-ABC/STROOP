using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using STROOP.Utilities;
using STROOP.Structs;
using STROOP.Extensions;
using System.Reflection;
using STROOP.Managers;
using STROOP.Structs.Configurations;

namespace STROOP.Controls
{
    public class WatchVariableLock
    {
        public readonly bool IsSpecial;
        public readonly Type MemoryType;
        public readonly int? ByteCount;
        public readonly uint? Mask;
        public readonly int? Shift;
        public readonly uint Address;
        public readonly string SpecialType;
        public readonly Func<object, uint, bool> SetterFunction;

        private object _value;
        public object Value { get { return _value; } }

        public WatchVariableLock(
            bool isSpecial,
            Type memoryType,
            int? byteCount,
            uint? mask,
            int? shift,
            uint address,
            string specialType,
            Func<object, uint, bool> setterFunction,
            object value)
        {
            IsSpecial = isSpecial;
            MemoryType = memoryType;
            ByteCount = byteCount;
            Mask = mask;
            Shift = shift;
            Address = address;
            SpecialType = specialType;
            SetterFunction = setterFunction;

            _value = value;
        }

        public void Invoke()
        {
            SetterFunction(_value, Address);
        }

        public void UpdateLockValue(object value)
        {
            _value = value;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is WatchVariableLock)) return false;
            WatchVariableLock other = (WatchVariableLock)obj;

            bool sameAddress = this.Address == other.Address &&
                   this.ByteCount == other.ByteCount &&
                   this.MemoryType == other.MemoryType;

            WatchVariableLock lock1 = this.Address < other.Address ? this : other;
            WatchVariableLock lock2 = this.Address < other.Address ? other : this;
            bool closeAddress = lock1.Address + 2 == lock2.Address &&
                (lock1.MemoryType == typeof(uint) || lock1.MemoryType == typeof(int)) &&
                (lock2.MemoryType == typeof(ushort) || lock2.MemoryType == typeof(short));

            return (sameAddress || closeAddress) &&
                   this.IsSpecial == other.IsSpecial &&
                   this.Mask == other.Mask &&
                   this.Shift == other.Shift &&
                   this.SpecialType == other.SpecialType;
        }

        public bool EqualsMemorySignature(uint address, Type type, uint? mask, int? shift)
        {
            bool sameAddress = this.Address == address && this.MemoryType == type;

            (uint address1, Type type1) = Address < address ? (Address, MemoryType) : (address, type);
            (uint address2, Type type2) = Address < address ? (address, type) : (Address, MemoryType);
            bool closeAddress = address1 + 2 == address2 &&
                (type1 == typeof(uint) || type1 == typeof(int)) &&
                (type2 == typeof(ushort) || type2 == typeof(short));

            return (sameAddress || closeAddress) &&
                IsSpecial == false &&
                Mask == mask &&
                Shift == shift;
        }

        public override int GetHashCode()
        {
            return IsSpecial ?
                SpecialType.GetHashCode() :
                unchecked((int)Address);
        }

    }
}

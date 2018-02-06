using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Extensions;
using System.Reflection;
using SM64_Diagnostic.Managers;
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic.Controls
{
    public class WatchVariableLock
    {
        public readonly bool IsSpecial;
        public readonly Type MemoryType;
        public readonly int? ByteCount;
        public readonly uint? Mask;
        public readonly uint Address;
        public readonly string SpecialType;
        public readonly Func<string, uint, bool> SetterFunction;

        private string _value;
        public string Value { get { return _value; } }

        public WatchVariableLock(
            bool isSpecial,
            Type memoryType,
            int? byteCount,
            uint? mask,
            uint effectiveAddress,
            string specialType,
            Func<string, uint, bool> setterFunction,
            string value)
        {
            IsSpecial = isSpecial;
            MemoryType = memoryType;
            ByteCount = byteCount;
            Mask = mask;
            Address = effectiveAddress;
            SpecialType = specialType;
            SetterFunction = setterFunction;

            _value = value;
        }

        public void Invoke()
        {
            SetterFunction(_value, Address);
        }

        public void UpdateLockValue(string value)
        {
            _value = value;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is WatchVariableLock)) return false;
            WatchVariableLock other = (WatchVariableLock)obj;
            return this.IsSpecial == other.IsSpecial &&
                   this.MemoryType == other.MemoryType &&
                   this.ByteCount == other.ByteCount &&
                   this.Mask == other.Mask &&
                   this.Address == other.Address &&
                   this.SpecialType == other.SpecialType;
        }

        public bool EqualsMemorySignature(uint address, Type type, uint? mask)
        {
            return IsSpecial == false &&
                Address == address &&
                MemoryType == type &&
                Mask == mask;
        }

        public override int GetHashCode()
        {
            return IsSpecial ?
                SpecialType.GetHashCode() :
                unchecked((int)Address);
        }

    }
}

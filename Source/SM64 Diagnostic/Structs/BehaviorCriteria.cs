using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public struct BehaviorCriteria
    {
        public uint BehaviorAddress;
        public uint? GfxId;
        public uint? SubType;
        public uint? Appearance;

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is BehaviorCriteria))
                return false;

            var otherCriteria = (BehaviorCriteria)obj;

            return otherCriteria == this;
        }

        public bool BehaviorOnly()
        {
            return (!GfxId.HasValue && !SubType.HasValue && !Appearance.HasValue);
        }

        public bool CongruentTo(BehaviorCriteria otherCriteria)
        {
            if (otherCriteria.BehaviorAddress != BehaviorAddress)
                return false;

            if (GfxId.HasValue && otherCriteria.GfxId.HasValue && GfxId.Value != otherCriteria.GfxId.Value)
                return false;

            if (SubType.HasValue && otherCriteria.SubType.HasValue && SubType.Value != otherCriteria.SubType.Value)
                return false;

            if (Appearance.HasValue && otherCriteria.Appearance.HasValue && Appearance.Value != otherCriteria.Appearance.Value)
                return false;

            return true;
        }

        public static bool operator ==(BehaviorCriteria a, BehaviorCriteria b)
        {
            return (a.BehaviorAddress == b.BehaviorAddress && a.GfxId == b.GfxId
                && a.SubType == b.SubType && a.Appearance == b.Appearance);
        }

        public static bool operator !=(BehaviorCriteria a, BehaviorCriteria b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return new Tuple<uint, uint?, uint?, uint?>(BehaviorAddress, GfxId, SubType, Appearance).GetHashCode();
        }
    }
}

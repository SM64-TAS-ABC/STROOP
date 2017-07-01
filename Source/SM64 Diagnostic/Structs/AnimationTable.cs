using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public class AnimationTable
    {
        public struct AnimationReference
        {
            public uint AnimationValue;
            public string AnimationName;

            public override int GetHashCode()
            {
                return (int)AnimationValue;
            }
        }

        Dictionary<uint, AnimationReference> _table = new Dictionary<uint, AnimationReference>();

        public AnimationTable()
        {
        }

        public void Add(AnimationReference animationRef)
        {
            _table.Add(animationRef.AnimationValue, animationRef);
        }

        public string GetAnimationName(uint animation)
        {
            if (!_table.ContainsKey(animation))
                return "Unknown Animation";

            return _table[animation].AnimationName;
        }
    }
}

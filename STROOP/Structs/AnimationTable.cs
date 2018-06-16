using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public class AnimationTable
    {
        public struct AnimationReference
        {
            public int AnimationValue;
            public string AnimationName;

            public override int GetHashCode()
            {
                return AnimationValue;
            }
        }

        Dictionary<int, AnimationReference> _animationTable = new Dictionary<int, AnimationReference>();
        Dictionary<string, AnimationReference> _animationNameTable = new Dictionary<string, AnimationReference>();

        public AnimationTable()
        {
        }

        public void Add(AnimationReference animationRef)
        {
            _animationTable.Add(animationRef.AnimationValue, animationRef);
            _animationNameTable.Add(animationRef.AnimationName, animationRef);
        }

        public List<string> GetAnimationNameList()
        {
            List<string> animationNameList = _animationTable.Keys.ToList().ConvertAll(
                animation => _animationTable[animation].AnimationName);
            animationNameList.Sort();
            return animationNameList;
        }

        public int? GetAnimationFromName(string animationName)
        {
            if (!_animationNameTable.ContainsKey(animationName))
                return null;
            return _animationNameTable[animationName].AnimationValue;
        }

        public string GetAnimationName()
        {
            uint marioObjRef = Config.Stream.GetUInt32(MarioObjectConfig.PointerAddress);
            short animation = Config.Stream.GetInt16(marioObjRef + MarioObjectConfig.AnimationOffset);
            return GetAnimationName(animation);
        }

        public string GetAnimationName(int animation)
        {
            if (!_animationTable.ContainsKey(animation))
                return "Unknown Animation";
            return _animationTable[animation].AnimationName;
        }
    }
}

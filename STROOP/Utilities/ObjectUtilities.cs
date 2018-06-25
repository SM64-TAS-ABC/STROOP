using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Utilities
{
    public static class ObjectUtilities
    {
        public static uint? GetObjectRelativeAddress(uint absoluteAddress)
        {
            uint objRangeMinAddress = ObjectSlotsConfig.LinkStartAddress;
            uint objRangeMaxAddress =
                objRangeMinAddress + (uint)ObjectSlotsConfig.MaxSlots * ObjectConfig.StructSize;

            if (absoluteAddress < objRangeMinAddress ||
                absoluteAddress >= objRangeMaxAddress) return null;

            uint relativeAddress = (absoluteAddress - objRangeMinAddress) % ObjectConfig.StructSize;
            return relativeAddress;
        }

        public static uint? GetCollisionObject(uint objAddress, int collisionIndex)
        {
            if (collisionIndex < 1 || collisionIndex > 4)
                throw new ArgumentOutOfRangeException();

            ushort numCollidedObjects = Config.Stream.GetUInt16(objAddress + ObjectConfig.NumCollidedObjectsOffset);
            if (collisionIndex > numCollidedObjects)
                return null;

            uint collisionObjectOffset = ((uint)collisionIndex - 1) * 4;
            return Config.Stream.GetUInt32(objAddress + ObjectConfig.CollidedObjectsListStartOffset + collisionObjectOffset);
        }

        public static uint? GetMarioCollisionObject(int collisionIndex)
        {
            uint marioObjRef = Config.Stream.GetUInt32(MarioObjectConfig.PointerAddress);
            return GetCollisionObject(marioObjRef, collisionIndex);
        }
    }
}

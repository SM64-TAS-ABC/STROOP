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
        public static bool IsObjectAddress(uint address)
        {
            return GetObjectRelativeAddress(address) == 0;
        }

        public static uint? GetObjectRelativeAddress(uint absoluteAddress)
        {
            uint objRangeMinAddress = ObjectSlotsConfig.ObjectSlotsStartAddress;
            uint objRangeMaxAddress =
                objRangeMinAddress + (uint)ObjectSlotsConfig.MaxSlots * ObjectConfig.StructSize;

            if (absoluteAddress < objRangeMinAddress ||
                absoluteAddress >= objRangeMaxAddress) return null;

            uint relativeAddress = (absoluteAddress - objRangeMinAddress) % ObjectConfig.StructSize;
            return relativeAddress;
        }

        public static int? GetObjectIndex(uint absoluteAddress)
        {
            if (!IsObjectAddress(absoluteAddress)) return null;
            int index = (int)((absoluteAddress - ObjectSlotsConfig.ObjectSlotsStartAddress) / ObjectConfig.StructSize);
            return index;
        }

        public static uint GetObjectAddress(int index)
        {
            if (index < 0 || index >= ObjectSlotsConfig.MaxSlots)
                throw new ArgumentOutOfRangeException();

            return ObjectSlotsConfig.ObjectSlotsStartAddress + (uint)index * ObjectConfig.StructSize;
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

        public static byte? GetProcessGroup(uint address)
        {
            foreach (byte processGroup in ObjectSlotsConfig.ProcessingGroups)
            {
                uint processGroupStructAddress = ObjectSlotsConfig.ProcessGroupsStartAddress + processGroup * ObjectSlotsConfig.ProcessGroupStructSize;
                if (address == processGroupStructAddress) return processGroup;
            }
            return null;
        }

        public static Color GetProcessingGroupColorForObjAddress(uint address)
        {
            ObjectDataModel obj = Config.ObjectSlotsManager.GetObjectFromAddress(address);
            byte? processGroup = obj?.CurrentProcessGroup;
            Color color = ObjectSlotsConfig.GetProcessingGroupColor(processGroup);
            return color;
        }
    }
}

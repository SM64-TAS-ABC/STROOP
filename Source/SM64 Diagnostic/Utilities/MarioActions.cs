using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Extensions;
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic.Utilities
{
    public static class MarioActions
    {
        static uint _prevMarioGraphic = 0x00;

        public static bool GoToObjects(ProcessStream stream, List<uint> objAddresses)
        {
            // Move mario to object
            var marioAddress = Config.Mario.StructAddress;

            stream.Suspend();

            // Get object position
            float x, y, z;
            x = objAddresses.Average(obj => stream.GetSingle(obj + Config.ObjectSlots.ObjectXOffset));
            y = objAddresses.Average(obj => stream.GetSingle(obj + Config.ObjectSlots.ObjectYOffset));
            z = objAddresses.Average(obj => stream.GetSingle(obj + Config.ObjectSlots.ObjectZOffset));

            // Add offset
            y += Config.Mario.MoveToObjectYOffset;

            // Move mario to object
            bool success = true;
            success &= stream.SetValue(x, marioAddress + Config.Mario.XOffset);
            success &= stream.SetValue(y, marioAddress + Config.Mario.YOffset);
            success &= stream.SetValue(z, marioAddress + Config.Mario.ZOffset);

            stream.Resume();

            return success;
        }

        public static bool RetreiveObjects(ProcessStream stream, List<uint> objAddresses)
        {
            // Move object to Mario
            var marioAddress = Config.Mario.StructAddress;

            stream.Suspend();

            // Get Mario position
            float x, y, z;
            x = stream.GetSingle(marioAddress + Config.Mario.XOffset);
            y = stream.GetSingle(marioAddress + Config.Mario.YOffset);
            z = stream.GetSingle(marioAddress + Config.Mario.ZOffset);

            // Add offset
            y += Config.ObjectSlots.MoveToMarioYOffset;

            // Move object to Mario
            bool success = true;
            foreach (var objAddress in objAddresses)
            {
                success &= stream.SetValue(x, objAddress + Config.ObjectSlots.ObjectXOffset);
                success &= stream.SetValue(y, objAddress + Config.ObjectSlots.ObjectYOffset);
                success &= stream.SetValue(z, objAddress + Config.ObjectSlots.ObjectZOffset);
            }
            stream.Resume();

            return success;
        }

        public static bool MoveObjects(ProcessStream stream, List<uint> objAddresses,
            float xOffset, float yOffset, float zOffset)
        {     
            stream.Suspend();

            // Move object to Mario
            bool success = true;
            foreach (var objAddress in objAddresses)
            {
                float x, y, z;
                x = stream.GetSingle(objAddress + Config.ObjectSlots.ObjectXOffset);
                y = stream.GetSingle(objAddress + Config.ObjectSlots.ObjectYOffset);
                z = stream.GetSingle(objAddress + Config.ObjectSlots.ObjectZOffset);

                x += xOffset;
                y += yOffset;
                z += zOffset;

                success &= stream.SetValue(x, objAddress + Config.ObjectSlots.ObjectXOffset);
                success &= stream.SetValue(y, objAddress + Config.ObjectSlots.ObjectYOffset);
                success &= stream.SetValue(z, objAddress + Config.ObjectSlots.ObjectZOffset);
            }
            stream.Resume();

            return success;
        }

        public static bool GoToObjectsHome(ProcessStream stream, List<uint> objAddresses)
        {
            // Move mario to object
            var marioAddress = Config.Mario.StructAddress;

            stream.Suspend();

            // Get object position
            float x, y, z;
            x = objAddresses.Average(obj => stream.GetSingle(obj + Config.ObjectSlots.HomeXOffset));
            y = objAddresses.Average(obj => stream.GetSingle(obj + Config.ObjectSlots.HomeYOffset));
            z = objAddresses.Average(obj => stream.GetSingle(obj + Config.ObjectSlots.HomeZOffset));

            // Add offset
            y += Config.Mario.MoveToObjectYOffset;

            // Move mario to object
            bool success = true;
            success &= stream.SetValue(x, marioAddress + Config.Mario.XOffset);
            success &= stream.SetValue(y, marioAddress + Config.Mario.YOffset);
            success &= stream.SetValue(z, marioAddress + Config.Mario.ZOffset);

            stream.Resume();

            return success;
        }

        public static bool RetreiveObjectsHome(ProcessStream stream, List<uint> objAddresses)
        {
            // Move object to Mario
            var marioAddress = Config.Mario.StructAddress;

            stream.Suspend();

            // Get Mario position
            float x, y, z;
            x = stream.GetSingle(marioAddress + Config.Mario.XOffset);
            y = stream.GetSingle(marioAddress + Config.Mario.YOffset);
            z = stream.GetSingle(marioAddress + Config.Mario.ZOffset);

            // Add offset
            y += Config.ObjectSlots.MoveToMarioYOffset;

            // Move object to Mario
            bool success = true;
            foreach (var objAddress in objAddresses)
            {
                success &= stream.SetValue(x, objAddress + Config.ObjectSlots.HomeXOffset);
                success &= stream.SetValue(y, objAddress + Config.ObjectSlots.HomeYOffset);
                success &= stream.SetValue(z, objAddress + Config.ObjectSlots.HomeZOffset);
            }
            stream.Resume();

            return success;
        }

        public static bool CloneObject(ProcessStream stream, uint objAddress)
        {
            bool success = true;
            var marioAddress = Config.Mario.StructAddress;
            stream.Suspend();

            uint lastObject = stream.GetUInt32(marioAddress + Config.Mario.HoldingObjectPointerOffset);
            
            // Set clone action flags
            if (lastObject == 0x00000000U)
            {
                // Set Next action
                uint currentAction = stream.GetUInt32(marioAddress + Config.Mario.ActionOffset);
                uint nextAction = Config.MarioActions.GetAfterCloneValue(currentAction);
                success &= stream.SetValue(nextAction, marioAddress + Config.Mario.ActionOffset);
            }

            // Set new holding value
            success &= stream.SetValue(objAddress, marioAddress + Config.Mario.HoldingObjectPointerOffset);

            stream.Resume();
            return success;
        }

        public static bool UnCloneObject(ProcessStream stream, uint objAddress)
        {
            bool success = true;
            var marioAddress = Config.Mario.StructAddress;

            stream.Suspend();

            // Set mario's next action
            uint currentAction = stream.GetUInt32(marioAddress + Config.Mario.ActionOffset);
            uint nextAction = Config.MarioActions.GetAfterUncloneValue(currentAction);
            success &= stream.SetValue(nextAction, marioAddress + Config.Mario.ActionOffset);

            // Clear mario's holding object
            success &= stream.SetValue(0x00000000U, marioAddress + Config.Mario.HoldingObjectPointerOffset);

            stream.Resume();
            return success;
        }

        public static bool UnloadObject(ProcessStream stream, List<uint> addresses)
        {
            bool success = true;
            foreach (var address in addresses)
            {
                var test = stream.GetUInt16(address + Config.ObjectSlots.ObjectActiveOffset);
                success &= stream.SetValue((short) 0x0000, address + Config.ObjectSlots.ObjectActiveOffset);
            }
            return success;
        }

        public static bool ReviveObject(ProcessStream stream, List<uint> addresses)
        {
            bool success = true;
            stream.Suspend();

            foreach (var address in addresses)
            {
                // Find process group
                uint scriptAddress = stream.GetUInt32(address + Config.ObjectSlots.BehaviorScriptOffset);
                if (scriptAddress == 0x00000000)
                    continue;
                uint firstScriptAction = stream.GetUInt32(scriptAddress);
                if ((firstScriptAction & 0xFF000000U) != 0x00000000U)
                    continue;
                byte processGroup = (byte)((firstScriptAction & 0x00FF0000U) >> 16);

                // Read first object in group
                var groupConfig = Config.ObjectGroups;
                uint groupAddress = groupConfig.FirstGroupingAddress + processGroup * groupConfig.ProcessGroupStructSize;

                // Loop through and find last object in group
                uint lastGroupObj = groupAddress;
                while (stream.GetUInt32(lastGroupObj + groupConfig.ProcessNextLinkOffset) != groupAddress)
                    lastGroupObj = stream.GetUInt32(lastGroupObj + groupConfig.ProcessNextLinkOffset);

                // Remove object from current group
                uint nextObj = stream.GetUInt32(address + groupConfig.ProcessNextLinkOffset);
                uint prevObj = stream.GetUInt32(groupConfig.VactantPointerAddress);
                if (prevObj == address)
                {
                    // Set new vacant pointer
                    success &= stream.SetValue(nextObj, groupConfig.VactantPointerAddress);
                }
                else
                {
                    for (int i = 0; i < Config.ObjectSlots.MaxSlots; i++)
                    {
                        uint obj = stream.GetUInt32(prevObj + groupConfig.ProcessNextLinkOffset);
                        if (obj == address)
                            break;
                        prevObj = obj;
                    }
                    success &= stream.SetValue(nextObj, prevObj + groupConfig.ProcessNextLinkOffset);
                }


                // Insert object in new group
                nextObj = stream.GetUInt32(lastGroupObj + groupConfig.ProcessNextLinkOffset);
                success &= stream.SetValue(address, nextObj + groupConfig.ProcessPreviousLinkOffset);
                success &= stream.SetValue(address, lastGroupObj + groupConfig.ProcessNextLinkOffset);
                success &= stream.SetValue(lastGroupObj, address + groupConfig.ProcessPreviousLinkOffset);
                success &= stream.SetValue(nextObj, address + groupConfig.ProcessNextLinkOffset);

                success &= stream.SetValue((short)0x0101, address + Config.ObjectSlots.ObjectActiveOffset);

                if (addresses.Count > 1)
                    if (!stream.RefreshRam() || !success)
                        break;
            }

            stream.Resume();
            return success;
        }

        public static bool ToggleHandsfree(ProcessStream stream)
        {
            bool success = true;
            var marioAddress = Config.Mario.StructAddress;

            stream.Suspend();

            var holdingObj = stream.GetUInt32(marioAddress + Config.Mario.HoldingObjectPointerOffset);

            if (holdingObj != 0x00000000U)
            {
                uint currentAction = stream.GetUInt32(marioAddress + Config.Mario.ActionOffset);
                uint nextAction = Config.MarioActions.GetHandsfreeValue(currentAction);
                success = stream.SetValue(nextAction, marioAddress + Config.Mario.ActionOffset);
            }

            stream.Resume();

            return success;
        }

        public static bool ToggleVisibility(ProcessStream stream)
        {
            bool success = true;
            stream.Suspend();

            var marioObjRef = stream.GetUInt32(Config.Mario.ObjectReferenceAddress);
            if (marioObjRef != 0x00000000U)
            {
                var marioGraphics = stream.GetUInt32(marioObjRef + Config.ObjectSlots.BehaviorGfxOffset);
                if (marioGraphics == 0)
                { 
                    success &= stream.SetValue(_prevMarioGraphic, marioObjRef + Config.ObjectSlots.BehaviorGfxOffset);
                }
                else
                {
                    _prevMarioGraphic = marioGraphics;
                    success &= stream.SetValue(0x00000000U, marioObjRef + Config.ObjectSlots.BehaviorGfxOffset);
                }
            }

            stream.Resume();

            return success;
        }

        public static bool RefillHp(ProcessStream stream)
        {
            return stream.SetValue(Config.Hud.FullHp, Config.Hud.HpAddress);
        }

        public static bool Die(ProcessStream stream)
        {
            return stream.SetValue((short)255, Config.Hud.HpAddress);
        }

        public static bool StandardHud(ProcessStream stream)
        {
            bool success = true;

            stream.Suspend();

            success &= stream.SetValue(Config.Hud.FullHp, Config.Hud.HpAddress);
            success &= stream.SetValue(Config.Hud.StandardCoins, Config.Hud.CoinCountAddress);
            success &= stream.SetValue(Config.Hud.StandardLives, Config.Hud.LiveCountAddress);
            success &= stream.SetValue(Config.Hud.StandardStars, Config.Hud.StarCountAddress);
            success &= stream.SetValue(Config.Hud.FullHpInt, Config.Hud.DisplayHpAddress);
            success &= stream.SetValue(Config.Hud.StandardCoins, Config.Hud.DisplayCoinCountAddress);
            success &= stream.SetValue((short)Config.Hud.StandardLives, Config.Hud.DisplayLiveCountAddress);
            success &= stream.SetValue(Config.Hud.StandardStars, Config.Hud.DisplayStarCountAddress);

            stream.Resume();

            return success;
        }

        public static bool GoToTriangle(ProcessStream stream, uint triangleAddress, int vertex, bool _useMisalignmentOffset = false)
        {
            if (triangleAddress == 0x0000)
                return false;

            float newX, newY, newZ;
            switch(vertex)
            {
                case 1:
                    newX = stream.GetInt16(triangleAddress + Config.TriangleOffsets.X1);
                    newY = stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y1);
                    newZ = stream.GetInt16(triangleAddress + Config.TriangleOffsets.Z1);
                    break;

                case 2:
                    newX = stream.GetInt16(triangleAddress + Config.TriangleOffsets.X2);
                    newY = stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y2);
                    newZ = stream.GetInt16(triangleAddress + Config.TriangleOffsets.Z2);
                    break;

                case 3:
                    newX = stream.GetInt16(triangleAddress + Config.TriangleOffsets.X3);
                    newY = stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y3);
                    newZ = stream.GetInt16(triangleAddress + Config.TriangleOffsets.Z3);
                    break;

                default:
                    throw new Exception("There are only 3 vertices in a triangle. You are an idiot :).");
            }

            if (_useMisalignmentOffset)
            {
                newX += (newX >= 0) ? 0.5f : -0.5f;
                newZ += (newZ >= 0) ? 0.5f : -0.5f;
            }

            // Move mario to triangle (while in same Pu)
            return PuUtilities.MoveToInCurrentPu(stream, newX, newY, newZ);
        }

        public static bool RetrieveTriangle(ProcessStream stream, uint triangleAddress)
        {
            if (triangleAddress == 0x0000)
                return false;

            float normX, normY, normZ, oldNormOffset;
            normX = stream.GetSingle(triangleAddress + Config.TriangleOffsets.NormX);
            normY = stream.GetSingle(triangleAddress + Config.TriangleOffsets.NormY);
            normZ = stream.GetSingle(triangleAddress + Config.TriangleOffsets.NormZ);
            oldNormOffset = stream.GetSingle(triangleAddress + Config.TriangleOffsets.Offset);

            // Get Mario position
            short marioX, marioY, marioZ;
            var marioAddress = Config.Mario.StructAddress;
            marioX = (short) stream.GetSingle(marioAddress + Config.Mario.XOffset);
            marioY = (short) stream.GetSingle(marioAddress + Config.Mario.YOffset);
            marioZ = (short) stream.GetSingle(marioAddress + Config.Mario.ZOffset);

            float normOffset = -(normX * marioX + normY * marioY + normZ * marioZ);
            float normDiff = normOffset - oldNormOffset;

            short yOffset = (short)(-normDiff * normY);

            short v1Y, v2Y, v3Y;
            v1Y = (short)(stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y1) + yOffset);
            v2Y = (short)(stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y2) + yOffset);
            v3Y = (short)(stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y3) + yOffset);

            short yMin = (short)(Math.Min(Math.Min(v1Y, v2Y), v3Y) + 5);
            short yMax = (short)(Math.Max(Math.Max(v1Y, v2Y), v3Y) - 5);

            stream.Suspend();

            // Update triangle
            bool success = true;
            
            success &= stream.SetValue(v1Y, triangleAddress + Config.TriangleOffsets.Y1);
            success &= stream.SetValue(v2Y, triangleAddress + Config.TriangleOffsets.Y2);
            success &= stream.SetValue(v3Y, triangleAddress + Config.TriangleOffsets.Y3);
            success &= stream.SetValue(yMin, triangleAddress + Config.TriangleOffsets.YMin);
            success &= stream.SetValue(yMax, triangleAddress + Config.TriangleOffsets.YMax);
            success &= stream.SetValue(normOffset, triangleAddress + Config.TriangleOffsets.Offset);

            stream.Resume();

            return success;
        }
    }
}

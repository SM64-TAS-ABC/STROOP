using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Extensions;

namespace SM64_Diagnostic.Utilities
{
    public static class MarioActions
    {
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

            // Make clone object mario's holding object
            success &= stream.SetValue(objAddress, marioAddress + Config.Mario.HoldingObjectPointerOffset);

            // Set clone action flags
            uint currentAction = stream.GetUInt32(marioAddress + Config.Mario.ActionOffset);
            uint nextAction = Config.MarioActions.GetAfterCloneValue(currentAction);
            success &= stream.SetValue(nextAction, marioAddress + Config.Mario.ActionOffset);

            stream.Resume();

            return success;
        }

        public static bool UnCloneObject(ProcessStream stream, uint objAddress)
        {
            bool success = true;
            var marioAddress = Config.Mario.StructAddress;

            stream.Suspend();

            // Make clone object mario's holding object
            success &= stream.SetValue((UInt32) 0x00000000U, marioAddress + Config.Mario.HoldingObjectPointerOffset);

            // Set clone action flags
            uint currentAction = stream.GetUInt32(marioAddress + Config.Mario.ActionOffset);
            uint nextAction = Config.MarioActions.GetAfterUncloneValue(currentAction);
            success &= stream.SetValue(nextAction, marioAddress + Config.Mario.ActionOffset);

            stream.Resume();

            return success;
        }

        public static bool UnloadObject(ProcessStream stream, List<uint> addresses)
        {
            bool success = true;
            foreach (var address in addresses)
            {
                success &= stream.SetValue((short) 0x0000, address + Config.ObjectSlots.ObjectActiveOffset);
            }
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

            stream.Suspend();

            // Move mario to triangle
            bool success = true;
            var marioAddress = Config.Mario.StructAddress;
            success &= stream.SetValue(newX, marioAddress + Config.Mario.XOffset);
            success &= stream.SetValue(newY, marioAddress + Config.Mario.YOffset);
            success &= stream.SetValue(newZ, marioAddress + Config.Mario.ZOffset);

            stream.Resume();

            return success;
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

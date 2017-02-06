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
            x = objAddresses.Average(obj => BitConverter.ToSingle(stream.ReadRam(obj + Config.ObjectSlots.ObjectXOffset, 4), 0));
            y = objAddresses.Average(obj => BitConverter.ToSingle(stream.ReadRam(obj + Config.ObjectSlots.ObjectYOffset, 4), 0));
            z = objAddresses.Average(obj => BitConverter.ToSingle(stream.ReadRam(obj + Config.ObjectSlots.ObjectZOffset, 4), 0));

            // Add offset
            y += Config.Mario.MoveToObjectYOffset;

            // Move mario to object
            bool success = true;
            success &= stream.WriteRam(BitConverter.GetBytes(x), marioAddress + Config.Mario.XOffset);
            success &= stream.WriteRam(BitConverter.GetBytes(y), marioAddress + Config.Mario.YOffset);
            success &= stream.WriteRam(BitConverter.GetBytes(z), marioAddress + Config.Mario.ZOffset);

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
            x = BitConverter.ToSingle(stream.ReadRam(marioAddress + Config.Mario.XOffset, 4), 0);
            y = BitConverter.ToSingle(stream.ReadRam(marioAddress + Config.Mario.YOffset, 4), 0);
            z = BitConverter.ToSingle(stream.ReadRam(marioAddress + Config.Mario.ZOffset, 4), 0);

            // Add offset
            y += Config.ObjectSlots.MoveToMarioYOffset;

            // Move object to Mario
            bool success = true;
            foreach (var objAddress in objAddresses)
            {
                success &= stream.WriteRam(BitConverter.GetBytes(x), objAddress + Config.ObjectSlots.ObjectXOffset);
                success &= stream.WriteRam(BitConverter.GetBytes(y), objAddress + Config.ObjectSlots.ObjectYOffset);
                success &= stream.WriteRam(BitConverter.GetBytes(z), objAddress + Config.ObjectSlots.ObjectZOffset);
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
            x = objAddresses.Average(obj => BitConverter.ToSingle(stream.ReadRam(obj + Config.ObjectSlots.HomeXOffset, 4), 0));
            y = objAddresses.Average(obj => BitConverter.ToSingle(stream.ReadRam(obj + Config.ObjectSlots.HomeYOffset, 4), 0));
            z = objAddresses.Average(obj => BitConverter.ToSingle(stream.ReadRam(obj + Config.ObjectSlots.HomeZOffset, 4), 0));

            // Add offset
            y += Config.Mario.MoveToObjectYOffset;

            // Move mario to object
            bool success = true;
            success &= stream.WriteRam(BitConverter.GetBytes(x), marioAddress + Config.Mario.XOffset);
            success &= stream.WriteRam(BitConverter.GetBytes(y), marioAddress + Config.Mario.YOffset);
            success &= stream.WriteRam(BitConverter.GetBytes(z), marioAddress + Config.Mario.ZOffset);

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
            x = BitConverter.ToSingle(stream.ReadRam(marioAddress + Config.Mario.XOffset, 4), 0);
            y = BitConverter.ToSingle(stream.ReadRam(marioAddress + Config.Mario.YOffset, 4), 0);
            z = BitConverter.ToSingle(stream.ReadRam(marioAddress + Config.Mario.ZOffset, 4), 0);

            // Add offset
            y += Config.ObjectSlots.MoveToMarioYOffset;

            // Move object to Mario
            bool success = true;
            foreach (var objAddress in objAddresses)
            {
                success &= stream.WriteRam(BitConverter.GetBytes(x), objAddress + Config.ObjectSlots.HomeXOffset);
                success &= stream.WriteRam(BitConverter.GetBytes(y), objAddress + Config.ObjectSlots.HomeYOffset);
                success &= stream.WriteRam(BitConverter.GetBytes(z), objAddress + Config.ObjectSlots.HomeZOffset);
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
            success &= stream.WriteRam(BitConverter.GetBytes(objAddress), marioAddress + Config.Mario.HoldingObjectPointerOffset);

            // Set clone action flags
            uint currentAction = stream.GetUInt32(marioAddress + Config.Mario.ActionOffset);
            uint nextAction = Config.MarioActions.GetAfterCloneValue(currentAction);
            success &= stream.WriteRam(BitConverter.GetBytes(nextAction), marioAddress + Config.Mario.ActionOffset);

            stream.Resume();

            return success;
        }

        public static bool UnCloneObject(ProcessStream stream, uint objAddress)
        {
            bool success = true;
            var marioAddress = Config.Mario.StructAddress;

            stream.Suspend();

            // Make clone object mario's holding object
            success &= stream.WriteRam(new byte[] { 0x00, 0x00, 0x00, 0x00 }, marioAddress + Config.Mario.HoldingObjectPointerOffset);

            // Set clone action flags
            uint currentAction = stream.GetUInt32(marioAddress + Config.Mario.ActionOffset);
            uint nextAction = Config.MarioActions.GetAfterUncloneValue(currentAction);
            success &= stream.WriteRam(BitConverter.GetBytes(nextAction), marioAddress + Config.Mario.ActionOffset);

            stream.Resume();

            return success;
        }

        public static bool UnloadObject(ProcessStream stream, List<uint> addresses)
        {
            bool success = true;
            foreach (var address in addresses)
            {
                success &= stream.WriteRam(new byte[] { 0x00, 0x00 }, address + Config.ObjectSlots.ObjectActiveOffset);
            }
            return success;
        }

        public static bool RefillHp(ProcessStream stream)
        {
            return stream.WriteRam(BitConverter.GetBytes(Config.Hud.FullHp), Config.Hud.HpAddress);
        }

        public static bool Die(ProcessStream stream)
        {
            return stream.WriteRam(BitConverter.GetBytes((short)255), Config.Hud.HpAddress);
        }

        public static bool StandardHud(ProcessStream stream)
        {
            bool success = true;

            success &= stream.WriteRam(BitConverter.GetBytes(Config.Hud.FullHp), Config.Hud.HpAddress);
            success &= stream.WriteRam(BitConverter.GetBytes(Config.Hud.StandardCoins), Config.Hud.CoinCountAddress);
            success &= stream.WriteRam(BitConverter.GetBytes(Config.Hud.StandardLives), Config.Hud.LiveCountAddress);
            success &= stream.WriteRam(BitConverter.GetBytes(Config.Hud.StandardStars), Config.Hud.StarCountAddress);
            success &= stream.WriteRam(BitConverter.GetBytes(Config.Hud.StandardCoins), Config.Hud.DisplayCoinCountAddress);
            success &= stream.WriteRam(BitConverter.GetBytes((short)Config.Hud.StandardLives), Config.Hud.DisplayLiveCountAddress);
            success &= stream.WriteRam(BitConverter.GetBytes(Config.Hud.StandardStars), Config.Hud.DisplayStarCountAddress);

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
                    newX = BitConverter.ToInt16(stream.ReadRam(triangleAddress + Config.TriangleOffsets.X1, 2), 0);
                    newY = BitConverter.ToInt16(stream.ReadRam(triangleAddress + Config.TriangleOffsets.Y1, 2), 0);
                    newZ = BitConverter.ToInt16(stream.ReadRam(triangleAddress + Config.TriangleOffsets.Z1, 2), 0);
                    break;

                case 2:
                    newX = BitConverter.ToInt16(stream.ReadRam(triangleAddress + Config.TriangleOffsets.X2, 2), 0);
                    newY = BitConverter.ToInt16(stream.ReadRam(triangleAddress + Config.TriangleOffsets.Y2, 2), 0);
                    newZ = BitConverter.ToInt16(stream.ReadRam(triangleAddress + Config.TriangleOffsets.Z2, 2), 0);
                    break;

                case 3:
                    newX = BitConverter.ToInt16(stream.ReadRam(triangleAddress + Config.TriangleOffsets.X3, 2), 0);
                    newY = BitConverter.ToInt16(stream.ReadRam(triangleAddress + Config.TriangleOffsets.Y3, 2), 0);
                    newZ = BitConverter.ToInt16(stream.ReadRam(triangleAddress + Config.TriangleOffsets.Z3, 2), 0);
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
            success &= stream.WriteRam(BitConverter.GetBytes(newX), marioAddress + Config.Mario.XOffset);
            success &= stream.WriteRam(BitConverter.GetBytes(newY), marioAddress + Config.Mario.YOffset);
            success &= stream.WriteRam(BitConverter.GetBytes(newZ), marioAddress + Config.Mario.ZOffset);

            stream.Resume();

            return success;
        }

        public static bool RetrieveTriangle(ProcessStream stream, uint triangleAddress)
        {
            if (triangleAddress == 0x0000)
                return false;

            float normX, normY, normZ, oldNormOffset;
            normX = BitConverter.ToSingle(stream.ReadRam(triangleAddress + Config.TriangleOffsets.NormX, 4), 0);
            normY = BitConverter.ToSingle(stream.ReadRam(triangleAddress + Config.TriangleOffsets.NormY, 4), 0);
            normZ = BitConverter.ToSingle(stream.ReadRam(triangleAddress + Config.TriangleOffsets.NormZ, 4), 0);
            oldNormOffset = BitConverter.ToSingle(stream.ReadRam(triangleAddress + Config.TriangleOffsets.Offset, 4), 0);

            // Get Mario position
            short marioX, marioY, marioZ;
            var marioAddress = Config.Mario.StructAddress;
            marioX = (short) BitConverter.ToSingle(stream.ReadRam(marioAddress + Config.Mario.XOffset, 4), 0);
            marioY = (short) BitConverter.ToSingle(stream.ReadRam(marioAddress + Config.Mario.YOffset, 4), 0);
            marioZ = (short) BitConverter.ToSingle(stream.ReadRam(marioAddress + Config.Mario.ZOffset, 4), 0);

            float normOffset = -(normX * marioX + normY * marioY + normZ * marioZ);
            float normDiff = normOffset - oldNormOffset;

            short yOffset = (short)(-normDiff * normY);

            short v1Y, v2Y, v3Y;
            v1Y = (short)(BitConverter.ToInt16(stream.ReadRam(triangleAddress + Config.TriangleOffsets.Y1, 2), 0) + yOffset);
            v2Y = (short)(BitConverter.ToInt16(stream.ReadRam(triangleAddress + Config.TriangleOffsets.Y2, 2), 0) + yOffset);
            v3Y = (short)(BitConverter.ToInt16(stream.ReadRam(triangleAddress + Config.TriangleOffsets.Y3, 2), 0) + yOffset);

            short yMin = (short)(Math.Min(Math.Min(v1Y, v2Y), v3Y) + 5);
            short yMax = (short)(Math.Max(Math.Max(v1Y, v2Y), v3Y) - 5);

            stream.Suspend();

            // Update triangle
            bool success = true;
            
            success &= stream.WriteRam(BitConverter.GetBytes(v1Y), triangleAddress + Config.TriangleOffsets.Y1);
            success &= stream.WriteRam(BitConverter.GetBytes(v2Y), triangleAddress + Config.TriangleOffsets.Y2);
            success &= stream.WriteRam(BitConverter.GetBytes(v3Y), triangleAddress + Config.TriangleOffsets.Y3);
            success &= stream.WriteRam(BitConverter.GetBytes(yMin), triangleAddress + Config.TriangleOffsets.YMin);
            success &= stream.WriteRam(BitConverter.GetBytes(yMax), triangleAddress + Config.TriangleOffsets.YMax);
            success &= stream.WriteRam(BitConverter.GetBytes(normOffset), triangleAddress + Config.TriangleOffsets.Offset);

            stream.Resume();

            return success;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;

namespace SM64_Diagnostic.Utilities
{
    public static class MarioActions
    {
        public static bool MoveMarioToObject(ProcessStream stream, uint objAddress)
        {
            // Move mario to object
            var marioAddress = Config.Mario.StructAddress;

            stream.Suspend();

            // Get object position
            float x, y, z;
            x = BitConverter.ToSingle(stream.ReadRam(objAddress + Config.ObjectSlots.ObjectXOffset, 4), 0);
            y = BitConverter.ToSingle(stream.ReadRam(objAddress + Config.ObjectSlots.ObjectYOffset, 4), 0);
            z = BitConverter.ToSingle(stream.ReadRam(objAddress + Config.ObjectSlots.ObjectZOffset, 4), 0);

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

        public static bool MoveObjectToMario(ProcessStream stream, uint objAddress)
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
            success &= stream.WriteRam(BitConverter.GetBytes(x), objAddress + Config.ObjectSlots.ObjectXOffset);
            success &= stream.WriteRam(BitConverter.GetBytes(y), objAddress + Config.ObjectSlots.ObjectYOffset);
            success &= stream.WriteRam(BitConverter.GetBytes(z), objAddress + Config.ObjectSlots.ObjectZOffset);

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
            success &= stream.WriteRam(BitConverter.GetBytes(0x8000207U), marioAddress + Config.Mario.ActionOffset);

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
            success &= stream.WriteRam(BitConverter.GetBytes(0x0C400201U), marioAddress + Config.Mario.ActionOffset);

            stream.Resume();

            return success;
        }

        public static bool UnloadObject(ProcessStream stream, uint address)
        {
            return stream.WriteRam(new byte[] { 0x00, 0x00 }, address + Config.ObjectSlots.ObjectActiveOffset);
        }

        public static bool RefillHp(ProcessStream stream)
        {
            return stream.WriteRam(BitConverter.GetBytes(Config.Hud.FullHp), Config.Hud.HpAddress);
        }

        public static bool Die(ProcessStream stream)
        {
            return stream.WriteRam(BitConverter.GetBytes((short)0), Config.Hud.HpAddress);
        }

        public static bool StandardHud(ProcessStream stream)
        {
            bool success = true;

            success &= stream.WriteRam(BitConverter.GetBytes(Config.Hud.FullHp), Config.Hud.HpAddress);
            success &= stream.WriteRam(BitConverter.GetBytes(Config.Hud.StandardCoins), Config.Hud.CoinCountAddress);
            success &= stream.WriteRam(BitConverter.GetBytes(Config.Hud.StandardLives), Config.Hud.LiveCountAddress);
            success &= stream.WriteRam(BitConverter.GetBytes(Config.Hud.StandardStars), Config.Hud.StarCountAddress);

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

            short yMin = Math.Min(Math.Min(v1Y, v2Y), v3Y);
            short yMax = Math.Max(Math.Max(v1Y, v2Y), v3Y);

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

        public static int GetClosestVertex(ProcessStream stream, uint triangleAddress)
        {
            if (triangleAddress == 0x0000)
                return 0;

            // Get Mario position
            short marioX, marioY, marioZ;
            var marioAddress = Config.Mario.StructAddress;
            marioX = (short)BitConverter.ToSingle(stream.ReadRam(marioAddress + Config.Mario.XOffset, 4), 0);
            marioY = (short)BitConverter.ToSingle(stream.ReadRam(marioAddress + Config.Mario.YOffset, 4), 0);
            marioZ = (short)BitConverter.ToSingle(stream.ReadRam(marioAddress + Config.Mario.ZOffset, 4), 0);

            short v1X, v1Y, v1Z;
            short v2X, v2Y, v2Z;
            short v3X, v3Y, v3Z;
            v1X = BitConverter.ToInt16(stream.ReadRam(triangleAddress + Config.TriangleOffsets.X1, 2), 0);
            v1Y = BitConverter.ToInt16(stream.ReadRam(triangleAddress + Config.TriangleOffsets.Y1, 2), 0);
            v1Z = BitConverter.ToInt16(stream.ReadRam(triangleAddress + Config.TriangleOffsets.Z1, 2), 0);
            v2X = BitConverter.ToInt16(stream.ReadRam(triangleAddress + Config.TriangleOffsets.X2, 2), 0);
            v2Y = BitConverter.ToInt16(stream.ReadRam(triangleAddress + Config.TriangleOffsets.Y2, 2), 0);
            v2Z = BitConverter.ToInt16(stream.ReadRam(triangleAddress + Config.TriangleOffsets.Z2, 2), 0);
            v3X = BitConverter.ToInt16(stream.ReadRam(triangleAddress + Config.TriangleOffsets.X3, 2), 0);
            v3Y = BitConverter.ToInt16(stream.ReadRam(triangleAddress + Config.TriangleOffsets.Y3, 2), 0);
            v3Z = BitConverter.ToInt16(stream.ReadRam(triangleAddress + Config.TriangleOffsets.Z3, 2), 0);

            double disToV1, disToV2, disToV3;
            disToV1 = Math.Pow(marioX - v1X, 2) + Math.Pow(marioY - v1Y, 2) + Math.Pow(marioZ - v1Z, 2);
            disToV2 = Math.Pow(marioX - v2X, 2) + Math.Pow(marioY - v2Y, 2) + Math.Pow(marioZ - v2Z, 2);
            disToV3 = Math.Pow(marioX - v3X, 2) + Math.Pow(marioY - v3Y, 2) + Math.Pow(marioZ - v3Z, 2);

            double minDis = Math.Min(Math.Min(disToV1, disToV2), disToV3);
            if (minDis == disToV1)
                return 1;
            if (minDis == disToV2)
                return 2;
            if (minDis == disToV3)
                return 3;

            return 0;
        }
    }
}

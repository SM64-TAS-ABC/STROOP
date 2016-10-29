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
            var marioAddress = Config.Mario.MarioStructAddress;

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
            var marioAddress = Config.Mario.MarioStructAddress;

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
            var marioAddress = Config.Mario.MarioStructAddress;

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
            var marioAddress = Config.Mario.MarioStructAddress;

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
    }
}

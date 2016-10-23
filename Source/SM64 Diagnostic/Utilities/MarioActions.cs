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
        public static bool MoveMarioToObject(ProcessStream stream, Config config, uint objAddress)
        {
            // Move mario to object
            var marioAddress = config.Mario.MarioStructAddress;

            stream.Suspend();

            // Get object position
            float x, y, z;
            x = BitConverter.ToSingle(stream.ReadRam(objAddress + config.ObjectSlots.ObjectXOffset, 4), 0);
            y = BitConverter.ToSingle(stream.ReadRam(objAddress + config.ObjectSlots.ObjectYOffset, 4), 0);
            z = BitConverter.ToSingle(stream.ReadRam(objAddress + config.ObjectSlots.ObjectZOffset, 4), 0);

            // Add offset
            y += config.Mario.MoveToObjectYOffset;

            // Move mario to object
            bool success = true;
            success &= stream.WriteRam(BitConverter.GetBytes(x), marioAddress + config.Mario.XOffset);
            success &= stream.WriteRam(BitConverter.GetBytes(y), marioAddress + config.Mario.YOffset);
            success &= stream.WriteRam(BitConverter.GetBytes(z), marioAddress + config.Mario.ZOffset);

            stream.Resume();

            return success;
        }

        public static bool MoveObjectToMario(ProcessStream stream, Config config, uint objAddress)
        {
            // Move object to Mario
            var marioAddress = config.Mario.MarioStructAddress;

            stream.Suspend();

            // Get Mario position
            float x, y, z;
            x = BitConverter.ToSingle(stream.ReadRam(marioAddress + config.Mario.XOffset, 4), 0);
            y = BitConverter.ToSingle(stream.ReadRam(marioAddress + config.Mario.YOffset, 4), 0);
            z = BitConverter.ToSingle(stream.ReadRam(marioAddress + config.Mario.ZOffset, 4), 0);

            // Add offset
            y += config.ObjectSlots.MoveToMarioYOffset;

            // Move object to Mario
            bool success = true;
            success &= stream.WriteRam(BitConverter.GetBytes(x), objAddress + config.ObjectSlots.ObjectXOffset);
            success &= stream.WriteRam(BitConverter.GetBytes(y), objAddress + config.ObjectSlots.ObjectYOffset);
            success &= stream.WriteRam(BitConverter.GetBytes(z), objAddress + config.ObjectSlots.ObjectZOffset);

            stream.Resume();

            return success;
        }


        public static bool CloneObject(ProcessStream stream, Config config, uint objAddress)
        {
            bool success = true;
            var marioAddress = config.Mario.MarioStructAddress;

            stream.Suspend();

            // Make clone object mario's holding object
            success &= stream.WriteRam(BitConverter.GetBytes(objAddress), marioAddress + config.Mario.HoldingObjectPointerOffset);

            // Set clone action flags
            success &= stream.WriteRam(BitConverter.GetBytes(0x8000207U), marioAddress + config.Mario.ActionOffset);

            stream.Resume();

            return success;
        }

        public static bool UnCloneObject(ProcessStream stream, Config config, uint objAddress)
        {
            bool success = true;
            var marioAddress = config.Mario.MarioStructAddress;

            stream.Suspend();

            // Make clone object mario's holding object
            success &= stream.WriteRam(new byte[] { 0x00, 0x00, 0x00, 0x00 }, marioAddress + config.Mario.HoldingObjectPointerOffset);

            // Set clone action flags
            success &= stream.WriteRam(BitConverter.GetBytes(0x0C400201U), marioAddress + config.Mario.ActionOffset);

            stream.Resume();

            return success;
        }

        public static bool UnloadObject(ProcessStream stream, Config config, uint address)
        {
            return stream.WriteRam(new byte[] { 0x00, 0x00 }, address + config.ObjectSlots.ObjectActiveOffset);
        }

        public static bool RefillHp(ProcessStream stream, Config config)
        {
            return stream.WriteRam(BitConverter.GetBytes(config.Hud.FullHp), config.Hud.HpAddress);
        }

        public static bool Die(ProcessStream stream, Config config)
        {
            return stream.WriteRam(BitConverter.GetBytes((short)0), config.Hud.HpAddress);
        }

        public static bool StandardHud(ProcessStream stream, Config config)
        {
            bool success = true;

            success &= stream.WriteRam(BitConverter.GetBytes(config.Hud.FullHp), config.Hud.HpAddress);
            success &= stream.WriteRam(BitConverter.GetBytes(config.Hud.StandardCoins), config.Hud.CoinCountAddress);
            success &= stream.WriteRam(BitConverter.GetBytes(config.Hud.StandardLives), config.Hud.LiveCountAddress);
            success &= stream.WriteRam(BitConverter.GetBytes(config.Hud.StandardStars), config.Hud.StarCountAddress);

            return success;
        }
    }
}

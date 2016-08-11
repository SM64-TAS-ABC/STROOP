﻿using SM64_Diagnostic.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Utilities
{
    public static class PuUtilities
    {
        const int PuOffset = 32768;
        const int PuSize = 65536;

        public static float GetRelativePuPosition(float coord, int puCoord)
        {
            // We find the relative object positon by subtracting the PU starting coordinates from the object
            return coord - puCoord * PuSize;
        }

        public static float GetRelativePuPosition(float coord)
        {
            // We find the relative object positon by subtracting the PU starting coordinates from the object
            return coord - GetPUFromCoord(coord) * PuSize;
        }

        public static int GetPUFromCoord(float coord)
        {
            int pu = (int)((coord + PuOffset) / PuSize);

            // If the object is located in the center of the (-1,-1) pu its coordinates will be (-0.5, -0.5). 
            // Because we used division this rounds down to (0,0), which is incorrect, we therefore add -1 to all negative PUs
            if (coord < -PuOffset)
                pu--;

            return pu;
        }

        public static bool MoveToRelativePu(ProcessStream stream, Config config, int newPuX, int newPuY, int newPuZ)
        {
            // Move object to Mario
            var marioAddress = config.Mario.MarioStructAddress;

            // Get Mario position
            float marioX, marioY, marioZ;
            marioX = BitConverter.ToSingle(stream.ReadRam(marioAddress + config.Mario.XOffset, 4), 0);
            marioY = BitConverter.ToSingle(stream.ReadRam(marioAddress + config.Mario.YOffset, 4), 0);
            marioZ = BitConverter.ToSingle(stream.ReadRam(marioAddress + config.Mario.ZOffset, 4), 0);

            // Calculate new coordinates
            float newMarioX, newMarioY, newMarioZ;
            newMarioX = marioX + newPuX * PuSize;
            newMarioY = marioY + newPuY * PuSize;
            newMarioZ = marioZ + newPuZ * PuSize;

            // Set new mario position
            bool success = true;
            success &= stream.WriteRam(BitConverter.GetBytes(newMarioX), marioAddress + config.Mario.XOffset);
            success &= stream.WriteRam(BitConverter.GetBytes(newMarioY), marioAddress + config.Mario.YOffset);
            success &= stream.WriteRam(BitConverter.GetBytes(newMarioZ), marioAddress + config.Mario.ZOffset);
            return success;
        }

        public static bool MoveToPu(ProcessStream stream, Config config, int newPuX, int newPuY, int newPuZ)
        {
            // Move object to Mario
            var marioAddress = config.Mario.MarioStructAddress;

            // Get Mario position
            float marioX, marioY, marioZ;
            marioX = BitConverter.ToSingle(stream.ReadRam(marioAddress + config.Mario.XOffset, 4), 0);
            marioY = BitConverter.ToSingle(stream.ReadRam(marioAddress + config.Mario.YOffset, 4), 0);
            marioZ = BitConverter.ToSingle(stream.ReadRam(marioAddress + config.Mario.ZOffset, 4), 0);

            // Calculate new coordinates
            float newMarioX, newMarioY, newMarioZ;
            newMarioX = PuUtilities.GetRelativePuPosition(marioX) + newPuX * PuSize;
            newMarioY = PuUtilities.GetRelativePuPosition(marioY) + newPuY * PuSize;
            newMarioZ = PuUtilities.GetRelativePuPosition(marioZ) + newPuZ * PuSize;

            // Set new mario position
            bool success = true;
            success &= stream.WriteRam(BitConverter.GetBytes(newMarioX), marioAddress + config.Mario.XOffset);
            success &= stream.WriteRam(BitConverter.GetBytes(newMarioY), marioAddress + config.Mario.YOffset);
            success &= stream.WriteRam(BitConverter.GetBytes(newMarioZ), marioAddress + config.Mario.ZOffset);
            return success;
        }
    }
}

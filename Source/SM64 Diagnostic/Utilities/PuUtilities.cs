using SM64_Diagnostic.Structs;
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

        public static bool MoveToRelativePu(ProcessStream stream, int newPuX, int newPuY, int newPuZ)
        {
            var marioAddress = Config.Mario.MarioStructAddress;

            // Get Mario position
            float marioX, marioY, marioZ;
            marioX = BitConverter.ToSingle(stream.ReadRam(marioAddress + Config.Mario.XOffset, 4), 0);
            marioY = BitConverter.ToSingle(stream.ReadRam(marioAddress + Config.Mario.YOffset, 4), 0);
            marioZ = BitConverter.ToSingle(stream.ReadRam(marioAddress + Config.Mario.ZOffset, 4), 0);

            // Get Camera Position
            float cameraX, cameraY, cameraZ;
            cameraX = BitConverter.ToSingle(stream.ReadRam(Config.CameraX, 4), 0);
            cameraY = BitConverter.ToSingle(stream.ReadRam(Config.CameraY, 4), 0);
            cameraZ = BitConverter.ToSingle(stream.ReadRam(Config.CameraZ, 4), 0);

            // Calculate new coordinates
            float newMarioX, newMarioY, newMarioZ;
            newMarioX = marioX + newPuX * PuSize;
            newMarioY = marioY + newPuY * PuSize;
            newMarioZ = marioZ + newPuZ * PuSize;

            float newCamX, newCamY, newCamZ;
            newCamX = cameraX + newPuX * PuSize;
            newCamY = cameraY + newPuY * PuSize;
            newCamZ = cameraZ + newPuZ * PuSize;

            // Set new mario + camera position
            bool success = true;
            success &= stream.WriteRam(BitConverter.GetBytes(newMarioX), marioAddress + Config.Mario.XOffset);
            success &= stream.WriteRam(BitConverter.GetBytes(newMarioY), marioAddress + Config.Mario.YOffset);
            success &= stream.WriteRam(BitConverter.GetBytes(newMarioZ), marioAddress + Config.Mario.ZOffset);
            if (Config.MoveCameraWithPu)
            {
                success &= stream.WriteRam(BitConverter.GetBytes(newCamX), Config.CameraX);
                success &= stream.WriteRam(BitConverter.GetBytes(newCamY), Config.CameraY);
                success &= stream.WriteRam(BitConverter.GetBytes(newCamZ), Config.CameraZ);
            }
            return success;
        }

        public static bool MoveToPu(ProcessStream stream, int newPuX, int newPuY, int newPuZ)
        {
            var marioAddress = Config.Mario.MarioStructAddress;

            // Get Mario position
            float marioX, marioY, marioZ;
            marioX = BitConverter.ToSingle(stream.ReadRam(marioAddress + Config.Mario.XOffset, 4), 0);
            marioY = BitConverter.ToSingle(stream.ReadRam(marioAddress + Config.Mario.YOffset, 4), 0);
            marioZ = BitConverter.ToSingle(stream.ReadRam(marioAddress + Config.Mario.ZOffset, 4), 0);


            // Get Camera Position
            float cameraX, cameraY, cameraZ;
            cameraX = BitConverter.ToSingle(stream.ReadRam(Config.CameraX, 4), 0);
            cameraY = BitConverter.ToSingle(stream.ReadRam(Config.CameraY, 4), 0);
            cameraZ = BitConverter.ToSingle(stream.ReadRam(Config.CameraZ, 4), 0);

            // Calculate new coordinates
            float newMarioX, newMarioY, newMarioZ;
            newMarioX = PuUtilities.GetRelativePuPosition(marioX) + newPuX * PuSize;
            newMarioY = PuUtilities.GetRelativePuPosition(marioY) + newPuY * PuSize;
            newMarioZ = PuUtilities.GetRelativePuPosition(marioZ) + newPuZ * PuSize;

            float newCamX, newCamY, newCamZ;
            newCamX = PuUtilities.GetRelativePuPosition(cameraX) + newPuX * PuSize;
            newCamY = PuUtilities.GetRelativePuPosition(cameraY) + newPuY * PuSize;
            newCamZ = PuUtilities.GetRelativePuPosition(cameraZ) + newPuZ * PuSize;

            // Set new mario + camera position
            bool success = true;
            success &= stream.WriteRam(BitConverter.GetBytes(newMarioX), marioAddress + Config.Mario.XOffset);
            success &= stream.WriteRam(BitConverter.GetBytes(newMarioY), marioAddress + Config.Mario.YOffset);
            success &= stream.WriteRam(BitConverter.GetBytes(newMarioZ), marioAddress + Config.Mario.ZOffset);
            if (Config.MoveCameraWithPu)
            {
                success &= stream.WriteRam(BitConverter.GetBytes(newCamX), Config.CameraX);
                success &= stream.WriteRam(BitConverter.GetBytes(newCamY), Config.CameraY);
                success &= stream.WriteRam(BitConverter.GetBytes(newCamZ), Config.CameraZ);
            }
            return success;
        }

        public static string GetPuPosString(ProcessStream stream)
        {
            var marioAddress = Config.Mario.MarioStructAddress;

            // Get Mario position
            float marioX, marioZ;
            marioX = BitConverter.ToSingle(stream.ReadRam(marioAddress + Config.Mario.XOffset, 4), 0);
            marioZ = BitConverter.ToSingle(stream.ReadRam(marioAddress + Config.Mario.ZOffset, 4), 0);

            // Update PU
            int puX = PuUtilities.GetPUFromCoord(marioX);
            int puZ = PuUtilities.GetPUFromCoord(marioZ);

            return string.Format("[{0}:{1}]", puX, puZ);
        }
        public static string GetQpuPosString(ProcessStream stream)
        {
            var marioAddress = Config.Mario.MarioStructAddress;

            // Get Mario position
            float marioX, marioZ;
            marioX = BitConverter.ToSingle(stream.ReadRam(marioAddress + Config.Mario.XOffset, 4), 0);
            marioZ = BitConverter.ToSingle(stream.ReadRam(marioAddress + Config.Mario.ZOffset, 4), 0);

            // Update PU
            int puX = PuUtilities.GetPUFromCoord(marioX);
            int puZ = PuUtilities.GetPUFromCoord(marioZ);

            // Update Qpu
            double qpuX = puX / 4d;
            double qpuZ = puZ / 4d;

            return string.Format("[{0}:{1}]", qpuX, qpuZ);
        }
    }
}

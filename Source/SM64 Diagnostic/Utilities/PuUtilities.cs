using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Utilities
{
    public static class PuUtilities
    {
        private static int PuOffset = 32768;
        private static int PuSize = 65536;

        public static float GetRelativePuPosition(float coord, int puCoord)
        {
            // We find the relative object position by subtracting the PU starting coordinates from the object
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

        public static bool MoveToInCurrentPu(float x, float y, float z)
        {
            var marioAddress = MarioConfig.StructAddress;

            x = GetRelativePuPosition(x);
            y = GetRelativePuPosition(y);
            z = GetRelativePuPosition(z);

            float marioX, marioY, marioZ;
            marioX = Config.Stream.GetSingle(marioAddress + MarioConfig.XOffset);
            marioY = Config.Stream.GetSingle(marioAddress + MarioConfig.YOffset);
            marioZ = Config.Stream.GetSingle(marioAddress + MarioConfig.ZOffset);

            int curPuX, curPuY, curPuZ;
            curPuX = GetPUFromCoord(marioX);
            curPuY = GetPUFromCoord(marioY);
            curPuZ = GetPUFromCoord(marioZ);

            float newMarioX, newMarioY, newMarioZ;
            newMarioX = x + curPuX * PuSize;
            newMarioY = y + curPuY * PuSize;
            newMarioZ = z + curPuZ * PuSize;

            bool success = true;
            success &= Config.Stream.SetValue(newMarioX, marioAddress + MarioConfig.XOffset);
            success &= Config.Stream.SetValue(newMarioY, marioAddress + MarioConfig.YOffset);
            success &= Config.Stream.SetValue(newMarioZ, marioAddress + MarioConfig.ZOffset);
            return success;
        }

        public static bool MoveToRelativePu(int newPuX, int newPuY, int newPuZ)
        {
            // Get Mario position
            float marioX, marioY, marioZ;
            marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);

            // Get Camera Position
            float cameraX, cameraY, cameraZ;
            cameraX = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.XOffset);
            cameraY = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.YOffset);
            cameraZ = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.ZOffset);

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
            success &= Config.Stream.SetValue(newMarioX, MarioConfig.StructAddress + MarioConfig.XOffset);
            success &= Config.Stream.SetValue(newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
            success &= Config.Stream.SetValue(newMarioZ, MarioConfig.StructAddress + MarioConfig.ZOffset);
            if (OptionsConfig.MoveCameraWithPu)
            {
                success &= Config.Stream.SetValue(newCamX, CameraConfig.CameraStructAddress + CameraConfig.XOffset);
                success &= Config.Stream.SetValue(newCamY, CameraConfig.CameraStructAddress + CameraConfig.YOffset);
                success &= Config.Stream.SetValue(newCamZ, CameraConfig.CameraStructAddress + CameraConfig.ZOffset);
            }
            return success;
        }

        public static bool MoveToPu(int newPuX, int newPuY, int newPuZ)
        {
            // Get Mario position
            float marioX, marioY, marioZ;
            marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);


            // Get Camera Position
            float cameraX, cameraY, cameraZ;
            cameraX = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.XOffset);
            cameraY = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.YOffset);
            cameraZ = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.ZOffset);

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
            success &= Config.Stream.SetValue(newMarioX, MarioConfig.StructAddress + MarioConfig.XOffset);
            success &= Config.Stream.SetValue(newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
            success &= Config.Stream.SetValue(newMarioZ, MarioConfig.StructAddress + MarioConfig.ZOffset);
            if (OptionsConfig.MoveCameraWithPu)
            {
                success &= Config.Stream.SetValue(newCamX, CameraConfig.CameraStructAddress + CameraConfig.XOffset);
                success &= Config.Stream.SetValue(newCamY, CameraConfig.CameraStructAddress + CameraConfig.YOffset);
                success &= Config.Stream.SetValue(newCamZ, CameraConfig.CameraStructAddress + CameraConfig.ZOffset);
            }
            return success;
        }

        public static string GetPuPosString()
        {
            // Get Mario position
            float marioX, marioZ;
            marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);

            // Update PU
            int puX = PuUtilities.GetPUFromCoord(marioX);
            int puZ = PuUtilities.GetPUFromCoord(marioZ);

            return string.Format("[{0}:{1}]", puX, puZ);
        }

        public static string GetQpuPosString()
        {
            var marioAddress = MarioConfig.StructAddress;

            // Get Mario position
            float marioX, marioZ;
            marioX = Config.Stream.GetSingle(marioAddress + MarioConfig.XOffset);
            marioZ = Config.Stream.GetSingle(marioAddress + MarioConfig.ZOffset);

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

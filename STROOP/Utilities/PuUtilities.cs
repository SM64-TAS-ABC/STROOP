using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Utilities
{
    public static class PuUtilities
    {
        private static int PuOffset = 32768;
        public static int PuSize = 65536;

        public static float GetRelativeCoordinate(float coord)
        {
            return (float)MoreMath.MaybeNegativeModulus(coord, PuSize);
        }

        public static int GetPuIndex(double coord)
        {
            return (int)Math.Floor((coord + PuOffset) / PuSize);
        }

        public static float GetCoordinateInPu(float coord, int puIndex)
        {
            float relativeCoord = GetRelativeCoordinate(coord);
            return relativeCoord + puIndex * PuSize;
        }

        public static bool MoveToInCurrentPu(float x, float y, float z)
        {
            x = (float)GetRelativeCoordinate(x);
            y = (float)GetRelativeCoordinate(y);
            z = (float)GetRelativeCoordinate(z);

            float marioX, marioY, marioZ;
            marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);

            int curPuX, curPuY, curPuZ;
            curPuX = GetPuIndex(marioX);
            curPuY = GetPuIndex(marioY);
            curPuZ = GetPuIndex(marioZ);

            float newMarioX, newMarioY, newMarioZ;
            newMarioX = x + curPuX * PuSize;
            newMarioY = y + curPuY * PuSize;
            newMarioZ = z + curPuZ * PuSize;

            bool success = true;
            success &= Config.Stream.SetValue(newMarioX, MarioConfig.StructAddress + MarioConfig.XOffset);
            success &= Config.Stream.SetValue(newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
            success &= Config.Stream.SetValue(newMarioZ, MarioConfig.StructAddress + MarioConfig.ZOffset);
            return success;
        }

        public static bool MoveToRelativePu(int newPuX, int newPuY, int newPuZ)
        {
            // Get Mario position
            float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);

            // Get Camera Position
            float cameraX = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.XOffset);
            float cameraY = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.YOffset);
            float cameraZ = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.ZOffset);

            // Calculate new coordinates
            float newMarioX = marioX + newPuX * PuSize;
            float newMarioY = marioY + newPuY * PuSize;
            float newMarioZ = marioZ + newPuZ * PuSize;

            float newCamX = cameraX + newPuX * PuSize;
            float newCamY = cameraY + newPuY * PuSize;
            float newCamZ = cameraZ + newPuZ * PuSize;

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
            float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);


            // Get Camera Position
            float cameraX = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.XOffset);
            float cameraY = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.YOffset);
            float cameraZ = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.ZOffset);

            // Calculate new coordinates
            float newMarioX, newMarioY, newMarioZ;
            newMarioX = (float)GetRelativeCoordinate(marioX) + newPuX * PuSize;
            newMarioY = (float)GetRelativeCoordinate(marioY) + newPuY * PuSize;
            newMarioZ = (float)GetRelativeCoordinate(marioZ) + newPuZ * PuSize;

            float newCamX, newCamY, newCamZ;
            newCamX = (float)GetRelativeCoordinate(cameraX) + newPuX * PuSize;
            newCamY = (float)GetRelativeCoordinate(cameraY) + newPuY * PuSize;
            newCamZ = (float)GetRelativeCoordinate(cameraZ) + newPuZ * PuSize;

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
            float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);

            // Update PU
            int puX = PuUtilities.GetPuIndex(marioX);
            int puZ = PuUtilities.GetPuIndex(marioZ);

            return string.Format("[{0}:{1}]", puX, puZ);
        }

        public static string GetQpuPosString()
        {
            // Get Mario position
            float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);

            // Update PU
            int puX = PuUtilities.GetPuIndex(marioX);
            int puZ = PuUtilities.GetPuIndex(marioZ);

            // Update Qpu
            double qpuX = puX / 4d;
            double qpuZ = puZ / 4d;

            return string.Format("[{0}:{1}]", qpuX, qpuZ);
        }
    }
}

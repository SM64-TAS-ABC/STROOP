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

        public static int GetPuIndex(float coord)
        {
            return (int)Math.Floor((coord + PuOffset) / PuSize);
        }

        public static float GetCoordinateInPu(float coord, int puIndex)
        {
            float relativeCoord = GetRelativeCoordinate(coord);
            return relativeCoord + puIndex * PuSize;
        }

        public static (int puXIndex, int puYIndex, int puZIndex) GetMarioPuIndexes()
        {
            float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);

            int puXIndex = GetPuIndex(marioX);
            int puYIndex = GetPuIndex(marioY);
            int puZIndex = GetPuIndex(marioZ);

            return (puXIndex, puYIndex, puZIndex);
        }

        public static bool SetMarioPositionInCurrentPu(float x, float y, float z)
        {
            (int puXIndex, int puYIndex, int puZIndex) = GetMarioPuIndexes();

            float newMarioX = GetCoordinateInPu(x, puXIndex);
            float newMarioY = GetCoordinateInPu(y, puYIndex);
            float newMarioZ = GetCoordinateInPu(z, puZIndex);

            bool success = true;
            success &= Config.Stream.SetValue(newMarioX, MarioConfig.StructAddress + MarioConfig.XOffset);
            success &= Config.Stream.SetValue(newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
            success &= Config.Stream.SetValue(newMarioZ, MarioConfig.StructAddress + MarioConfig.ZOffset);
            return success;
        }

        public static bool TranslateMarioPu(int puXOffset, int puYOffset, int puZOffset)
        {
            (int puXIndex, int puYIndex, int puZIndex) = GetMarioPuIndexes();
            int newPuXIndex = puXIndex + puXOffset;
            int newPuYIndex = puYIndex + puYOffset;
            int newPuZIndex = puZIndex + puZOffset;
            return SetMarioPu(newPuXIndex, newPuYIndex, newPuZIndex);
        }

        public static bool SetMarioPu(int newPuX, int newPuY, int newPuZ)
        {
            float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);

            float newMarioX = GetCoordinateInPu(marioX, newPuX);
            float newMarioY = GetCoordinateInPu(marioY, newPuY);
            float newMarioZ = GetCoordinateInPu(marioZ, newPuZ);

            float cameraX = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.XOffset);
            float cameraY = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.YOffset);
            float cameraZ = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.ZOffset);

            float newCamX = GetCoordinateInPu(cameraX, newPuX);
            float newCamY = GetCoordinateInPu(cameraY, newPuY);
            float newCamZ = GetCoordinateInPu(cameraZ, newPuZ);

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
            (int puXIndex, int puYIndex, int puZIndex) = GetMarioPuIndexes();
            return string.Format("[{0}:{1}]", puXIndex, puZIndex);
        }

        public static string GetQpuPosString()
        {
            (int puXIndex, int puYIndex, int puZIndex) = GetMarioPuIndexes();
            double qpuX = puXIndex / 4d;
            double qpuZ = puZIndex / 4d;
            return string.Format("[{0}:{1}]", qpuX, qpuZ);
        }
    }
}

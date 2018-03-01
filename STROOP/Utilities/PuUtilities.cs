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
        private static readonly int PuOffset = 32768;
        public static readonly int PuSize = 65536;

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

        public static (float relX, float relY, float relZ) GetMarioRelativePosition()
        {
            float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);

            float relX = GetRelativeCoordinate(marioX);
            float relY = GetRelativeCoordinate(marioY);
            float relZ = GetRelativeCoordinate(marioZ);

            return (relX, relY, relZ);
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

        public static string GetPuIndexString(bool useQpu, bool includeY)
        {
            (int puXIndex, int puYIndex, int puZIndex) = GetMarioPuIndexes();

            double xValue = useQpu ? puXIndex / 4d : puXIndex;
            double yValue = useQpu ? puYIndex / 4d : puYIndex;
            double zValue = useQpu ? puZIndex / 4d : puZIndex;

            List<double> values = new List<double>();
            values.Add(xValue);
            if (includeY) values.Add(yValue);
            values.Add(zValue);
            return "[" + String.Join(",", values) + "]";
        }
    }
}

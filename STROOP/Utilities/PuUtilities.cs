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
        public static readonly int HalfPuSize = 32768;
        public static readonly int PuSize = 65536;
        public static readonly int PuSpeed = 65536;
        public static readonly int QpuSpeed = 262144;

        public static double GetRelativeCoordinate(double coord)
        {
            return MoreMath.MaybeNegativeModulus(coord, PuSize);
        }

        public static int GetPuIndex(double coord)
        {
            return (int)Math.Floor((coord + HalfPuSize) / PuSize);
        }

        public static double GetCoordinateInPu(double coord, int puIndex)
        {
            double relativeCoord = GetRelativeCoordinate(coord);
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

        public static bool SetMarioPositionInCurrentPu(double x, double y, double z)
        {
            (int puXIndex, int puYIndex, int puZIndex) = GetMarioPuIndexes();

            double newMarioX = GetCoordinateInPu(x, puXIndex);
            double newMarioY = GetCoordinateInPu(y, puYIndex);
            double newMarioZ = GetCoordinateInPu(z, puZIndex);

            bool success = true;
            success &= Config.Stream.SetValue((float)newMarioX, MarioConfig.StructAddress + MarioConfig.XOffset);
            success &= Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
            success &= Config.Stream.SetValue((float)newMarioZ, MarioConfig.StructAddress + MarioConfig.ZOffset);
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

            double newMarioX = GetCoordinateInPu(marioX, newPuX);
            double newMarioY = GetCoordinateInPu(marioY, newPuY);
            double newMarioZ = GetCoordinateInPu(marioZ, newPuZ);

            float cameraX = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.XOffset);
            float cameraY = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.YOffset);
            float cameraZ = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.ZOffset);

            double newCamX = GetCoordinateInPu(cameraX, newPuX);
            double newCamY = GetCoordinateInPu(cameraY, newPuY);
            double newCamZ = GetCoordinateInPu(cameraZ, newPuZ);

            // Set new mario + camera position
            bool success = true;
            success &= Config.Stream.SetValue((float)newMarioX, MarioConfig.StructAddress + MarioConfig.XOffset);
            success &= Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
            success &= Config.Stream.SetValue((float)newMarioZ, MarioConfig.StructAddress + MarioConfig.ZOffset);
            if (SavedSettingsConfig.MoveCameraWithPu)
            {
                success &= Config.Stream.SetValue((float)newCamX, CameraConfig.CameraStructAddress + CameraConfig.XOffset);
                success &= Config.Stream.SetValue((float)newCamY, CameraConfig.CameraStructAddress + CameraConfig.YOffset);
                success &= Config.Stream.SetValue((float)newCamZ, CameraConfig.CameraStructAddress + CameraConfig.ZOffset);
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

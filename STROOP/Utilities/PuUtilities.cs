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
        public static int HalfPuSize => 32768 * ExtendedLevelBoundariesUtilities.TriangleVertexMultiplier;
        public static int PuSize => 65536 * ExtendedLevelBoundariesUtilities.TriangleVertexMultiplier;
        public static int PuSpeed => 65536 * ExtendedLevelBoundariesUtilities.TriangleVertexMultiplier;
        public static int QpuSpeed => 262144 * ExtendedLevelBoundariesUtilities.TriangleVertexMultiplier;

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
            float marioX = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
            float marioZ = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZOffset);

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
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            success &= Config.Stream.SetValue((float)newMarioX, MarioConfig.StructAddress + MarioConfig.XOffset);
            success &= Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
            success &= Config.Stream.SetValue((float)newMarioZ, MarioConfig.StructAddress + MarioConfig.ZOffset);

            if (!streamAlreadySuspended) Config.Stream.Resume();
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
            float marioX = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
            float marioZ = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZOffset);

            double newMarioX = GetCoordinateInPu(marioX, newPuX);
            double newMarioY = GetCoordinateInPu(marioY, newPuY);
            double newMarioZ = GetCoordinateInPu(marioZ, newPuZ);

            float cameraX = Config.Stream.GetFloat(CameraConfig.StructAddress + CameraConfig.XOffset);
            float cameraY = Config.Stream.GetFloat(CameraConfig.StructAddress + CameraConfig.YOffset);
            float cameraZ = Config.Stream.GetFloat(CameraConfig.StructAddress + CameraConfig.ZOffset);

            double newCamX = GetCoordinateInPu(cameraX, newPuX);
            double newCamY = GetCoordinateInPu(cameraY, newPuY);
            double newCamZ = GetCoordinateInPu(cameraZ, newPuZ);

            bool success = true;
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            success &= Config.Stream.SetValue((float)newMarioX, MarioConfig.StructAddress + MarioConfig.XOffset);
            success &= Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
            success &= Config.Stream.SetValue((float)newMarioZ, MarioConfig.StructAddress + MarioConfig.ZOffset);
            if (SavedSettingsConfig.MoveCameraWithPu)
            {
                success &= Config.Stream.SetValue((float)newCamX, CameraConfig.StructAddress + CameraConfig.XOffset);
                success &= Config.Stream.SetValue((float)newCamY, CameraConfig.StructAddress + CameraConfig.YOffset);
                success &= Config.Stream.SetValue((float)newCamZ, CameraConfig.StructAddress + CameraConfig.ZOffset);
            }

            if (!streamAlreadySuspended) Config.Stream.Resume();
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

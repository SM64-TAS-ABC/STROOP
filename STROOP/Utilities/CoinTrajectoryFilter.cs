using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Utilities
{
    public class CoinTrajectoryFilter
    {
        public readonly double? HSpeedMinNullable;
        public readonly double? HSpeedMaxNullable;
        public readonly double? VSpeedMinNullable;
        public readonly double? VSpeedMaxNullable;
        public readonly double? AngleMinNullable;
        public readonly double? AngleMaxNullable;
        public readonly int? NumQualifiedCoinsMinNullable;

        public CoinTrajectoryFilter(
            double? hSpeedMinNullable,
            double? hSpeedMaxNullable,
            double? vSpeedMinNullable,
            double? vSpeedMaxNullable,
            double? angleMinNullable,
            double? angleMaxNullable,
            int? numQualifiedCoinsMinNullable)
        {
            HSpeedMinNullable = hSpeedMinNullable;
            HSpeedMaxNullable = hSpeedMaxNullable;
            VSpeedMinNullable = vSpeedMinNullable;
            VSpeedMaxNullable = vSpeedMaxNullable;
            AngleMinNullable = angleMinNullable;
            AngleMaxNullable = angleMaxNullable;
            NumQualifiedCoinsMinNullable = numQualifiedCoinsMinNullable;
        }

        public bool Qualifies(List<CoinTrajectory> coinTrajectoryList)
        {
            if (NumQualifiedCoinsMinNullable.HasValue)
            {
                int numQualifiedCoinsMin = NumQualifiedCoinsMinNullable.Value;
                int numQualifiedCoins = coinTrajectoryList.FindAll(
                    coinTrajectory => Qualifies(coinTrajectory)).Count;
                if (numQualifiedCoins < numQualifiedCoinsMin) return false;
            }
            return true;
        }

        public bool Qualifies(CoinTrajectory coinTrajectory)
        {
            double hSpeed = coinTrajectory.HSpeed;
            double vSpeed = coinTrajectory.VSpeed;
            double angle = coinTrajectory.Angle;

            if (HSpeedMinNullable.HasValue)
            {
                double hSpeedMin = HSpeedMinNullable.Value;
                if (hSpeed < hSpeedMin) return false;
            }

            if (HSpeedMaxNullable.HasValue)
            {
                double hSpeedMax = HSpeedMaxNullable.Value;
                if (hSpeed > hSpeedMax) return false;
            }

            if (VSpeedMinNullable.HasValue)
            {
                double vSpeedMin = VSpeedMinNullable.Value;
                if (vSpeed < vSpeedMin) return false;
            }

            if (VSpeedMaxNullable.HasValue)
            {
                double vSpeedMax = VSpeedMaxNullable.Value;
                if (vSpeed > vSpeedMax) return false;
            }

            if (AngleMinNullable.HasValue && AngleMaxNullable.HasValue)
            {
                double angleMin = AngleMinNullable.Value;
                double angleMax = AngleMaxNullable.Value;
                if (!MoreMath.IsAngleBetweenAngles(angle, angleMin, angleMax)) return false;
            }

            return true;
        }

    }
}

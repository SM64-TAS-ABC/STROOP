using STROOP.Forms;
using STROOP.Managers;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public static class GroundMovementCalculator
    {
        // act_hold_walking
        public static MarioState ApplyInput(MarioState initialState, Input input)
        {
            MutableMarioState mutableMarioState = initialState.GetMutableMarioState(input);
            mutableMarioState.IntendedMagnitude *= 0.4f;
            UpdateWalkingSpeed(mutableMarioState);
            PerformGroundStep(mutableMarioState);
            MarioState finalState = mutableMarioState.GetMarioState(initialState, input);
            return finalState;
        }

        // update_walking_speed
        private static void UpdateWalkingSpeed(MutableMarioState marioState)
        {
            float maxTargetSpeed;
            float targetSpeed;

            bool slowSurface = false;
            if (slowSurface)
                maxTargetSpeed = 24.0f;
            else
                maxTargetSpeed = 32.0f;

            targetSpeed = marioState.IntendedMagnitude < maxTargetSpeed ? marioState.IntendedMagnitude : maxTargetSpeed;

            if (marioState.HSpeed <= 0.0f)
                marioState.HSpeed += 1.1f;
            else if (marioState.HSpeed <= targetSpeed)
                marioState.HSpeed += 1.1f - marioState.HSpeed / 43.0f;

            if (marioState.HSpeed > 48.0f)
                marioState.HSpeed = 48.0f;

            marioState.MarioAngle = MoreMath.NormalizeAngleUshort(
                marioState.IntendedAngle - CalculatorUtilities.ApproachInt(
                    MoreMath.NormalizeAngleShort(marioState.IntendedAngle - marioState.MarioAngle), 0, 0x800, 0x800));
            ApplySlopeAccel(marioState);
        }

        // apply_slope_accel
        private static void ApplySlopeAccel(MutableMarioState marioState)
        {
            marioState.XSpeed = marioState.HSpeed * InGameTrigUtilities.InGameSine(marioState.MarioAngle);
            marioState.YSpeed = 0.0f;
            marioState.ZSpeed = marioState.HSpeed * InGameTrigUtilities.InGameCosine(marioState.MarioAngle);
        }

        // perform_ground_step
        private static void PerformGroundStep(MutableMarioState marioState)
        {
            for (int i = 0; i < 4; i++)
            {
                marioState.X = marioState.X + marioState.XSpeed / 4.0f;
                marioState.Z = marioState.Z + marioState.ZSpeed / 4.0f;
            }
        }
    }
}

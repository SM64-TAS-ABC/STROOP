using STROOP.Forms;
using STROOP.Managers;
using STROOP.Models;
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

        public static MarioState ApplyInput(MarioState initialState, int angleDiff)
        {
            MutableMarioState mutableMarioState = initialState.GetMutableMarioState(angleDiff);
            mutableMarioState.IntendedMagnitude *= 0.4f;
            UpdateWalkingSpeed(mutableMarioState);
            PerformGroundStep(mutableMarioState);
            MarioState finalState = mutableMarioState.GetMarioState(initialState, null);
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
            else
                marioState.HSpeed -= 1.0f;

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

        public static void PerformButtSlideMovement(MutableMarioState marioState, TriangleDataModel floor)
        {
            short intendedDYaw = MoreMath.NormalizeAngleShort(marioState.IntendedAngle - marioState.SlidingAngle);
            float forward = InGameTrigUtilities.InGameCosine(intendedDYaw);

            //! 10k glitch
            if (forward < 0.0f && marioState.HSpeed >= 0.0f)
            {
                forward *= 0.5f + 0.5f * marioState.HSpeed / 100.0f;
            }

            float oldSpeed = (float)Math.Sqrt(
                marioState.SlidingSpeedX * marioState.SlidingSpeedX +
                marioState.SlidingSpeedZ * marioState.SlidingSpeedZ);

            marioState.SlidingSpeedX += marioState.SlidingSpeedZ * (marioState.IntendedMagnitude / 32.0f) * InGameTrigUtilities.InGameSine(intendedDYaw) * 0.05f;
            marioState.SlidingSpeedZ -= marioState.SlidingSpeedX * (marioState.IntendedMagnitude / 32.0f) * InGameTrigUtilities.InGameSine(intendedDYaw) * 0.05f;

            float newSpeed = (float)Math.Sqrt(
                marioState.SlidingSpeedX * marioState.SlidingSpeedX +
                marioState.SlidingSpeedZ * marioState.SlidingSpeedZ);

            if (oldSpeed > 0.0f && newSpeed > 0.0f)
            {
                marioState.SlidingSpeedX = marioState.SlidingSpeedX * oldSpeed / newSpeed;
                marioState.SlidingSpeedZ = marioState.SlidingSpeedZ * oldSpeed / newSpeed;
            }

            short slopeAngle = MoreMath.NormalizeAngleShort(InGameTrigUtilities.InGameATan(floor.NormZ, floor.NormX));
            marioState.SlidingSpeedX += 10.0f * floor.NormY * InGameTrigUtilities.InGameSine(slopeAngle);
            marioState.SlidingSpeedZ += 10.0f * floor.NormY * InGameTrigUtilities.InGameCosine(slopeAngle);

            float lossFactor = (marioState.IntendedMagnitude / 32.0f * forward * 0.02f + 0.98f);
            marioState.SlidingSpeedX *= lossFactor;
            marioState.SlidingSpeedZ *= lossFactor;

            marioState.SlidingAngle = InGameTrigUtilities.InGameATan(marioState.SlidingSpeedZ, marioState.SlidingSpeedX);

            short facingDYaw = MoreMath.NormalizeAngleShort(marioState.MarioAngle - marioState.SlidingAngle);
            int newFacingDYaw = facingDYaw;

            //! -0x4000 not handled - can slide down a slope while facing perpendicular to it
            if (newFacingDYaw > 0 && newFacingDYaw <= 0x4000)
            {
                if ((newFacingDYaw -= 0x200) < 0)
                    newFacingDYaw = 0;
            }
            else if (newFacingDYaw > -0x4000 && newFacingDYaw < 0)
            {
                if ((newFacingDYaw += 0x200) > 0)
                    newFacingDYaw = 0;
            }
            else if (newFacingDYaw > 0x4000 && newFacingDYaw < 0x8000)
            {
                if ((newFacingDYaw += 0x200) > 0x8000)
                    newFacingDYaw = 0x8000;
            }
            else if (newFacingDYaw > -0x8000 && newFacingDYaw < -0x4000)
            {
                if ((newFacingDYaw -= 0x200) < -0x8000)
                    newFacingDYaw = -0x8000;
            }

            marioState.MarioAngle = MoreMath.NormalizeAngleUshort(marioState.SlidingAngle + newFacingDYaw);

            marioState.XSpeed = marioState.SlidingSpeedX;
            marioState.YSpeed = 0.0f;
            marioState.ZSpeed = marioState.SlidingSpeedZ;

            marioState.HSpeed = (float)Math.Sqrt(
                marioState.SlidingSpeedX * marioState.SlidingSpeedX +
                marioState.SlidingSpeedZ * marioState.SlidingSpeedZ);
            if (marioState.HSpeed > 100.0f)
            {
                marioState.SlidingSpeedX *= 100.0f / marioState.HSpeed;
                marioState.SlidingSpeedZ *= 100.0f / marioState.HSpeed;
            }

            if (newFacingDYaw < -0x4000 || newFacingDYaw > 0x4000)
            {
                marioState.HSpeed *= -1.0f;
            }
        }
    }
}

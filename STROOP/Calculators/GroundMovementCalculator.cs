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

        public static MarioState PerformButtSlide(MarioState marioState, Input input, TriangleDataModel floor, List<TriangleDataModel> walls)
        {
            MutableMarioState mutableMarioState = marioState.GetMutableMarioState(input);
            common_slide_action_with_jump(mutableMarioState, floor, walls);
            return mutableMarioState.GetMarioState(marioState, input);
        }

        public static MarioState PerformButtSlide(MarioState marioState, int angleDiff, TriangleDataModel floor, List<TriangleDataModel> walls)
        {
            MutableMarioState mutableMarioState = marioState.GetMutableMarioState(angleDiff);
            common_slide_action_with_jump(mutableMarioState, floor, walls);
            return mutableMarioState.GetMarioState(marioState, new Input(angleDiff, 0));
        }

        private static void common_slide_action_with_jump(MutableMarioState marioState, TriangleDataModel floor, List<TriangleDataModel> walls)
        {
            update_sliding(marioState, 4.0f, floor, walls);
            common_slide_action(marioState, floor, walls);
        }

        private static void update_sliding(MutableMarioState marioState, float stopSpeed, TriangleDataModel floor, List<TriangleDataModel> walls)
        {
            short intendedDYaw = MoreMath.NormalizeAngleShort(marioState.IntendedAngle - marioState.SlidingAngle);
            float forward = InGameTrigUtilities.InGameCosine(intendedDYaw);
            float sideward = InGameTrigUtilities.InGameSine(intendedDYaw);

            //! 10k glitch
            if (forward < 0.0f && marioState.HSpeed >= 0.0f)
            {
                forward *= 0.5f + 0.5f * marioState.HSpeed / 100.0f;

            }

            float accel;
            float lossFactor;
            int floorClass = 0x13;
            switch (floorClass)
            {
                case 0x0013:
                    accel = 10.0f;
                    lossFactor = marioState.IntendedMagnitude / 32.0f * forward * 0.02f + 0.98f;
                    break;

                case 0x0014:
                    accel = 8.0f;
                    lossFactor = marioState.IntendedMagnitude / 32.0f * forward * 0.02f + 0.96f;
                    break;

                default:
                    accel = 7.0f;
                    lossFactor = marioState.IntendedMagnitude / 32.0f * forward * 0.02f + 0.92f;
                    break;

                case 0x0015:
                    accel = 5.0f;
                    lossFactor = marioState.IntendedMagnitude / 32.0f * forward * 0.02f + 0.92f;
                    break;
            }

            float oldSpeed = (float)Math.Sqrt(
                marioState.SlidingSpeedX * marioState.SlidingSpeedX +
                marioState.SlidingSpeedZ * marioState.SlidingSpeedZ);

            //! This is attempting to use trig derivatives to rotate mario's speed.
            // It is slightly off/asymmetric since it uses the new X speed, but the old
            // Z speed.
            marioState.SlidingSpeedX += marioState.SlidingSpeedZ * (marioState.IntendedMagnitude / 32.0f) * sideward * 0.05f;
            marioState.SlidingSpeedZ -= marioState.SlidingSpeedX * (marioState.IntendedMagnitude / 32.0f) * sideward * 0.05f;

            float newSpeed = (float)Math.Sqrt(
                marioState.SlidingSpeedX * marioState.SlidingSpeedX + 
                marioState.SlidingSpeedZ * marioState.SlidingSpeedZ);

            if (oldSpeed > 0.0f && newSpeed > 0.0f)
            {
                marioState.SlidingSpeedX = marioState.SlidingSpeedX * oldSpeed / newSpeed;
                marioState.SlidingSpeedZ = marioState.SlidingSpeedZ * oldSpeed / newSpeed;
            }

            update_sliding_angle(marioState, accel, lossFactor, floor, walls);
        }

        private static void update_sliding_angle(MutableMarioState marioState, float accel, float lossFactor, TriangleDataModel floor, List<TriangleDataModel> walls)
        {
            short slopeAngle = MoreMath.NormalizeAngleShort(InGameTrigUtilities.InGameATan(floor.NormZ, floor.NormX));
            float steepness = (float)Math.Sqrt(floor.NormX * floor.NormX + floor.NormZ * floor.NormZ);

            marioState.SlidingSpeedX += accel * steepness * InGameTrigUtilities.InGameSine(slopeAngle);
            marioState.SlidingSpeedZ += accel * steepness * InGameTrigUtilities.InGameCosine(slopeAngle);

            marioState.SlidingSpeedX *= lossFactor;
            marioState.SlidingSpeedZ *= lossFactor;

            marioState.SlidingAngle = InGameTrigUtilities.InGameATan(marioState.SlidingSpeedZ, marioState.SlidingSpeedX);

            short facingDYaw = MoreMath.NormalizeAngleShort(marioState.MarioAngle - marioState.SlidingAngle);
            int newFacingDYaw = facingDYaw;

            //! -0x4000 not handled - can slide down a slope while facing perpendicular to it
            if (newFacingDYaw > 0 && newFacingDYaw <= 0x4000) {
                if ((newFacingDYaw -= 0x200) < 0) {
                    newFacingDYaw = 0;
                }
            } else if (newFacingDYaw > -0x4000 && newFacingDYaw < 0) {
                if ((newFacingDYaw += 0x200) > 0) {
                    newFacingDYaw = 0;
                }
            } else if (newFacingDYaw > 0x4000 && newFacingDYaw < 0x8000) {
                if ((newFacingDYaw += 0x200) > 0x8000) {
                    newFacingDYaw = 0x8000;
                }
            } else if (newFacingDYaw > -0x8000 && newFacingDYaw < -0x4000) {
                if ((newFacingDYaw -= 0x200) < -0x8000) {
                    newFacingDYaw = -0x8000;
                }
            }

            marioState.MarioAngle = MoreMath.NormalizeAngleUshort(marioState.SlidingAngle + newFacingDYaw);

            marioState.XSpeed = marioState.SlidingSpeedX;
            marioState.YSpeed = 0.0f;
            marioState.ZSpeed = marioState.SlidingSpeedZ;

            //! Speed is capped a frame late (butt slide HSG)
            marioState.HSpeed = (float)Math.Sqrt(
                marioState.SlidingSpeedX * marioState.SlidingSpeedX +
                marioState.SlidingSpeedZ * marioState.SlidingSpeedZ);

            if (marioState.HSpeed > 100.0f) {
                marioState.SlidingSpeedX = marioState.SlidingSpeedX * 100.0f / marioState.HSpeed;
                marioState.SlidingSpeedZ = marioState.SlidingSpeedZ * 100.0f / marioState.HSpeed;
            }

            if (newFacingDYaw < -0x4000 || newFacingDYaw > 0x4000) {
                marioState.HSpeed *= -1.0f;
            }
        }

        private static void common_slide_action(
            MutableMarioState marioState, TriangleDataModel floor, List<TriangleDataModel> walls)
        {
            perform_ground_step(marioState, floor, walls);
            // TODO: confirm that result doesn't matter
        }

        private static void perform_ground_step(
            MutableMarioState marioState, TriangleDataModel floor, List<TriangleDataModel> walls)
        {
            for (int i = 0; i < 4; i++)
            {
                float intendedPosX = marioState.X + floor.NormY * (marioState.XSpeed / 4.0f);
                float intendedPosZ = marioState.Z + floor.NormY * (marioState.ZSpeed / 4.0f);
                float intendedPosY = marioState.Y;
                perform_ground_quarter_step(
                    marioState, intendedPosX, intendedPosY, intendedPosZ, floor, walls);
            }
        }

        private static void perform_ground_quarter_step(
            MutableMarioState marioState, float intendedPosX, float intendedPosY, float intendedPosZ,
            TriangleDataModel floor, List<TriangleDataModel> walls)
        {
            (intendedPosX, intendedPosZ) =
                WallDisplacementCalculator.HandleWallDisplacement(
                    intendedPosX, intendedPosY, intendedPosZ, walls, 50, 60);

            float floorHeight = floor.GetTruncatedHeightOnTriangle(intendedPosX, intendedPosZ);

            marioState.X = intendedPosX;
            marioState.Y = floorHeight;
            marioState.Z = intendedPosZ;
        }
    }
}

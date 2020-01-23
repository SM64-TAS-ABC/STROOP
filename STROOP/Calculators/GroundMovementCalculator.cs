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

        //public static void PerformButtSlideMovement(MutableMarioState marioState)
        //{
        //    short intendedDYaw = marioState.IntendedAngle - m->slideYaw;
        //    f32 forward = coss(intendedDYaw);

        //    //! 10k glitch
        //    if (forward < 0.0f && m->forwardVel >= 0.0f)
        //    {
        //        forward *= 0.5f + 0.5f * m->forwardVel / 100.0f;
        //    }

        //    oldSpeed = sqrt(slideVelX ^ 2 + slideVelZ ^ 2);

        //    m->slideVelX += m->slideVelZ * (m->intendedMag / 32.0f) * sins(intendedDYaw) * 0.05f;
        //    m->slideVelZ -= m->slideVelX * (m->intendedMag / 32.0f) * sins(intendedDYaw) * 0.05f;

        //    newSpeed = sqrt(slideVelX ^ 2 + slideVelZ ^ 2);

        //    if (oldSpeed > 0.0f && newSpeed > 0.0f)
        //    {
        //        m->slideVelX = m->slideVelX * oldSpeed / newSpeed;
        //        m->slideVelZ = m->slideVelZ * oldSpeed / newSpeed;
        //    }

        //    s16 slopeAngle = atan2s(floor->normal.z, floor->normal.x);
        //    m->slideVelX += 10.0f * floor->normal.y * sins(slopeAngle);
        //    m->slideVelZ += 10.0f * floor->normal.y * coss(slopeAngle);

        //    f32 lossFactor = (m->intendedMag / 32.0f * forward * 0.02f + 0.98f);
        //    m->slideVelX *= lossFactor;
        //    m->slideVelZ *= lossFactor;

        //    m->slideYaw = atan2s(m->slideVelZ, m->slideVelX);

        //    facingDYaw = m->faceAngle[1] - m->slideYaw;
        //    newFacingDYaw = facingDYaw;

        //    //! -0x4000 not handled - can slide down a slope while facing perpendicular to it
        //    if (newFacingDYaw > 0 && newFacingDYaw <= 0x4000)
        //    {
        //        if ((newFacingDYaw -= 0x200) < 0)
        //            newFacingDYaw = 0;
        //    }
        //    else if (newFacingDYaw > -0x4000 && newFacingDYaw < 0)
        //    {
        //        if ((newFacingDYaw += 0x200) > 0)
        //            newFacingDYaw = 0;
        //    }
        //    else if (newFacingDYaw > 0x4000 && newFacingDYaw < 0x8000)
        //    {
        //        if ((newFacingDYaw += 0x200) > 0x8000)
        //            newFacingDYaw = 0x8000;
        //    }
        //    else if (newFacingDYaw > -0x8000 && newFacingDYaw < -0x4000)
        //    {
        //        if ((newFacingDYaw -= 0x200) < -0x8000)
        //            newFacingDYaw = -0x8000;
        //    }

        //    m->faceAngle[1] = m->slideYaw + newFacingDYaw;

        //    m->vel[0] = m->slideVelX;
        //    m->vel[1] = 0.0f;
        //    m->vel[2] = m->slideVelZ;

        //    m->forwardVel = sqrtf(m->slideVelX * m->slideVelX + m->slideVelZ * m->slideVelZ);
        //    if (m->forwardVel > 100.0f)
        //    {
        //        m->slideVelX *= 100.0f / m->forwardVel;
        //        m->slideVelZ *= 100.0f / m->forwardVel;
        //    }

        //    if (newFacingDYaw < -0x4000 || newFacingDYaw > 0x4000)
        //    {
        //        m->forwardVel *= -1.0f;
        //    }
        //}
    }
}

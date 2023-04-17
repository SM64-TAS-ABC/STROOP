using STROOP.Forms;
using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Utilities
{
    public class TrackPlatform
    {
        private static float sObjSavedPosX;
        private static float sObjSavedPosY;
        private static float sObjSavedPosZ;

        const int POS_OP_SAVE_POSITION = 0;
        const int POS_OP_COMPUTE_VELOCITY = 1;
        const int POS_OP_RESTORE_POSITION = 2;

        const int WAYPOINT_FLAGS_END = -1;

        const int PLATFORM_ON_TRACK_ACT_INIT = 0;
        const int PLATFORM_ON_TRACK_ACT_WAIT_FOR_MARIO = 1;
        const int PLATFORM_ON_TRACK_ACT_MOVE_ALONG_TRACK = 2;
        const int PLATFORM_ON_TRACK_ACT_PAUSE_BRIEFLY = 3;
        const int PLATFORM_ON_TRACK_ACT_FALL = 4;

        const int PLATFORM_ON_TRACK_TYPE_CARPET = 0;
        const int PLATFORM_ON_TRACK_TYPE_SKI_LIFT = 1;
        const int PLATFORM_ON_TRACK_TYPE_CHECKERED = 2;
        const int PLATFORM_ON_TRACK_TYPE_GRATE = 3;

        const ushort PLATFORM_ON_TRACK_BP_MASK_PATH = 0xF;
        const ushort PLATFORM_ON_TRACK_BP_MASK_TYPE = (0x7 << 4);
        const ushort PLATFORM_ON_TRACK_BP_RETURN_TO_START = (1 << 8);
        const ushort PLATFORM_ON_TRACK_BP_DONT_DISAPPEAR = (1 << 9);
        const ushort PLATFORM_ON_TRACK_BP_DONT_TURN_YAW = (1 << 10);
        const ushort PLATFORM_ON_TRACK_BP_DONT_TURN_ROLL = (1 << 11);

        public int oBehParams;
        public int oBehParams2ndByte;

        public float oPosX;
        public float oPosY;
        public float oPosZ;

        public float oHomeX;
        public float oHomeY;
        public float oHomeZ;

        public float oVelX;
        public float oVelY;
        public float oVelZ;
        public float oForwardVel;

        public int oFaceAnglePitch;
        public int oFaceAngleYaw;
        public int oFaceAngleRoll;

        public int oMoveAnglePitch;
        public int oMoveAngleYaw;
        public int oMoveAngleRoll;

        public int oAngleVelPitch;
        public int oAngleVelYaw;
        public int oAngleVelRoll;

        public float oGravity;

        public int oAction;
        public int oPrevAction;
        public int oTimer;

        public int oPlatformOnTrackBaseBallIndex;
        public float oPlatformOnTrackDistMovedSinceLastBall;
        public float oPlatformOnTrackSkiLiftRollVel;
        public TrackPlatformWaypoint oPlatformOnTrackStartWaypoint;
        public TrackPlatformWaypoint oPlatformOnTrackPrevWaypoint;
        public int oPlatformOnTrackPrevWaypointFlags;
        public int oPlatformOnTrackPitch;
        public int oPlatformOnTrackYaw;
        public float oPlatformOnTrackOffsetY;
        public short oPlatformOnTrackIsNotSkiLift;
        public short oPlatformOnTrackIsNotHMC;
        public short oPlatformOnTrackType;
        public short oPlatformOnTrackWasStoodOn;

        public bool isMarioStandingOnPlatform;

        public List<object> GetVariableValues()
        {
            return new List<object>()
            {
                oBehParams,
                oBehParams2ndByte,

                oPosX,
                oPosY,
                oPosZ,

                oHomeX,
                oHomeY,
                oHomeZ,

                oVelX,
                oVelY,
                oVelZ,
                oForwardVel,

                oFaceAnglePitch,
                oFaceAngleYaw,
                oFaceAngleRoll,

                oMoveAnglePitch,
                oMoveAngleYaw,
                oMoveAngleRoll,

                oAngleVelPitch,
                oAngleVelYaw,
                oAngleVelRoll,

                oGravity,

                oAction,
                oPrevAction,
                oTimer,

                oPlatformOnTrackBaseBallIndex,
                oPlatformOnTrackDistMovedSinceLastBall,
                oPlatformOnTrackSkiLiftRollVel,
                oPlatformOnTrackStartWaypoint,
                oPlatformOnTrackPrevWaypoint,
                oPlatformOnTrackPrevWaypointFlags,
                oPlatformOnTrackPitch,
                oPlatformOnTrackYaw,
                oPlatformOnTrackOffsetY,
                oPlatformOnTrackIsNotSkiLift,
                oPlatformOnTrackIsNotHMC,
                oPlatformOnTrackType,
                oPlatformOnTrackWasStoodOn,
            };
        }

        public List<string> GetVariableNames()
        {
            return new List<string>()
            {
                "oBehParams",
                "oBehParams2ndByte",

                "oPosX",
                "oPosY",
                "oPosZ",

                "oHomeX",
                "oHomeY",
                "oHomeZ",

                "oVelX",
                "oVelY",
                "oVelZ",
                "oForwardVel",

                "oFaceAnglePitch",
                "oFaceAngleYaw",
                "oFaceAngleRoll",

                "oMoveAnglePitch",
                "oMoveAngleYaw",
                "oMoveAngleRoll",

                "oAngleVelPitch",
                "oAngleVelYaw",
                "oAngleVelRoll",

                "oGravity",

                "oAction",
                "oPrevAction",
                "oTimer",

                "oPlatformOnTrackBaseBallIndex",
                "oPlatformOnTrackDistMovedSinceLastBall",
                "oPlatformOnTrackSkiLiftRollVel",
                "oPlatformOnTrackStartWaypoint",
                "oPlatformOnTrackPrevWaypoint",
                "oPlatformOnTrackPrevWaypointFlags",
                "oPlatformOnTrackPitch",
                "oPlatformOnTrackYaw",
                "oPlatformOnTrackOffsetY",
                "oPlatformOnTrackIsNotSkiLift",
                "oPlatformOnTrackIsNotHMC",
                "oPlatformOnTrackType",
                "oPlatformOnTrackWasStoodOn",
            };
        }

        public class TrackPlatformWaypoint
        {
            public readonly int Index;
            public readonly int X;
            public readonly int Y;
            public readonly int Z;
            public readonly uint AddressUS;
            public readonly uint AddressJP;

            public TrackPlatformWaypoint(
                int index, int x, int y, int z, uint addressUS, uint addressJP)
            {
                Index = index;
                X = x;
                Y = y;
                Z = z;
                AddressUS = addressUS;
                AddressJP = addressJP;
            }

            public override string ToString()
            {
                return string.Format("Waypoint({0} ({1},{2},{3}) A={4},{5})", Index, X, Y, Z, AddressUS, AddressJP);
            }
        }

        public List<TrackPlatformWaypoint> Waypoints =
            new List<TrackPlatformWaypoint>()
            {
                new TrackPlatformWaypoint(0, -5744, -3072, 0, 2148600892, 2148588988),
                new TrackPlatformWaypoint(1, -5444, -3072, 0, 2148600900, 2148588996),
                new TrackPlatformWaypoint(2, -5144, -3072, 0, 2148600908, 2148589004),
                new TrackPlatformWaypoint(3, -4954, -3122, 0, 2148600916, 2148589012),
                new TrackPlatformWaypoint(4, -4754, -3172, 0, 2148600924, 2148589020),
                new TrackPlatformWaypoint(5, -4300, -3172, 0, 2148600932, 2148589028),
                new TrackPlatformWaypoint(6, -3850, -3172, 0, 2148600940, 2148589036),
                new TrackPlatformWaypoint(7, -3650, -3122, 0, 2148600948, 2148589044),
                new TrackPlatformWaypoint(8, -3460, -3072, 0, 2148600956, 2148589052),
                new TrackPlatformWaypoint(9, -3160, -3072, 0, 2148600964, 2148589060),
                new TrackPlatformWaypoint(10, -3000, -3150, 0, 2148600972, 2148589068),
                new TrackPlatformWaypoint(11, -2800, -3300, 0, 2148600980, 2148589076),
                new TrackPlatformWaypoint(12, -2600, -3450, 0, 2148600988, 2148589084),
                new TrackPlatformWaypoint(13, -2400, -3500, 0, 2148600996, 2148589092),
                new TrackPlatformWaypoint(14, -2200, -3450, 0, 2148601004, 2148589100),
                new TrackPlatformWaypoint(15, -2000, -3300, 0, 2148601012, 2148589108),
                new TrackPlatformWaypoint(16, -1800, -3150, 0, 2148601020, 2148589116),
                new TrackPlatformWaypoint(17, -1600, -3072, 0, 2148601028, 2148589124),
                new TrackPlatformWaypoint(18, -1300, -3072, 0, 2148601036, 2148589132),
                new TrackPlatformWaypoint(19, -1000, -3072, 0, 2148601044, 2148589140),
                new TrackPlatformWaypoint(20, -608, -3072, 0, 2148601052, 2148589148),
                new TrackPlatformWaypoint(21, -1000, -3072, 0, 2148601060, 2148589156),
                new TrackPlatformWaypoint(22, -1300, -3072, 0, 2148601068, 2148589164),
                new TrackPlatformWaypoint(23, -1600, -3072, 0, 2148601076, 2148589172),
                new TrackPlatformWaypoint(24, -1800, -3150, 0, 2148601084, 2148589180),
                new TrackPlatformWaypoint(25, -2000, -3300, 0, 2148601092, 2148589188),
                new TrackPlatformWaypoint(26, -2200, -3450, 0, 2148601100, 2148589196),
                new TrackPlatformWaypoint(27, -2400, -3500, 0, 2148601108, 2148589204),
                new TrackPlatformWaypoint(28, -2600, -3450, 0, 2148601116, 2148589212),
                new TrackPlatformWaypoint(29, -2800, -3300, 0, 2148601124, 2148589220),
                new TrackPlatformWaypoint(30, -3000, -3150, 0, 2148601132, 2148589228),
                new TrackPlatformWaypoint(31, -3160, -3072, 0, 2148601140, 2148589236),
                new TrackPlatformWaypoint(32, -3460, -3072, 0, 2148601148, 2148589244),
                new TrackPlatformWaypoint(33, -3650, -3122, 0, 2148601156, 2148589252),
                new TrackPlatformWaypoint(34, -3850, -3172, 0, 2148601164, 2148589260),
                new TrackPlatformWaypoint(35, -4300, -3172, 0, 2148601172, 2148589268),
                new TrackPlatformWaypoint(36, -4754, -3172, 0, 2148601180, 2148589276),
                new TrackPlatformWaypoint(37, -4954, -3122, 0, 2148601188, 2148589284),
                new TrackPlatformWaypoint(38, -5144, -3072, 0, 2148601196, 2148589292),
                new TrackPlatformWaypoint(39, -5444, -3072, 0, 2148601204, 2148589300),
                null,
            };

        public TrackPlatform()
        {
            oBehParams = 120782848;
            bhv_platform_on_track_init();
        }

        public void Update(bool isMarioStandingOnPlatform)
        {
            this.isMarioStandingOnPlatform = isMarioStandingOnPlatform;
            
            bhv_platform_on_track_update();

            if (oAction != oPrevAction)
            {
                oTimer = 0;
                oPrevAction = oAction;
            }

            oTimer++;
        }

        private void platform_on_track_reset()
        {
            oAction = PLATFORM_ON_TRACK_ACT_INIT;
            oPlatformOnTrackBaseBallIndex += 99;
        }

        private void platform_on_track_mario_not_on_platform()
        {
            throw new NotImplementedException("platform_on_track_mario_not_on_platform");
        }

        private void bhv_platform_on_track_init()
        {
            short pathIndex = 0; // (short)((ushort)(oBehParams >> 16) & PLATFORM_ON_TRACK_BP_MASK_PATH);
            oPlatformOnTrackType = 3; // (short)(((ushort)(oBehParams >> 16) & PLATFORM_ON_TRACK_BP_MASK_TYPE) >> 4);

            oPlatformOnTrackIsNotSkiLift = (short)(oPlatformOnTrackType - PLATFORM_ON_TRACK_TYPE_SKI_LIFT);

            oPlatformOnTrackStartWaypoint = Waypoints[pathIndex];

            oPlatformOnTrackIsNotHMC = -1; // (short)(pathIndex - 4);

            oBehParams2ndByte = oMoveAngleYaw;
        }

        private void platform_on_track_act_init()
        {
            int i;

            oPlatformOnTrackPrevWaypoint = oPlatformOnTrackStartWaypoint;
            oPlatformOnTrackPrevWaypointFlags = 0;
            oPlatformOnTrackBaseBallIndex = 0;

            oHomeX = oPlatformOnTrackStartWaypoint.X;
            oHomeY = oPlatformOnTrackStartWaypoint.Y;
            oHomeZ = oPlatformOnTrackStartWaypoint.Z;

            oPosX = oHomeX;
            oPosY = oHomeY;
            oPosZ = oHomeZ;

            oFaceAngleYaw = oBehParams2ndByte;
            oPlatformOnTrackDistMovedSinceLastBall = 0;
            oVelX = 0;
            oVelY = 0;
            oVelZ = 0;
            oForwardVel = 0;

            oPlatformOnTrackWasStoodOn = 0;

            if (oPlatformOnTrackIsNotSkiLift != 0)
            {
                oFaceAngleRoll = 0;
            }

            for (i = 1; i < 6; i++)
            {
                platform_on_track_update_pos_or_spawn_ball(i, oHomeX, oHomeY, oHomeZ);
            }

            oAction = PLATFORM_ON_TRACK_ACT_WAIT_FOR_MARIO;
        }

        private uint obj_perform_position_op(int op)
        {
            switch (op)
            {
                case POS_OP_SAVE_POSITION:
                    sObjSavedPosX = oPosX;
                    sObjSavedPosY = oPosY;
                    sObjSavedPosZ = oPosZ;
                    break;

                case POS_OP_COMPUTE_VELOCITY:
                    oVelX = oPosX - sObjSavedPosX;
                    oVelY = oPosY - sObjSavedPosY;
                    oVelZ = oPosZ - sObjSavedPosZ;
                    break;

                case POS_OP_RESTORE_POSITION:
                    oPosX = sObjSavedPosX;
                    oPosY = sObjSavedPosY;
                    oPosZ = sObjSavedPosZ;
                    break;
            }

            return 0;
        }

        private void platform_on_track_update_pos_or_spawn_ball(int ballIndex, float x, float y, float z)
        {
            TrackPlatformWaypoint initialPrevWaypoint;
            TrackPlatformWaypoint nextWaypoint;
            TrackPlatformWaypoint prevWaypoint;

            float amountToMove;
            float dx;
            float dy;
            float dz;
            float distToNextWaypoint;

            if (ballIndex == 0 || ((ushort)(oBehParams >> 16) & 0x0080) != 0)
            {
                initialPrevWaypoint = oPlatformOnTrackPrevWaypoint;
                nextWaypoint = initialPrevWaypoint;

                if (ballIndex != 0) {
                    amountToMove = 300.0f * ballIndex;
                } else {
                    obj_perform_position_op(POS_OP_SAVE_POSITION);
                    oPlatformOnTrackPrevWaypointFlags = 0;
                    amountToMove = oForwardVel;
                }

                do {
                    prevWaypoint = nextWaypoint;

                    nextWaypoint = Waypoints[nextWaypoint.Index + 1];
                    if (nextWaypoint == null)
                    {
                        if (ballIndex == 0)
                        {
                            oPlatformOnTrackPrevWaypointFlags = WAYPOINT_FLAGS_END;
                        }

                        if (((ushort)(oBehParams >> 16) & PLATFORM_ON_TRACK_BP_RETURN_TO_START) != 0)
                        {
                            nextWaypoint = oPlatformOnTrackStartWaypoint;
                        }
                        else
                        {
                            return;
                        }
                    }

                    dx = nextWaypoint.X - x;
                    dy = nextWaypoint.Y - y;
                    dz = nextWaypoint.Z - z;

                    distToNextWaypoint = (float)Math.Sqrt(dx* dx + dy* dy + dz* dz);

                    amountToMove -= distToNextWaypoint;
                    x += dx;
                    y += dy;
                    z += dz;
                } while (amountToMove > 0.0f);

                distToNextWaypoint = amountToMove / distToNextWaypoint;
                x += dx * distToNextWaypoint;
                y += dy * distToNextWaypoint;
                z += dz * distToNextWaypoint;

                if (ballIndex != 0)
                {
                    throw new NotImplementedException("spawning ball");
                }
                else
                {
                    if (prevWaypoint != initialPrevWaypoint)
                    {
                        if (oPlatformOnTrackPrevWaypointFlags == 0)
                        {
                            oPlatformOnTrackPrevWaypointFlags = initialPrevWaypoint.Index;
                        }
                        oPlatformOnTrackPrevWaypoint = prevWaypoint;
                    }

                    oPosX = x;
                    oPosY = y;
                    oPosZ = z;

                    obj_perform_position_op(POS_OP_COMPUTE_VELOCITY);

                    oPlatformOnTrackPitch = InGameTrigUtilities.InGameATan((float)Math.Sqrt(oVelX * oVelX + oVelZ * oVelZ), -oVelY);
                    oPlatformOnTrackYaw = InGameTrigUtilities.InGameATan(oVelZ, oVelX);
                }
            }
        }

        private bool IsMarioStandingOnPlatform()
        {
            return isMarioStandingOnPlatform;
        }

        private void platform_on_track_act_wait_for_mario()
        {
            if (IsMarioStandingOnPlatform())
            {
                if (oTimer > 20)
                {
                    oAction = PLATFORM_ON_TRACK_ACT_MOVE_ALONG_TRACK;
                }
            }
            else
            {
                oTimer = 0;
            }
        }

        private int obj_forward_vel_approach(float target, float delta)
        {
            return approach_f32_ptr(ref oForwardVel, target, delta);
        }

        private int approach_f32_ptr(ref float px, float target, float delta)
        {
            if (px > target)
            {
                delta = -delta;
            }

            px += delta;

            if ((px - target) * delta >= 0)
            {
                px = target;
                return 1;
            }
            return 0;
        }

        private short abs_angle_diff(short x0, short x1)
        {
            short diff = (short)(x1 - x0);

            if (diff == -0x8000)
            {
                diff = -0x7FFF;
            }

            if (diff < 0)
            {
                diff = (short)(-diff);
            }

            return diff;
        }

        private int clamp_s16(ref short value, short minimum, short maximum)
        {
            if (value <= minimum)
            {
                value = minimum;
            }
            else if (value >= maximum)
            {
                value = maximum;
            }
            else
            {
                return 0;
            }

            return 1;
        }

        short approach_s16_symmetric(short value, short target, short increment)
        {
            short dist = (short)(target - value);

            if (dist >= 0)
            {
                if (dist > increment)
                {
                    value += increment;
                }
                else
                {
                    value = target;
                }
            }
            else
            {
                if (dist < -increment)
                {
                    value -= increment;
                }
                else
                {
                    value = target;
                }
            }

            return value;
        }

        private int obj_face_yaw_approach(short targetYaw, short deltaYaw)
        {
            oFaceAngleYaw = approach_s16_symmetric((short)oFaceAngleYaw, targetYaw, deltaYaw);

            if ((short)oFaceAngleYaw == targetYaw)
            {
                return 1;
            }

            return 0;
        }

        private int obj_face_roll_approach(short targetRoll, short deltaRoll)
        {
            oFaceAngleRoll = approach_s16_symmetric((short)oFaceAngleRoll, targetRoll, deltaRoll);

            if (oFaceAngleRoll == targetRoll)
            {
                return 1;
            }

            return 0;
        }

        private void platform_on_track_act_move_along_track()
        {
            short initialAngle;

            if (oPlatformOnTrackIsNotSkiLift == 0)
            {
                obj_forward_vel_approach(10f, 0.1f);
            }
            else
            {
                oForwardVel = 10.0f;
            }

            if (approach_f32_ptr(ref oPlatformOnTrackDistMovedSinceLastBall, 300.0f, oForwardVel) != 0)
            {
                oPlatformOnTrackDistMovedSinceLastBall -= 300.0f;

                oHomeX = oPosX;
                oHomeY = oPosY;
                oHomeZ = oPosZ;
                oPlatformOnTrackBaseBallIndex = (ushort)(oPlatformOnTrackBaseBallIndex + 1);

                platform_on_track_update_pos_or_spawn_ball(5, oHomeX, oHomeY, oHomeZ);
            }

            platform_on_track_update_pos_or_spawn_ball(0, oPosX, oPosY, oPosZ);

            oMoveAnglePitch = oPlatformOnTrackPitch;
            oMoveAngleYaw = oPlatformOnTrackYaw;

            if (((ushort)(oBehParams >> 16) & PLATFORM_ON_TRACK_BP_DONT_TURN_YAW) == 0)
            {
                short targetFaceYaw = (short)(oMoveAngleYaw + 0x4000);
                short yawSpeed = (short)(abs_angle_diff(targetFaceYaw, (short)oFaceAngleYaw) / 20);

                initialAngle = (short)oFaceAngleYaw;
                clamp_s16(ref yawSpeed, 100, 500);
                obj_face_yaw_approach(targetFaceYaw, yawSpeed);
                oAngleVelYaw = (short)oFaceAngleYaw - initialAngle;
            }

            if (((ushort)(oBehParams >> 16) & PLATFORM_ON_TRACK_BP_DONT_TURN_ROLL) != 0)
            {
                short rollSpeed = (short)(abs_angle_diff((short)oMoveAnglePitch, (short)oFaceAngleRoll) / 20);

                initialAngle = (short)oFaceAngleRoll;
                clamp_s16(ref rollSpeed, 100, 500);
                obj_face_roll_approach((short)oMoveAnglePitch, rollSpeed);
                oAngleVelRoll = (short)oFaceAngleRoll - initialAngle;
            }

            if (!IsMarioStandingOnPlatform())
            {
                platform_on_track_mario_not_on_platform();
            }
            else
            {
                oTimer = 0;
            }
        }

        private void platform_on_track_act_pause_briefly()
        {
            if (oTimer > 20)
            {
                oAction = PLATFORM_ON_TRACK_ACT_MOVE_ALONG_TRACK;
            }
        }

        private int cur_obj_within_12k_bounds()
        {
            if (oPosX < -12000.0f || 12000.0f < oPosX)
            {
                return 0;
            }

            if (oPosY < -12000.0f || 12000.0f < oPosY)
            {
                return 0;
            }

            if (oPosZ < -12000.0f || 12000.0f < oPosZ)
            {
                return 0;
            }

            return 1;
        }

        private void cur_obj_move_using_vel_and_gravity()
        {
            if (cur_obj_within_12k_bounds() != 0)
            {
                oPosX += oVelX;
                oPosZ += oVelZ;
                oVelY += oGravity;
                oPosY += oVelY;
            }
        }

        private void platform_on_track_act_fall()
        {
            cur_obj_move_using_vel_and_gravity();

            if (!IsMarioStandingOnPlatform())
            {
                platform_on_track_mario_not_on_platform();
            }
            else
            {
                oTimer = 0;
            }
        }

        private void platform_on_track_rock_ski_lift()
        {
            throw new NotSupportedException("platform_on_track_rock_ski_lift");
        }

        private void bhv_platform_on_track_update()
        {
            switch (oAction)
            {
                case PLATFORM_ON_TRACK_ACT_INIT:
                    platform_on_track_act_init();
                    break;
                case PLATFORM_ON_TRACK_ACT_WAIT_FOR_MARIO:
                    platform_on_track_act_wait_for_mario();
                    break;
                case PLATFORM_ON_TRACK_ACT_MOVE_ALONG_TRACK:
                    platform_on_track_act_move_along_track();
                    break;
                case PLATFORM_ON_TRACK_ACT_PAUSE_BRIEFLY:
                    platform_on_track_act_pause_briefly();
                    break;
                case PLATFORM_ON_TRACK_ACT_FALL:
                    platform_on_track_act_fall();
                    break;
            }

            if (oPlatformOnTrackIsNotSkiLift == 0)
            {
                platform_on_track_rock_ski_lift();
            }
            else if (oPlatformOnTrackType == PLATFORM_ON_TRACK_TYPE_CARPET)
            {
                if ((oPlatformOnTrackWasStoodOn == 0) && IsMarioStandingOnPlatform())
                {
                    oPlatformOnTrackOffsetY = -8.0f;
                    oPlatformOnTrackWasStoodOn = 1;
                }

                approach_f32_ptr(ref oPlatformOnTrackOffsetY, 0.0f, 0.5f);
                oPosY += oPlatformOnTrackOffsetY;
            }
        }

        private void bhv_track_ball_update()
        {
            // do nothing
        }

        public override string ToString()
        {
            List<string> variableNames = GetVariableNames();
            List<object> variableValues = GetVariableValues();

            string output = "";
            for (int i = 0; i < variableNames.Count; i++)
            {
                output += variableNames[i] + ": " + variableValues[i] + "\r\n";
            }
            return output;
        }
    }
}

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
    public static class SlideCalculator
    {
        private static CellSnapshot _cellSnapshot;

        const uint LEVEL_BOUNDARY_MAX = 0x2000;
        const int CELL_SIZE = (1 << 10);
        const int NUM_CELLS = (int)(2 * LEVEL_BOUNDARY_MAX / CELL_SIZE);
        const int NUM_CELLS_INDEX = (NUM_CELLS - 1);

        const int GROUND_STEP_LEFT_GROUND = 0;
        const int GROUND_STEP_NONE = 1;
        const int GROUND_STEP_HIT_WALL = 2;
        const int GROUND_STEP_HIT_WALL_STOP_QSTEPS = 2;
        const int GROUND_STEP_HIT_WALL_CONTINUE_QSTEPS = 3;

        const uint ACT_CROUCHING = 0x0C008220;
        const uint ACT_JUMP = 0x03000880;
        const uint ACT_FREEFALL = 0x0100088C;
        const uint ACT_CRAWLING = 0x04008448;
        const uint ACT_GROUND_BONK = 0x00020466;

        const byte MARIO_ANIM_START_CROUCHING = 0x97;

        const ushort TERRAIN_SLIDE = 0x0006;
        const ushort TERRAIN_MASK = 0x0007;

        const ushort SURFACE_CLASS_DEFAULT = 0x0000;
        const ushort SURFACE_CLASS_VERY_SLIPPERY = 0x0013;
        const ushort SURFACE_CLASS_SLIPPERY = 0x0014;
        const ushort SURFACE_CLASS_NOT_SLIPPERY = 0x0015;

        const ushort SURFACE_NOT_SLIPPERY = 0x0015;
        const ushort SURFACE_HARD_NOT_SLIPPERY = 0x0037;
        const ushort SURFACE_SWITCH = 0x007A;
        const ushort SURFACE_SLIPPERY = 0x0014;
        const ushort SURFACE_NOISE_SLIPPERY = 0x002A;
        const ushort SURFACE_HARD_SLIPPERY = 0x0035;
        const ushort SURFACE_NO_CAM_COL_SLIPPERY = 0x0079;
        const ushort SURFACE_VERY_SLIPPERY = 0x0013;
        const ushort SURFACE_ICE = 0x002E;
        const ushort SURFACE_HARD_VERY_SLIPPERY = 0x0036;
        const ushort SURFACE_NOISE_VERY_SLIPPERY_73 = 0x0073;
        const ushort SURFACE_NOISE_VERY_SLIPPERY_74 = 0x0074;
        const ushort SURFACE_NOISE_VERY_SLIPPERY = 0x0075;
        const ushort SURFACE_NO_CAM_COL_VERY_SLIPPERY = 0x0078;

        public static void SetCellSnapshot(CellSnapshot cellSnapshot)
        {
            _cellSnapshot = cellSnapshot;
        }

        public static int act_crouch_slide(SlidingMarioState m)
        {
            int cancel;

            cancel = common_slide_action_with_jump(
                m, ACT_CROUCHING, ACT_JUMP, ACT_FREEFALL, MARIO_ANIM_START_CROUCHING);

            return cancel;
        }

        public static int common_slide_action_with_jump(
            SlidingMarioState m, uint stopAction, uint jumpAction, uint airAction, int animation)
        {

            if (update_sliding(m, 4.0f) != 0)
            {
                //return set_mario_action(m, stopAction, 0);
                return 1;
            }

            common_slide_action(m, stopAction, airAction, animation);
            
            return 0;
        }

        public static void common_slide_action(SlidingMarioState m, uint endAction, uint airAction, int animation) {
            (float x, float y, float z) pos;

            pos = (m.X, m.Y, m.Z);

            switch (perform_ground_step(m)) {
                case GROUND_STEP_LEFT_GROUND:
                    //set_mario_action(m, airAction, 0);
                    break;

                case GROUND_STEP_NONE:
                    //set_mario_animation(m, animation);
                    align_with_floor(m);
                    break;

                case GROUND_STEP_HIT_WALL:
                    if (!(mario_floor_is_slippery(m) != 0))
                    {
                        slide_bonk(m, ACT_GROUND_BONK, endAction);
                    }
                    else if (m.Wall != null)
                    {
                        short wallAngle = MoreMath.NormalizeAngleShort(InGameTrigUtilities.InGameATan(m.Wall.NormZ, m.Wall.NormX));
                        float slideSpeed = (float)Math.Sqrt(m.SlidingSpeedX * m.SlidingSpeedX + m.SlidingSpeedZ * m.SlidingSpeedZ);

                        slideSpeed = (float)(slideSpeed * 0.9);
                        if (slideSpeed < 4.0f)
                        {
                            slideSpeed = 4.0f;
                        }

                        m.SlidingAngle = MoreMath.NormalizeAngleUshort(wallAngle - (short)(m.SlidingAngle - wallAngle) + 0x8000);

                        m.XSpeed = m.SlidingSpeedX = slideSpeed * InGameTrigUtilities.InGameSine(m.SlidingAngle);
                        m.ZSpeed = m.SlidingSpeedZ = slideSpeed * InGameTrigUtilities.InGameCosine(m.SlidingAngle);
                    }

                    align_with_floor(m);
                    break;
            }
        }

        public static void slide_bonk(SlidingMarioState m, uint fastAction, uint slowAction)
        {
            if (m.HSpeed > 16.0f)
            {
                mario_bonk_reflection(m, 1);
                //drop_and_set_mario_action(m, fastAction, 0);
            }
            else
            {
                mario_set_forward_vel(m, 0.0f);
                //set_mario_action(m, slowAction, 0);
            }
        }

        public static void mario_bonk_reflection(SlidingMarioState m, uint negateSpeed)
        {
            if (m.Wall != null)
            {
                short wallAngle = MoreMath.NormalizeAngleShort(InGameTrigUtilities.InGameATan(m.Wall.NormZ, m.Wall.NormX));
                m.MarioAngle = (ushort)(wallAngle - (short)(m.MarioAngle - wallAngle));
            }
            else
            {

            }

            if (negateSpeed != 0)
            {
                mario_set_forward_vel(m, -m.HSpeed);
            }
            else
            {
                m.MarioAngle += 0x8000;
            }
        }

        public static void align_with_floor(SlidingMarioState m)
        {
            m.Y = m.FloorHeight;
        }

        public static int perform_ground_step(SlidingMarioState m)
        {
            int i;
            uint stepResult = 0;
            (float x, float y, float z) intendedPos;

            for (i = 0; i < 4; i++)
            {
                intendedPos.x = m.X + m.Floor.NormY * (m.XSpeed / 4.0f);
                intendedPos.z = m.Z + m.Floor.NormY * (m.ZSpeed / 4.0f);
                intendedPos.y = m.Y;

                stepResult = (uint)perform_ground_quarter_step(m, ref intendedPos);
                if (stepResult == GROUND_STEP_LEFT_GROUND || stepResult == GROUND_STEP_HIT_WALL_STOP_QSTEPS) {
                    break;
                }
            }

            if (stepResult == GROUND_STEP_HIT_WALL_CONTINUE_QSTEPS)
            {
                stepResult = GROUND_STEP_HIT_WALL;
            }
            return (int)stepResult;
        }

        static int perform_ground_quarter_step(SlidingMarioState m, ref (float x, float y, float z) nextPos)
        {
            TriangleDataModel lowerWall;
            TriangleDataModel upperWall;
            TriangleDataModel ceil;
            TriangleDataModel floor;
            float ceilHeight;
            float floorHeight;
            float waterLevel;

            lowerWall = resolve_and_return_wall_collisions(ref nextPos, 30.0f, 24.0f);
            upperWall = resolve_and_return_wall_collisions(ref nextPos, 60.0f, 50.0f);

            (floor, floorHeight) = _cellSnapshot.FindFloorAndY(nextPos.x, nextPos.y, nextPos.z);
            (ceil, ceilHeight) = _cellSnapshot.FindCeilingAndY(nextPos.x, floorHeight, nextPos.z);

            //waterLevel = find_water_level(nextPos[0], nextPos[2]);
            waterLevel = -11000;

            m.Wall = upperWall;

            if (floor == null)
            {
                return GROUND_STEP_HIT_WALL_STOP_QSTEPS;
            }

            if (nextPos.y > floorHeight + 100.0f)
            {
                if (nextPos.y + 160.0f >= ceilHeight)
                {
                    return GROUND_STEP_HIT_WALL_STOP_QSTEPS;
                }

                m.X = nextPos.x;
                m.Y = nextPos.y;
                m.Z = nextPos.z;
                m.Floor = floor;
                m.FloorHeight = floorHeight;
                return GROUND_STEP_LEFT_GROUND;
            }

            if (floorHeight + 160.0f >= ceilHeight)
            {
                return GROUND_STEP_HIT_WALL_STOP_QSTEPS;
            }

            m.X = nextPos.x;
            m.Y = floorHeight;
            m.Z = nextPos.z;
            m.Floor = floor;
            m.FloorHeight = floorHeight;

            if (upperWall != null)
            {
                short wallDYaw = MoreMath.NormalizeAngleShort(InGameTrigUtilities.InGameATan(upperWall.NormZ, upperWall.NormX) - m.MarioAngle);

                if (wallDYaw >= 0x2AAA && wallDYaw <= 0x5555)
                {
                    return GROUND_STEP_NONE;
                }
                if (wallDYaw <= -0x2AAA && wallDYaw >= -0x5555)
                {
                    return GROUND_STEP_NONE;
                }

                return GROUND_STEP_HIT_WALL_CONTINUE_QSTEPS;
            }

            return GROUND_STEP_NONE;
        }

        public class WallCollisionData
        {
            public float x;
            public float y;
            public float z;
            public float offsetY;
            public float radius;
            public short numWalls;
            public TriangleDataModel[] walls = new TriangleDataModel[4];
        };

        public static TriangleDataModel resolve_and_return_wall_collisions(
            ref (float x, float y, float z) pos, float offset, float radius)
        {
            WallCollisionData collisionData = new WallCollisionData();
            TriangleDataModel wall = null;

            collisionData.x = pos.x;
            collisionData.y = pos.y;
            collisionData.z = pos.z;
            collisionData.radius = radius;
            collisionData.offsetY = offset;

            if (find_wall_collisions(collisionData) != 0)
            {
                wall = collisionData.walls[collisionData.numWalls - 1];
            }

            pos.x = collisionData.x;
            pos.y = collisionData.y;
            pos.z = collisionData.z;

            return wall;
        }

        public static int find_wall_collisions(WallCollisionData colData)
        {
            short cellX, cellZ;
            int numCollisions = 0;
            short x = (short)colData.x;
            short z = (short)colData.z;

            colData.numWalls = 0;

            if (x <= -LEVEL_BOUNDARY_MAX || x >= LEVEL_BOUNDARY_MAX) {
                return numCollisions;
            }
            if (z <= -LEVEL_BOUNDARY_MAX || z >= LEVEL_BOUNDARY_MAX) {
                return numCollisions;
            }

            cellX = (short)(((x + LEVEL_BOUNDARY_MAX) / CELL_SIZE) & NUM_CELLS_INDEX);
            cellZ = (short)(((z + LEVEL_BOUNDARY_MAX) / CELL_SIZE) & NUM_CELLS_INDEX);

            List<TriangleDataModel> dynamicWalls = _cellSnapshot.GetTrianglesInCell(cellX, cellZ, false, TriangleClassification.Wall);
            numCollisions += find_wall_collisions_from_list(colData, dynamicWalls);

            List<TriangleDataModel> staticWalls = _cellSnapshot.GetTrianglesInCell(cellX, cellZ, true, TriangleClassification.Wall);
            numCollisions += find_wall_collisions_from_list(colData, staticWalls);

            return numCollisions;
        }

        public static int find_wall_collisions_from_list(WallCollisionData data, List<TriangleDataModel> walls)
        {
            float offset;
            float radius = data.radius;
            float x = data.x;
            float y = data.y + data.offsetY;
            float z = data.z;
            float px, pz;
            float w1, w2, w3;
            float y1, y2, y3;
            int numCols = 0;

            if (radius > 200.0f)
            {
                radius = 200.0f;
            }

            foreach (TriangleDataModel surf in walls)
            {
                if (y < surf.YMinMinus5 || y > surf.YMaxPlus5)
                {
                    continue;
                }

                offset = surf.NormX * x + surf.NormY * y + surf.NormZ * z + surf.NormOffset;

                if (offset < -radius || offset > radius)
                {
                    continue;
                }

                px = x;
                pz = z;

                if (surf.XProjection)
                {
                    w1 = -surf.Z1;            
                    w2 = -surf.Z2;            
                    w3 = -surf.Z3;
                    y1 = surf.Y1;            
                    y2 = surf.Y2;            
                    y3 = surf.Y3;

                    if (surf.NormX > 0.0f)
                    {
                        if ((y1 - y) * (w2 - w1) - (w1 - -pz) * (y2 - y1) > 0.0f)
                        {
                            continue;
                        }
                        if ((y2 - y) * (w3 - w2) - (w2 - -pz) * (y3 - y2) > 0.0f)
                        {
                            continue;
                        }
                        if ((y3 - y) * (w1 - w3) - (w3 - -pz) * (y1 - y3) > 0.0f)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if ((y1 - y) * (w2 - w1) - (w1 - -pz) * (y2 - y1) < 0.0f)
                        {
                            continue;
                        }
                        if ((y2 - y) * (w3 - w2) - (w2 - -pz) * (y3 - y2) < 0.0f)
                        {
                            continue;
                        }
                        if ((y3 - y) * (w1 - w3) - (w3 - -pz) * (y1 - y3) < 0.0f)
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    w1 = surf.X1;
                    w2 = surf.X2;
                    w3 = surf.X3;
                    y1 = surf.Y1;
                    y2 = surf.Y2;
                    y3 = surf.Y3;

                    if (surf.NormZ > 0.0f)
                    {
                        if ((y1 - y) * (w2 - w1) - (w1 - px) * (y2 - y1) > 0.0f)
                        {
                            continue;
                        }
                        if ((y2 - y) * (w3 - w2) - (w2 - px) * (y3 - y2) > 0.0f)
                        {
                            continue;
                        }
                        if ((y3 - y) * (w1 - w3) - (w3 - px) * (y1 - y3) > 0.0f)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if ((y1 - y) * (w2 - w1) - (w1 - px) * (y2 - y1) < 0.0f)
                        {
                            continue;
                        }
                        if ((y2 - y) * (w3 - w2) - (w2 - px) * (y3 - y2) < 0.0f)
                        {
                            continue;
                        }
                        if ((y3 - y) * (w1 - w3) - (w3 - px) * (y1 - y3) < 0.0f)
                        {
                            continue;
                        }
                    }
                }

                data.x += surf.NormX * (radius - offset);
                data.z += surf.NormZ * (radius - offset);

                if (data.numWalls < 4)
                {
                    data.walls[data.numWalls++] = surf;
                }

                numCols++;
            }

            return numCols;
        }

        public static int update_sliding(SlidingMarioState m, float stopSpeed)
        {
            float lossFactor;
            float accel;
            float oldSpeed;
            float newSpeed;

            int stopped = 0;

            short intendedDYaw = MoreMath.NormalizeAngleShort(m.IntendedAngle - m.SlidingAngle);
            float forward = InGameTrigUtilities.InGameCosine(intendedDYaw);
            float sideward = InGameTrigUtilities.InGameSine(intendedDYaw);

            //! 10k glitch
            if (forward < 0.0f && m.HSpeed >= 0.0f) {
                forward *= 0.5f + 0.5f * m.HSpeed / 100.0f;
            }

            switch (mario_get_floor_class(m)) {
                case SURFACE_CLASS_VERY_SLIPPERY:
                    accel = 10.0f;
                    lossFactor = m.IntendedMagnitude / 32.0f * forward * 0.02f + 0.98f;
                    break;

                case SURFACE_CLASS_SLIPPERY:
                    accel = 8.0f;
                    lossFactor = m.IntendedMagnitude / 32.0f * forward * 0.02f + 0.96f;
                    break;

                default:
                    accel = 7.0f;
                    lossFactor = m.IntendedMagnitude / 32.0f * forward * 0.02f + 0.92f;
                    break;

                case SURFACE_CLASS_NOT_SLIPPERY:
                    accel = 5.0f;
                    lossFactor = m.IntendedMagnitude / 32.0f * forward * 0.02f + 0.92f;
                    break;
            }

            oldSpeed = (float)Math.Sqrt(m.SlidingSpeedX * m.SlidingSpeedX + m.SlidingSpeedZ * m.SlidingSpeedZ);

            m.SlidingSpeedX += m.SlidingSpeedZ * (m.IntendedMagnitude / 32.0f) * sideward * 0.05f;
            m.SlidingSpeedZ -= m.SlidingSpeedX * (m.IntendedMagnitude / 32.0f) * sideward * 0.05f;

            newSpeed = (float)Math.Sqrt(m.SlidingSpeedX * m.SlidingSpeedX + m.SlidingSpeedZ * m.SlidingSpeedZ);

            if (oldSpeed > 0.0f && newSpeed > 0.0f) {
                m.SlidingSpeedX = m.SlidingSpeedX * oldSpeed / newSpeed;
                m.SlidingSpeedZ = m.SlidingSpeedZ * oldSpeed / newSpeed;
            }

            update_sliding_angle(m, accel, lossFactor);

            if (!(mario_floor_is_slope(m) != 0) && m.HSpeed * m.HSpeed < stopSpeed * stopSpeed)
            {
                mario_set_forward_vel(m, 0.0f);
                stopped = 1;
            }

            return stopped;
        }

        public static void mario_set_forward_vel(SlidingMarioState m, float forwardVel)
        {
            m.HSpeed = forwardVel;

            m.SlidingSpeedX = InGameTrigUtilities.InGameSine(m.MarioAngle) * m.HSpeed;
            m.SlidingSpeedZ = InGameTrigUtilities.InGameCosine(m.MarioAngle) * m.HSpeed;

            m.XSpeed = (float) m.SlidingSpeedX;
            m.ZSpeed = (float) m.SlidingSpeedZ;
        }

        public static void update_sliding_angle(SlidingMarioState m, float accel, float lossFactor)
        {
            int newFacingDYaw;
            short facingDYaw;

            TriangleDataModel floor = m.Floor;
            short slopeAngle = MoreMath.NormalizeAngleShort(InGameTrigUtilities.InGameATan(floor.NormZ, floor.NormX));
            float steepness = (float)Math.Sqrt(floor.NormX * floor.NormX + floor.NormZ * floor.NormZ);
            float normalY = floor.NormY;

            m.SlidingSpeedX += accel * steepness * InGameTrigUtilities.InGameSine(slopeAngle);
            m.SlidingSpeedZ += accel * steepness * InGameTrigUtilities.InGameCosine(slopeAngle);

            m.SlidingSpeedX *= lossFactor;
            m.SlidingSpeedZ *= lossFactor;

            m.SlidingAngle = InGameTrigUtilities.InGameATan(m.SlidingSpeedZ, m.SlidingSpeedX);

            facingDYaw = MoreMath.NormalizeAngleShort(m.MarioAngle - m.SlidingAngle);
            newFacingDYaw = facingDYaw;

            if (newFacingDYaw > 0 && newFacingDYaw <= 0x4000)
            {
                if ((newFacingDYaw -= 0x200) < 0)
                {
                    newFacingDYaw = 0;
                }
            } else if (newFacingDYaw > -0x4000 && newFacingDYaw < 0)
            {
                if ((newFacingDYaw += 0x200) > 0)
                {
                    newFacingDYaw = 0;
                }
            } else if (newFacingDYaw > 0x4000 && newFacingDYaw < 0x8000)
            {
                if ((newFacingDYaw += 0x200) > 0x8000)
                {
                    newFacingDYaw = 0x8000;
                }
            } else if (newFacingDYaw > -0x8000 && newFacingDYaw < -0x4000)
            {
                if ((newFacingDYaw -= 0x200) < -0x8000)
                {
                    newFacingDYaw = -0x8000;
                }
            }

            m.MarioAngle = MoreMath.NormalizeAngleUshort(m.SlidingAngle + newFacingDYaw);

            m.XSpeed = m.SlidingSpeedX;
            m.YSpeed = 0.0f;
            m.ZSpeed = m.SlidingSpeedZ;

            //mario_update_moving_sand(m);
            //mario_update_windy_ground(m);

            m.HSpeed = (float)Math.Sqrt(m.SlidingSpeedX * m.SlidingSpeedX + m.SlidingSpeedZ * m.SlidingSpeedZ);
            if (m.HSpeed > 100.0f)
            {
                m.SlidingSpeedX = m.SlidingSpeedX * 100.0f / m.HSpeed;
                m.SlidingSpeedZ = m.SlidingSpeedZ * 100.0f / m.HSpeed;
            }

            if (newFacingDYaw < -0x4000 || newFacingDYaw > 0x4000) {
                m.HSpeed *= -1.0f;
            }
        }

        public static int mario_get_floor_class(SlidingMarioState m) {
            int floorClass;

            if ((m.TerrainType & TERRAIN_MASK) == TERRAIN_SLIDE) {
                floorClass = SURFACE_CLASS_VERY_SLIPPERY;
            } else {
                floorClass = SURFACE_CLASS_DEFAULT;
            }

            if (m.Floor != null) {
                switch ((ushort)m.Floor.SurfaceType) {
                    case SURFACE_NOT_SLIPPERY:
                    case SURFACE_HARD_NOT_SLIPPERY:
                    case SURFACE_SWITCH:
                        floorClass = SURFACE_CLASS_NOT_SLIPPERY;
                        break;

                    case SURFACE_SLIPPERY:
                    case SURFACE_NOISE_SLIPPERY:
                    case SURFACE_HARD_SLIPPERY:
                    case SURFACE_NO_CAM_COL_SLIPPERY:
                        floorClass = SURFACE_CLASS_SLIPPERY;
                        break;

                    case SURFACE_VERY_SLIPPERY:
                    case SURFACE_ICE:
                    case SURFACE_HARD_VERY_SLIPPERY:
                    case SURFACE_NOISE_VERY_SLIPPERY_73:
                    case SURFACE_NOISE_VERY_SLIPPERY_74:
                    case SURFACE_NOISE_VERY_SLIPPERY:
                    case SURFACE_NO_CAM_COL_VERY_SLIPPERY:
                        floorClass = SURFACE_CLASS_VERY_SLIPPERY;
                        break;
                }
            }

            if (m.Action == ACT_CRAWLING && m.Floor.NormY > 0.5f && floorClass == SURFACE_CLASS_DEFAULT) {
                floorClass = SURFACE_CLASS_NOT_SLIPPERY;
            }

            return floorClass;
        }

        public static int mario_floor_is_slope(SlidingMarioState m) {
            float normY;

            if ((m.TerrainType & TERRAIN_MASK) == TERRAIN_SLIDE
                && m.Floor.NormY < 0.9998477f) { // ~cos(1 deg)
                return 1;
            }

            switch (mario_get_floor_class(m)) {
                case SURFACE_VERY_SLIPPERY:
                    normY = 0.9961947f; // ~cos(5 deg)
                    break;

                case SURFACE_SLIPPERY:
                    normY = 0.9848077f; // ~cos(10 deg)
                    break;

                default:
                    normY = 0.9659258f; // ~cos(15 deg)
                    break;

                case SURFACE_NOT_SLIPPERY:
                    normY = 0.9396926f; // ~cos(20 deg)
                    break;
            }

            return m.Floor.NormY <= normY ? 1 : 0;
        }

        public static uint mario_floor_is_slippery(SlidingMarioState m)
        {
            float normY;

            if ((m.TerrainType & TERRAIN_MASK) == TERRAIN_SLIDE
                && m.Floor.NormY < 0.9998477f)
            {
                return 1;
            }

            switch (mario_get_floor_class(m))
            {
                case SURFACE_VERY_SLIPPERY:
                    normY = 0.9848077f; //~cos(10 deg)
                    break;

                case SURFACE_SLIPPERY:
                    normY = 0.9396926f; //~cos(20 deg)
                    break;

                default:
                    normY = 0.7880108f; //~cos(38 deg)
                    break;

                case SURFACE_NOT_SLIPPERY:
                    normY = 0.0f;
                    break;
            }

            return m.Floor.NormY <= normY ? (uint)1 : (uint)0;
        }
    }
}

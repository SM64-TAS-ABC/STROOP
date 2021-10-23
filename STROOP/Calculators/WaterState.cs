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
    public class WaterState
    {
        public int Index;
        public float Y;
        public float YSpeed;
        public float HSpeed;
        public ushort Yaw;
        public short YawVel;
        public short Pitch;

        public WaterState()
        {
            Index = 0;
            Y = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
            YSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YSpeedOffset);
            HSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
            Yaw = Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
            YawVel = Config.Stream.GetShort(MarioConfig.StructAddress + MarioConfig.YawVelocityOffset);
            Pitch = Config.Stream.GetShort(MarioConfig.StructAddress + MarioConfig.FacingPitchOffset);
        }

        public override string ToString()
        {
            return string.Format(
                //"[{0}] Y={1} YSpeed={2} HSpeed={3} Yaw={4} YawVel={5} Pitch={6}",
                "Y={1} YSpeed={2} HSpeed={3} Yaw={4} YawVel={5} Pitch={6}",
                Index, (double)Y, (double)YSpeed, (double)HSpeed, Yaw, YawVel, Pitch);
        }

        public void Update(Input input, int waterLevel)
        {
            act_water_punch(input, waterLevel);
            Index++;
        }

        public void act_water_punch(Input input, int waterLevel)
        {
            update_swimming_yaw(input);
            update_swimming_pitch(input);
            update_swimming_speed(waterLevel);
            perform_water_step(waterLevel);
        }

        public void update_swimming_yaw(Input input)
        {
            short targetYawVel = ParsingUtilities.ParseShort(-(10.0f * input.X));

            if (targetYawVel > 0)
            {
                if (YawVel < 0)
                {
                    YawVel += 0x40;
                    if (YawVel > 0x10)
                    {
                        YawVel = 0x10;
                    }
                }
                else
                {
                    YawVel = ParsingUtilities.ParseShort(approach_s32(YawVel, targetYawVel, 0x10, 0x20));
                }
            }
            else if (targetYawVel < 0)
            {
                if (YawVel > 0)
                {
                    YawVel -= 0x40;
                    if (YawVel < -0x10)
                    {
                        YawVel = -0x10;
                    }
                }
                else
                {
                    YawVel = ParsingUtilities.ParseShort(approach_s32(YawVel, targetYawVel, 0x20, 0x10));
                }
            }
            else
            {
                YawVel = ParsingUtilities.ParseShort(approach_s32(YawVel, 0, 0x40, 0x40));
            }

            Yaw = ParsingUtilities.ParseUShort(Yaw + YawVel);
        }

        public void update_swimming_pitch(Input input)
        {
            short targetPitch = ParsingUtilities.ParseShort(-(252.0f * input.Y));

            short pitchVel;
            if (Pitch < 0)
            {
                pitchVel = 0x100;
            }
            else
            {
                pitchVel = 0x200;
            }

            if (Pitch < targetPitch)
            {
                if ((Pitch += pitchVel) > targetPitch)
                {
                    Pitch = targetPitch;
                }
            }
            else if (Pitch > targetPitch)
            {
                if ((Pitch -= pitchVel) < targetPitch)
                {
                    Pitch = targetPitch;
                }
            }
        }

        public void perform_water_step(int waterLevel)
        {
            float nextPosY = Y + YSpeed;

            if (nextPosY > waterLevel - 80) {
                nextPosY = waterLevel - 80;
                YSpeed = 0.0f;
            }

            Y = nextPosY;
        }

        public void update_swimming_speed(int waterLevel)
        {
            float buoyancy = get_buoyancy(waterLevel);
            YSpeed = HSpeed * InGameTrigUtilities.InGameSine(Pitch) + buoyancy;
        }

        public int approach_s32(int current, int target, int inc, int dec)
        {
            if (current < target)
            {
                current += inc;
                if (current > target)
                {
                    current = target;
                }
            }
            else
            {
                current -= dec;
                if (current < target)
                {
                    current = target;
                }
            }
            return current;
        }

        public float get_buoyancy(int waterLevel) {
            float buoyancy = 0.0f;

            if (swimming_near_surface(waterLevel))
            {
                buoyancy = 1.25f;
            }

            return buoyancy;
        }

        public bool swimming_near_surface(int waterLevel) {
            return (waterLevel - 80) - Y < 400.0f;
        }
    }
}

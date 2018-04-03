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
    public static class MovementCalculator
    {
        /*
        def computeAirSpeed(mario): # 8026a554
            perpSpeed = 0
            if windyAirSpeed(mario):
                return
            if mario.action == MarioAction.longJump:
                maxSpeed = 48
            else:
                maxSpeed = 32
            mario.hspeed = approachHSpeed(mario.hspeed, 0, 0.35, 0.35)
            if mario.input[0]: # tilting analog stick
                mario.hspeed += (mario.analogTilt/32)*1.5*cosTable(mario.h_0x24 - mario.euler.yaw)
                perpSpeed = sinTable(mario.h_0x24 - mario.euler.yaw)*(mario.analogTilt/32)*10
            if mario.hspeed > maxSpeed:
                mario.hspeed -= 1
            if mario.hspeed < -16:
                mario.hspeed += 2
            mario.specialXSpeed = sinTable(mario.euler.yaw)*mario.hspeed
            mario.specialZSpeed = cosTable(mario.euler.yaw)*mario.hspeed
            mario.specialXSpeed += perpSpeed*sinTable(mario.euler.yaw + 0x4000)
            mario.specialZSpeed += perpSpeed*cosTable(mario.euler.yaw + 0x4000)
            mario.velocity.x = mario.specialXSpeed
            mario.velocity.z = mario.specialZSpeed

        def airMove(mario,a1): #80256940
            finalValue = 0
            mario.wallTri = None
            newPos = mario.pos #stored @ sp24
            for i in range(4):
                newPos += (mario.vel/4)
                value = checkAirQframe(mario,newPos,a1)
                if value != 0: # note that a 2 followed by 0s will give 2
                    finalValue = value
                if value  in [1,3,4,6]:
                    break
            if 0 <= mario.vel.y:
                mario.fallPeak = mario.pos.y
            sw(mario+0x14,fn8025167c(mario))
            if mario.action != 0x10880899:
                mario.determineGravity()
            applyWind(mario)
            mario.obj.pos = mario.pos.copy() #via copyVector
            setSignedShortVector(mario.obj+0x1a,0,short(mario+0x2e),0)
            return finalValue

        def approachHSpeed(speed,maxSpeed,a2,a3):   #8037A8B4
            # f12 current hspeed
            # f14 max hspeed
            # a2,a3 floats
            if speed < maxSpeed:
                return min(maxSpeed, speed+a2)
            else:
                return max(maxSpeed, speed-a3)
        */

        public static int AirMove()
        {
            float startX = 0;
            float startY = 0;
            float startZ = 0;
            float xSpeed = 0;
            float ySpeed = 0;
            float zSpeed = 0;


            int finalValue = 0;

            float newX = startX;
            float newY = startY;
            float newZ = startZ;

            for (int i = 0; i < 4; i++)
            {
                newX += xSpeed / 4;
                newY += ySpeed / 4;
                newZ += zSpeed / 4;
            }

            return finalValue;
        }

        public static void ComputeAirSpeed(float hSpeed, int xInput, int yInput)
        {
            float perpSpeed = 0;
            bool longJump = false;
            int facingAngle = 49152;
            int cameraAngle = 32768;


            int maxSpeed = longJump ? 48 : 32;
            int deltaFacingCamera = facingAngle - cameraAngle;
            float analogTilt = MoreMath.GetScaledEffectiveInputMagnitude(xInput, yInput, false);

            hSpeed = ApproachHSpeed(hSpeed, 0, 0.35f, 0.35f);
            if (xInput != 0 || yInput != 0)
            {
                hSpeed += (analogTilt / 32) * 1.5f * MoreMath.InGameCosine(deltaFacingCamera);
                perpSpeed = MoreMath.InGameSine(deltaFacingCamera) * (analogTilt / 32) * 10;
            }

            if (hSpeed > maxSpeed) hSpeed -= 1;
            if (hSpeed < -16) hSpeed += 2;

            float specialXSpeed = MoreMath.InGameSine(facingAngle) * hSpeed;
            float specialZSpeed = MoreMath.InGameCosine(facingAngle) * hSpeed;
            specialXSpeed += perpSpeed * MoreMath.InGameSine(facingAngle + 0x4000);
            specialZSpeed += perpSpeed * MoreMath.InGameCosine(facingAngle + 0x4000);
            float xSpeed = specialXSpeed;
            float zSpeed = specialZSpeed;
        }


        public static float ApproachHSpeed(float speed, float maxSpeed, float increase, float decrease)
        {
            if (speed < maxSpeed)
                return Math.Min(maxSpeed, speed + increase);
            else
                return Math.Max(maxSpeed, speed - decrease);
        }
    }
}

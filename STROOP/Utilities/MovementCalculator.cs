using STROOP.Managers;
using STROOP.Structs.Configurations;
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
    */


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.TTC
{
    /** A spinning triangle is the triangle platform that spins about
     *  a vertical axis (i.e. changes its yaw).
     *  It functions exactly as the cog does.
     */
    public class TTCSpinningTriangle : TTCObject
    {

        public int angle;
        public int currentAngularVelocity;
        public int targetAngularVelocity;

        public TTCSpinningTriangle(int startingAngle)
        {
            angle = startingAngle;
            currentAngularVelocity = 0;
            targetAngularVelocity = 0;
        }

        public override void update()
        {
            if (currentAngularVelocity > targetAngularVelocity) currentAngularVelocity -= 50;
            else if (currentAngularVelocity < targetAngularVelocity) currentAngularVelocity += 50;

            angle += currentAngularVelocity;
            angle = normalize(angle);

            if (currentAngularVelocity == targetAngularVelocity)
            {
                int magnitude = (pollRNG() % 7) * 200; // = 0, 200, 400, 600, 800, 1000, 1200
                int sign = (pollRNG() <= 32766) ? -1 : 1; // = -1, 1
                targetAngularVelocity = magnitude * sign; // = -1200, -1000, ... , 1000, 1200
            }
        }

        public override string ToString()
        {
            return id + OPENER + angle + SEPARATOR +
                    currentAngularVelocity + SEPARATOR +
                    targetAngularVelocity + CLOSER;
        }

    }


}

using STROOP.Forms;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Utilities
{
    public static class InGameTrigUtilities
    {
        private static readonly List<float> sineData;
        private static readonly List<ushort> arcSineData;

        static InGameTrigUtilities()
        {
            sineData = XmlConfigParser.OpenSineData();
            arcSineData = XmlConfigParser.OpenArcSineData();
        }

        public static float InGameSine(int value)
        {
            int truncatedValue = MoreMath.NormalizeAngleTruncated(value);
            int index = truncatedValue / 16;
            return sineData[index];
        }

        public static float InGameCosine(int value)
        {
            int truncatedValue = MoreMath.NormalizeAngleTruncated(value);
            int index = truncatedValue / 16;
            return sineData[index + 1024];
        }

        public static ushort InGameAngleTo(float xTo, float zTo)
        {
            return InGameAngleTo(0, 0, xTo, zTo);
        }

        public static ushort InGameAngleTo(float xFrom, float zFrom, float xTo, float zTo)
        {
            float xDiff = xTo - xFrom;
            float zDiff = zTo - zFrom;
            return InGameATan(zDiff, xDiff);
        }

        public static ushort InGameATan(float xComp, float yComp)
        {
            int returnValue;
            if (0 <= yComp)
                if (0 <= xComp)
                    if (yComp <= xComp)
                        returnValue = InGameATan45Degrees(yComp, xComp);
                    else
                        returnValue = 0x4000 - InGameATan45Degrees(xComp, yComp);
                else
                    if (-xComp < yComp)
                    returnValue = 0x4000 + InGameATan45Degrees(-xComp, yComp);
                else
                    returnValue = 0x8000 - InGameATan45Degrees(yComp, -xComp);
            else
                if (xComp < 0)
                if (-yComp < -xComp)
                    returnValue = 0x8000 + InGameATan45Degrees(-yComp, -xComp);
                else
                    returnValue = 0xC000 - InGameATan45Degrees(-xComp, -yComp);
            else
                    if (xComp < -yComp)
                returnValue = 0xC000 + InGameATan45Degrees(xComp, -yComp);
            else
                returnValue = 0x10000 - InGameATan45Degrees(-yComp, xComp);

            return (ushort)returnValue;
        }

        private static ushort InGameATan45Degrees(float yComp, float xComp)
        {
            //  if f14 == 0:
            //     return short(0x8038b000)
            //  else:
            //      return short(0x8038b000+2*int( (f12/f14)*1024.0 + 0.5)) # with +0.5 this is normal rounding, not towards zero

            uint offset;
            if (xComp == 0)
                offset = 0;
            else
                offset = 2 * (uint)((yComp / xComp) * 1024f + 0.5f);

            //return Config.Stream.GetUInt16(0x8038B000 + offset);
            int index = (int)(offset / 2);
            return arcSineData[index];
        }


    }
} 

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
    public static class TtcSpeedSettingUtilities
    {

        public static string GetTtcSpeedSettingDescription(short? ttcSpeedSettingNullable = null)
        {
            short ttcSpeedSetting = ttcSpeedSettingNullable ?? Config.Stream.GetInt16(MiscConfig.TtcSpeedSettingAddress);
            switch (ttcSpeedSetting)
            {
                case 0:
                    return "Slow";
                case 1:
                    return "Fast";
                case 2:
                    return "Random";
                case 3:
                    return "Still";
                default:
                    return "Unknown";
            }
        }

        public static short? GetTtcSpeedSetting(string description)
        {
            if (description == null) return null;
            switch (description.ToLower())
            {
                case "slow":
                case "3":
                    return 0;
                case "fast":
                case "9":
                    return 1;
                case "random":
                case "6":
                    return 2;
                case "still":
                case "frozen":
                case "12":
                    return 3;
            }
            return null;
        }

        public static List<string> GetDescriptions()
        {
            return new List<string>()
            {
                "Slow",
                "Fast",
                "Random",
                "Still",
            };
        }

    }
}

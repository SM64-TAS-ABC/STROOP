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
    public static class DemoCounterUtilities
    {

        public static string GetDemoCounterDescription(short? demoCounterNullable = null)
        {
            switch (RomVersionConfig.Version)
            {
                case RomVersion.US:
                    return GetDemoCounterDescriptionUS(demoCounterNullable);
                case RomVersion.JP:
                    return GetDemoCounterDescriptionJP(demoCounterNullable);
                case RomVersion.PAL:
                    return "Unknown";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static string GetDemoCounterDescriptionUS(short? demoCounterNullable = null)
        {
            short demoCounter = demoCounterNullable ?? Config.Stream.GetInt16(MiscConfig.DemoCounterAddress);
            switch (demoCounter)
            {
                case 0:
                    return "Bowser 1";
                case 1:
                    return "WF";
                case 2:
                    return "CCM";
                case 3:
                    return "BBH";
                case 4:
                    return "JRB";
                case 5:
                    return "HMC";
                case 6:
                    return "PSS";
                default:
                    return "Unknown";
            }
        }

        public static string GetDemoCounterDescriptionJP(short? demoCounterNullable = null)
        {
            short demoCounter = demoCounterNullable ?? Config.Stream.GetInt16(MiscConfig.DemoCounterAddress);
            switch (demoCounter)
            {
                case 0:
                    return "WF";
                case 1:
                    return "CCM";
                case 2:
                    return "BBH";
                case 3:
                    return "JRB";
                case 4:
                    return "HMC";
                case 5:
                    return "PSS";
                default:
                    return "Unknown";
            }
        }

        public static short? GetDemoCounter(string description)
        {
            switch (RomVersionConfig.Version)
            {
                case RomVersion.US:
                    return GetDemoCounterUS(description);
                case RomVersion.JP:
                    return GetDemoCounterJP(description);
                case RomVersion.PAL:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static short? GetDemoCounterUS(string description)
        {
            if (description == null) return null;
            switch (description.ToLower())
            {
                case "bitdw":
                case "bowser1":
                case "bowser 1":
                    return 0;
                case "wf":
                    return 1;
                case "ccm":
                    return 2;
                case "bbh":
                    return 3;
                case "jrb":
                    return 4;
                case "hmc":
                    return 5;
                case "pss":
                    return 6;
                default:
                    return null;
            }
        }

        public static short? GetDemoCounterJP(string description)
        {
            if (description == null) return null;
            switch (description.ToLower())
            {
                case "wf":
                    return 0;
                case "ccm":
                    return 1;
                case "bbh":
                    return 2;
                case "jrb":
                    return 3;
                case "hmc":
                    return 4;
                case "pss":
                    return 5;
                default:
                    return null;
            }
        }

        public static List<string> GetDescriptions()
        {
            return new List<string>()
            {
                "Bowser 1",
                "WF",
                "CCM",
                "BBH",
                "JRB",
                "HMC",
                "PSS",
            };
        }

    }
}

using STROOP.Managers;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;

namespace STROOP.Structs
{
    public static class VarHackSpecialUtilities
    {
        private readonly static Func<string> DEFAULT_GETTER = () => "NOT IMPL";

        public static Func<string> CreateGetterFunction(string specialType)
        {
            Func<string> getterFunction = DEFAULT_GETTER;

            switch (specialType)
            {
                case "RngIndex":
                    getterFunction = () =>
                    {
                        return "Index " + RngIndexer.GetRngIndex();
                    };
                    break;

                case "FloorYNorm":
                    getterFunction = () =>
                    {
                        uint triFloorAddress = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
                        float yNorm = Config.Stream.GetSingle(triFloorAddress + TriangleOffsetsConfig.NormY);
                        return "YNorm " + FormatDouble(yNorm, 4, true);
                    };
                    break;

                case "DefactoSpeed":
                    getterFunction = () =>
                    {
                        return "Defacto " + FormatInteger(WatchVariableSpecialUtilities.GetMarioDeFactoSpeed());
                    };
                    break;

                case "MarioAction":
                    getterFunction = () =>
                    {
                        return "Action " + TableConfig.MarioActions.GetActionName();
                    };
                    break;

                case "MarioAnimation":
                    getterFunction = () =>
                    {
                        return "Animation " + TableConfig.MarioAnimations.GetAnimationName();
                    };
                    break;

                case "DYawIntendFacing":
                    getterFunction = () =>
                    {
                        return "DYaw " + FormatInteger(WatchVariableSpecialUtilities.GetDeltaYawIntendedFacing());
                    };
                    break;

                case "DYawIntendFacingHau":
                    getterFunction = () =>
                    {
                        return "DYaw " + FormatInteger(WatchVariableSpecialUtilities.GetDeltaYawIntendedFacing() / 16);
                    };
                    break;

                default:
                    break;
            }

            return getterFunction;
        }

        private static string FormatDouble(double value, int numDigits = 4, bool usePadding = true)
        {
            string stringValue = Math.Round(value, numDigits).ToString();
            if (usePadding)
            {
                int decimalIndex = stringValue.IndexOf(".");
                if (decimalIndex == -1)
                {
                    stringValue += ".";
                    decimalIndex = stringValue.Length - 1;
                }
                while (stringValue.Length <= decimalIndex + numDigits)
                {
                    stringValue += "0";
                }
            }
            stringValue = stringValue.Replace(".", VarHackConfig.CoinChar);
            return stringValue;
        }

        private static string FormatInteger(double value)
        {
            string stringValue = Math.Truncate(value).ToString();
            stringValue = stringValue.Replace("-", "M");
            return stringValue;
        }
    }
}
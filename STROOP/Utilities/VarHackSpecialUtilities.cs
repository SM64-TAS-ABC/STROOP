using STROOP.Managers;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;

namespace STROOP.Structs
{
    public static class VarHackSpecialUtilities
    {
        private readonly static string DEFAULT_NAME = "NOT IMPL";
        private readonly static Func<string> DEFAULT_GETTER = () => "";

        public static (string, Func<string>) CreateGetterFunction(string specialType)
        {
            string name = DEFAULT_NAME;
            Func<string> getterFunction = DEFAULT_GETTER;

            switch (specialType)
            {
                case "RngIndex":
                    name = "Index " + VarHackConfig.EscapeChar;
                    getterFunction = () =>
                    {
                        return RngIndexer.GetRngIndex().ToString();
                    };
                    break;

                case "FloorYNorm":
                    name = "YNorm " + VarHackConfig.EscapeChar;
                    getterFunction = () =>
                    {
                        uint triFloorAddress = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
                        float yNorm = Config.Stream.GetSingle(triFloorAddress + TriangleOffsetsConfig.NormY);
                        return FormatDouble(yNorm, 4, true);
                    };
                    break;

                case "DefactoSpeed":
                    name = "Defacto " + VarHackConfig.EscapeChar;
                    getterFunction = () =>
                    {
                        return FormatInteger(WatchVariableSpecialUtilities.GetMarioDeFactoSpeed());
                    };
                    break;

                case "SlidingSpeed":
                    name = "Spd " + VarHackConfig.EscapeChar;
                    getterFunction = () =>
                    {
                        return FormatInteger(WatchVariableSpecialUtilities.GetMarioSlidingSpeed());
                    };
                    break;

                case "MarioAction":
                    name = "Action " + VarHackConfig.EscapeChar;
                    getterFunction = () =>
                    {
                        return TableConfig.MarioActions.GetActionName();
                    };
                    break;

                case "MarioAnimation":
                    name = "Animation " + VarHackConfig.EscapeChar;
                    getterFunction = () =>
                    {
                        return TableConfig.MarioAnimations.GetAnimationName();
                    };
                    break;

                case "DYawIntendFacing":
                    name = "DYaw " + VarHackConfig.EscapeChar;
                    getterFunction = () =>
                    {
                        return FormatInteger(WatchVariableSpecialUtilities.GetDeltaYawIntendedFacing());
                    };
                    break;

                case "DYawIntendFacingHau":
                    name = "DYaw " + VarHackConfig.EscapeChar;
                    getterFunction = () =>
                    {
                        return FormatInteger(WatchVariableSpecialUtilities.GetDeltaYawIntendedFacing() / 16);
                    };
                    break;

                default:
                    break;
            }

            return (name, getterFunction);
        }

        private static string FormatDouble(double value, int numDigits, bool usePadding)
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
            stringValue = stringValue.Replace("-", "M");
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
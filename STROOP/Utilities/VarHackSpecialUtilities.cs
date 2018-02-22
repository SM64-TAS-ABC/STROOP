using STROOP.Managers;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;

namespace STROOP.Structs
{
    public static class VarHackSpecialUtilities
    {
        private readonly static Func<uint, string> DEFAULT_GETTER = (uint address) => "NOT IMPL";
        private readonly static Func<string, uint, bool> DEFAULT_SETTER = (string value, uint address) => false;

        public static (Func<uint, string> getter, Func<string, uint, bool> setter) CreateGetterSetterFunctions(string specialType)
        {
            Func<uint, string> getterFunction = DEFAULT_GETTER;
            Func<string, uint, bool> setterFunction = DEFAULT_SETTER;

            switch (specialType)
            {
                case "MarioDistanceToObject":
                    getterFunction = (uint objAddress) =>
                    {
                        return "";
                    };
                    setterFunction = (string stringValue, uint objAddress) =>
                    {
                        return false;
                    };
                    break;

                default:
                    break;
            }

            return (getterFunction, setterFunction);
        }
    }
}
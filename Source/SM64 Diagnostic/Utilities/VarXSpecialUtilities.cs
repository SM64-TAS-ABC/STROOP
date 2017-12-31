using SM64_Diagnostic.Managers;
using SM64_Diagnostic.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public static class VarXSpecialUtilities
    {
        public readonly static Func<List<object>> DEFAULT_GETTER = () => new List<object>() { "UNIMPLEMENTED" };
        public readonly static Action<string> DEFAULT_SETTER = (string stringValue) => { };

        public static (Func<List<object>> getter, Action<string> setter) CreateGetterSetterFunctions(string specialType)
        {
            switch(specialType)
            {
                case "MarioDistanceToObject":
                    {
                        return (DEFAULT_GETTER, DEFAULT_SETTER);
                    }
                case "MarioHorizontalDistanceToObject":
                    {
                        return (DEFAULT_GETTER, DEFAULT_SETTER);
                    }
                case "MarioVerticalDistanceToObject":
                    {
                        return (DEFAULT_GETTER, DEFAULT_SETTER);
                    }
                case "RngIndex":
                    {
                        return (DEFAULT_GETTER, DEFAULT_SETTER);
                    }
                default:
                    {
                        return (DEFAULT_GETTER, DEFAULT_SETTER);
                    }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs.Configurations
{
    public static class GotoRetrieveConfig
    {
        public static readonly float GotoAboveDefault = 300;
        public static readonly float GotoInfrontDefault = 0;
        public static readonly float RetrieveAboveDefault = 300;
        public static readonly float RetrieveInfrontDefault = 0;

        public static float GotoAboveOffset = GotoAboveDefault;
        public static float GotoInfrontOffset = GotoInfrontDefault;
        public static float RetrieveAboveOffset = RetrieveAboveDefault;
        public static float RetrieveInfrontOffset = RetrieveInfrontDefault;
    }
}

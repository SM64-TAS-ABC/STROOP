using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Utilities;

namespace SM64_Diagnostic.Extensions
{
    public static class WatchExpressionExtensions
    {
        public static string Evaluate(this WatchExpression watchExp, ProcessStream stream)
        {
            if (watchExp.Error != null)
                return watchExp.Error;

            return "";
        }
    }
}

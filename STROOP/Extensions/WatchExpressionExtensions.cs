using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;
using STROOP.Utilities;

namespace STROOP.Extensions
{
    public static class WatchExpressionExtensions
    {
        public static string Evaluate(this WatchExpression watchExp)
        {
            if (watchExp.Error != null)
                return watchExp.Error;

            return "";
        }
    }
}

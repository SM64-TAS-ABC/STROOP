using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public class WatchExpression
    {
        public string UnProcessedExpression;
        public string PreProcessedExpression;
        public bool Active = true;
        public string Error = null;
    }
}

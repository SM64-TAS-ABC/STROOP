using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public struct DropAction
    {
        public enum ActionType { Mario, Object };
        public ActionType Action;
        public uint Address;

        public DropAction(ActionType actionType, uint address)
        {
            Action = actionType;
            Address = address;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public class ActionTable
    {
        public struct ActionReference
        {
            public uint Action;
            public string ActionName;
            public uint? AfterClone;
            public uint? AfterUnclone;
            public uint? Handsfree;

            public override int GetHashCode()
            {
                return (int)Action;
            }
        }

        Dictionary<uint, ActionReference> _table = new Dictionary<uint, ActionReference>();

        uint _defaultAfterClone;
        uint _defaultAfterUnclone;
        uint _defaultHandsfree;

        public ActionTable(uint defaultAfterClone, uint defaultAfterUnclone, uint defaultHandsfree)
        {
            _defaultAfterClone = defaultAfterClone;
            _defaultAfterUnclone = defaultAfterUnclone;
            _defaultHandsfree = defaultHandsfree;
        }

        public void Add(ActionReference actionRef)
        {
            // Check for default afterCloneValue
            if (!actionRef.AfterClone.HasValue)
                actionRef.AfterClone = _defaultAfterClone;

            // Check for default afterUncloneValue
            if (!actionRef.AfterUnclone.HasValue)
                actionRef.AfterUnclone = _defaultAfterUnclone;

            // Check for default handsfreeValue
            if (!actionRef.Handsfree.HasValue)
                actionRef.Handsfree = _defaultHandsfree;

            // Add action to table
            _table.Add(actionRef.Action, actionRef);
        }

        public string GetActionName(uint action)
        {
            if (!_table.ContainsKey(action))
                return "Unknown Action";

            return _table[action].ActionName;
        }

        public uint GetAfterCloneValue(uint action)
        {
            if (!_table.ContainsKey(action))
                return _defaultAfterClone;

            return _table[action].AfterClone.Value;
        }

        public uint GetAfterUncloneValue(uint action)
        {
            if (!_table.ContainsKey(action))
                return _defaultAfterUnclone;

            return _table[action].AfterUnclone.Value;
        }

        public uint GetHandsfreeValue(uint action)
        {
            if (!_table.ContainsKey(action))
                return _defaultHandsfree;

            return _table[action].Handsfree.Value;
        }
    }
}

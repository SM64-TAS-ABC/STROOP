using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public class ActionTable
    {
        Dictionary<uint, Tuple<string, uint, uint>> _table = new Dictionary<uint, Tuple<string, uint, uint>>();

        uint _defaultAfterClone;
        uint _defaultAfterUnclone;

        public ActionTable(uint defaultAfterClone, uint defaultAfterUnclone)
        {
            _defaultAfterClone = defaultAfterClone;
            _defaultAfterUnclone = defaultAfterUnclone;
        }

        public void Add(Tuple<uint, string, uint?, uint?> action)
        {
            // Check for default afterCloneValue
            if (!action.Item3.HasValue)
                action = new Tuple<uint, string, uint?, uint?>(action.Item1,
                    action.Item2, _defaultAfterClone, action.Item4);

            // Check for default afterUncloneValue
            if (!action.Item4.HasValue)
               action = new Tuple<uint, string, uint?, uint?>(action.Item1,
                    action.Item2, action.Item3, _defaultAfterUnclone);

            // Add action to table
            _table.Add(action.Item1, new Tuple<string, uint, uint>(action.Item2, 
                action.Item3.Value, action.Item4.Value));
        }

        public string GetActionName(uint action)
        {
            if (!_table.Keys.Contains(action))
                return "Unknown Action";

            return _table[action].Item1;
        }

        public uint GetAfterCloneValue(uint action)
        {
            if (!_table.Keys.Contains(action))
                return _defaultAfterClone;

            return _table[action].Item2;
        }

        public uint GetAfterUncloneValue(uint action)
        {
            if (!_table.Keys.Contains(action))
                return _defaultAfterUnclone;

            return _table[action].Item3;
        }
    }
}

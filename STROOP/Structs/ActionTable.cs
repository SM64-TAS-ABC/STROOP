using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
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

        Dictionary<uint, ActionReference> _actionTable = new Dictionary<uint, ActionReference>();
        Dictionary<string, ActionReference> _actionNameTable = new Dictionary<string, ActionReference>();

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
            _actionTable.Add(actionRef.Action, actionRef);
            _actionNameTable.Add(actionRef.ActionName, actionRef);
        }

        public List<string> GetActionNameList()
        {
            List<string> actionNameList = _actionTable.Keys.ToList().ConvertAll(
                action => _actionTable[action].ActionName);
            actionNameList.Sort();
            return actionNameList;
        }

        public uint? GetActionFromName(string actionName)
        {
            if (!_actionNameTable.ContainsKey(actionName))
                return null;
            return _actionNameTable[actionName].Action;
        }

        public string GetActionName()
        {
            uint currentAction = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.ActionOffset);
            return GetActionName(currentAction);
        }

        public string GetPrevActionName()
        {
            uint prevAction = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.PrevActionOffset);
            return GetActionName(prevAction);
        }

        public string GetActionName(uint action)
        {
            if (!_actionTable.ContainsKey(action))
                return "Unknown Action";
            return _actionTable[action].ActionName;
        }

        public uint GetAfterCloneValue(uint action)
        {
            if (!_actionTable.ContainsKey(action))
                return _defaultAfterClone;
            return _actionTable[action].AfterClone.Value;
        }

        public uint GetAfterUncloneValue(uint action)
        {
            if (!_actionTable.ContainsKey(action))
                return _defaultAfterUnclone;
            return _actionTable[action].AfterUnclone.Value;
        }

        public uint GetHandsfreeValue(uint action)
        {
            if (!_actionTable.ContainsKey(action))
                return _defaultHandsfree;
            return _actionTable[action].Handsfree.Value;
        }

        public string GetGroupName(uint? actionNullable = null)
        {
            uint action = actionNullable ?? Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.ActionOffset);
            uint actionGroup = action & 0x000001C0;
            switch (actionGroup)
            {
                case 0x000:
                    return "Stationary";
                case 0x040:
                    return "Moving";
                case 0x080:
                    return "Airborne";
                case 0x0C0:
                    return "Submerged";
                case 0x100:
                    return "Cutscene";
                case 0x140:
                    return "Automatic";
                case 0x180:
                    return "Object";
                default:
                    return "Unknown Group";
            }
        }
    }
}

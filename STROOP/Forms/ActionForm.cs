using STROOP.Models;
using STROOP.Structs;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Forms
{
    public partial class ActionForm : Form
    {
        public ActionForm()
        {
            InitializeComponent();

            List<uint> actions = TableConfig.MarioActions.GetActionList();
            foreach (uint action in actions)
            {
                List<object> paramList = GetRowParams(action);
                dataGridViewActions.Rows.Add(paramList.ToArray());
            }
        }

        public List<object> GetRowParams(uint action)
        {
            string name = TableConfig.MarioActions.GetActionName(action);
            ushort group = TableConfig.MarioActions.GetGroup(action);
            string groupName = TableConfig.MarioActions.GetGroupName(action);
            ushort id = TableConfig.MarioActions.GetId(action);
            List<object> actionBits = Enumerable.Range(9, 23).ToList()
                .ConvertAll(bit => (object)GetBit(action, bit));

            List<object> paramList = new List<object>();
            paramList.Add(name);
            paramList.Add(HexUtilities.FormatValue(action, 8));
            paramList.Add(HexUtilities.FormatValue(group, 3));
            paramList.Add(groupName);
            paramList.Add(HexUtilities.FormatValue(id, 3));
            paramList.AddRange(actionBits);
            return paramList;
        }

        private bool GetBit(uint action, int bit)
        {
            long value = action & (1 << bit);
            return value != 0;
        }
    }
}

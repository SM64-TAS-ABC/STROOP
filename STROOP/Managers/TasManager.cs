using STROOP.Controls;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STROOP.Managers
{
    public class TasManager : DataManager
    {
        private DataGridView _dataGridViewTas;
        private Dictionary<uint, DataGridViewRow> _rowDictionary;

        private static readonly int TABLE_INDEX_GLOBAL_TIMER = 0;
        private static readonly int TABLE_INDEX_GOAL_ANGLE = 1;
        private static readonly int TABLE_INDEX_CURRENT_CAM_ANGLE = 2;
        private static readonly int TABLE_INDEX_NEXT_CAM_ANGLE = 3;
        private static readonly int TABLE_INDEX_X_INPUT = 4;
        private static readonly int TABLE_INDEX_Y_INPUT = 5;

        public TasManager(List<WatchVariableControlPrecursor> variables, TabPage tabControl, WatchVariableFlowLayoutPanel watchVariablePanel)
            : base(variables, watchVariablePanel)
        {
            SplitContainer splitContainerTas = tabControl.Controls["splitContainerTas"] as SplitContainer;
            SplitContainer splitContainerTasTable = splitContainerTas.Panel1.Controls["splitContainerTasTable"] as SplitContainer;
            _dataGridViewTas = splitContainerTasTable.Panel2.Controls["dataGridViewTas"] as DataGridView;

            _rowDictionary = new Dictionary<uint, DataGridViewRow>();
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;
            base.Update(updateView);

            uint currentGlobalTimer = Config.Stream.GetUInt32(MiscConfig.GlobalTimerAddress);

            if (!_rowDictionary.ContainsKey(currentGlobalTimer))
            {
                ushort marioFacingYaw = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                ushort cameraAngle = Config.Stream.GetUInt16(CameraConfig.CameraStructAddress + CameraConfig.CentripetalAngleOffset);
                _dataGridViewTas.Rows.Add(currentGlobalTimer, marioFacingYaw, cameraAngle, "", "", "");
                DataGridViewRow lastRow = _dataGridViewTas.Rows[_dataGridViewTas.RowCount - 1];
                _rowDictionary.Add(currentGlobalTimer, lastRow);
            }

            DataGridViewRow currentRow = _rowDictionary[currentGlobalTimer];
            currentRow.Selected = true;
        }
    }
}

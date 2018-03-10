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
        private CheckBox _checkBoxTasRecordData;
        private Button _buttonTasClearData;

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
            _checkBoxTasRecordData = splitContainerTasTable.Panel1.Controls["checkBoxTasRecordData"] as CheckBox;
            _buttonTasClearData = splitContainerTasTable.Panel1.Controls["buttonTasClearData"] as Button;
            _buttonTasClearData.Click += (sender, e) => ClearData();

            _rowDictionary = new Dictionary<uint, DataGridViewRow>();
        }

        private void ClearData()
        {
            _dataGridViewTas.Rows.Clear();
            _rowDictionary.Clear();
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;
            base.Update(updateView);

            if (!_checkBoxTasRecordData.Checked) return;
            uint currentGlobalTimer = Config.Stream.GetUInt32(MiscConfig.GlobalTimerAddress);
            if (!_rowDictionary.ContainsKey(currentGlobalTimer))
            {
                ushort marioFacingYaw = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                ushort cameraAngle = Config.Stream.GetUInt16(CameraConfig.CameraStructAddress + CameraConfig.CentripetalAngleOffset);
                _dataGridViewTas.Rows.Add(currentGlobalTimer, marioFacingYaw, cameraAngle, "", "", "");
                DataGridViewRow lastRow = _dataGridViewTas.Rows[_dataGridViewTas.RowCount - 1];
                lastRow.Selected = true;
                _rowDictionary.Add(currentGlobalTimer, lastRow);
            }
            else
            {
                DataGridViewRow currentRow = _rowDictionary[currentGlobalTimer];
                currentRow.Selected = true;
                uint nextGlobalTimer = currentGlobalTimer + 1;
                if (_rowDictionary.ContainsKey(nextGlobalTimer))
                {
                    DataGridViewRow nextRow = _rowDictionary[nextGlobalTimer];
                    ushort nextCameraAngle = ParsingUtilities.ParseUShort(nextRow.Cells[TABLE_INDEX_CURRENT_CAM_ANGLE].Value);
                    ushort goalAngle = ParsingUtilities.ParseUShort(currentRow.Cells[TABLE_INDEX_GOAL_ANGLE].Value);
                    (int xInput, int yInput) = MoreMath.CalculateInputsFromAngle(goalAngle, nextCameraAngle);
                    currentRow.Cells[TABLE_INDEX_NEXT_CAM_ANGLE].Value = nextCameraAngle;
                    currentRow.Cells[TABLE_INDEX_X_INPUT].Value = xInput;
                    currentRow.Cells[TABLE_INDEX_Y_INPUT].Value = yInput;
                }
            }
        }
    }
}

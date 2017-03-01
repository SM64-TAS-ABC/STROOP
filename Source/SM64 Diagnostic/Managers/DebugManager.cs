using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Utilities;
using System.Windows.Forms;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic.Managers
{
    public class DebugManager
    {
        ProcessStream _stream;
        bool _changedByUser = true;
        CheckBox _spawnDebugCheckbox, _classicCheckbox, _resourceCheckbox, _stageSelectCheckbox;
        RadioButton[] _dbgSettingRadioButton;
        RadioButton _dbgSettingRadioButtonOff;

        public DebugManager(ProcessStream stream, Control tabControl)
        {
            _stream = stream;

            var panel = tabControl.Controls["NoTearFlowLayoutPanelDebugDisplayType"];

            var freeMovementButton = tabControl.Controls["buttonDbgFreeMovement"] as Button;
            _spawnDebugCheckbox = tabControl.Controls["checkBoxDbgSpawnDbg"] as CheckBox;
            _classicCheckbox = tabControl.Controls["checkBoxDbgClassicDbg"] as CheckBox;
            _resourceCheckbox = tabControl.Controls["checkBoxDbgResource"] as CheckBox;
            _stageSelectCheckbox = tabControl.Controls["checkBoxDbgStageSelect"] as CheckBox;

            _spawnDebugCheckbox.CheckedChanged += SpawnDebugCheckbox_CheckedChanged;
            _classicCheckbox.CheckedChanged += _classicCheckbox_CheckedChanged;
            _resourceCheckbox.CheckedChanged += _resourceCheckbox_CheckedChanged;
            _stageSelectCheckbox.CheckedChanged += _stageSelectCheckbox_CheckedChanged;
            freeMovementButton.Click += FreeMovementButton_Click;

            _dbgSettingRadioButtonOff = panel.Controls["radioButtonDbgOff"] as RadioButton;
            _dbgSettingRadioButton = new RadioButton[6];
            _dbgSettingRadioButton[0] = panel.Controls["radioButtonDbgObjCnt"] as RadioButton;
            _dbgSettingRadioButton[1] = panel.Controls["radioButtonDbgChkInfo"] as RadioButton;
            _dbgSettingRadioButton[2] = panel.Controls["radioButtonDbgMapInfo"] as RadioButton;
            _dbgSettingRadioButton[3] = panel.Controls["radioButtonDbgStgInfo"] as RadioButton;
            _dbgSettingRadioButton[4] = panel.Controls["radioButtonDbgFxInfo"] as RadioButton;
            _dbgSettingRadioButton[5] = panel.Controls["radioButtonDbgEnemyInfo"] as RadioButton;

            _dbgSettingRadioButtonOff.Click += radioButtonDbgOff_CheckedChanged;
            for (int i = 0; i < _dbgSettingRadioButton.Length; i++)
            {
                byte localI = (byte)i;
                _dbgSettingRadioButton[i].Click += (sender, e) => radioButtonDbgSetting_CheckChanged(sender, e, localI);
            }

        }

        private void radioButtonDbgSetting_CheckChanged(object sender, EventArgs e, byte settingValue)
        {
            if (!_changedByUser)
                return;

            // Turn debug on
            _stream.SetValue((byte)1, Config.Debug.AdvancedMode);

            // Set mode
            _stream.SetValue(settingValue, Config.Debug.Setting);
        }

        private void SpawnDebugCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!_changedByUser)
                return;

            _stream.SetValue(_spawnDebugCheckbox.Checked ? (byte)0x03 : (byte)0x00, Config.Debug.Setting);
            _stream.SetValue(_spawnDebugCheckbox.Checked ? (byte)0x01 : (byte)0x00, Config.Debug.SpawnMode);
        }

        private void _classicCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!_changedByUser)
                return;

            _stream.SetValue(_classicCheckbox.Checked ? (byte)0x01 : (byte)0x00, Config.Debug.ClassicMode);
        }

        private void _resourceCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!_changedByUser)
                return;

            _stream.SetValue(_resourceCheckbox.Checked ? (byte)0x01 : (byte)0x00, Config.Debug.ResourceMode);
        }

        private void _stageSelectCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!_changedByUser)
                return;

            _stream.SetValue(_stageSelectCheckbox.Checked ? (byte)0x01 : (byte)0x00, Config.Debug.StageSelect);
        }

        private void FreeMovementButton_Click(object sender, EventArgs e)
        {
            if (!_changedByUser)
                return;

            _stream.SetValue(Config.Debug.FreeMovementValue, Config.Debug.FreeMovementAddress);
        }

        private void radioButtonDbgOff_CheckedChanged(object sender, EventArgs e)
        {
            if (!_changedByUser)
                return;

            // Turn debug off
            _stream.SetValue((byte)0, Config.Debug.AdvancedMode);

            // Set mode
            _stream.SetValue((byte)0, Config.Debug.Setting);
        }

        private void radioButtonDbgFxInfo_CheckedChanged(object sender, EventArgs e)
        {
            if (!_changedByUser)
                return;

            // Turn debug on
            _stream.SetValue((byte)1, Config.Debug.AdvancedMode);

            // Set mode
            _stream.SetValue((byte)4, Config.Debug.Setting);
        }

        private void radioButtonDbgEnemyInfo_CheckedChanged(object sender, EventArgs e)
        {
            if (!_changedByUser)
                return;

            // Turn debug on
            _stream.SetValue((byte)1, Config.Debug.AdvancedMode);

            // Set mode
            _stream.SetValue((byte)5, Config.Debug.Setting);
        }

        void SetChecked(CheckBox checkBox, bool value)
        {
            if (checkBox.Checked != value)
                checkBox.Checked = value;
        }

        public void Update(bool updateView = false)
        {
            if (!updateView)
                return;

            _changedByUser = false;

            SetChecked(_spawnDebugCheckbox, _stream.GetByte(Config.Debug.Setting) == 0x03
                 && _stream.GetByte(Config.Debug.SpawnMode) == 0x01);
            SetChecked(_classicCheckbox, _stream.GetByte(Config.Debug.ClassicMode) == 0x01);
            SetChecked(_resourceCheckbox, _stream.GetByte(Config.Debug.ResourceMode) == 0x01);
            SetChecked(_stageSelectCheckbox, _stream.GetByte(Config.Debug.StageSelect) == 0x01);

            var setting = _stream.GetByte(Config.Debug.Setting);
            var on = _stream.GetByte(Config.Debug.AdvancedMode);
            if (on == 0x01 && setting <= _dbgSettingRadioButton.Length)
                _dbgSettingRadioButton[setting].Checked = true;
            else
                _dbgSettingRadioButtonOff.Checked = true;

            _changedByUser = true;
        }
    }
}

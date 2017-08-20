using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Utilities;
using System.Windows.Forms;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Structs.Configurations;
using SM64_Diagnostic.Controls;

namespace SM64_Diagnostic.Managers
{
    public class DebugManager : DataManager
    {
        CheckBox _spawnDebugCheckbox, _classicCheckbox, _resourceCheckbox, _stageSelectCheckbox, _freeMovementCheckbox;
        RadioButton[] _dbgSettingRadioButton;
        RadioButton _dbgSettingRadioButtonOff;

        public DebugManager(List<WatchVariable> variableData, Control tabControl, NoTearFlowLayoutPanel variableTable)
            : base(variableData, variableTable)
        {
            SplitContainer splitContainerDebug = tabControl.Controls["splitContainerDebug"] as SplitContainer;

            GroupBox advancedModeGroupbox = splitContainerDebug.Panel1.Controls["groupBoxAdvancedMode"] as GroupBox;

            _spawnDebugCheckbox = tabControl.Controls["checkBoxDbgSpawnDbg"] as CheckBox;
            _spawnDebugCheckbox.Click += (sender, e) =>
            {
                Config.Stream.SetValue(_spawnDebugCheckbox.Checked ? (byte)0x03 : (byte)0x00, Config.Debug.AdvancedModeSettingAddress);
                Config.Stream.SetValue(_spawnDebugCheckbox.Checked ? (byte)0x01 : (byte)0x00, Config.Debug.SpawnModeAddress);
            };

            _classicCheckbox = tabControl.Controls["checkBoxDbgClassicDbg"] as CheckBox;
            _classicCheckbox.Click += (sender, e) =>
            {
                Config.Stream.SetValue(_classicCheckbox.Checked ? (byte)0x01 : (byte)0x00, Config.Debug.ClassicModeAddress);
            };

            _resourceCheckbox = tabControl.Controls["checkBoxDbgResource"] as CheckBox;
            _resourceCheckbox.Click += (sender, e) =>
            {
                Config.Stream.SetValue(_resourceCheckbox.Checked ? (byte)0x01 : (byte)0x00, Config.Debug.ResourceModeAddress);
            };

            _stageSelectCheckbox = tabControl.Controls["checkBoxDbgStageSelect"] as CheckBox;
            _stageSelectCheckbox.Click += (sender, e) =>
            {
                Config.Stream.SetValue(_stageSelectCheckbox.Checked ? (byte)0x01 : (byte)0x00, Config.Debug.StageSelectAddress);
            };

            _freeMovementCheckbox = tabControl.Controls["checkBoxDbgFreeMovement"] as CheckBox;
            _freeMovementCheckbox.Click += (sender, e) => 
            {
                Config.Stream.SetValue(
                    _freeMovementCheckbox.Checked ? Config.Debug.FreeMovementOnValue : Config.Debug.FreeMovementOffValue,
                    Config.Debug.FreeMovementAddress);
            };

            _dbgSettingRadioButtonOff = advancedModeGroupbox.Controls["radioButtonDbgOff"] as RadioButton;
            _dbgSettingRadioButtonOff.Click += (sender, e) =>
            {
                Config.Stream.SetValue((byte)0, Config.Debug.AdvancedModeAddress);
                Config.Stream.SetValue((byte)0, Config.Debug.AdvancedModeSettingAddress);
            };

            _dbgSettingRadioButton = new RadioButton[6];
            _dbgSettingRadioButton[0] = advancedModeGroupbox.Controls["radioButtonDbgObjCnt"] as RadioButton;
            _dbgSettingRadioButton[1] = advancedModeGroupbox.Controls["radioButtonDbgChkInfo"] as RadioButton;
            _dbgSettingRadioButton[2] = advancedModeGroupbox.Controls["radioButtonDbgMapInfo"] as RadioButton;
            _dbgSettingRadioButton[3] = advancedModeGroupbox.Controls["radioButtonDbgStgInfo"] as RadioButton;
            _dbgSettingRadioButton[4] = advancedModeGroupbox.Controls["radioButtonDbgFxInfo"] as RadioButton;
            _dbgSettingRadioButton[5] = advancedModeGroupbox.Controls["radioButtonDbgEnemyInfo"] as RadioButton;
            for (int i = 0; i < _dbgSettingRadioButton.Length; i++)
            {
                byte localIndex = (byte)i;
                _dbgSettingRadioButton[i].Click += (sender, e) =>
                {
                    Config.Stream.SetValue((byte)1, Config.Debug.AdvancedModeAddress);
                    Config.Stream.SetValue(localIndex, Config.Debug.AdvancedModeSettingAddress);
                };
            }
        }

        public override void Update(bool updateView)
        {
            if (!updateView)
                return;

            _spawnDebugCheckbox.Checked = Config.Stream.GetByte(Config.Debug.AdvancedModeSettingAddress) == 0x03
                 && Config.Stream.GetByte(Config.Debug.SpawnModeAddress) == 0x01;
            _classicCheckbox.Checked = Config.Stream.GetByte(Config.Debug.ClassicModeAddress) == 0x01;
            _resourceCheckbox.Checked = Config.Stream.GetByte(Config.Debug.ResourceModeAddress) == 0x01;
            _stageSelectCheckbox.Checked = Config.Stream.GetByte(Config.Debug.StageSelectAddress) == 0x01;
            _freeMovementCheckbox.Checked = Config.Stream.GetUInt16(Config.Debug.FreeMovementAddress) == Config.Debug.FreeMovementOnValue;

            var setting = Config.Stream.GetByte(Config.Debug.AdvancedModeSettingAddress);
            var on = Config.Stream.GetByte(Config.Debug.AdvancedModeAddress);
            if (on % 2 != 0)
            {
                if (setting > 0 && setting < _dbgSettingRadioButton.Length)
                    _dbgSettingRadioButton[setting].Checked = true;
                else
                    _dbgSettingRadioButton[0].Checked = true;
            }
            else
            {
                _dbgSettingRadioButtonOff.Checked = true;
            }
        }
    }
}

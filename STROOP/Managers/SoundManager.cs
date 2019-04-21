using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;
using System.Windows.Forms;
using STROOP.Utilities;
using STROOP.Controls;
using STROOP.Structs.Configurations;
using System.Drawing;

namespace STROOP.Managers
{
    public class SoundManager
    {
        public SoundManager(TabPage tabPage)
        {
            /*
            CurrentMainSaveMode = MainSaveMode.MainSave;

            SplitContainer splitContainerMainSave = tabPage.Controls["splitContainerMainSave"] as SplitContainer;
            TableLayoutPanel tableLayoutPanelMainSaveCoinRank = splitContainerMainSave.Panel1.Controls["tableLayoutPanelMainSaveCoinRank"] as TableLayoutPanel;

            _mainSaveTextboxes = new List<MainSaveTextbox>();
            for (int row = 1; row <= 15; row++)
            {
                for (int col = 1; col <= 4; col++)
                {
                    string controlName = String.Format("textBoxMainSaveCoinRankRow{0}Col{1}", row, col);
                    MainSaveTextbox mainSaveTextbox = tableLayoutPanelMainSaveCoinRank.Controls[controlName] as MainSaveTextbox;
                    mainSaveTextbox.Initialize(row - 1, col - 1);
                    _mainSaveTextboxes.Add(mainSaveTextbox);
                }
            }

            GroupBox groupBoxMainSaveStruct = splitContainerMainSave.Panel1.Controls["groupBoxMainSaveStruct"] as GroupBox;

            _radioButtonMainSaveStructMainSave = groupBoxMainSaveStruct.Controls["radioButtonMainSaveStructMainSave"] as RadioButton;
            _radioButtonMainSaveStructMainSave.Click += (sender, e) => CurrentMainSaveMode = MainSaveMode.MainSave;

            _radioButtonMainSaveStructMainSaveSaved = groupBoxMainSaveStruct.Controls["radioButtonMainSaveStructMainSaveSaved"] as RadioButton;
            _radioButtonMainSaveStructMainSaveSaved.Click += (sender, e) => CurrentMainSaveMode = MainSaveMode.MainSaveSaved;

            GroupBox groupBoxMainSaveSoundMode = splitContainerMainSave.Panel1.Controls["groupBoxMainSaveSoundMode"] as GroupBox;

            _radioButtonMainSaveSoundModeStereo = groupBoxMainSaveSoundMode.Controls["radioButtonMainSaveSoundModeStereo"] as RadioButton;
            _radioButtonMainSaveSoundModeStereo.Click += (sender, e) =>
                Config.Stream.SetValue(MainSaveConfig.SoundModeStereoValue, CurrentMainSaveAddress + MainSaveConfig.SoundModeOffset);

            _radioButtonMainSaveSoundModeMono = groupBoxMainSaveSoundMode.Controls["radioButtonMainSaveSoundModeMono"] as RadioButton;
            _radioButtonMainSaveSoundModeMono.Click += (sender, e) =>
                Config.Stream.SetValue(MainSaveConfig.SoundModeMonoValue, CurrentMainSaveAddress + MainSaveConfig.SoundModeOffset);

            _radioButtonMainSaveSoundModeHeadset = groupBoxMainSaveSoundMode.Controls["radioButtonMainSaveSoundModeHeadset"] as RadioButton;
            _radioButtonMainSaveSoundModeHeadset.Click += (sender, e) =>
                Config.Stream.SetValue(MainSaveConfig.SoundModeHeadsetValue, CurrentMainSaveAddress + MainSaveConfig.SoundModeOffset);

            _buttonMainSaveSave = splitContainerMainSave.Panel1.Controls["buttonMainSaveSave"] as Button;
            _buttonMainSaveSave.Click += (sender, e) => Save();
            */
        }

        public void Update(bool updateView)
        {
            if (!updateView) return;
        }
    }
}

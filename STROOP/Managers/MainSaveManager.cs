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
    public class MainSaveManager : DataManager
    {
        public enum MainSaveMode { MainSave, MainSaveSaved };

        public MainSaveMode CurrentMainSaveMode { get; private set; }
        public uint CurrentMainSaveAddress
        {
            get => GetMainSaveAddress();
        }

        private List<MainSaveTextbox> _mainSaveTextboxes;

        private RadioButton _radioButtonMainSaveStructMainSave;
        private RadioButton _radioButtonMainSaveStructMainSaveSaved;

        private RadioButton _radioButtonMainSaveSoundModeStereo;
        private RadioButton _radioButtonMainSaveSoundModeMono;
        private RadioButton _radioButtonMainSaveSoundModeHeadset;

        private Button _buttonMainSaveSave;

        public MainSaveManager(string varFilePath, TabPage tabPage, WatchVariableFlowLayoutPanel watchVariablePanel)
            : base(varFilePath, watchVariablePanel)
        {
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
        }

        public ushort GetChecksum(uint? nullableMainSaveAddress = null)
        {
            uint mainSaveAddress = nullableMainSaveAddress ?? CurrentMainSaveAddress;
            ushort checksum = (ushort)(MainSaveConfig.ChecksumConstantValue % 256 + MainSaveConfig.ChecksumConstantValue / 256);
            for (uint i = 0; i < MainSaveConfig.MainSaveStructSize - 4; i++)
            {
                byte b = Config.Stream.GetByte(mainSaveAddress + i);
                checksum += b;
            }
            return checksum;
        }

        private void Save()
        {
            ushort checksum = GetChecksum(MainSaveConfig.MainSaveAddress);

            Config.Stream.SetValue(MainSaveConfig.ChecksumConstantValue, MainSaveConfig.MainSaveAddress + MainSaveConfig.ChecksumConstantOffset);
            Config.Stream.SetValue(checksum, MainSaveConfig.MainSaveAddress + MainSaveConfig.ChecksumOffset);

            Config.Stream.SetValue(MainSaveConfig.ChecksumConstantValue, MainSaveConfig.MainSaveSavedAddress + MainSaveConfig.ChecksumConstantOffset);
            Config.Stream.SetValue(checksum, MainSaveConfig.MainSaveSavedAddress + MainSaveConfig.ChecksumOffset);

            for (int i = 0; i < MainSaveConfig.MainSaveStructSize - 4; i++)
            {
                byte b = Config.Stream.GetByte(MainSaveConfig.MainSaveAddress + (uint)i);
                Config.Stream.SetValue(b, MainSaveConfig.MainSaveSavedAddress + (uint)i);
            }
        }
       
        private uint GetMainSaveAddress(MainSaveMode? nullableMode = null)
        {
            MainSaveMode mode = nullableMode ?? CurrentMainSaveMode;
            switch (mode)
            {
                case MainSaveMode.MainSave:
                    return MainSaveConfig.MainSaveAddress;
                case MainSaveMode.MainSaveSaved:
                    return MainSaveConfig.MainSaveSavedAddress;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;

            foreach (MainSaveTextbox mainSaveTextbox in _mainSaveTextboxes)
            {
                mainSaveTextbox.UpdateText();
            }

            ushort soundModeValue = Config.Stream.GetUInt16(CurrentMainSaveAddress + MainSaveConfig.SoundModeOffset);
            _radioButtonMainSaveSoundModeStereo.Checked = soundModeValue == 0;
            _radioButtonMainSaveSoundModeMono.Checked = soundModeValue == 1;
            _radioButtonMainSaveSoundModeHeadset.Checked = soundModeValue == 2;

            base.Update(updateView);
        }
    }
}

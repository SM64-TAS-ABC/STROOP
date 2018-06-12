using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;
using System.Windows.Forms;
using STROOP.Utilities;
using STROOP.Extensions;
using STROOP.Controls;
using STROOP.Structs.Configurations;

namespace STROOP.Managers
{
    public class MiscManager : DataManager
    {
        CheckBox _checkBoxTurnOffMusic;

        private static readonly List<VariableGroup> ALL_VAR_GROUPS =
            new List<VariableGroup>()
            {
                VariableGroup.Basic,
                VariableGroup.Intermediate,
                VariableGroup.Advanced,
                VariableGroup.Rng,
            };

        private static readonly List<VariableGroup> VISIBLE_VAR_GROUPS =
            new List<VariableGroup>()
            {
                VariableGroup.Basic,
                VariableGroup.Intermediate,
                VariableGroup.Advanced,
            };

        public MiscManager(string varFilePath, WatchVariableFlowLayoutPanel variableTable, Control miscControl)
            : base(varFilePath, variableTable, ALL_VAR_GROUPS, VISIBLE_VAR_GROUPS)
        {
            SplitContainer splitContainerMisc = miscControl.Controls["splitContainerMisc"] as SplitContainer;

            _checkBoxTurnOffMusic = splitContainerMisc.Panel1.Controls["checkBoxTurnOffMusic"] as CheckBox;

            GroupBox groupBoxRNGIndexTester = splitContainerMisc.Panel1.Controls["groupBoxRNGIndexTester"] as GroupBox;
            TextBox textBoxRNGIndexTester = groupBoxRNGIndexTester.Controls["textBoxRNGIndexTester"] as TextBox;
            Button buttonRNGIndexTester = groupBoxRNGIndexTester.Controls["buttonRNGIndexTester"] as Button;
            buttonRNGIndexTester.Click += (sender, e) =>
            {
                int? rngIndexNullable = ParsingUtilities.ParseIntNullable(textBoxRNGIndexTester.Text);
                if (!rngIndexNullable.HasValue) return;
                int rngIndex = rngIndexNullable.Value;
                ushort rngValue = RngIndexer.GetRngValue(rngIndex);
                Config.Stream.SetValue(rngValue, MiscConfig.RngAddress);
                int nextRngIndex = rngIndex + 1;
                textBoxRNGIndexTester.Text = nextRngIndex.ToString();
            };
        }

        public override void Update(bool updateView)
        {
            if (_checkBoxTurnOffMusic.Checked)
            {
                byte oldMusicByte = Config.Stream.GetByte(MiscConfig.MusicOnAddress);
                byte newMusicByte = MoreMath.ApplyValueToMaskedByte(oldMusicByte, MiscConfig.MusicOnMask, true);
                Config.Stream.SetValue(newMusicByte, MiscConfig.MusicOnAddress);
                Config.Stream.SetValue(0f, MiscConfig.MusicVolumeAddress);
            }

            if (!updateView) return;
            base.Update(updateView);
        }

    }
}

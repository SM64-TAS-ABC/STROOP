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
                VariableGroup.Coin,
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

            Button buttonMiscGoToCourse = splitContainerMisc.Panel1.Controls["buttonMiscGoToCourse"] as Button;
            buttonMiscGoToCourse.ContextMenuStrip = new ContextMenuStrip();
            foreach (CourseToGoTo courseToGoTo in _coursesToGoTo)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(courseToGoTo.Name);
                item.Click += (sender, e) => InGameFunctionCall.WriteInGameFunctionCall(
                    RomVersionConfig.SwitchMap(0x8024978C, 0x8024975C), (uint)courseToGoTo.Index);
                buttonMiscGoToCourse.ContextMenuStrip.Items.Add(item);
            }
            buttonMiscGoToCourse.Click += (sender, e) => buttonMiscGoToCourse.ContextMenuStrip.Show(Cursor.Position);
        }

        private static readonly List<CourseToGoTo> _coursesToGoTo = new List<CourseToGoTo>()
        {
            new CourseToGoTo(09, "Bob-omb Battlefield"),
            new CourseToGoTo(24, "Whomp's Fortress"),
            new CourseToGoTo(12, "Jolly Roger Bay"),
            new CourseToGoTo(05, "Cool, Cool Mountain"),
            new CourseToGoTo(04, "Big Boo's Haunt"),
            new CourseToGoTo(07, "Hazy Maze Cave"),
            new CourseToGoTo(22, "Lethal Lava Land"),
            new CourseToGoTo(08, "Shifting Sand Land"),
            new CourseToGoTo(23, "Dire, Dire Docks"),
            new CourseToGoTo(10, "Snowman's Land"),
            new CourseToGoTo(11, "Wet-Dry World"),
            new CourseToGoTo(36, "Tall, Tall Mountain"),
            new CourseToGoTo(13, "Tiny-Huge Island"),
            new CourseToGoTo(14, "Tick Tock Clock"),
            new CourseToGoTo(15, "Rainbow Ride"),

            new CourseToGoTo(29, "Tower of the Wing Cap"),
            new CourseToGoTo(28, "Cavern of the Metal Cap"),
            new CourseToGoTo(18, "Vanish Cap under the Moat"),

            new CourseToGoTo(27, "The Princess's Secret Slide"),
            new CourseToGoTo(20, "The Secret Aquarium"),
            new CourseToGoTo(31, "Wing Mario over the Rainbow"),

            new CourseToGoTo(17, "Bowser in the Dark World"),
            new CourseToGoTo(30, "Bowser in the Dark World Fight"),
            new CourseToGoTo(19, "Bowser in the Fire Sea"),
            new CourseToGoTo(33, "Bowser in the Fire Sea Fight"),
            new CourseToGoTo(21, "Bowser in the Sky"),
            new CourseToGoTo(34, "Bowser in the Sky Fight"),

            new CourseToGoTo(06, "Castle Inside"),
            new CourseToGoTo(16, "Castle Grounds"),
            new CourseToGoTo(26, "Castle Courtyard"),

            new CourseToGoTo(25, "Ending Cutscene"),
        };

        private class CourseToGoTo
        {
            public readonly int Index;
            public readonly string Name;

            public CourseToGoTo(int index, string name)
            {
                Index = index;
                Name = name;
            }

            public override string ToString()
            {
                return Name;
            }
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

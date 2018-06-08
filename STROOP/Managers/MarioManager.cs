using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;
using System.Windows.Forms;
using STROOP.Utilities;
using STROOP.Controls;
using STROOP.Extensions;
using STROOP.Structs.Configurations;

namespace STROOP.Managers
{
    public class MarioManager : DataManager
    {
        private static readonly List<VariableGroup> ALL_VAR_GROUPS =
            new List<VariableGroup>()
            {
                VariableGroup.Basic,
                VariableGroup.Intermediate,
                VariableGroup.Advanced,
                VariableGroup.Trajectory,
                VariableGroup.Hacks,
            };

        private static readonly List<VariableGroup> VISIBLE_VAR_GROUPS =
            new List<VariableGroup>()
            {
                VariableGroup.Basic,
                VariableGroup.Intermediate,
                VariableGroup.Advanced,
            };

        public MarioManager(string varFilePath, Control marioControl, WatchVariableFlowLayoutPanel variableTable)
            : base(varFilePath, variableTable, ALL_VAR_GROUPS, VISIBLE_VAR_GROUPS)
        {
            SplitContainer splitContainerMario = marioControl.Controls["splitContainerMario"] as SplitContainer;

            Button toggleHandsfree = splitContainerMario.Panel1.Controls["buttonMarioToggleHandsfree"] as Button;
            toggleHandsfree.Click += (sender, e) => ButtonUtilities.ToggleHandsfree();

            Button toggleVisibility = splitContainerMario.Panel1.Controls["buttonMarioVisibility"] as Button;
            toggleVisibility.Click += (sender, e) => ButtonUtilities.ToggleVisibility();

            var marioPosGroupBox = splitContainerMario.Panel1.Controls["groupBoxMarioPos"] as GroupBox;
            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Euler,
                true,
                marioPosGroupBox,
                marioPosGroupBox.Controls["buttonMarioPosXn"] as Button,
                marioPosGroupBox.Controls["buttonMarioPosXp"] as Button,
                marioPosGroupBox.Controls["buttonMarioPosZn"] as Button,
                marioPosGroupBox.Controls["buttonMarioPosZp"] as Button,
                marioPosGroupBox.Controls["buttonMarioPosXnZn"] as Button,
                marioPosGroupBox.Controls["buttonMarioPosXnZp"] as Button,
                marioPosGroupBox.Controls["buttonMarioPosXpZn"] as Button,
                marioPosGroupBox.Controls["buttonMarioPosXpZp"] as Button,
                marioPosGroupBox.Controls["buttonMarioPosYp"] as Button,
                marioPosGroupBox.Controls["buttonMarioPosYn"] as Button,
                marioPosGroupBox.Controls["textBoxMarioPosXZ"] as TextBox,
                marioPosGroupBox.Controls["textBoxMarioPosY"] as TextBox,
                marioPosGroupBox.Controls["checkBoxMarioPosRelative"] as CheckBox,
                (float hOffset, float vOffset, float nOffset, bool useRelative) =>
                {
                    ButtonUtilities.TranslateMario(
                        hOffset,
                        nOffset,
                        -1 * vOffset,
                        useRelative);
                });

            var marioStatsGroupBox = splitContainerMario.Panel1.Controls["groupBoxMarioStats"] as GroupBox;
            ControlUtilities.InitializeScalarController(
                marioStatsGroupBox.Controls["buttonMarioStatsYawN"] as Button,
                marioStatsGroupBox.Controls["buttonMarioStatsYawP"] as Button,
                marioStatsGroupBox.Controls["textBoxMarioStatsYaw"] as TextBox,
                (float value) =>
                {
                    ButtonUtilities.MarioChangeYaw((int)Math.Round(value));
                });
            ControlUtilities.InitializeScalarController(
                marioStatsGroupBox.Controls["buttonMarioStatsHspdN"] as Button,
                marioStatsGroupBox.Controls["buttonMarioStatsHspdP"] as Button,
                marioStatsGroupBox.Controls["textBoxMarioStatsHspd"] as TextBox,
                (float value) =>
                {
                    ButtonUtilities.MarioChangeHspd(value);
                });
            ControlUtilities.InitializeScalarController(
                marioStatsGroupBox.Controls["buttonMarioStatsVspdN"] as Button,
                marioStatsGroupBox.Controls["buttonMarioStatsVspdP"] as Button,
                marioStatsGroupBox.Controls["textBoxMarioStatsVspd"] as TextBox,
                (float value) =>
                {
                    ButtonUtilities.MarioChangeVspd(value);
                });

            var marioSlidingSpeedGroupBox = splitContainerMario.Panel1.Controls["groupBoxMarioSlidingSpeed"] as GroupBox;
            ControlUtilities.InitializeScalarController(
                marioSlidingSpeedGroupBox.Controls["buttonMarioSlidingSpeedXn"] as Button,
                marioSlidingSpeedGroupBox.Controls["buttonMarioSlidingSpeedXp"] as Button,
                marioSlidingSpeedGroupBox.Controls["textBoxMarioSlidingSpeedX"] as TextBox,
                (float value) =>
                {
                    ButtonUtilities.MarioChangeSlidingSpeedX(value);
                });
            ControlUtilities.InitializeScalarController(
                marioSlidingSpeedGroupBox.Controls["buttonMarioSlidingSpeedZn"] as Button,
                marioSlidingSpeedGroupBox.Controls["buttonMarioSlidingSpeedZp"] as Button,
                marioSlidingSpeedGroupBox.Controls["textBoxMarioSlidingSpeedZ"] as TextBox,
                (float value) =>
                {
                    ButtonUtilities.MarioChangeSlidingSpeedZ(value);
                });
            ControlUtilities.InitializeScalarController(
                marioSlidingSpeedGroupBox.Controls["buttonMarioSlidingSpeedHn"] as Button,
                marioSlidingSpeedGroupBox.Controls["buttonMarioSlidingSpeedHp"] as Button,
                marioSlidingSpeedGroupBox.Controls["textBoxMarioSlidingSpeedH"] as TextBox,
                (float value) =>
                {
                    ButtonUtilities.MarioChangeSlidingSpeedH(value);
                });
            ControlUtilities.InitializeScalarController(
                marioSlidingSpeedGroupBox.Controls["buttonMarioSlidingSpeedYawN"] as Button,
                marioSlidingSpeedGroupBox.Controls["buttonMarioSlidingSpeedYawP"] as Button,
                marioSlidingSpeedGroupBox.Controls["textBoxMarioSlidingSpeedYaw"] as TextBox,
                (float value) =>
                {
                    ButtonUtilities.MarioChangeSlidingSpeedYaw(value);
                });

            Button buttonMarioHOLPGoto = splitContainerMario.Panel1.Controls["buttonMarioHOLPGoto"] as Button;
            buttonMarioHOLPGoto.Click += (sender, e) => ButtonUtilities.GotoHOLP();
            ControlUtilities.AddContextMenuStripFunctions(
                buttonMarioHOLPGoto,
                new List<string>() { "Goto HOLP", "Goto HOLP Laterally", "Goto HOLP X", "Goto HOLP Y", "Goto HOLP Z" },
                new List<Action>() {
                    () => ButtonUtilities.GotoHOLP((true, true, true)),
                    () => ButtonUtilities.GotoHOLP((true, false, true)),
                    () => ButtonUtilities.GotoHOLP((true, false, false)),
                    () => ButtonUtilities.GotoHOLP((false, true, false)),
                    () => ButtonUtilities.GotoHOLP((false, false, true)),
                });

            Button buttonMarioHOLPRetrieve = splitContainerMario.Panel1.Controls["buttonMarioHOLPRetrieve"] as Button;
            buttonMarioHOLPRetrieve.Click += (sender, e) => ButtonUtilities.RetrieveHOLP();
            ControlUtilities.AddContextMenuStripFunctions(
                buttonMarioHOLPRetrieve,
                new List<string>() { "Retrieve HOLP", "Retrieve HOLP Laterally", "Retrieve HOLP X", "Retrieve HOLP Y", "Retrieve HOLP Z" },
                new List<Action>() {
                    () => ButtonUtilities.RetrieveHOLP((true, true, true)),
                    () => ButtonUtilities.RetrieveHOLP((true, false, true)),
                    () => ButtonUtilities.RetrieveHOLP((true, false, false)),
                    () => ButtonUtilities.RetrieveHOLP((false, true, false)),
                    () => ButtonUtilities.RetrieveHOLP((false, false, true)),
                });

            var marioHOLPGroupBox = splitContainerMario.Panel1.Controls["groupBoxMarioHOLP"] as GroupBox;
            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Euler,
                true,
                marioHOLPGroupBox,
                marioHOLPGroupBox.Controls["buttonMarioHOLPXn"] as Button,
                marioHOLPGroupBox.Controls["buttonMarioHOLPXp"] as Button,
                marioHOLPGroupBox.Controls["buttonMarioHOLPZn"] as Button,
                marioHOLPGroupBox.Controls["buttonMarioHOLPZp"] as Button,
                marioHOLPGroupBox.Controls["buttonMarioHOLPXnZn"] as Button,
                marioHOLPGroupBox.Controls["buttonMarioHOLPXnZp"] as Button,
                marioHOLPGroupBox.Controls["buttonMarioHOLPXpZn"] as Button,
                marioHOLPGroupBox.Controls["buttonMarioHOLPXpZp"] as Button,
                marioHOLPGroupBox.Controls["buttonMarioHOLPYp"] as Button,
                marioHOLPGroupBox.Controls["buttonMarioHOLPYn"] as Button,
                marioHOLPGroupBox.Controls["textBoxMarioHOLPXZ"] as TextBox,
                marioHOLPGroupBox.Controls["textBoxMarioHOLPY"] as TextBox,
                marioHOLPGroupBox.Controls["checkBoxMarioHOLPRelative"] as CheckBox,
                (float hOffset, float vOffset, float nOffset, bool useRelative) =>
                {
                    ButtonUtilities.TranslateHOLP(
                        hOffset,
                        nOffset,
                        -1 * vOffset,
                        useRelative);
                });
        }

        public override void Update(bool updateView)
        {
            if (!updateView)
                return;

            base.Update(updateView);
        }
    }
}

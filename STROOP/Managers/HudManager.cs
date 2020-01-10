﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;
using System.Windows.Forms;
using STROOP.Utilities;
using STROOP.Controls;
using STROOP.Structs.Configurations;

namespace STROOP.Managers
{
    public class HudManager : DataManager
    {
        Control _tabControl;
        BinaryButton _turnOnOffHudButton;
        CheckBox _checkBoxFullHP;

        public HudManager(string varFilePath, Control tabControl, WatchVariableFlowLayoutPanel watchVariablePanelHud)
            : base(varFilePath, watchVariablePanelHud)
        {
            _tabControl = tabControl;

            SplitContainer splitContainerHud = tabControl.Controls["splitContainerHud"] as SplitContainer;

            (splitContainerHud.Panel1.Controls["buttonFullHp"] as Button).Click += (sender, e) => ButtonUtilities.FullHp();
            (splitContainerHud.Panel1.Controls["buttonDie"] as Button).Click += (sender, e) => ButtonUtilities.Die();
            (splitContainerHud.Panel1.Controls["buttonGameOver"] as Button).Click += (sender, e) => ButtonUtilities.GameOver();
            (splitContainerHud.Panel1.Controls["button100CoinStar"] as Button).Click += (sender, e) => ButtonUtilities.CoinStar100();
            (splitContainerHud.Panel1.Controls["button100Lives"] as Button).Click += (sender, e) => ButtonUtilities.Lives100();
            (splitContainerHud.Panel1.Controls["buttonStandardHud"] as Button).Click += (sender, e) => ButtonUtilities.StandardHud();

            _turnOnOffHudButton = splitContainerHud.Panel1.Controls["buttonTurnOnOffHud"] as BinaryButton;
            _turnOnOffHudButton.Initialize(
                "Turn Off HUD",
                "Turn On HUD",
                () => ButtonUtilities.SetHudVisibility(false),
                () => ButtonUtilities.SetHudVisibility(true),
                () => (Config.Stream.GetByte(MarioConfig.StructAddress + HudConfig.VisibilityOffset) & HudConfig.VisibilityMask) == 0);

            ControlUtilities.AddContextMenuStripFunctions(
                _turnOnOffHudButton,
                new List<string>
                {
                    "Disable HUD by Changing Level Index",
                    "Enable HUD by Changing Level Index",
                    "Disable HUD by Removing Function",
                    "Enable HUD by Removing Function"
                },
                new List<Action>
                {
                    () => ButtonUtilities.SetHudVisibility(false, true),
                    () => ButtonUtilities.SetHudVisibility(true, true),
                    () => ButtonUtilities.SetHudVisibility(false, false),
                    () => ButtonUtilities.SetHudVisibility(true, false)
                });

            _checkBoxFullHP = splitContainerHud.Panel1.Controls["checkBoxFullHP"] as CheckBox;
        }

        public override void Update(bool updateView)
        {
            if (_checkBoxFullHP.Checked)
            {
                ButtonUtilities.FullHp();
            }

            if (!updateView) return;

            _turnOnOffHudButton.UpdateButton();

            base.Update(updateView);
        }
    }
}

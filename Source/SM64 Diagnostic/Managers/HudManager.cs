using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using System.Windows.Forms;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Controls;
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic.Managers
{
    public class HudManager : DataManager
    {
        Control _tabControl;
        bool _turnOnHud;
        BinaryButton _turnOnOffHudButton;

        public HudManager(List<WatchVariable> hudData, Control tabControl, NoTearFlowLayoutPanel noTearFlowLayoutPanelHud)
            : base(hudData, noTearFlowLayoutPanelHud)
        {
            _tabControl = tabControl;

            SplitContainer splitContainerHud = tabControl.Controls["splitContainerHud"] as SplitContainer;

            (splitContainerHud.Panel1.Controls["buttonFillHp"] as Button).Click += (sender, e) => ButtonUtilities.RefillHp();
            (splitContainerHud.Panel1.Controls["buttonDie"] as Button).Click += (sender, e) => ButtonUtilities.Die();
            (splitContainerHud.Panel1.Controls["buttonStandardHud"] as Button).Click += (sender, e) => ButtonUtilities.StandardHud();
            (splitContainerHud.Panel1.Controls["button99Coins"] as Button).Click += (sender, e) => ButtonUtilities.Coins99();

            uint hudAddressOffset = 0xFB;
            uint hudMask = 0x0F;

            _turnOnOffHudButton = splitContainerHud.Panel1.Controls["buttonTurnOnOffHud"] as BinaryButton;
            _turnOnOffHudButton.Initialize(
                "Turn Off HUD",
                "Turn On HUD",
                () => ButtonUtilities.SetHudVisibility(false),
                () => ButtonUtilities.SetHudVisibility(true),
                () => (Config.Stream.GetByte(Config.Mario.StructAddress + hudAddressOffset) & hudMask) == 0);
        }

        public override void Update(bool updateView)
        {
            _turnOnOffHudButton.UpdateButton();

            base.Update(updateView);
        }
    }
}

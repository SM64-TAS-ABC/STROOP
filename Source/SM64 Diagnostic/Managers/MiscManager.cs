using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using System.Windows.Forms;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Extensions;
using SM64_Diagnostic.Controls;
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic.Managers
{
    public class MiscManager : DataManager
    {
        CheckBox _checkBoxTurnOffMusic;

        public MiscManager(List<WatchVariableControlPrecursor> variables, WatchVariablePanel variableTable, Control miscControl)
            : base(variables, variableTable)
        {
            SplitContainer splitContainerMisc = miscControl.Controls["splitContainerMisc"] as SplitContainer;
            _checkBoxTurnOffMusic = splitContainerMisc.Panel1.Controls["checkBoxTurnOffMusic"] as CheckBox;
        }

        public override void Update(bool updateView)
        {
            if (_checkBoxTurnOffMusic.Checked)
            {
                byte oldMusicByte = Config.Stream.GetByte(Config.MusicOnAddress);
                byte newMusicByte = MoreMath.ApplyValueToMaskedByte(oldMusicByte, Config.MusicOnMask, true);
                Config.Stream.SetValue(newMusicByte, Config.MusicOnAddress);
                Config.Stream.SetValue(0f, Config.MusicVolumeAddress);
            }

            if (!updateView)
                return;

            base.Update();
        }

    }
}

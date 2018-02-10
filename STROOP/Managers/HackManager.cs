using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;
using System.Windows.Forms;
using STROOP.Utilities;
using STROOP.Structs.Configurations;

namespace STROOP.Managers
{
    public class HackManager
    {
        List<RomHack> _hacks;
        CheckedListBox _checkList;
        ListBox _spawnList;
        TextBox _behaviorTextbox, _gfxIdTextbox, _extraTextbox;

        object _listLocker = new object();

        public HackManager(List<RomHack> hacks, List<SpawnHack> spawnCodes,  Control tabControl)
        {
            _hacks = hacks;

            var splitContainter = tabControl.Controls["splitContainerHacks"] as SplitContainer;
            _checkList = splitContainter.Panel1.Controls["groupBoxHackRam"].Controls["checkedListBoxHacks"] as CheckedListBox;

            var spawnGroup = splitContainter.Panel2.Controls["groupBoxHackSpawn"];
            _spawnList = spawnGroup.Controls["listBoxSpawn"] as ListBox;
            var spawnButton = spawnGroup.Controls["buttonHackSpawn"] as Button;
            _behaviorTextbox = spawnGroup.Controls["textBoxSpawnBehavior"] as TextBox;
            _gfxIdTextbox = spawnGroup.Controls["textBoxSpawnGfxId"] as TextBox;
            _extraTextbox = spawnGroup.Controls["textBoxSpawnExtra"] as TextBox;
            var resetButton = spawnGroup.Controls["buttonSpawnReset"] as Button;

            // Load spawn objects codes
            foreach (var code in spawnCodes)
                _spawnList.Items.Add(code);

            // Load hack lists
            foreach (var hack in _hacks)
                _checkList.Items.Add(hack);
            
            _checkList.ItemCheck += _checkList_ItemCheck;
            _spawnList.SelectedIndexChanged += _spawnList_SelectedIndexChanged;
            spawnButton.Click += SpawnButton_Click;
            resetButton.Click += ResetButton_Click;
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            HackConfig.SpawnHack.ClearPayload();
        }

        private void _spawnList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_spawnList.SelectedItems.Count == 0)
                return;

            var selectedHack = _spawnList.SelectedItem as SpawnHack;

            _behaviorTextbox.Text = String.Format("0x{0:X8}", selectedHack.Behavior);
            _gfxIdTextbox.Text = String.Format("0x{0:X2}", selectedHack.GfxId);
            _extraTextbox.Text = String.Format("0x{0:X2}", selectedHack.Extra);
        }

        private void SpawnButton_Click(object sender, EventArgs e)
        {
            if (_spawnList.SelectedItems.Count == 0)
                return;

            uint behavior, gfxId, extra;
            if (!ParsingUtilities.TryParseHex(_behaviorTextbox.Text, out behavior))
            {
                MessageBox.Show("Could not parse behavior!");
                return;
            }
            if (!ParsingUtilities.TryParseHex(_gfxIdTextbox.Text, out gfxId))
            {
                MessageBox.Show("Could not parse gfxId!");
                return;
            }
            if (!ParsingUtilities.TryParseHex(_extraTextbox.Text, out extra))
            {
                MessageBox.Show("Could not parse extra!");
                return;
            }

            Config.Stream.Suspend();

            HackConfig.SpawnHack.LoadPayload(false);

            Config.Stream.SetValue(behavior, HackConfig.BehaviorAddress);
            Config.Stream.SetValue((UInt16)gfxId, HackConfig.GfxIdAddress);
            Config.Stream.SetValue((UInt16)extra, HackConfig.ExtraAddress);

            Config.Stream.Resume();
        }

        private void _checkList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var hack = (RomHack)_checkList.Items[e.Index];
            if (e.NewValue == CheckState.Checked)
                hack.LoadPayload();
            else
                hack.ClearPayload();
        }

        public void Update()
        {
            // Update rom hack statuses
            for (int i = 0; i < _checkList.Items.Count; i++)
            {
                var hack = (RomHack) _checkList.Items[i];
                hack.UpdateEnabledStatus();

                if (_checkList.GetItemChecked(i) != hack.Enabled)
                    _checkList.SetItemChecked(i, hack.Enabled);
            }
        }
    }
}

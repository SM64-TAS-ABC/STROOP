using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using System.Windows.Forms;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic.Managers
{
    public class HackManager
    {
        List<RomHack> _hacks;
        ProcessStream _stream;
        CheckedListBox _checkList;
        ListBox _spawnList;
        TextBox _gfxIdTextbox, _extraTextbox;

        object _listLocker = new object();

        public HackManager(ProcessStream stream, List<RomHack> hacks, List<SpawnHack> spawnCodes,  Control tabControl)
        {
            _hacks = hacks;
            _stream = stream;

            var splitContainter = tabControl.Controls["splitContainerHacks"] as SplitContainer;
            _checkList = splitContainter.Panel1.Controls["groupBoxHackRam"].Controls["checkedListBoxHacks"] as CheckedListBox;

            var spawnGroup = splitContainter.Panel2.Controls["groupBoxHackSpawn"];
            _spawnList = spawnGroup.Controls["listBoxSpawn"] as ListBox;
            var spawnButton = spawnGroup.Controls["buttonHackSpawn"] as Button;
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
            Config.Hacks.SpawnHack.ClearPayload(_stream);
        }

        private void _spawnList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_spawnList.SelectedItems.Count == 0)
                return;

            var selectedHack = _spawnList.SelectedItem as SpawnHack;

            _gfxIdTextbox.Text = String.Format("0x{0:X2}", selectedHack.GfxId);
            _extraTextbox.Text = String.Format("0x{0:X2}", selectedHack.Extra);
        }

        private void SpawnButton_Click(object sender, EventArgs e)
        {
            if (_spawnList.SelectedItems.Count == 0)
                return;

            uint gfxId, extra;
            if (!ParsingUtilities.TryParseHex(_gfxIdTextbox.Text, out gfxId))
            {
                MessageBox.Show("Fail");
                return;
            }
            if (!ParsingUtilities.TryParseHex(_extraTextbox.Text, out extra))
            {
                MessageBox.Show("Fail");
                return;
            }

            _stream.Suspend();

            Config.Hacks.SpawnHack.LoadPayload(_stream, false);

            var selectedHack = _spawnList.SelectedItem as SpawnHack;

            _stream.SetValue(selectedHack.Behavior, Config.Hacks.BehaviorAddress);
            _stream.SetValue((UInt16)gfxId, Config.Hacks.GfxIdAddress);
            _stream.SetValue((UInt16)extra, Config.Hacks.ExtraAddress);

            _stream.Resume();
        }

        private void _checkList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var hack = (RomHack)_checkList.Items[e.Index];
            if (e.NewValue == CheckState.Checked)
                hack.LoadPayload(_stream);
            else
                hack.ClearPayload(_stream);
        }

        public void Update()
        {
            // Update rom hack statuses
            for (int i = 0; i < _checkList.Items.Count; i++)
            {
                var hack = (RomHack) _checkList.Items[i];
                hack.UpdateEnabledStatus(_stream);

                if (_checkList.GetItemChecked(i) != hack.Enabled)
                    _checkList.SetItemChecked(i, hack.Enabled);
            }
        }
    }
}

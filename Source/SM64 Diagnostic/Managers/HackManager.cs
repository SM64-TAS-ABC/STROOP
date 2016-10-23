using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using System.Windows.Forms;
using SM64_Diagnostic.Utilities;

namespace SM64_Diagnostic.ManagerClasses
{
    public class HackManager
    {
        List<RomHack> _hacks;
        ProcessStream _stream;
        CheckedListBox _checkList;

        object _listLocker = new object();

        public HackManager(ProcessStream stream, List<RomHack> hacks, CheckedListBox checkList)
        {
            _checkList = checkList;
            _hacks = hacks;
            _stream = stream;

            foreach (var hack in _hacks)
                _checkList.Items.Add(hack);
            
            _checkList.ItemCheck += _checkList_ItemCheck;
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

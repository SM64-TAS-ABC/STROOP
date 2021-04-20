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

namespace STROOP.Managers
{
    public class MusicManager : DataManager
    {
        private ListBox _listBoxMusic;

        public MusicManager(string varFilePath, WatchVariableFlowLayoutPanel variables, TabPage tabPage)
            : base(varFilePath, variables)
        {
            SplitContainer splitContainer = tabPage.Controls["splitContainerMusic"] as SplitContainer;
            _listBoxMusic = splitContainer.Panel1.Controls["listBoxMusic"] as ListBox;
            for (int i = 0; i < 3; i++)
            {
                _listBoxMusic.Items.Add(i);
            }
        }

        public uint? GetMusicAddress()
        {
            object value = _listBoxMusic.SelectedItem;
            if (value is int intValue)
            {
                uint baseAddress = 0x80222A18;
                uint size = 0x140;
                uint address = (uint)(baseAddress + intValue * size);
                return Config.Stream.GetUInt(address);
            }
            return null;
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;
            base.Update(updateView);
        }
    }
}

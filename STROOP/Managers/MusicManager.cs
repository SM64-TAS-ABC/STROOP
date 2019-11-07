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
        private ListBox _listBoxPainting;

        public MusicManager(string varFilePath, WatchVariableFlowLayoutPanel variables, TabPage tabPage)
            : base(varFilePath, variables)
        {
            /*
            SplitContainer splitContainer = tabPage.Controls["splitContainerPainting"] as SplitContainer;
            _listBoxPainting = splitContainer.Panel1.Controls["listBoxPainting"] as ListBox;
            foreach (PaintingData paintingData in paintingDataList)
            {
                _listBoxPainting.Items.Add(paintingData);
            }
            */
        }

        public uint? GetMusicAddress()
        {
            return null;
            /*
            PaintingData paintingData = _listBoxPainting.SelectedItem as PaintingData;
            return paintingData?.GetAddress();
            */
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;
            base.Update(updateView);
        }
    }
}

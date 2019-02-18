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
    public class PaintingManager : DataManager
    {
        private class PaintingData
        {
            private readonly string _name;
            private readonly PaintingListTypeEnum _paintingListType;
            private readonly int _index;

            public PaintingData(string name, PaintingListTypeEnum paintingListType, int index)
            {
                _name = name;
                _paintingListType = paintingListType;
                _index = index;
            }

            public override string ToString()
            {
                return _name;
            }

            public uint GetAddress()
            {
                return 0;
            }
        }

        private List<PaintingData> paintingDataList =
            new List<PaintingData>()
            {
                new PaintingData("BoB", PaintingListTypeEnum.Castle, 0),
                new PaintingData("WF", PaintingListTypeEnum.Castle, 2),
                new PaintingData("JRB", PaintingListTypeEnum.Castle, 3),
                new PaintingData("CCM", PaintingListTypeEnum.Castle, 1),
                new PaintingData("HMC", PaintingListTypeEnum.Castle, 6),
                new PaintingData("LLL", PaintingListTypeEnum.Castle, 4),
                new PaintingData("SSL", PaintingListTypeEnum.Castle, 5),
                new PaintingData("DDD", PaintingListTypeEnum.Castle, 7),
                new PaintingData("SL", PaintingListTypeEnum.Castle, 12),
                new PaintingData("WDW", PaintingListTypeEnum.Castle, 8),
                new PaintingData("TTM", PaintingListTypeEnum.Castle, 10),
                new PaintingData("TTM Slide", PaintingListTypeEnum.TTM, 0),
                new PaintingData("THI Tiny", PaintingListTypeEnum.Castle, 9),
                new PaintingData("THI Huge", PaintingListTypeEnum.Castle, 13),
                new PaintingData("TTC", PaintingListTypeEnum.Castle, 11),
                new PaintingData("CotMC", PaintingListTypeEnum.HMC, 0),
            };

        public PaintingManager(string varFilePath, WatchVariableFlowLayoutPanel variables, TabPage tabPage)
            : base(varFilePath, variables)
        {
            SplitContainer splitContainer = tabPage.Controls["splitContainerPainting"] as SplitContainer;
            ListBox listBox = splitContainer.Panel1.Controls["listBoxPainting"] as ListBox;
            foreach (PaintingData paintingData in paintingDataList)
            {
                listBox.Items.Add(paintingData);
            }
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;
            base.Update(updateView);


        }
    }
}

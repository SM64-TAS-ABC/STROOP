using STROOP.Controls;
using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace STROOP.Managers
{
    public class SearchManager : DataManager
    {
        public SearchManager(TabPage tabControl, WatchVariableFlowLayoutPanel watchVariablePanel)
            : base(null, watchVariablePanel)
        {
            SplitContainer splitContainerSearch = tabControl.Controls["splitContainerSearch"] as SplitContainer;
            SplitContainer splitContainerSearchOptions = splitContainerSearch.Panel1.Controls["splitContainerSearchOptions"] as SplitContainer;

            ComboBox comboBoxSearchMemoryType = splitContainerSearchOptions.Panel1.Controls["comboBoxSearchMemoryType"] as ComboBox;
            ComboBox comboBoxValueRelationship = splitContainerSearchOptions.Panel1.Controls["comboBoxValueRelationship"] as ComboBox;
            BetterTextbox textBoxSearchValue = splitContainerSearchOptions.Panel1.Controls["textBoxSearchValue"] as BetterTextbox;
            Button buttonSearchFirstScan = splitContainerSearchOptions.Panel1.Controls["buttonSearchFirstScan"] as Button;
            Button buttonSearchNextScan = splitContainerSearchOptions.Panel1.Controls["buttonSearchNextScan"] as Button;
            DataGridView dataGridViewSearch = splitContainerSearchOptions.Panel2.Controls["dataGridViewSearch"] as DataGridView;
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;

            base.Update(updateView);
        }
    }
}

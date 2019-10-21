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
    public class CellsManager : DataManager
    {
        private Button _buttonCellsBuildTree;
        private TreeView _treeViewCells;

        public CellsManager(TabPage tabControl, WatchVariableFlowLayoutPanel watchVariablePanel)
            : base(null, watchVariablePanel)
        {
            SplitContainer splitContainerCells = tabControl.Controls["splitContainerCells"] as SplitContainer;
            SplitContainer splitContainerCellsControls = splitContainerCells.Panel1.Controls["splitContainerCellsControls"] as SplitContainer;

            _buttonCellsBuildTree = splitContainerCellsControls.Panel1.Controls["buttonCellsBuildTree"] as Button;
            _treeViewCells = splitContainerCellsControls.Panel2.Controls["treeViewCells"] as TreeView;
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;

            base.Update(updateView);
        }
    }
}

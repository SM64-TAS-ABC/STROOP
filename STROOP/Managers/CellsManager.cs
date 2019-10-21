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
        public uint? TriangleAddress;

        private Button _buttonCellsBuildTree;
        private TreeView _treeViewCells;

        public CellsManager(string varFilePath, TabPage tabControl, WatchVariableFlowLayoutPanel watchVariablePanel)
            : base(varFilePath, watchVariablePanel)
        {
            SplitContainer splitContainerCells = tabControl.Controls["splitContainerCells"] as SplitContainer;
            SplitContainer splitContainerCellsControls = splitContainerCells.Panel1.Controls["splitContainerCellsControls"] as SplitContainer;

            _buttonCellsBuildTree = splitContainerCellsControls.Panel1.Controls["buttonCellsBuildTree"] as Button;
            _buttonCellsBuildTree.Click += (sender, e) => BuildTree();

            _treeViewCells = splitContainerCellsControls.Panel2.Controls["treeViewCells"] as TreeView;
        }

        private void BuildTree()
        {
            _treeViewCells.Nodes.Clear();
            _treeViewCells.Nodes.Add(GetTreeNodeForPartition(true));
            _treeViewCells.Nodes.Add(GetTreeNodeForPartition(false));
        }

        private TreeNode GetTreeNodeForPartition(bool staticPartition)
        {
            string name = staticPartition ? "Static Cells" : "Dynamic Cells";
            uint address = staticPartition ? 0x8038BE98 : 0x8038D698;
            TreeNode node = new TreeNode(name);
            for (int z = 0; z < 16; z++)
            {
                node.Nodes.Add(GetTreeNodeForZ(address, z));
            }
            return node;
        }

        private TreeNode GetTreeNodeForZ(uint address, int z)
        {
            int lowerBound = -8192 + z * 1024;
            int upperBound = lowerBound + 1024;
            string name = "z = " + z + " (" + lowerBound + " < z < " + upperBound + ")";
            TreeNode node = new TreeNode(name);
            for (int x = 0; x < 16; x++)
            {
                node.Nodes.Add(GetTreeNodeForX(address, z, x));
            }
            return node;
        }

        private TreeNode GetTreeNodeForX(uint address, int z, int x)
        {
            int lowerBound = -8192 + x * 1024;
            int upperBound = lowerBound + 1024;
            string name = "x = " + x + " (" + lowerBound + " < x < " + upperBound + ")";
            TreeNode node = new TreeNode(name);
            for (int type = 0; type < 3; type++)
            {
                node.Nodes.Add(GetTreeNodeForType(address, z, x, type));
            }
            return node;
        }

        private TreeNode GetTreeNodeForType(uint address, int z, int x, int type)
        {
            string name = address + " " + z + " " + x + " " + type;
            return new TreeNode(name);
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;

            base.Update(updateView);
        }
    }
}

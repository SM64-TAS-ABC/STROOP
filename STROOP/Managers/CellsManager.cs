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
            uint partitionAddress = staticPartition ? 0x8038BE98 : 0x8038D698;
            TreeNode node = new TreeNode(name);
            for (int z = 0; z < 16; z++)
            {
                node.Nodes.Add(GetTreeNodeForZ(partitionAddress, z));
            }
            return node;
        }

        private TreeNode GetTreeNodeForZ(uint partitionAddress, int z)
        {
            int lowerBound = -8192 + z * 1024;
            int upperBound = lowerBound + 1024;
            string name = "z = " + z + " (" + lowerBound + " < z < " + upperBound + ")";
            TreeNode node = new TreeNode(name);
            for (int x = 0; x < 16; x++)
            {
                node.Nodes.Add(GetTreeNodeForX(partitionAddress, z, x));
            }
            return node;
        }

        private TreeNode GetTreeNodeForX(uint partitionAddress, int z, int x)
        {
            int lowerBound = -8192 + x * 1024;
            int upperBound = lowerBound + 1024;
            string name = "x = " + x + " (" + lowerBound + " < x < " + upperBound + ")";
            TreeNode node = new TreeNode(name);
            for (int type = 0; type < 3; type++)
            {
                node.Nodes.Add(GetTreeNodeForType(partitionAddress, z, x, type));
            }
            return node;
        }

        private TreeNode GetTreeNodeForType(uint partitionAddress, int z, int x, int type)
        {
            int typeSize = 2 * 4;
            int xSize = 3 * typeSize;
            int zSize = 16 * xSize;
            uint address = (uint)(partitionAddress + z * zSize + x * xSize + type * typeSize);
            address = Config.Stream.GetUInt32(address);

            string name = type == 0 ? "Floors" : type == 1 ? "Ceilings" : "Walls";
            TreeNode node = new TreeNode(name);
            while (address != 0)
            {
                uint triAddress = Config.Stream.GetUInt32(address + 4);
                string triAddressString = HexUtilities.FormatValue(triAddress);
                node.Nodes.Add(new TreeNode(triAddressString));
                address = Config.Stream.GetUInt32(address);
            }
            return node;
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;

            base.Update(updateView);
        }
    }
}

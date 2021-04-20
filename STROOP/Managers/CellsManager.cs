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
        public uint TriangleAddress;

        private Button _buttonCellsBuildTree;
        private TreeView _treeViewCells;

        public CellsManager(string varFilePath, TabPage tabControl, WatchVariableFlowLayoutPanel watchVariablePanel)
            : base(varFilePath, watchVariablePanel)
        {
            TriangleAddress = 0;

            SplitContainer splitContainerCells = tabControl.Controls["splitContainerCells"] as SplitContainer;
            SplitContainer splitContainerCellsControls = splitContainerCells.Panel1.Controls["splitContainerCellsControls"] as SplitContainer;

            _buttonCellsBuildTree = splitContainerCellsControls.Panel1.Controls["buttonCellsBuildTree"] as Button;
            _buttonCellsBuildTree.Click += (sender, e) => BuildTree();

            _treeViewCells = splitContainerCellsControls.Panel2.Controls["treeViewCells"] as TreeView;
            _treeViewCells.AfterSelect += (sender, e) => SetTriangleAddress();
        }

        private void SetTriangleAddress()
        {
            object tag = _treeViewCells.SelectedNode.Tag;
            TriangleAddress = tag is uint uintTag ? uintTag : 0;
        }

        private void BuildTree()
        {
            _treeViewCells.BeginUpdate();
            _treeViewCells.Nodes.Clear();
            _treeViewCells.Nodes.Add(GetTreeNodeForPartition(true));
            _treeViewCells.Nodes.Add(GetTreeNodeForPartition(false));
            _treeViewCells.EndUpdate();
        }

        private TreeNode GetTreeNodeForPartition(bool staticPartition)
        {
            uint partitionAddress = staticPartition ? TriangleConfig.StaticTrianglePartitionAddress : TriangleConfig.DynamicTrianglePartitionAddress;

            List<TreeNode> nodes = new List<TreeNode>();
            int sum = 0;
            for (int z = 0; z < 16; z++)
            {
                TreeNode subNode = GetTreeNodeForZ(partitionAddress, z);
                nodes.Add(subNode);
                sum += (int)subNode.Tag;
            }

            string name = (staticPartition ? "Static Triangles" : "Dynamic Triangles") + " [" + sum + "]";
            TreeNode node = new TreeNode(name);
            node.Tag = sum;
            node.Nodes.AddRange(nodes.ToArray());
            return node;
        }

        private TreeNode GetTreeNodeForZ(uint partitionAddress, int z)
        {
            int lowerBound = -8192 + z * 1024;
            int upperBound = lowerBound + 1024;

            List<TreeNode> nodes = new List<TreeNode>();
            int sum = 0;
            for (int x = 0; x < 16; x++)
            {
                TreeNode subNode = GetTreeNodeForX(partitionAddress, z, x);
                nodes.Add(subNode);
                sum += (int)subNode.Tag;
            }

            string name = "Z:" + z + " (" + lowerBound + " < z < " + upperBound + ") [" + sum + "]";
            TreeNode node = new TreeNode(name);
            node.Tag = sum;
            node.Nodes.AddRange(nodes.ToArray());
            return node;
        }

        private TreeNode GetTreeNodeForX(uint partitionAddress, int z, int x)
        {
            int lowerBound = -8192 + x * 1024;
            int upperBound = lowerBound + 1024;

            List<TreeNode> nodes = new List<TreeNode>();
            int sum = 0;
            for (int type = 0; type < 3; type++)
            {
                TreeNode subNode = GetTreeNodeForType(partitionAddress, z, x, type);
                nodes.Add(subNode);
                sum += (int)subNode.Tag;
            }

            string name = "X:" + x + " (" + lowerBound + " < x < " + upperBound + ") [" + sum + "]";
            TreeNode node = new TreeNode(name);
            node.Tag = sum;
            node.Nodes.AddRange(nodes.ToArray());
            return node;
        }

        private TreeNode GetTreeNodeForType(uint partitionAddress, int z, int x, int type)
        {
            int typeSize = 2 * 4;
            int xSize = 3 * typeSize;
            int zSize = 16 * xSize;
            uint address = (uint)(partitionAddress + z * zSize + x * xSize + type * typeSize);
            address = Config.Stream.GetUInt(address);

            List<TreeNode> nodes = new List<TreeNode>();
            while (address != 0)
            {
                uint triAddress = Config.Stream.GetUInt(address + 4);
                short y1 = TriangleOffsetsConfig.GetY1(triAddress);
                string triAddressString = HexUtilities.FormatValue(triAddress) + " (y1 = " + y1 + ")";
                TreeNode subNode = new TreeNode(triAddressString);
                subNode.Tag = triAddress;
                nodes.Add(subNode);
                address = Config.Stream.GetUInt(address);
            }

            string name = (type == 0 ? "Floors" : type == 1 ? "Ceilings" : "Walls") + " [" + nodes.Count + "]";
            TreeNode node = new TreeNode(name);
            node.Tag = nodes.Count;
            node.Nodes.AddRange(nodes.ToArray());
            return node;
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;

            base.Update(updateView);
        }
    }
}

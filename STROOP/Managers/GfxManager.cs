using STROOP.Controls;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using STROOP.Structs.Configurations;
using STROOP.Utilities;

namespace STROOP.Managers
{
    /**
    * The Gfx tree is responsible for drawing everything in SM64 except HUD text
    * Nodes that actually draw things are 'DisplayLists' for static things (like level geometry) or 'GeoLayout scripts'
    * for more complex things (like water rectangles, snow, painting wobble).
    * Other nodes affect everything below it. There is a child selector that ensures only one room in the castle / BBH / HMC is drawn at a time,
    * there are nodes setting up a camera, rotationg / scaling models, handling animation, all kinds of stuff
    * This manager makes it easy to browse all the nodes and edit them
    */
    public class GfxManager : DataManager
    {
        Control _tabControl;
        TreeView _treeView;
        public GfxNode SelectedNode;
        List<WatchVariableControl> SpecificVariables;
        WatchVariablePanel _watchVariablePanel;
        RichTextBox _outputTextBox;

        public GfxManager(Control tabControl, List<WatchVariableControlPrecursor> variables, WatchVariablePanel watchVariablePanel)
            : base(variables, watchVariablePanel)
        {
            var left = tabControl.Controls["splitContainerGfxLeft"] as SplitContainer;
            var right = left.Panel2.Controls["splitContainerGfxright"] as SplitContainer;
            var middle = right.Panel1.Controls["splitContainerGfxmiddle"] as SplitContainer;
            var refreshButtonRoot = middle.Panel1.Controls["buttonGfxRefresh"] as Button;
            var refreshButtonObject = middle.Panel1.Controls["buttonGfxRefreshObject"] as Button;
            var dumpButton = middle.Panel1.Controls["buttonGfxDumpDisplayList"] as Button;

            _outputTextBox = right.Panel2.Controls["richTextBoxGfx"] as RichTextBox;
            _outputTextBox.Font = new System.Drawing.Font("Courier New", 8);
            _outputTextBox.ForeColor = System.Drawing.Color.Black;

            _watchVariablePanel = watchVariablePanel;
            _treeView = left.Panel1.Controls["treeViewGfx"] as TreeView;
            _treeView.AfterSelect += _treeView_AfterSelect;
            refreshButtonRoot.Click += RefreshButton_Click;
            refreshButtonObject.Click += RefreshButtonObject_Click;
            dumpButton.Click += DumpButton_Click;
            _tabControl = tabControl;

            foreach (var factory in GfxNode.GetCommonVariables())
            {
                watchVariablePanel.AddVariable(factory.CreateWatchVariableControl());
            }

            SpecificVariables = new List<WatchVariableControl>();

        }

        // Dump the display list of the currently selected gfx node (if applicable)
        // This can contain vertices and triangles, but also draw settings like lighting and fog
        private void DumpButton_Click(object sender, EventArgs e)
        {
            if (SelectedNode != null && SelectedNode is GfxDisplayList)
            {
                var address = Config.Stream.GetUInt32(SelectedNode.address + 0x14);
                _outputTextBox.Text = Fast3DDecoder.DecodeList(Fast3DDecoder.DecodeSegmentedAddress(address));

            } else
            {
                MessageBox.Show("Select a display list node first");
            }
        }

        // The variables in the first 0x14 bytes in a GFX node are common, but after that there are type-specific variables
        void UpdateSpecificVariables(GfxNode node)
        {
            _watchVariablePanel.RemoveVariables(SpecificVariables);
            SpecificVariables.Clear();
            if (node != null) foreach (var factory in node.GetTypeSpecificVariables())
                {
                    SpecificVariables.Add(factory.CreateWatchVariableControl());
                }
            _watchVariablePanel.AddVariables(SpecificVariables);
        }

        // Build a GFX tree for every object that is selected in the object slot view
        private void RefreshButtonObject_Click(object sender, EventArgs e)
        {
            
            var list = Config.ObjectSlotsManager.SelectedSlotsAddresses;
            if (list != null && list.Count>0)
            {
                _treeView.Nodes.Clear();
                foreach (var address in list)
                {
                    AddToTreeView(address);
                }
                ExpandNodesUpTo(_treeView.Nodes, 4);
            }
            else
            {
                MessageBox.Show("Select at least one object slot.");
            }
        }

        /**
         * When selecting a node, ensure the variable containers are related to that node
         */
        private void _treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            GfxNode node = (GfxNode) e.Node.Tag;
            SelectedNode = node;
            UpdateSpecificVariables(SelectedNode);
        }

        /**
         * When refresh is clicked, the old GFX tree is discarded and a new one is read
         */
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            _treeView.Nodes.Clear();

            // A pointer to the root node of the GFX tree is stored at a fixed address
            AddToTreeView(Config.Stream.GetUInt32(Config.SwitchRomVersion(0x33B910, 0x33A5A0)));
            ExpandNodesUpTo(_treeView.Nodes, 4);
        }

        // By default, a new TreeNode is collapsed. If you expand all, then the treeview will be overwhelmed with 240 object nodes
        // This function allows to expand only the nodes a certain amount of levels deep while keeping the deeper ones collapsed
        private void ExpandNodesUpTo(TreeNodeCollection nodes, int level) {
            if (level <= 0) return;

            foreach (TreeNode node in nodes)
            {
                node.Expand();
                ExpandNodesUpTo(node.Nodes, level-1);
            }
        }

        public void AddToTreeView(uint rootAddress)
        {
            var root = GfxNode.ReadGfxNode(rootAddress);
            _treeView.Nodes.Add(GfxToTreeNode(root));
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;
            base.Update(true);
        }

        /*
         * Recursively converts a tree of GfxNodes to a tree of TreeNodes so that they can be displayed in the tree viewer
         */
        public TreeNode GfxToTreeNode(GfxNode node)
        {
            // Should only happen when memory is invalid (for example when the US setting is used on a JP ROM)
            if (node == null) return new TreeNode("Invalid Gfx Node"); 

            TreeNode res = new TreeNode(node.Name, node.children.Select(x => GfxToTreeNode(x)).ToArray());
            res.Tag = node;
            return res;
        }
    }

    public class GfxNode
    {
        private const int maxSiblings = 1000; //Siblings are stored as a circular list. This limit prevent infinite loops on malformed memory.
        public virtual string Name { get { return "GFX node"; } } //This name is overridden by all the sub classes corresponding 
        public uint address;
        public List<GfxNode> children;

        public static GfxNode ReadGfxNode(uint address)
        {
            if (address < 0x80000000u || address > 0x80800000u)
            {
                return null;
            }

            var type = Config.Stream.GetUInt16(address + 0x00);
            GfxNode res;

            switch (type)
            {
                case 0x001: res = new GfxRootnode(); break;
                case 0x002: res = new GfxScreenSpace(); break;
                case 0x004: res = new GfxMasterList(); break;
                case 0x00A: res = new GfxGroupParent(); break;
                case 0x00B: res = new GfxHeightGate(); break;
                case 0x015: res = new GfxUnknown15(); break;
                case 0x016: res = new GfxUnknown16(); break;
                case 0x017: res = new GfxRotationNode(); break;
                case 0x018: res = new GfxGameObject(); break;
                case 0x019: res = new GfxTranslationNode(); break;
                case 0x01A: res = new GfxMenuModel(); break;
                case 0x01B: res = new GfxDisplayList(); break;
                case 0x01C: res = new GfxScalingNode(); break;
                case 0x028: res = new GfxShadowNode(); break;
                case 0x029: res = new GfxObjectParent(); break;
                case 0x103: res = new GfxProjection3D(); break;
                case 0x10C: res = new GfxChildSelector(); break;
                case 0x114: res = new GfxCamera(); break;
                case 0x12A: res = new GfxGeoLayoutScript(); break;
                case 0x12C: res = new GfxBackgroundImage(); break;
                case 0x12E: res = new GfxHeldObject(); break;
                default: res = new GfxNode(); break;
            }
            res.address = address;
            res.children = new List<GfxNode>();

            uint childAddress;
            
            if (type == 0x018 || type == 0x029)
            {
                // For some reason, the object parent has a null pointer as a child inbetween frames,
                // but during updatng it temporarily sets it to the pointer at offset 0x14
                // Object nodes also do something like that
                childAddress = Config.Stream.GetUInt32(address + 0x14);
            }
            else
            {
                childAddress = Config.Stream.GetUInt32(address + 0x10);  //offset 0x10 = child pointer
            }

            if (childAddress != 0)
            {
                //Traverse the circularly linked list of siblings until the first child is seen again
                var currentAddress = childAddress;
                for (int i = 0; i < maxSiblings; i++)
                {
                    res.children.Add(ReadGfxNode(currentAddress));
                    currentAddress = Config.Stream.GetUInt32(currentAddress + 0x08); //offset 0x08 = next pointer 
                    if (currentAddress == childAddress) break;
                }
            }

            return res;
        }

        public static List<WatchVariableControlPrecursor> GetCommonVariables()
        {
            var res = new List<WatchVariableControlPrecursor>();
            res.Add(gfxProperty("Type", "ushort", 0x00));
            res.Add(gfxProperty("Active", "ushort", 0x02, Structs.WatchVariableSubclass.Boolean, 0x01));
            res.Add(gfxProperty("Bit 1", "ushort", 0x02, Structs.WatchVariableSubclass.Boolean, 0x02));
            res.Add(gfxProperty("Billboard object", "ushort", 0x02, Structs.WatchVariableSubclass.Boolean, 0x04));
            res.Add(gfxProperty("Bit 3", "ushort", 0x02, Structs.WatchVariableSubclass.Boolean, 0x08));
            res.Add(gfxProperty("Invisible object", "ushort", 0x02, Structs.WatchVariableSubclass.Boolean, 0x10));
            res.Add(gfxProperty("Bit 5", "ushort", 0x02, Structs.WatchVariableSubclass.Boolean, 0x20));
            res.Add(gfxProperty("Is object", "ushort", 0x02, Structs.WatchVariableSubclass.Boolean, 0x20));
            res.Add(gfxProperty("Previous", "uint", 0x04));
            res.Add(gfxProperty("Next", "uint", 0x08));
            res.Add(gfxProperty("Parent", "uint", 0x0C));
            res.Add(gfxProperty("Child", "uint", 0x10));
            res.Add(gfxProperty("14", "uint", 0x14)); //Placeholders, used for investigating type-specific variables
            res.Add(gfxProperty("18", "uint", 0x18));
            res.Add(gfxProperty("1C", "uint", 0x1C));
            res.Add(gfxProperty("20", "uint", 0x20));
            return res;

        }

        // Wrapper to make defining variables easier
        protected static WatchVariableControlPrecursor gfxProperty(string name, string type, uint offset, 
            Structs.WatchVariableSubclass subclass = Structs.WatchVariableSubclass.Number, uint? mask = null)
        {
            var col = (offset <= 0x13) ? Color.Beige : Color.PowderBlue;
            var wv = new WatchVariable(type, null, Structs.BaseAddressTypeEnum.GfxNode, offset, offset, offset, offset, mask);
            var wvp = new WatchVariableControlPrecursor(name, wv, subclass, col, 
                type == "uint" || type == "ushort", false, null, new List<Structs.VariableGroup>());
            return wvp;
        }

        // If there are type specific variables, this should be overridden 
        public virtual List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            return new List<WatchVariableControlPrecursor>();
        }
    }

    internal class GfxChildSelector : GfxNode
    {
        public override string Name { get { return "Child selector"; } }

        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            var res = new List<WatchVariableControlPrecursor>();
            res.Add(gfxProperty("Selection function", "uint", 0x14));
            res.Add(gfxProperty("Selected child", "ushort", 0x1E));
            return res;
        }
    }

    internal class GfxBackgroundImage : GfxNode
    {
        public override string Name { get { return "Background image"; } }
    }

    internal class GfxHeldObject : GfxNode
    {
        public override string Name { get { return "Held object"; } }
        //function gfxFunction  0x14
        //int marioOffset  0x18        memory offset from marioData to check
        //void* heldObj      0x1c        another struct
        //short[3] position     0x20,2,4
        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            var res = new List<WatchVariableControlPrecursor>();
            res.Add(gfxProperty("Function", "uint", 0x14));
            res.Add(gfxProperty("Mario offset", "int", 0x18));
            res.Add(gfxProperty("Held object", "uint", 0x1C));
            res.Add(gfxProperty("Position x", "short", 0x20));
            res.Add(gfxProperty("Position y", "short", 0x22));
            res.Add(gfxProperty("Position z", "short", 0x24));
            return res;
        }
    }

    internal class GfxGeoLayoutScript : GfxNode
    {
        public override string Name { get { return "Geo Layout script"; } }

        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            var res = new List<WatchVariableControlPrecursor>();
            res.Add(gfxProperty("Function", "uint", 0x14));
            return res;
        }
    }

    internal class GfxCamera : GfxNode
    {
        public override string Name { get { return "Camera"; } }
        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            var res = new List<WatchVariableControlPrecursor>();
            res.Add(gfxProperty("Update function", "uint", 0x14));
            res.Add(gfxProperty("X from", "float", 0x1C));
            res.Add(gfxProperty("X from", "float", 0x20));
            res.Add(gfxProperty("Z from", "float", 0x24));
            res.Add(gfxProperty("X to", "float", 0x28));
            res.Add(gfxProperty("Y to", "float", 0x2C));
            res.Add(gfxProperty("Z to", "float", 0x30));
            return res;
        }
    }

    internal class GfxProjection3D : GfxNode
    {
        public override string Name { get { return "Projection 3D"; } }
        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            var res = new List<WatchVariableControlPrecursor>();
            res.Add(gfxProperty("Update function", "uint", 0x14));
            res.Add(gfxProperty("Fov", "float", 0x1C));
            res.Add(gfxProperty("Z clip near", "short", 0x20));
            res.Add(gfxProperty("Z clip far", "short", 0x22));
            return res;
        }
    }

    internal class GfxObjectParent : GfxNode
    {
        public override string Name { get { return "Object parent"; } }
        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            var res = new List<WatchVariableControlPrecursor>();
            res.Add(gfxProperty("Gfx tree root", "uint", 0x14));
            return res;
        }
    }

    internal class GfxShadowNode : GfxNode
    {
        public override string Name { get { return "Shadow"; } }
        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            var res = new List<WatchVariableControlPrecursor>();
            res.Add(gfxProperty("radius", "short", 0x14));
            res.Add(gfxProperty("opacity", "byte", 0x16));
            res.Add(gfxProperty("type", "byte", 0x17));
            return res;
        }
    }

    internal class GfxScalingNode : GfxNode
    {
        public override string Name { get { return "Scaling node"; } }
        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            var res = new List<WatchVariableControlPrecursor>();
            res.Add(gfxProperty("Scale", "float", 0x18));
            return res;
        }
    }

    // Temporary name. This is used to draw the "S U P E R M A R I O" in debug level select
    internal class GfxMenuModel : GfxNode
    {
        public override string Name { get { return "Menu model"; } }
        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            var res = new List<WatchVariableControlPrecursor>();
            res.Add(gfxProperty("Segmented address", "uint", 0x14));
            res.Add(gfxProperty("X offset", "short", 0x18)); //todo: check these
            res.Add(gfxProperty("Y offset", "short", 0x1A));
            return res;
        }
    }

    internal class GfxTranslationNode : GfxNode
    {
        public override string Name { get { return "Translation"; } }
        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            var res = new List<WatchVariableControlPrecursor>();
            res.Add(gfxProperty("Segmented address", "uint", 0x14));
            res.Add(gfxProperty("X", "short", 0x18));
            res.Add(gfxProperty("Y", "short", 0x1A));
            res.Add(gfxProperty("Z", "short", 0x1C));
            return res;
        }
    }

    internal class GfxGameObject : GfxNode
    {
        public override string Name { get { return "Game object"; } }
        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            var res = new List<WatchVariableControlPrecursor>();
            res.Add(gfxProperty("Actual child", "uint", 0x14));
            return res;
        }
    }

    internal class GfxRotationNode : GfxNode
    {
        public override string Name { get { return "Rotation"; } }

        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            var res = new List<WatchVariableControlPrecursor>();
            res.Add(gfxProperty("Segmented address", "uint", 0x14));
            res.Add(gfxProperty("Angle x", "short", 0x18)); //Todo: make these angle types
            res.Add(gfxProperty("Angle y", "short", 0x1A));
            res.Add(gfxProperty("Angle z", "short", 0x1C));
            return res;
        }
    }

    internal class GfxUnknown16 : GfxNode
    {
        public override string Name { get { return "Unknown 0x16"; } }
    }

    internal class GfxUnknown15 : GfxNode
    {
        public override string Name { get { return "Unknown 0x15"; } }
    }

    internal class GfxHeightGate : GfxNode
    {
        public override string Name { get { return "Height gate"; } }
    }

    internal class GfxMasterList : GfxNode
    {
        public override string Name { get { return "Master list"; } }
    }

    // Possibly some extra things?
    internal class GfxGroupParent : GfxNode
    {
        public override string Name { get { return "Group"; } }
    }

    internal class GfxScreenSpace : GfxNode
    {
        public override string Name { get { return "Screenspace"; } }
    }

    internal class GfxRootnode : GfxNode
    {
        public override string Name { get { return "Root"; } }
        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            var res = new List<WatchVariableControlPrecursor>();
            res.Add(gfxProperty("Some short", "short", 0x14));
            res.Add(gfxProperty("Screen xoffset", "short", 0x16));
            res.Add(gfxProperty("Screen yoffset", "short", 0x18));
            res.Add(gfxProperty("Screen half width", "short", 0x1A));
            res.Add(gfxProperty("Screen half height", "short", 0x1C));
            return res;
        }
    }

    internal class GfxDisplayList : GfxNode
    {
        public override string Name { get { return "Display List"; } }
        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            var res = new List<WatchVariableControlPrecursor>();
            res.Add(gfxProperty("Segmented address", "uint", 0x14));
            return res;
        }
    }
}

using STROOP.Controls;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using STROOP.Structs.Configurations;
using STROOP.Structs;
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
        WatchVariableFlowLayoutPanel _watchVariablePanel;
        RichTextBox _outputTextBox;

        public GfxManager(Control tabControl, List<WatchVariableControlPrecursor> variables, WatchVariableFlowLayoutPanel watchVariablePanel)
            : base(variables, watchVariablePanel)
        {
            SplitContainer left = tabControl.Controls["splitContainerGfxLeft"] as SplitContainer;
            SplitContainer right = left.Panel2.Controls["splitContainerGfxright"] as SplitContainer;
            SplitContainer middle = right.Panel1.Controls["splitContainerGfxmiddle"] as SplitContainer;
            Button refreshButtonRoot = middle.Panel1.Controls["buttonGfxRefresh"] as Button;
            Button refreshButtonObject = middle.Panel1.Controls["buttonGfxRefreshObject"] as Button;
            Button dumpButton = middle.Panel1.Controls["buttonGfxDumpDisplayList"] as Button;
            Button hitboxViewButton = middle.Panel1.Controls["buttonGfxHitboxHack"] as Button;

            _outputTextBox = right.Panel2.Controls["richTextBoxGfx"] as RichTextBox;
            _outputTextBox.Font = new System.Drawing.Font("Courier New", 8);
            _outputTextBox.ForeColor = System.Drawing.Color.Black;

            _watchVariablePanel = watchVariablePanel;
            _treeView = left.Panel1.Controls["treeViewGfx"] as TreeView;
            _treeView.AfterSelect += _treeView_AfterSelect;
            refreshButtonRoot.Click += RefreshButton_Click;
            refreshButtonObject.Click += RefreshButtonObject_Click;
            dumpButton.Click += DumpButton_Click;
            hitboxViewButton.Click += HitboxView_Click;
            _tabControl = tabControl;

            foreach (WatchVariableControlPrecursor precursor in GfxNode.GetCommonVariables())
            {
                watchVariablePanel.AddVariable(precursor.CreateWatchVariableControl());
            }

            SpecificVariables = new List<WatchVariableControl>();

        }

        // Inject code that shows hitboxes in-game
        // Note: a bit ugly at the moment. Hack folder is hardcoded instead of taken from Config file,
        // and it's put here in the GFX tab by a lack of a better place. The hacks in the hack tab are
        // constantly reapplied when memory is changed, which doesn't work with this hack which initializes 
        // variables that are later changed.
        private void HitboxView_Click(object sender, EventArgs args)
        {
            RomHack hck = null;
            try
            {
                if (RomVersionConfig.Version == Structs.RomVersion.US)
                {
                    hck = new RomHack("Resources\\Hacks\\HitboxViewU.hck", "HitboxView");
                }
                else if (RomVersionConfig.Version == Structs.RomVersion.JP)
                {
                    hck = new RomHack("Resources\\Hacks\\HitboxViewJ.hck", "HitboxView");
                }
                else
                {
                    MessageBox.Show("Hitbox view hack only available on US and JP versions");
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("Hack files are missing in Resources\\Hacks folder");
            }
            hck?.LoadPayload();
        }

        // Dump the display list of the currently selected gfx node (if applicable)
        // This can contain vertices and triangles, but also draw settings like lighting and fog
        private void DumpButton_Click(object sender, EventArgs e)
        {
            if (SelectedNode != null && SelectedNode is GfxDisplayList)
            {
                uint address = Config.Stream.GetUInt32(SelectedNode.Address + 0x14);
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
            if (node != null)
            {
                foreach (WatchVariableControlPrecursor precursor in node.GetTypeSpecificVariables())
                {
                    SpecificVariables.Add(precursor.CreateWatchVariableControl());
                }
            }
            _watchVariablePanel.AddVariables(SpecificVariables);
        }

        // Build a GFX tree for every object that is selected in the object slot view
        private void RefreshButtonObject_Click(object sender, EventArgs e)
        {
            
            HashSet<uint> list = Config.ObjectSlotsManager.SelectedSlotsAddresses;
            if (list != null && list.Count>0)
            {
                _treeView.Nodes.Clear();
                foreach (uint address in list)
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

            // A pointer to the root node of the GFX tree is stored at offset 0x04 in a certain struct
            var StructWithGfxRoot = Config.Stream.GetUInt32(RomVersionConfig.Switch(0x32DDCC, 0x32CE6C));

            if (StructWithGfxRoot > 0x80000000u)
            {
                AddToTreeView(Config.Stream.GetUInt32(StructWithGfxRoot + 0x04));
            }

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
            GfxNode root = GfxNode.ReadGfxNode(rootAddress);
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

            TreeNode res = new TreeNode(node.Name, node.Children.Select(x => GfxToTreeNode(x)).ToArray());
            res.Tag = node;
            return res;
        }
    }

    public class GfxNode
    {
        private const int _maxSiblings = 1000; //Siblings are stored as a circular list. This limit prevent infinite loops on malformed memory.
        public virtual string Name { get { return "GFX node"; } } //This name is overridden by all the sub classes corresponding 
        public uint Address;
        public List<GfxNode> Children;

        public static GfxNode ReadGfxNode(uint address)
        {
            if (address < 0x80000000u || address > 0x80800000u)
            {
                return null;
            }

            ushort type = Config.Stream.GetUInt16(address + 0x00);
            GfxNode res;

            switch (type)
            {
                case 0x001: res = new GfxRootnode(); break;
                case 0x002: res = new GfxScreenSpace(); break;
                case 0x004: res = new GfxMasterList(); break;
                case 0x00A: res = new GfxGroupParent(); break;
                case 0x00B: res = new GfxHeightGate(); break;
                case 0x015: res = new GfxUnknown15(); break;
                case 0x016: res = new GfxTranslatedModel(); break;
                case 0x017: res = new GfxRotationNode(); break;
                case 0x018: res = new GfxGameObject(); break;
                case 0x019: res = new GfxTranslationNode(); break;
                case 0x01A: res = new GfxBillboard(); break;
                case 0x01B: res = new GfxDisplayList(); break;
                case 0x01C: res = new GfxScalingNode(); break;
                case 0x028: res = new GfxShadowNode(); break;
                case 0x029: res = new GfxObjectParent(); break;
                    //Todo: add 0x2F
                case 0x103: res = new GfxProjection3D(); break;
                case 0x10C: res = new GfxChildSelector(); break;
                case 0x114: res = new GfxCamera(); break;
                case 0x12A: res = new GfxGeoLayoutScript(); break;
                case 0x12C: res = new GfxBackgroundImage(); break;
                case 0x12E: res = new GfxHeldObject(); break;
                default: res = new GfxNode(); break;
            }
            res.Address = address;
            res.Children = new List<GfxNode>();

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
                uint currentAddress = childAddress;
                for (int i = 0; i < _maxSiblings; i++)
                {
                    res.Children.Add(ReadGfxNode(currentAddress));
                    currentAddress = Config.Stream.GetUInt32(currentAddress + 0x08); //offset 0x08 = next pointer 
                    if (currentAddress == childAddress) break;
                }
            }

            return res;
        }

        public static List<WatchVariableControlPrecursor> GetCommonVariables()
        {
            List<WatchVariableControlPrecursor> precursors = new List<WatchVariableControlPrecursor>();
            precursors.Add(gfxProperty("Type", "ushort", 0x00));
            precursors.Add(gfxProperty("Active", "ushort", 0x02, Structs.WatchVariableSubclass.Boolean, 0x01));
            precursors.Add(gfxProperty("Bit 1", "ushort", 0x02, Structs.WatchVariableSubclass.Boolean, 0x02));
            precursors.Add(gfxProperty("Billboard object", "ushort", 0x02, Structs.WatchVariableSubclass.Boolean, 0x04));
            precursors.Add(gfxProperty("Bit 3", "ushort", 0x02, Structs.WatchVariableSubclass.Boolean, 0x08));
            precursors.Add(gfxProperty("Invisible object", "ushort", 0x02, Structs.WatchVariableSubclass.Boolean, 0x10));
            precursors.Add(gfxProperty("Is object", "ushort", 0x02, Structs.WatchVariableSubclass.Boolean, 0x20));
            precursors.Add(gfxProperty("List index", "byte", 0x02));   //note: not actually a byte, but the result of (short>>8)
            precursors.Add(gfxProperty("Previous", "uint", 0x04));
            precursors.Add(gfxProperty("Next", "uint", 0x08));
            precursors.Add(gfxProperty("Parent", "uint", 0x0C));
            precursors.Add(gfxProperty("Child", "uint", 0x10));
            return precursors;

        }

        // Wrapper to make defining variables easier
        protected static WatchVariableControlPrecursor gfxProperty(string name, string type, uint offset, 
            Structs.WatchVariableSubclass subclass = Structs.WatchVariableSubclass.Number, uint? mask = null)
        {
            Color color = (offset <= 0x13)
                ? ColorUtilities.GetColorFromString("Yellow")
                : ColorUtilities.GetColorFromString("LightBlue");
            WatchVariable watchVar = new WatchVariable(
                type,
                null /* specialType */,
                Structs.BaseAddressTypeEnum.GfxNode,
                null /* offsetUS */,
                null /* offsetJP */,
                null /* offsetPAL */,
                offset,
                mask);
            WatchVariableControlPrecursor precursor = new WatchVariableControlPrecursor(
                name,
                watchVar,
                subclass,
                color,
                null /* displayType */,
                null /* roundingLimit */,
                (type == "uint" || type == "ushort") ? true : (bool?)null,
                null /* invertBool */,
                null /* coordinate */,
                null /* isYaw */,
                new List<Structs.VariableGroup>());
            return precursor;
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
            List<WatchVariableControlPrecursor> precursors = new List<WatchVariableControlPrecursor>();
            precursors.Add(gfxProperty("Selection function", "uint", 0x14));
            precursors.Add(gfxProperty("Selected child", "ushort", 0x1E));
            return precursors;
        }
    }

    internal class GfxBackgroundImage : GfxNode
    {
        public override string Name { get { return "Background image"; } }
        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            var precursors = new List<WatchVariableControlPrecursor>();
            precursors.Add(gfxProperty("Draw function", "uint", 0x14));
            return precursors;
        }
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
            List<WatchVariableControlPrecursor> precursors = new List<WatchVariableControlPrecursor>();
            precursors.Add(gfxProperty("Function", "uint", 0x14));
            precursors.Add(gfxProperty("Mario offset", "int", 0x18));
            precursors.Add(gfxProperty("Held object", "uint", 0x1C));
            precursors.Add(gfxProperty("Position x", "short", 0x20));
            precursors.Add(gfxProperty("Position y", "short", 0x22));
            precursors.Add(gfxProperty("Position z", "short", 0x24));
            return precursors;
        }
    }

    internal class GfxGeoLayoutScript : GfxNode
    {
        // Todo: put these in external files and expand them
        public static readonly Dictionary<uint, string> DictionaryUS = new Dictionary<uint, string>
        {
            { 0x802D01E0, "Water flow pause controller" },
            { 0x802D1B70, "Waterfall drawer" },
            { 0x8029D924, "Transparency controller" },  //makes peach / toad / dust particles transparent
            { 0x802D104C, "Water rectangle drawer" },
            { 0x802D1CDC, "SSL Pyramid sand flow" },
            { 0x802761D0, "Snow controller" },
            { 0x802CD1E8, "Overlay?" },
            { 0x802D5D0C, "Painting wobble controller" },
            { 0x802D5B98, "Painting drawer" },
            { 0x80277B14, "Mirror Mario controller"}
        };

        public static readonly Dictionary<uint, string> DictionaryJP = new Dictionary<uint, string>
        {
            { 0x802CF700, "Waterflow pause controller" },
            { 0x802D1090, "Waterfall drawer" },
            { 0x8029D194, "Transparency controller" },
            { 0x802D11FC, "SSL Pyramid sand flow" },
            { 0x802D056C, "Water rectangle drawer" },
            { 0x80275C20, "Snow controller" },
            { 0x802CC708, "Overlay?" },
            { 0x802D522C, "Painting wobble controller" },
            { 0x802D50B8, "Painting drawer" },
            { 0x80277564, "Mirror Mario controller" }
        };

        public override string Name {
            get {
                var currentDict = RomVersionConfig.Version == Structs.RomVersion.US ? DictionaryUS : DictionaryJP;
                var function = Config.Stream.GetUInt32(Address + 0x14);
                if (currentDict.ContainsKey(function)) return currentDict[function];
                return "Geo Layout script";
            }
        }

        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            var precursors = new List<WatchVariableControlPrecursor>();
            precursors.Add(gfxProperty("Draw function", "uint", 0x14));
            precursors.Add(gfxProperty("Parameter 1", "ushort", 0x18));
            precursors.Add(gfxProperty("Parameter 2", "ushort", 0x1A));
            return precursors;
        }
    }

    internal class GfxCamera : GfxNode
    {
        public override string Name { get { return "Camera"; } }
        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            List<WatchVariableControlPrecursor> precursors = new List<WatchVariableControlPrecursor>();
            precursors.Add(gfxProperty("Update function", "uint", 0x14));
            precursors.Add(gfxProperty("X from", "float", 0x1C));
            precursors.Add(gfxProperty("X from", "float", 0x20));
            precursors.Add(gfxProperty("Z from", "float", 0x24));
            precursors.Add(gfxProperty("X to", "float", 0x28));
            precursors.Add(gfxProperty("Y to", "float", 0x2C));
            precursors.Add(gfxProperty("Z to", "float", 0x30));
            return precursors;
        }
    }

    internal class GfxProjection3D : GfxNode
    {
        public override string Name { get { return "Projection 3D"; } }
        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            List<WatchVariableControlPrecursor> precursors = new List<WatchVariableControlPrecursor>();
            precursors.Add(gfxProperty("Update function", "uint", 0x14));
            precursors.Add(gfxProperty("Fov", "float", 0x1C));
            precursors.Add(gfxProperty("Z clip near", "short", 0x20));
            precursors.Add(gfxProperty("Z clip far", "short", 0x22));
            return precursors;
        }
    }

    internal class GfxObjectParent : GfxNode
    {
        public override string Name { get { return "Object parent"; } }
        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            List<WatchVariableControlPrecursor> precursors = new List<WatchVariableControlPrecursor>();
            precursors.Add(gfxProperty("Temp child", "uint", 0x14));
            return precursors;
        }
    }

    internal class GfxShadowNode : GfxNode
    {
        public override string Name { get { return "Shadow"; } }
        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            List<WatchVariableControlPrecursor> precursors = new List<WatchVariableControlPrecursor>();
            precursors.Add(gfxProperty("Radius", "short", 0x14));
            precursors.Add(gfxProperty("Opacity", "byte", 0x16));
            precursors.Add(gfxProperty("Type", "byte", 0x17));
            return precursors;
        }
    }

    internal class GfxScalingNode : GfxNode
    {
        public override string Name { get { return "Scaling node"; } }
        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            List<WatchVariableControlPrecursor> precursors = new List<WatchVariableControlPrecursor>();
            precursors.Add(gfxProperty("Scale", "float", 0x18));
            return precursors;
        }
    }

    //For example Goomba body
    internal class GfxBillboard : GfxNode
    {
        public override string Name { get { return "Billboard"; } }
    }

    internal class GfxTranslationNode : GfxNode
    {
        public override string Name { get { return "Translation"; } }
        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            List<WatchVariableControlPrecursor> precursors = new List<WatchVariableControlPrecursor>();
            precursors.Add(gfxProperty("Display list", "uint", 0x14));
            precursors.Add(gfxProperty("X", "short", 0x18));
            precursors.Add(gfxProperty("Y", "short", 0x1A));
            precursors.Add(gfxProperty("Z", "short", 0x1C));
            return precursors;
        }
    }

    internal class GfxGameObject : GfxNode
    {
        public override string Name { get { return "Game object"; } }
        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            List<WatchVariableControlPrecursor> precursors = new List<WatchVariableControlPrecursor>();
            precursors.Add(gfxProperty("Actual child", "uint", 0x14));
            return precursors;
        }
    }

    internal class GfxRotationNode : GfxNode
    {
        public override string Name { get { return "Rotation"; } }

        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            List<WatchVariableControlPrecursor> precursors = new List<WatchVariableControlPrecursor>();
            precursors.Add(gfxProperty("Segmented address", "uint", 0x14));
            precursors.Add(gfxProperty("Angle x", "short", 0x18)); //Todo: make these angle types
            precursors.Add(gfxProperty("Angle y", "short", 0x1A));
            precursors.Add(gfxProperty("Angle z", "short", 0x1C));
            return precursors;
        }
    }

    // This is used to draw the "S U P E R M A R I O" in debug level select
    internal class GfxTranslatedModel : GfxNode
    {
        public override string Name { get { return "Menu model"; } }
        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            List<WatchVariableControlPrecursor> precursors = new List<WatchVariableControlPrecursor>();
            precursors.Add(gfxProperty("Segmented address", "uint", 0x14));
            precursors.Add(gfxProperty("X offset", "short", 0x18));
            precursors.Add(gfxProperty("Y offset", "short", 0x1A));
            precursors.Add(gfxProperty("Z offset", "short", 0x1C));
            return precursors;
        }
    }

    internal class GfxUnknown15 : GfxNode
    {
        public override string Name { get { return "Unknown 0x15"; } }
    }

    internal class GfxHeightGate : GfxNode
    {
        public override string Name { get { return "Height gate"; } }
        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            var precursors = new List<WatchVariableControlPrecursor>();
            precursors.Add(gfxProperty("Y min", "short", 0x14));
            precursors.Add(gfxProperty("Y max", "short", 0x16));
            precursors.Add(gfxProperty("Pointer 1", "uint", 0x18));

            return precursors;
        }
    }

    internal class GfxMasterList : GfxNode
    {
        public override string Name { get { return "Master list"; } }
        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            var precursors = new List<WatchVariableControlPrecursor>();
            precursors.Add(gfxProperty("Pointer 0", "uint", 0x14));
            precursors.Add(gfxProperty("Pointer 1", "uint", 0x18));
            precursors.Add(gfxProperty("Pointer 2", "uint", 0x1C));
            precursors.Add(gfxProperty("Pointer 3", "uint", 0x20));
            precursors.Add(gfxProperty("Pointer 4", "uint", 0x24));
            precursors.Add(gfxProperty("Pointer 5", "uint", 0x28));
            precursors.Add(gfxProperty("Pointer 6", "uint", 0x2C));
            precursors.Add(gfxProperty("Pointer 7", "uint", 0x30));
            precursors.Add(gfxProperty("Pointer 8", "uint", 0x34));
            precursors.Add(gfxProperty("Pointer 9", "uint", 0x3C));
            precursors.Add(gfxProperty("Pointer 10", "uint", 0x40));
            precursors.Add(gfxProperty("Pointer 11", "uint", 0x44));
            precursors.Add(gfxProperty("Pointer 12", "uint", 0x48));
            precursors.Add(gfxProperty("Pointer 13", "uint", 0x4C));
            precursors.Add(gfxProperty("Pointer 14", "uint", 0x50));
            precursors.Add(gfxProperty("Pointer 15", "uint", 0x54));
            return precursors;
        }
    }

    // Possibly some extra things?
    internal class GfxGroupParent : GfxNode
    {
        public override string Name { get { return "Group"; } }
    }

    internal class GfxScreenSpace : GfxNode
    {
        public override string Name { get { return "Screenspace"; } }
        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            var precursors = new List<WatchVariableControlPrecursor>();
            precursors.Add(gfxProperty("??? 0x14", "float", 0x14));
            precursors.Add(gfxProperty("??? 0x18", "uint", 0x18));
            return precursors;
        }
    }

    internal class GfxRootnode : GfxNode
    {
        public override string Name { get { return "Root"; } }
        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            List<WatchVariableControlPrecursor> precursors = new List<WatchVariableControlPrecursor>();
            precursors.Add(gfxProperty("Some short", "short", 0x14));
            precursors.Add(gfxProperty("Screen xoffset", "short", 0x16));
            precursors.Add(gfxProperty("Screen yoffset", "short", 0x18));
            precursors.Add(gfxProperty("Screen half width", "short", 0x1A));
            precursors.Add(gfxProperty("Screen half height", "short", 0x1C));
            return precursors;
        }
    }

    internal class GfxDisplayList : GfxNode
    {
        public override string Name { get { return "Display List"; } }
        public override List<WatchVariableControlPrecursor> GetTypeSpecificVariables()
        {
            List<WatchVariableControlPrecursor> precursors = new List<WatchVariableControlPrecursor>();
            precursors.Add(gfxProperty("Segmented address", "uint", 0x14));
            return precursors;
        }
    }
}

using STROOP.Controls;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using STROOP.Structs.Configurations;

namespace STROOP.Managers
{
    public class GfxManager : DataManager
    {
        Control _tabControl;
        TreeView _treeView;
        GfxNode currentNode;

        public GfxManager(Control tabControl, List<WatchVariableControlPrecursor> variables, WatchVariablePanel watchVariablePanel)
            : base(variables, watchVariablePanel)
        {
            var left = tabControl.Controls["splitContainerGfxLeft"] as SplitContainer;
            var right = left.Panel2.Controls["splitContainerGfxright"] as SplitContainer;
            var middle = right.Panel1.Controls["splitContainerGfxmiddle"] as SplitContainer;
            var output = right.Panel2.Controls["richTextBoxGfx"] as RichTextBox;
            var refreshButton = middle.Panel1.Controls["buttonGfxRefresh"] as Button;
            
            _treeView = left.Panel1.Controls["treeViewGfx"] as TreeView;
            _treeView.AfterSelect += _treeView_AfterSelect;
            refreshButton.Click += RefreshButton_Click;
            _tabControl = tabControl;

            var wv = new WatchVariable("uint", null, Structs.BaseAddressTypeEnum.Relative, 0x8032D5D4, 0x8032C694, null, null, null);
            var wvp = new WatchVariableControlPrecursor("Global timer", wv, Structs.WatchVariableSubclass.Number, System.Drawing.Color.Goldenrod, true, false, null, new List<Structs.VariableGroup>());
            var wvc = wvp.CreateWatchVariableControl();
            watchVariablePanel.AddVariable(wvc);
            watchVariablePanel.AddVariable(wvc);
            watchVariablePanel.AddVariable(wvc);
            watchVariablePanel.AddVariable(wvc);
            watchVariablePanel.AddVariable(wvc);
            watchVariablePanel.AddVariable(wvc);
            watchVariablePanel.AddVariable(wvc);
            watchVariablePanel.AddVariable(wvc);
            watchVariablePanel.AddVariable(wvc);
            watchVariablePanel.AddVariable(wvc);
            watchVariablePanel.AddVariable(wvc);
            watchVariablePanel.AddVariable(wvc);
            watchVariablePanel.AddVariable(wvc);
            watchVariablePanel.AddVariable(wvc);
            watchVariablePanel.AddVariable(wvc);
            watchVariablePanel.AddVariable(wvc);
        }

        private void _treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            GfxNode node = (GfxNode)e.Node.Tag;
            MessageBox.Show(node.name);
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            //Config.SwitchRomVersion(0x33A5A0, 0x33A5A0);
            var root = GfxNode.ReadGfxNode(Config.Stream.GetUInt32(0x33A5A0));
            _treeView.Nodes.Add("Test");
            _treeView.Nodes.Add(GfxToTreeNode(root));
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;
            base.Update(true);
        }

        public TreeNode GfxToTreeNode(GfxNode node)
        {
            if (node == null) return new TreeNode("Invalid Gfx Node");
            TreeNode res = new TreeNode(node.name, node.children.Select(x => GfxToTreeNode(x)).ToArray());
            res.Tag = node;
            return res;
        }
    }

    public class GfxNode
    {
        private const int maxSiblings = 1000; //To prevent infinite loops on malformed memory

        public string name = "GFX node";
        public uint address;
        GfxNode parent;
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
                case 0x00A: res = new GfxRoomObjectParent(); break;
                case 0x00B: res = new GfxHeightGate(); break;
                case 0x015: res = new GfxUnknown15(); break;
                case 0x016: res = new GfxUnknown16(); break;
                case 0x017: res = new GfxTransformNode(); break;
                case 0x018: res = new GfxGameObject(); break;
                case 0x019: res = new GfxAnimationNode(); break;
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
            var childAddress = Config.Stream.GetUInt32(address + 0x10);  //offset 0x10 = child pointer

            if (childAddress != 0)
            {
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

        public virtual List<WatchVariableControlPrecursor> getSpecialVars()
        {
            return new List<WatchVariableControlPrecursor>();
        }
    }

    internal class GfxChildSelector : GfxNode
    {
        new string name = "Child selector";
    }

    internal class GfxBackgroundImage : GfxNode
    {
        new string name = "Background image";
    }

    internal class GfxHeldObject : GfxNode
    {
        new string name = "Held object";
    }

    internal class GfxGeoLayoutScript : GfxNode
    {
        new string name = "Geo Layout script";
    }

    internal class GfxCamera : GfxNode
    {
        new string name = "Camera";
    }

    internal class GfxProjection3D : GfxNode
    {
        new string name = "Projection 3D";
    }

    internal class GfxObjectParent : GfxNode
    {
        new string name = "Object parent";
    }

    internal class GfxShadowNode : GfxNode
    {
        new string name = "Shadow";
    }

    internal class GfxScalingNode : GfxNode
    {
        new string name = "Scaling node";
    }

    internal class GfxMenuModel : GfxNode
    {
        new string name = "Menu model";
    }

    internal class GfxAnimationNode : GfxNode
    {
        new string name = "Animation";
    }

    internal class GfxGameObject : GfxNode
    {
        new string name = "Game object";
    }

    internal class GfxTransformNode : GfxNode
    {
        new string name = "Transformation";
    }

    internal class GfxUnknown16 : GfxNode
    {
        new string name = "Unknown 0x16";
    }

    internal class GfxUnknown15 : GfxNode
    {
        new string name = "Unknown 0x15";
    }

    internal class GfxHeightGate : GfxNode
    {
        new string name = "Height gate";
    }

    internal class GfxMasterList : GfxNode
    {
        new string name = "Master list";
    }

    internal class GfxRoomObjectParent : GfxNode
    {
        new string name = "Room/Object parent";
    }

    internal class GfxScreenSpace : GfxNode
    {
        new string name = "Screenspace";
    }

    internal class GfxRootnode : GfxNode
    {
        new string name = "Root";
    }

    internal class GfxDisplayList : GfxNode
    {
        new string name = "DisplayList";
    }
}

using STROOP.Controls;
using STROOP.Forms;
using STROOP.Script;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STROOP.Managers
{
    public class WarpManager : DataManager
    {
        private List<uint> _warpNodeAddresses;

        private static readonly List<VariableGroup> ALL_VAR_GROUPS =
            new List<VariableGroup>()
            {
                VariableGroup.Basic,
                VariableGroup.WarpNode,
            };

        private static readonly List<VariableGroup> VISIBLE_VAR_GROUPS =
            new List<VariableGroup>()
            {
                VariableGroup.Basic,
                VariableGroup.WarpNode,
            };

        public WarpManager(string varFilePath, TabPage tabPage, WatchVariableFlowLayoutPanel watchVariablePanel)
            : base(varFilePath, watchVariablePanel, ALL_VAR_GROUPS, VISIBLE_VAR_GROUPS)
        {
            _warpNodeAddresses = new List<uint>();

            /*
            SplitContainer splitContainer = tabPage.Controls["splitContainerScript"] as SplitContainer;
            SplitContainer splitContainerLeft = splitContainer.Panel1.Controls["splitContainerScriptLeft"] as SplitContainer;
            _checkBoxScriptRunContinuously = splitContainerLeft.Panel1.Controls["checkBoxScriptRunContinuously"] as CheckBox;
            _buttonScriptRunOnce = splitContainerLeft.Panel1.Controls["buttonScriptRunOnce"] as Button;
            _buttonScriptInstructions = splitContainerLeft.Panel1.Controls["buttonScriptInstructions"] as Button;
            _buttonScriptExamples = splitContainerLeft.Panel1.Controls["buttonScriptExamples"] as Button;
            _richTextBoxScript = splitContainerLeft.Panel2.Controls["richTextBoxScript"] as RichTextBoxEx;

            _script = new TokenScript();

            _checkBoxScriptRunContinuously.Click += (sender, e) =>
            {
                if (_checkBoxScriptRunContinuously.Checked)
                {
                    _script.SetScript(_richTextBoxScript.Text);
                }
                _script.SetIsEnabled(_checkBoxScriptRunContinuously.Checked);
                _richTextBoxScript.ReadOnly = _checkBoxScriptRunContinuously.Checked;
            };

            _buttonScriptRunOnce.Click += (sender, e) =>
            {
                _script.SetScript(_richTextBoxScript.Text);
                _script.Run();
            };

            _buttonScriptInstructions.Click += (sender, e) =>
            {
                InfoForm.ShowValue(
                    string.Join("\r\n", _instructions),
                    "Instructions",
                    "Instructions");
            };

            _buttonScriptExamples.ContextMenuStrip = new ContextMenuStrip();
            for (int i = 0; i < _exampleNames.Count; i++)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(_exampleNames[i]);
                string text = string.Join("\r\n", _exampleLines[i]);
                item.Click += (sender, e) => _richTextBoxScript.Text = text;
                _buttonScriptExamples.ContextMenuStrip.Items.Add(item);
            }
            _buttonScriptExamples.Click += (sender, e) =>
                _buttonScriptExamples.ContextMenuStrip.Show(Cursor.Position);
                */
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;

            List<uint> warpNodeAddresses = WatchVariableSpecialUtilities.GetWarpNodeAddresses();
            if (!Enumerable.SequenceEqual(warpNodeAddresses, _warpNodeAddresses))
            {
                RemoveVariableGroup(VariableGroup.WarpNode);
                AddVariables(GetWarpNodeVariables(warpNodeAddresses));
                _warpNodeAddresses = warpNodeAddresses;
            }

            base.Update(updateView);
        }

        private List<WatchVariableControl> GetWarpNodeVariables(List<uint> addresses)
        {
            List<WatchVariableControl> controls = new List<WatchVariableControl>();
            for (int i = 0; i < addresses.Count; i++)
            {
                uint address = addresses[i];
                controls.AddRange(GetWarpNodeVariables(address, i));
            }
            return controls;
        }

        private List<WatchVariableControl> GetWarpNodeVariables(uint address, int index)
        {
            List<string> names = new List<string>()
            {
                string.Format("Warp {0} ID", index),
                string.Format("Warp {0} Dest Level", index),
                string.Format("Warp {0} Dest Area", index),
                string.Format("Warp {0} Dest Node", index),
                string.Format("Warp {0} Object", index),
                string.Format("Warp {0} Next", index),
            };
            List<uint> offsets = new List<uint>()
            {
                address + 0x0,
                address + 0x1,
                address + 0x2,
                address + 0x3,
                address + 0x4,
                address + 0x8,
            };
            List<string> types = new List<string>()
            {
                "byte",
                "byte",
                "byte",
                "byte",
                "uint",
                "uint",
            };
            List<WatchVariableSubclass> subclasses = new List<WatchVariableSubclass>()
            {
                WatchVariableSubclass.Number,
                WatchVariableSubclass.Number,
                WatchVariableSubclass.Number,
                WatchVariableSubclass.Number,
                WatchVariableSubclass.Object,
                WatchVariableSubclass.Address,
            };
            List<bool?> useHexes = new List<bool?>()
            {
                true,
                null,
                null,
                true,
                null,
                null,
            };

            List<WatchVariableControl> controls = new List<WatchVariableControl>();
            for (int i = 0; i < 6; i++)
            {
                WatchVariable watchVar = new WatchVariable(
                    memoryTypeName: types[i],
                    specialType: null,
                    baseAddressType: BaseAddressTypeEnum.Relative,
                    offsetUS: null,
                    offsetJP: null,
                    offsetSH: null,
                    offsetDefault: offsets[i],
                    mask: null,
                    shift: null);
                WatchVariableControlPrecursor precursor = new WatchVariableControlPrecursor(
                    name: names[i],
                    watchVar: watchVar,
                    subclass: subclasses[i],
                    backgroundColor: null,
                    displayType: null,
                    roundingLimit: null,
                    useHex: useHexes[i],
                    invertBool: null,
                    isYaw: null,
                    coordinate: null,
                    groupList: new List<VariableGroup>() { VariableGroup.WarpNode });
                controls.Add(precursor.CreateWatchVariableControl());
            }
            return controls;
        }

        public void AllocateMemory()
        {
            uint mainSegmentEnd = 0x80367460;
            uint engineSegmentStart = 0x80378800;

            uint lastWarpNodeAddress = WatchVariableSpecialUtilities.GetWarpNodeAddresses().LastOrDefault();
            if (lastWarpNodeAddress == 0) return;

            List<uint> objAddresses = Config.ObjectSlotsManager.SelectedObjects.ConvertAll(obj => obj.Address);
            if (objAddresses.Count < 2) return;

            uint teleporter1Address = objAddresses[0];
            uint teleporter2Address = objAddresses[1];
            short teleporter1Id = Config.Stream.GetInt16(teleporter1Address + 0x188);
            short teleporter2Id = Config.Stream.GetInt16(teleporter2Address + 0x188);

            uint warpNode1Address = mainSegmentEnd;
            uint warpNode2Address = mainSegmentEnd + 0xC;

            byte level = Config.Stream.GetByte(MiscConfig.LevelAddress);
            byte area = Config.Stream.GetByte(MiscConfig.AreaAddress);

            Config.Stream.SetValue((byte)teleporter1Id, warpNode1Address + 0x0);
            Config.Stream.SetValue(level, warpNode1Address + 0x1);
            Config.Stream.SetValue(area, warpNode1Address + 0x2);
            Config.Stream.SetValue((byte)teleporter2Id, warpNode1Address + 0x3);
            Config.Stream.SetValue(teleporter1Address, warpNode1Address + 0x4);
            Config.Stream.SetValue(warpNode2Address, warpNode1Address + 0x8);

            Config.Stream.SetValue((byte)teleporter2Id, warpNode2Address + 0x0);
            Config.Stream.SetValue(level, warpNode2Address + 0x1);
            Config.Stream.SetValue(area, warpNode2Address + 0x2);
            Config.Stream.SetValue((byte)teleporter1Id, warpNode2Address + 0x3);
            Config.Stream.SetValue(teleporter2Address, warpNode2Address + 0x4);
            Config.Stream.SetValue(0x00000000U, warpNode2Address + 0x8);

            Config.Stream.SetValue(warpNode1Address, lastWarpNodeAddress + 0x8);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting.Providers;
using STROOP.Structs.Configurations;
using System.IO;
using STROOP.Controls;
using System.Windows.Forms.Integration;
using STROOP.Utilities;
using STROOP.Properties;

using System.Drawing;

namespace STROOP.Managers
{
    public class DecompilerManager
    {
        /// <summary>
        /// Used to save a 'history' or 'historic point' for 'Back' and 'Next' Navigation
        /// This include the current location, and the root node
        /// </summary>
        private struct HistoricPoint
        {
            public uint CurrentAddress;
            public List<uint> RootPathAddresses;

            public HistoricPoint(uint currentAddress, IEnumerable<uint> rootPathAddresses)
            {
                CurrentAddress = currentAddress;
                if (rootPathAddresses != null)
                    RootPathAddresses = new List<uint>(rootPathAddresses);
                else
                    RootPathAddresses = null;
            }

            public static bool operator ==(HistoricPoint a, HistoricPoint b)
            {
                if (a.CurrentAddress != b.CurrentAddress)
                    return false;

                // Null checks
                if (a.RootPathAddresses == null)
                    return b.RootPathAddresses == null;
                else if (b.RootPathAddresses == null)
                    return false;

                return (a.CurrentAddress == b.CurrentAddress) && (a.RootPathAddresses.SequenceEqual(b.RootPathAddresses));
            }

            public static bool operator !=(HistoricPoint a, HistoricPoint b)
            {
                return !(a == b);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(obj, null)
                    || !(obj is HistoricPoint))
                    return false;

                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                int hashCode = 13;
                hashCode = hashCode * 17 + CurrentAddress.GetHashCode();
                hashCode = hashCode * 17 + RootPathAddresses.GetHashCode();
                return hashCode;
            }
        }

        static readonly string marioRamPath = @"Resources\DAKompiler\marioRam";
        static readonly string decompile_py;

        Button _buttonBack, _buttonNext;
        TextBox _textBoxAddress;
        CompiledCode _pythonDecompiler;
        DecompilerView _decompilerView;
        TreeView _treeViewDecompile;

        byte[] _ramState = null;

        const int MaxHistoryCount = 200;
        LinkedList<HistoricPoint> _history = new LinkedList<HistoricPoint>();
        LinkedListNode<HistoricPoint> _currentHistoricPoint;

        uint? _currentAddress, _currentRootAddress;

        static DecompilerManager()
        {
            // Load python script
            using (Stream stream = new MemoryStream(Resources.decompile_py))
            using (StreamReader reader = new StreamReader(stream))
            {
                decompile_py = reader.ReadToEnd();
            }
        }

        public DecompilerManager(Control tabControl)
        {
            var splitContainerDecompiler = tabControl.Controls["splitContainerDecompiler"] as SplitContainer;
            _textBoxAddress = tabControl.Controls["textBoxDecompilerAddress"] as TextBox;
            var buttonDecompile = tabControl.Controls["buttonDecompilerDecompile"] as Button;
            _buttonBack = tabControl.Controls["buttonDecompilerBack"] as Button;
            _buttonNext = tabControl.Controls["buttonDecompilerNext"] as Button;

            _treeViewDecompile = splitContainerDecompiler.Panel1.Controls["treeViewDecompile"] as TreeView;
            _treeViewDecompile.NodeMouseClick += (sender, e) => ExpandNode(e.Node, true);

            ElementHost decompilerViewHost = splitContainerDecompiler.Panel2.Controls["decompilerViewHost"] as ElementHost;
            _decompilerView = decompilerViewHost.Child as DecompilerView;
            _decompilerView.OnFunctionClicked += _decompilerView_OnFunctionClicked; 

            buttonDecompile.Click += (sender, e) => Decompile(_textBoxAddress.Text, true);
            _buttonBack.Click += _buttonBack_Click;
            _buttonNext.Click += _buttonNext_Click;

            CreateDecompileEngine();
            UpdateHistoryButtons();
        }

        private void _decompilerView_OnFunctionClicked(object sender, string e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                uint? address = TryParseFunctionAddress(e);
                if (!address.HasValue)
                    return;
                Config.DisassemblyManager.Disassemble($"0x{address:X8}");
                Config.StroopMainForm.SwitchTab("tabPageDisassembly");
            }
            else
            {
                Decompile(e, false);
            }
                       
        }

        private void _buttonBack_Click(object sender, EventArgs e)
        {
            if (_currentHistoricPoint == null || _currentHistoricPoint == _history.First)
                return;

            _currentHistoricPoint = _currentHistoricPoint.Previous;

            UpdateHistoryButtons();
            Decompile(_currentHistoricPoint.Value.CurrentAddress, false, false);
        }

        private void _buttonNext_Click(object sender, EventArgs e)
        {
            if (_currentHistoricPoint == null || _currentHistoricPoint == _history.Last)
                return;

            _currentHistoricPoint = _currentHistoricPoint.Next;

            UpdateHistoryButtons();
            Decompile(_currentHistoricPoint.Value.CurrentAddress, false, false);
        }

        public void Decompile(string strAddress, bool setRoot)
        {
            uint? address = TryParseFunctionAddress(strAddress);
            if (!address.HasValue)
            {
                MessageBox.Show(String.Format("Address {0} is not valid!", _textBoxAddress.Text),
                    "Address Invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Decompile(address.Value, setRoot);
        }

        public void Decompile(uint address, bool setRoot, bool updateHistory = true)
        {
            address |= 0x80000000;

            if (_ramState == null)
                UpdateSnapshot();

            if (setRoot)
                SetRoot(address);

            if (updateHistory)
            {
                HistoricPoint currentPoint = new HistoricPoint(address, null);
                if (_currentHistoricPoint != null && currentPoint == _currentHistoricPoint.Value)
                {
                    // Same history, don't update
                }
                // End of history is free; add to end
                else if (_currentHistoricPoint == null || _currentHistoricPoint == _history.Last)
                {
                    _currentHistoricPoint = _history.AddLast(currentPoint);
                }
                else // End of history contians history
                {
                    // Same history; don't change
                    if (currentPoint == _currentHistoricPoint.Next.Value)
                    {
                        _currentHistoricPoint = _currentHistoricPoint.Next;
                    }
                    else // Writing over existing history
                    {
                        // Remove existing history
                        while (_history.Last != null && _history.Last != _currentHistoricPoint)
                            _history.RemoveLast();

                        // Add new item
                        _currentHistoricPoint = _history.AddLast(currentPoint);
                    }
                }

                // Clean history
                while (_history.Count > MaxHistoryCount)
                {
                    // We need to make sure that we are not removing the first element, which 
                    // will never happen, since we are always at least moving away from the first item
                    // Therefore, the only case would be when MaxHistory is set to 0, which it never is.
                    _history.RemoveFirst();
                }
                UpdateHistoryButtons();
            }

            _decompilerView.Text = DecompileFunction(address);
            _textBoxAddress.Text = DecompilerFunctionUtilities.AddressToString(address);
            _currentAddress = address;
        }

        void CreateDecompileEngine()
        {
            ScriptEngine python = Python.CreateEngine();

            // Disable Zip (This causes a weird error)
            var pc = HostingHelpers.GetLanguageContext(python) as PythonContext;
            var hooks = pc.SystemState.Get__dict__()["path_hooks"] as List;
            hooks.Clear();

            python.SetSearchPaths(new List<string>() { @"Resources\DAKompiler", @"Lib" });
            _pythonDecompiler = python.CreateScriptSourceFromString(decompile_py).Compile();
        }

        uint? TryParseFunctionAddress(string strAddress)
        {
            int fnIndex = strAddress.IndexOf("fn");
            if (fnIndex == 0)
                strAddress = strAddress.Substring(2);

            uint address;
            if (!ParsingUtilities.TryParseHex(strAddress, out address))
                return null;

            return address;
        }

        string DecompileFunction(uint address)
        {
            _pythonDecompiler.DefaultScope.SetVariable("function_address", address | 0x80000000);
            string result;
            try
            {
                result = _pythonDecompiler.Execute<string>();
            }
            catch (Exception e)
            {
                result = $"#{e.Message}";
            }

            return result;
        }

        void SetRoot(uint address)
        {
            address |= 0x80000000;

            _treeViewDecompile.Nodes.Clear();
            var rootNode = _treeViewDecompile.Nodes.Add(DecompilerFunctionUtilities.AddressToString(address));

            // Expand root node
            ExpandNode(rootNode, false);

            _currentRootAddress = address;
        }

        void ExpandNode(TreeNode node, bool decompile)
        {
            if (node.Text == null || node.Text == "" || node.Text == "?")
                return;

            uint? address = TryParseFunctionAddress(node.Text);
            if (!address.HasValue)
                return;

            // Expand if not already expanded
            if (node.ForeColor != Color.Blue)
            {
                int? instructionCount = DecompilerFunctionUtilities.FindEndAddress(address.Value, _ramState);
                List<uint?> calls = new List<uint?>();
                if (instructionCount.HasValue)
                    calls.AddRange(DecompilerFunctionUtilities.GetCalls(address.Value, instructionCount.Value, _ramState));

                foreach (var function in calls)
                {
                    TreeNode newNode;
                    if (!function.HasValue)
                        newNode = node.Nodes.Add("?");
                    else
                        newNode = node.Nodes.Add(DecompilerFunctionUtilities.AddressToString(function.Value));
                    newNode.ForeColor = Color.Red;
                }
            }

            if (decompile)
                Decompile(address.Value, false);
            node.ForeColor = Color.Blue;
        }

        // Unused, but partially implemented in the case of smarters history that can resolve the selected function in the tree view
        public void SelectSetRootPath(IEnumerable<uint> rootPath)
        {
            if (rootPath == null)
                return;

            bool first = true;
            TreeNode currentNode = null;
            foreach (uint address in rootPath)
            {
                // First node should update the root
                if (first)
                {
                    if (address != _currentRootAddress)
                        SetRoot(address);

                    currentNode = _treeViewDecompile.TopNode;
                    first = false;
                    continue;
                }

                // Failed to find node, end
                if (currentNode == null)
                    return;

                // Create subnodes
                ExpandNode(currentNode, false);

                // Find address in sub-nodes
                var foundNodes = currentNode.Nodes.Find(DecompilerFunctionUtilities.AddressToString(address), false);
                if (foundNodes.Length == 0)
                    return;
                currentNode = foundNodes[0];
            }

            _treeViewDecompile.SelectedNode = currentNode;
        }

        void UpdateHistoryButtons()
        {
            if (_currentHistoricPoint == null)
            {
                _buttonBack.Enabled = false;
                _buttonNext.Enabled = false;
                return;
            }

            _buttonBack.Enabled = _currentHistoricPoint.Previous != null;
            _buttonNext.Enabled = _currentHistoricPoint.Next != null;
        }

        void UpdateSnapshot()
        {
            _ramState = (byte[]) Config.Stream.Ram.Clone();
            using (var file = new FileStream(marioRamPath, FileMode.Create))
            {
                // Reverse endianness
                for (int i = 0; i < _ramState.Length; i += 4)
                {
                    file.WriteByte(_ramState[i + 3]);
                    file.WriteByte(_ramState[i + 2]);
                    file.WriteByte(_ramState[i + 1]);
                    file.WriteByte(_ramState[i]);
                }
            }
        }
    }
}

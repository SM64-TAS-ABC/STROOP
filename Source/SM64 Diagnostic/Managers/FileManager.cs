using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using System.Windows.Forms;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Controls;
using SM64_Diagnostic.Structs.Configurations;
using SM64Diagnostic.Controls;

namespace SM64_Diagnostic.Managers
{
    public class FileManager : DataManager
    {
        TabPage _tabControl;

        uint _fileAddress = 0;
        public uint FileAddress
        {
            get
            {
                return _fileAddress;
            }
            set
            {
                if (_fileAddress == value)
                    return;

                _fileAddress = value;

                foreach (var dataContainer in _dataControls)
                {
                    if (dataContainer is WatchVariableControl)
                    {
                        var watchVar = dataContainer as WatchVariableControl;
                        watchVar.OtherOffsets = new List<uint>() { _fileAddress };
                    }
                }
            }
        }

        private enum FileMode { FileA, FileB, FileC, FileD, FileASaved, FileBSaved, FileCSaved, FileDSaved };

        private FileMode _currentMode;

        public FileManager(ProcessStream stream, List<WatchVariable> fileData, TabPage tabControl, NoTearFlowLayoutPanel noTearFlowLayoutPanelFile)
            : base(stream, fileData, noTearFlowLayoutPanelFile)
        {
            _tabControl = tabControl;
            _currentMode = FileMode.FileA;

            SplitContainer splitContainerFile = tabControl.Controls["splitContainerFile"] as SplitContainer;

            /*
            (splitContainerTriangles.Panel1.Controls["radioButtonTriFloor"] as RadioButton).CheckedChanged
                += (sender, e) => Mode_CheckedChanged(sender, e, TriangleMode.Floor);
            (splitContainerTriangles.Panel1.Controls["radioButtonTriWall"] as RadioButton).CheckedChanged
                += (sender, e) => Mode_CheckedChanged(sender, e, TriangleMode.Wall);
            (splitContainerTriangles.Panel1.Controls["radioButtonTriCeiling"] as RadioButton).CheckedChanged
                += (sender, e) => Mode_CheckedChanged(sender, e, TriangleMode.Ceiling);
            (splitContainerTriangles.Panel1.Controls["radioButtonTriOther"] as RadioButton).CheckedChanged
                += (sender, e) => Mode_CheckedChanged(sender, e, TriangleMode.Other);
            */
        }

        private void Mode_CheckedChanged(object sender, EventArgs e, FileMode mode)
        {
            
        }
    }
}

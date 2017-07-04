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
        private enum FileMode { FileA, FileB, FileC, FileD, FileASaved, FileBSaved, FileCSaved, FileDSaved };

        TabPage _tabControl;
        FileMode _currentFileMode;
        uint _currentFileAddress;

        public FileManager(ProcessStream stream, List<WatchVariable> fileData, TabPage tabControl, NoTearFlowLayoutPanel noTearFlowLayoutPanelFile)
            : base(stream, fileData, noTearFlowLayoutPanelFile)
        {
            _tabControl = tabControl;
            _currentFileMode = FileMode.FileA;
            _currentFileAddress = Config.File.FileAAddress;

            SplitContainer splitContainerFile = tabControl.Controls["splitContainerFile"] as SplitContainer;

            (splitContainerFile.Panel1.Controls["radioButtonFileA"] as RadioButton).CheckedChanged
                += (sender, e) => FileMode_CheckedChanged(sender, e, FileMode.FileA);
            (splitContainerFile.Panel1.Controls["radioButtonFileB"] as RadioButton).CheckedChanged
                += (sender, e) => FileMode_CheckedChanged(sender, e, FileMode.FileB);
            (splitContainerFile.Panel1.Controls["radioButtonFileC"] as RadioButton).CheckedChanged
                += (sender, e) => FileMode_CheckedChanged(sender, e, FileMode.FileC);
            (splitContainerFile.Panel1.Controls["radioButtonFileD"] as RadioButton).CheckedChanged
                += (sender, e) => FileMode_CheckedChanged(sender, e, FileMode.FileD);

            (splitContainerFile.Panel1.Controls["radioButtonFileASaved"] as RadioButton).CheckedChanged
                += (sender, e) => FileMode_CheckedChanged(sender, e, FileMode.FileASaved);
            (splitContainerFile.Panel1.Controls["radioButtonFileBSaved"] as RadioButton).CheckedChanged
                += (sender, e) => FileMode_CheckedChanged(sender, e, FileMode.FileBSaved);
            (splitContainerFile.Panel1.Controls["radioButtonFileCSaved"] as RadioButton).CheckedChanged
                += (sender, e) => FileMode_CheckedChanged(sender, e, FileMode.FileCSaved);
            (splitContainerFile.Panel1.Controls["radioButtonFileDSaved"] as RadioButton).CheckedChanged
                += (sender, e) => FileMode_CheckedChanged(sender, e, FileMode.FileDSaved    );
        }

        private uint getFileAddressFromFileMode(FileMode mode)
        {
            switch (mode)
            {
                case FileMode.FileA:
                    return Config.File.FileAAddress;
                case FileMode.FileB:
                    return Config.File.FileBAddress;
                case FileMode.FileC:
                    return Config.File.FileCAddress;
                case FileMode.FileD:
                    return Config.File.FileDAddress;
                case FileMode.FileASaved:
                    return Config.File.FileASavedAddress;
                case FileMode.FileBSaved:
                    return Config.File.FileBSavedAddress;
                case FileMode.FileCSaved:
                    return Config.File.FileCSavedAddress;
                case FileMode.FileDSaved:
                    return Config.File.FileDSavedAddress;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void FileMode_CheckedChanged(object sender, EventArgs e, FileMode mode)
        {
            if (_currentFileMode == mode) return;

            _currentFileMode = mode;
            _currentFileAddress = getFileAddressFromFileMode(mode);

            foreach (var dataContainer in _dataControls)
            {
                if (dataContainer is WatchVariableControl)
                {
                    var watchVar = dataContainer as WatchVariableControl;
                    watchVar.OtherOffsets = new List<uint>() { _currentFileAddress };
                }
            }
        }
    }
}

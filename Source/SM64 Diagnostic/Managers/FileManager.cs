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

        Button _saveFileButton;

        public FileManager(ProcessStream stream, List<WatchVariable> fileData, TabPage tabControl, NoTearFlowLayoutPanel noTearFlowLayoutPanelFile)
            : base(stream, fileData, noTearFlowLayoutPanelFile, Config.File.FileAAddress)
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
                += (sender, e) => FileMode_CheckedChanged(sender, e, FileMode.FileDSaved);

            _saveFileButton = splitContainerFile.Panel1.Controls["buttonFileSave"] as Button;
            _saveFileButton.Click += FileSaveButton_Click;
        }

        private bool IsSavedFileMode(FileMode fileMode)
        {
            return fileMode == FileMode.FileASaved ||
                   fileMode == FileMode.FileBSaved ||
                   fileMode == FileMode.FileCSaved ||
                   fileMode == FileMode.FileDSaved;
        }

        private void FileSaveButton_Click(object sender, EventArgs e)
        {
            uint nonSavedAddress = GetNonSavedFileAddress(_currentFileMode);

            ushort checksumConstantOffset = 52;
            ushort checksumConstantValue = 17473;
            _stream.SetValue(checksumConstantValue, nonSavedAddress + checksumConstantOffset);

            ushort sum = (ushort)(checksumConstantValue % 256 + checksumConstantValue / 256);
            for (uint i = 0; i < Config.File.FileStructSize-4; i++)
            {
                // skip over the current checksum
                //if (i == Config.File.FileStructSize - 2 || i == Config.File.FileStructSize - 1) continue;

                byte b = _stream.GetByte(nonSavedAddress + i);
                sum += b;
                Console.WriteLine(i + " = " + b + " [" + sum + "]");
                //_stream.SetValue((byte)i, nonSavedAddress + i);
            }
            Console.WriteLine("");

            ushort checksumOffset = 54;
            _stream.SetValue(sum, nonSavedAddress + checksumOffset);
        }

        private uint GetNonSavedFileAddress(FileMode mode)
        {
            switch (mode)
            {
                case FileMode.FileA:
                case FileMode.FileASaved:
                    return Config.File.FileAAddress;
                case FileMode.FileB:
                case FileMode.FileBSaved:
                    return Config.File.FileBAddress;
                case FileMode.FileC:
                case FileMode.FileCSaved:
                    return Config.File.FileCAddress;
                case FileMode.FileD:
                case FileMode.FileDSaved:
                    return Config.File.FileDAddress;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private uint getFileAddress(FileMode mode)
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
            _currentFileAddress = getFileAddress(mode);

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

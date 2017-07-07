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
        private enum HatLocation { Mario, SSLKlepto, SSLGround, SLSnowman, SLGround, TTMUkiki, TTMGround };

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

            GroupBox fileGroupbox = splitContainerFile.Panel1.Controls["groupBoxFile"] as GroupBox;
            (fileGroupbox.Controls["radioButtonFileA"] as RadioButton).Click
                += (sender, e) => FileMode_Click(sender, e, FileMode.FileA);
            (fileGroupbox.Controls["radioButtonFileB"] as RadioButton).Click
                += (sender, e) => FileMode_Click(sender, e, FileMode.FileB);
            (fileGroupbox.Controls["radioButtonFileC"] as RadioButton).Click
                += (sender, e) => FileMode_Click(sender, e, FileMode.FileC);
            (fileGroupbox.Controls["radioButtonFileD"] as RadioButton).Click
                += (sender, e) => FileMode_Click(sender, e, FileMode.FileD);
            (fileGroupbox.Controls["radioButtonFileASaved"] as RadioButton).Click
                += (sender, e) => FileMode_Click(sender, e, FileMode.FileASaved);
            (fileGroupbox.Controls["radioButtonFileBSaved"] as RadioButton).Click
                += (sender, e) => FileMode_Click(sender, e, FileMode.FileBSaved);
            (fileGroupbox.Controls["radioButtonFileCSaved"] as RadioButton).Click
                += (sender, e) => FileMode_Click(sender, e, FileMode.FileCSaved);
            (fileGroupbox.Controls["radioButtonFileDSaved"] as RadioButton).Click
                += (sender, e) => FileMode_Click(sender, e, FileMode.FileDSaved);

            _saveFileButton = splitContainerFile.Panel1.Controls["buttonFileSave"] as Button;
            _saveFileButton.Click += FileSaveButton_Click;

            GroupBox hatLocationGroupbox = splitContainerFile.Panel1.Controls["groupBoxHatLocation"] as GroupBox;
            (hatLocationGroupbox.Controls["radioButtonHatLocationMario"] as RadioButton).Click
                += (sender, e) => HatLocation_Click(sender, e, HatLocation.Mario);
            (hatLocationGroupbox.Controls["radioButtonHatLocationSSLKlepto"] as RadioButton).Click
                += (sender, e) => HatLocation_Click(sender, e, HatLocation.SSLKlepto);
            (hatLocationGroupbox.Controls["radioButtonHatLocationSSLGround"] as RadioButton).Click
                += (sender, e) => HatLocation_Click(sender, e, HatLocation.SSLGround);
            (hatLocationGroupbox.Controls["radioButtonHatLocationSLSnowman"] as RadioButton).Click
                += (sender, e) => HatLocation_Click(sender, e, HatLocation.SLSnowman);
            (hatLocationGroupbox.Controls["radioButtonHatLocationSLGround"] as RadioButton).Click
                += (sender, e) => HatLocation_Click(sender, e, HatLocation.SLGround);
            (hatLocationGroupbox.Controls["radioButtonHatLocationTTMUkiki"] as RadioButton).Click
                += (sender, e) => HatLocation_Click(sender, e, HatLocation.TTMUkiki);
            (hatLocationGroupbox.Controls["radioButtonHatLocationTTMGround"] as RadioButton).Click
                += (sender, e) => HatLocation_Click(sender, e, HatLocation.TTMGround);
        }

        private void SetHatMode(byte hatModeByte)
        {
            byte oldByte = _stream.GetByte(_currentFileAddress + Config.File.HatLocationModeOffset);
            byte newByte = (byte)((oldByte & ~Config.File.HatLocationModeMask) | hatModeByte);
            _stream.SetValue(newByte, _currentFileAddress + Config.File.HatLocationModeOffset);
        }

        private void HatLocation_Click(object sender, EventArgs e, HatLocation hatLocation)
        {
            switch (hatLocation)
            {
                case HatLocation.Mario:
                    SetHatMode(0);
                    break;

                case HatLocation.SSLKlepto:
                    SetHatMode(Config.File.HatLocationKleptoMask);
                    break;

                case HatLocation.SSLGround:
                    SetHatMode(Config.File.HatLocationGroundMask);
                    _stream.SetValue(Config.File.HatLocationCourseSSLValue, _currentFileAddress + Config.File.HatLocationCourseOffset);
                    break;

                case HatLocation.SLSnowman:
                    SetHatMode(Config.File.HatLocationSnowmanMask);
                    break;

                case HatLocation.SLGround:
                    SetHatMode(Config.File.HatLocationGroundMask);
                    _stream.SetValue(Config.File.HatLocationCourseSLValue, _currentFileAddress + Config.File.HatLocationCourseOffset);
                    break;

                case HatLocation.TTMUkiki:
                    SetHatMode(Config.File.HatLocationUkikiMask);
                    break;

                case HatLocation.TTMGround:
                    SetHatMode(Config.File.HatLocationGroundMask);
                    _stream.SetValue(Config.File.HatLocationCourseTTMValue, _currentFileAddress + Config.File.HatLocationCourseOffset);
                    break;
            }

            Console.WriteLine(hatLocation);
        }

        private void FileSaveButton_Click(object sender, EventArgs e)
        {
            // Get the corresponding unsaved file struct address
            uint nonSavedAddress = GetNonSavedFileAddress(_currentFileMode);

            // Set the checksum constant
            _stream.SetValue(Config.File.ChecksumConstantValue, nonSavedAddress + Config.File.ChecksumConstantOffset);

            // Sum up all bytes to calculate the checksum
            ushort checksum = (ushort)(Config.File.ChecksumConstantValue % 256 + Config.File.ChecksumConstantValue / 256);
            for (uint i = 0; i < Config.File.FileStructSize-4; i++)
            {
                byte b = _stream.GetByte(nonSavedAddress + i);
                checksum += b;
            }

            // Set the checksum
            _stream.SetValue(checksum, nonSavedAddress + Config.File.ChecksumOffset);

            // Copy all values from the unsaved struct to the saved struct
            uint savedAddress = nonSavedAddress + Config.File.FileStructSize;
            for (uint i = 0; i < Config.File.FileStructSize - 4; i++)
            {
                byte b = _stream.GetByte(nonSavedAddress + i);
                _stream.SetValue(b, savedAddress + i);
            }
            _stream.SetValue(Config.File.ChecksumConstantValue, savedAddress + Config.File.ChecksumConstantOffset);
            _stream.SetValue(checksum, savedAddress + Config.File.ChecksumOffset);
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

        private void FileMode_Click(object sender, EventArgs e, FileMode mode)
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

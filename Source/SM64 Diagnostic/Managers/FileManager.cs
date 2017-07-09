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
using System.Drawing;

namespace SM64_Diagnostic.Managers
{
    public class FileManager : DataManager
    {
        public static FileManager Instance = null;

        public enum FileMode { FileA, FileB, FileC, FileD, FileASaved, FileBSaved, FileCSaved, FileDSaved };
        private enum HatLocation { Mario, SSLKlepto, SSLGround, SLSnowman, SLGround, TTMUkiki, TTMGround };

        TabPage _tabControl;
        FileImageGui _gui;

        public FileMode CurrentFileMode { get; private set; }
        public uint CurrentFileAddress { get; private set; }

        Button _saveFileButton;
        Button _numStarsButton;

        RadioButton _hatLocationMarioRadioButton;
        RadioButton _hatLocationSSLKleptoRadioButton;
        RadioButton _hatLocationSSLGroundRadioButton;
        RadioButton _hatLocationSLSnowmanRadioButton;
        RadioButton _hatLocationSLGroundRadioButton;
        RadioButton _hatLocationTTMUkikiRadioButton;
        RadioButton _hatLocationTTMGroundRadioButton;

        List<FilePictureBox> _filePictureBoxList;
        List<FileCoinScoreTextbox> _fileCoinScoreTextboxList;

        HatLocation? _currentHatLocation;

        public FileManager(ProcessStream stream, List<WatchVariable> fileData, TabPage tabControl, NoTearFlowLayoutPanel noTearFlowLayoutPanelFile, FileImageGui gui)
            : base(stream, fileData, noTearFlowLayoutPanelFile, Config.File.FileAAddress)
        {
            Instance = this;
            _tabControl = tabControl;
            _gui = gui;
            CurrentFileMode = FileMode.FileA;
            CurrentFileAddress = Config.File.FileAAddress;

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

            _numStarsButton = splitContainerFile.Panel1.Controls["buttonFileNumStars"] as Button;
            _numStarsButton.Click += NumStarsButton_Click;

            GroupBox hatLocationGroupbox = splitContainerFile.Panel1.Controls["groupBoxHatLocation"] as GroupBox;

            _hatLocationMarioRadioButton = hatLocationGroupbox.Controls["radioButtonHatLocationMario"] as RadioButton;
            _hatLocationSSLKleptoRadioButton = hatLocationGroupbox.Controls["radioButtonHatLocationSSLKlepto"] as RadioButton;
            _hatLocationSSLGroundRadioButton = hatLocationGroupbox.Controls["radioButtonHatLocationSSLGround"] as RadioButton;
            _hatLocationSLSnowmanRadioButton = hatLocationGroupbox.Controls["radioButtonHatLocationSLSnowman"] as RadioButton;
            _hatLocationSLGroundRadioButton = hatLocationGroupbox.Controls["radioButtonHatLocationSLGround"] as RadioButton;
            _hatLocationTTMUkikiRadioButton = hatLocationGroupbox.Controls["radioButtonHatLocationTTMUkiki"] as RadioButton;
            _hatLocationTTMGroundRadioButton = hatLocationGroupbox.Controls["radioButtonHatLocationTTMGround"] as RadioButton;

            _hatLocationMarioRadioButton.Click += (sender, e) => HatLocation_Click(sender, e, HatLocation.Mario);
            _hatLocationSSLKleptoRadioButton.Click += (sender, e) => HatLocation_Click(sender, e, HatLocation.SSLKlepto);
            _hatLocationSSLGroundRadioButton.Click += (sender, e) => HatLocation_Click(sender, e, HatLocation.SSLGround);
            _hatLocationSLSnowmanRadioButton.Click += (sender, e) => HatLocation_Click(sender, e, HatLocation.SLSnowman);
            _hatLocationSLGroundRadioButton.Click += (sender, e) => HatLocation_Click(sender, e, HatLocation.SLGround);
            _hatLocationTTMUkikiRadioButton.Click += (sender, e) => HatLocation_Click(sender, e, HatLocation.TTMUkiki);
            _hatLocationTTMGroundRadioButton.Click += (sender, e) => HatLocation_Click(sender, e, HatLocation.TTMGround);

            _currentHatLocation = getCurrentHatLocation();

            TableLayoutPanel fileTable = splitContainerFile.Panel1.Controls["tableLayoutPanelFile"] as TableLayoutPanel;

            _filePictureBoxList = new List<FilePictureBox>();
            int numRows = 26;
            uint[] courseAddressOffsets = new uint[numRows];
            byte[] courseMasks = new byte[numRows];
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    string controlName = String.Format("filePictureBoxTableRow{0}Col{1}", row + 1, col + 1);
                    FileStarPictureBox fileStarPictureBox = fileTable.Controls[controlName] as FileStarPictureBox;
                    if (fileStarPictureBox == null) continue;

                    uint addressOffset = GetStarAddressOffset(row, col);
                    byte mask = GetStarMask(row, col);
                    string missionName = Config.Missions.GetMissionName(row + 1, col + 1);
                    fileStarPictureBox.Initialize(_stream, _gui, addressOffset, mask, _gui.PowerStarImage, _gui.PowerStarBlackImage, missionName);
                    _filePictureBoxList.Add(fileStarPictureBox);

                    courseAddressOffsets[row] = addressOffset;
                    courseMasks[row] = (byte)(courseMasks[row] | mask);
                }
            }

            for (int row = 0; row < numRows; row++)
            {
                string controlName = String.Format("labelFileTableRow{0}", row + 1);
                FileCourseLabel fileCourseLabel = fileTable.Controls[controlName] as FileCourseLabel;
                fileCourseLabel.Initialize(_stream, courseAddressOffsets[row], courseMasks[row]);
            }

            for (int row = 0; row < numRows; row++)
            {
                int col = 7;
                string controlName = String.Format("filePictureBoxTableRow{0}Col{1}", row + 1, col + 1);
                FileBinaryPictureBox fileCannonPictureBox = fileTable.Controls[controlName] as FileBinaryPictureBox;
                if (fileCannonPictureBox == null) continue;

                uint addressOffset = GetCannonAddressOffset(row, col);
                byte mask = 0x80;
                fileCannonPictureBox.Initialize(_stream, _gui, addressOffset, mask, _gui.CannonImage, _gui.CannonLidImage);
                _filePictureBoxList.Add(fileCannonPictureBox);
            }

            for (int row = 0; row < numRows; row++)
            {
                int col = 8;
                string controlName = String.Format("filePictureBoxTableRow{0}Col{1}", row + 1, col + 1);
                FileBinaryPictureBox fileBinaryPictureBox = fileTable.Controls[controlName] as FileBinaryPictureBox;
                if (fileBinaryPictureBox == null) continue;

                uint addressOffset = GetDoorAddressOffset(row, col);
                byte mask = GetDoorMask(row, col);
                (Image onImage, Image offImage) = GetDoorImages(row, col);
                fileBinaryPictureBox.Initialize(_stream, _gui, addressOffset, mask, onImage, offImage);
                _filePictureBoxList.Add(fileBinaryPictureBox);
            }

            _fileCoinScoreTextboxList = new List<FileCoinScoreTextbox>();
            for (int row = 0; row < 15; row++)
            {
                int col = 9;
                string controlName = String.Format("textBoxTableRow{0}Col{1}", row + 1, col + 1);
                FileCoinScoreTextbox fileCoinScoreTextBox = fileTable.Controls[controlName] as FileCoinScoreTextbox;
                fileCoinScoreTextBox.Initialize(_stream, 0x25 + (uint)row);
                _fileCoinScoreTextboxList.Add(fileCoinScoreTextBox);
            }
        }

        private uint GetCourseLabelAddressOffset(int row)
        {
            return 0;
        }

        private byte GetCourseLabelMask(int row)
        {
            return 0x7F;
        }

        private short CalculateNumStars()
        {
            short starCount = 0;
            byte starByte;
            
            // go through the 25 contiguous star bytes
            for (int i = 0; i < 25; i++)
            {
                starByte = _stream.GetByte(CurrentFileAddress + 0x0C + (uint)i);
                for (int b = 0; b < 7; b++)
                {
                    starCount += (byte)((starByte >> b) & 1);
                }
            }

            // go through the 1 non-contiguous star byte (for toads and MIPS)
            starByte = _stream.GetByte(CurrentFileAddress + 0x08);
            for (int b = 0; b < 7; b++)
            {
                starCount += (byte)((starByte >> b) & 1);
            }

            return starCount;
        }

        private (Image onImage, Image offImage) GetDoorImages(int row, int col)
        {
            switch (row)
            {
                case 1:
                case 18:
                    return (_gui.DoorBlackImage, _gui.Door1StarImage);
                case 2:
                case 3:
                    return (_gui.DoorBlackImage, _gui.Door3StarImage);
                case 21:
                case 22:
                case 23:
                    return (_gui.StarDoorOpenImage, _gui.StarDoorClosedImage);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private uint GetDoorAddressOffset(int row, int col)
        {
            if (row == 23)
                return 0x09;
            else
                return 0x0A;
        }

        private byte GetDoorMask(int row, int col)
        {
            switch (row)
            {
                case 1:
                    return 0x08; // WF 1 star door
                case 2:
                    return 0x20; // JRB 3 star door
                case 3:
                    return 0x10; // CCM 3 star door
                case 18:
                    return 0x04; // PSS 1 star door
                case 21:
                    return 0x40; // BitDW star door
                case 22:
                    return 0x80; // BitFS star door
                case 23:
                    return 0x10; // BitS star door
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private uint GetCannonAddressOffset(int row, int col)
        {
            if (row == 20)
                return 0x23; // WMotR
            else
                return 0x0D + (uint)row; // main courses
        }

        private uint GetStarAddressOffset(int row, int col)
        {
            switch (row)
            {
                default:
                    return 0x0C + (uint)row; // main courses
                case 15:
                    return 0x20; // TotWC
                case 16:
                    return 0x1F; // CotMC
                case 17:
                    return 0x21; // VCutM
                case 18:
                    return 0x1E; // PSS
                case 19:
                    return 0x23; // SA
                case 20:
                    return 0x22; // WMotR
                case 21:
                    return 0x1B; // BitDW
                case 22:
                    return 0x1C; // BitFS
                case 23:
                    return 0x1D; // BitS
                case 24:
                case 25:
                    return 0x08; // Toad, MIPS
            }
        }

        private byte GetStarMask(int row, int col)
        {
            int bitOffset = row == 25 ? 3 : 0;
            return (byte)Math.Pow(2, col + bitOffset);
        }

        private void SetHatMode(byte hatModeByte)
        {
            byte oldByte = _stream.GetByte(CurrentFileAddress + Config.File.HatLocationModeOffset);
            byte newByte = (byte)((oldByte & ~Config.File.HatLocationModeMask) | hatModeByte);
            _stream.SetValue(newByte, CurrentFileAddress + Config.File.HatLocationModeOffset);
        }

        private void HatLocation_Click(object sender, EventArgs e, HatLocation hatLocation)
        {
            switch (hatLocation)
            {
                case HatLocation.Mario:
                    SetHatMode(Config.File.HatLocationMarioMask);
                    break;

                case HatLocation.SSLKlepto:
                    SetHatMode(Config.File.HatLocationKleptoMask);
                    break;

                case HatLocation.SSLGround:
                    SetHatMode(Config.File.HatLocationGroundMask);
                    _stream.SetValue(Config.File.HatLocationCourseSSLValue, CurrentFileAddress + Config.File.HatLocationCourseOffset);
                    break;

                case HatLocation.SLSnowman:
                    SetHatMode(Config.File.HatLocationSnowmanMask);
                    break;

                case HatLocation.SLGround:
                    SetHatMode(Config.File.HatLocationGroundMask);
                    _stream.SetValue(Config.File.HatLocationCourseSLValue, CurrentFileAddress + Config.File.HatLocationCourseOffset);
                    break;

                case HatLocation.TTMUkiki:
                    SetHatMode(Config.File.HatLocationUkikiMask);
                    break;

                case HatLocation.TTMGround:
                    SetHatMode(Config.File.HatLocationGroundMask);
                    _stream.SetValue(Config.File.HatLocationCourseTTMValue, CurrentFileAddress + Config.File.HatLocationCourseOffset);
                    break;
            }
        }

        private void NumStarsButton_Click(object sender, EventArgs e)
        {
            short numStars = CalculateNumStars();
            _stream.SetValue(numStars, Config.Hud.StarCountAddress);
            _stream.SetValue(numStars, Config.Hud.DisplayStarCountAddress);
        }

        private void FileSaveButton_Click(object sender, EventArgs e)
        {
            // Get the corresponding unsaved file struct address
            uint nonSavedAddress = GetNonSavedFileAddress(CurrentFileMode);

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
            if (CurrentFileMode == mode) return;

            CurrentFileMode = mode;
            CurrentFileAddress = getFileAddress(mode);

            foreach (var dataContainer in _dataControls)
            {
                if (dataContainer is WatchVariableControl)
                {
                    var watchVar = dataContainer as WatchVariableControl;
                    watchVar.OtherOffsets = new List<uint>() { CurrentFileAddress };
                }
            }
        }

        private HatLocation? getCurrentHatLocation()
        {
            ushort hatLocationCourse = _stream.GetUInt16(CurrentFileAddress + Config.File.HatLocationCourseOffset);
            byte hatLocationMode = (byte)(_stream.GetByte(CurrentFileAddress + Config.File.HatLocationModeOffset) & Config.File.HatLocationModeMask);

            return hatLocationMode == Config.File.HatLocationMarioMask ? HatLocation.Mario :
                   hatLocationMode == Config.File.HatLocationKleptoMask ? HatLocation.SSLKlepto :
                   hatLocationMode == Config.File.HatLocationSnowmanMask ? HatLocation.SLSnowman :
                   hatLocationMode == Config.File.HatLocationUkikiMask ? HatLocation.TTMUkiki :
                   hatLocationMode == Config.File.HatLocationGroundMask ?
                       (hatLocationCourse == Config.File.HatLocationCourseSSLValue ? HatLocation.SSLGround :
                        hatLocationCourse == Config.File.HatLocationCourseSLValue ? HatLocation.SLGround :
                        hatLocationCourse == Config.File.HatLocationCourseTTMValue ? HatLocation.TTMGround :
                        (HatLocation?)null) :
                   null;
        }

        public override void Update(bool updateView)
        {
            short currentNumStars = CalculateNumStars();
            _numStarsButton.Text = currentNumStars == 1 ? currentNumStars + " Star" : currentNumStars + " Stars";

            _currentHatLocation = getCurrentHatLocation();
             
            _hatLocationMarioRadioButton.Checked = _currentHatLocation == HatLocation.Mario;
            _hatLocationSSLKleptoRadioButton.Checked = _currentHatLocation == HatLocation.SSLKlepto;
            _hatLocationSSLGroundRadioButton.Checked = _currentHatLocation == HatLocation.SSLGround;
            _hatLocationSLSnowmanRadioButton.Checked = _currentHatLocation == HatLocation.SLSnowman;
            _hatLocationSLGroundRadioButton.Checked = _currentHatLocation == HatLocation.SLGround;
            _hatLocationTTMUkikiRadioButton.Checked = _currentHatLocation == HatLocation.TTMUkiki;
            _hatLocationTTMGroundRadioButton.Checked = _currentHatLocation == HatLocation.TTMGround;

            foreach (FilePictureBox filePictureBox in _filePictureBoxList)
            {
                filePictureBox.UpdateImage();
            }

            foreach (FileCoinScoreTextbox fileCoinScoreTextbox in _fileCoinScoreTextboxList)
            {
                fileCoinScoreTextbox.UpdateText();
            }

            base.Update(updateView);
        }
    }
}

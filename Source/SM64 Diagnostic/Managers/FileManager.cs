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
        public enum HatLocation { Mario, SSLKlepto, SSLGround, SLSnowman, SLGround, TTMUkiki, TTMGround };

        TabPage _tabControl;
        FileImageGui _gui;

        public FileMode CurrentFileMode { get; private set; }
        public uint CurrentFileAddress { get; private set; }

        Button _saveFileButton;
        Button _eraseFileButton;
        Button _allStarsButton;
        Button _noStarsButton;
        Button _everythingButton;
        Button _nothingButton;
        Button _numStarsButton;

        List<FilePictureBox> _filePictureBoxList;
        List<FileTextbox> _fileTextboxList;

        int numRows = 26;

        // Keep track of each row's address and masks
        uint[] _courseStarsAddressOffsets;
        byte[] _courseStarsMasks;
        uint?[] _courseCannonAddressOffsets;
        byte?[] _courseCannonMasks;
        uint?[] _courseDoorAddressOffsets;
        byte?[] _courseDoorMasks;

        public FileManager(ProcessStream stream, List<WatchVariable> fileData, TabPage tabControl, NoTearFlowLayoutPanel noTearFlowLayoutPanelFile, FileImageGui gui)
            : base(stream, fileData, noTearFlowLayoutPanelFile, Config.File.FileAAddress)
        {
            Instance = this;
            _tabControl = tabControl;
            _gui = gui;

            _filePictureBoxList = new List<FilePictureBox>();
            _fileTextboxList = new List<FileTextbox>();
            _courseStarsAddressOffsets = new uint[numRows];
            _courseStarsMasks = new byte[numRows];
            _courseCannonAddressOffsets = new uint?[numRows];
            _courseCannonMasks = new byte?[numRows];
            _courseDoorAddressOffsets = new uint?[numRows];
            _courseDoorMasks = new byte?[numRows];

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

            TableLayoutPanel fileTable = splitContainerFile.Panel1.Controls["tableLayoutPanelFile"] as TableLayoutPanel;

            // stars
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    string controlName = String.Format("filePictureBoxTableRow{0}Col{1}", row + 1, col + 1);
                    FileStarPictureBox fileStarPictureBox = fileTable.Controls[controlName] as FileStarPictureBox;
                    if (fileStarPictureBox == null) continue;

                    uint addressOffset = GetStarAddressOffset(row);
                    byte mask = GetStarMask(row, col);
                    string missionName = Config.Missions.GetMissionName(row + 1, col + 1);
                    fileStarPictureBox.Initialize(_stream, _gui, addressOffset, mask, _gui.PowerStarImage, _gui.PowerStarBlackImage, missionName);
                    _filePictureBoxList.Add(fileStarPictureBox);

                    _courseStarsAddressOffsets[row] = addressOffset;
                    _courseStarsMasks[row] = (byte)(_courseStarsMasks[row] | mask);
                }
            }

            // course labels
            for (int row = 0; row < numRows; row++)
            {
                string controlName = String.Format("labelFileTableRow{0}", row + 1);
                FileCourseLabel fileCourseLabel = fileTable.Controls[controlName] as FileCourseLabel;
                fileCourseLabel.Initialize(_stream, _courseStarsAddressOffsets[row], _courseStarsMasks[row]);
            }

            // cannons
            for (int row = 0; row < numRows; row++)
            {
                int col = 7;
                string controlName = String.Format("filePictureBoxTableRow{0}Col{1}", row + 1, col + 1);
                FileBinaryPictureBox fileCannonPictureBox = fileTable.Controls[controlName] as FileBinaryPictureBox;
                if (fileCannonPictureBox == null) continue;

                uint addressOffset = GetCannonAddressOffset(row);
                byte mask = Config.File.CannonMask;
                fileCannonPictureBox.Initialize(_stream, addressOffset, mask, _gui.CannonImage, _gui.CannonLidImage);
                _filePictureBoxList.Add(fileCannonPictureBox);

                _courseCannonAddressOffsets[row] = addressOffset;
                _courseCannonMasks[row] = mask;
            }

            // doors
            for (int row = 0; row < numRows; row++)
            {
                int col = 8;
                string controlName = String.Format("filePictureBoxTableRow{0}Col{1}", row + 1, col + 1);
                FileBinaryPictureBox fileBinaryPictureBox = fileTable.Controls[controlName] as FileBinaryPictureBox;
                if (fileBinaryPictureBox == null) continue;

                uint addressOffset = GetDoorAddressOffset(row);
                byte mask = GetDoorMask(row);
                (Image onImage, Image offImage) = GetDoorImages(row);
                fileBinaryPictureBox.Initialize(_stream, addressOffset, mask, onImage, offImage);
                _filePictureBoxList.Add(fileBinaryPictureBox);

                _courseDoorAddressOffsets[row] = addressOffset;
                _courseDoorMasks[row] = mask;
            }

            // coin scores
            for (int row = 0; row < 15; row++)
            {
                int col = 9;
                string controlName = String.Format("textBoxTableRow{0}Col{1}", row + 1, col + 1);
                FileCoinScoreTextbox fileCoinScoreTextBox = fileTable.Controls[controlName] as FileCoinScoreTextbox;
                fileCoinScoreTextBox.Initialize(_stream, Config.File.CoinScoreAddressStart + (uint)row);
                _fileTextboxList.Add(fileCoinScoreTextBox);
            }

            GroupBox hatLocationGroupbox = splitContainerFile.Panel1.Controls["groupBoxHatLocation"] as GroupBox;

            // hat location radio button pictures
            FileHatLocationPictureBox filePictureBoxHatLocationMario = hatLocationGroupbox.Controls["filePictureBoxHatLocationMario"] as FileHatLocationPictureBox;
            filePictureBoxHatLocationMario.Initialize(_stream, HatLocation.Mario, _gui.HatOnMarioImage, _gui.HatOnMarioGreyImage);
            _filePictureBoxList.Add(filePictureBoxHatLocationMario);

            FileHatLocationPictureBox filePictureBoxHatLocationKlepto = hatLocationGroupbox.Controls["filePictureBoxHatLocationKlepto"] as FileHatLocationPictureBox;
            filePictureBoxHatLocationKlepto.Initialize(_stream, HatLocation.SSLKlepto, _gui.HatOnKleptoImage, _gui.HatOnKleptoGreyImage);
            _filePictureBoxList.Add(filePictureBoxHatLocationKlepto);

            FileHatLocationPictureBox filePictureBoxHatLocationSnowman = hatLocationGroupbox.Controls["filePictureBoxHatLocationSnowman"] as FileHatLocationPictureBox;
            filePictureBoxHatLocationSnowman.Initialize(_stream, HatLocation.SLSnowman, _gui.HatOnSnowmanImage, _gui.HatOnSnowmanGreyImage);
            _filePictureBoxList.Add(filePictureBoxHatLocationSnowman);

            FileHatLocationPictureBox filePictureBoxHatLocationUkiki = hatLocationGroupbox.Controls["filePictureBoxHatLocationUkiki"] as FileHatLocationPictureBox;
            filePictureBoxHatLocationUkiki.Initialize(_stream, HatLocation.TTMUkiki, _gui.HatOnUkikiImage, _gui.HatOnUkikiGreyImage);
            _filePictureBoxList.Add(filePictureBoxHatLocationUkiki);

            FileHatLocationPictureBox filePictureBoxHatLocationSSLGround = hatLocationGroupbox.Controls["filePictureBoxHatLocationSSLGround"] as FileHatLocationPictureBox;
            filePictureBoxHatLocationSSLGround.Initialize(_stream, HatLocation.SSLGround, _gui.HatOnGroundInSSLImage, _gui.HatOnGroundInSSLGreyImage);
            _filePictureBoxList.Add(filePictureBoxHatLocationSSLGround);

            FileHatLocationPictureBox filePictureBoxHatLocationSLGround = hatLocationGroupbox.Controls["filePictureBoxHatLocationSLGround"] as FileHatLocationPictureBox;
            filePictureBoxHatLocationSLGround.Initialize(_stream, HatLocation.SLGround, _gui.HatOnGroundInSLImage, _gui.HatOnGroundInSLGreyImage);
            _filePictureBoxList.Add(filePictureBoxHatLocationSLGround);

            FileHatLocationPictureBox filePictureBoxHatLocationTTMGround = hatLocationGroupbox.Controls["filePictureBoxHatLocationTTMGround"] as FileHatLocationPictureBox;
            filePictureBoxHatLocationTTMGround.Initialize(_stream, HatLocation.TTMGround, _gui.HatOnGroundInTTMImage, _gui.HatOnGroundInTTMGreyImage);
            _filePictureBoxList.Add(filePictureBoxHatLocationTTMGround);

            // hat position textboxes
            FileHatPositionTextbox textboxHatLocationPositionX = hatLocationGroupbox.Controls["textboxHatLocationPositionX"] as FileHatPositionTextbox;
            textboxHatLocationPositionX.Initialize(_stream, Config.File.HatPositionXAddress);
            _fileTextboxList.Add(textboxHatLocationPositionX);

            FileHatPositionTextbox textboxHatLocationPositionY = hatLocationGroupbox.Controls["textboxHatLocationPositionY"] as FileHatPositionTextbox;
            textboxHatLocationPositionY.Initialize(_stream, Config.File.HatPositionYAddress);
            _fileTextboxList.Add(textboxHatLocationPositionY);

            FileHatPositionTextbox textboxHatLocationPositionZ = hatLocationGroupbox.Controls["textboxHatLocationPositionZ"] as FileHatPositionTextbox;
            textboxHatLocationPositionZ.Initialize(_stream, Config.File.HatPositionZAddress);
            _fileTextboxList.Add(textboxHatLocationPositionZ);

            // miscellaneous checkbox pictures
            FileBinaryPictureBox filePictureBoxFileStarted = splitContainerFile.Panel1.Controls["filePictureBoxFileStarted"] as FileBinaryPictureBox;
            filePictureBoxFileStarted.Initialize(_stream, Config.File.FileStartedAddress, Config.File.FileStartedMask, _gui.FileStartedImage, _gui.FileNotStartedImage);
            _filePictureBoxList.Add(filePictureBoxFileStarted);

            FileBinaryPictureBox filePictureBoxRedCapSwitchPressed = splitContainerFile.Panel1.Controls["filePictureBoxRedCapSwitchPressed"] as FileBinaryPictureBox;
            filePictureBoxRedCapSwitchPressed.Initialize(_stream, Config.File.CapSwitchPressedAddress, Config.File.RedCapSwitchMask, _gui.CapSwitchRedPressedImage, _gui.CapSwitchRedUnpressedImage);
            _filePictureBoxList.Add(filePictureBoxRedCapSwitchPressed);

            FileBinaryPictureBox filePictureBoxGreenCapSwitchPressed = splitContainerFile.Panel1.Controls["filePictureBoxGreenCapSwitchPressed"] as FileBinaryPictureBox;
            filePictureBoxGreenCapSwitchPressed.Initialize(_stream, Config.File.CapSwitchPressedAddress, Config.File.GreenCapSwitchMask, _gui.CapSwitchGreenPressedImage, _gui.CapSwitchGreenUnpressedImage);
            _filePictureBoxList.Add(filePictureBoxGreenCapSwitchPressed);

            FileBinaryPictureBox filePictureBoxBlueCapSwitchPressed = splitContainerFile.Panel1.Controls["filePictureBoxBlueCapSwitchPressed"] as FileBinaryPictureBox;
            filePictureBoxBlueCapSwitchPressed.Initialize(_stream, Config.File.CapSwitchPressedAddress, Config.File.BlueCapSwitchMask, _gui.CapSwitchBluePressedImage, _gui.CapSwitchBlueUnpressedImage);
            _filePictureBoxList.Add(filePictureBoxBlueCapSwitchPressed);

            FileKeyDoorPictureBox filePictureBoxKeyDoor1Opened = splitContainerFile.Panel1.Controls["filePictureBoxKeyDoor1Opened"] as FileKeyDoorPictureBox;
            filePictureBoxKeyDoor1Opened.Initialize(_stream, Config.File.KeyDoorAddress, Config.File.KeyDoor1KeyMask, Config.File.KeyDoor1OpenedMask, _gui.KeyDoorOpenKeyImage, _gui.KeyDoorClosedKeyImage, _gui.KeyDoorOpenImage, _gui.KeyDoorClosedImage);
            _filePictureBoxList.Add(filePictureBoxKeyDoor1Opened);

            FileKeyDoorPictureBox filePictureBoxKeyDoor2Opened = splitContainerFile.Panel1.Controls["filePictureBoxKeyDoor2Opened"] as FileKeyDoorPictureBox;
            filePictureBoxKeyDoor2Opened.Initialize(_stream, Config.File.KeyDoorAddress, Config.File.KeyDoor2KeyMask, Config.File.KeyDoor2OpenedMask, _gui.KeyDoorOpenKeyImage, _gui.KeyDoorClosedKeyImage, _gui.KeyDoorOpenImage, _gui.KeyDoorClosedImage);
            _filePictureBoxList.Add(filePictureBoxKeyDoor2Opened);

            FileBinaryPictureBox filePictureBoxMoatDrained = splitContainerFile.Panel1.Controls["filePictureBoxMoatDrained"] as FileBinaryPictureBox;
            filePictureBoxMoatDrained.Initialize(_stream, Config.File.MoatDrainedAddress, Config.File.MoatDrainedMask, _gui.MoatDrainedImage, _gui.MoatNotDrainedImage);
            _filePictureBoxList.Add(filePictureBoxMoatDrained);

            FileBinaryPictureBox filePictureBoxDDDMovedBack = splitContainerFile.Panel1.Controls["filePictureBoxDDDMovedBack"] as FileBinaryPictureBox;
            filePictureBoxDDDMovedBack.Initialize(_stream, Config.File.DDDMovedBackAddress, Config.File.DDDMovedBackMask, _gui.DDDPaintingMovedBackImage, _gui.DDDPaintingNotMovedBackImage);
            _filePictureBoxList.Add(filePictureBoxDDDMovedBack);

            // buttons
            _saveFileButton = splitContainerFile.Panel1.Controls["buttonFileSave"] as Button;
            _saveFileButton.Click += FileSaveButton_Click;

            _eraseFileButton = splitContainerFile.Panel1.Controls["buttonFileErase"] as Button;
            _eraseFileButton.Click += FileEraseButton_Click;

            _allStarsButton = splitContainerFile.Panel1.Controls["buttonAllStars"] as Button;
            _allStarsButton.Click += (sender, e) => FileSetStars(true);

            _noStarsButton = splitContainerFile.Panel1.Controls["buttonNoStars"] as Button;
            _noStarsButton.Click += (sender, e) => FileSetStars(false);

            _everythingButton = splitContainerFile.Panel1.Controls["buttonEverything"] as Button;
            _everythingButton.Click += (sender, e) => FileSetEverything(true);

            _nothingButton = splitContainerFile.Panel1.Controls["buttonNothing"] as Button;
            _nothingButton.Click += (sender, e) => FileSetEverything(false);

            _numStarsButton = splitContainerFile.Panel1.Controls["buttonFileNumStars"] as Button;
            _numStarsButton.Click += NumStarsButton_Click;
        }

        private short CalculateNumStars()
        {
            short starCount = 0;
            byte starByte;
            
            // go through the 25 contiguous star bytes
            for (int i = 0; i < 25; i++)
            {
                starByte = _stream.GetByte(CurrentFileAddress + Config.File.CourseStarsAddressStart + (uint)i);
                for (int b = 0; b < 7; b++)
                {
                    starCount += (byte)((starByte >> b) & 1);
                }
            }

            // go through the 1 non-contiguous star byte (for toads and MIPS)
            starByte = _stream.GetByte(CurrentFileAddress + Config.File.ToadMIPSStarsAddress);
            for (int b = 0; b < 7; b++)
            {
                starCount += (byte)((starByte >> b) & 1);
            }

            return starCount;
        }

        private (Image onImage, Image offImage) GetDoorImages(int row)
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

        private uint GetDoorAddressOffset(int row)
        {
            switch (row)
            {
                case 1:
                    return Config.File.WFDoorAddress;
                case 2:
                    return Config.File.JRBDoorAddress;
                case 3:
                    return Config.File.CCMDoorAddress;
                case 18:
                    return Config.File.PSSDoorAddress;
                case 21:
                    return Config.File.BitDWDoorAddress;
                case 22:
                    return Config.File.BitFSDoorAddress;
                case 23:
                    return Config.File.BitSDoorAddress;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private byte GetDoorMask(int row)
        {
            switch (row)
            {
                case 1:
                    return Config.File.WFDoorMask;
                case 2:
                    return Config.File.JRBDoorMask;
                case 3:
                    return Config.File.CCMDoorMask;
                case 18:
                    return Config.File.PSSDoorMask;
                case 21:
                    return Config.File.BitDWDoorMask;
                case 22:
                    return Config.File.BitFSDoorMask;
                case 23:
                    return Config.File.BitSDoorMask;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private uint GetCannonAddressOffset(int row)
        {
            if (row == 20)
                return Config.File.WMotRCannonAddress;
            else
                return Config.File.MainCourseCannonsAddressStart + (uint)row;
        }

        private uint GetStarAddressOffset(int row)
        {
            switch (row)
            {
                default:
                    return Config.File.CourseStarsAddressStart + (uint)row;
                case 15:
                    return Config.File.TotWCStarAddress;
                case 16:
                    return Config.File.CotMCStarAddress;
                case 17:
                    return Config.File.VCutMStarAddress;
                case 18:
                    return Config.File.PSSStarsAddress;
                case 19:
                    return Config.File.SAStarAddress;
                case 20:
                    return Config.File.WMotRStarAddress;
                case 21:
                    return Config.File.BitDWStarAddress;
                case 22:
                    return Config.File.BitFSStarAddress;
                case 23:
                    return Config.File.BitSStarAddress;
                case 24:
                case 25:
                    return Config.File.ToadMIPSStarsAddress;
            }
        }

        private byte GetStarMask(int row, int col)
        {
            int bitOffset = row == 25 ? 3 : 0;
            return (byte)Math.Pow(2, col + bitOffset);
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

        private void FileEraseButton_Click(object sender, EventArgs e)
        {
            // Get the corresponding unsaved and saved file struct address
            uint nonSavedAddress = GetNonSavedFileAddress(CurrentFileMode);
            uint savedAddress = nonSavedAddress + Config.File.FileStructSize;

            // Get checksum value
            ushort checksum = (ushort)(Config.File.ChecksumConstantValue % 256 + Config.File.ChecksumConstantValue / 256);

            // Set the checksum constant and checksum (in both unsaved and saved)
            _stream.SetValue(Config.File.ChecksumConstantValue, nonSavedAddress + Config.File.ChecksumConstantOffset);
            _stream.SetValue(Config.File.ChecksumConstantValue, savedAddress + Config.File.ChecksumConstantOffset);
            _stream.SetValue(checksum, nonSavedAddress + Config.File.ChecksumOffset);
            _stream.SetValue(checksum, savedAddress + Config.File.ChecksumOffset);

            // Set all bytes to 0 (in both unsaved and saved)
            for (uint i = 0; i < Config.File.FileStructSize - 4; i++)
            {
                _stream.SetValue((byte)0, nonSavedAddress + i);
                _stream.SetValue((byte)0, savedAddress + i);
            }
        }

        private byte[] GetBufferedBytes()
        {
            byte[] bufferedBytes = new byte[Config.File.FileStructSize];
            for (int i = 0; i < Config.File.FileStructSize; i++)
            {
                bufferedBytes[i] = _stream.GetByte(CurrentFileAddress + (uint)i);
            }
            return bufferedBytes;
        }

        private void SetBufferedBytes(byte[] bufferedBytes)
        {
            for (int i = 0; i < Config.File.FileStructSize; i++)
            {
                _stream.SetValue(bufferedBytes[i], CurrentFileAddress + (uint)i);
            }
        }

        private void FileSetStars(bool starsOn)
        {
            byte[] bufferedBytes = GetBufferedBytes();
            for (int i = 0; i < numRows; i++)
            {
                uint courseAddressOffset = _courseStarsAddressOffsets[i];
                byte courseMask = _courseStarsMasks[i];

                byte oldByte = bufferedBytes[courseAddressOffset];
                byte newByte = MoreMath.ApplyValueToMaskedByte(oldByte, courseMask, starsOn);
                bufferedBytes[courseAddressOffset] = newByte;
            }
            SetBufferedBytes(bufferedBytes);
        }

        private void FileSetEverything(bool everythingOn)
        {
            byte[] bufferedBytes = GetBufferedBytes();

            Action<uint?, byte?, bool?> setValues = (uint? addressOffset, byte? mask, bool? newVal) =>
            {
                if (addressOffset == null || mask == null || newVal == null) return;
                byte oldByte = bufferedBytes[(uint)addressOffset];
                byte newByte = MoreMath.ApplyValueToMaskedByte(oldByte, (byte)mask, (bool)newVal);
                bufferedBytes[(uint)addressOffset] = newByte;
            };

            for (int i = 0; i < numRows; i++)
            {
                setValues(_courseStarsAddressOffsets[i], _courseStarsMasks[i], everythingOn);
                setValues(_courseCannonAddressOffsets[i], _courseCannonMasks[i], everythingOn);
                setValues(_courseDoorAddressOffsets[i], _courseDoorMasks[i], everythingOn);
            }

            for (int i = 0; i < 15; i++)
            {
                bufferedBytes[Config.File.CoinScoreAddressStart + (uint)i] = everythingOn ? (byte)100 : (byte)0;
            }

            setValues(Config.File.FileStartedAddress, Config.File.FileStartedMask, everythingOn ? true : (bool?)null);
            setValues(Config.File.CapSwitchPressedAddress, Config.File.RedCapSwitchMask, everythingOn);
            setValues(Config.File.CapSwitchPressedAddress, Config.File.GreenCapSwitchMask, everythingOn);
            setValues(Config.File.CapSwitchPressedAddress, Config.File.BlueCapSwitchMask, everythingOn);
            setValues(Config.File.KeyDoorAddress, Config.File.KeyDoor1KeyMask, false);
            setValues(Config.File.KeyDoorAddress, Config.File.KeyDoor1OpenedMask, everythingOn);
            setValues(Config.File.KeyDoorAddress, Config.File.KeyDoor2KeyMask, false);
            setValues(Config.File.KeyDoorAddress, Config.File.KeyDoor2OpenedMask, everythingOn);
            setValues(Config.File.MoatDrainedAddress, Config.File.MoatDrainedMask, everythingOn);
            setValues(Config.File.DDDMovedBackAddress, Config.File.DDDMovedBackMask, everythingOn);
            setValues(Config.File.HatLocationModeOffset, Config.File.HatLocationModeMask, everythingOn ? false : (bool?)null);

            SetBufferedBytes(bufferedBytes);
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

        public override void Update(bool updateView)
        {
            short currentNumStars = CalculateNumStars();
            _numStarsButton.Text = string.Format("Update HUD\r\nto " + (currentNumStars == 1 ? currentNumStars + " Star" : currentNumStars + " Stars"));

            foreach (FilePictureBox filePictureBox in _filePictureBoxList)
            {
                filePictureBox.UpdateImage();
            }

            foreach (FileTextbox fileTextbox in _fileTextboxList)
            {
                fileTextbox.UpdateText();
            }

            base.Update(updateView);
        }
    }
}

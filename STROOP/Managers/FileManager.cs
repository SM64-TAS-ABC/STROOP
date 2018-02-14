using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;
using System.Windows.Forms;
using STROOP.Utilities;
using STROOP.Controls;
using STROOP.Structs.Configurations;
using System.Drawing;

namespace STROOP.Managers
{
    public class FileManager : DataManager
    {
        public enum FileMode { FileA, FileB, FileC, FileD, FileASaved, FileBSaved, FileCSaved, FileDSaved };
        private enum AllCoinsMeaning { Coins100, Coins255, MaxWithoutGlitches, MaxWithGlitches };
        private enum FileCategory { Stars, Cannons, Doors, Coins, Misc };

        TabPage _tabControl;
        FileImageGui _gui;

        public FileMode CurrentFileMode { get; private set; }
        public uint CurrentFileAddress
        {
            get
            {
                return getFileAddress();
            }
        }

        private AllCoinsMeaning currentAllCoinsMeaning;

        List<FilePictureBox> _filePictureBoxList;
        List<FileTextbox> _fileTextboxList;

        CheckBox _inGameCopyPasteCheckbox;
        Button _numStarsButton;

        int numRows = 26;

        // Keep track of each row's address and masks
        uint[] _courseStarsAddressOffsets;
        byte[] _courseStarsMasks;
        uint?[] _courseCannonAddressOffsets;
        byte?[] _courseCannonMasks;
        uint?[] _courseDoorAddressOffsets;
        byte?[] _courseDoorMasks;

        byte[] _copiedFile;

        public FileManager(List<WatchVariableControlPrecursor> variables, TabPage tabControl, WatchVariableFlowLayoutPanel watchVariablePanelFile, FileImageGui gui)
            : base(variables, watchVariablePanelFile)
        {
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
            currentAllCoinsMeaning = AllCoinsMeaning.Coins100;

            SplitContainer splitContainerFile = tabControl.Controls["splitContainerFile"] as SplitContainer;

            GroupBox fileGroupbox = splitContainerFile.Panel1.Controls["groupBoxFile"] as GroupBox;
            (fileGroupbox.Controls["radioButtonFileA"] as RadioButton).Click
                += (sender, e) => FileMode_Click(FileMode.FileA);
            (fileGroupbox.Controls["radioButtonFileB"] as RadioButton).Click
                += (sender, e) => FileMode_Click(FileMode.FileB);
            (fileGroupbox.Controls["radioButtonFileC"] as RadioButton).Click
                += (sender, e) => FileMode_Click(FileMode.FileC);
            (fileGroupbox.Controls["radioButtonFileD"] as RadioButton).Click
                += (sender, e) => FileMode_Click(FileMode.FileD);
            (fileGroupbox.Controls["radioButtonFileASaved"] as RadioButton).Click
                += (sender, e) => FileMode_Click(FileMode.FileASaved);
            (fileGroupbox.Controls["radioButtonFileBSaved"] as RadioButton).Click
                += (sender, e) => FileMode_Click(FileMode.FileBSaved);
            (fileGroupbox.Controls["radioButtonFileCSaved"] as RadioButton).Click
                += (sender, e) => FileMode_Click(FileMode.FileCSaved);
            (fileGroupbox.Controls["radioButtonFileDSaved"] as RadioButton).Click
                += (sender, e) => FileMode_Click(FileMode.FileDSaved);

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
                    string missionName = TableConfig.Missions.GetMissionName(row + 1, col + 1);
                    fileStarPictureBox.Initialize(_gui, addressOffset, mask, _gui.PowerStarImage, _gui.PowerStarBlackImage, missionName);
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
                fileCourseLabel.Initialize(_courseStarsAddressOffsets[row], _courseStarsMasks[row], row + 1);
            }

            // cannons
            for (int row = 0; row < numRows; row++)
            {
                int col = 7;
                string controlName = String.Format("filePictureBoxTableRow{0}Col{1}", row + 1, col + 1);
                FileBinaryPictureBox fileCannonPictureBox = fileTable.Controls[controlName] as FileBinaryPictureBox;
                if (fileCannonPictureBox == null) continue;

                uint addressOffset = GetCannonAddressOffset(row);
                byte mask = FileConfig.CannonMask;
                fileCannonPictureBox.Initialize(addressOffset, mask, _gui.CannonImage, _gui.CannonLidImage);
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
                fileBinaryPictureBox.Initialize(addressOffset, mask, onImage, offImage);
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
                fileCoinScoreTextBox.Initialize(FileConfig.CoinScoreOffsetStart + (uint)row);
                _fileTextboxList.Add(fileCoinScoreTextBox);
            }

            GroupBox hatLocationGroupbox = splitContainerFile.Panel1.Controls["groupBoxHatLocation"] as GroupBox;

            // hat location radio button pictures
            FileHatLocationPictureBox filePictureBoxHatLocationMario = hatLocationGroupbox.Controls["filePictureBoxHatLocationMario"] as FileHatLocationPictureBox;
            filePictureBoxHatLocationMario.Initialize(HatLocation.Mario, _gui.HatOnMarioImage, _gui.HatOnMarioGreyImage);
            _filePictureBoxList.Add(filePictureBoxHatLocationMario);

            FileHatLocationPictureBox filePictureBoxHatLocationKlepto = hatLocationGroupbox.Controls["filePictureBoxHatLocationKlepto"] as FileHatLocationPictureBox;
            filePictureBoxHatLocationKlepto.Initialize(HatLocation.SSLKlepto, _gui.HatOnKleptoImage, _gui.HatOnKleptoGreyImage);
            _filePictureBoxList.Add(filePictureBoxHatLocationKlepto);

            FileHatLocationPictureBox filePictureBoxHatLocationSnowman = hatLocationGroupbox.Controls["filePictureBoxHatLocationSnowman"] as FileHatLocationPictureBox;
            filePictureBoxHatLocationSnowman.Initialize(HatLocation.SLSnowman, _gui.HatOnSnowmanImage, _gui.HatOnSnowmanGreyImage);
            _filePictureBoxList.Add(filePictureBoxHatLocationSnowman);

            FileHatLocationPictureBox filePictureBoxHatLocationUkiki = hatLocationGroupbox.Controls["filePictureBoxHatLocationUkiki"] as FileHatLocationPictureBox;
            filePictureBoxHatLocationUkiki.Initialize(HatLocation.TTMUkiki, _gui.HatOnUkikiImage, _gui.HatOnUkikiGreyImage);
            _filePictureBoxList.Add(filePictureBoxHatLocationUkiki);

            FileHatLocationPictureBox filePictureBoxHatLocationSSLGround = hatLocationGroupbox.Controls["filePictureBoxHatLocationSSLGround"] as FileHatLocationPictureBox;
            filePictureBoxHatLocationSSLGround.Initialize(HatLocation.SSLGround, _gui.HatOnGroundInSSLImage, _gui.HatOnGroundInSSLGreyImage);
            _filePictureBoxList.Add(filePictureBoxHatLocationSSLGround);

            FileHatLocationPictureBox filePictureBoxHatLocationSLGround = hatLocationGroupbox.Controls["filePictureBoxHatLocationSLGround"] as FileHatLocationPictureBox;
            filePictureBoxHatLocationSLGround.Initialize(HatLocation.SLGround, _gui.HatOnGroundInSLImage, _gui.HatOnGroundInSLGreyImage);
            _filePictureBoxList.Add(filePictureBoxHatLocationSLGround);

            FileHatLocationPictureBox filePictureBoxHatLocationTTMGround = hatLocationGroupbox.Controls["filePictureBoxHatLocationTTMGround"] as FileHatLocationPictureBox;
            filePictureBoxHatLocationTTMGround.Initialize(HatLocation.TTMGround, _gui.HatOnGroundInTTMImage, _gui.HatOnGroundInTTMGreyImage);
            _filePictureBoxList.Add(filePictureBoxHatLocationTTMGround);

            // hat position textboxes
            FileHatPositionTextbox textboxHatLocationPositionX = hatLocationGroupbox.Controls["textboxHatLocationPositionX"] as FileHatPositionTextbox;
            textboxHatLocationPositionX.Initialize(FileConfig.HatPositionXOffset);
            _fileTextboxList.Add(textboxHatLocationPositionX);

            FileHatPositionTextbox textboxHatLocationPositionY = hatLocationGroupbox.Controls["textboxHatLocationPositionY"] as FileHatPositionTextbox;
            textboxHatLocationPositionY.Initialize(FileConfig.HatPositionYOffset);
            _fileTextboxList.Add(textboxHatLocationPositionY);

            FileHatPositionTextbox textboxHatLocationPositionZ = hatLocationGroupbox.Controls["textboxHatLocationPositionZ"] as FileHatPositionTextbox;
            textboxHatLocationPositionZ.Initialize(FileConfig.HatPositionZOffset);
            _fileTextboxList.Add(textboxHatLocationPositionZ);

            // miscellaneous checkbox pictures
            FileBinaryPictureBox filePictureBoxFileStarted = splitContainerFile.Panel1.Controls["filePictureBoxFileStarted"] as FileBinaryPictureBox;
            filePictureBoxFileStarted.Initialize(FileConfig.FileStartedOffset, FileConfig.FileStartedMask, _gui.FileStartedImage, _gui.FileNotStartedImage);
            _filePictureBoxList.Add(filePictureBoxFileStarted);

            FileBinaryPictureBox filePictureBoxRedCapSwitchPressed = splitContainerFile.Panel1.Controls["filePictureBoxRedCapSwitchPressed"] as FileBinaryPictureBox;
            filePictureBoxRedCapSwitchPressed.Initialize(FileConfig.CapSwitchPressedOffset, FileConfig.RedCapSwitchMask, _gui.CapSwitchRedPressedImage, _gui.CapSwitchRedUnpressedImage);
            _filePictureBoxList.Add(filePictureBoxRedCapSwitchPressed);

            FileBinaryPictureBox filePictureBoxGreenCapSwitchPressed = splitContainerFile.Panel1.Controls["filePictureBoxGreenCapSwitchPressed"] as FileBinaryPictureBox;
            filePictureBoxGreenCapSwitchPressed.Initialize(FileConfig.CapSwitchPressedOffset, FileConfig.GreenCapSwitchMask, _gui.CapSwitchGreenPressedImage, _gui.CapSwitchGreenUnpressedImage);
            _filePictureBoxList.Add(filePictureBoxGreenCapSwitchPressed);

            FileBinaryPictureBox filePictureBoxBlueCapSwitchPressed = splitContainerFile.Panel1.Controls["filePictureBoxBlueCapSwitchPressed"] as FileBinaryPictureBox;
            filePictureBoxBlueCapSwitchPressed.Initialize(FileConfig.CapSwitchPressedOffset, FileConfig.BlueCapSwitchMask, _gui.CapSwitchBluePressedImage, _gui.CapSwitchBlueUnpressedImage);
            _filePictureBoxList.Add(filePictureBoxBlueCapSwitchPressed);

            FileKeyDoorPictureBox filePictureBoxKeyDoor1Opened = splitContainerFile.Panel1.Controls["filePictureBoxKeyDoor1Opened"] as FileKeyDoorPictureBox;
            filePictureBoxKeyDoor1Opened.Initialize(FileConfig.KeyDoorOffset, FileConfig.KeyDoor1KeyMask, FileConfig.KeyDoor1OpenedMask, _gui.KeyDoorOpenKeyImage, _gui.KeyDoorClosedKeyImage, _gui.KeyDoorOpenImage, _gui.KeyDoorClosedImage);
            _filePictureBoxList.Add(filePictureBoxKeyDoor1Opened);

            FileKeyDoorPictureBox filePictureBoxKeyDoor2Opened = splitContainerFile.Panel1.Controls["filePictureBoxKeyDoor2Opened"] as FileKeyDoorPictureBox;
            filePictureBoxKeyDoor2Opened.Initialize(FileConfig.KeyDoorOffset, FileConfig.KeyDoor2KeyMask, FileConfig.KeyDoor2OpenedMask, _gui.KeyDoorOpenKeyImage, _gui.KeyDoorClosedKeyImage, _gui.KeyDoorOpenImage, _gui.KeyDoorClosedImage);
            _filePictureBoxList.Add(filePictureBoxKeyDoor2Opened);

            FileBinaryPictureBox filePictureBoxMoatDrained = splitContainerFile.Panel1.Controls["filePictureBoxMoatDrained"] as FileBinaryPictureBox;
            filePictureBoxMoatDrained.Initialize(FileConfig.MoatDrainedOffset, FileConfig.MoatDrainedMask, _gui.MoatDrainedImage, _gui.MoatNotDrainedImage);
            _filePictureBoxList.Add(filePictureBoxMoatDrained);

            FileBinaryPictureBox filePictureBoxDDDMovedBack = splitContainerFile.Panel1.Controls["filePictureBoxDDDMovedBack"] as FileBinaryPictureBox;
            filePictureBoxDDDMovedBack.Initialize(FileConfig.DDDMovedBackOffset, FileConfig.DDDMovedBackMask, _gui.DDDPaintingMovedBackImage, _gui.DDDPaintingNotMovedBackImage);
            _filePictureBoxList.Add(filePictureBoxDDDMovedBack);

            //checkbox
            _inGameCopyPasteCheckbox = splitContainerFile.Panel1.Controls["checkboxInGameCopyPaste"] as CheckBox;

            // buttons
            Button saveFileButton = splitContainerFile.Panel1.Controls["buttonFileSave"] as Button;
            saveFileButton.Click += FileSaveButton_Click;

            Button eraseFileButton = splitContainerFile.Panel1.Controls["buttonFileErase"] as Button;
            eraseFileButton.Click += FileEraseButton_Click;

            Button copyFileButton = splitContainerFile.Panel1.Controls["buttonFileCopy"] as Button;
            copyFileButton.Click += FileCopyButton_Click;

            Button pasteFileButton = splitContainerFile.Panel1.Controls["buttonFilePaste"] as Button;
            pasteFileButton.Click += FilePasteButton_Click;

            Button allStarsButton = splitContainerFile.Panel1.Controls["buttonAllStars"] as Button;
            allStarsButton.Click += (sender, e) => FileSetCategory(true, new List<FileCategory> { FileCategory.Stars });

            Button noStarsButton = splitContainerFile.Panel1.Controls["buttonNoStars"] as Button;
            noStarsButton.Click += (sender, e) => FileSetCategory(false, new List<FileCategory> { FileCategory.Stars });

            Button allCannonsButton = splitContainerFile.Panel1.Controls["buttonAllCannons"] as Button;
            allCannonsButton.Click += (sender, e) => FileSetCategory(true, new List<FileCategory> { FileCategory.Cannons });

            Button noCannonsButton = splitContainerFile.Panel1.Controls["buttonNoCannons"] as Button;
            noCannonsButton.Click += (sender, e) => FileSetCategory(false, new List<FileCategory> { FileCategory.Cannons });

            Button allDoorsButton = splitContainerFile.Panel1.Controls["buttonAllDoors"] as Button;
            allDoorsButton.Click += (sender, e) => FileSetCategory(true, new List<FileCategory> { FileCategory.Doors });

            Button noDoorsButton = splitContainerFile.Panel1.Controls["buttonNoDoors"] as Button;
            noDoorsButton.Click += (sender, e) => FileSetCategory(false, new List<FileCategory> { FileCategory.Doors });

            Button allCoinsButton = splitContainerFile.Panel1.Controls["buttonAllCoins"] as Button;
            allCoinsButton.Click += (sender, e) => FileSetCategory(true, new List<FileCategory> { FileCategory.Coins });

            Button noCoinsButton = splitContainerFile.Panel1.Controls["buttonNoCoins"] as Button;
            noCoinsButton.Click += (sender, e) => FileSetCategory(false, new List<FileCategory> { FileCategory.Coins });

            Button everythingButton = splitContainerFile.Panel1.Controls["buttonEverything"] as Button;
            everythingButton.Click += (sender, e) =>
                FileSetCategory(
                    true,
                    new List<FileCategory>
                    {
                        FileCategory.Stars,
                        FileCategory.Cannons,
                        FileCategory.Doors,
                        FileCategory.Coins,
                        FileCategory.Misc
                    });

            Button nothingButton = splitContainerFile.Panel1.Controls["buttonNothing"] as Button;
            nothingButton.Click += (sender, e) =>
                FileSetCategory(
                    false,
                    new List<FileCategory>
                    {
                        FileCategory.Stars,
                        FileCategory.Cannons,
                        FileCategory.Doors,
                        FileCategory.Coins,
                        FileCategory.Misc
                    });

            _numStarsButton = splitContainerFile.Panel1.Controls["buttonFileNumStars"] as Button;
            _numStarsButton.Click += NumStarsButton_Click;

            // everything coin score radio buttons
            GroupBox allCoinsMeaningGroupbox = splitContainerFile.Panel1.Controls["groupBoxAllCoinsMeaning"] as GroupBox;
            (allCoinsMeaningGroupbox.Controls["radioButtonAllCoinsMeaning100Coins"] as RadioButton).Click
                += (sender, e) => { currentAllCoinsMeaning = AllCoinsMeaning.Coins100; };
            (allCoinsMeaningGroupbox.Controls["radioButtonAllCoinsMeaning255Coins"] as RadioButton).Click
                += (sender, e) => { currentAllCoinsMeaning = AllCoinsMeaning.Coins255; };
            (allCoinsMeaningGroupbox.Controls["radioButtonAllCoinsMeaningMaxWithoutGlitches"] as RadioButton).Click
                += (sender, e) => { currentAllCoinsMeaning = AllCoinsMeaning.MaxWithoutGlitches; };
            (allCoinsMeaningGroupbox.Controls["radioButtonAllCoinsMeaningMaxWithGlitches"] as RadioButton).Click
                += (sender, e) => { currentAllCoinsMeaning = AllCoinsMeaning.MaxWithGlitches; };
        }

        private short CalculateNumStars()
        {
            short starCount = 0;
            byte starByte;
            
            // go through the 25 contiguous star bytes
            for (int i = 0; i < 25; i++)
            {
                starByte = Config.Stream.GetByte(CurrentFileAddress + FileConfig.CourseStarsOffsetStart + (uint)i);
                for (int b = 0; b < 7; b++)
                {
                    starCount += (byte)((starByte >> b) & 1);
                }
            }

            // go through the 1 non-contiguous star byte (for toads and MIPS)
            starByte = Config.Stream.GetByte(CurrentFileAddress + FileConfig.ToadMIPSStarsOffset);
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
                    return FileConfig.WFDoorOffset;
                case 2:
                    return FileConfig.JRBDoorOffset;
                case 3:
                    return FileConfig.CCMDoorOffset;
                case 18:
                    return FileConfig.PSSDoorOffset;
                case 21:
                    return FileConfig.BitDWDoorOffset;
                case 22:
                    return FileConfig.BitFSDoorOffset;
                case 23:
                    return FileConfig.BitSDoorOffset;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private byte GetDoorMask(int row)
        {
            switch (row)
            {
                case 1:
                    return FileConfig.WFDoorMask;
                case 2:
                    return FileConfig.JRBDoorMask;
                case 3:
                    return FileConfig.CCMDoorMask;
                case 18:
                    return FileConfig.PSSDoorMask;
                case 21:
                    return FileConfig.BitDWDoorMask;
                case 22:
                    return FileConfig.BitFSDoorMask;
                case 23:
                    return FileConfig.BitSDoorMask;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private uint GetCannonAddressOffset(int row)
        {
            if (row == 20)
                return FileConfig.WMotRCannonOffset;
            else
                return FileConfig.MainCourseCannonsOffsetStart + (uint)row;
        }

        private uint GetStarAddressOffset(int row)
        {
            switch (row)
            {
                default:
                    return FileConfig.CourseStarsOffsetStart + (uint)row;
                case 15:
                    return FileConfig.TotWCStarOffset;
                case 16:
                    return FileConfig.CotMCStarOffset;
                case 17:
                    return FileConfig.VCutMStarOffset;
                case 18:
                    return FileConfig.PSSStarsOffset;
                case 19:
                    return FileConfig.SAStarOffset;
                case 20:
                    return FileConfig.WMotRStarOffset;
                case 21:
                    return FileConfig.BitDWStarOffset;
                case 22:
                    return FileConfig.BitFSStarOffset;
                case 23:
                    return FileConfig.BitSStarOffset;
                case 24:
                case 25:
                    return FileConfig.ToadMIPSStarsOffset;
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
            Config.Stream.SetValue(numStars, MarioConfig.StructAddress + HudConfig.StarCountOffset);
            Config.Stream.SetValue(numStars, MarioConfig.StructAddress + HudConfig.StarDisplayOffset);
        }

        public ushort GetChecksum(uint? nullableFileAddress = null)
        {
            uint fileAddress = nullableFileAddress ?? CurrentFileAddress;
            ushort checksum = (ushort)(FileConfig.ChecksumConstantValue % 256 + FileConfig.ChecksumConstantValue / 256);
            for (uint i = 0; i < FileConfig.FileStructSize - 4; i++)
            {
                byte b = Config.Stream.GetByte(fileAddress + i);
                checksum += b;
            }
            return checksum;
        }

        private void FileSaveButton_Click(object sender, EventArgs e)
        {
            // Get the corresponding unsaved file struct address
            uint nonSavedAddress = GetNonSavedFileAddress();

            // Set the checksum constant
            Config.Stream.SetValue(FileConfig.ChecksumConstantValue, nonSavedAddress + FileConfig.ChecksumConstantOffset);

            // Sum up all bytes to calculate the checksum
            ushort checksum = (ushort)(FileConfig.ChecksumConstantValue % 256 + FileConfig.ChecksumConstantValue / 256);
            for (uint i = 0; i < FileConfig.FileStructSize-4; i++)
            {
                byte b = Config.Stream.GetByte(nonSavedAddress + i);
                checksum += b;
            }

            // Set the checksum
            Config.Stream.SetValue(checksum, nonSavedAddress + FileConfig.ChecksumOffset);

            // Copy all values from the unsaved struct to the saved struct
            uint savedAddress = nonSavedAddress + FileConfig.FileStructSize;
            for (uint i = 0; i < FileConfig.FileStructSize - 4; i++)
            {
                byte b = Config.Stream.GetByte(nonSavedAddress + i);
                Config.Stream.SetValue(b, savedAddress + i);
            }
            Config.Stream.SetValue(FileConfig.ChecksumConstantValue, savedAddress + FileConfig.ChecksumConstantOffset);
            Config.Stream.SetValue(checksum, savedAddress + FileConfig.ChecksumOffset);
        }

        private void FileEraseButton_Click(object sender, EventArgs e)
        {
            // Get the corresponding unsaved and saved file struct address
            uint nonSavedAddress = GetNonSavedFileAddress();
            uint savedAddress = nonSavedAddress + FileConfig.FileStructSize;

            // Get checksum value
            ushort checksum = (ushort)(FileConfig.ChecksumConstantValue % 256 + FileConfig.ChecksumConstantValue / 256);

            // Set the checksum constant and checksum (in both unsaved and saved)
            Config.Stream.SetValue(FileConfig.ChecksumConstantValue, nonSavedAddress + FileConfig.ChecksumConstantOffset);
            Config.Stream.SetValue(FileConfig.ChecksumConstantValue, savedAddress + FileConfig.ChecksumConstantOffset);
            Config.Stream.SetValue(checksum, nonSavedAddress + FileConfig.ChecksumOffset);
            Config.Stream.SetValue(checksum, savedAddress + FileConfig.ChecksumOffset);

            // Set all bytes to 0 (in both unsaved and saved)
            for (uint i = 0; i < FileConfig.FileStructSize - 4; i++)
            {
                Config.Stream.SetValue((byte)0, nonSavedAddress + i);
                Config.Stream.SetValue((byte)0, savedAddress + i);
            }
        }

        private void FileCopyButton_Click(object sender, EventArgs e)
        {
            uint addressToCopy = _inGameCopyPasteCheckbox.Checked ?
                GetNonSavedFileAddress() :
                getFileAddress();
            _copiedFile = GetBufferedBytes(addressToCopy);
        }

        private void FilePasteButton_Click(object sender, EventArgs e)
        {
            if (_copiedFile == null) return;

            uint nonSavedAddress = GetNonSavedFileAddress();
            List<uint> addressesToPaste = _inGameCopyPasteCheckbox.Checked ?
                new List<uint> { nonSavedAddress, nonSavedAddress + FileConfig.FileStructSize } :
                new List<uint> { CurrentFileAddress };

            foreach (uint addressToPaste in addressesToPaste)
            {
                SetBufferedBytes(_copiedFile, addressToPaste);
            }
        }

        public byte[] GetBufferedBytes(uint? nullableFileAddress = null)
        {
            uint fileAddress = nullableFileAddress ?? CurrentFileAddress;
            byte[] bufferedBytes = new byte[FileConfig.FileStructSize];
            for (int i = 0; i < FileConfig.FileStructSize; i++)
            {
                bufferedBytes[i] = Config.Stream.GetByte(fileAddress + (uint)i);
            }
            return bufferedBytes;
        }

        public void SetBufferedBytes(byte[] bufferedBytes, uint? nullableFileAddress = null)
        {
            uint fileAddress = nullableFileAddress ?? CurrentFileAddress;
            for (int i = 0; i < FileConfig.FileStructSize; i++)
            {
                Config.Stream.SetValue(bufferedBytes[i], fileAddress + (uint)i);
            }
        }

        private byte GetCoinScoreForCourse(int courseIndex)
        {
            switch (currentAllCoinsMeaning)
            {
                case AllCoinsMeaning.Coins100:
                    return 100;
                case AllCoinsMeaning.Coins255:
                    return 255;
                case AllCoinsMeaning.MaxWithoutGlitches:
                    return (byte)TableConfig.CourseData.GetMaxCoinsWithoutGlitches(courseIndex);
                case AllCoinsMeaning.MaxWithGlitches:
                    return (byte)TableConfig.CourseData.GetMaxCoinsWithGlitches(courseIndex);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void FileSetCategory(bool setOn, List<FileCategory> fileCategories)
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
                if (fileCategories.Contains(FileCategory.Stars))
                {
                    setValues(_courseStarsAddressOffsets[i], _courseStarsMasks[i], setOn);
                }
                if (fileCategories.Contains(FileCategory.Cannons))
                {
                    setValues(_courseCannonAddressOffsets[i], _courseCannonMasks[i], setOn);
                }
                if (fileCategories.Contains(FileCategory.Doors))
                {
                    setValues(_courseDoorAddressOffsets[i], _courseDoorMasks[i], setOn);
                }
            }

            if (fileCategories.Contains(FileCategory.Coins))
            {
                for (int i = 0; i < 15; i++)
                {
                    bufferedBytes[FileConfig.CoinScoreOffsetStart + (uint)i] = setOn ? GetCoinScoreForCourse(i + 1) : (byte)0;
                }
            }

            if (fileCategories.Contains(FileCategory.Doors))
            {
                setValues(FileConfig.KeyDoorOffset, FileConfig.KeyDoor1KeyMask, false);
                setValues(FileConfig.KeyDoorOffset, FileConfig.KeyDoor1OpenedMask, setOn);
                setValues(FileConfig.KeyDoorOffset, FileConfig.KeyDoor2KeyMask, false);
                setValues(FileConfig.KeyDoorOffset, FileConfig.KeyDoor2OpenedMask, setOn);
            }

            if (fileCategories.Contains(FileCategory.Misc))
            {
                setValues(FileConfig.FileStartedOffset, FileConfig.FileStartedMask, setOn);
                setValues(FileConfig.CapSwitchPressedOffset, FileConfig.RedCapSwitchMask, setOn);
                setValues(FileConfig.CapSwitchPressedOffset, FileConfig.GreenCapSwitchMask, setOn);
                setValues(FileConfig.CapSwitchPressedOffset, FileConfig.BlueCapSwitchMask, setOn);
                setValues(FileConfig.MoatDrainedOffset, FileConfig.MoatDrainedMask, setOn);
                setValues(FileConfig.DDDMovedBackOffset, FileConfig.DDDMovedBackMask, setOn);
                setValues(FileConfig.HatLocationModeOffset, FileConfig.HatLocationModeMask, false);
            }

            SetBufferedBytes(bufferedBytes);
        }

        private uint GetNonSavedFileAddress(FileMode? nullableMode = null)
        {
            FileMode mode = nullableMode ?? CurrentFileMode;
            switch (mode)
            {
                case FileMode.FileA:
                case FileMode.FileASaved:
                    return FileConfig.FileAAddress;
                case FileMode.FileB:
                case FileMode.FileBSaved:
                    return FileConfig.FileBAddress;
                case FileMode.FileC:
                case FileMode.FileCSaved:
                    return FileConfig.FileCAddress;
                case FileMode.FileD:
                case FileMode.FileDSaved:
                    return FileConfig.FileDAddress;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private uint getFileAddress(FileMode? nullableMode = null)
        {
            FileMode mode = nullableMode ?? CurrentFileMode;
            switch (mode)
            {
                case FileMode.FileA:
                    return FileConfig.FileAAddress;
                case FileMode.FileB:
                    return FileConfig.FileBAddress;
                case FileMode.FileC:
                    return FileConfig.FileCAddress;
                case FileMode.FileD:
                    return FileConfig.FileDAddress;
                case FileMode.FileASaved:
                    return FileConfig.FileASavedAddress;
                case FileMode.FileBSaved:
                    return FileConfig.FileBSavedAddress;
                case FileMode.FileCSaved:
                    return FileConfig.FileCSavedAddress;
                case FileMode.FileDSaved:
                    return FileConfig.FileDSavedAddress;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public uint GetInGameFileAddress()
        {
            short inGameFile = Config.Stream.GetInt16(MiscConfig.CurrentFileAddress);
            switch (inGameFile)
            {
                case 1:
                    return FileConfig.FileAAddress;
                case 2:
                    return FileConfig.FileBAddress;
                case 3:
                    return FileConfig.FileCAddress;
                case 4:
                    return FileConfig.FileDAddress;
                default:
                    return FileConfig.FileAAddress;
            }
        }

        private void FileMode_Click(FileMode mode)
        {
            CurrentFileMode = mode;
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;

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

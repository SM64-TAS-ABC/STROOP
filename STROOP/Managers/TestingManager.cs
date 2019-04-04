using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System.Windows.Forms;
using STROOP.Utilities;
using STROOP.Forms;
using STROOP.Models;
using STROOP.Ttc;
using STROOP.Controls;

namespace STROOP.Managers
{
    public class TestingManager
    {
        // Conversion
        GroupBox _groupBoxTestingConversion;
        BetterTextbox _textBoxTestingConversionAddress;
        BetterTextbox _textBoxTestingConversionBytes;
        BetterTextbox _textBoxTestingConversionResult;
        Button _buttonTestingConversionConvert;

        // Control stick
        GroupBox _groupBoxControlStick;
        CheckBox _checkBoxUseInput;
        BetterTextbox _betterTextboxControlStick1;
        BetterTextbox _betterTextboxControlStick2;
        Label _labelControlStick1;
        Label _labelControlStick2;
        Label _labelControlStick3;
        Label _labelControlStick4;
        Label _labelControlStick5;
        Label _labelControlStick6;

        // Goto
        GroupBox _groupBoxGoto;
        BetterTextbox _betterTextboxGotoX;
        BetterTextbox _betterTextboxGotoY;
        BetterTextbox _betterTextboxGotoZ;
        Button _buttonGoto;
        Button _buttonGotoGetCurrent;
        Button _buttonPasteAndGoto;

        // State Transfer
        GroupBox _groupBoxStateTransfer;
        Button _buttonStateTransferInstructions;
        Button _buttonStateTransferSave;
        Button _buttonStateTransferApply;
        CheckBox _checkBoxStateTransferOffsetTimers;

        BetterTextbox _betterTextboxStateTransferVar1Current;
        BetterTextbox _betterTextboxStateTransferVar2Current;
        BetterTextbox _betterTextboxStateTransferVar3Current;
        BetterTextbox _betterTextboxStateTransferVar4Current;
        BetterTextbox _betterTextboxStateTransferVar5Current;
        BetterTextbox _betterTextboxStateTransferVar6Current;
        BetterTextbox _betterTextboxStateTransferVar7Current;
        BetterTextbox _betterTextboxStateTransferVar8Current;
        BetterTextbox _betterTextboxStateTransferVar9Current;
        BetterTextbox _betterTextboxStateTransferVar10Current;
        BetterTextbox _betterTextboxStateTransferVar11Current;
        BetterTextbox _betterTextboxStateTransferVar12Current;
        BetterTextbox _betterTextboxStateTransferVar13Current;
        BetterTextbox _betterTextboxStateTransferVar14Current;

        BetterTextbox _betterTextboxStateTransferVar1Saved;
        BetterTextbox _betterTextboxStateTransferVar2Saved;
        BetterTextbox _betterTextboxStateTransferVar3Saved;
        BetterTextbox _betterTextboxStateTransferVar4Saved;
        BetterTextbox _betterTextboxStateTransferVar5Saved;
        BetterTextbox _betterTextboxStateTransferVar6Saved;
        BetterTextbox _betterTextboxStateTransferVar7Saved;
        BetterTextbox _betterTextboxStateTransferVar8Saved;
        BetterTextbox _betterTextboxStateTransferVar9Saved;
        BetterTextbox _betterTextboxStateTransferVar10Saved;
        BetterTextbox _betterTextboxStateTransferVar11Saved;
        BetterTextbox _betterTextboxStateTransferVar12Saved;
        BetterTextbox _betterTextboxStateTransferVar13Saved;
        BetterTextbox _betterTextboxStateTransferVar14Saved;
        byte[] _stateTransferFileData;

        // Obj at HOLP
        GroupBox _groupBoxObjAtHOLP;
        CheckBox _checkBoxObjAtHOLPOn;
        BetterTextbox _betterTextboxObjAtHOLP;

        // Obj at Home
        GroupBox _groupBoxObjAtHome;
        CheckBox _checkBoxObjAtHomeOn;
        BetterTextbox _betterTextboxObjAtHomeObj;
        BetterTextbox _betterTextboxObjAtHomeHome;

        // Obj at Obj
        GroupBox _groupBoxObjAtObj;
        CheckBox _checkBoxObjAtObjOn;
        BetterTextbox _betterTextboxObjAtObj1;
        BetterTextbox _betterTextboxObjAtObj2;

        // Schedule
        GroupBox _groupBoxSchedule;
        Label _labelSchedule1;
        Label _labelSchedule2;
        Label _labelSchedule3;
        Label _labelSchedule4;
        Label _labelSchedule5;
        Label _labelSchedule6;
        Label _labelSchedule7;
        Label _labelScheduleIndex;
        Label _labelScheduleDescription;
        Button _buttonScheduleButtonSet;
        Button _buttonSchedulePrevious;
        Button _buttonScheduleNext;
        Button _buttonScheduleButtonReset;

        // Scuttlebug Stuff
        GroupBox _groupBoxScuttlebugStuff;
        RadioButton _radioButtonScuttlebugStuffBBHBalconyEye;
        RadioButton _radioButtonScuttlebugStuffBBHMerryGoRound;
        RadioButton _radioButtonScuttlebugStuffHMCAmazing;
        RadioButton _radioButtonScuttlebugStuffHMCRedCoins;
        Button _buttonScuttlebugStuffLungeToHome;
        Button _buttonScuttlebugStuff3rdFloor;
        Button _buttonScuttlebugStuff2ndFloor;
        Button _buttonScuttlebugStuff1stFloor;
        Button _buttonScuttlebugStuffBasement;
        BinaryButton _buttonScuttlebugStuffGetTris;

        // Tri Rooms
        TextBox _textBoxTriRoomsFromValue;
        TextBox _textBoxTriRoomsToValue;
        Button _buttonTriRoomsConvert;

        enum ScuttlebugMission
        {
            BBHBalconyEye,
            BBHMerryGoRound,
            HMCAmazing,
            HMCRedCoins,
        }
        ScuttlebugMission _scuttlebugMission = ScuttlebugMission.BBHBalconyEye;

        List<TriangleDataModel> _scuttlebugTriangleList = new List<TriangleDataModel>();

        // Memory Reader

        GroupBox _groupBoxMemoryReader;
        ComboBox _comboBoxMemoryReaderTypeValue;
        BetterTextbox _textBoxMemoryReaderAddressValue;
        BetterTextbox _textBoxMemoryReaderCountValue;
        Button _buttonMemoryReaderRead;

        // TTC Simulator
        BetterTextbox _textBoxTestingTtcSimulatorEndFrame;
        BetterTextbox _textBoxTestingTtcSimulatorDustFrames;
        Button _buttonTestingTtcSimulatorCalculate;

        // TTC Logger
        GroupBox _groupBoxTtcLogger;
        CheckBox _checkBoxTtcLoggerLogStates;
        Label _labelTtcLoggerStatus;
        BetterTextbox _textBoxTtcLoggerState;
        BetterTextbox _textBoxTtcLoggerLogs;
        Button _buttonTtcLoggerClear;
        string _lastTtcSaveState;
        HashSet<string> _ttcSaveStates;

        public TestingManager(TabPage tabControl)
        {
            // Conversion
            _groupBoxTestingConversion = tabControl.Controls["groupBoxTestingConversion"] as GroupBox;
            _textBoxTestingConversionAddress = _groupBoxTestingConversion.Controls["textBoxTestingConversionAddress"] as BetterTextbox;
            _textBoxTestingConversionBytes = _groupBoxTestingConversion.Controls["textBoxTestingConversionBytes"] as BetterTextbox;
            _textBoxTestingConversionResult = _groupBoxTestingConversion.Controls["textBoxTestingConversionResult"] as BetterTextbox;
            _buttonTestingConversionConvert = _groupBoxTestingConversion.Controls["buttonTestingConversionConvert"] as Button;
            _buttonTestingConversionConvert.Click += (sender, e) =>
            {
                uint? address = ParsingUtilities.ParseHexNullable(_textBoxTestingConversionAddress.Text);
                int? bytes = ParsingUtilities.ParseIntNullable(_textBoxTestingConversionBytes.Text);
                if (!address.HasValue || !bytes.HasValue) return;
                uint result = TypeUtilities.GetRelativeAddressFromAbsoluteAddress(address.Value, bytes.Value);
                string resultString = HexUtilities.FormatValue(result);
                _textBoxTestingConversionResult.Text = resultString;
            };

            // Control stick
            _groupBoxControlStick = tabControl.Controls["groupBoxControlStick"] as GroupBox;
            _checkBoxUseInput = _groupBoxControlStick.Controls["checkBoxUseInput"] as CheckBox;
            _betterTextboxControlStick1 = _groupBoxControlStick.Controls["betterTextboxControlStick1"] as BetterTextbox;
            _betterTextboxControlStick2 = _groupBoxControlStick.Controls["betterTextboxControlStick2"] as BetterTextbox;
            _labelControlStick1 = _groupBoxControlStick.Controls["labelControlStick1"] as Label;
            _labelControlStick2 = _groupBoxControlStick.Controls["labelControlStick2"] as Label;
            _labelControlStick3 = _groupBoxControlStick.Controls["labelControlStick3"] as Label;
            _labelControlStick4 = _groupBoxControlStick.Controls["labelControlStick4"] as Label;
            _labelControlStick5 = _groupBoxControlStick.Controls["labelControlStick5"] as Label;
            _labelControlStick6 = _groupBoxControlStick.Controls["labelControlStick6"] as Label;

            // Goto
            _groupBoxGoto = tabControl.Controls["groupBoxGoto"] as GroupBox;
            _betterTextboxGotoX = _groupBoxGoto.Controls["betterTextboxGotoX"] as BetterTextbox;
            _betterTextboxGotoY = _groupBoxGoto.Controls["betterTextboxGotoY"] as BetterTextbox;
            _betterTextboxGotoZ = _groupBoxGoto.Controls["betterTextboxGotoZ"] as BetterTextbox;
            _buttonGoto = _groupBoxGoto.Controls["buttonGoto"] as Button;
            _buttonGoto.Click += (sender, e) => GotoClick();
            _buttonGotoGetCurrent = _groupBoxGoto.Controls["buttonGotoGetCurrent"] as Button;
            _buttonGotoGetCurrent.Click += (sender, e) => GotoGetCurrentClick();
            _buttonPasteAndGoto = _groupBoxGoto.Controls["buttonPasteAndGoto"] as Button;
            _buttonPasteAndGoto.Click += (sender, e) => PasteAndGotoClick();

            // State Transfer
            _groupBoxStateTransfer = tabControl.Controls["groupBoxStateTransfer"] as GroupBox;
            _buttonStateTransferInstructions = _groupBoxStateTransfer.Controls["buttonStateTransferInstructions"] as Button;
            _buttonStateTransferInstructions.Click += (sender, e) => StateTransferInstructions();
            _buttonStateTransferSave = _groupBoxStateTransfer.Controls["buttonStateTransferSave"] as Button;
            _buttonStateTransferSave.Click += (sender, e) => StateTransferSave();
            _buttonStateTransferApply = _groupBoxStateTransfer.Controls["buttonStateTransferApply"] as Button;
            _buttonStateTransferApply.Click += (sender, e) => StateTransferApply();
            _checkBoxStateTransferOffsetTimers = _groupBoxStateTransfer.Controls["checkBoxStateTransferOffsetTimers"] as CheckBox;

            _betterTextboxStateTransferVar1Current = _groupBoxStateTransfer.Controls["betterTextboxStateTransferVar1Current"] as BetterTextbox;
            _betterTextboxStateTransferVar2Current = _groupBoxStateTransfer.Controls["betterTextboxStateTransferVar2Current"] as BetterTextbox;
            _betterTextboxStateTransferVar3Current = _groupBoxStateTransfer.Controls["betterTextboxStateTransferVar3Current"] as BetterTextbox;
            _betterTextboxStateTransferVar4Current = _groupBoxStateTransfer.Controls["betterTextboxStateTransferVar4Current"] as BetterTextbox;
            _betterTextboxStateTransferVar5Current = _groupBoxStateTransfer.Controls["betterTextboxStateTransferVar5Current"] as BetterTextbox;
            _betterTextboxStateTransferVar6Current = _groupBoxStateTransfer.Controls["betterTextboxStateTransferVar6Current"] as BetterTextbox;
            _betterTextboxStateTransferVar7Current = _groupBoxStateTransfer.Controls["betterTextboxStateTransferVar7Current"] as BetterTextbox;
            _betterTextboxStateTransferVar8Current = _groupBoxStateTransfer.Controls["betterTextboxStateTransferVar8Current"] as BetterTextbox;
            _betterTextboxStateTransferVar9Current = _groupBoxStateTransfer.Controls["betterTextboxStateTransferVar9Current"] as BetterTextbox;
            _betterTextboxStateTransferVar10Current = _groupBoxStateTransfer.Controls["betterTextboxStateTransferVar10Current"] as BetterTextbox;
            _betterTextboxStateTransferVar11Current = _groupBoxStateTransfer.Controls["betterTextboxStateTransferVar11Current"] as BetterTextbox;
            _betterTextboxStateTransferVar12Current = _groupBoxStateTransfer.Controls["betterTextboxStateTransferVar12Current"] as BetterTextbox;
            _betterTextboxStateTransferVar13Current = _groupBoxStateTransfer.Controls["betterTextboxStateTransferVar13Current"] as BetterTextbox;
            _betterTextboxStateTransferVar14Current = _groupBoxStateTransfer.Controls["betterTextboxStateTransferVar14Current"] as BetterTextbox;

            _betterTextboxStateTransferVar1Saved = _groupBoxStateTransfer.Controls["betterTextboxStateTransferVar1Saved"] as BetterTextbox;
            _betterTextboxStateTransferVar2Saved = _groupBoxStateTransfer.Controls["betterTextboxStateTransferVar2Saved"] as BetterTextbox;
            _betterTextboxStateTransferVar3Saved = _groupBoxStateTransfer.Controls["betterTextboxStateTransferVar3Saved"] as BetterTextbox;
            _betterTextboxStateTransferVar4Saved = _groupBoxStateTransfer.Controls["betterTextboxStateTransferVar4Saved"] as BetterTextbox;
            _betterTextboxStateTransferVar5Saved = _groupBoxStateTransfer.Controls["betterTextboxStateTransferVar5Saved"] as BetterTextbox;
            _betterTextboxStateTransferVar6Saved = _groupBoxStateTransfer.Controls["betterTextboxStateTransferVar6Saved"] as BetterTextbox;
            _betterTextboxStateTransferVar7Saved = _groupBoxStateTransfer.Controls["betterTextboxStateTransferVar7Saved"] as BetterTextbox;
            _betterTextboxStateTransferVar8Saved = _groupBoxStateTransfer.Controls["betterTextboxStateTransferVar8Saved"] as BetterTextbox;
            _betterTextboxStateTransferVar9Saved = _groupBoxStateTransfer.Controls["betterTextboxStateTransferVar9Saved"] as BetterTextbox;
            _betterTextboxStateTransferVar10Saved = _groupBoxStateTransfer.Controls["betterTextboxStateTransferVar10Saved"] as BetterTextbox;
            _betterTextboxStateTransferVar11Saved = _groupBoxStateTransfer.Controls["betterTextboxStateTransferVar11Saved"] as BetterTextbox;
            _betterTextboxStateTransferVar12Saved = _groupBoxStateTransfer.Controls["betterTextboxStateTransferVar12Saved"] as BetterTextbox;
            _betterTextboxStateTransferVar13Saved = _groupBoxStateTransfer.Controls["betterTextboxStateTransferVar13Saved"] as BetterTextbox;
            _betterTextboxStateTransferVar14Saved = _groupBoxStateTransfer.Controls["betterTextboxStateTransferVar14Saved"] as BetterTextbox;

            // Obj at HOLP
            _groupBoxObjAtHOLP = tabControl.Controls["groupBoxObjAtHOLP"] as GroupBox;
            _checkBoxObjAtHOLPOn = _groupBoxObjAtHOLP.Controls["checkBoxObjAtHOLPOn"] as CheckBox;
            _betterTextboxObjAtHOLP = _groupBoxObjAtHOLP.Controls["betterTextboxObjAtHOLP"] as BetterTextbox;

            // Obj at Home
            _groupBoxObjAtHome = tabControl.Controls["groupBoxObjAtHome"] as GroupBox;
            _checkBoxObjAtHomeOn = _groupBoxObjAtHome.Controls["checkBoxObjAtHomeOn"] as CheckBox;
            _betterTextboxObjAtHomeObj = _groupBoxObjAtHome.Controls["betterTextboxObjAtHomeObj"] as BetterTextbox;
            _betterTextboxObjAtHomeHome = _groupBoxObjAtHome.Controls["betterTextboxObjAtHomeHome"] as BetterTextbox;

            // Obj at Obj
            _groupBoxObjAtObj = tabControl.Controls["groupBoxObjAtObj"] as GroupBox;
            _checkBoxObjAtObjOn = _groupBoxObjAtObj.Controls["checkBoxObjAtObjOn"] as CheckBox;
            _betterTextboxObjAtObj1 = _groupBoxObjAtObj.Controls["betterTextboxObjAtObj1"] as BetterTextbox;
            _betterTextboxObjAtObj2 = _groupBoxObjAtObj.Controls["betterTextboxObjAtObj2"] as BetterTextbox;

            // Schedule
            _groupBoxSchedule = tabControl.Controls["groupBoxSchedule"] as GroupBox;
            _labelSchedule1 = _groupBoxSchedule.Controls["labelSchedule1"] as Label;
            _labelSchedule2 = _groupBoxSchedule.Controls["labelSchedule2"] as Label;
            _labelSchedule3 = _groupBoxSchedule.Controls["labelSchedule3"] as Label;
            _labelSchedule4 = _groupBoxSchedule.Controls["labelSchedule4"] as Label;
            _labelSchedule5 = _groupBoxSchedule.Controls["labelSchedule5"] as Label;
            _labelSchedule6 = _groupBoxSchedule.Controls["labelSchedule6"] as Label;
            _labelSchedule7 = _groupBoxSchedule.Controls["labelSchedule7"] as Label;
            _labelScheduleIndex = _groupBoxSchedule.Controls["labelScheduleIndex"] as Label;
            _labelScheduleDescription = _groupBoxSchedule.Controls["labelScheduleDescription"] as Label;
            _buttonScheduleButtonSet = _groupBoxSchedule.Controls["buttonScheduleButtonSet"] as Button;
            _buttonScheduleButtonSet.Click += (sender, e) => buttonScheduleButtonSetClick();
            _buttonSchedulePrevious = _groupBoxSchedule.Controls["buttonSchedulePrevious"] as Button;
            _buttonSchedulePrevious.Click += (sender, e) => buttonScheduleButtonPreviousClick();
            _buttonScheduleNext = _groupBoxSchedule.Controls["buttonScheduleNext"] as Button;
            _buttonScheduleNext.Click += (sender, e) => buttonScheduleButtonNextClick();
            _buttonScheduleButtonReset = _groupBoxSchedule.Controls["buttonScheduleButtonReset"] as Button;
            _buttonScheduleButtonReset.Click += (sender, e) => buttonScheduleButtonResetClick();

            // Scuttlebug Stuff
            _groupBoxScuttlebugStuff = tabControl.Controls["groupBoxScuttlebugStuff"] as GroupBox;
            _radioButtonScuttlebugStuffBBHBalconyEye = _groupBoxScuttlebugStuff.Controls["radioButtonScuttlebugStuffBBHBalconyEye"] as RadioButton;
            _radioButtonScuttlebugStuffBBHMerryGoRound = _groupBoxScuttlebugStuff.Controls["radioButtonScuttlebugStuffBBHMerryGoRound"] as RadioButton;
            _radioButtonScuttlebugStuffHMCAmazing = _groupBoxScuttlebugStuff.Controls["radioButtonScuttlebugStuffHMCAmazing"] as RadioButton;
            _radioButtonScuttlebugStuffHMCRedCoins = _groupBoxScuttlebugStuff.Controls["radioButtonScuttlebugStuffHMCRedCoins"] as RadioButton;
            _buttonScuttlebugStuffLungeToHome = _groupBoxScuttlebugStuff.Controls["buttonScuttlebugStuffLungeToHome"] as Button;

            _buttonScuttlebugStuff3rdFloor = _groupBoxScuttlebugStuff.Controls["buttonScuttlebugStuff3rdFloor"] as Button;
            _buttonScuttlebugStuff2ndFloor = _groupBoxScuttlebugStuff.Controls["buttonScuttlebugStuff2ndFloor"] as Button;
            _buttonScuttlebugStuff1stFloor = _groupBoxScuttlebugStuff.Controls["buttonScuttlebugStuff1stFloor"] as Button;
            _buttonScuttlebugStuffBasement = _groupBoxScuttlebugStuff.Controls["buttonScuttlebugStuffBasement"] as Button;
            _buttonScuttlebugStuffGetTris = _groupBoxScuttlebugStuff.Controls["buttonScuttlebugStuffGetTris"] as BinaryButton;

            _radioButtonScuttlebugStuffBBHBalconyEye.Click += (sender, e) => _scuttlebugMission = ScuttlebugMission.BBHBalconyEye;
            _radioButtonScuttlebugStuffBBHMerryGoRound.Click += (sender, e) => _scuttlebugMission = ScuttlebugMission.BBHMerryGoRound;
            _radioButtonScuttlebugStuffHMCAmazing.Click += (sender, e) => _scuttlebugMission = ScuttlebugMission.HMCAmazing;
            _radioButtonScuttlebugStuffHMCRedCoins.Click += (sender, e) => _scuttlebugMission = ScuttlebugMission.HMCRedCoins;
            _buttonScuttlebugStuffLungeToHome.Click += (sender, e) => InvokeScuttlebugsLungeToHome();

            _buttonScuttlebugStuff3rdFloor.Click += (sender, e) => HandleScuttlebugRoomTransition(3); 
            _buttonScuttlebugStuff2ndFloor.Click += (sender, e) => HandleScuttlebugRoomTransition(2);
            _buttonScuttlebugStuff1stFloor.Click += (sender, e) => HandleScuttlebugRoomTransition(1);
            _buttonScuttlebugStuffBasement.Click += (sender, e) => HandleScuttlebugRoomTransition(0);
            _buttonScuttlebugStuffGetTris.Initialize(
                "Get Tris",
                "Clear Tris",
                () => _scuttlebugTriangleList = TriangleUtilities.GetLevelTriangles(),
                () => _scuttlebugTriangleList.Clear(),
                () => _scuttlebugTriangleList.Count != 0);

            // Tri Rooms
            GroupBox groupBoxTriRooms = tabControl.Controls["groupBoxTriRooms"] as GroupBox;
            _textBoxTriRoomsFromValue = groupBoxTriRooms.Controls["textBoxTriRoomsFromValue"] as TextBox;
            _textBoxTriRoomsToValue = groupBoxTriRooms.Controls["textBoxTriRoomsToValue"] as TextBox;
            _buttonTriRoomsConvert = groupBoxTriRooms.Controls["buttonTriRoomsConvert"] as Button;
            _buttonTriRoomsConvert.Click += (sender, e) =>
            {
                bool fromEmpty = _textBoxTriRoomsFromValue.Text == "";
                List<string> fromListStrings = ParsingUtilities.ParseStringList(_textBoxTriRoomsFromValue.Text);
                List<byte> fromListBytes = new List<byte>();
                fromListStrings.ForEach(fromString =>
                {
                    byte? fromByteNullable = ParsingUtilities.ParseByteNullable(fromString);
                    if (fromByteNullable.HasValue) fromListBytes.Add(fromByteNullable.Value);
                });

                byte? toByteNullable = ParsingUtilities.ParseByteNullable(_textBoxTriRoomsToValue.Text);
                if (!toByteNullable.HasValue) return;
                byte toByte = toByteNullable.Value;

                List<TriangleDataModel> tris = TriangleUtilities.GetLevelTriangles();
                tris.ForEach(tri =>
                {
                    if (fromEmpty || fromListBytes.Contains(tri.Room))
                    {
                        Config.Stream.SetValue(toByte, tri.Address + TriangleOffsetsConfig.Room);
                    }
                });
            };

            // Memory Reader
            _groupBoxMemoryReader = tabControl.Controls["groupBoxMemoryReader"] as GroupBox;
            _comboBoxMemoryReaderTypeValue = _groupBoxMemoryReader.Controls["comboBoxMemoryReaderTypeValue"] as ComboBox;
            _textBoxMemoryReaderAddressValue = _groupBoxMemoryReader.Controls["textBoxMemoryReaderAddressValue"] as BetterTextbox;
            _textBoxMemoryReaderCountValue = _groupBoxMemoryReader.Controls["textBoxMemoryReaderCountValue"] as BetterTextbox;
            _buttonMemoryReaderRead = _groupBoxMemoryReader.Controls["buttonMemoryReaderRead"] as Button;

            _comboBoxMemoryReaderTypeValue.DataSource = TypeUtilities.StringToType.Keys.ToList();

            _buttonMemoryReaderRead.Click += (sender, e) =>
            {
                bool showHex = KeyboardUtilities.IsCtrlHeld();
                uint address = ParsingUtilities.ParseHex(_textBoxMemoryReaderAddressValue.Text);
                int count = ParsingUtilities.ParseInt(_textBoxMemoryReaderCountValue.Text);
                string typeString = _comboBoxMemoryReaderTypeValue.SelectedValue as string;
                Type type = TypeUtilities.StringToType[typeString];
                int typeSize = TypeUtilities.TypeSize[type];
                Type unsignedByteType = TypeUtilities.UnsignedByteType[typeSize];
                List <object> values = new List<object>();
                for (int i = 0; i < count; i++)
                {
                    uint addr = (uint)(address + i * typeSize);
                    object value = Config.Stream.GetValue(type, addr);
                    if (showHex)
                    {
                        object hexNumber = Config.Stream.GetValue(unsignedByteType, addr);
                        string hexString = HexUtilities.FormatValue(hexNumber, 2 * typeSize);
                        value = hexString + "\t" + value;
                    }
                    values.Add(value);
                }
                InfoForm.ShowValue(
                    String.Join("\r\n", values),
                    "Memory Reader",
                    count + " " + typeString + " value(s) at 0x" + String.Format("{0:X}", address));
            };

            // TTC Simulator
            GroupBox groupBoxTestingTtcSimulator = tabControl.Controls["groupBoxTestingTtcSimulator"] as GroupBox;
            _textBoxTestingTtcSimulatorEndFrame = groupBoxTestingTtcSimulator.Controls["textBoxTestingTtcSimulatorEndFrame"] as BetterTextbox;
            _textBoxTestingTtcSimulatorDustFrames = groupBoxTestingTtcSimulator.Controls["textBoxTestingTtcSimulatorDustFrames"] as BetterTextbox;
            _buttonTestingTtcSimulatorCalculate = groupBoxTestingTtcSimulator.Controls["buttonTestingTtcSimulatorCalculate"] as Button;
            _buttonTestingTtcSimulatorCalculate.Click += (sender, e) =>
            {
                int? endFrameNullable = ParsingUtilities.ParseIntNullable(_textBoxTestingTtcSimulatorEndFrame.Text);
                List<int?> dustFramesNullable = ParsingUtilities.ParseIntList(_textBoxTestingTtcSimulatorDustFrames.Text);
                if (!endFrameNullable.HasValue || dustFramesNullable.Any(dustFrame => !dustFrame.HasValue)) return;
                int endFrame = endFrameNullable.Value;
                List<int> dustFrames = dustFramesNullable.ConvertAll(dustFrameNullable => dustFrameNullable.Value);
                InfoForm.ShowValue(TtcMain.Simulate(endFrame, dustFrames));
            };

            // Pendulum Manipulation
            GroupBox groupBoxTestingPendulumManipulation = tabControl.Controls["groupBoxTestingPendulumManipulation"] as GroupBox;
            BetterTextbox textBoxTestingPendulumManipulationPendulum = groupBoxTestingPendulumManipulation.Controls["textBoxTestingPendulumManipulationPendulum"] as BetterTextbox;
            BetterTextbox textBoxTestingPendulumManipulationIterations = groupBoxTestingPendulumManipulation.Controls["textBoxTestingPendulumManipulationIterations"] as BetterTextbox;
            Button buttonTestingPendulumManipulationCalculate = groupBoxTestingPendulumManipulation.Controls["buttonTestingPendulumManipulationCalculate"] as Button;
            buttonTestingPendulumManipulationCalculate.Click += (sender, e) =>
            {
                uint pendulumAddress = ParsingUtilities.ParseHexNullable(textBoxTestingPendulumManipulationPendulum.Text) ?? 0;
                int numIterations = ParsingUtilities.ParseInt(textBoxTestingPendulumManipulationIterations.Text);
                TtcMain.PrintIdealPendulumManipulation(pendulumAddress, numIterations, true);
            };

            // TTC Logger
            _groupBoxTtcLogger = tabControl.Controls["groupBoxTtcLogger"] as GroupBox;
            _checkBoxTtcLoggerLogStates = _groupBoxTtcLogger.Controls["checkBoxTtcLoggerLogStates"] as CheckBox;
            _labelTtcLoggerStatus = _groupBoxTtcLogger.Controls["labelTtcLoggerStatus"] as Label;
            _textBoxTtcLoggerState = _groupBoxTtcLogger.Controls["textBoxTtcLoggerState"] as BetterTextbox;
            _textBoxTtcLoggerLogs = _groupBoxTtcLogger.Controls["textBoxTtcLoggerLogs"] as BetterTextbox;
            _buttonTtcLoggerClear = _groupBoxTtcLogger.Controls["buttonTtcLoggerClear"] as Button;
            _lastTtcSaveState = null;
            _ttcSaveStates = new HashSet<string>();
            _buttonTtcLoggerClear.Click += (sender, e) =>
            {
                _lastTtcSaveState = null;
                _ttcSaveStates = new HashSet<string>();
            };
        }

        private List<uint> GetScuttlebugAddresses()
        {
            switch (_scuttlebugMission)
            {
                case ScuttlebugMission.BBHBalconyEye:
                    return new List<uint>() { 0x803441C8, 0x80344428, 0x80344B48 };
                case ScuttlebugMission.BBHMerryGoRound:
                    return new List<uint>() { 0x803441C8 };
                case ScuttlebugMission.HMCAmazing:
                    return new List<uint>() { 0x803408C8 };
                case ScuttlebugMission.HMCRedCoins:
                    return new List<uint>() { 0x803422E8 };
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void InvokeScuttlebugsLungeToHome()
        {
            List<uint> scuttlebugAddresses = GetScuttlebugAddresses();
            foreach (uint objAddress in scuttlebugAddresses)
            {
                float objX = Config.Stream.GetSingle(objAddress + ObjectConfig.XOffset);
                float objZ = Config.Stream.GetSingle(objAddress + ObjectConfig.ZOffset);
                float homeX = Config.Stream.GetSingle(objAddress + ObjectConfig.HomeXOffset);
                float homeZ = Config.Stream.GetSingle(objAddress + ObjectConfig.HomeZOffset);
                ushort angleToHome = MoreMath.AngleTo_AngleUnitsRounded(objX, objZ, homeX, homeZ);
                Config.Stream.SetValue(angleToHome, objAddress + ObjectConfig.YawFacingOffset);
                Config.Stream.SetValue(angleToHome, objAddress + ObjectConfig.YawMovingOffset);
                Config.Stream.SetValue(20f, objAddress + ObjectConfig.YSpeedOffset);
                Config.Stream.SetValue(1, objAddress + ObjectConfig.ScuttlebugPhaseOffset);
            }
        }

        private static readonly byte Dummy_Room = 127;
        private static readonly byte Outside_Room = 13;
        private static readonly byte Balcony_3rdFloor_Room = 2;
        private static readonly byte MerryGoRound_3rdFloor_Room = 6;
        private static readonly byte MerryGoRound_1stFloor_Room = 5;
        private static readonly byte MerryGoRound_Basement_Room = 10;

        private void HandleScuttlebugRoomTransition(int transition)
        {
            switch (_scuttlebugMission)
            {
                case ScuttlebugMission.BBHBalconyEye:
                    switch (transition)
                    {
                        case 3:
                            HandleScuttlebugRoomTransition(Balcony_3rdFloor_Room);
                            break;
                    }
                    break;
                case ScuttlebugMission.BBHMerryGoRound:
                    switch (transition)
                    {
                        case 3:
                            HandleScuttlebugRoomTransition(MerryGoRound_3rdFloor_Room);
                            break;
                        case 2:
                            // do nothing
                            break;
                        case 1:
                            HandleScuttlebugRoomTransition(MerryGoRound_1stFloor_Room);
                            break;
                        case 0:
                            HandleScuttlebugRoomTransition(MerryGoRound_Basement_Room);
                            break;
                    }
                    break;
            }
        }

        private void HandleScuttlebugRoomTransition(byte newRoom)
        {
            // Convert new room triangles to dummy room value
            foreach (TriangleDataModel triStruct in _scuttlebugTriangleList)
            {
                if (triStruct.Room == newRoom)
                {
                    Config.Stream.SetValue(Dummy_Room, triStruct.Address + TriangleOffsetsConfig.Room);
                }
            }
            
            // Convert all outside triangles to the new room value
            foreach (TriangleDataModel triStruct in _scuttlebugTriangleList)
            {
                if (triStruct.Room == Outside_Room)
                {
                    Config.Stream.SetValue(newRoom, triStruct.Address + TriangleOffsetsConfig.Room);
                }
            }

            // Convert scuttlebugs to have native room as new room value
            List<uint> scuttlebugAddresses = GetScuttlebugAddresses();
            foreach (uint objAddress in scuttlebugAddresses)
            {
                Config.Stream.SetValue((int)newRoom, objAddress + ObjectConfig.NativeRoomOffset);
            }
        }

        public abstract class VarState
        {
            public abstract List<Object> VarValues();
        }

        public class VarStateMario : VarState
        {
            public float X;
            public float Y;
            public float Z;
            public ushort Angle;
            public float Vspd;
            public float Hspd;

            public static VarState GetCurrent()
            {
                return new VarStateMario()
                {
                    X = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset),
                    Y = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset),
                    Z = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset),
                    Angle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset),
                    Vspd = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.VSpeedOffset),
                    Hspd = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HSpeedOffset),
                };
            }

            public static List<string> VarNames()
            {
                return new List<string>()
                {
                    "X", "Y", "Z", "Angle", "Vspd", "Hspd"
                };
            }

            public override List<Object> VarValues()
            {
                return new List<Object>()
                {
                    X, Y, Z, Angle, Vspd, Hspd
                };
            }

            public static string VarNamesString()
            {
                return String.Join("\t", VarNames());
            }

            public override string ToString()
            {
                return String.Join("\t", VarValues());
            }

            public override bool Equals(object obj)
            {
                if (!(obj is VarStateMario)) return false;
                VarStateMario other = obj as VarStateMario;
                return Enumerable.SequenceEqual(this.VarValues(), other.VarValues());
            }

            public override int GetHashCode()
            {
                return VarValues().GetHashCode();
            }
        }

        public class VarStatePenguin : VarState
        {
            public double Progress;

            public static VarStatePenguin GetCurrent()
            {
                return new VarStatePenguin()
                {
                    Progress = TableConfig.RacingPenguinWaypoints.GetProgress(RomVersionConfig.Switch(0x80348448, 0x803451F8)),
                };
            }

            public static List<string> VarNames()
            {
                return new List<string>()
                {
                    "Progress"
                };
            }

            public override List<Object> VarValues()
            {
                return new List<Object>()
                {
                    Progress
                };
            }

            public static string VarNamesString()
            {
                return String.Join("\t", VarNames());
            }

            public override string ToString()
            {
                return String.Join("\t", VarValues());
            }

            public override bool Equals(object obj)
            {
                if (!(obj is VarStatePenguin)) return false;
                VarStatePenguin other = obj as VarStatePenguin;
                return Enumerable.SequenceEqual(this.VarValues(), other.VarValues());
            }

            public override int GetHashCode()
            {
                return VarValues().GetHashCode();
            }
        }

        private void SetRecordOn(bool recordOn)
        {
            if (recordOn) RefreshRateConfig.RefreshRateFreq = 0;
            else RefreshRateConfig.RefreshRateFreq = 30;
        }

        public void Update(bool updateView)
        {
            /*
            if (updateView)
            {
                int koopaTurnAngle = 1536;
                uint koopaAddress = 0x8034E0E8;
                ushort koopaAngle = Config.Stream.GetUInt16(koopaAddress + ObjectConfig.YawFacingOffset);
                uint cameraHackAngleAddress = 0x803E001E;
                ushort cameraHackAngle = Config.Stream.GetUInt16(cameraHackAngleAddress);
                if (MoreMath.GetAngleDistance(koopaAngle, cameraHackAngle) > 2 * koopaTurnAngle)
                {
                    ushort newCameraHackAngle = (ushort)MoreMath.RotateAngleTowards(cameraHackAngle, koopaAngle, koopaTurnAngle);
                    Config.Stream.SetValue(newCameraHackAngle, cameraHackAngleAddress);
                }
            }
            */

            // Show Invisible Objects as Signs
            if (TestingConfig.ShowInvisibleObjectsAsSigns)
            {
                DataModels.Objects.ToList().ForEach(obj =>
                {
                    if (obj == null) return;
                    if (obj.GraphicsID == 0) obj.GraphicsID = ObjectConfig.SignGraphicsId;
                });
            }

            // Obj at HOLP
            if (_checkBoxObjAtHOLPOn.Checked)
            {
                uint? objAddress = ParsingUtilities.ParseHexNullable(_betterTextboxObjAtHOLP.Text);
                if (objAddress.HasValue)
                {
                    float holpX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HolpXOffset);
                    float holpY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HolpYOffset);
                    float holpZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HolpZOffset);

                    Config.Stream.SetValue(holpX, objAddress.Value + ObjectConfig.XOffset);
                    Config.Stream.SetValue(holpY, objAddress.Value + ObjectConfig.YOffset);
                    Config.Stream.SetValue(holpZ, objAddress.Value + ObjectConfig.ZOffset);
                }
            }

            // Obj at Home
            if (_checkBoxObjAtHomeOn.Checked)
            {
                uint? objAddress = ParsingUtilities.ParseHexNullable(_betterTextboxObjAtHomeObj.Text);
                uint? homeObjAddress = ParsingUtilities.ParseHexNullable(_betterTextboxObjAtHomeHome.Text);
                if (objAddress.HasValue && homeObjAddress.HasValue)
                {
                    float homeX = Config.Stream.GetSingle(homeObjAddress.Value + ObjectConfig.HomeXOffset);
                    float homeY = Config.Stream.GetSingle(homeObjAddress.Value + ObjectConfig.HomeYOffset);
                    float homeZ = Config.Stream.GetSingle(homeObjAddress.Value + ObjectConfig.HomeZOffset);

                    Config.Stream.SetValue(homeX, objAddress.Value + ObjectConfig.XOffset);
                    Config.Stream.SetValue(homeY, objAddress.Value + ObjectConfig.YOffset);
                    Config.Stream.SetValue(homeZ, objAddress.Value + ObjectConfig.ZOffset);
                }
            }

            // Obj at Obj
            if (_checkBoxObjAtObjOn.Checked)
            {
                uint? obj1Address = ParsingUtilities.ParseHexNullable(_betterTextboxObjAtObj1.Text);
                uint? obj2Address = ParsingUtilities.ParseHexNullable(_betterTextboxObjAtObj2.Text);
                if (obj1Address.HasValue && obj2Address.HasValue)
                {
                    float posX = Config.Stream.GetSingle(obj2Address.Value + ObjectConfig.XOffset);
                    float posY = Config.Stream.GetSingle(obj2Address.Value + ObjectConfig.YOffset);
                    float posZ = Config.Stream.GetSingle(obj2Address.Value + ObjectConfig.ZOffset);

                    Config.Stream.SetValue(posX, obj1Address.Value + ObjectConfig.XOffset);
                    Config.Stream.SetValue(posY, obj1Address.Value + ObjectConfig.YOffset);
                    Config.Stream.SetValue(posZ, obj1Address.Value + ObjectConfig.ZOffset);
                }
            }

            if (_checkBoxTtcLoggerLogStates.Checked)
            {
                string saveStateString = new TtcSaveState().ToString();
                if (saveStateString != _lastTtcSaveState)
                {
                    _lastTtcSaveState = saveStateString;
                    _textBoxTtcLoggerState.Text = saveStateString;
                    bool newStatus = !_ttcSaveStates.Contains(saveStateString);
                    _ttcSaveStates.Add(saveStateString);
                    _labelTtcLoggerStatus.Text = newStatus ? "NEW" : "OLD";
                    _textBoxTtcLoggerLogs.Text = _ttcSaveStates.Count.ToString();
                }
            }

            if (!updateView) return;

            // Schedule
            {
                (int frame, double? x, double? y, double? z, double? hspd, string description) = _rollingRocksScheduleList[_rollingRocksScheduleIndex];
                _labelSchedule1.Text = Config.Stream.GetInt32(MiscConfig.GlobalTimerAddress).ToString();
                _labelSchedule2.Text = (frame + _rollingRocksScheduleIndexOffset).ToString();
                if (x.HasValue) _labelSchedule3.Text = x.Value.ToString();
                if (y.HasValue) _labelSchedule4.Text = y.Value.ToString();
                if (z.HasValue) _labelSchedule5.Text = z.Value.ToString();
                _labelSchedule6.Text = (0).ToString();
                if (hspd.HasValue) _labelSchedule7.Text = hspd.Value.ToString();
                _labelScheduleIndex.Text = _rollingRocksScheduleIndex.ToString();
                _labelScheduleDescription.Text = description.ToString();
            }

            // Control stick
            sbyte currentX = Config.Stream.GetSByte(InputConfig.CurrentInputAddress + InputConfig.ControlStickXOffset);
            sbyte currentY = Config.Stream.GetSByte(InputConfig.CurrentInputAddress + InputConfig.ControlStickYOffset);

            if (_checkBoxUseInput.Checked)
            {
                _betterTextboxControlStick1.Text = currentX.ToString();
                _betterTextboxControlStick2.Text = (-1 * currentY).ToString();
            }

            int rawX = ParsingUtilities.ParseInt(_betterTextboxControlStick1.Text);
            int rawY = ParsingUtilities.ParseInt(_betterTextboxControlStick2.Text);
            (float effectiveX, float effectiveY) = MoreMath.GetEffectiveInput(rawX, -1 * rawY);
            _labelControlStick1.Text = effectiveX.ToString();
            _labelControlStick2.Text = effectiveY.ToString();
            ushort marioFacingYaw = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
            _labelControlStick3.Text = marioFacingYaw.ToString();

            ushort angle = InGameTrigUtilities.InGameATan(effectiveY, -effectiveX);
            ushort cameraAngle = Config.Stream.GetUInt16(CameraConfig.StructAddress + 0xFC);
            cameraAngle = MoreMath.NormalizeAngleUshort(MoreMath.ReverseAngle(cameraAngle));
            //cameraAngle = MoreMath.NormalizeAngleTruncated(cameraAngle);
            ushort summedAngle = MoreMath.NormalizeAngleUshort(angle + cameraAngle);
            _labelControlStick4.Text = summedAngle.ToString();
            _labelControlStick5.Text = InGameTrigUtilities.InGameATan(rawX, rawY).ToString();
            _labelControlStick6.Text = MoreMath.CalculateAngleFromInputs(currentX, currentY).ToString();

            /*
            int angleGuess = MoreMath.NormalizeAngleUshort(angle);
            _labelControlStick4.Text = angleGuess.ToString();
            int angleInteded = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.IntendedYawOffset);
            _labelControlStick5.Text = angleInteded.ToString();
            int diff = angleGuess - angleInteded;
            _labelControlStick6.Text = diff.ToString();
            */

            // State Transfer
            StateTransferUpdate();

            // Scuttlebug stuff
            _buttonScuttlebugStuffGetTris.UpdateButton();
        }

        private void buttonScheduleButtonPreviousClick()
        {
            _rollingRocksScheduleIndex--;
        }

        private void buttonScheduleButtonNextClick()
        {
            _rollingRocksScheduleIndex++;
        }

        private void buttonScheduleButtonSetClick()
        {
            // Schedule
            {
                (int frame, double? x, double? y, double? z, double? hspd, string description) = _rollingRocksScheduleList[_rollingRocksScheduleIndex];
                if (x.HasValue) Config.Stream.SetValue((float)x.Value, MarioConfig.StructAddress + MarioConfig.XOffset);
                if (y.HasValue) Config.Stream.SetValue((float)y.Value, MarioConfig.StructAddress + MarioConfig.YOffset);
                if (z.HasValue) Config.Stream.SetValue((float)z.Value, MarioConfig.StructAddress + MarioConfig.ZOffset);
                if (hspd.HasValue) Config.Stream.SetValue((float)hspd.Value, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);

                if (frame == 8288)
                {
                    Config.Stream.SetValue((uint)0x04000471, MarioConfig.StructAddress + MarioConfig.ActionOffset);
                }

                if (frame == 8819 || frame == 9926 || frame == 10060 || frame == 10463 || frame == 10475)
                {
                    Config.Stream.SetValue((uint)16779404, MarioConfig.StructAddress + MarioConfig.ActionOffset);
                }

                if (frame == 10476)
                {
                    Config.Stream.SetValue((float)0, MarioConfig.StructAddress + MarioConfig.VSpeedOffset);
                }

                if (frame == 10060)
                {
                    ButtonUtilities.UnloadObject(new List<ObjectDataModel> { new ObjectDataModel(0x8034DC28) });
                }

                if (frame == 10475)
                {
                    Config.Stream.SetValue((ushort)32832, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                }

                _rollingRocksScheduleIndex++;
            }
        }

        private void buttonScheduleButtonResetClick()
        {
            _rollingRocksScheduleIndex = 47;
        }

        private void GotoClick()
        {
            double? gotoX = ParsingUtilities.ParseDoubleNullable(_betterTextboxGotoX.Text);
            double? gotoY = ParsingUtilities.ParseDoubleNullable(_betterTextboxGotoY.Text);
            double? gotoZ = ParsingUtilities.ParseDoubleNullable(_betterTextboxGotoZ.Text);
            if (gotoX.HasValue && gotoY.HasValue && gotoZ.HasValue)
            {
                ButtonUtilities.SetMarioPosition(
                    (float)gotoX.Value, (float)gotoY.Value, (float)gotoZ.Value);
            }
        }

        private void GotoGetCurrentClick()
        {
            float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
            _betterTextboxGotoX.Text = marioX.ToString();
            _betterTextboxGotoY.Text = marioY.ToString();
            _betterTextboxGotoZ.Text = marioZ.ToString();
        }

        private void PasteAndGotoClick()
        {
            string clipboardText = Clipboard.GetText();
            List<string> parsedStrings = ParsingUtilities.ParseStringList(clipboardText);
            List<TextBox> textboxes = new List<TextBox>() { _betterTextboxGotoX, _betterTextboxGotoY, _betterTextboxGotoZ };
            for (int i = 0; i < parsedStrings.Count && i < textboxes.Count; i++)
            {
                textboxes[i].Text = parsedStrings[i];
            }
            GotoClick();
        }

        private void StateTransferInstructions()
        {
            List<string> instructionList = new List<string>()
            {
                "This is a tool for having one m64 file start with the same state",
                "(i.e. RNG, global timer, HOLP, etc) as another m64 file.",
                "This assumes the m64 starts from the mission select screen.",
                "To use it, just follow these instructions:",
                "(1) Pause the emulator.",
                "(2) Open the m64 that you would like to copy state from.",
                "(3) Advance 1 frame.",
                "(4) Press the Save button in the State Transfer box in STROOP.",
                "(5) Go to the mission select screen where you would like to paste the state to (possibly on a different ROM).",
                "(6) Make sure you're selecting the correct mission.",
                "(7) Pause the emulator.",
                "(8) Press the Apply button in the State Transfer box in STROOP.",
                "(9) Start a new m64 from snapshot.",
            };
            string instructions = String.Join("\r\n", instructionList);
            InfoForm.ShowValue(instructions, "State Transfer", "Instructions");
        }

        private void StateTransferUpdate()
        {
            _betterTextboxStateTransferVar1Current.Text = Config.Stream.GetInt32(MiscConfig.GlobalTimerAddress).ToString();
            _betterTextboxStateTransferVar2Current.Text = Config.Stream.GetUInt16(MiscConfig.RngAddress).ToString();
            _betterTextboxStateTransferVar3Current.Text = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HolpXOffset).ToString();
            _betterTextboxStateTransferVar4Current.Text = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HolpYOffset).ToString();
            _betterTextboxStateTransferVar5Current.Text = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HolpZOffset).ToString();
            _betterTextboxStateTransferVar6Current.Text = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.SlidingYawOffset).ToString();
            _betterTextboxStateTransferVar7Current.Text = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.TwirlYawOffset).ToString();
            _betterTextboxStateTransferVar8Current.Text =
                ((Config.Stream.GetByte(
                    CameraConfig.StructAddress + CameraConfig.MarioCamPossibleOffset) & CameraConfig.MarioCamPossibleMask) != 0).ToString();
            _betterTextboxStateTransferVar9Current.Text = Config.FileManager.GetChecksum(Config.FileManager.GetInGameFileAddress()).ToString();
            _betterTextboxStateTransferVar10Current.Text = Config.Stream.GetInt16(MarioConfig.StructAddress + HudConfig.HpCountOffset).ToString();
            _betterTextboxStateTransferVar11Current.Text = Config.Stream.GetSByte(MarioConfig.StructAddress + HudConfig.LifeCountOffset).ToString();
            _betterTextboxStateTransferVar12Current.Text = Config.Stream.GetInt16(MarioConfig.StructAddress + HudConfig.StarCountOffset).ToString();
            _betterTextboxStateTransferVar13Current.Text = Config.Stream.GetByte(MiscConfig.SpecialTripleJumpAddress).ToString();
            _betterTextboxStateTransferVar14Current.Text = Config.Stream.GetInt16(MiscConfig.AnimationTimerAddress).ToString();
        }

        private void StateTransferSave()
        {
            _betterTextboxStateTransferVar1Saved.Text = _betterTextboxStateTransferVar1Current.Text;
            _betterTextboxStateTransferVar2Saved.Text = _betterTextboxStateTransferVar2Current.Text;
            _betterTextboxStateTransferVar3Saved.Text = _betterTextboxStateTransferVar3Current.Text;
            _betterTextboxStateTransferVar4Saved.Text = _betterTextboxStateTransferVar4Current.Text;
            _betterTextboxStateTransferVar5Saved.Text = _betterTextboxStateTransferVar5Current.Text;
            _betterTextboxStateTransferVar6Saved.Text = _betterTextboxStateTransferVar6Current.Text;
            _betterTextboxStateTransferVar7Saved.Text = _betterTextboxStateTransferVar7Current.Text;
            _betterTextboxStateTransferVar8Saved.Text = _betterTextboxStateTransferVar8Current.Text;
            _betterTextboxStateTransferVar9Saved.Text = _betterTextboxStateTransferVar9Current.Text;
            _betterTextboxStateTransferVar10Saved.Text = _betterTextboxStateTransferVar10Current.Text;
            _betterTextboxStateTransferVar11Saved.Text = _betterTextboxStateTransferVar11Current.Text;
            _betterTextboxStateTransferVar12Saved.Text = _betterTextboxStateTransferVar12Current.Text;
            _betterTextboxStateTransferVar13Saved.Text = _betterTextboxStateTransferVar13Current.Text;
            _betterTextboxStateTransferVar14Saved.Text = _betterTextboxStateTransferVar14Current.Text;
            _stateTransferFileData = Config.FileManager.GetBufferedBytes();
        }

        private void StateTransferApply()
        {
            int timersOffset = _checkBoxStateTransferOffsetTimers.Checked ? -1 : 0;
            int? value1 = ParsingUtilities.ParseIntNullable(_betterTextboxStateTransferVar1Saved.Text);
            if (value1.HasValue) Config.Stream.SetValue(value1.Value + timersOffset, MiscConfig.GlobalTimerAddress);

            ushort? value2 = ParsingUtilities.ParseUShortNullable(_betterTextboxStateTransferVar2Saved.Text);
            if (value2.HasValue) Config.Stream.SetValue(value2.Value, MiscConfig.RngAddress);

            float? value3 = ParsingUtilities.ParseFloatNullable(_betterTextboxStateTransferVar3Saved.Text);
            if (value3.HasValue) Config.Stream.SetValue(value3.Value, MarioConfig.StructAddress + MarioConfig.HolpXOffset);

            float? value4 = ParsingUtilities.ParseFloatNullable(_betterTextboxStateTransferVar4Saved.Text);
            if (value4.HasValue) Config.Stream.SetValue(value4.Value, MarioConfig.StructAddress + MarioConfig.HolpYOffset);

            float? value5 = ParsingUtilities.ParseFloatNullable(_betterTextboxStateTransferVar5Saved.Text);
            if (value5.HasValue) Config.Stream.SetValue(value5.Value, MarioConfig.StructAddress + MarioConfig.HolpZOffset);

            ushort? value6 = ParsingUtilities.ParseUShortNullable(_betterTextboxStateTransferVar6Saved.Text);
            if (value6.HasValue) Config.Stream.SetValue(value6.Value, MarioConfig.StructAddress + MarioConfig.SlidingYawOffset);

            ushort? value7 = ParsingUtilities.ParseUShortNullable(_betterTextboxStateTransferVar7Saved.Text);
            if (value7.HasValue) Config.Stream.SetValue(value7.Value, MarioConfig.StructAddress + MarioConfig.TwirlYawOffset);

            bool? value8 = ParsingUtilities.ParseBoolNullable(_betterTextboxStateTransferVar8Saved.Text);
            if (value8.HasValue)
            {
                byte oldByte = Config.Stream.GetByte(CameraConfig.StructAddress + CameraConfig.MarioCamPossibleOffset);
                byte newByte = MoreMath.ApplyValueToMaskedByte(oldByte, CameraConfig.MarioCamPossibleMask, value8.Value);
                Config.Stream.SetValue(newByte, CameraConfig.StructAddress + CameraConfig.MarioCamPossibleOffset);
            }

            if (_stateTransferFileData != null) Config.FileManager.SetBufferedBytes(_stateTransferFileData, Config.FileManager.GetInGameFileAddress());

            short? value10 = ParsingUtilities.ParseShortNullable(_betterTextboxStateTransferVar10Saved.Text);
            if (value10.HasValue)
            {
                Config.Stream.SetValue(value10.Value, MarioConfig.StructAddress + HudConfig.HpCountOffset);
                Config.Stream.SetValue((short)(value10.Value / 256), MarioConfig.StructAddress + HudConfig.HpDisplayOffset);
            }

            sbyte? value11 = ParsingUtilities.ParseSByteNullable(_betterTextboxStateTransferVar11Saved.Text);
            if (value11.HasValue)
            {
                Config.Stream.SetValue(value11.Value, MarioConfig.StructAddress + HudConfig.LifeCountOffset);
                Config.Stream.SetValue((short)value11.Value, MarioConfig.StructAddress + HudConfig.LifeDisplayOffset);
            }

            short? value12 = ParsingUtilities.ParseShortNullable(_betterTextboxStateTransferVar12Saved.Text);
            if (value12.HasValue)
            {
                Config.Stream.SetValue(value12.Value, MarioConfig.StructAddress + HudConfig.StarCountOffset);
                Config.Stream.SetValue(value12.Value, MarioConfig.StructAddress + HudConfig.StarDisplayOffset);
            }

            byte? value13 = ParsingUtilities.ParseByteNullable(_betterTextboxStateTransferVar13Saved.Text);
            if (value13.HasValue) Config.Stream.SetValue(value13.Value, MiscConfig.SpecialTripleJumpAddress);

            short? value14 = ParsingUtilities.ParseShortNullable(_betterTextboxStateTransferVar14Saved.Text);
            if (value14.HasValue) Config.Stream.SetValue((short)(value14.Value + timersOffset), MiscConfig.AnimationTimerAddress);
        }

        private static int _rollingRocksScheduleIndexOffset = -8582;
        private static int _rollingRocksScheduleIndex = 47;
        private static List<(int, double?, double?, double?, double?, string)> _rollingRocksScheduleList = new List<(int, double?, double?, double?, double?, string)>() {
            (6431,-3075.048,-4929.741,-1614.7,null,"T0 up to T1"),
            (6508,-3444.3,-5082.954,-1614.7,null,"T1 left to T2"),
            (6646,-3444.3,-4874.296,-2182.7,null,"up on T2"),
            (6668,-3990,-4977.589,-2182.7,null,"T2 left to T3"),
            (6789,-4469,-4837.459,-2182.7,null,"left on T3"),
            (6918,-4469,-4732.842,-2461.3,null,"T3 up to T4"),
            (6919,-4469,-4604.239,-2801,null,"T4 up to T5"),
            (7045,-4304,-4474.807,-2967,null,"up right on T5"),
            (7053,-4464,-4378.722,-3127,null,"up left on T5"),
            (7054,-4624,-4283.233,-3287,null,"up left on T5"),
            (7055,-4783,-4189.12,-3446,null,"T5 up left to T6"),
            (7184,-4595,-4017.744,-3635,null,"up right on T6"),
            (7185,-4407,-3843.24,-3823,null,"T6 up right to T7"),
            (7310,-4193,-3685.867,-3823,null,"right on T7"),
            (7431,-4193,-3573.963,-3950,null,"up on T7"),
            (7432,-4193,-3473.41,-4077,null,"T7 up to T8"),
            (7552,-4193,-3221.987,-4804,null,"T8 up to T9"),
            (7687,-4193,-3221.812,-4796,null,"up on T9"),
            (7688,-4193,-3221.658,-4789,null,"up on T9"),
            (7689,-4193,-3221.527,-4783,null,"up on T9"),
            (7690,-4193,-3221.417,-4778,null,"up on T9"),
            (7691,-4193,-3221.329,-4774,null,"up on T9"),
            (7692,-4193,-3221.263,-4771,null,"up on T9"),
            (7693,-4193,-3221.219,-4769,null,"up on T9"),
            (7694,-4193,-3221.197,-4768,null,"up on T9"),
            (7695,-4193,-3221.197,-4768,null,"up on T9"),
            (7696,-4193,-3221.219,-4769.3,null,"up on T9"),
            (7697,-4193,-3221.263,-4771,null,"up on T9"),
            (7698,-4193,-3221.307,-4773.8,null,"up on T9"),
            (7699,-4193,-3221.395,-4777.5,null,"up on T9"),
            (7700,-4193,-3221.527,-4782.2,null,"up on T9"),
            (7701,-4193,-3221.636,-4787.8,null,"up on T9"),
            (7702,-4193,-3221.79,-4794.3,null,"up on T9"),
            (7703,-4193,-3221.944,-4801.8,null,"up on T9"),
            (7832,-3738,-2956.358,-5604.6,null,"T9 up right to T10"),
            (7960,-3738,-2855.501,-5963.5,null,"up on T10"),
            (7961,-3336,-2610,-6321,null,"up rightish on T10"),
            (8091,-3267,-2585.499,-6321,null,"right on T10"),
            (8125,-3267,-2575.666,-6355.5,null,"up on T10"),
            (8154,-3262,-2573.504,-6355.5,null,"right on T10"),
            (8155,-3256.8,-2571.703,-6355.5,null,"right on T10"),
            (8156,-3253,-2570.622,-6355.5,null,"right on T10"),
            (8157,-3250.3,-2569.541,-6355.5,null,"right on T10"),
            (8158,-3248.516,-2568.82,-6355.5,null,"right on T10"),

            //(8287,-3343.166,-2568.82,-2522.1,-271351.3,"T10 down to air"),
            //(8287,-3556.288,-2400,-7124.406,-271349,"FAKE air to E1"),

            (8288,-3556.288,-2559,-7124.406,null,"air to E1"),
            (8466,-3556.288,-2509,-7124.406,null,"first AB kick observed on E1"),
            (8678,-3556.288,-419,-7124.406,null,"last AB kick observed on E1"),



            (8682,3738.2,-409,-7124.406,null,"E1 right to end of hallway"),
            (8819,2508.7,-409,-5997.712,null,"end of hallway down left to maze"),
            (8820,1279.6,-409,-4870.587,-100000,"maze down left to ground"),
            (8821,-1178.719,-409,-2616.338,null,"down left on ground"),
            (9318,-3575.1,-409,-2616.338,null,"to end of swooper hall"),
            (9455,-2610.438,-409,-2616.338,null,"end of hallway to E2"),
            //(9635,-2610.438,-359,-2616.338,null,"first AB kick observed on E2"),
            //(9911,-2610.438,2355,-2616.338,null,"last AB kick observed on E2"),
            (9926,-2610.438,2355,-2709.56,null,"up in E2 room (E2 to air)"),
            (9927,-2610.438,2355,-2802.2,-100000,"up in E2 room (air to ground)"),
            (9928,-2610.438,2355,-3172.8,null,"up in E2 room (full frame)"),
            (9929,-2610.438,2355,-3265.4,null,"up in E2 room (partial frame)"),
            (10060,-2573.59,2355,-2762.4,null,"down right to air"),
            (10061,-2537.3,2355,-2259.5,-100000,"down right to ground"),
            (10201,-2389.3,2355,-2259.5,null,"right in E2 room"),
            (10202,-2240.2,2355,-2259.5,null,"right in E2 room"),
            (10203,-2089.9,2355,-2259.5,null,"right in E2 room"),
            (10333,-2089.9,2355,-2996,null,"up in E2 room"),
            (10462,-3931.4,2355,-2596.7,null,"left out of E2 room"),
            (10463,-5776.75,null,-2194.125,null,"SELF HACK left to air"),
            (10464,-6695.484,2450,-1992.8,-100000,"air to amazing"),
            (10475,-6304.189,2458,-3758.5,null,"up off of amazing"),
            (10476,-5516.607,2458,-7291.004,null,"to scuttlebug"),
            (10486,-5426.081,2640,-7295.098,null,"into misalignment"),
            (10502,-5426.081,2810,-7295.098,null,"GP landing"),
            (10530,-5426.138,2830,-7285.947,null,"AB kick observed"),
            (10542,-5424.576,2810,-7107.368,null,"dive observed"),
            (10554,-5288.326,2840,-6887.377,null,"DR observed"),
            (10566,-5112.072,2888,-6730.258,null,"star collect"),
            (10571,-5112.072,2810,-6730.258,null,"star land"),
            (10682,-5112.072,2810,-6730.258,null,"black frame"),
        };

        private static List<(int, double)> _plushRacingPenguinProgress = new List<(int, double)> {
            (   1, 0   ),
            (   2, 0   ),
            (   3, 0   ),
            (   4, 0   ),
            (   5, 0   ),
            (   6, 0   ),
            (   7, 0   ),
            (   8, 0   ),
            (   9, 0   ),
            (   10, 0   ),
            (   11, 0   ),
            (   12, 0   ),
            (   13, 0   ),
            (   14, 0   ),
            (   15, 0   ),
            (   16, 0   ),
            (   17, 0   ),
            (   18, 0   ),
            (   19, 0   ),
            (   20, 0   ),
            (   21, 0   ),
            (   22, 0   ),
            (   23, 0   ),
            (   24, 0   ),
            (   25, 0   ),
            (   26, 0   ),
            (   27, 0   ),
            (   28, 0   ),
            (   29, 0   ),
            (   30, 0   ),
            (   31, 0   ),
            (   32, 0   ),
            (   33, 0   ),
            (   34, 0   ),
            (   35, 0   ),
            (   36, 0   ),
            (   37, 0   ),
            (   38, 0   ),
            (   39, 0   ),
            (   40, 0   ),
            (   41, 0   ),
            (   42, 0   ),
            (   43, 0   ),
            (   44, 0   ),
            (   45, 0   ),
            (   46, 0   ),
            (   47, 0   ),
            (   48, 0   ),
            (   49, 0   ),
            (   50, 0   ),
            (   51, 0   ),
            (   52, 0   ),
            (   53, 0   ),
            (   54, 0   ),
            (   55, 0   ),
            (   56, 0   ),
            (   57, 0   ),
            (   58, 0   ),
            (   59, 0   ),
            (   60, 0   ),
            (   61, 0   ),
            (   62, -190.6994967    ),
            (   63, -204.3051589    ),
            (   64, -215.4370644    ),
            (   65, -224.0952132    ),
            (   66, -230.2796051    ),
            (   67, -233.9902403    ),
            (   68, -235.2271187    ),
            (   69, -233.9902403    ),
            (   70, -230.2796051    ),
            (   71, -224.0952132    ),
            (   72, -215.4370644    ),
            (   73, -204.3051589    ),
            (   74, -190.6994967    ),
            (   75, -174.6200776    ),
            (   76, -174.6200776    ),
            (   77, -174.6200776    ),
            (   78, -174.6200776    ),
            (   79, -174.6200776    ),
            (   80, -174.6200776    ),
            (   81, -174.6200776    ),
            (   82, -174.6200776    ),
            (   83, -174.6200776    ),
            (   84, -174.6200776    ),
            (   85, -174.6200776    ),
            (   86, -174.6200776    ),
            (   87, -174.6200776    ),
            (   88, -174.6200776    ),
            (   89, -174.6200776    ),
            (   90, -174.6200776    ),
            (   91, -174.6200776    ),
            (   92, -174.6200776    ),
            (   93, -174.6200776    ),
            (   94, -174.6200776    ),
            (   95, -174.6200776    ),
            (   96, -174.6200776    ),
            (   97, -174.6200776    ),
            (   98, -174.6200776    ),
            (   99, -174.6200776    ),
            (   100, -174.6200776    ),
            (   101, -174.6200776    ),
            (   102, -174.6200776    ),
            (   103, -174.6200776    ),
            (   104, -174.6200776    ),
            (   105, -174.6200776    ),
            (   106, -174.6200776    ),
            (   107, -174.6200776    ),
            (   108, -174.6200776    ),
            (   109, -174.6200776    ),
            (   110, -174.6200776    ),
            (   111, -174.6200776    ),
            (   112, -174.6200776    ),
            (   113, -174.6200776    ),
            (   114, -155.644553 ),
            (   115, -136.3365239    ),
            (   116, -116.6528221    ),
            (   117, -96.59049417    ),
            (   118, -76.1495402 ),
            (   119, -55.32996015    ),
            (   120, -34.13175402    ),
            (   121, -12.55538508    ),
            (   122, 9.399609947 ),
            (   123, 31.73319938 ),
            (   124, 54.4454149  ),
            (   125, 77.53625649 ),
            (   126, 103.3612485 ),
            (   127, 132.0386234 ),
            (   128, 284.0235907 ),
            (   129, 318.3739322 ),
            (   130, 355.5758448 ),
            (   131, 385.2867941 ),
            (   132, 412.731396  ),
            (   133, 442.6059067 ),
            (   134, 471.3368868 ),
            (   135, 501.6467075 ),
            (   136, 531.1028998 ),
            (   137, 562.301788  ),
            (   138, 592.5154946 ),
            (   139, 624.6691498 ),
            (   140, 655.6438737 ),
            (   141, 688.655902  ),
            (   142, 716.7357064 ),
            (   143, 748.5246636 ),
            (   144, 780.866907  ),
            (   145, 813.6719901 ),
            (   146, 846.8794916 ),
            (   147, 880.5083048 ),
            (   148, 914.5316895 ),
            (   149, 948.949601  ),
            (   150, 983.7887091 ),
            (   151, 1019.022504 ),
            (   152, 1054.67745  ),
            (   153, 1090.726969 ),
            (   154, 1127.197798 ),
            (   155, 1164.06312  ),
            (   156, 1201.323048 ),
            (   157, 1238.977468 ),
            (   158, 1277.079745 ),
            (   159, 1315.550128 ),
            (   160, 1354.441582 ),
            (   161, 1393.728162 ),
            (   162, 1433.435902 ),
            (   163, 1473.538259 ),
            (   164, 1514.035108 ),
            (   165, 1554.953198 ),
            (   166, 1596.29253  ),
            (   167, 1637.999853 ),
            (   168, 1680.128373 ),
            (   169, 1722.677974 ),
            (   170, 1765.622192 ),
            (   171, 1808.960982 ),
            (   172, 1852.721093 ),
            (   173, 1896.87574  ),
            (   174, 1941.425004 ),
            (   175, 1986.395385 ),
            (   176, 2031.760302 ),
            (   177, 2077.54662  ),
            (   178, 2123.727476 ),
            (   179, 2170.302902 ),
            (   180, 2217.29949  ),
            (   181, 2264.690615 ),
            (   182, 2312.500218 ),
            (   183, 2360.710194 ),
            (   184, 2409.243387 ),
            (   185, 2458.334465 ),
            (   186, 2507.660673 ),
            (   187, 2557.591505 ),
            (   188, 2607.706555 ),
            (   189, 2658.47988  ),
            (   190, 2709.38372  ),
            (   191, 2760.972753 ),
            (   192, 2812.666289 ),
            (   193, 2865.098168 ),
            (   194, 2915.796763 ),
            (   195, 2968.220363 ),
            (   196, 3021.133967 ),
            (   197, 3074.355271 ),
            (   198, 3127.915363 ),
            (   199, 3181.876786 ),
            (   200, 3236.236863 ),
            (   201, 3290.998458 ),
            (   202, 3346.15864  ),
            (   203, 3401.720299 ),
            (   204, 3457.945016 ),
            (   205, 3514.642068 ),
            (   206, 3571.746856 ),
            (   207, 3629.251728 ),
            (   208, 3687.156671 ),
            (   209, 3743.755284 ),
            (   210, 3803.556419 ),
            (   211, 3862.67085  ),
            (   212, 3923.664235 ),
            (   213, 3984.582081 ),
            (   214, 4044.803881 ),
            (   215, 4106.905561 ),
            (   216, 4169.255049 ),
            (   217, 4230.659421 ),
            (   218, 4293.943673 ),
            (   219, 4357.613542 ),
            (   220, 4420.196723 ),
            (   221, 4484.659813 ),
            (   222, 4549.620459 ),
            (   223, 4613.382703 ),
            (   224, 4679.024783 ),
            (   225, 4745.312708 ),
            (   226, 4810.253949 ),
            (   227, 4877.075069 ),
            (   228, 4944.616367 ),
            (   229, 5010.736606 ),
            (   230, 5078.736724 ),
            (   231, 5147.642522 ),
            (   232, 5214.941809 ),
            (   233, 5284.120975 ),
            (   234, 5354.354051 ),
            (   235, 5422.831079 ),
            (   236, 5493.18797  ),
            (   237, 5564.710239 ),
            (   238, 5634.36625  ),
            (   239, 5705.902124 ),
            (   240, 5778.714727 ),
            (   241, 5922.265901 ),
            (   243, 5995.943015 ),
            (   244, 6067.820296 ),
            (   245, 6140.307968 ),
            (   246, 6213.080659 ),
            (   247, 6287.834534 ),
            (   248, 6361.386957 ),
            (   249, 6436.938496 ),
            (   250, 6514.48927  ),
            (   251, 6591.104065 ),
            (   252, 6666.063159 ),
            (   253, 6743.021489 ),
            (   254, 6821.979053 ),
            (   255, 6900.897595 ),
            (   256, 6977.432123 ),
            (   257, 7055.965885 ),
            (   258, 7136.498884 ),
            (   259, 7217.215579 ),
            (   260, 7295.316984 ),
            (   261, 7375.417624 ),
            (   262, 7457.5175   ),
            (   263, 7540.099804 ),
            (   264, 7619.768716 ),
            (   265, 7701.436865 ),
            (   266, 7785.104248 ),
            (   267, 7869.508601 ),
            (   268, 7950.745363 ),
            (   270, 8119.21649  ),
            (   272, 8288.290288 ),
            (   273, 8373.09352  ),
            (   274, 8459.896056 ),
            (   275, 8547.945392 ),
            (   276, 8632.317236 ),
            (   277, 8718.687907 ),
            (   278, 8807.058153 ),
            (   279, 8889.600012 ),
            (   280, 8972.459528 ),
            (   281, 9059.451205 ),
            (   282, 9149.293586 ),
            (   283, 9240.989961 ),
            (   284, 9330.986775 ),
            (   285, 9418.877026 ),
            (   286, 9508.665027 ),
            (   287, 9600.350877 ),
            (   288, 9693.071303 ),
            (   289, 9782.610485 ),
            (   290, 9874.053243 ),
            (   291, 9967.372062 ),
            (   292, 10058.166   ),
            (   293, 10150.85179 ),
            (   294, 10245.30148 ),
            (   295, 10337.26957 ),
            (   296, 10431.13531 ),
            (   297, 10526.8988  ),
            (   298, 10624.55426 ),
            (   299, 10723.44907 ),
            (   300, 10817.2291  ),
            (   301, 10911.49302 ),
            (   302, 11006.10712 ),
            (   303, 11101.10811 ),
            (   304, 11165.32591 ),
            (   305, 11239.69506 ),
            (   306, 11322.51146 ),
            (   307, 11410.81968 ),
            (   308, 11504.5296  ),
            (   309, 11602.09744 ),
            (   310, 11700.62695 ),
            (   311, 11799.70103 ),
            (   312, 11895.70855 ),
            (   313, 11986.8473  ),
            (   314, 12072.27434 ),
            (   315, 12148.91093 ),
            (   316, 12216.71836 ),
            (   317, 12310.47536 ),
            (   318, 12412.22713 ),
            (   319, 12514.54094 ),
            (   320, 12617.10428 ),
            (   321, 12720.18538 ),
            (   322, 12823.54786 ),
            (   323, 12927.43127 ),
            (   324, 13031.38457 ),
            (   325, 13135.65192 ),
            (   326, 13240.31715 ),
            (   327, 13345.42221 ),
            (   328, 13450.95909 ),
            (   329, 13556.89566 ),
            (   330, 13663.23113 ),
            (   331, 13769.96699 ),
            (   332, 13877.10255 ),
            (   333, 13946.7285  ),
            (   334, 14039.62096 ),
            (   335, 14140.33414 ),
            (   336, 14246.55828 ),
            (   337, 14355.8233  ),
            (   338, 14465.15358 ),
            (   339, 14574.1175  ),
            (   340, 14683.34372 ),
            (   341, 14792.38091 ),
            (   342, 14900.8204  ),
            (   343, 15009.06576 ),
            (   344, 15116.71699 ),
            (   345, 15224.16285 ),
            (   346, 15331.02587 ),
            (   347, 15437.67883 ),
            (   348, 15543.75383 ),
            (   349, 15649.60941 ),
            (   350, 15754.89621 ),
            (   351, 15859.95187 ),
            (   352, 15964.50162 ),
            (   353, 16064.57178 ),
            (   354, 16168.08721 ),
            (   355, 16272.70063 ),
            (   356, 16376.90662 ),
            (   357, 16480.7166  ),
            (   358, 16584.12311 ),
            (   359, 16687.12657 ),
            (   360, 16789.72919 ),
            (   361, 16891.93298 ),
            (   362, 16993.73558 ),
            (   363, 17095.13719 ),
            (   364, 17196.13799 ),
            (   365, 17296.73761 ),
            (   366, 17396.94223 ),
            (   367, 17496.74602 ),
            (   368, 17596.14644 ),
            (   369, 17695.13554 ),
            (   370, 17793.71601 ),
            (   371, 17891.89634 ),
            (   372, 17989.6788  ),
            (   373, 18087.05679 ),
            (   374, 18184.03708 ),
            (   375, 18280.61726 ),
            (   376, 18376.79397 ),
            (   377, 18472.57175 ),
            (   378, 18567.94943 ),
            (   379, 18662.9251  ),
            (   380, 18757.50043 ),
            (   381, 18851.68242 ),
            (   382, 18933.96417 ),
            (   383, 19026.247   ),
            (   384, 19122.9647  ),
            (   385, 19222.42839 ),
            (   386, 19323.91476 ),
            (   387, 19427.59631 ),
            (   388, 19533.48235 ),
            (   389, 19641.56355 ),
            (   390, 19738.70417 ),
            (   391, 19834.19644 ),
            (   392, 19931.88388 ),
            (   393, 20030.76367 ),
            (   394, 20128.00052 ),
            (   395, 20227.43253 ),
            (   396, 20326.43393 ),
            (   397, 20424.80849 ),
            (   398, 20525.38769 ),
            (   399, 20625.70074 ),
            (   400, 20712.09593 ),
            (   401, 20809.06924 ),
            (   402, 20911.55289 ),
            (   403, 21017.18842 ),
            (   404, 21118.32088 ),
            (   405, 21220.74609 ),
            (   406, 21323.74896 ),
            (   407, 21425.51953 ),
            (   408, 21529.18782 ),
            (   409, 21634.75383 ),
            (   410, 21742.21752 ),
            (   411, 21851.1096  ),
            (   412, 21930.3909  ),
            (   413, 22020.8761  ),
            (   414, 22119.58744 ),
            (   415, 22222.77375 ),
            (   416, 22330.397   ),
            (   417, 22439.98211 ),
            (   418, 22548.29055 ),
            (   419, 22649.7056  ),
            (   420, 22745.43236 ),
            (   421, 22837.37655 ),
            (   422, 22929.34069 ),
            (   423, 23043.98051 ),
            (   424, 23154.7382  ),
            (   425, 23267.15357 ),
            (   426, 23378.05968 ),
            (   427, 23490.57077 ),
            (   428, 23602.25156 ),
            (   429, 23714.45861 ),
            (   430, 23828.27366 ),
            (   431, 23943.69696 ),
            (   432, 24059.95602 ),
            (   433, 24173.55352 ),
            (   434, 24288.76466 ),
            (   435, 24405.5838  ),
            (   436, 24524.0112  ),
            (   437, 24642.79416 ),
            (   438, 24717.53645 ),
            (   439, 24810.99371 ),
            (   440, 24915.49876 ),
            (   441, 25026.71244 ),
            (   442, 25143.54491 ),
            (   443, 25261.38792 ),
            (   444, 25378.61738 ),
            (   445, 25495.79676 ),
            (   446, 25611.61007 ),
            (   447, 25728.91563 ),
            (   448, 25847.71349 ),
            (   449, 25967.30415 ),
            (   450, 26084.60678 ),
            (   451, 26203.40185 ),
            (   452, 26323.64603 ),
            (   453, 26445.3825  ),
            (   454, 26565.84904 ),
            (   455, 26651.61288 ),
            (   456, 26760.99025 ),
            (   457, 26880.4505  ),
            (   458, 27006.36701 ),
            (   459, 27131.48326 ),
            (   460, 27252.81241 ),
            (   461, 27368.91825 ),
            (   462, 27475.97804 ),
            (   463, 27570.25868 ),
            (   464, 27681.7101  ),
            (   465, 27805.91874 ),
            (   466, 27934.86291 ),
            (   467, 28064.54542 ),
            (   468, 28195.75468 ),
            (   469, 28325.04536 ),
            (   470, 28452.50967 ),
            (   471, 28581.45431 ),
            (   472, 28712.16132 ),
            (   473, 28812.51657 ),
            (   474, 28938.91079 ),
            (   475, 29070.21338 ),
            (   476, 29201.89922 ),
            (   477, 29328.23273 ),
            (   478, 29441.78202 ),
            (   479, 29545.48963 ),
            (   480, 29696.10269 ),
            (   481, 29832.29683 ),
            (   482, 29969.24832 ),
            (   483, 30100.95025 ),
            (   484, 30233.53015 ),
            (   485, 30367.00123 ),
            (   486, 30501.35012 ),
            (   487, 30626.65897 ),
            (   488, 30756.20394 ),
            (   489, 30886.67612 ),
            (   490, 31017.98878 ),
            (   491, 31150.14215 ),
            (   492, 31283.05868 ),
            (   493, 31410.70455 ),
            (   494, 31539.2078  ),
            (   495, 31655.18423 ),
            (   496, 31784.29684 ),
            (   497, 31913.81381 ),
            (   498, 32038.37962 ),
            (   499, 32163.81683 ),
            (   500, 32290.12507 ),
            (   501, 32417.2746  ),
            (   502, 32542.42941 ),
            (   503, 32664.23062 ),
            (   504, 32789.18525 ),
            (   505, 32914.81402 ),
            (   506, 33039.15121 ),
            (   507, 33161.90119 ),
            (   508, 33285.32531 ),
            (   509, 33409.4238  ),
            (   510, 33532.23913 ),
            (   511, 33644.96852 ),
            (   512, 33766.14228 ),
            (   513, 33888.77858 ),
            (   514, 34012.87721 ),
            (   515, 34138.43838 ),
            (   516, 34265.46211 ),
            (   517, 34391.78964 ),
            (   518, 34508.49971 ),
            (   519, 34626.66947 ),
            (   520, 34746.30175 ),
            (   521, 34867.39924 ),
            (   522, 34989.95658 ),
            (   523, 35113.97645 ),
            (   524, 35238.26291 ),
            (   525, 35352.26046 ),
            (   526, 35467.72053 ),
            (   527, 35584.64571 ),
            (   528, 35703.03078 ),
            (   529, 35822.87839 ),
            (   530, 35944.1911  ),
            (   531, 36063.39929 ),
            (   532, 36174.67252 ),
            (   533, 36287.41084 ),
            (   534, 36401.60912 ),
            (   535, 36517.2699  ),
            (   536, 36625.67334 ),
            (   537, 36737.78364 ),
            (   538, 36847.23566 ),
            (   539, 36958.25273 ),
            (   540, 37069.88289 ),
            (   541, 37182.38386 ),
            (   542, 37292.94029 ),
            (   543, 37400.88114 ),
            (   544, 37509.70637 ),
            (   545, 37619.41604 ),
            (   546, 37728.55441 ),
            (   547, 37834.98201 ),
            (   548, 37942.29399 ),
            (   549, 38050.49035 ),
            (   550, 38157.18898 ),
            (   551, 38260.28204 ),
            (   552, 38365.94723 ),
            (   553, 38472.55965 ),
            (   554, 38577.82702 ),
            (   555, 38681.75611 ),
            (   556, 38786.23274 ),
            (   557, 38891.06001 ),
            (   558, 38993.84506 ),
            (   559, 39097.17774 ),
            (   560, 39200.65969 ),
            (   561, 39302.25121 ),
            (   562, 39404.39035 ),
            (   563, 39506.15993 ),
            (   564, 39606.5603  ),
            (   565, 39707.41864 ),
            (   566, 39807.07201 ),
            (   567, 39907.06778 ),
            (   568, 39991.82058 ),
            (   569, 40088.87144 ),
            (   570, 40188.14846 ),
            (   571, 40285.74067 ),
            (   572, 40383.69705 ),
            (   573, 40481.99901 ),
            (   574, 40578.40027 ),
            (   575, 40675.16569 ),
            (   576, 40772.17042 ),
            (   577, 40867.37879 ),
            (   578, 40962.95132 ),
            (   579, 41058.31129 ),
            (   580, 41152.32681 ),
            (   581, 41246.70626 ),
            (   582, 41340.76612 ),
            (   583, 41433.58903 ),
            (   584, 41526.03643 ),
            (   585, 41618.07017 ),
            (   586, 41709.25467 ),
            (   587, 41799.17653 ),
            (   588, 41888.68633 ),
            (   589, 41977.7963  ),
            (   590, 42035.59118 ),
            (   591, 42106.16008 ),
            (   592, 42182.86946 ),
            (   593, 42264.41823 ),
            (   594, 42349.40055 ),
            (   595, 42436.52018 ),
            (   596, 42523.87914 ),
            (   597, 42610.46455 ),
            (   598, 42694.975   ),
            (   599, 42776.10415 ),
            (   600, 42852.43473 ),
            (   601, 42923.13476 ),
            (   602, 42987.4088  ),
            (   603, 43044.32206 ),
            (   604, 43121.52932 ),
            (   605, 43203.12028 ),
            (   606, 43287.11706 ),
            (   607, 43372.33077 ),
            (   608, 43457.57492 ),
            (   609, 43540.0635  ),
            (   610, 43623.08683 ),
            (   611, 43705.57714 ),
            (   612, 43786.92268 ),
            (   613, 43868.80345 ),
            (   614, 43949.95079 ),
            (   615, 44030.10264 ),
            (   616, 44110.70803 ),
            (   617, 44190.10909 ),
            (   618, 44269.60727 ),
            (   619, 44348.21182 ),
            (   620, 44426.93609 ),
            (   621, 44504.74659 ),
            (   622, 44583.12643 ),
            (   623, 44661.42816 ),
            (   624, 44740.60217 ),
            (   625, 44819.69808 ),
            (   626, 44899.66911 ),
            (   627, 44979.55919 ),
            (   628, 45060.40486 ),
            (   629, 45141.07806 ),
            (   630, 45116.31331 ),
            (   631, 45134.66998 ),
            (   632, 45161.26124 ),
            (   633, 45197.95844 ),
            (   634, 45244.21445 ),
            (   635, 45297.09654 ),
            (   636, 45359.31949 ),
            (   637, 45430.11371 ),
            (   638, 45508.66911 ),
            (   639, 45591.82062 ),
            (   640, 45672.20402 ),
            (   641, 45757.18367 ),
            (   642, 45845.55413 ),
            (   643, 45934.8774  ),
            (   644, 46018.06971 ),
            (   645, 46100.79185 ),
            (   646, 46181.81735 ),
            (   647, 46254.2006  ),
            (   648, 46328.97585 ),
            (   649, 46406.2151  ),
            (   650, 46484.11263 ),
            (   651, 46557.7495  ),
            (   652, 46633.77856 ),
            (   653, 46712.19981 ),
            (   654, 46791.89549 ),
            (   655, 46866.82839 ),
            (   656, 46944.15348 ),
            (   657, 47023.9456  ),
            (   658, 47105.35816 ),
            (   659, 47181.65656 ),
            (   660, 47282.71915 ),
            (   661, 47378.55647 ),
            (   662, 47476.34156 ),
            (   663, 47569.72911 ),
            (   664, 47665.12497 ),
            (   665, 47762.52913 ),
            (   666, 47861.9416  ),
            (   667, 47960.14949 ),
            (   668, 48055.41507 ),
            (   669, 48152.68895 ),
            (   670, 48251.97113 ),
            (   671, 48353.26162 ),
            (   672, 48454.51269 ),
            (   673, 48551.74092 ),
            (   674, 48650.97661 ),
            (   675, 48752.22062 ),
            (   676, 48855.47296 ),
            (   677, 48959.08619 ),
            (   678, 49058.27206 ),
            (   679, 49159.46627 ),
            (   680, 49262.6688  ),
            (   681, 49367.87956 ),
            (   682, 49473.8304  ),
            (   683, 49574.97501 ),
            (   684, 49678.12794 ),
            (   685, 49783.2892  ),
            (   686, 49890.45868 ),
            (   687, 49998.83055 ),
            (   688, 50101.93477 ),
            (   689, 50207.04643 ),
            (   690, 50314.1663  ),
            (   691, 50423.2945  ),
            (   692, 50534.04274 ),
            (   693, 50624.48177 ),
            (   694, 50724.41082 ),
            (   695, 50821.88282 ),
            (   696, 50919.71515 ),
            (   697, 51019.94366 ),
            (   698, 51124.01039 ),
            (   699, 51231.91534 ),
            (   700, 51343.6585  ),
            (   701, 51459.23987 ),
            (   702, 51578.65946 ),
            (   703, 51701.91726 ),
            (   704, 51829.01328 ),
            (   705, 51959.94685 ),
            (   706, 52094.01505 ),
            (   707, 52228.44349 ),
            (   708, 52363.23212 ),
            (   709, 52498.38094 ),
            (   710, 52633.89073 ),
            (   711, 52769.75801 ),
            (   712, 52904.7465  ),
            (   713, 53063.09025 ),
            (   714, 53188.81067 ),
            (   715, 53316.11919 ),
            (   716, 53443.76916 ),
            (   717, 53565.40238 ),
            (   718, 53680.08748 ),
            (   719, 53796.65383 ),
            (   720, 53915.10154 ),
            (   721, 54032.53812 ),
            (   722, 54149.25855 ),
            (   723, 54267.8601  ),
            (   724, 54387.11185 ),
            (   725, 54505.06793 ),
            (   726, 54624.89914 ),
            (   727, 54745.28555 ),
            (   728, 54864.41123 ),
            (   729, 54985.41826 ),
            (   730, 55107.0664  ),
            (   731, 55227.29365 ),
            (   732, 55350.61306 ),
            (   733, 55473.73793 ),
            (   734, 55596.02141 ),
            (   735, 55719.65822 ),
            (   736, 55844.11785 ),
            (   737, 55967.59073 ),
            (   738, 56090.33451 ),
            (   739, 56201.80871 ),
            (   740, 56322.12387 ),
            (   741, 56447.68891 ),
            (   742, 56573.88789 ),
            (   743, 56698.44055 ),
            (   744, 56824.31679 ),
            (   745, 56951.07159 ),
            (   746, 57076.80333 ),
            (   747, 57203.88726 ),
            (   748, 57332.02507 ),
            (   749, 57458.99261 ),
            (   750, 57559.07471 ),
            (   751, 57680.36321 ),
            (   752, 57809.58794 ),
            (   753, 57942.58641 ),
            (   754, 58072.9696  ),
            (   755, 58190.17457 ),
            (   756, 58299.18378 ),
            (   757, 58366.24227 ),
            (   758, 58494.52015 ),
            (   759, 58623.31636 ),
            (   760, 58751.52743 ),
            (   761, 58875.44362 ),
            (   762, 58999.98754 ),
            (   763, 59125.15872 ),
            (   764, 59250.95763 ),
            (   765, 59376.95945 ),
            (   766, 59498.95388 ),
            (   767, 59621.5756  ),
            (   768, 59744.82458 ),
            (   769, 59867.60197 ),
            (   770, 59988.05953 ),
            (   771, 60109.14459 ),
            (   772, 60230.34988 ),
            (   773, 60349.66739 ),
            (   774, 60469.61243 ),
            (   775, 60588.90557 ),
            (   776, 60707.03968 ),
            (   777, 60825.80133 ),
            (   778, 60943.87797 ),
            (   779, 61060.81536 ),
            (   780, 61178.38794 ),
            (   781, 61289.66281 ),
            (   782, 61404.45259 ),
            (   783, 61520.87355 ),
            (   784, 61636.32527 ),
            (   785, 61750.92143 ),
            (   786, 61865.90526 ),
            (   787, 61980.28032 ),
            (   788, 62093.68506 ),
            (   789, 62207.48907 ),
            (   790, 62320.65349 ),
            (   791, 62432.91143 ),
            (   792, 62545.22735 ),
            (   793, 62657.48917 ),
            (   794, 62768.75222 ),
            (   795, 62880.4292  ),
            (   796, 62991.49257 ),
            (   797, 63101.56231 ),
            (   798, 63212.04556 ),
            (   799, 63321.86889 ),
            (   800, 63430.74443 ),
            (   801, 63540.03376 ),
            (   802, 63648.63022 ),
            (   803, 63756.31188 ),
            (   804, 63864.40745 ),
            (   805, 63971.76348 ),
            (   806, 64078.25129 ),
            (   807, 64184.79944 ),
            (   808, 64290.53249 ),
            (   809, 64396.6789  ),
            (   810, 64502.6027  ),
            (   811, 64561.80361 ),
            (   812, 64649.02842 ),
            (   813, 64744.58331 ),
            (   814, 64843.02639 ),
            (   815, 64944.9444  ),
            (   816, 65047.18885 ),
            (   817, 65148.89222 ),
            (   818, 65246.20607 ),
            (   819, 65339.22527 ),
            (   820, 65426.1592  ),
            (   821, 65512.73774 ),
            (   822, 65596.10463 ),
            (   823, 65680.53295 ),
            (   824, 65789.54846 ),
            (   825, 65889.47771 ),
            (   826, 65987.84783 ),
            (   827, 66086.82955 ),
            (   828, 66186.42287 ),
            (   829, 66284.80546 ),
            (   830, 66381.58811 ),
            (   831, 66478.98236 ),
            (   832, 66575.79501 ),
            (   833, 66671.43785 ),
            (   834, 66767.69206 ),
            (   835, 66863.17923 ),
            (   836, 66957.63003 ),
            (   837, 67052.69147 ),
            (   838, 67146.02487 ),
            (   839, 67232.17508 ),
            (   840, 67323.69725 ),
            (   841, 67415.82502 ),
            (   842, 67507.47375 ),
            (   843, 67598.72372 ),
            (   844, 67689.57491 ),
            (   845, 67780.11629 ),
            (   846, 67870.67267 ),
            (   847, 67960.83028 ),
            (   848, 68050.92204 ),
            (   849, 68140.24959 ),
            (   850, 68229.63833 ),
            (   851, 68318.16991 ),
            (   852, 68406.74837 ),
            (   853, 68494.48853 ),
            (   854, 68582.25757 ),
            (   855, 68669.19421 ),
            (   856, 68741.89783 ),
            (   857, 68819.48095 ),
            (   858, 68900.74931 ),
            (   859, 68984.36328 ),
            (   860, 69069.14239 ),
            (   861, 69153.73948 ),
            (   862, 69237.21073 ),
            (   863, 69319.28132 ),
            (   865, 69482.71534 ),
            (   866, 69564.02328 ),
            (   867, 69644.50865 ),
            (   868, 69725.29186 ),
            (   869, 69804.99799 ),
            (   870, 69885.01312 ),
            (   871, 69969.0719  ),
            (   872, 70048.96575 ),
            (   873, 70129.87064 ),
            (   874, 70210.11439 ),
            (   875, 70288.85631 ),
            (   876, 70368.58263 ),
            (   877, 70447.74632 ),
            (   878, 70525.30605 ),
            (   879, 70603.8513  ),
            (   880, 70682.15147 ),
            (   881, 70758.5301  ),
            (   882, 70835.89329 ),
            (   883, 70912.89458 ),
            (   884, 70988.09114 ),
            (   885, 71064.27224 ),
            (   886, 71140.00553 ),
            (   887, 71214.02025 ),
            (   888, 71289.0195  ),
            (   889, 71363.45905 ),
            (   890, 71436.29169 ),
            (   891, 71510.10887 ),
            (   892, 71583.2801  ),
            (   893, 71654.93066 ),
            (   894, 71727.5666  ),
            (   895, 71799.43904 ),
            (   896, 71869.90816 ),
            (   897, 71941.36197 ),
            (   898, 72011.96962 ),
            (   899, 72081.25707 ),
            (   900, 72152.01474 ),
            (   901, 72221.1687  ),
            (   902, 72289.45405 ),
            (   903, 72357.59662 ),
            (   904, 72425.05234 ),
            (   905, 72492.1101  ),
            (   906, 72558.75909 ),
            (   907, 72625.00957 ),
            (   908, 72690.86308 ),
            (   909, 72756.30807 ),
            (   910, 72821.35511 ),
            (   911, 72885.95597 ),
            (   912, 72950.13545 ),
            (   913, 73013.90743 ),
            (   914, 73077.29082 ),
            (   915, 73134.8211  ),
            (   916, 73195.08086 ),
            (   917, 73256.20651 ),
            (   918, 73317.66864 ),
            (   919, 73378.98384 ),
            (   920, 73439.52981 ),
            (   921, 73499.04328 ),
            (   922, 73557.2986  ),
            (   923, 73615.53541 ),
            (   924, 73673.78102 ),
            (   925, 73732.54606 ),
            (   926, 73783.0009  ),
            (   927, 73842.34404 ),
            (   928, 73905.33723 ),
            (   929, 73968.31246 ),
            (   930, 74027.44049 ),
            (   931, 74088.97024 ),
            (   932, 74152.46027 ),
            (   933, 74214.05911 ),
            (   934, 74272.66109 ),
            (   935, 74332.85121 ),
            (   936, 74391.39093 ),
            (   937, 74451.83999 ),
            (   938, 74511.88798 ),
            (   939, 74570.32311 ),
            (   940, 74630.66757 ),
            (   941, 74691.50732 ),
            (   942, 74750.95314 ),
            (   943, 74812.16058 ),
            (   944, 74873.59191 ),
            (   945, 74933.72795 ),
            (   946, 74995.46914 ),
            (   947, 75057.25426 ),
            (   948, 75117.39265 ),
            (   949, 75179.13384 ),
            (   950, 75240.97198 ),
            (   951, 75301.11037 ),
            (   952, 75362.85156 ),
            (   953, 75424.6897  ),
            (   954, 75484.82809 ),
            (   956, 75608.35585 ),
            (   957, 75668.49412 ),
            (   958, 75730.23296 ),
            (   959, 75791.88824 ),
            (   960, 75852.02651 ),
            (   961, 75913.76759 ),
            (   962, 75975.46337 ),
            (   963, 76035.60164 ),
            (   964, 76097.34271 ),
            (   965, 76159.03849 ),
            (   966, 76219.17903 ),
            (   968, 76341.55337 ),
            (   969, 76401.84114 ),
            (   970, 76463.4757  ),
            (   971, 76525.07152 ),
            (   972, 76585.33828 ),
            (   973, 76646.97284 ),
            (   974, 76708.53427 ),
            (   975, 76768.80103 ),
            (   976, 76830.43559 ),
            (   977, 76892.0315  ),
            (   978, 76952.29826 ),
            (   979, 77012.83016 ),
            (   980, 77115.28984 ),
            (   981, 77176.23389 ),
            (   982, 77236.94591 ),
            (   983, 77297.32297 ),
            (   984, 77357.70022 ),
            (   985, 77418.07757 ),
            (   986, 77478.45368 ),
            (   987, 77538.82371 ),
            (   988, 77599.19375 ),
            (   989, 77659.56379 ),
            (   990, 77719.93371 ),
            (   991, 77780.30375 ),
            (   992, 77840.67379 ),
            (   993, 77901.04382 ),
            (   994, 77961.4133  ),
            (   995, 78021.78277 ),
            (   996, 78082.1523  ),
            (   997, 78142.52183 ),
            (   998, 78202.89131 ),
            (   999, 78263.26076 ),
            (   1000, 78323.63029 ),
            (   1001, 78383.711   ),
            (   1002, 78444.07519 ),
            (   1003, 78504.44412 ),
            (   1004, 78564.81317 ),
            (   1005, 78625.18221 ),
            (   1006, 78685.55125 ),
            (   1007, 78745.92019 ),
            (   1008, 78806.28923 ),
            (   1009, 78866.65828 ),
            (   1010, 78927.02732 ),
            (   1011, 78987.39631 ),
            (   1012, 79047.7653  ),
            (   1013, 79108.13434 ),
            (   1014, 79168.50339 ),
            (   1015, 79228.87232 ),
            (   1016, 79289.24136 ),
            (   1017, 79349.61041 ),
            (   1018, 79409.97918 ),
            (   1019, 79470.34576 ),
            (   1020, 79530.71239 ),
            (   1021, 79591.07902 ),
            (   1022, 79651.4456  ),
            (   1023, 79711.30447 ),
            (   1024, 79771.19615 ),
            (   1025, 79830.16634 ),
            (   1026, 79890.12229 ),
            (   1027, 79950.07796 ),
            (   1028, 80009.31097 ),
            (   1029, 80068.15852 ),
            (   1030, 80128.1142  ),
            (   1031, 80188.06987 ),
            (   1032, 80248.02554 ),
            (   1033, 80307.98122 ),
            (   1034, 80367.93689 ),
            (   1035, 80427.89257 ),
            (   1036, 80487.84824 ),
            (   1037, 80547.80391 ),
            (   1038, 80607.75959 ),
            (   1039, 80667.71526 ),
            (   1040, 80727.67094 ),
            (   1041, 80720.09904 ),
            (   1042, 80720.09904 ),
            (   1043, 80720.09904 ),
            (   1044, 80720.09904 ),
            (   1045, 80720.09904 ),
            (   1046, 80720.09904 ),
            (   1047, 80720.09904 ),
            (   1048, 80720.09904 ),
            (   1049, 80720.09904 ),
            (   1050, 80720.09904 ),
            (   1051, 80720.09904 ),
            (   1052, 80720.09904 ),
            (   1053, 80720.09904 ),
            (   1054, 80720.09904 ),
            (   1055, 80720.09904 ),
            (   1056, 80720.09904 ),
            (   1057, 80720.09904 ),
            (   1058, 80720.09904 ),
            (   1059, 80720.09904 ),
            (   1060, 80720.09904 ),
            (   1061, 80720.09904 ),
            (   1062, 80720.09904 ),
            (   1063, 80720.09904 ),
            (   1064, 80720.09904 ),
            (   1065, 80720.09904 ),
            (   1066, 80720.09904 ),
            (   1067, 80724.09528 ),
            (   1068, 80724.0928  ),
            (   1069, 80724.08897 ),
            (   1070, 80724.08355 ),
            (   1071, 80724.07726 ),
            (   1072, 80724.06888 ),
            (   1073, 80724.05963 ),
            (   1074, 80724.04782 ),
            (   1075, 80724.03562 ),
            (   1076, 80724.02134 ),
            (   1077, 80724.06739 ),
            (   1078, 80724.11038 ),
            (   1079, 80724.15348 ),
            (   1080, 80724.19349 ),
            (   1081, 80724.23409 ),
            (   1082, 80724.27161 ),
            (   1083, 80724.30923 ),
            (   1084, 80724.34328 ),
            (   1085, 80724.3784  ),
            (   1086, 80724.40993 ),
            (   1087, 80724.44253 ),
            (   1088, 80724.47104 ),
            (   1089, 80724.50115 ),
            (   1090, 80724.52714 ),
            (   1091, 80724.55423 ),
            (   1092, 80724.63795 ),
            (   1093, 80724.72375 ),
            (   1094, 80724.80495 ),
            (   1095, 80724.88822 ),
            (   1096, 80724.96686 ),
            (   1097, 80725.04758 ),
            (   1098, 80725.1237  ),
            (   1099, 80725.20186 ),
            (   1100, 80725.27592 ),
            (   1101, 80725.35204 ),
            (   1102, 80725.42302 ),
            (   1103, 80725.49709 ),
            (   1104, 80725.566   ),
            (   1105, 80725.638   ),
            (   1106, 80725.70433 ),
            (   1107, 80725.77426 ),
            (   1108, 80725.89926 ),
            (   1109, 80726.02783 ),
            (   1110, 80726.15123 ),
            (   1111, 80726.27773 ),
            (   1112, 80726.39903 ),
            (   1113, 80726.52393 ),
            (   1114, 80726.64313 ),
            (   1115, 80726.76645 ),
            (   1116, 80726.88352 ),
            (   1117, 80727.00523 ),
            (   1118, 80727.1207  ),
            (   1119, 80727.24079 ),
            (   1120, 80727.35465 ),
            (   1121, 80727.47311 ),
            (   1122, 80727.58581 ),
            (   1123, 80727.70267 ),
            (   1124, 80727.81376 ),
            (   1125, 80727.92949 ),
            (   1126, 80728.0394  ),
            (   1127, 80728.15401 ),
            (   1128, 80728.2628  ),
            (   1129, 80728.37625 ),
            (   1130, 80728.48386 ),
            (   1131, 80728.59619 ),
            (   1132, 80728.70268 ),
            (   1133, 80728.81432 ),
            (   1134, 80728.92014 ),
            (   1135, 80729.03114 ),
            (   1136, 80729.13581 ),
            (   1137, 80729.24615 ),
            (   1138, 80729.35015 ),
            (   1139, 80729.45982 ),
            (   1140, 80729.56365 ),
            (   1141, 80729.67317 ),
            (   1142, 80729.77631 ),
            (   1143, 80729.88517 ),
            (   1144, 80729.98809 ),
            (   1145, 80730.0968  ),
            (   1146, 80730.23545 ),
            (   1147, 80730.3009  ),
            (   1148, 80730.28669 ),
            (   1149, 80730.19863 ),
            (   1150, 80730.03121 ),
            (   1151, 80729.79025 ),
            (   1152, 80729.46976 ),
            (   1153, 80729.07602 ),
            (   1155, 80728.05667 ),
            (   1156, 80727.43185 ),
            (   1157, 80726.73443 ),
            (   1158, 80725.95842 ),
            (   1159, 80725.11061 ),
            (   1160, 80724.18496 ),
            (   1161, 80723.18786 ),
            (   1162, 80722.11372 ),
            (   1163, 80720.96894 ),
            (   1164, 80719.74793 ),
            (   1165, 80718.45708 ),
            (   1166, 80717.09127 ),
            (   1167, 80715.65645 ),
            (   1168, 80714.1475  ),
            (   1169, 80712.57081 ),
            (   1170, 80710.92129 ),
            (   1171, 80709.20488 ),
            (   1172, 80707.41692 ),
            (   1173, 80705.56339 ),
            (   1174, 80703.63961 ),
            (   1175, 80701.65155 ),
            (   1176, 80699.59504 ),
            (   1177, 80697.47559 ),
            (   1178, 80695.28898 ),
            (   1179, 80693.04123 ),
            (   1180, 80690.72814 ),
            (   1181, 80688.35523 ),
            (   1182, 80685.91878 ),
            (   1183, 80683.42433 ),
            (   1184, 80680.86817 ),
            (   1185, 80678.25582 ),
            (   1186, 80675.58356 ),
            (   1187, 80672.85696 ),
            (   1188, 80670.07275 ),
            (   1189, 80667.23602 ),
            (   1190, 80664.34399 ),
            (   1191, 80661.40177 ),
            (   1192, 80658.40608 ),
            (   1193, 80655.36202 ),
            (   1194, 80652.26683 ),
            (   1195, 80649.12562 ),
            (   1196, 80645.93607 ),
            (   1197, 80642.70235 ),
            (   1198, 80639.42263 ),
            (   1199, 80636.10158 ),
            (   1200, 80632.73687 ),
            (   1201, 80629.33269 ),
            (   1202, 80625.88769 ),
            (   1203, 80622.40605 ),
            (   1204, 80618.88593 ),
            (   1205, 80615.33156 ),
            (   1206, 80611.74108 ),
            (   1207, 80608.11917 ),
            (   1208, 80604.46399 ),
            (   1209, 80600.77976 ),
            (   1210, 80597.06564 ),
            (   1211, 80593.32485 ),
            (   1212, 80589.55651 ),
            (   1213, 80585.76438 ),
            (   1214, 80581.94761 ),
            (   1215, 80578.10991 ),
            (   1216, 80574.25043 ),
            (   1217, 80570.37243 ),
            (   1218, 80566.47605 ),
            (   1219, 80562.56405 ),
            (   1220, 80558.63653 ),
            (   1221, 80554.69579 ),
            (   1222, 80550.74243 ),
            (   1223, 80546.77927 ),
            (   1224, 80542.80639 ),
            (   1225, 80538.82612 ),
            (   1226, 80534.83907 ),
            (   1227, 80530.8494  ),
            (   1228, 80530.8494  ),
            (   1229, 80530.8494  ),
            (   1230, 80530.8494  ),
            (   1231, 80530.8494  ),
            (   1232, 80530.8494  ),
            (   1233, 80530.8494  ),
            (   1234, 80530.8494  ),
            (   1235, 80530.8494  ),
            (   1236, 80530.8494  ),
            (   1237, 80530.8494  ),
            (   1238, 80530.8494  ),
            (   1239, 80530.8494  ),
            (   1240, 80530.8494  ),
            (   1241, 80530.8494  ),
            (   1242, 80530.8494  ),
            (   1243, 80530.8494  ),
            (   1244, 80530.8494  ),
            (   1245, 80530.8494  ),
            (   1246, 80530.8494  ),
            (   1247, 80530.8494  ),
            (   1248, 80530.8494  ),
            (   1249, 80530.8494  ),
            (   1250, 80530.8494  ),
            (   1251, 80530.8494  ),
            (   1252, 80530.8494  ),
            (   1253, 80530.8494  ),
            (   1254, 80530.8494  ),
            (   1255, 80530.8494  ),
            (   1256, 80530.8494  ),
            (   1257, 80530.8494  ),
            (   1258, 80530.8494  ),
            (   1259, 80530.8494  ),
            (   1260, 80530.8494  ),
            (   1261, 80530.8494  ),
            (   1262, 80530.8494  ),
            (   1263, 80530.8494  ),
            (   1264, 80530.8494  ),
            (   1265, 80530.8494  ),
            (   1266, 80530.8494  ),
            (   1267, 80530.8494  ),
            (   1268, 80530.8494  ),
            (   1269, 80530.8494  ),
            (   1270, 80530.8494  ),
            (   1271, 80530.8494  ),
            (   1272, 80530.8494  ),
            (   1273, 80530.8494  ),
            (   1274, 80530.8494  ),
            (   1275, 80530.8494  ),
            (   1276, 80530.8494  ),
            (   1277, 80530.8494  ),
            (   1278, 80530.8494  ),
            (   1279, 80530.8494  ),
            (   1280, 80530.8494  ),
            (   1281, 80530.8494  ),
            (   1282, 80530.8494  ),
            (   1283, 80530.8494  ),
            (   1284, 80530.8494  ),
            (   1285, 80530.8494  ),
            (   1286, 80530.8494  ),
            (   1287, 80530.8494  ),
            (   1288, 80530.8494  ),
            (   1289, 80530.8494  ),
            (   1290, 80530.8494  ),
            (   1291, 80530.8494  ),
            (   1292, 80530.8494  ),
            (   1293, 80530.8494  ),
            (   1294, 80530.8494  ),
            (   1295, 80530.8494  ),
            (   1296, 80530.8494  ),
            (   1297, 80530.8494  ),
            (   1298, 80530.8494  ),
            (   1299, 80530.8494  ),
            (   1300, 80530.8494  ),
            (   1301, 80530.8494  ),
            (   1302, 80530.8494  ),
            (   1303, 80530.8494  ),
            (   1304, 80530.8494  ),
            (   1305, 80530.8494  ),
            (   1306, 80530.8494  ),
            (   1307, 80530.8494  ),
            (   1308, 80530.8494  ),
            (   1309, 80530.8494  ),
            (   1310, 80530.8494  ),
            (   1311, 80530.8494  ),
            (   1312, 80530.8494  ),
            (   1313, 80530.8494  ),
            (   1314, 80530.8494  ),
            (   1315, 80530.8494  ),
            (   1316, 80530.8494  ),
            (   1317, 80530.8494  ),
            (   1318, 80530.8494  ),
            (   1319, 80530.8494  ),
            (   1320, 80530.8494  ),
            (   1321, 80530.8494  ),
            (   1322, 80530.8494  ),
            (   1323, 80530.8494  ),
            (   1324, 80530.8494  ),
            (   1325, 80530.8494  ),
            (   1326, 80530.8494  ),
            (   1327, 80530.8494  ),
            (   1328, 80530.8494  ),
            (   1329, 80530.8494  ),
            (   1330, 80530.8494  ),
            (   1331, 80530.8494  ),
            (   1332, 80530.8494  ),
            (   1333, 80530.8494  ),
            (   1334, 80530.8494  ),
            (   1335, 80530.8494  ),
            (   1336, 80530.8494  ),
            (   1337, 80530.8494  ),
            (   1338, 80530.8494  ),
            (   1339, 80530.8494  ),
            (   1340, 80530.8494  ),
            (   1341, 80530.8494  ),
            (   1342, 80530.8494  ),
            (   1343, 80530.8494  ),
            (   1344, 80530.8494  ),
            (   1345, 80530.8494  ),
            (   1346, 80530.8494  ),
            (   1347, 80530.8494  ),
            (   1348, 80530.8494  ),
            (   1349, 80530.8494  ),
            (   1350, 80530.8494  ),
            (   1351, 80530.8494  ),
            (   1352, 80530.8494  ),
            (   1353, 80530.8494  ),
            (   1354, 80530.8494  ),
            (   1355, 80530.8494  ),
            (   1356, 80530.8494  ),
            (   1357, 80530.8494  ) };
    }
}

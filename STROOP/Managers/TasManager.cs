using STROOP.Controls;
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
    public class TasManager : DataManager
    {
        private static readonly List<VariableGroup> ALL_VAR_GROUPS =
            new List<VariableGroup>()
            {
                VariableGroup.Basic,
                VariableGroup.Advanced,
                VariableGroup.TAS,
                VariableGroup.Point,
                VariableGroup.Point2,
                VariableGroup.MorePoint,
                VariableGroup.Scheduler,
            };

        private static readonly List<VariableGroup> VISIBLE_VAR_GROUPS =
            new List<VariableGroup>()
            {
                VariableGroup.Basic,
                VariableGroup.Advanced,
                VariableGroup.Point,
                VariableGroup.MorePoint,
                VariableGroup.Scheduler,
            };

        public TasManager(string varFilePath, TabPage tabControl, WatchVariableFlowLayoutPanel watchVariablePanel)
            : base(varFilePath, watchVariablePanel, ALL_VAR_GROUPS, VISIBLE_VAR_GROUPS)
        {
            SplitContainer splitContainerTas = tabControl.Controls["splitContainerTas"] as SplitContainer;
            
            Button buttonTasStorePosition = splitContainerTas.Panel1.Controls["buttonTasStorePosition"] as Button;
            buttonTasStorePosition.Click += (sender, e) => StoreInfo(x: true, y: true, z: true);
            ControlUtilities.AddContextMenuStripFunctions(
                buttonTasStorePosition,
                new List<string>()
                {
                    "Store Position",
                    "Store Lateral Position",
                    "Store X",
                    "Store Y",
                    "Store Z",
                    "Go to Closest Floor Vertex",
                    "Go to Closest Floor Vertex Misalignment",
                },
                new List<Action>()
                {
                    () => StoreInfo(x: true, y: true, z: true),
                    () => StoreInfo(x: true, z: true),
                    () => StoreInfo(x: true),
                    () => StoreInfo(y: true),
                    () => StoreInfo(z: true),
                    () => ButtonUtilities.GotoTriangleVertexClosest(Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset), false),
                    () => ButtonUtilities.GotoTriangleVertexClosest(Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset), true),
                });

            Button buttonTasStoreAngle = splitContainerTas.Panel1.Controls["buttonTasStoreAngle"] as Button;
            buttonTasStoreAngle.Click += (sender, e) => StoreInfo(angle: true);
            
            Button buttonTasTakePosition = splitContainerTas.Panel1.Controls["buttonTasTakePosition"] as Button;
            buttonTasTakePosition.Click += (sender, e) => TakeInfo(x: true, y: true, z: true);
            ControlUtilities.AddContextMenuStripFunctions(
                buttonTasTakePosition,
                new List<string>()
                {
                    "Take Position",
                    "Take Lateral Position",
                    "Take X",
                    "Take Y",
                    "Take Z"
                },
                new List<Action>()
                {
                    () => TakeInfo(x: true, y: true, z: true),
                    () => TakeInfo(x: true, z: true),
                    () => TakeInfo(x: true),
                    () => TakeInfo(y: true),
                    () => TakeInfo(z: true),
                });

            Button buttonTasTakeMarioAngle = splitContainerTas.Panel1.Controls["buttonTasTakeAngle"] as Button;
            buttonTasTakeMarioAngle.Click += (sender, e) => TakeInfo(angle: true);

            Button buttonTasPasteSchedule = splitContainerTas.Panel1.Controls["buttonTasPasteSchedule"] as Button;
            buttonTasPasteSchedule.Click += (sender, e) => SetScheduler(Clipboard.GetText(), false);
            ControlUtilities.AddContextMenuStripFunctions(
                buttonTasPasteSchedule,
                new List<string>()
                {
                    "Paste Schedule as Floats"
                },
                new List<Action>()
                {
                    () => SetScheduler(Clipboard.GetText(), true)
                });
        }

        private void StoreInfo(
            bool x = false, bool y = false, bool z = false, bool angle = false)
        {
            if (x) SpecialConfig.CustomX = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XOffset);
            if (y) SpecialConfig.CustomY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
            if (z) SpecialConfig.CustomZ = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZOffset);
            if (angle) SpecialConfig.CustomAngle = Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
        }

        private void TakeInfo(
            bool x = false, bool y = false, bool z = false, bool angle = false)
        {
            if (x) Config.Stream.SetValue((float)SpecialConfig.CustomX, MarioConfig.StructAddress + MarioConfig.XOffset);
            if (y) Config.Stream.SetValue((float)SpecialConfig.CustomY, MarioConfig.StructAddress + MarioConfig.YOffset);
            if (z) Config.Stream.SetValue((float)SpecialConfig.CustomZ, MarioConfig.StructAddress + MarioConfig.ZOffset);
            if (angle) Config.Stream.SetValue((ushort)SpecialConfig.CustomAngle, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
        }

        public void ShowTaserVariables()
        {
            _variablePanel.ShowOnlyVariableGroups(
                new List<VariableGroup>() { VariableGroup.TAS, VariableGroup.Custom });
        }

        public void MakeYawVariablesBeTruncated()
        {
            _variablePanel.MakeYawVariablesBeTruncated();
        }

        public void SetScheduler(string text, bool useFloats)
        {
            List<string> lines = text.Split('\n').ToList();
            List<List<string>> linePartsList = lines.ConvertAll(line => ParsingUtilities.ParseStringList(line));

            Dictionary<uint, (double, double, double, double, List<double>)> schedule =
                new Dictionary<uint, (double, double, double, double, List<double>)>();
            foreach (List<string> lineParts in linePartsList)
            {
                if (lineParts.Count == 0) continue;
                uint? globalTimerNullable = ParsingUtilities.ParseUIntNullable(lineParts[0]);
                if (!globalTimerNullable.HasValue) continue;
                uint globalTimer = globalTimerNullable.Value;

                double x = lineParts.Count >= 2 ? ParsingUtilities.ParseDoubleNullable(lineParts[1]) ?? Double.NaN : Double.NaN;
                double y = lineParts.Count >= 3 ? ParsingUtilities.ParseDoubleNullable(lineParts[2]) ?? Double.NaN : Double.NaN;
                double z = lineParts.Count >= 4 ? ParsingUtilities.ParseDoubleNullable(lineParts[3]) ?? Double.NaN : Double.NaN;
                double angle = lineParts.Count >= 5 ? ParsingUtilities.ParseDoubleNullable(lineParts[4]) ?? Double.NaN : Double.NaN;

                if (useFloats)
                {
                    x = (float)x;
                    y = (float)y;
                    z = (float)z;
                    angle = (float)angle;
                }

                List<double> doubleList = new List<double>();
                for (int i = 5; i < lineParts.Count; i++)
                {
                    double value = ParsingUtilities.ParseDoubleNullable(lineParts[i]) ?? Double.NaN;
                    if (useFloats) value = (float)value;
                    doubleList.Add(value);
                }

                schedule[globalTimer] = (x, y, z, angle, doubleList);
            }

            SetScheduler(schedule);
        }

        private void SetScheduler(Dictionary<uint, (double, double, double, double, List<double>)> schedule) {
            PositionAngle.Schedule = schedule;
            SpecialConfig.PointPosPA = PositionAngle.Scheduler;
            SpecialConfig.PointAnglePA = PositionAngle.Scheduler;

            RemoveVariableGroup(VariableGroup.Scheduler);
            List<List<double>> doubleListList = schedule.Values.ToList().ConvertAll(tuple => tuple.Item5);
            int maxDoubleListCount = doubleListList.Count == 0 ? 0 : doubleListList.Max(doubleList => doubleList.Count);
            for (int i = 0; i < maxDoubleListCount; i++)
            {
                string specialType = WatchVariableSpecialUtilities.AddSchedulerEntry(i);
                WatchVariable watchVariable =
                    new WatchVariable(
                        memoryTypeName: null,
                        specialType: specialType,
                        baseAddressType: BaseAddressTypeEnum.None,
                        offsetUS: null,
                        offsetJP: null,
                        offsetSH: null,
                        offsetEU: null,
                        offsetDefault: null,
                        mask: null,
                        shift: null,
                        handleMapping: true);
                WatchVariableControlPrecursor precursor =
                    new WatchVariableControlPrecursor(
                        name: "Var " + (i + 1),
                        watchVar: watchVariable,
                        subclass: WatchVariableSubclass.Number,
                        backgroundColor: ColorUtilities.GetColorFromString("Purple"),
                        displayType: null,
                        roundingLimit: null,
                        useHex: null,
                        invertBool: null,
                        isYaw: null,
                        coordinate: null,
                        groupList: new List<VariableGroup>() { VariableGroup.Scheduler });
                WatchVariableControl control = precursor.CreateWatchVariableControl();
                AddVariable(control);
            }
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;
            base.Update(updateView);
        }
    }
}

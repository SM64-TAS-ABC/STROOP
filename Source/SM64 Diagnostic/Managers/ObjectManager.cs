using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SM64_Diagnostic.Controls;
using SM64_Diagnostic.Extensions;
using SM64_Diagnostic.Structs.Configurations;
using SM64Diagnostic.Controls;
using static SM64_Diagnostic.Controls.AngleDataContainer;

namespace SM64_Diagnostic.Managers
{
    public class ObjectManager : DataManager
    {
        List<WatchVariableControl> _behaviorDataControls = new List<WatchVariableControl>();
        ObjectAssociations _objAssoc;

        object _watchVarLocker = new object();

        string _slotIndex;
        string _slotPos;
        string _behavior;
        bool _unclone = false;
        bool _revive = false;

        Button _cloneButton;
        Button _unloadButton;
        Label _objAddressLabelValue;
        Label _objAddressLabel;
        Label _objSlotIndexLabel;
        Label _objSlotPositionLabel;
        Label _objBehaviorLabel;
        TextBox _objectNameTextBox;
        Panel _objectBorderPanel;
        IntPictureBox _objectImagePictureBox;

        #region Fields
        public void SetBehaviorWatchVariables(List<WatchVariable> value, Color color)
        {
            lock (_watchVarLocker)
            {
                // Remove old watchVars from list
                RemoveWatchVaraibles(_behaviorDataControls);
                _behaviorDataControls.Clear();

                // Add new watchVars
                _behaviorDataControls.AddRange(AddWatchVariables(value, color));
                _behaviorDataControls.ForEach(w => w.OtherOffsets = _currentAddresses);
            }
        }

        List<uint> _currentAddresses = new List<uint>();
        public List<uint> CurrentAddresses
        {
            get
            {
                return _currentAddresses;
            }
            set
            {
                if (_currentAddresses.SequenceEqual(value))
                    return;

                _currentAddresses = value.ToList();

                if (_currentAddresses.Count > 1)
                    _objAddressLabelValue.Text = "";
                else if (_currentAddresses.Count > 0)
                    _objAddressLabelValue.Text = "0x" + _currentAddresses[0].ToString("X8");
                else
                    _objAddressLabelValue.Text = "";

                AddressChanged();

                foreach (WatchVariableControl watchVar in _dataControls)
                {
                    watchVar.OtherOffsets = _currentAddresses;
                }
            }
        }

        public string SlotIndex
        {
            get
            {
                return _slotIndex;
            }
            set
            {
                if (_slotIndex != value)
                {
                    _slotIndex = value;
                    _objSlotIndexLabel.Text = _slotIndex;
                }
            }
        }

        public string SlotPos
        {
            get
            {
                return _slotPos;
            }
            set
            {
                if (_slotPos != value)
                {
                    _slotPos = value;
                    _objSlotPositionLabel.Text = _slotPos;
                }
            }
        }

        public string Behavior
        {
            get
            {
                return _behavior;
            }
            set
            {
                if (_behavior != value)
                {
                    _behavior = value;
                    _objBehaviorLabel.Text = value;
                }
            }
        }

        public string Name
        {
            get
            {
                return _objectNameTextBox.Text;
            }
            set
            {
                if (_objectNameTextBox.Text != value)
                    _objectNameTextBox.Text = value;
            }
        }

        public Color BackColor
        {
            set
            {
                if (_objectBorderPanel.BackColor != value)
                {
                    _objectBorderPanel.BackColor = value;
                    _objectImagePictureBox.BackColor = value.Lighten(0.7);
                }
            }
            get
            {
                return _objectBorderPanel.BackColor;
            }
        }

        public Image Image
        {
            get
            {
                return _objectImagePictureBox.Image;
            }
            set
            {
                if (_objectImagePictureBox.Image != value)
                    _objectImagePictureBox.Image = value;
            }
        }

        #endregion

        protected override void InitializeSpecialVariables()
        {
            _specialWatchVars = new List<IDataContainer>()
            {
                new DataContainer("MarioDistanceToObject"),
                new DataContainer("MarioLateralDistanceToObject"),
                new DataContainer("MarioVerticalDistanceToObject"),
                new DataContainer("MarioDistanceToObjectHome"),
                new DataContainer("MarioLateralDistanceToObjectHome"),
                new DataContainer("MarioVerticalDistanceToObjectHome"),
                new AngleDataContainer("AngleObjectToMario"),
                new AngleDataContainer("DeltaAngleObjectToMario", AngleViewModeType.Signed),
                new AngleDataContainer("AngleMarioToObject"),
                new AngleDataContainer("DeltaAngleMarioToObject", AngleViewModeType.Signed),
                new AngleDataContainer("AngleObjectToHome"),
                new AngleDataContainer("DeltaAngleObjectToHome", AngleViewModeType.Signed),
                new AngleDataContainer("AngleHomeToObject"),
                new DataContainer("ObjectDistanceToHome"),
                new DataContainer("LateralObjectDistanceToHome"),
                new DataContainer("VerticalObjectDistanceToHome"),
                new DataContainer("MarioHitboxAwayFromObject"),
                new DataContainer("MarioHitboxAboveObject"),
                new DataContainer("MarioHitboxBelowObject"),
                new DataContainer("MarioHitboxOverlapsObject"),
                new DataContainer("PendulumAmplitude"),
                new DataContainer("PendulumSwingIndex"),
                new DataContainer("RngCallsPerFrame"),
            };
        }

        public ObjectManager(ProcessStream stream, ObjectAssociations objAssoc, List<WatchVariable> objectData, Control objectControl, NoTearFlowLayoutPanel variableTable)
            : base(stream, objectData, variableTable)
        { 
            _objAssoc = objAssoc;
            _objAddressLabelValue = objectControl.Controls["labelObjAddValue"] as Label;
            _objAddressLabel = objectControl.Controls["labelObjAdd"] as Label;
            _objSlotIndexLabel = objectControl.Controls["labelObjSlotIndValue"] as Label;
            _objSlotPositionLabel = objectControl.Controls["labelObjSlotPosValue"] as Label;
            _objBehaviorLabel = objectControl.Controls["labelObjBhvValue"] as Label;
            _objectNameTextBox = objectControl.Controls["textBoxObjName"] as TextBox;
            _objectBorderPanel = objectControl.Controls["panelObjectBorder"] as Panel;
            _objectImagePictureBox = _objectBorderPanel.Controls["pictureBoxObject"] as IntPictureBox;

            _objAddressLabelValue.Click += ObjAddressLabel_Click;
            _objAddressLabel.Click += ObjAddressLabel_Click;

            Panel objPanel = objectControl.Controls["panelObj"] as Panel;

            var goToButton = objPanel.Controls["buttonObjGoTo"] as Button;
            goToButton.Click += (sender, e) => MarioActions.GoToObjects(_stream, _currentAddresses);

            var retrieveButton = objPanel.Controls["buttonObjRetrieve"] as Button;
            retrieveButton.Click += (sender, e) => MarioActions.RetrieveObjects(_stream, _currentAddresses);

            var goToHomeButton = objPanel.Controls["buttonObjGoToHome"] as Button;
            goToHomeButton.Click += (sender, e) => MarioActions.GoToObjectsHome(_stream, _currentAddresses);

            var retrieveHomeButton = objPanel.Controls["buttonObjRetrieveHome"] as Button;
            retrieveHomeButton.Click += (sender, e) => MarioActions.RetrieveObjectsHome(_stream, _currentAddresses);

            var debilitateButton = objPanel.Controls["buttonObjDebilitate"] as Button;
            debilitateButton.Click += (sender, e) => MarioActions.DebilitateObject(_stream, _currentAddresses);

            var interactButton = objPanel.Controls["buttonObjInteract"] as Button;
            interactButton.Click += (sender, e) => MarioActions.InteractObject(_stream, _currentAddresses);

            _cloneButton = objPanel.Controls["buttonObjClone"] as Button;
            _cloneButton.Click += (sender, e) =>
            {
                if (CurrentAddresses.Count == 0)
                    return;

                if (_unclone)
                    MarioActions.UnCloneObject(_stream, CurrentAddresses[0]);
                else
                    MarioActions.CloneObject(_stream, CurrentAddresses[0]);
            };

            _unloadButton = objPanel.Controls["buttonObjUnload"] as Button;
            _unloadButton.Click += (sender, e) =>
            {
                if (CurrentAddresses.Count == 0)
                    return;

                if (_revive)
                    MarioActions.ReviveObject(_stream, CurrentAddresses);
                else
                    MarioActions.UnloadObject(_stream, CurrentAddresses);
            };

            var objPosGroupBox = objPanel.Controls["groupBoxObjPos"] as GroupBox;
            PositionController.initialize(
                objPosGroupBox.Controls["buttonObjPosXn"] as Button,
                objPosGroupBox.Controls["buttonObjPosXp"] as Button,
                objPosGroupBox.Controls["buttonObjPosZn"] as Button,
                objPosGroupBox.Controls["buttonObjPosZp"] as Button,
                objPosGroupBox.Controls["buttonObjPosXnZn"] as Button,
                objPosGroupBox.Controls["buttonObjPosXnZp"] as Button,
                objPosGroupBox.Controls["buttonObjPosXpZn"] as Button,
                objPosGroupBox.Controls["buttonObjPosXpZp"] as Button,
                objPosGroupBox.Controls["buttonObjPosYp"] as Button,
                objPosGroupBox.Controls["buttonObjPosYn"] as Button,
                objPosGroupBox.Controls["textBoxObjPosXZ"] as TextBox,
                objPosGroupBox.Controls["textBoxObjPosY"] as TextBox,
                objPosGroupBox.Controls["checkBoxObjPosRelative"] as CheckBox,
                (float xOffset, float yOffset, float zOffset, bool useRelative) =>
                {
                    MarioActions.TranslateObjects(
                        _stream,
                        _currentAddresses,
                        xOffset,
                        yOffset,
                        zOffset,
                        useRelative);
                });

            var objAngleGroupBox = objPanel.Controls["groupBoxObjAngle"] as GroupBox;
            ScalarController.initialize(
                objAngleGroupBox.Controls["buttonObjAngleYawN"] as Button,
                objAngleGroupBox.Controls["buttonObjAngleYawP"] as Button,
                objAngleGroupBox.Controls["textBoxObjAngleYaw"] as TextBox,
                (float yawValue) =>
                {
                    MarioActions.RotateObjects(stream, _currentAddresses, (int)yawValue, 0, 0);
                });
            ScalarController.initialize(
                objAngleGroupBox.Controls["buttonObjAnglePitchN"] as Button,
                objAngleGroupBox.Controls["buttonObjAnglePitchP"] as Button,
                objAngleGroupBox.Controls["textBoxObjAnglePitch"] as TextBox,
                (float pitchValue) =>
                {
                    MarioActions.RotateObjects(stream, _currentAddresses, 0, (int)pitchValue, 0);
                });
            ScalarController.initialize(
                objAngleGroupBox.Controls["buttonObjAngleRollN"] as Button,
                objAngleGroupBox.Controls["buttonObjAngleRollP"] as Button,
                objAngleGroupBox.Controls["textBoxObjAngleRoll"] as TextBox,
                (float rollValue) =>
                {
                    MarioActions.RotateObjects(stream, _currentAddresses, 0, 0, (int)rollValue);
                });

            var objHomeGroupBox = objPanel.Controls["groupBoxObjHome"] as GroupBox;
            PositionController.initialize(
                objHomeGroupBox.Controls["buttonObjHomeXn"] as Button,
                objHomeGroupBox.Controls["buttonObjHomeXp"] as Button,
                objHomeGroupBox.Controls["buttonObjHomeZn"] as Button,
                objHomeGroupBox.Controls["buttonObjHomeZp"] as Button,
                objHomeGroupBox.Controls["buttonObjHomeXnZn"] as Button,
                objHomeGroupBox.Controls["buttonObjHomeXnZp"] as Button,
                objHomeGroupBox.Controls["buttonObjHomeXpZn"] as Button,
                objHomeGroupBox.Controls["buttonObjHomeXpZp"] as Button,
                objHomeGroupBox.Controls["buttonObjHomeYp"] as Button,
                objHomeGroupBox.Controls["buttonObjHomeYn"] as Button,
                objHomeGroupBox.Controls["textBoxObjHomeXZ"] as TextBox,
                objHomeGroupBox.Controls["textBoxObjHomeY"] as TextBox,
                objHomeGroupBox.Controls["checkBoxObjHomeRelative"] as CheckBox,
                (float xOffset, float yOffset, float zOffset, bool useRelative) =>
                {
                    MarioActions.TranslateObjectHomes(
                        _stream,
                        _currentAddresses,
                        xOffset,
                        yOffset,
                        zOffset,
                        useRelative);
                });
        }

        private void AddressChanged()
        {
            var test = _dataControls.Where(d => d is WatchVariableControl);
            foreach (WatchVariableControl dataControl in test)
                dataControl.EditMode = false;

            if (CurrentAddresses.Count == 1)
            {
                _cloneButton.Enabled = true;
            }
            else
            {
                _cloneButton.Enabled = false;
            }
        }

        private void ObjAddressLabel_Click(object sender, EventArgs e)
        {
            if (_currentAddresses.Count == 0)
                return;

            var variableTitle = "Object Address" + (_currentAddresses.Count > 1 ? " (First of Multiple)" : ""); 
            var variableInfo = new VariableViewerForm(variableTitle, "Object",
                String.Format("0x{0:X8}", _currentAddresses[0]), String.Format("0x{0:X8}", (_currentAddresses[0] & 0x0FFFFFFF) + _stream.ProcessMemoryOffset));
            variableInfo.ShowDialog();
        }

        private void ProcessSpecialVars()
        {
            // Get Mario position
            float mX, mY, mZ, mFacing;
            mX = _stream.GetSingle(Config.Mario.StructAddress + Config.Mario.XOffset);
            mY = _stream.GetSingle(Config.Mario.StructAddress + Config.Mario.YOffset);
            mZ = _stream.GetSingle(Config.Mario.StructAddress + Config.Mario.ZOffset);
            mFacing = (float)(((_stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.RotationOffset) >> 16) % 65536) / 65536f * 2 * Math.PI);

            // Get Mario object position
            var marioObjRef = _stream.GetUInt32(Config.Mario.ObjectReferenceAddress);
            float mObjX, mObjY, mObjZ;
            mObjX = _stream.GetSingle(marioObjRef + Config.ObjectSlots.ObjectXOffset);
            mObjY = _stream.GetSingle(marioObjRef + Config.ObjectSlots.ObjectYOffset);
            mObjZ = _stream.GetSingle(marioObjRef + Config.ObjectSlots.ObjectZOffset);

            // Get Mario object hitbox variables
            float mObjHitboxRadius, mObjHitboxHeight, mObjHitboxDownOffset, mObjHitboxBottom, mObjHitboxTop;
            mObjHitboxRadius = _stream.GetSingle(marioObjRef + Config.ObjectSlots.HitboxRadius);
            mObjHitboxHeight = _stream.GetSingle(marioObjRef + Config.ObjectSlots.HitboxHeight);
            mObjHitboxDownOffset = _stream.GetSingle(marioObjRef + Config.ObjectSlots.HitboxDownOffset);
            mObjHitboxBottom = mObjY - mObjHitboxDownOffset;
            mObjHitboxTop = mObjY + mObjHitboxHeight - mObjHitboxDownOffset;

            bool firstObject = true;

            foreach (var objAddress in _currentAddresses)
            { 
                // Get object position
                float objX, objY, objZ, objFacing;
                objX = _stream.GetSingle(objAddress + Config.ObjectSlots.ObjectXOffset);
                objY = _stream.GetSingle(objAddress + Config.ObjectSlots.ObjectYOffset);
                objZ = _stream.GetSingle(objAddress + Config.ObjectSlots.ObjectZOffset);
                objFacing = (float)((UInt16)(_stream.GetUInt32(objAddress + Config.ObjectSlots.ObjectRotationOffset)) / 65536f * 2 * Math.PI);

                // Get object position
                float objHomeX, objHomeY, objHomeZ;
                objHomeX = _stream.GetSingle(objAddress + Config.ObjectSlots.HomeXOffset);
                objHomeY = _stream.GetSingle(objAddress + Config.ObjectSlots.HomeYOffset);
                objHomeZ = _stream.GetSingle(objAddress + Config.ObjectSlots.HomeZOffset);

                double angleObjectToMario = MoreMath.AngleTo_Radians(objX, objZ, mX, mZ);
                double angleObjectToHome = MoreMath.AngleTo_Radians(objX, objZ, objHomeX, objHomeZ);

                // Get object hitbox variables
                float objHitboxRadius, objHitboxHeight, objHitboxDownOffset, objHitboxBottom, objHitboxTop;
                objHitboxRadius = _stream.GetSingle(objAddress + Config.ObjectSlots.HitboxRadius);
                objHitboxHeight = _stream.GetSingle(objAddress + Config.ObjectSlots.HitboxHeight);
                objHitboxDownOffset = _stream.GetSingle(objAddress + Config.ObjectSlots.HitboxDownOffset);
                objHitboxBottom = objY - objHitboxDownOffset;
                objHitboxTop = objY + objHitboxHeight - objHitboxDownOffset;

                // Compute hitbox distances between Mario obj and obj
                double marioHitboxAwayFromObject = MoreMath.DistanceTo(mObjX, mObjZ, objX, objZ) - mObjHitboxRadius - objHitboxRadius;
                double marioHitboxAboveObject = mObjHitboxBottom - objHitboxTop;
                double marioHitboxBelowObject = objHitboxBottom - mObjHitboxTop;

                foreach (IDataContainer specialVar in _specialWatchVars)
                {
                    var newText = "";
                    double? newAngle = null;
                    switch (specialVar.SpecialName)
                    {
                        case "MarioDistanceToObject":
                            newText = Math.Round(MoreMath.DistanceTo(mX, mY, mZ, objX, objY, objZ),3).ToString();
                            break;

                        case "MarioLateralDistanceToObject":
                            newText = Math.Round(MoreMath.DistanceTo(mX, mZ, objX, objZ), 3).ToString();
                            break;

                        case "MarioVerticalDistanceToObject":
                            newText = Math.Round(mY - objY, 3).ToString();
                            break;

                        case "MarioDistanceToObjectHome":
                            newText = Math.Round(MoreMath.DistanceTo(mX, mY, mZ, objHomeX, objHomeY, objHomeZ), 3).ToString();
                            break;

                        case "MarioLateralDistanceToObjectHome":
                            newText = Math.Round(MoreMath.DistanceTo(mX, mZ, objHomeX, objHomeZ), 3).ToString();
                            break;

                        case "MarioVerticalDistanceToObjectHome":
                            newText = Math.Round(mY - objHomeY, 3).ToString();
                            break;

                        case "ObjectDistanceToHome":
                            newText = Math.Round(MoreMath.DistanceTo(objX, objY, objZ, objHomeX, objHomeY, objHomeZ), 3).ToString();
                            break;

                        case "LateralObjectDistanceToHome":
                            newText = Math.Round(MoreMath.DistanceTo(objX, objZ, objHomeX, objHomeZ), 3).ToString();
                            break;

                        case "VerticalObjectDistanceToHome":
                            newText = Math.Round(objY - objHomeY, 3).ToString();
                            break;

                        case "AngleMarioToObject":
                            newAngle = angleObjectToMario + Math.PI;
                            break;

                        case "DeltaAngleMarioToObject":
                            newAngle = mFacing - (angleObjectToMario + Math.PI);
                            break;

                        case "AngleObjectToMario":
                            newAngle = angleObjectToMario;
                            break;

                        case "DeltaAngleObjectToMario":
                            newAngle = objFacing - angleObjectToMario;
                            break;

                        case "AngleObjectToHome":
                            newAngle = angleObjectToHome;
                            break;

                        case "DeltaAngleObjectToHome":
                            newAngle = objFacing - angleObjectToHome;
                            break;

                        case "AngleHomeToObject":
                            newAngle = angleObjectToHome + Math.PI;
                            break;

                        case "MarioHitboxAwayFromObject":
                            newText = Math.Round(marioHitboxAwayFromObject, 3).ToString();
                            break;

                       case "MarioHitboxAboveObject":
                            newText = Math.Round(marioHitboxAboveObject, 3).ToString();
                            break;

                        case "MarioHitboxBelowObject":
                            newText = Math.Round(marioHitboxBelowObject, 3).ToString();
                            break;

                        case "MarioHitboxOverlapsObject":
                            if (marioHitboxAwayFromObject < 0 &&
                                marioHitboxAboveObject <= 0 &&
                                marioHitboxBelowObject <= 0)
                            {
                                newText = "True";
                            }
                            else
                            {
                                newText = "False";
                            }
                            break;

                        case "PendulumAmplitude":
                            // Get pendulum variables
                            float accelerationDirection = _stream.GetSingle(objAddress + Config.ObjectSlots.PendulumAccelerationDirection);
                            float accelerationMagnitude = _stream.GetSingle(objAddress + Config.ObjectSlots.PendulumAccelerationMagnitude);
                            float angularVelocity = _stream.GetSingle(objAddress + Config.ObjectSlots.PendulumAngularVelocity);
                            float angle = _stream.GetSingle(objAddress + Config.ObjectSlots.PendulumAngle);
                            float acceleration = accelerationDirection * accelerationMagnitude;

                            // Calculate one frame forwards to see if pendulum is speeding up or slowing down
                            float nextAccelerationDirection = accelerationDirection;
                            if (angle > 0) nextAccelerationDirection = -1;
                            if (angle < 0) nextAccelerationDirection = 1;
                            float nextAcceleration = nextAccelerationDirection * accelerationMagnitude;
                            float nextAngularVelocity = angularVelocity + nextAcceleration;
                            float nextAngle = angle + nextAngularVelocity;
                            bool speedingUp = Math.Abs(nextAngularVelocity) > Math.Abs(angularVelocity);

                            // Calculate duration of speeding up phase
                            float inflectionAngle = angle;
                            float inflectionAngularVelocity = nextAngularVelocity;
                            float speedUpDistance = 0;
                            int speedUpDuration = 0;
                            if (speedingUp)
                            {
                                // d = t * v + t(t-1)/2 * a
                                // d = tv + (t^2)a/2-ta/2
                                // d = t(v-a/2) + (t^2)a/2
                                // 0 = (t^2)a/2 + t(v-a/2) + -d
                                // t = (-B +- sqrt(B^2 - 4AC)) / (2A)
                                float tentativeSlowDownStartAngle = nextAccelerationDirection;
                                float tentativeSpeedUpDistance = tentativeSlowDownStartAngle - angle;
                                float A = nextAcceleration / 2;
                                float B = nextAngularVelocity - nextAcceleration / 2;
                                float C = -1 * tentativeSpeedUpDistance;
                                double tentativeSpeedUpDuration = (-B + nextAccelerationDirection * Math.Sqrt(B * B - 4 * A * C)) / (2 * A);
                                speedUpDuration = (int)Math.Ceiling(tentativeSpeedUpDuration);

                                // d = t * v + t(t-1)/2 * a
                                speedUpDistance = speedUpDuration * nextAngularVelocity + speedUpDuration * (speedUpDuration - 1) / 2 * nextAcceleration;
                                inflectionAngle = angle + speedUpDistance;

                                // v_f = v_i + t * a
                                inflectionAngularVelocity = nextAngularVelocity + (speedUpDuration - 2) * nextAcceleration;
                            }

                            // Calculate duration of slowing down phase

                            // v_f = v_i + t * a
                            // 0 = v_i + t * a
                            // t = v_i / a
                            int slowDownDuration = (int)Math.Abs(inflectionAngularVelocity / accelerationMagnitude);

                            // d = t * (v_i + v_f)/2
                            // d = t * (v_i + 0)/2
                            // d = t * v_i/2
                            float slowDownDistance = (slowDownDuration + 1) * inflectionAngularVelocity / 2;

                            // Combine the results from the speeding up phase and the slowing down phase
                            float totalDistance = speedUpDistance + slowDownDistance;
                            float amplitude = angle + totalDistance;
                            newText = amplitude + "";
                            break;

                        case "PendulumSwingIndex":
                            newText = "swing index";
                            break;

                        case "RngCallsPerFrame":
                            newText = GetNumRngCalls(objAddress).ToString();
                            break;
                    }

                    if (specialVar is AngleDataContainer)
                    {
                        var angleContainer = specialVar as AngleDataContainer;
                        if (firstObject)
                        {
                            angleContainer.ValueExists = newAngle.HasValue;
                            if (newAngle.HasValue)
                                angleContainer.AngleValue = newAngle.Value;
                        }

                        newAngle %= Math.PI * 2;
                        if (newAngle < 0)
                            newAngle += Math.PI * 2;

                        // Check when multiple objects have different values
                        angleContainer.ValueExists &= newAngle == angleContainer.AngleValue;
                    }
                    else if (specialVar is DataContainer)
                    {
                        var dataContainer = specialVar as DataContainer;
                        if (firstObject)
                            dataContainer.Text = newText;
                        // Check when multiple objects have different values
                        else if (dataContainer.Text != newText)
                            dataContainer.Text = "";
                    }
                }

                firstObject = false;
            }
        }

        public override void Update(bool updateView)
        {
            if (!updateView)
                return;

            // Determine which object is being held
            uint holdingObj = _stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.HoldingObjectPointerOffset);

            // Change to unclone if we are already holding the object
            if ((_currentAddresses.Contains(holdingObj)) != _unclone)
            {
                _unclone = !_unclone;

                // Update button text
                _cloneButton.Text = _unclone ? "UnClone" : "Clone";
            }

            // Determine load or unload
            bool anyActive = _currentAddresses.Any(address => _stream.GetUInt16(address + Config.ObjectSlots.ObjectActiveOffset) != 0x0000);
            if (anyActive == _revive)
            {
                _revive = !anyActive;

                // Update button text
                _unloadButton.Text = _revive ? "Revive" : "Unload";
            }

            base.Update(updateView);
            ProcessSpecialVars();
        }

        private int GetNumRngCalls(uint objAddress)
        {
            var numberOfRngObjs = _stream.GetUInt32(Config.RngRecordingAreaAddress);

            int numOfCalls = 0;

            for (int i = 0; i < numberOfRngObjs; i++)
            {
                uint rngStructAdd = (uint)(Config.RngRecordingAreaAddress + 0x30 + 0x08 * i);
                var address = _stream.GetUInt32(rngStructAdd + 0x04);
                if (address != objAddress)
                    continue;

                var preRng = _stream.GetUInt16(rngStructAdd + 0x00);
                var postRng = _stream.GetUInt16(rngStructAdd + 0x02);

                numOfCalls = RngIndexer.GetRngIndexDiff(preRng, postRng);
                break;
            }

            return numOfCalls;
        }
    }
}

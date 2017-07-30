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
        public static ObjectManager Instance = null;

        List<IDataContainer> _behaviorDataControls = new List<IDataContainer>();
        ObjectAssociations _objAssoc;

        object _watchVarLocker = new object();

        string _slotIndex;
        string _slotPos;
        string _behavior;
        bool _unrelease = false;
        bool _uninteract = false;
        bool _unclone = false;
        bool _revive = false;

        Button _releaseButton;
        Button _interactButton;
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
                RemoveWatchVariables(_behaviorDataControls);
                _behaviorDataControls.Clear();

                // Add new watchVars
                _behaviorDataControls.AddRange(AddWatchVariables(value, color));
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

                // Pendulum vars
                new DataContainer("PendulumAmplitude"),
                new DataContainer("PendulumSwingIndex"),

                // Racing penguin vars
                new DataContainer("RacingPenguinEffortTarget"),
                new DataContainer("RacingPenguinEffortChange"),
                new DataContainer("RacingPenguinMinHSpeed"),
                new DataContainer("RacingPenguinHSpeedTarget"),

                // Koopa the Quick vars
                new DataContainer("KoopaTheQuickHSpeedTarget"),
                new DataContainer("KoopaTheQuickHSpeedChange"),

                // Hacked vars
                new DataContainer("RngCallsPerFrame"),
            };
        }

        public ObjectManager(ProcessStream stream, ObjectAssociations objAssoc, List<WatchVariable> objectData, Control objectControl, NoTearFlowLayoutPanel variableTable)
            : base(stream, objectData, variableTable)
        {
            Instance = this;

            SplitContainer splitContainerObject = objectControl.Controls["splitContainerObject"] as SplitContainer;

            _objAssoc = objAssoc;
            _objAddressLabelValue = splitContainerObject.Panel1.Controls["labelObjAddValue"] as Label;
            _objAddressLabel = splitContainerObject.Panel1.Controls["labelObjAdd"] as Label;
            _objSlotIndexLabel = splitContainerObject.Panel1.Controls["labelObjSlotIndValue"] as Label;
            _objSlotPositionLabel = splitContainerObject.Panel1.Controls["labelObjSlotPosValue"] as Label;
            _objBehaviorLabel = splitContainerObject.Panel1.Controls["labelObjBhvValue"] as Label;
            _objectNameTextBox = splitContainerObject.Panel1.Controls["textBoxObjName"] as TextBox;
            _objectBorderPanel = splitContainerObject.Panel1.Controls["panelObjectBorder"] as Panel;
            _objectImagePictureBox = _objectBorderPanel.Controls["pictureBoxObject"] as IntPictureBox;

            _objAddressLabelValue.Click += ObjAddressLabel_Click;
            _objAddressLabel.Click += ObjAddressLabel_Click;

            Panel objPanel = splitContainerObject.Panel1.Controls["panelObj"] as Panel;

            var goToButton = objPanel.Controls["buttonObjGoTo"] as Button;
            goToButton.Click += (sender, e) => ButtonUtilities.GoToObjects(_stream, _currentAddresses);

            var retrieveButton = objPanel.Controls["buttonObjRetrieve"] as Button;
            retrieveButton.Click += (sender, e) => ButtonUtilities.RetrieveObjects(_stream, _currentAddresses);

            var goToHomeButton = objPanel.Controls["buttonObjGoToHome"] as Button;
            goToHomeButton.Click += (sender, e) => ButtonUtilities.GoToObjectsHome(_stream, _currentAddresses);

            var retrieveHomeButton = objPanel.Controls["buttonObjRetrieveHome"] as Button;
            retrieveHomeButton.Click += (sender, e) => ButtonUtilities.RetrieveObjectsHome(_stream, _currentAddresses);

            _releaseButton = objPanel.Controls["buttonObjRelease"] as Button;
            _releaseButton.Click += (sender, e) =>
            {
                if (_unrelease)
                    ButtonUtilities.UnReleaseObject(_stream, _currentAddresses);
                else
                    ButtonUtilities.ReleaseObject(_stream, _currentAddresses);
            };

            _interactButton = objPanel.Controls["buttonObjInteract"] as Button;
            _interactButton.Click += (sender, e) =>
            {
                if (_uninteract)
                    ButtonUtilities.UnInteractObject(_stream, _currentAddresses);
                else
                    ButtonUtilities.InteractObject(_stream, _currentAddresses);
            };

            _cloneButton = objPanel.Controls["buttonObjClone"] as Button;
            _cloneButton.Click += (sender, e) =>
            {
                if (CurrentAddresses.Count == 0)
                    return;

                if (_unclone)
                    ButtonUtilities.UnCloneObject(_stream, CurrentAddresses[0]);
                else
                    ButtonUtilities.CloneObject(_stream, CurrentAddresses[0]);
            };

            _unloadButton = objPanel.Controls["buttonObjUnload"] as Button;
            _unloadButton.Click += (sender, e) =>
            {
                if (_revive)
                    ButtonUtilities.ReviveObject(_stream, CurrentAddresses);
                else
                    ButtonUtilities.UnloadObject(_stream, CurrentAddresses);
            };

            var objPosGroupBox = objPanel.Controls["groupBoxObjPos"] as GroupBox;
            ThreeDimensionController.initialize(
                CoordinateSystem.Euler,
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
                (float hOffset, float vOffset, float nOffset, bool useRelative) =>
                {
                    ButtonUtilities.TranslateObjects(
                        _stream,
                        _currentAddresses,
                        hOffset,
                        nOffset,
                        -1 * vOffset,
                        useRelative);
                });

            var objAngleGroupBox = objPanel.Controls["groupBoxObjAngle"] as GroupBox;
            ScalarController.initialize(
                objAngleGroupBox.Controls["buttonObjAngleYawN"] as Button,
                objAngleGroupBox.Controls["buttonObjAngleYawP"] as Button,
                objAngleGroupBox.Controls["textBoxObjAngleYaw"] as TextBox,
                (float yawValue) =>
                {
                    ButtonUtilities.RotateObjects(stream, _currentAddresses, (int)Math.Round(yawValue), 0, 0);
                });
            ScalarController.initialize(
                objAngleGroupBox.Controls["buttonObjAnglePitchN"] as Button,
                objAngleGroupBox.Controls["buttonObjAnglePitchP"] as Button,
                objAngleGroupBox.Controls["textBoxObjAnglePitch"] as TextBox,
                (float pitchValue) =>
                {
                    ButtonUtilities.RotateObjects(stream, _currentAddresses, 0, (int)Math.Round(pitchValue), 0);
                });
            ScalarController.initialize(
                objAngleGroupBox.Controls["buttonObjAngleRollN"] as Button,
                objAngleGroupBox.Controls["buttonObjAngleRollP"] as Button,
                objAngleGroupBox.Controls["textBoxObjAngleRoll"] as TextBox,
                (float rollValue) =>
                {
                    ButtonUtilities.RotateObjects(stream, _currentAddresses, 0, 0, (int)Math.Round(rollValue));
                });

            var objScaleGroupBox = objPanel.Controls["groupBoxObjScale"] as GroupBox;
            ScaleController.initialize(
                objScaleGroupBox.Controls["buttonObjScaleWidthN"] as Button,
                objScaleGroupBox.Controls["buttonObjScaleWidthP"] as Button,
                objScaleGroupBox.Controls["buttonObjScaleHeightN"] as Button,
                objScaleGroupBox.Controls["buttonObjScaleHeightP"] as Button,
                objScaleGroupBox.Controls["buttonObjScaleDepthN"] as Button,
                objScaleGroupBox.Controls["buttonObjScaleDepthP"] as Button,
                objScaleGroupBox.Controls["buttonObjScaleAggregateN"] as Button,
                objScaleGroupBox.Controls["buttonObjScaleAggregateP"] as Button,
                objScaleGroupBox.Controls["textBoxObjScaleWidth"] as TextBox,
                objScaleGroupBox.Controls["textBoxObjScaleHeight"] as TextBox,
                objScaleGroupBox.Controls["textBoxObjScaleDepth"] as TextBox,
                objScaleGroupBox.Controls["textBoxObjScaleAggregate"] as TextBox,
                objScaleGroupBox.Controls["checkBoxObjScaleAggregate"] as CheckBox,
                objScaleGroupBox.Controls["checkBoxObjScaleMultiply"] as CheckBox,
                (float widthChange, float heightChange, float depthChange, bool multiply) =>
                {
                    ButtonUtilities.ScaleObjects(stream, _currentAddresses, widthChange, heightChange, depthChange, multiply);
                });

            var objHomeGroupBox = objPanel.Controls["groupBoxObjHome"] as GroupBox;
            ThreeDimensionController.initialize(
                CoordinateSystem.Euler,
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
                (float hOffset, float vOffset, float nOffset, bool useRelative) =>
                {
                    ButtonUtilities.TranslateObjectHomes(
                        _stream,
                        _currentAddresses,
                        hOffset,
                        nOffset,
                        -1 * vOffset,
                        useRelative);
                });
        }

        private void AddressChanged()
        {
            var test = _dataControls.Where(d => d is WatchVariableControl);
            foreach (WatchVariableControl dataControl in test)
                dataControl.EditMode = false;

            if (CurrentAddresses.Count <= 1)
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

                // Get pendulum variables
                float pendulumAccelerationDirection = _stream.GetSingle(objAddress + Config.ObjectSlots.PendulumAccelerationDirection);
                float pendulumAccelerationMagnitude = _stream.GetSingle(objAddress + Config.ObjectSlots.PendulumAccelerationMagnitude);
                float pendulumAngularVelocity = _stream.GetSingle(objAddress + Config.ObjectSlots.PendulumAngularVelocity);
                float pendulumAngle = _stream.GetSingle(objAddress + Config.ObjectSlots.PendulumAngle);
                float pendulumAmplitude;

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
                            pendulumAmplitude =
                                MoreMath.getPendulumAmplitude(
                                    pendulumAccelerationDirection,
                                    pendulumAccelerationMagnitude,
                                    pendulumAngularVelocity,
                                    pendulumAngle);
                            newText = pendulumAmplitude.ToString();
                            break;

                        case "PendulumSwingIndex":
                            pendulumAmplitude =
                                MoreMath.getPendulumAmplitude(
                                    pendulumAccelerationDirection,
                                    pendulumAccelerationMagnitude,
                                    pendulumAngularVelocity,
                                    pendulumAngle);
                            int? pendulumSwingIndex = Config.PendulumSwings.GetPendulumSwingIndex((int)pendulumAmplitude);
                            newText = pendulumSwingIndex == null ? "Unknown Index" : pendulumSwingIndex.ToString();
                            break;

                        case "RacingPenguinEffortTarget":
                            {
                                (double temp, _, _, _) = MoreMath.GetRacingPenguinSpecialVars(_stream, objAddress);
                                newText = Math.Round(temp, 3).ToString();
                                break;
                            }

                        case "RacingPenguinEffortChange":
                            {
                                (_, double temp, _, _) = MoreMath.GetRacingPenguinSpecialVars(_stream, objAddress);
                                newText = Math.Round(temp, 3).ToString();
                                break;
                            }

                        case "RacingPenguinMinHSpeed":
                            {
                                (_, _, double temp, _) = MoreMath.GetRacingPenguinSpecialVars(_stream, objAddress);
                                newText = Math.Round(temp, 3).ToString();
                                break;
                            }

                        case "RacingPenguinHSpeedTarget":
                            {
                                (_, _, _, double temp) = MoreMath.GetRacingPenguinSpecialVars(_stream, objAddress);
                                newText = Math.Round(temp, 3).ToString();
                                break;
                            }

                        case "KoopaTheQuickHSpeedTarget":
                            {
                                (double temp, _) = MoreMath.GetKoopaTheQuickSpecialVars(_stream, objAddress);
                                newText = Math.Round(temp, 3).ToString();
                                break;
                            }

                        case "KoopaTheQuickHSpeedChange":
                            {
                                (_, double temp) = MoreMath.GetKoopaTheQuickSpecialVars(_stream, objAddress);
                                newText = Math.Round(temp, 3).ToString();
                                break;
                            }

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

            if (_currentAddresses.Count == 0)
            {
                foreach (IDataContainer specialVar in _specialWatchVars)
                {
                    if (specialVar is AngleDataContainer)
                    {
                        var angleContainer = specialVar as AngleDataContainer;
                        angleContainer.ValueExists = false;
                    }
                    else if (specialVar is DataContainer)
                    {
                        var dataContainer = specialVar as DataContainer;
                        dataContainer.Text = "";
                    }
                }
            }
        }

        public override void Update(bool updateView)
        {
            if (!updateView)
                return;

            // Determine which object is being held
            uint heldObj = _stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.HeldObjectPointerOffset);

            // Change to unclone if we are already holding the object
            if ((_currentAddresses.Contains(heldObj)) != _unclone)
            {
                _unclone = !_unclone;

                // Update button text
                _cloneButton.Text = _unclone ? "UnClone" : "Clone";
            }

            // Determine load or unload
            bool revive = _currentAddresses.Count > 0 && _currentAddresses.All(address => _stream.GetUInt16(address + Config.ObjectSlots.ObjectActiveOffset) == 0x0000);
            if (_revive != revive)
            {
                _revive = revive;

                // Update button text
                _unloadButton.Text = _revive ? "Revive" : "Unload";
            }

            // Determine release or unrelease
            bool unrelease = _currentAddresses.Count > 0 && _currentAddresses.All(address => _stream.GetUInt32(address + Config.ObjectSlots.ReleaseStatusOffset) == Config.ObjectSlots.ReleaseStatusReleasedValue);
            if (_unrelease != unrelease)
            {
                _unrelease = unrelease;

                // Update button text
                _releaseButton.Text = _unrelease ? "UnRelease" : "Release";
            }

            // Determine interact or uninteract
            bool uninteract = _currentAddresses.Count > 0 && _currentAddresses.All(address => _stream.GetUInt32(address + Config.ObjectSlots.InteractionStatusOffset) != 0);
            if (_uninteract != uninteract)
            {
                _uninteract = uninteract;

                // Update button text
                _interactButton.Text = _uninteract ? "UnInteract" : "Interact";
            }

            base.Update(updateView);
            ProcessSpecialVars();
        }

        private int GetNumRngCalls(uint objAddress)
        {
            var numberOfRngObjs = _stream.GetUInt32(Config.HackedAreaAddress);

            int numOfCalls = 0;

            for (int i = 0; i < numberOfRngObjs; i++)
            {
                uint rngStructAdd = (uint)(Config.HackedAreaAddress + 0x30 + 0x08 * i);
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

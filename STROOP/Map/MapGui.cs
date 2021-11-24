using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using STROOP.Controls;

namespace STROOP.Map
{
    public class MapGui
    {
        // Major Controls
        public GLControl GLControlMap2D;
        public GLControl GLControlMap3D;
        public MapTrackerFlowLayoutPanel flowLayoutPanelMapTrackers;
        public SplitContainer splitContainerMap;

        public GLControl CurrentControl;

        // Options - CheckBoxes
        public CheckBox checkBoxMapOptionsTrackMario;
        public CheckBox checkBoxMapOptionsTrackHolp;
        public CheckBox checkBoxMapOptionsTrackCamera;
        public CheckBox checkBoxMapOptionsTrackCameraFocus;
        public CheckBox checkBoxMapOptionsTrackGhost;
        public CheckBox checkBoxMapOptionsTrackSelf;
        public CheckBox checkBoxMapOptionsTrackPoint;
        public CheckBox checkBoxMapOptionsTrackFloorTri;
        public CheckBox checkBoxMapOptionsTrackWallTri;
        public CheckBox checkBoxMapOptionsTrackCeilingTri;
        public CheckBox checkBoxMapOptionsTrackUnitGridlines;

        public CheckBox checkBoxMapOptionsEnable3D;
        public CheckBox checkBoxMapOptionsDisableHitboxHackTris;
        public CheckBox checkBoxMapOptionsEnableOrthographicView;
        public CheckBox checkBoxMapOptionsEnableCrossSection;
        public CheckBox checkBoxMapOptionsEnablePuView;
        public CheckBox checkBoxMapOptionsReverseDragging;
        public CheckBox checkBoxMapOptionsSelectionMode;
        public CheckBox checkBoxMapOptionsObjectDrag;

        // Options - Global Icon Size
        public Label labelMapOptionsGlobalIconSize;
        public BetterTextbox textBoxMapOptionsGlobalIconSize;
        public TrackBarEx trackBarMapOptionsGlobalIconSize;

        // Options - Buttons
        public Button buttonMapOptionsAddNewTracker;
        public Button buttonMapOptionsClearAllTrackers;
        public Button buttonMapOptionsOpen;
        public Button buttonMapOptionsSave;

        // Options - ComboBoxes
        public ComboBox comboBoxMapOptionsMap;
        public ComboBox comboBoxMapOptionsBackground;



        // Controllers - GroupBoxes
        public GroupBox groupBoxMapControllersScale;
        public GroupBox groupBoxMapControllersCenter;
        public GroupBox groupBoxMapControllersAngle;

        // Controllers - Scale - Left
        public RadioButton radioButtonMapControllersScaleCourseDefault;
        public RadioButton radioButtonMapControllersScaleMaxCourseSize;
        public RadioButton radioButtonMapControllersScaleCustom;
        public BetterTextbox textBoxMapControllersScaleCustom;

        // Controllers - Scale - Right
        public BetterTextbox textBoxMapControllersScaleChange;
        public Button buttonMapControllersScaleMinus;
        public Button buttonMapControllersScalePlus;
        public BetterTextbox textBoxMapControllersScaleChange2;
        public Button buttonMapControllersScaleDivide;
        public Button buttonMapControllersScaleTimes;

        // Controllers - Center - Left
        public RadioButton radioButtonMapControllersCenterBestFit;
        public RadioButton radioButtonMapControllersCenterOrigin;
        public RadioButton radioButtonMapControllersCenterMario;
        public RadioButton radioButtonMapControllersCenterCustom;
        public BetterTextbox textBoxMapControllersCenterCustom;

        // Controllers - Center - Right
        public CheckBox checkBoxMapControllersCenterChangeByPixels;
        public CheckBox checkBoxMapControllersCenterUseMarioDepth;
        public BetterTextbox textBoxMapControllersCenterChange;
        public Button buttonMapControllersCenterUp;
        public Button buttonMapControllersCenterUpRight;
        public Button buttonMapControllersCenterRight;
        public Button buttonMapControllersCenterDownRight;
        public Button buttonMapControllersCenterDown;
        public Button buttonMapControllersCenterDownLeft;
        public Button buttonMapControllersCenterLeft;
        public Button buttonMapControllersCenterUpLeft;
        public Button buttonMapControllersCenterIn;
        public Button buttonMapControllersCenterOut;





        // Controllers - Angle - Left
        public RadioButton radioButtonMapControllersAngle0;
        public RadioButton radioButtonMapControllersAngle16384;
        public RadioButton radioButtonMapControllersAngle32768;
        public RadioButton radioButtonMapControllersAngle49152;
        public RadioButton radioButtonMapControllersAngleMario;
        public RadioButton radioButtonMapControllersAngleMarioSide;
        public RadioButton radioButtonMapControllersAngleCamera;
        public RadioButton radioButtonMapControllersAngleCentripetal;
        public RadioButton radioButtonMapControllersAngleCustom;
        public BetterTextbox textBoxMapControllersAngleCustom;

        // Controllers - Angle - Right
        public BetterTextbox textBoxMapControllersAngleChange;
        public Button buttonMapControllersAngleCCW;
        public Button buttonMapControllersAngleCW;
        public Button buttonMapControllersAngleUp;
        public Button buttonMapControllersAngleDown;



        // Data
        public Label labelMapDataMapName;
        public Label labelMapDataMapSubName;
        public Label labelMapDataPuCoordinateValues;
        public Label labelMapDataQpuCoordinateValues;
        public Label labelMapDataIdValues;
        public Label labelMapDataYNormValue;



        // 3D Vars
        public WatchVariableFlowLayoutPanel watchVariablePanelMapVars;

        // 3D GroupBoxes
        public GroupBox groupBoxMapCameraPosition;
        public GroupBox groupBoxMapFocusPosition;
        public GroupBox groupBoxMapCameraSpherical;
        public GroupBox groupBoxMapFocusSpherical;
        public GroupBox groupBoxMapCameraFocus;

        // FOV
        public BetterTextbox textBoxMapFov;
        public TrackBarEx trackBarMapFov;
    }
}

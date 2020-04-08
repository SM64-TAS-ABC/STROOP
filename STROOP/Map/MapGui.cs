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



        // Options - CheckBoxes
        public CheckBox checkBoxMapOptionsTrackMario;
        public CheckBox checkBoxMapOptionsTrackHolp;
        public CheckBox checkBoxMapOptionsTrackCamera;
        public CheckBox checkBoxMapOptionsTrackFloorTri;
        public CheckBox checkBoxMapOptionsTrackCeilingTri;
        public CheckBox checkBoxMapOptionsTrackCellGridlines;
        public CheckBox checkBoxMapOptionsTrackCurrentCell;
        public CheckBox checkBoxMapOptionsTrackUnitGridlines;
        public CheckBox checkBoxMapOptionsTrackCurrentUnit;
        public CheckBox checkBoxMapOptionsTrackNextPositions;
        public CheckBox checkBoxMapOptionsTrackSelf;
        public CheckBox checkBoxMapOptionsTrackPoint;
        public CheckBox checkBoxMapOptionsEnable3D;
        public CheckBox checkBoxMapOptionsDisable3DHitboxHackTris;
        public CheckBox checkBoxMapOptionsEnablePuView;
        public CheckBox checkBoxMapOptionsReverseMapControls;
        public CheckBox checkBoxMapOptionsScaleIconSizes;

        // Options - Global Icon Size
        public Label labelMapOptionsGlobalIconSize;
        public BetterTextbox textBoxMapOptionsGlobalIconSize;
        public TrackBarEx trackBarMapOptionsGlobalIconSize;

        // Options - Buttons
        public Button buttonMapOptionsAddNewTracker;
        public Button buttonMapOptionsClearAllTrackers;

        // Options - ComboBoxes
        public ComboBox comboBoxMapOptionsLevel;
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
        public BetterTextbox textBoxMapControllersCenterChange;
        public Button buttonMapControllersCenterUp;
        public Button buttonMapControllersCenterUpRight;
        public Button buttonMapControllersCenterRight;
        public Button buttonMapControllersCenterDownRight;
        public Button buttonMapControllersCenterDown;
        public Button buttonMapControllersCenterDownLeft;
        public Button buttonMapControllersCenterLeft;
        public Button buttonMapControllersCenterUpLeft;





        // Controllers - Angle - Left
        public RadioButton radioButtonMapControllersAngle0;
        public RadioButton radioButtonMapControllersAngle16384;
        public RadioButton radioButtonMapControllersAngle32768;
        public RadioButton radioButtonMapControllersAngle49152;
        public RadioButton radioButtonMapControllersAngleMario;
        public RadioButton radioButtonMapControllersAngleCamera;
        public RadioButton radioButtonMapControllersAngleCentripetal;
        public RadioButton radioButtonMapControllersAngleCustom;
        public BetterTextbox textBoxMapControllersAngleCustom;

        // Controllers - Angle - Right
        public BetterTextbox textBoxMapControllersAngleChange;
        public Button buttonMapControllersAngleCCW;
        public Button buttonMapControllersAngleCW;



        // Data
        public Label labelMapDataMapName;
        public Label labelMapDataMapSubName;
        public Label labelMapDataPuCoordinateValues;
        public Label labelMapDataQpuCoordinateValues;
        public Label labelMapDataId;
        public Label labelMapDataYNorm;



        // 3D Vars
        public WatchVariableFlowLayoutPanel watchVariablePanelMap3DVars;

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

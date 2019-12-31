using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using STROOP.Controls;

namespace STROOP.Map3
{
    public class MapGui
    {
        // Major Controls
        public GLControl GLControl2D;
        public GLControl GLControl3D;
        public MapTrackerFlowLayoutPanel flowLayoutPanelMap3Trackers;



        // Options - CheckBoxes
        public CheckBox checkBoxMap3OptionsTrackMario;
        public CheckBox checkBoxMap3OptionsTrackHolp;
        public CheckBox checkBoxMap3OptionsTrackCamera;
        public CheckBox checkBoxMap3OptionsTrackFloorTri;
        public CheckBox checkBoxMap3OptionsTrackCeilingTri;
        public CheckBox checkBoxMap3OptionsTrackCellGridlines;
        public CheckBox checkBoxMap3OptionsTrackCurrentCell;
        public CheckBox checkBoxMap3OptionsTrackUnitGridlines;
        public CheckBox checkBoxMap3OptionsTrackCurrentUnit;
        public CheckBox checkBoxMap3OptionsTrackNextPositions;
        public CheckBox checkBoxMap3OptionsTrackSelf;
        public CheckBox checkBoxMap3OptionsTrackPoint;
        public CheckBox checkBoxMap3OptionsEnable3D;
        public CheckBox checkBoxMap3OptionsDisable3DHitboxHackTris;
        public CheckBox checkBoxMap3OptionsEnablePuView;
        public CheckBox checkBoxMap3OptionsScaleIconSizes;

        // Options - Global Icon Size
        public Label labelMap3OptionsGlobalIconSize;
        public BetterTextbox textBoxMap3OptionsGlobalIconSize;
        public TrackBarEx trackBarMap3OptionsGlobalIconSize;

        // Options - Buttons
        public Button buttonMap3OptionsAddNewTracker;
        public Button buttonMap3OptionsClearAllTrackers;
        public Button buttonMap3OptionsTrackAllObjects;
        public Button buttonMap3OptionsTrackMarkedObjects;

        // Options - ComboBoxes
        public ComboBox comboBoxMap3OptionsLevel;
        public ComboBox comboBoxMap3OptionsBackground;



        // Controllers - GroupBoxes
        public GroupBox groupBoxMap3ControllersScale;
        public GroupBox groupBoxMap3ControllersCenter;
        public GroupBox groupBoxMap3ControllersAngle;

        // Controllers - Scale - Left
        public RadioButton radioButtonMap3ControllersScaleCourseDefault;
        public RadioButton radioButtonMap3ControllersScaleMaxCourseSize;
        public RadioButton radioButtonMap3ControllersScaleCustom;
        public BetterTextbox textBoxMap3ControllersScaleCustom;

        // Controllers - Scale - Right
        public BetterTextbox textBoxMap3ControllersScaleChange;
        public Button buttonMap3ControllersScaleMinus;
        public Button buttonMap3ControllersScalePlus;
        public BetterTextbox textBoxMap3ControllersScaleChange2;
        public Button buttonMap3ControllersScaleDivide;
        public Button buttonMap3ControllersScaleTimes;

        // Controllers - Center - Left
        public RadioButton radioButtonMap3ControllersCenterBestFit;
        public RadioButton radioButtonMap3ControllersCenterOrigin;
        public RadioButton radioButtonMap3ControllersCenterMario;
        public RadioButton radioButtonMap3ControllersCenterCustom;
        public BetterTextbox textBoxMap3ControllersCenterCustom;

        // Controllers - Center - Right
        public CheckBox checkBoxMap3ControllersCenterChangeByPixels;
        public BetterTextbox textBoxMap3ControllersCenterChange;
        public Button buttonMap3ControllersCenterUp;
        public Button buttonMap3ControllersCenterUpRight;
        public Button buttonMap3ControllersCenterRight;
        public Button buttonMap3ControllersCenterDownRight;
        public Button buttonMap3ControllersCenterDown;
        public Button buttonMap3ControllersCenterDownLeft;
        public Button buttonMap3ControllersCenterLeft;
        public Button buttonMap3ControllersCenterUpLeft;





        // Controllers - Angle - Left
        public RadioButton radioButtonMap3ControllersAngle0;
        public RadioButton radioButtonMap3ControllersAngle16384;
        public RadioButton radioButtonMap3ControllersAngle32768;
        public RadioButton radioButtonMap3ControllersAngle49152;
        public RadioButton radioButtonMap3ControllersAngleMario;
        public RadioButton radioButtonMap3ControllersAngleCamera;
        public RadioButton radioButtonMap3ControllersAngleCentripetal;
        public RadioButton radioButtonMap3ControllersAngleCustom;
        public BetterTextbox textBoxMap3ControllersAngleCustom;

        // Controllers - Angle - Right
        public BetterTextbox textBoxMap3ControllersAngleChange;
        public Button buttonMap3ControllersAngleCCW;
        public Button buttonMap3ControllersAngleCW;



        // Data
        public Label labelMap3DataMapName;
        public Label labelMap3DataMapSubName;
        public Label labelMap3DataPuCoordinateValues;
        public Label labelMap3DataQpuCoordinateValues;
        public Label labelMap3DataId;
        public Label labelMap3DataYNorm;



        // 3D Vars
        public WatchVariableFlowLayoutPanel watchVariablePanel3DVars;

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

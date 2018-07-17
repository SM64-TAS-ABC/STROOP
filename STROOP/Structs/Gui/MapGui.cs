using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using STROOP.Controls;
using STROOP.Controls.Map.Trackers;

namespace STROOP.Structs
{
    public class MapGui
    {
        // Main controls
        public GLControl GLControl;
        public MapTrackerFlowLayoutPanel MapTrackerFlowLayoutPanel;

        public TabControl TabControlView;

        // Controls in options tab
        public TabPage TabPageOptions;
        public CheckBox CheckBoxTrackMario;
        public CheckBox CheckBoxTrackHolp;
        public CheckBox CheckBoxTrackCamera;
        public CheckBox CheckBoxTrackFloorTriangle;
        public CheckBox CheckBoxTrackWallTriangle;
        public CheckBox CheckBoxTrackCeilingTriangle;
        public CheckBox CheckBoxTrackAllObjects;
        public CheckBox CheckBoxTrackGridlines;

        public Button ButtonAddNewTracker;
        public Button ButtonClearAllTrackers;
        public Button ButtonTrackSelectedObjects;

        public ComboBox ComboBoxLevel;
        public ComboBox ComboBoxBackground;

        // Controls in 2D tab
        public TabPage TabPage2D;
        public RadioButton RadioButtonScaleCourseDefault;
        public RadioButton RadioButtonScaleMaxCourseSize;
        public RadioButton RadioButtonScaleCustom;
        public BetterTextbox TextBoxScaleCustom;

        public BetterTextbox TextBoxScaleChange;
        public Button ButtonCenterScaleChangeMinus;
        public Button ButtonCenterScaleChangePlus;

        public RadioButton RadioButtonCenterBestFit;
        public RadioButton RadioButtonCenterOrigin;
        public RadioButton RadioButtonCenterCustom;
        public BetterTextbox TextBoxCenterCustom;

        public BetterTextbox TextBoxCenterChange;
        public Button ButtonCenterChangeUp;
        public Button ButtonCenterChangeDown;
        public Button ButtonCenterChangeLeft;
        public Button ButtonCenterChangeRight;
        public Button ButtonCenterChangeUpLeft;
        public Button ButtonCenterChangeUpRight;
        public Button ButtonCenterChangeDownLeft;
        public Button ButtonCenterChangeDownRight;

        public RadioButton RadioButtonAngle0;
        public RadioButton RadioButtonAngle16384;
        public RadioButton RadioButtonAngle32768;
        public RadioButton RadioButtonAngle49152;
        public RadioButton RadioButtonAngleCustom;
        public BetterTextbox TextBoxAngleCustom;

        public BetterTextbox TextBoxAngleChange;
        public Button ButtonAngleChangeCounterclockwise;
        public Button ButtonAngleChangeClockwise;

        // Controls in the 3D tab
        public TabPage TabPage3D;
        public CheckBox CheckBoxMapGameCamOrientation;
        public ComboBox ComboBoxMapColorMethod;
    }
}

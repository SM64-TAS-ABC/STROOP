using SM64_Diagnostic.Controls;
using System.Windows.Forms;

namespace SM64_Diagnostic
{
    partial class StroopMainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StroopMainForm));
            this.labelProcessSelect = new System.Windows.Forms.Label();
            this.groupBoxObjects = new System.Windows.Forms.GroupBox();
            this.comboBoxLabelMethod = new System.Windows.Forms.ComboBox();
            this.labelLabelMethod = new System.Windows.Forms.Label();
            this.labelSlotSize = new System.Windows.Forms.Label();
            this.labelToggleMode = new System.Windows.Forms.Label();
            this.comboBoxMapToggleMode = new System.Windows.Forms.ComboBox();
            this.checkBoxObjLockLabels = new System.Windows.Forms.CheckBox();
            this.labelSortMethod = new System.Windows.Forms.Label();
            this.comboBoxSortMethod = new System.Windows.Forms.ComboBox();
            this.trackBarObjSlotSize = new System.Windows.Forms.TrackBar();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageObjects = new System.Windows.Forms.TabPage();
            this.splitContainerObject = new System.Windows.Forms.SplitContainer();
            this.panelObj = new System.Windows.Forms.Panel();
            this.buttonObjRelease = new System.Windows.Forms.Button();
            this.buttonObjInteract = new System.Windows.Forms.Button();
            this.groupBoxObjHome = new System.Windows.Forms.GroupBox();
            this.checkBoxObjHomeRelative = new System.Windows.Forms.CheckBox();
            this.textBoxObjHomeY = new System.Windows.Forms.TextBox();
            this.buttonObjHomeYp = new System.Windows.Forms.Button();
            this.buttonObjHomeYn = new System.Windows.Forms.Button();
            this.buttonObjHomeXpZp = new System.Windows.Forms.Button();
            this.textBoxObjHomeXZ = new System.Windows.Forms.TextBox();
            this.buttonObjHomeXp = new System.Windows.Forms.Button();
            this.buttonObjHomeXpZn = new System.Windows.Forms.Button();
            this.buttonObjHomeZn = new System.Windows.Forms.Button();
            this.buttonObjHomeZp = new System.Windows.Forms.Button();
            this.buttonObjHomeXnZp = new System.Windows.Forms.Button();
            this.buttonObjHomeXn = new System.Windows.Forms.Button();
            this.buttonObjHomeXnZn = new System.Windows.Forms.Button();
            this.groupBoxObjScale = new System.Windows.Forms.GroupBox();
            this.checkBoxObjScaleMultiply = new System.Windows.Forms.CheckBox();
            this.checkBoxObjScaleAggregate = new System.Windows.Forms.CheckBox();
            this.textBoxObjScaleDepth = new System.Windows.Forms.TextBox();
            this.textBoxObjScaleHeight = new System.Windows.Forms.TextBox();
            this.textBoxObjScaleWidth = new System.Windows.Forms.TextBox();
            this.buttonObjScaleDepthN = new System.Windows.Forms.Button();
            this.buttonObjScaleHeightN = new System.Windows.Forms.Button();
            this.buttonObjScaleWidthN = new System.Windows.Forms.Button();
            this.buttonObjScaleDepthP = new System.Windows.Forms.Button();
            this.buttonObjScaleHeightP = new System.Windows.Forms.Button();
            this.buttonObjScaleWidthP = new System.Windows.Forms.Button();
            this.textBoxObjScaleAggregate = new System.Windows.Forms.TextBox();
            this.buttonObjScaleAggregateN = new System.Windows.Forms.Button();
            this.buttonObjScaleAggregateP = new System.Windows.Forms.Button();
            this.groupBoxObjAngle = new System.Windows.Forms.GroupBox();
            this.textBoxObjAngleRoll = new System.Windows.Forms.TextBox();
            this.textBoxObjAnglePitch = new System.Windows.Forms.TextBox();
            this.textBoxObjAngleYaw = new System.Windows.Forms.TextBox();
            this.buttonObjAngleRollN = new System.Windows.Forms.Button();
            this.buttonObjAnglePitchN = new System.Windows.Forms.Button();
            this.buttonObjAngleYawN = new System.Windows.Forms.Button();
            this.buttonObjAngleRollP = new System.Windows.Forms.Button();
            this.buttonObjAnglePitchP = new System.Windows.Forms.Button();
            this.buttonObjAngleYawP = new System.Windows.Forms.Button();
            this.groupBoxObjPos = new System.Windows.Forms.GroupBox();
            this.checkBoxObjPosRelative = new System.Windows.Forms.CheckBox();
            this.textBoxObjPosY = new System.Windows.Forms.TextBox();
            this.buttonObjPosYp = new System.Windows.Forms.Button();
            this.buttonObjPosYn = new System.Windows.Forms.Button();
            this.buttonObjPosXpZp = new System.Windows.Forms.Button();
            this.textBoxObjPosXZ = new System.Windows.Forms.TextBox();
            this.buttonObjPosXp = new System.Windows.Forms.Button();
            this.buttonObjPosXpZn = new System.Windows.Forms.Button();
            this.buttonObjPosZn = new System.Windows.Forms.Button();
            this.buttonObjPosZp = new System.Windows.Forms.Button();
            this.buttonObjPosXnZp = new System.Windows.Forms.Button();
            this.buttonObjPosXn = new System.Windows.Forms.Button();
            this.buttonObjPosXnZn = new System.Windows.Forms.Button();
            this.buttonObjGoToHome = new System.Windows.Forms.Button();
            this.buttonObjRetrieve = new System.Windows.Forms.Button();
            this.buttonObjRetrieveHome = new System.Windows.Forms.Button();
            this.buttonObjGoTo = new System.Windows.Forms.Button();
            this.buttonObjClone = new System.Windows.Forms.Button();
            this.buttonObjUnload = new System.Windows.Forms.Button();
            this.textBoxObjName = new System.Windows.Forms.TextBox();
            this.labelObjAddValue = new System.Windows.Forms.Label();
            this.labelObjSlotIndValue = new System.Windows.Forms.Label();
            this.panelObjectBorder = new System.Windows.Forms.Panel();
            this.labelObjSlotPosValue = new System.Windows.Forms.Label();
            this.labelObjBhv = new System.Windows.Forms.Label();
            this.labelObjBhvValue = new System.Windows.Forms.Label();
            this.labelObjSlotPos = new System.Windows.Forms.Label();
            this.labelObjAdd = new System.Windows.Forms.Label();
            this.labelObjSlotInd = new System.Windows.Forms.Label();
            this.tabPageMario = new System.Windows.Forms.TabPage();
            this.splitContainerMario = new System.Windows.Forms.SplitContainer();
            this.groupBoxMarioStats = new System.Windows.Forms.GroupBox();
            this.textBoxMarioStatsVspd = new System.Windows.Forms.TextBox();
            this.textBoxMarioStatsHspd = new System.Windows.Forms.TextBox();
            this.textBoxMarioStatsYaw = new System.Windows.Forms.TextBox();
            this.buttonMarioStatsVspdN = new System.Windows.Forms.Button();
            this.buttonMarioStatsHspdN = new System.Windows.Forms.Button();
            this.buttonMarioStatsYawN = new System.Windows.Forms.Button();
            this.buttonMarioStatsVspdP = new System.Windows.Forms.Button();
            this.buttonMarioStatsHspdP = new System.Windows.Forms.Button();
            this.buttonMarioStatsYawP = new System.Windows.Forms.Button();
            this.groupBoxMarioHOLP = new System.Windows.Forms.GroupBox();
            this.checkBoxMarioHOLPRelative = new System.Windows.Forms.CheckBox();
            this.textBoxMarioHOLPY = new System.Windows.Forms.TextBox();
            this.buttonMarioHOLPYp = new System.Windows.Forms.Button();
            this.buttonMarioHOLPYn = new System.Windows.Forms.Button();
            this.buttonMarioHOLPXpZp = new System.Windows.Forms.Button();
            this.textBoxMarioHOLPXZ = new System.Windows.Forms.TextBox();
            this.buttonMarioHOLPXp = new System.Windows.Forms.Button();
            this.buttonMarioHOLPXpZn = new System.Windows.Forms.Button();
            this.buttonMarioHOLPZn = new System.Windows.Forms.Button();
            this.buttonMarioHOLPZp = new System.Windows.Forms.Button();
            this.buttonMarioHOLPXnZp = new System.Windows.Forms.Button();
            this.buttonMarioHOLPXn = new System.Windows.Forms.Button();
            this.buttonMarioHOLPXnZn = new System.Windows.Forms.Button();
            this.panelMarioBorder = new System.Windows.Forms.Panel();
            this.groupBoxMarioPos = new System.Windows.Forms.GroupBox();
            this.checkBoxMarioPosRelative = new System.Windows.Forms.CheckBox();
            this.textBoxMarioPosY = new System.Windows.Forms.TextBox();
            this.buttonMarioPosYp = new System.Windows.Forms.Button();
            this.buttonMarioPosYn = new System.Windows.Forms.Button();
            this.buttonMarioPosXpZp = new System.Windows.Forms.Button();
            this.textBoxMarioPosXZ = new System.Windows.Forms.TextBox();
            this.buttonMarioPosXp = new System.Windows.Forms.Button();
            this.buttonMarioPosXpZn = new System.Windows.Forms.Button();
            this.buttonMarioPosZn = new System.Windows.Forms.Button();
            this.buttonMarioPosZp = new System.Windows.Forms.Button();
            this.buttonMarioPosXnZp = new System.Windows.Forms.Button();
            this.buttonMarioPosXn = new System.Windows.Forms.Button();
            this.buttonMarioPosXnZn = new System.Windows.Forms.Button();
            this.buttonMarioToggleHandsfree = new System.Windows.Forms.Button();
            this.buttonMarioVisibility = new System.Windows.Forms.Button();
            this.tabPageActions = new System.Windows.Forms.TabPage();
            this.tabPageHud = new System.Windows.Forms.TabPage();
            this.splitContainerHud = new System.Windows.Forms.SplitContainer();
            this.buttonStandardHud = new System.Windows.Forms.Button();
            this.panelHudBorder = new System.Windows.Forms.Panel();
            this.buttonDie = new System.Windows.Forms.Button();
            this.buttonFillHp = new System.Windows.Forms.Button();
            this.tabPageCamera = new System.Windows.Forms.TabPage();
            this.SplitContainerCamera = new System.Windows.Forms.SplitContainer();
            this.groupBoxCameraSphericalPos = new System.Windows.Forms.GroupBox();
            this.checkBoxCameraSphericalPosPivotOnFocus = new System.Windows.Forms.CheckBox();
            this.textBoxCameraSphericalPosR = new System.Windows.Forms.TextBox();
            this.buttonCameraSphericalPosRn = new System.Windows.Forms.Button();
            this.buttonCameraSphericalPosRp = new System.Windows.Forms.Button();
            this.buttonCameraSphericalPosTpPp = new System.Windows.Forms.Button();
            this.textBoxCameraSphericalPosTP = new System.Windows.Forms.TextBox();
            this.buttonCameraSphericalPosTp = new System.Windows.Forms.Button();
            this.buttonCameraSphericalPosTpPn = new System.Windows.Forms.Button();
            this.buttonCameraSphericalPosPn = new System.Windows.Forms.Button();
            this.buttonCameraSphericalPosPp = new System.Windows.Forms.Button();
            this.buttonCameraSphericalPosTnPp = new System.Windows.Forms.Button();
            this.buttonCameraSphericalPosTn = new System.Windows.Forms.Button();
            this.buttonCameraSphericalPosTnPn = new System.Windows.Forms.Button();
            this.panelCameraBorder = new System.Windows.Forms.Panel();
            this.groupBoxCameraPos = new System.Windows.Forms.GroupBox();
            this.checkBoxCameraPosRelative = new System.Windows.Forms.CheckBox();
            this.textBoxCameraPosY = new System.Windows.Forms.TextBox();
            this.buttonCameraPosYp = new System.Windows.Forms.Button();
            this.buttonCameraPosYn = new System.Windows.Forms.Button();
            this.buttonCameraPosXpZp = new System.Windows.Forms.Button();
            this.textBoxCameraPosXZ = new System.Windows.Forms.TextBox();
            this.buttonCameraPosXp = new System.Windows.Forms.Button();
            this.buttonCameraPosXpZn = new System.Windows.Forms.Button();
            this.buttonCameraPosZn = new System.Windows.Forms.Button();
            this.buttonCameraPosZp = new System.Windows.Forms.Button();
            this.buttonCameraPosXnZp = new System.Windows.Forms.Button();
            this.buttonCameraPosXn = new System.Windows.Forms.Button();
            this.buttonCameraPosXnZn = new System.Windows.Forms.Button();
            this.tabPageTriangles = new System.Windows.Forms.TabPage();
            this.splitContainerTriangles = new System.Windows.Forms.SplitContainer();
            this.groupBoxTrianglePos = new System.Windows.Forms.GroupBox();
            this.checkBoxTrianglePosRelative = new System.Windows.Forms.CheckBox();
            this.textBoxTrianglePosY = new System.Windows.Forms.TextBox();
            this.buttonTrianglePosYp = new System.Windows.Forms.Button();
            this.buttonTrianglePosYn = new System.Windows.Forms.Button();
            this.buttonTrianglePosXpZp = new System.Windows.Forms.Button();
            this.textBoxTrianglePosXZ = new System.Windows.Forms.TextBox();
            this.buttonTrianglePosXp = new System.Windows.Forms.Button();
            this.buttonTrianglePosXpZn = new System.Windows.Forms.Button();
            this.buttonTrianglePosZn = new System.Windows.Forms.Button();
            this.buttonTrianglePosZp = new System.Windows.Forms.Button();
            this.buttonTrianglePosXnZp = new System.Windows.Forms.Button();
            this.buttonTrianglePosXn = new System.Windows.Forms.Button();
            this.buttonTrianglePosXnZn = new System.Windows.Forms.Button();
            this.groupBoxTriangleNormal = new System.Windows.Forms.GroupBox();
            this.textBoxTriangleNormal = new System.Windows.Forms.TextBox();
            this.buttonTriangleNormalN = new System.Windows.Forms.Button();
            this.buttonTriangleNormalP = new System.Windows.Forms.Button();
            this.radioButtonTriOther = new System.Windows.Forms.RadioButton();
            this.buttonAnnihilateTriangle = new System.Windows.Forms.Button();
            this.radioButtonTriFloor = new System.Windows.Forms.RadioButton();
            this.buttonNeutralizeTriangle = new System.Windows.Forms.Button();
            this.radioButtonTriWall = new System.Windows.Forms.RadioButton();
            this.buttonGoToVClosest = new System.Windows.Forms.Button();
            this.radioButtonTriCeiling = new System.Windows.Forms.RadioButton();
            this.checkBoxVertexMisalignment = new System.Windows.Forms.CheckBox();
            this.maskedTextBoxOtherTriangle = new System.Windows.Forms.MaskedTextBox();
            this.buttonRetrieveTriangle = new System.Windows.Forms.Button();
            this.labelTriangleSelection = new System.Windows.Forms.Label();
            this.buttonGoToV3 = new System.Windows.Forms.Button();
            this.buttonGoToV1 = new System.Windows.Forms.Button();
            this.buttonGoToV2 = new System.Windows.Forms.Button();
            this.tabPageWater = new System.Windows.Forms.TabPage();
            this.tabPageController = new System.Windows.Forms.TabPage();
            this.splitContainerController = new System.Windows.Forms.SplitContainer();
            this.tabPageFile = new System.Windows.Forms.TabPage();
            this.splitContainerFile = new System.Windows.Forms.SplitContainer();
            this.buttonFileNumStars = new System.Windows.Forms.Button();
            this.tableLayoutPanelFile = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxHatLocation = new System.Windows.Forms.GroupBox();
            this.radioButtonHatLocationSLGround = new System.Windows.Forms.RadioButton();
            this.radioButtonHatLocationTTMGround = new System.Windows.Forms.RadioButton();
            this.radioButtonHatLocationTTMUkiki = new System.Windows.Forms.RadioButton();
            this.radioButtonHatLocationSSLKlepto = new System.Windows.Forms.RadioButton();
            this.radioButtonHatLocationMario = new System.Windows.Forms.RadioButton();
            this.radioButtonHatLocationSLSnowman = new System.Windows.Forms.RadioButton();
            this.radioButtonHatLocationSSLGround = new System.Windows.Forms.RadioButton();
            this.groupBoxFile = new System.Windows.Forms.GroupBox();
            this.radioButtonFileASaved = new System.Windows.Forms.RadioButton();
            this.radioButtonFileB = new System.Windows.Forms.RadioButton();
            this.radioButtonFileCSaved = new System.Windows.Forms.RadioButton();
            this.radioButtonFileA = new System.Windows.Forms.RadioButton();
            this.radioButtonFileDSaved = new System.Windows.Forms.RadioButton();
            this.radioButtonFileD = new System.Windows.Forms.RadioButton();
            this.radioButtonFileC = new System.Windows.Forms.RadioButton();
            this.radioButtonFileBSaved = new System.Windows.Forms.RadioButton();
            this.buttonFileSave = new System.Windows.Forms.Button();
            this.tabPageMisc = new System.Windows.Forms.TabPage();
            this.panelMiscBorder = new System.Windows.Forms.Panel();
            this.tabPageDebug = new System.Windows.Forms.TabPage();
            this.checkBoxDbgResource = new System.Windows.Forms.CheckBox();
            this.checkBoxDbgStageSelect = new System.Windows.Forms.CheckBox();
            this.checkBoxDbgClassicDbg = new System.Windows.Forms.CheckBox();
            this.buttonDbgFreeMovement = new System.Windows.Forms.Button();
            this.checkBoxDbgSpawnDbg = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panelDebugBorder = new System.Windows.Forms.Panel();
            this.tabPageMap = new System.Windows.Forms.TabPage();
            this.splitContainerMap = new System.Windows.Forms.SplitContainer();
            this.checkBoxMapShowCeiling = new System.Windows.Forms.CheckBox();
            this.checkBoxMapShowFloor = new System.Windows.Forms.CheckBox();
            this.checkBoxMapShowCamera = new System.Windows.Forms.CheckBox();
            this.checkBoxMapShowHolp = new System.Windows.Forms.CheckBox();
            this.checkBoxMapShowMario = new System.Windows.Forms.CheckBox();
            this.labelMapName = new System.Windows.Forms.Label();
            this.trackBarMapIconSize = new System.Windows.Forms.TrackBar();
            this.labelMapIconSize = new System.Windows.Forms.Label();
            this.checkBoxMapShowInactive = new System.Windows.Forms.CheckBox();
            this.labelMapZoom = new System.Windows.Forms.Label();
            this.labelMapId = new System.Windows.Forms.Label();
            this.labelMapSubName = new System.Windows.Forms.Label();
            this.trackBarMapZoom = new System.Windows.Forms.TrackBar();
            this.glControlMap = new OpenTK.GLControl();
            this.labelMapPu = new System.Windows.Forms.Label();
            this.labelMapPuValue = new System.Windows.Forms.Label();
            this.labelMapQpu = new System.Windows.Forms.Label();
            this.labelMapQpuValue = new System.Windows.Forms.Label();
            this.tabPagePu = new System.Windows.Forms.TabPage();
            this.groupBoxPuController = new System.Windows.Forms.GroupBox();
            this.labelPuConPu = new System.Windows.Forms.Label();
            this.labelPuConQpuValue = new System.Windows.Forms.Label();
            this.labelPuConQpu = new System.Windows.Forms.Label();
            this.labelPuConPuValue = new System.Windows.Forms.Label();
            this.labelPuContXp = new System.Windows.Forms.Label();
            this.labelPuContXm = new System.Windows.Forms.Label();
            this.labelPuContZp = new System.Windows.Forms.Label();
            this.labelPuContZm = new System.Windows.Forms.Label();
            this.buttonPuConZnPu = new System.Windows.Forms.Button();
            this.buttonPuConXpQpu = new System.Windows.Forms.Button();
            this.buttonPuConXnQpu = new System.Windows.Forms.Button();
            this.buttonPuConXnPu = new System.Windows.Forms.Button();
            this.buttonPuConZnQpu = new System.Windows.Forms.Button();
            this.buttonPuConXpPu = new System.Windows.Forms.Button();
            this.buttonPuConZpPu = new System.Windows.Forms.Button();
            this.buttonPuConZpQpu = new System.Windows.Forms.Button();
            this.buttonPuConHome = new System.Windows.Forms.Button();
            this.tabPageExpressions = new System.Windows.Forms.TabPage();
            this.checkBoxAbsoluteAddress = new System.Windows.Forms.CheckBox();
            this.buttonOtherDelete = new System.Windows.Forms.Button();
            this.buttonOtherModify = new System.Windows.Forms.Button();
            this.buttonOtherAdd = new System.Windows.Forms.Button();
            this.dataGridViewExpressions = new System.Windows.Forms.DataGridView();
            this.tabPageDisassembly = new System.Windows.Forms.TabPage();
            this.buttonDisMore = new System.Windows.Forms.Button();
            this.buttonDisGo = new System.Windows.Forms.Button();
            this.maskedTextBoxDisStart = new System.Windows.Forms.MaskedTextBox();
            this.labelDisStart = new System.Windows.Forms.Label();
            this.richTextBoxDissasembly = new System.Windows.Forms.RichTextBox();
            this.tabPageHacks = new System.Windows.Forms.TabPage();
            this.splitContainerHacks = new System.Windows.Forms.SplitContainer();
            this.groupBoxHackRam = new System.Windows.Forms.GroupBox();
            this.labelPureInterpretterRequire = new System.Windows.Forms.Label();
            this.checkedListBoxHacks = new System.Windows.Forms.CheckedListBox();
            this.groupBoxHackSpawn = new System.Windows.Forms.GroupBox();
            this.labelSpawnBehavior = new System.Windows.Forms.Label();
            this.textBoxSpawnBehavior = new System.Windows.Forms.TextBox();
            this.labelSpawnHint = new System.Windows.Forms.Label();
            this.buttonSpawnReset = new System.Windows.Forms.Button();
            this.labelSpawnExtra = new System.Windows.Forms.Label();
            this.labelSpawnGfxId = new System.Windows.Forms.Label();
            this.textBoxSpawnExtra = new System.Windows.Forms.TextBox();
            this.textBoxSpawnGfxId = new System.Windows.Forms.TextBox();
            this.buttonHackSpawn = new System.Windows.Forms.Button();
            this.listBoxSpawn = new System.Windows.Forms.ListBox();
            this.tabPageCamHack = new System.Windows.Forms.TabPage();
            this.splitContainerCamHack = new System.Windows.Forms.SplitContainer();
            this.groupBoxCameraHackSphericalFocusPos = new System.Windows.Forms.GroupBox();
            this.textBoxCameraHackSphericalFocusPosR = new System.Windows.Forms.TextBox();
            this.buttonCameraHackSphericalFocusPosRp = new System.Windows.Forms.Button();
            this.buttonCameraHackSphericalFocusPosRn = new System.Windows.Forms.Button();
            this.buttonCameraHackSphericalFocusPosTnPp = new System.Windows.Forms.Button();
            this.textBoxCameraHackSphericalFocusPosTP = new System.Windows.Forms.TextBox();
            this.buttonCameraHackSphericalFocusPosTn = new System.Windows.Forms.Button();
            this.buttonCameraHackSphericalFocusPosTnPn = new System.Windows.Forms.Button();
            this.buttonCameraHackSphericalFocusPosPn = new System.Windows.Forms.Button();
            this.buttonCameraHackSphericalFocusPosPp = new System.Windows.Forms.Button();
            this.buttonCameraHackSphericalFocusPosTpPp = new System.Windows.Forms.Button();
            this.buttonCameraHackSphericalFocusPosTp = new System.Windows.Forms.Button();
            this.buttonCameraHackSphericalFocusPosTpPn = new System.Windows.Forms.Button();
            this.groupBoxCameraHackFocusPos = new System.Windows.Forms.GroupBox();
            this.checkBoxCameraHackFocusPosRelative = new System.Windows.Forms.CheckBox();
            this.textBoxCameraHackFocusPosY = new System.Windows.Forms.TextBox();
            this.buttonCameraHackFocusPosYp = new System.Windows.Forms.Button();
            this.buttonCameraHackFocusPosYn = new System.Windows.Forms.Button();
            this.buttonCameraHackFocusPosXpZp = new System.Windows.Forms.Button();
            this.textBoxCameraHackFocusPosXZ = new System.Windows.Forms.TextBox();
            this.buttonCameraHackFocusPosXp = new System.Windows.Forms.Button();
            this.buttonCameraHackFocusPosXpZn = new System.Windows.Forms.Button();
            this.buttonCameraHackFocusPosZn = new System.Windows.Forms.Button();
            this.buttonCameraHackFocusPosZp = new System.Windows.Forms.Button();
            this.buttonCameraHackFocusPosXnZp = new System.Windows.Forms.Button();
            this.buttonCameraHackFocusPosXn = new System.Windows.Forms.Button();
            this.buttonCameraHackFocusPosXnZn = new System.Windows.Forms.Button();
            this.groupBoxCameraHackSphericalPos = new System.Windows.Forms.GroupBox();
            this.textBoxCameraHackSphericalPosR = new System.Windows.Forms.TextBox();
            this.buttonCameraHackSphericalPosRn = new System.Windows.Forms.Button();
            this.buttonCameraHackSphericalPosRp = new System.Windows.Forms.Button();
            this.buttonCameraHackSphericalPosTpPp = new System.Windows.Forms.Button();
            this.textBoxCameraHackSphericalPosTP = new System.Windows.Forms.TextBox();
            this.buttonCameraHackSphericalPosTp = new System.Windows.Forms.Button();
            this.buttonCameraHackSphericalPosTpPn = new System.Windows.Forms.Button();
            this.buttonCameraHackSphericalPosPn = new System.Windows.Forms.Button();
            this.buttonCameraHackSphericalPosPp = new System.Windows.Forms.Button();
            this.buttonCameraHackSphericalPosTnPp = new System.Windows.Forms.Button();
            this.buttonCameraHackSphericalPosTn = new System.Windows.Forms.Button();
            this.buttonCameraHackSphericalPosTnPn = new System.Windows.Forms.Button();
            this.groupBoxCameraHackPos = new System.Windows.Forms.GroupBox();
            this.checkBoxCameraHackPosRelative = new System.Windows.Forms.CheckBox();
            this.textBoxCameraHackPosY = new System.Windows.Forms.TextBox();
            this.buttonCameraHackPosYp = new System.Windows.Forms.Button();
            this.buttonCameraHackPosYn = new System.Windows.Forms.Button();
            this.buttonCameraHackPosXpZp = new System.Windows.Forms.Button();
            this.textBoxCameraHackPosXZ = new System.Windows.Forms.TextBox();
            this.buttonCameraHackPosXp = new System.Windows.Forms.Button();
            this.buttonCameraHackPosXpZn = new System.Windows.Forms.Button();
            this.buttonCameraHackPosZn = new System.Windows.Forms.Button();
            this.buttonCameraHackPosZp = new System.Windows.Forms.Button();
            this.buttonCameraHackPosXnZp = new System.Windows.Forms.Button();
            this.buttonCameraHackPosXn = new System.Windows.Forms.Button();
            this.buttonCameraHackPosXnZn = new System.Windows.Forms.Button();
            this.labelCamHackMode = new System.Windows.Forms.Label();
            this.radioButtonCamHackMode3 = new System.Windows.Forms.RadioButton();
            this.radioButtonCamHackMode0 = new System.Windows.Forms.RadioButton();
            this.radioButtonCamHackMode2 = new System.Windows.Forms.RadioButton();
            this.radioButtonCamHackMode1AbsoluteAngle = new System.Windows.Forms.RadioButton();
            this.radioButtonCamHackMode1RelativeAngle = new System.Windows.Forms.RadioButton();
            this.tabPageQuarterFrame = new System.Windows.Forms.TabPage();
            this.tabPageOptions = new System.Windows.Forms.TabPage();
            this.groupBoxGotoRetrieveOffsets = new System.Windows.Forms.GroupBox();
            this.labelRetrieveInfrontSuffix = new System.Windows.Forms.Label();
            this.labelRetrieveInfrontPrefix = new System.Windows.Forms.Label();
            this.textBoxRetrieveInfront = new System.Windows.Forms.TextBox();
            this.labelRetrieveAboveSuffix = new System.Windows.Forms.Label();
            this.labelRetrieveAbovePrefix = new System.Windows.Forms.Label();
            this.textBoxRetrieveAbove = new System.Windows.Forms.TextBox();
            this.labelGotoInfrontSuffix = new System.Windows.Forms.Label();
            this.labelGotoInfrontPrefix = new System.Windows.Forms.Label();
            this.textBoxGotoInfront = new System.Windows.Forms.TextBox();
            this.labelGotoAboveSuffix = new System.Windows.Forms.Label();
            this.labelGotoAbovePrefix = new System.Windows.Forms.Label();
            this.textBoxGotoAbove = new System.Windows.Forms.TextBox();
            this.checkBoxNeutralizeTriangleWith21 = new System.Windows.Forms.CheckBox();
            this.checkBoxDisableActionUpdateWhenCloning = new System.Windows.Forms.CheckBox();
            this.groupBoxShowOverlay = new System.Windows.Forms.GroupBox();
            this.checkBoxShowOverlayCameraHackObject = new System.Windows.Forms.CheckBox();
            this.checkBoxShowOverlayCeilingObject = new System.Windows.Forms.CheckBox();
            this.checkBoxShowOverlayWallObject = new System.Windows.Forms.CheckBox();
            this.checkBoxShowOverlayFloorObject = new System.Windows.Forms.CheckBox();
            this.checkBoxShowOverlayUsedObject = new System.Windows.Forms.CheckBox();
            this.checkBoxShowOverlayInteractionObject = new System.Windows.Forms.CheckBox();
            this.checkBoxShowOverlayCameraObject = new System.Windows.Forms.CheckBox();
            this.checkBoxShowOverlayClosestObject = new System.Windows.Forms.CheckBox();
            this.checkBoxShowOverlayStoodOnObject = new System.Windows.Forms.CheckBox();
            this.checkBoxShowOverlayHeldObject = new System.Windows.Forms.CheckBox();
            this.checkBoxScaleDiagonalPositionControllerButtons = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBoxMoveCamWithPu = new System.Windows.Forms.CheckBox();
            this.checkBoxUseRomHack = new System.Windows.Forms.CheckBox();
            this.checkBoxStartSlotIndexOne = new System.Windows.Forms.CheckBox();
            this.labelVersionNumber = new System.Windows.Forms.Label();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.panelConnect = new System.Windows.Forms.Panel();
            this.buttonRefreshAndConnect = new System.Windows.Forms.Button();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.labelNotConnected = new System.Windows.Forms.Label();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.listBoxProcessesList = new System.Windows.Forms.ListBox();
            this.labelFpsCounter = new System.Windows.Forms.Label();
            this.buttonShowTopPane = new System.Windows.Forms.Button();
            this.buttonShowTopBottomPane = new System.Windows.Forms.Button();
            this.buttonReadOnly = new System.Windows.Forms.Button();
            this.buttonShowBottomPane = new System.Windows.Forms.Button();
            this.buttonShowRightPane = new System.Windows.Forms.Button();
            this.buttonShowLeftRightPane = new System.Windows.Forms.Button();
            this.buttonShowLeftPane = new System.Windows.Forms.Button();
            this.pictureBoxObject = new SM64_Diagnostic.Controls.IntPictureBox();
            this.NoTearFlowLayoutPanelObject = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.pictureBoxMario = new SM64_Diagnostic.Controls.IntPictureBox();
            this.NoTearFlowLayoutPanelMario = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.noTearFlowLayoutPanelActions = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.pictureBoxHud = new SM64_Diagnostic.Controls.IntPictureBox();
            this.NoTearFlowLayoutPanelHud = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.pictureBoxCamera = new SM64_Diagnostic.Controls.IntPictureBox();
            this.NoTearFlowLayoutPanelCamera = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.NoTearFlowLayoutPanelTriangles = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.noTearFlowLayoutPanelWater = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.controllerDisplayPanel = new SM64_Diagnostic.ControllerDisplayPanel();
            this.NoTearFlowLayoutPanelController = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.textBoxTableRow15Col10 = new SM64_Diagnostic.FileCoinScoreTextbox();
            this.textBoxTableRow14Col10 = new SM64_Diagnostic.FileCoinScoreTextbox();
            this.textBoxTableRow13Col10 = new SM64_Diagnostic.FileCoinScoreTextbox();
            this.textBoxTableRow12Col10 = new SM64_Diagnostic.FileCoinScoreTextbox();
            this.textBoxTableRow11Col10 = new SM64_Diagnostic.FileCoinScoreTextbox();
            this.textBoxTableRow10Col10 = new SM64_Diagnostic.FileCoinScoreTextbox();
            this.textBoxTableRow9Col10 = new SM64_Diagnostic.FileCoinScoreTextbox();
            this.textBoxTableRow8Col10 = new SM64_Diagnostic.FileCoinScoreTextbox();
            this.textBoxTableRow7Col10 = new SM64_Diagnostic.FileCoinScoreTextbox();
            this.textBoxTableRow6Col10 = new SM64_Diagnostic.FileCoinScoreTextbox();
            this.textBoxTableRow5Col10 = new SM64_Diagnostic.FileCoinScoreTextbox();
            this.textBoxTableRow4Col10 = new SM64_Diagnostic.FileCoinScoreTextbox();
            this.textBoxTableRow3Col10 = new SM64_Diagnostic.FileCoinScoreTextbox();
            this.textBoxTableRow2Col10 = new SM64_Diagnostic.FileCoinScoreTextbox();
            this.textBoxTableRow1Col10 = new SM64_Diagnostic.FileCoinScoreTextbox();
            this.filePictureBoxTableRow24Col9 = new SM64_Diagnostic.FileBinaryPictureBox();
            this.filePictureBoxTableRow23Col9 = new SM64_Diagnostic.FileBinaryPictureBox();
            this.filePictureBoxTableRow22Col9 = new SM64_Diagnostic.FileBinaryPictureBox();
            this.filePictureBoxTableRow4Col9 = new SM64_Diagnostic.FileBinaryPictureBox();
            this.filePictureBoxTableRow3Col9 = new SM64_Diagnostic.FileBinaryPictureBox();
            this.filePictureBoxTableRow2Col9 = new SM64_Diagnostic.FileBinaryPictureBox();
            this.filePictureBoxTableRow21Col8 = new SM64_Diagnostic.FileBinaryPictureBox();
            this.filePictureBoxTableRow15Col8 = new SM64_Diagnostic.FileBinaryPictureBox();
            this.filePictureBoxTableRow13Col8 = new SM64_Diagnostic.FileBinaryPictureBox();
            this.filePictureBoxTableRow12Col8 = new SM64_Diagnostic.FileBinaryPictureBox();
            this.filePictureBoxTableRow11Col8 = new SM64_Diagnostic.FileBinaryPictureBox();
            this.filePictureBoxTableRow10Col8 = new SM64_Diagnostic.FileBinaryPictureBox();
            this.filePictureBoxTableRow4Col8 = new SM64_Diagnostic.FileBinaryPictureBox();
            this.filePictureBoxTableRow3Col8 = new SM64_Diagnostic.FileBinaryPictureBox();
            this.filePictureBoxTableRow2Col8 = new SM64_Diagnostic.FileBinaryPictureBox();
            this.filePictureBoxTableRow1Col8 = new SM64_Diagnostic.FileBinaryPictureBox();
            this.filePictureBoxTableRow19Col2 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow26Col2 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow25Col3 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow25Col2 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow26Col1 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow25Col1 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow24Col1 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow23Col1 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow22Col1 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow21Col1 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow20Col1 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow19Col1 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow18Col1 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow17Col1 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow16Col1 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow15Col7 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow14Col7 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow13Col7 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow12Col7 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow11Col7 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow10Col7 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow9Col7 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow8Col7 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow7Col7 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow6Col7 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow5Col7 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow4Col7 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow3Col7 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow2Col7 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow1Col7 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow15Col6 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow14Col6 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow13Col6 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow12Col6 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow11Col6 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow10Col6 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow9Col6 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow8Col6 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow7Col6 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow6Col6 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow5Col6 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow4Col6 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow3Col6 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow2Col6 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow1Col6 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow15Col5 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow14Col5 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow13Col5 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow12Col5 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow11Col5 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow10Col5 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow9Col5 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow8Col5 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow7Col5 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow6Col5 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow5Col5 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow4Col5 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow3Col5 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow2Col5 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow1Col5 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow15Col4 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow14Col4 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow13Col4 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow12Col4 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow11Col4 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow10Col4 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow9Col4 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow8Col4 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow7Col4 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow6Col4 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow5Col4 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow4Col4 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow3Col4 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow2Col4 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow1Col4 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow15Col3 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow14Col3 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow13Col3 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow12Col3 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow11Col3 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow10Col3 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow9Col3 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow8Col3 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow7Col3 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow6Col3 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow5Col3 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow4Col3 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow3Col3 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow2Col3 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow1Col3 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow15Col2 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow14Col2 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow13Col2 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow12Col2 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow11Col2 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow10Col2 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow9Col2 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow8Col2 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow7Col2 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow6Col2 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow5Col2 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow4Col2 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow3Col2 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow2Col2 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow1Col2 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow15Col1 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow14Col1 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow13Col1 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow12Col1 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow11Col1 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow10Col1 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow9Col1 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow8Col1 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow7Col1 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow6Col1 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow5Col1 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow4Col1 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow3Col1 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow2Col1 = new SM64_Diagnostic.FileStarPictureBox();
            this.labelFileTableRow1 = new SM64_Diagnostic.FileCourseLabel();
            this.labelFileTableRow2 = new SM64_Diagnostic.FileCourseLabel();
            this.labelFileTableRow3 = new SM64_Diagnostic.FileCourseLabel();
            this.labelFileTableRow4 = new SM64_Diagnostic.FileCourseLabel();
            this.labelFileTableRow5 = new SM64_Diagnostic.FileCourseLabel();
            this.labelFileTableRow6 = new SM64_Diagnostic.FileCourseLabel();
            this.labelFileTableRow7 = new SM64_Diagnostic.FileCourseLabel();
            this.labelFileTableRow8 = new SM64_Diagnostic.FileCourseLabel();
            this.labelFileTableRow9 = new SM64_Diagnostic.FileCourseLabel();
            this.labelFileTableRow10 = new SM64_Diagnostic.FileCourseLabel();
            this.labelFileTableRow11 = new SM64_Diagnostic.FileCourseLabel();
            this.labelFileTableRow12 = new SM64_Diagnostic.FileCourseLabel();
            this.labelFileTableRow13 = new SM64_Diagnostic.FileCourseLabel();
            this.labelFileTableRow14 = new SM64_Diagnostic.FileCourseLabel();
            this.labelFileTableRow15 = new SM64_Diagnostic.FileCourseLabel();
            this.labelFileTableRow16 = new SM64_Diagnostic.FileCourseLabel();
            this.labelFileTableRow17 = new SM64_Diagnostic.FileCourseLabel();
            this.labelFileTableRow18 = new SM64_Diagnostic.FileCourseLabel();
            this.labelFileTableRow19 = new SM64_Diagnostic.FileCourseLabel();
            this.labelFileTableRow20 = new SM64_Diagnostic.FileCourseLabel();
            this.labelFileTableRow21 = new SM64_Diagnostic.FileCourseLabel();
            this.labelFileTableRow22 = new SM64_Diagnostic.FileCourseLabel();
            this.labelFileTableRow23 = new SM64_Diagnostic.FileCourseLabel();
            this.labelFileTableRow24 = new SM64_Diagnostic.FileCourseLabel();
            this.labelFileTableRow25 = new SM64_Diagnostic.FileCourseLabel();
            this.labelFileTableRow26 = new SM64_Diagnostic.FileCourseLabel();
            this.filePictureBoxTableRow1Col1 = new SM64_Diagnostic.FileStarPictureBox();
            this.filePictureBoxTableRow8Col8 = new SM64_Diagnostic.FileBinaryPictureBox();
            this.filePictureBoxTableRow19Col9 = new SM64_Diagnostic.FileBinaryPictureBox();
            this.noTearFlowLayoutPanelFile = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.pictureBoxMisc = new SM64_Diagnostic.Controls.IntPictureBox();
            this.NoTearFlowLayoutPanelMisc = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.pictureBoxDebug = new SM64_Diagnostic.Controls.IntPictureBox();
            this.NoTearFlowLayoutPanelDebugDisplayType = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.radioButtonDbgOff = new System.Windows.Forms.RadioButton();
            this.radioButtonDbgObjCnt = new System.Windows.Forms.RadioButton();
            this.radioButtonDbgChkInfo = new System.Windows.Forms.RadioButton();
            this.radioButtonDbgMapInfo = new System.Windows.Forms.RadioButton();
            this.radioButtonDbgStgInfo = new System.Windows.Forms.RadioButton();
            this.radioButtonDbgFxInfo = new System.Windows.Forms.RadioButton();
            this.radioButtonDbgEnemyInfo = new System.Windows.Forms.RadioButton();
            this.noTearFlowLayoutPanelCamHack = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.noTearFlowLayoutPanelQuarterFrame = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.NoTearFlowLayoutPanelObjects = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.buttonFileErase = new System.Windows.Forms.Button();
            this.groupBoxObjects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarObjSlotSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageObjects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerObject)).BeginInit();
            this.splitContainerObject.Panel1.SuspendLayout();
            this.splitContainerObject.Panel2.SuspendLayout();
            this.splitContainerObject.SuspendLayout();
            this.panelObj.SuspendLayout();
            this.groupBoxObjHome.SuspendLayout();
            this.groupBoxObjScale.SuspendLayout();
            this.groupBoxObjAngle.SuspendLayout();
            this.groupBoxObjPos.SuspendLayout();
            this.panelObjectBorder.SuspendLayout();
            this.tabPageMario.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMario)).BeginInit();
            this.splitContainerMario.Panel1.SuspendLayout();
            this.splitContainerMario.Panel2.SuspendLayout();
            this.splitContainerMario.SuspendLayout();
            this.groupBoxMarioStats.SuspendLayout();
            this.groupBoxMarioHOLP.SuspendLayout();
            this.panelMarioBorder.SuspendLayout();
            this.groupBoxMarioPos.SuspendLayout();
            this.tabPageActions.SuspendLayout();
            this.tabPageHud.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerHud)).BeginInit();
            this.splitContainerHud.Panel1.SuspendLayout();
            this.splitContainerHud.Panel2.SuspendLayout();
            this.splitContainerHud.SuspendLayout();
            this.panelHudBorder.SuspendLayout();
            this.tabPageCamera.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainerCamera)).BeginInit();
            this.SplitContainerCamera.Panel1.SuspendLayout();
            this.SplitContainerCamera.Panel2.SuspendLayout();
            this.SplitContainerCamera.SuspendLayout();
            this.groupBoxCameraSphericalPos.SuspendLayout();
            this.panelCameraBorder.SuspendLayout();
            this.groupBoxCameraPos.SuspendLayout();
            this.tabPageTriangles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTriangles)).BeginInit();
            this.splitContainerTriangles.Panel1.SuspendLayout();
            this.splitContainerTriangles.Panel2.SuspendLayout();
            this.splitContainerTriangles.SuspendLayout();
            this.groupBoxTrianglePos.SuspendLayout();
            this.groupBoxTriangleNormal.SuspendLayout();
            this.tabPageWater.SuspendLayout();
            this.tabPageController.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerController)).BeginInit();
            this.splitContainerController.Panel1.SuspendLayout();
            this.splitContainerController.Panel2.SuspendLayout();
            this.splitContainerController.SuspendLayout();
            this.tabPageFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerFile)).BeginInit();
            this.splitContainerFile.Panel1.SuspendLayout();
            this.splitContainerFile.Panel2.SuspendLayout();
            this.splitContainerFile.SuspendLayout();
            this.tableLayoutPanelFile.SuspendLayout();
            this.groupBoxHatLocation.SuspendLayout();
            this.groupBoxFile.SuspendLayout();
            this.tabPageMisc.SuspendLayout();
            this.panelMiscBorder.SuspendLayout();
            this.tabPageDebug.SuspendLayout();
            this.panelDebugBorder.SuspendLayout();
            this.tabPageMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMap)).BeginInit();
            this.splitContainerMap.Panel1.SuspendLayout();
            this.splitContainerMap.Panel2.SuspendLayout();
            this.splitContainerMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMapIconSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMapZoom)).BeginInit();
            this.tabPagePu.SuspendLayout();
            this.groupBoxPuController.SuspendLayout();
            this.tabPageExpressions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExpressions)).BeginInit();
            this.tabPageDisassembly.SuspendLayout();
            this.tabPageHacks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerHacks)).BeginInit();
            this.splitContainerHacks.Panel1.SuspendLayout();
            this.splitContainerHacks.Panel2.SuspendLayout();
            this.splitContainerHacks.SuspendLayout();
            this.groupBoxHackRam.SuspendLayout();
            this.groupBoxHackSpawn.SuspendLayout();
            this.tabPageCamHack.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerCamHack)).BeginInit();
            this.splitContainerCamHack.Panel1.SuspendLayout();
            this.splitContainerCamHack.Panel2.SuspendLayout();
            this.splitContainerCamHack.SuspendLayout();
            this.groupBoxCameraHackSphericalFocusPos.SuspendLayout();
            this.groupBoxCameraHackFocusPos.SuspendLayout();
            this.groupBoxCameraHackSphericalPos.SuspendLayout();
            this.groupBoxCameraHackPos.SuspendLayout();
            this.tabPageQuarterFrame.SuspendLayout();
            this.tabPageOptions.SuspendLayout();
            this.groupBoxGotoRetrieveOffsets.SuspendLayout();
            this.groupBoxShowOverlay.SuspendLayout();
            this.panelConnect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxObject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMario)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCamera)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow24Col9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow23Col9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow22Col9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow4Col9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow3Col9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow2Col9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow21Col8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow15Col8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow13Col8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow12Col8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow11Col8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow10Col8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow4Col8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow3Col8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow2Col8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow1Col8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow19Col2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow26Col2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow25Col3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow25Col2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow26Col1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow25Col1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow24Col1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow23Col1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow22Col1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow21Col1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow20Col1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow19Col1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow18Col1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow17Col1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow16Col1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow15Col7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow14Col7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow13Col7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow12Col7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow11Col7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow10Col7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow9Col7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow8Col7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow7Col7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow6Col7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow5Col7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow4Col7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow3Col7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow2Col7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow1Col7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow15Col6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow14Col6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow13Col6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow12Col6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow11Col6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow10Col6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow9Col6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow8Col6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow7Col6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow6Col6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow5Col6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow4Col6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow3Col6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow2Col6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow1Col6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow15Col5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow14Col5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow13Col5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow12Col5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow11Col5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow10Col5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow9Col5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow8Col5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow7Col5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow6Col5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow5Col5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow4Col5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow3Col5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow2Col5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow1Col5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow15Col4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow14Col4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow13Col4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow12Col4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow11Col4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow10Col4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow9Col4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow8Col4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow7Col4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow6Col4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow5Col4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow4Col4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow3Col4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow2Col4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow1Col4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow15Col3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow14Col3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow13Col3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow12Col3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow11Col3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow10Col3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow9Col3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow8Col3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow7Col3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow6Col3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow5Col3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow4Col3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow3Col3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow2Col3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow1Col3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow15Col2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow14Col2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow13Col2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow12Col2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow11Col2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow10Col2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow9Col2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow8Col2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow7Col2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow6Col2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow5Col2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow4Col2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow3Col2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow2Col2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow1Col2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow15Col1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow14Col1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow13Col1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow12Col1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow11Col1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow10Col1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow9Col1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow8Col1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow7Col1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow6Col1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow5Col1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow4Col1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow3Col1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow2Col1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow1Col1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow8Col8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow19Col9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMisc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDebug)).BeginInit();
            this.NoTearFlowLayoutPanelDebugDisplayType.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelProcessSelect
            // 
            this.labelProcessSelect.AutoSize = true;
            this.labelProcessSelect.Location = new System.Drawing.Point(138, 15);
            this.labelProcessSelect.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelProcessSelect.Name = "labelProcessSelect";
            this.labelProcessSelect.Size = new System.Drawing.Size(78, 13);
            this.labelProcessSelect.TabIndex = 1;
            this.labelProcessSelect.Text = "Connected To:";
            // 
            // groupBoxObjects
            // 
            this.groupBoxObjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxObjects.Controls.Add(this.comboBoxLabelMethod);
            this.groupBoxObjects.Controls.Add(this.labelLabelMethod);
            this.groupBoxObjects.Controls.Add(this.labelSlotSize);
            this.groupBoxObjects.Controls.Add(this.labelToggleMode);
            this.groupBoxObjects.Controls.Add(this.comboBoxMapToggleMode);
            this.groupBoxObjects.Controls.Add(this.checkBoxObjLockLabels);
            this.groupBoxObjects.Controls.Add(this.labelSortMethod);
            this.groupBoxObjects.Controls.Add(this.NoTearFlowLayoutPanelObjects);
            this.groupBoxObjects.Controls.Add(this.comboBoxSortMethod);
            this.groupBoxObjects.Controls.Add(this.trackBarObjSlotSize);
            this.groupBoxObjects.Location = new System.Drawing.Point(2, 2);
            this.groupBoxObjects.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxObjects.Name = "groupBoxObjects";
            this.groupBoxObjects.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxObjects.Size = new System.Drawing.Size(923, 231);
            this.groupBoxObjects.TabIndex = 2;
            this.groupBoxObjects.TabStop = false;
            this.groupBoxObjects.Text = "Objects";
            // 
            // comboBoxLabelMethod
            // 
            this.comboBoxLabelMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxLabelMethod.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBoxLabelMethod.Location = new System.Drawing.Point(641, 15);
            this.comboBoxLabelMethod.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxLabelMethod.Name = "comboBoxLabelMethod";
            this.comboBoxLabelMethod.Size = new System.Drawing.Size(102, 21);
            this.comboBoxLabelMethod.TabIndex = 13;
            // 
            // labelLabelMethod
            // 
            this.labelLabelMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLabelMethod.AutoSize = true;
            this.labelLabelMethod.Location = new System.Drawing.Point(561, 18);
            this.labelLabelMethod.Name = "labelLabelMethod";
            this.labelLabelMethod.Size = new System.Drawing.Size(75, 13);
            this.labelLabelMethod.TabIndex = 12;
            this.labelLabelMethod.Text = "Label Method:";
            // 
            // labelSlotSize
            // 
            this.labelSlotSize.AutoSize = true;
            this.labelSlotSize.Location = new System.Drawing.Point(110, 19);
            this.labelSlotSize.Name = "labelSlotSize";
            this.labelSlotSize.Size = new System.Drawing.Size(51, 13);
            this.labelSlotSize.TabIndex = 11;
            this.labelSlotSize.Text = "Slot Size:";
            // 
            // labelToggleMode
            // 
            this.labelToggleMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelToggleMode.AutoSize = true;
            this.labelToggleMode.Location = new System.Drawing.Point(357, 19);
            this.labelToggleMode.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelToggleMode.Name = "labelToggleMode";
            this.labelToggleMode.Size = new System.Drawing.Size(73, 13);
            this.labelToggleMode.TabIndex = 10;
            this.labelToggleMode.Text = "Toggle Mode:";
            this.labelToggleMode.Visible = false;
            // 
            // comboBoxMapToggleMode
            // 
            this.comboBoxMapToggleMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMapToggleMode.FormattingEnabled = true;
            this.comboBoxMapToggleMode.Location = new System.Drawing.Point(434, 15);
            this.comboBoxMapToggleMode.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxMapToggleMode.Name = "comboBoxMapToggleMode";
            this.comboBoxMapToggleMode.Size = new System.Drawing.Size(122, 21);
            this.comboBoxMapToggleMode.TabIndex = 9;
            this.comboBoxMapToggleMode.Visible = false;
            // 
            // checkBoxObjLockLabels
            // 
            this.checkBoxObjLockLabels.AutoSize = true;
            this.checkBoxObjLockLabels.Location = new System.Drawing.Point(4, 18);
            this.checkBoxObjLockLabels.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxObjLockLabels.Name = "checkBoxObjLockLabels";
            this.checkBoxObjLockLabels.Size = new System.Drawing.Size(84, 17);
            this.checkBoxObjLockLabels.TabIndex = 7;
            this.checkBoxObjLockLabels.Text = "Lock Labels";
            this.checkBoxObjLockLabels.UseVisualStyleBackColor = true;
            // 
            // labelSortMethod
            // 
            this.labelSortMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSortMethod.AutoSize = true;
            this.labelSortMethod.Location = new System.Drawing.Point(747, 18);
            this.labelSortMethod.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSortMethod.Name = "labelSortMethod";
            this.labelSortMethod.Size = new System.Drawing.Size(68, 13);
            this.labelSortMethod.TabIndex = 5;
            this.labelSortMethod.Text = "Sort Method:";
            // 
            // comboBoxSortMethod
            // 
            this.comboBoxSortMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSortMethod.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBoxSortMethod.Location = new System.Drawing.Point(817, 15);
            this.comboBoxSortMethod.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxSortMethod.Name = "comboBoxSortMethod";
            this.comboBoxSortMethod.Size = new System.Drawing.Size(102, 21);
            this.comboBoxSortMethod.TabIndex = 4;
            // 
            // trackBarObjSlotSize
            // 
            this.trackBarObjSlotSize.Location = new System.Drawing.Point(167, 15);
            this.trackBarObjSlotSize.Maximum = 100;
            this.trackBarObjSlotSize.Minimum = 15;
            this.trackBarObjSlotSize.Name = "trackBarObjSlotSize";
            this.trackBarObjSlotSize.Size = new System.Drawing.Size(104, 45);
            this.trackBarObjSlotSize.TabIndex = 3;
            this.trackBarObjSlotSize.TickFrequency = 10;
            this.trackBarObjSlotSize.Value = 40;
            this.trackBarObjSlotSize.ValueChanged += new System.EventHandler(this.trackBarObjSlotSize_ValueChanged);
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerMain.Location = new System.Drawing.Point(12, 36);
            this.splitContainerMain.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.tabControlMain);
            this.splitContainerMain.Panel1MinSize = 0;
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.groupBoxObjects);
            this.splitContainerMain.Panel2MinSize = 0;
            this.splitContainerMain.Size = new System.Drawing.Size(927, 698);
            this.splitContainerMain.SplitterDistance = 491;
            this.splitContainerMain.SplitterWidth = 3;
            this.splitContainerMain.TabIndex = 4;
            // 
            // tabControlMain
            // 
            this.tabControlMain.AllowDrop = true;
            this.tabControlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlMain.Controls.Add(this.tabPageObjects);
            this.tabControlMain.Controls.Add(this.tabPageMario);
            this.tabControlMain.Controls.Add(this.tabPageActions);
            this.tabControlMain.Controls.Add(this.tabPageHud);
            this.tabControlMain.Controls.Add(this.tabPageCamera);
            this.tabControlMain.Controls.Add(this.tabPageTriangles);
            this.tabControlMain.Controls.Add(this.tabPageWater);
            this.tabControlMain.Controls.Add(this.tabPageController);
            this.tabControlMain.Controls.Add(this.tabPageFile);
            this.tabControlMain.Controls.Add(this.tabPageMisc);
            this.tabControlMain.Controls.Add(this.tabPageDebug);
            this.tabControlMain.Controls.Add(this.tabPageMap);
            this.tabControlMain.Controls.Add(this.tabPagePu);
            this.tabControlMain.Controls.Add(this.tabPageExpressions);
            this.tabControlMain.Controls.Add(this.tabPageDisassembly);
            this.tabControlMain.Controls.Add(this.tabPageHacks);
            this.tabControlMain.Controls.Add(this.tabPageCamHack);
            this.tabControlMain.Controls.Add(this.tabPageQuarterFrame);
            this.tabControlMain.Controls.Add(this.tabPageOptions);
            this.tabControlMain.HotTrack = true;
            this.tabControlMain.Location = new System.Drawing.Point(2, 2);
            this.tabControlMain.Margin = new System.Windows.Forms.Padding(2);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(923, 489);
            this.tabControlMain.TabIndex = 3;
            this.tabControlMain.SelectedIndexChanged += new System.EventHandler(this.tabControlMain_SelectedIndexChanged);
            // 
            // tabPageObjects
            // 
            this.tabPageObjects.BackColor = System.Drawing.Color.Transparent;
            this.tabPageObjects.Controls.Add(this.splitContainerObject);
            this.tabPageObjects.Location = new System.Drawing.Point(4, 22);
            this.tabPageObjects.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageObjects.Name = "tabPageObjects";
            this.tabPageObjects.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageObjects.Size = new System.Drawing.Size(915, 463);
            this.tabPageObjects.TabIndex = 0;
            this.tabPageObjects.Text = "Object";
            // 
            // splitContainerObject
            // 
            this.splitContainerObject.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerObject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerObject.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerObject.Location = new System.Drawing.Point(0, 0);
            this.splitContainerObject.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainerObject.Name = "splitContainerObject";
            // 
            // splitContainerObject.Panel1
            // 
            this.splitContainerObject.Panel1.AutoScroll = true;
            this.splitContainerObject.Panel1.Controls.Add(this.panelObj);
            this.splitContainerObject.Panel1.Controls.Add(this.textBoxObjName);
            this.splitContainerObject.Panel1.Controls.Add(this.labelObjAddValue);
            this.splitContainerObject.Panel1.Controls.Add(this.labelObjSlotIndValue);
            this.splitContainerObject.Panel1.Controls.Add(this.panelObjectBorder);
            this.splitContainerObject.Panel1.Controls.Add(this.labelObjSlotPosValue);
            this.splitContainerObject.Panel1.Controls.Add(this.labelObjBhv);
            this.splitContainerObject.Panel1.Controls.Add(this.labelObjBhvValue);
            this.splitContainerObject.Panel1.Controls.Add(this.labelObjSlotPos);
            this.splitContainerObject.Panel1.Controls.Add(this.labelObjAdd);
            this.splitContainerObject.Panel1.Controls.Add(this.labelObjSlotInd);
            this.splitContainerObject.Panel1MinSize = 0;
            // 
            // splitContainerObject.Panel2
            // 
            this.splitContainerObject.Panel2.Controls.Add(this.NoTearFlowLayoutPanelObject);
            this.splitContainerObject.Panel2MinSize = 0;
            this.splitContainerObject.Size = new System.Drawing.Size(915, 463);
            this.splitContainerObject.SplitterDistance = 217;
            this.splitContainerObject.SplitterWidth = 1;
            this.splitContainerObject.TabIndex = 20;
            // 
            // panelObj
            // 
            this.panelObj.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panelObj.AutoScroll = true;
            this.panelObj.Controls.Add(this.buttonObjRelease);
            this.panelObj.Controls.Add(this.buttonObjInteract);
            this.panelObj.Controls.Add(this.groupBoxObjHome);
            this.panelObj.Controls.Add(this.groupBoxObjScale);
            this.panelObj.Controls.Add(this.groupBoxObjAngle);
            this.panelObj.Controls.Add(this.groupBoxObjPos);
            this.panelObj.Controls.Add(this.buttonObjGoToHome);
            this.panelObj.Controls.Add(this.buttonObjRetrieve);
            this.panelObj.Controls.Add(this.buttonObjRetrieveHome);
            this.panelObj.Controls.Add(this.buttonObjGoTo);
            this.panelObj.Controls.Add(this.buttonObjClone);
            this.panelObj.Controls.Add(this.buttonObjUnload);
            this.panelObj.Location = new System.Drawing.Point(2, 87);
            this.panelObj.Name = "panelObj";
            this.panelObj.Size = new System.Drawing.Size(211, 370);
            this.panelObj.TabIndex = 19;
            // 
            // buttonObjRelease
            // 
            this.buttonObjRelease.Location = new System.Drawing.Point(2, 53);
            this.buttonObjRelease.Margin = new System.Windows.Forms.Padding(2);
            this.buttonObjRelease.Name = "buttonObjRelease";
            this.buttonObjRelease.Size = new System.Drawing.Size(91, 21);
            this.buttonObjRelease.TabIndex = 38;
            this.buttonObjRelease.Text = "Release";
            this.buttonObjRelease.UseVisualStyleBackColor = true;
            // 
            // buttonObjInteract
            // 
            this.buttonObjInteract.Location = new System.Drawing.Point(97, 53);
            this.buttonObjInteract.Margin = new System.Windows.Forms.Padding(2);
            this.buttonObjInteract.Name = "buttonObjInteract";
            this.buttonObjInteract.Size = new System.Drawing.Size(91, 21);
            this.buttonObjInteract.TabIndex = 37;
            this.buttonObjInteract.Text = "Interact";
            this.buttonObjInteract.UseVisualStyleBackColor = true;
            // 
            // groupBoxObjHome
            // 
            this.groupBoxObjHome.Controls.Add(this.checkBoxObjHomeRelative);
            this.groupBoxObjHome.Controls.Add(this.textBoxObjHomeY);
            this.groupBoxObjHome.Controls.Add(this.buttonObjHomeYp);
            this.groupBoxObjHome.Controls.Add(this.buttonObjHomeYn);
            this.groupBoxObjHome.Controls.Add(this.buttonObjHomeXpZp);
            this.groupBoxObjHome.Controls.Add(this.textBoxObjHomeXZ);
            this.groupBoxObjHome.Controls.Add(this.buttonObjHomeXp);
            this.groupBoxObjHome.Controls.Add(this.buttonObjHomeXpZn);
            this.groupBoxObjHome.Controls.Add(this.buttonObjHomeZn);
            this.groupBoxObjHome.Controls.Add(this.buttonObjHomeZp);
            this.groupBoxObjHome.Controls.Add(this.buttonObjHomeXnZp);
            this.groupBoxObjHome.Controls.Add(this.buttonObjHomeXn);
            this.groupBoxObjHome.Controls.Add(this.buttonObjHomeXnZn);
            this.groupBoxObjHome.Location = new System.Drawing.Point(3, 458);
            this.groupBoxObjHome.Name = "groupBoxObjHome";
            this.groupBoxObjHome.Size = new System.Drawing.Size(185, 146);
            this.groupBoxObjHome.TabIndex = 36;
            this.groupBoxObjHome.TabStop = false;
            this.groupBoxObjHome.Text = "Home";
            // 
            // checkBoxObjHomeRelative
            // 
            this.checkBoxObjHomeRelative.AutoSize = true;
            this.checkBoxObjHomeRelative.Location = new System.Drawing.Point(120, 0);
            this.checkBoxObjHomeRelative.Name = "checkBoxObjHomeRelative";
            this.checkBoxObjHomeRelative.Size = new System.Drawing.Size(65, 17);
            this.checkBoxObjHomeRelative.TabIndex = 38;
            this.checkBoxObjHomeRelative.Text = "Relative";
            this.checkBoxObjHomeRelative.UseVisualStyleBackColor = true;
            // 
            // textBoxObjHomeY
            // 
            this.textBoxObjHomeY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxObjHomeY.Location = new System.Drawing.Point(140, 70);
            this.textBoxObjHomeY.Name = "textBoxObjHomeY";
            this.textBoxObjHomeY.Size = new System.Drawing.Size(42, 20);
            this.textBoxObjHomeY.TabIndex = 33;
            this.textBoxObjHomeY.Text = "100";
            this.textBoxObjHomeY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonObjHomeYp
            // 
            this.buttonObjHomeYp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonObjHomeYp.Location = new System.Drawing.Point(140, 16);
            this.buttonObjHomeYp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjHomeYp.Name = "buttonObjHomeYp";
            this.buttonObjHomeYp.Size = new System.Drawing.Size(42, 42);
            this.buttonObjHomeYp.TabIndex = 35;
            this.buttonObjHomeYp.Text = "Y+";
            this.buttonObjHomeYp.UseVisualStyleBackColor = true;
            // 
            // buttonObjHomeYn
            // 
            this.buttonObjHomeYn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonObjHomeYn.Location = new System.Drawing.Point(140, 100);
            this.buttonObjHomeYn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjHomeYn.Name = "buttonObjHomeYn";
            this.buttonObjHomeYn.Size = new System.Drawing.Size(42, 42);
            this.buttonObjHomeYn.TabIndex = 34;
            this.buttonObjHomeYn.Text = "Y-";
            this.buttonObjHomeYn.UseVisualStyleBackColor = true;
            // 
            // buttonObjHomeXpZp
            // 
            this.buttonObjHomeXpZp.Location = new System.Drawing.Point(87, 100);
            this.buttonObjHomeXpZp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjHomeXpZp.Name = "buttonObjHomeXpZp";
            this.buttonObjHomeXpZp.Size = new System.Drawing.Size(42, 42);
            this.buttonObjHomeXpZp.TabIndex = 32;
            this.buttonObjHomeXpZp.Text = "X+Z+";
            this.buttonObjHomeXpZp.UseVisualStyleBackColor = true;
            // 
            // textBoxObjHomeXZ
            // 
            this.textBoxObjHomeXZ.Location = new System.Drawing.Point(45, 70);
            this.textBoxObjHomeXZ.Name = "textBoxObjHomeXZ";
            this.textBoxObjHomeXZ.Size = new System.Drawing.Size(42, 20);
            this.textBoxObjHomeXZ.TabIndex = 27;
            this.textBoxObjHomeXZ.Text = "100";
            this.textBoxObjHomeXZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonObjHomeXp
            // 
            this.buttonObjHomeXp.Location = new System.Drawing.Point(87, 58);
            this.buttonObjHomeXp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjHomeXp.Name = "buttonObjHomeXp";
            this.buttonObjHomeXp.Size = new System.Drawing.Size(42, 42);
            this.buttonObjHomeXp.TabIndex = 31;
            this.buttonObjHomeXp.Text = "X+";
            this.buttonObjHomeXp.UseVisualStyleBackColor = true;
            // 
            // buttonObjHomeXpZn
            // 
            this.buttonObjHomeXpZn.Location = new System.Drawing.Point(87, 16);
            this.buttonObjHomeXpZn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjHomeXpZn.Name = "buttonObjHomeXpZn";
            this.buttonObjHomeXpZn.Size = new System.Drawing.Size(42, 42);
            this.buttonObjHomeXpZn.TabIndex = 30;
            this.buttonObjHomeXpZn.Text = "X+Z-";
            this.buttonObjHomeXpZn.UseVisualStyleBackColor = true;
            // 
            // buttonObjHomeZn
            // 
            this.buttonObjHomeZn.Location = new System.Drawing.Point(45, 16);
            this.buttonObjHomeZn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjHomeZn.Name = "buttonObjHomeZn";
            this.buttonObjHomeZn.Size = new System.Drawing.Size(42, 42);
            this.buttonObjHomeZn.TabIndex = 29;
            this.buttonObjHomeZn.Text = "Z-";
            this.buttonObjHomeZn.UseVisualStyleBackColor = true;
            // 
            // buttonObjHomeZp
            // 
            this.buttonObjHomeZp.Location = new System.Drawing.Point(45, 100);
            this.buttonObjHomeZp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjHomeZp.Name = "buttonObjHomeZp";
            this.buttonObjHomeZp.Size = new System.Drawing.Size(42, 42);
            this.buttonObjHomeZp.TabIndex = 28;
            this.buttonObjHomeZp.Text = "Z+";
            this.buttonObjHomeZp.UseVisualStyleBackColor = true;
            // 
            // buttonObjHomeXnZp
            // 
            this.buttonObjHomeXnZp.Location = new System.Drawing.Point(3, 100);
            this.buttonObjHomeXnZp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjHomeXnZp.Name = "buttonObjHomeXnZp";
            this.buttonObjHomeXnZp.Size = new System.Drawing.Size(42, 42);
            this.buttonObjHomeXnZp.TabIndex = 27;
            this.buttonObjHomeXnZp.Text = "X-Z+";
            this.buttonObjHomeXnZp.UseVisualStyleBackColor = true;
            // 
            // buttonObjHomeXn
            // 
            this.buttonObjHomeXn.Location = new System.Drawing.Point(3, 58);
            this.buttonObjHomeXn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjHomeXn.Name = "buttonObjHomeXn";
            this.buttonObjHomeXn.Size = new System.Drawing.Size(42, 42);
            this.buttonObjHomeXn.TabIndex = 26;
            this.buttonObjHomeXn.Text = "X-";
            this.buttonObjHomeXn.UseVisualStyleBackColor = true;
            // 
            // buttonObjHomeXnZn
            // 
            this.buttonObjHomeXnZn.Location = new System.Drawing.Point(3, 16);
            this.buttonObjHomeXnZn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjHomeXnZn.Name = "buttonObjHomeXnZn";
            this.buttonObjHomeXnZn.Size = new System.Drawing.Size(42, 42);
            this.buttonObjHomeXnZn.TabIndex = 25;
            this.buttonObjHomeXnZn.Text = "X-Z-";
            this.buttonObjHomeXnZn.UseVisualStyleBackColor = true;
            // 
            // groupBoxObjScale
            // 
            this.groupBoxObjScale.Controls.Add(this.checkBoxObjScaleMultiply);
            this.groupBoxObjScale.Controls.Add(this.checkBoxObjScaleAggregate);
            this.groupBoxObjScale.Controls.Add(this.textBoxObjScaleDepth);
            this.groupBoxObjScale.Controls.Add(this.textBoxObjScaleHeight);
            this.groupBoxObjScale.Controls.Add(this.textBoxObjScaleWidth);
            this.groupBoxObjScale.Controls.Add(this.buttonObjScaleDepthN);
            this.groupBoxObjScale.Controls.Add(this.buttonObjScaleHeightN);
            this.groupBoxObjScale.Controls.Add(this.buttonObjScaleWidthN);
            this.groupBoxObjScale.Controls.Add(this.buttonObjScaleDepthP);
            this.groupBoxObjScale.Controls.Add(this.buttonObjScaleHeightP);
            this.groupBoxObjScale.Controls.Add(this.buttonObjScaleWidthP);
            this.groupBoxObjScale.Controls.Add(this.textBoxObjScaleAggregate);
            this.groupBoxObjScale.Controls.Add(this.buttonObjScaleAggregateN);
            this.groupBoxObjScale.Controls.Add(this.buttonObjScaleAggregateP);
            this.groupBoxObjScale.Location = new System.Drawing.Point(3, 357);
            this.groupBoxObjScale.Name = "groupBoxObjScale";
            this.groupBoxObjScale.Size = new System.Drawing.Size(185, 95);
            this.groupBoxObjScale.TabIndex = 29;
            this.groupBoxObjScale.TabStop = false;
            this.groupBoxObjScale.Text = "Scale";
            // 
            // checkBoxObjScaleMultiply
            // 
            this.checkBoxObjScaleMultiply.AutoSize = true;
            this.checkBoxObjScaleMultiply.Location = new System.Drawing.Point(124, 0);
            this.checkBoxObjScaleMultiply.Name = "checkBoxObjScaleMultiply";
            this.checkBoxObjScaleMultiply.Size = new System.Drawing.Size(61, 17);
            this.checkBoxObjScaleMultiply.TabIndex = 38;
            this.checkBoxObjScaleMultiply.Text = "Multiply";
            this.checkBoxObjScaleMultiply.UseVisualStyleBackColor = true;
            // 
            // checkBoxObjScaleAggregate
            // 
            this.checkBoxObjScaleAggregate.AutoSize = true;
            this.checkBoxObjScaleAggregate.Location = new System.Drawing.Point(50, 0);
            this.checkBoxObjScaleAggregate.Name = "checkBoxObjScaleAggregate";
            this.checkBoxObjScaleAggregate.Size = new System.Drawing.Size(75, 17);
            this.checkBoxObjScaleAggregate.TabIndex = 39;
            this.checkBoxObjScaleAggregate.Text = "Aggregate";
            this.checkBoxObjScaleAggregate.UseVisualStyleBackColor = true;
            // 
            // textBoxObjScaleDepth
            // 
            this.textBoxObjScaleDepth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxObjScaleDepth.Location = new System.Drawing.Point(67, 69);
            this.textBoxObjScaleDepth.Name = "textBoxObjScaleDepth";
            this.textBoxObjScaleDepth.Size = new System.Drawing.Size(51, 20);
            this.textBoxObjScaleDepth.TabIndex = 33;
            this.textBoxObjScaleDepth.Text = "1";
            this.textBoxObjScaleDepth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxObjScaleHeight
            // 
            this.textBoxObjScaleHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxObjScaleHeight.Location = new System.Drawing.Point(67, 44);
            this.textBoxObjScaleHeight.Name = "textBoxObjScaleHeight";
            this.textBoxObjScaleHeight.Size = new System.Drawing.Size(51, 20);
            this.textBoxObjScaleHeight.TabIndex = 33;
            this.textBoxObjScaleHeight.Text = "1";
            this.textBoxObjScaleHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxObjScaleWidth
            // 
            this.textBoxObjScaleWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxObjScaleWidth.Location = new System.Drawing.Point(67, 19);
            this.textBoxObjScaleWidth.Name = "textBoxObjScaleWidth";
            this.textBoxObjScaleWidth.Size = new System.Drawing.Size(51, 20);
            this.textBoxObjScaleWidth.TabIndex = 33;
            this.textBoxObjScaleWidth.Text = "1";
            this.textBoxObjScaleWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonObjScaleDepthN
            // 
            this.buttonObjScaleDepthN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonObjScaleDepthN.Location = new System.Drawing.Point(3, 66);
            this.buttonObjScaleDepthN.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjScaleDepthN.Name = "buttonObjScaleDepthN";
            this.buttonObjScaleDepthN.Size = new System.Drawing.Size(61, 25);
            this.buttonObjScaleDepthN.TabIndex = 35;
            this.buttonObjScaleDepthN.Text = "Depth-";
            this.buttonObjScaleDepthN.UseVisualStyleBackColor = true;
            // 
            // buttonObjScaleHeightN
            // 
            this.buttonObjScaleHeightN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonObjScaleHeightN.Location = new System.Drawing.Point(3, 41);
            this.buttonObjScaleHeightN.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjScaleHeightN.Name = "buttonObjScaleHeightN";
            this.buttonObjScaleHeightN.Size = new System.Drawing.Size(61, 25);
            this.buttonObjScaleHeightN.TabIndex = 35;
            this.buttonObjScaleHeightN.Text = "Height-";
            this.buttonObjScaleHeightN.UseVisualStyleBackColor = true;
            // 
            // buttonObjScaleWidthN
            // 
            this.buttonObjScaleWidthN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonObjScaleWidthN.Location = new System.Drawing.Point(3, 16);
            this.buttonObjScaleWidthN.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjScaleWidthN.Name = "buttonObjScaleWidthN";
            this.buttonObjScaleWidthN.Size = new System.Drawing.Size(61, 25);
            this.buttonObjScaleWidthN.TabIndex = 35;
            this.buttonObjScaleWidthN.Text = "Width-";
            this.buttonObjScaleWidthN.UseVisualStyleBackColor = true;
            // 
            // buttonObjScaleDepthP
            // 
            this.buttonObjScaleDepthP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonObjScaleDepthP.Location = new System.Drawing.Point(121, 66);
            this.buttonObjScaleDepthP.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjScaleDepthP.Name = "buttonObjScaleDepthP";
            this.buttonObjScaleDepthP.Size = new System.Drawing.Size(61, 25);
            this.buttonObjScaleDepthP.TabIndex = 35;
            this.buttonObjScaleDepthP.Text = "Depth+";
            this.buttonObjScaleDepthP.UseVisualStyleBackColor = true;
            // 
            // buttonObjScaleHeightP
            // 
            this.buttonObjScaleHeightP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonObjScaleHeightP.Location = new System.Drawing.Point(121, 41);
            this.buttonObjScaleHeightP.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjScaleHeightP.Name = "buttonObjScaleHeightP";
            this.buttonObjScaleHeightP.Size = new System.Drawing.Size(61, 25);
            this.buttonObjScaleHeightP.TabIndex = 35;
            this.buttonObjScaleHeightP.Text = "Height+";
            this.buttonObjScaleHeightP.UseVisualStyleBackColor = true;
            // 
            // buttonObjScaleWidthP
            // 
            this.buttonObjScaleWidthP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonObjScaleWidthP.Location = new System.Drawing.Point(121, 16);
            this.buttonObjScaleWidthP.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjScaleWidthP.Name = "buttonObjScaleWidthP";
            this.buttonObjScaleWidthP.Size = new System.Drawing.Size(61, 25);
            this.buttonObjScaleWidthP.TabIndex = 35;
            this.buttonObjScaleWidthP.Text = "Width+";
            this.buttonObjScaleWidthP.UseVisualStyleBackColor = true;
            // 
            // textBoxObjScaleAggregate
            // 
            this.textBoxObjScaleAggregate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxObjScaleAggregate.Location = new System.Drawing.Point(67, 44);
            this.textBoxObjScaleAggregate.Name = "textBoxObjScaleAggregate";
            this.textBoxObjScaleAggregate.Size = new System.Drawing.Size(51, 20);
            this.textBoxObjScaleAggregate.TabIndex = 40;
            this.textBoxObjScaleAggregate.Text = "1";
            this.textBoxObjScaleAggregate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxObjScaleAggregate.Visible = false;
            // 
            // buttonObjScaleAggregateN
            // 
            this.buttonObjScaleAggregateN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonObjScaleAggregateN.Location = new System.Drawing.Point(3, 16);
            this.buttonObjScaleAggregateN.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjScaleAggregateN.Name = "buttonObjScaleAggregateN";
            this.buttonObjScaleAggregateN.Size = new System.Drawing.Size(61, 75);
            this.buttonObjScaleAggregateN.TabIndex = 41;
            this.buttonObjScaleAggregateN.Text = "Scale-";
            this.buttonObjScaleAggregateN.UseVisualStyleBackColor = true;
            this.buttonObjScaleAggregateN.Visible = false;
            // 
            // buttonObjScaleAggregateP
            // 
            this.buttonObjScaleAggregateP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonObjScaleAggregateP.Location = new System.Drawing.Point(121, 16);
            this.buttonObjScaleAggregateP.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjScaleAggregateP.Name = "buttonObjScaleAggregateP";
            this.buttonObjScaleAggregateP.Size = new System.Drawing.Size(61, 75);
            this.buttonObjScaleAggregateP.TabIndex = 42;
            this.buttonObjScaleAggregateP.Text = "Scale+";
            this.buttonObjScaleAggregateP.UseVisualStyleBackColor = true;
            this.buttonObjScaleAggregateP.Visible = false;
            // 
            // groupBoxObjAngle
            // 
            this.groupBoxObjAngle.Controls.Add(this.textBoxObjAngleRoll);
            this.groupBoxObjAngle.Controls.Add(this.textBoxObjAnglePitch);
            this.groupBoxObjAngle.Controls.Add(this.textBoxObjAngleYaw);
            this.groupBoxObjAngle.Controls.Add(this.buttonObjAngleRollN);
            this.groupBoxObjAngle.Controls.Add(this.buttonObjAnglePitchN);
            this.groupBoxObjAngle.Controls.Add(this.buttonObjAngleYawN);
            this.groupBoxObjAngle.Controls.Add(this.buttonObjAngleRollP);
            this.groupBoxObjAngle.Controls.Add(this.buttonObjAnglePitchP);
            this.groupBoxObjAngle.Controls.Add(this.buttonObjAngleYawP);
            this.groupBoxObjAngle.Location = new System.Drawing.Point(3, 256);
            this.groupBoxObjAngle.Name = "groupBoxObjAngle";
            this.groupBoxObjAngle.Size = new System.Drawing.Size(185, 95);
            this.groupBoxObjAngle.TabIndex = 29;
            this.groupBoxObjAngle.TabStop = false;
            this.groupBoxObjAngle.Text = "Angle";
            // 
            // textBoxObjAngleRoll
            // 
            this.textBoxObjAngleRoll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxObjAngleRoll.Location = new System.Drawing.Point(67, 69);
            this.textBoxObjAngleRoll.Name = "textBoxObjAngleRoll";
            this.textBoxObjAngleRoll.Size = new System.Drawing.Size(51, 20);
            this.textBoxObjAngleRoll.TabIndex = 33;
            this.textBoxObjAngleRoll.Text = "1024";
            this.textBoxObjAngleRoll.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxObjAnglePitch
            // 
            this.textBoxObjAnglePitch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxObjAnglePitch.Location = new System.Drawing.Point(67, 44);
            this.textBoxObjAnglePitch.Name = "textBoxObjAnglePitch";
            this.textBoxObjAnglePitch.Size = new System.Drawing.Size(51, 20);
            this.textBoxObjAnglePitch.TabIndex = 33;
            this.textBoxObjAnglePitch.Text = "1024";
            this.textBoxObjAnglePitch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxObjAngleYaw
            // 
            this.textBoxObjAngleYaw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxObjAngleYaw.Location = new System.Drawing.Point(67, 19);
            this.textBoxObjAngleYaw.Name = "textBoxObjAngleYaw";
            this.textBoxObjAngleYaw.Size = new System.Drawing.Size(51, 20);
            this.textBoxObjAngleYaw.TabIndex = 33;
            this.textBoxObjAngleYaw.Text = "1024";
            this.textBoxObjAngleYaw.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonObjAngleRollN
            // 
            this.buttonObjAngleRollN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonObjAngleRollN.Location = new System.Drawing.Point(3, 66);
            this.buttonObjAngleRollN.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjAngleRollN.Name = "buttonObjAngleRollN";
            this.buttonObjAngleRollN.Size = new System.Drawing.Size(61, 25);
            this.buttonObjAngleRollN.TabIndex = 35;
            this.buttonObjAngleRollN.Text = "Roll-";
            this.buttonObjAngleRollN.UseVisualStyleBackColor = true;
            // 
            // buttonObjAnglePitchN
            // 
            this.buttonObjAnglePitchN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonObjAnglePitchN.Location = new System.Drawing.Point(3, 41);
            this.buttonObjAnglePitchN.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjAnglePitchN.Name = "buttonObjAnglePitchN";
            this.buttonObjAnglePitchN.Size = new System.Drawing.Size(61, 25);
            this.buttonObjAnglePitchN.TabIndex = 35;
            this.buttonObjAnglePitchN.Text = "Pitch-";
            this.buttonObjAnglePitchN.UseVisualStyleBackColor = true;
            // 
            // buttonObjAngleYawN
            // 
            this.buttonObjAngleYawN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonObjAngleYawN.Location = new System.Drawing.Point(3, 16);
            this.buttonObjAngleYawN.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjAngleYawN.Name = "buttonObjAngleYawN";
            this.buttonObjAngleYawN.Size = new System.Drawing.Size(61, 25);
            this.buttonObjAngleYawN.TabIndex = 35;
            this.buttonObjAngleYawN.Text = "Yaw-";
            this.buttonObjAngleYawN.UseVisualStyleBackColor = true;
            // 
            // buttonObjAngleRollP
            // 
            this.buttonObjAngleRollP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonObjAngleRollP.Location = new System.Drawing.Point(121, 66);
            this.buttonObjAngleRollP.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjAngleRollP.Name = "buttonObjAngleRollP";
            this.buttonObjAngleRollP.Size = new System.Drawing.Size(61, 25);
            this.buttonObjAngleRollP.TabIndex = 35;
            this.buttonObjAngleRollP.Text = "Roll+";
            this.buttonObjAngleRollP.UseVisualStyleBackColor = true;
            // 
            // buttonObjAnglePitchP
            // 
            this.buttonObjAnglePitchP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonObjAnglePitchP.Location = new System.Drawing.Point(121, 41);
            this.buttonObjAnglePitchP.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjAnglePitchP.Name = "buttonObjAnglePitchP";
            this.buttonObjAnglePitchP.Size = new System.Drawing.Size(61, 25);
            this.buttonObjAnglePitchP.TabIndex = 35;
            this.buttonObjAnglePitchP.Text = "Pitch+";
            this.buttonObjAnglePitchP.UseVisualStyleBackColor = true;
            // 
            // buttonObjAngleYawP
            // 
            this.buttonObjAngleYawP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonObjAngleYawP.Location = new System.Drawing.Point(121, 16);
            this.buttonObjAngleYawP.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjAngleYawP.Name = "buttonObjAngleYawP";
            this.buttonObjAngleYawP.Size = new System.Drawing.Size(61, 25);
            this.buttonObjAngleYawP.TabIndex = 35;
            this.buttonObjAngleYawP.Text = "Yaw+";
            this.buttonObjAngleYawP.UseVisualStyleBackColor = true;
            // 
            // groupBoxObjPos
            // 
            this.groupBoxObjPos.Controls.Add(this.checkBoxObjPosRelative);
            this.groupBoxObjPos.Controls.Add(this.textBoxObjPosY);
            this.groupBoxObjPos.Controls.Add(this.buttonObjPosYp);
            this.groupBoxObjPos.Controls.Add(this.buttonObjPosYn);
            this.groupBoxObjPos.Controls.Add(this.buttonObjPosXpZp);
            this.groupBoxObjPos.Controls.Add(this.textBoxObjPosXZ);
            this.groupBoxObjPos.Controls.Add(this.buttonObjPosXp);
            this.groupBoxObjPos.Controls.Add(this.buttonObjPosXpZn);
            this.groupBoxObjPos.Controls.Add(this.buttonObjPosZn);
            this.groupBoxObjPos.Controls.Add(this.buttonObjPosZp);
            this.groupBoxObjPos.Controls.Add(this.buttonObjPosXnZp);
            this.groupBoxObjPos.Controls.Add(this.buttonObjPosXn);
            this.groupBoxObjPos.Controls.Add(this.buttonObjPosXnZn);
            this.groupBoxObjPos.Location = new System.Drawing.Point(3, 104);
            this.groupBoxObjPos.Name = "groupBoxObjPos";
            this.groupBoxObjPos.Size = new System.Drawing.Size(185, 146);
            this.groupBoxObjPos.TabIndex = 28;
            this.groupBoxObjPos.TabStop = false;
            this.groupBoxObjPos.Text = "Position";
            // 
            // checkBoxObjPosRelative
            // 
            this.checkBoxObjPosRelative.AutoSize = true;
            this.checkBoxObjPosRelative.Location = new System.Drawing.Point(120, 0);
            this.checkBoxObjPosRelative.Name = "checkBoxObjPosRelative";
            this.checkBoxObjPosRelative.Size = new System.Drawing.Size(65, 17);
            this.checkBoxObjPosRelative.TabIndex = 37;
            this.checkBoxObjPosRelative.Text = "Relative";
            this.checkBoxObjPosRelative.UseVisualStyleBackColor = true;
            // 
            // textBoxObjPosY
            // 
            this.textBoxObjPosY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxObjPosY.Location = new System.Drawing.Point(140, 70);
            this.textBoxObjPosY.Name = "textBoxObjPosY";
            this.textBoxObjPosY.Size = new System.Drawing.Size(42, 20);
            this.textBoxObjPosY.TabIndex = 33;
            this.textBoxObjPosY.Text = "100";
            this.textBoxObjPosY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonObjPosYp
            // 
            this.buttonObjPosYp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonObjPosYp.Location = new System.Drawing.Point(140, 16);
            this.buttonObjPosYp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjPosYp.Name = "buttonObjPosYp";
            this.buttonObjPosYp.Size = new System.Drawing.Size(42, 42);
            this.buttonObjPosYp.TabIndex = 35;
            this.buttonObjPosYp.Text = "Y+";
            this.buttonObjPosYp.UseVisualStyleBackColor = true;
            // 
            // buttonObjPosYn
            // 
            this.buttonObjPosYn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonObjPosYn.Location = new System.Drawing.Point(140, 100);
            this.buttonObjPosYn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjPosYn.Name = "buttonObjPosYn";
            this.buttonObjPosYn.Size = new System.Drawing.Size(42, 42);
            this.buttonObjPosYn.TabIndex = 34;
            this.buttonObjPosYn.Text = "Y-";
            this.buttonObjPosYn.UseVisualStyleBackColor = true;
            // 
            // buttonObjPosXpZp
            // 
            this.buttonObjPosXpZp.Location = new System.Drawing.Point(87, 100);
            this.buttonObjPosXpZp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjPosXpZp.Name = "buttonObjPosXpZp";
            this.buttonObjPosXpZp.Size = new System.Drawing.Size(42, 42);
            this.buttonObjPosXpZp.TabIndex = 32;
            this.buttonObjPosXpZp.Text = "X+Z+";
            this.buttonObjPosXpZp.UseVisualStyleBackColor = true;
            // 
            // textBoxObjPosXZ
            // 
            this.textBoxObjPosXZ.Location = new System.Drawing.Point(45, 70);
            this.textBoxObjPosXZ.Name = "textBoxObjPosXZ";
            this.textBoxObjPosXZ.Size = new System.Drawing.Size(42, 20);
            this.textBoxObjPosXZ.TabIndex = 27;
            this.textBoxObjPosXZ.Text = "100";
            this.textBoxObjPosXZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonObjPosXp
            // 
            this.buttonObjPosXp.Location = new System.Drawing.Point(87, 58);
            this.buttonObjPosXp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjPosXp.Name = "buttonObjPosXp";
            this.buttonObjPosXp.Size = new System.Drawing.Size(42, 42);
            this.buttonObjPosXp.TabIndex = 31;
            this.buttonObjPosXp.Text = "X+";
            this.buttonObjPosXp.UseVisualStyleBackColor = true;
            // 
            // buttonObjPosXpZn
            // 
            this.buttonObjPosXpZn.Location = new System.Drawing.Point(87, 16);
            this.buttonObjPosXpZn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjPosXpZn.Name = "buttonObjPosXpZn";
            this.buttonObjPosXpZn.Size = new System.Drawing.Size(42, 42);
            this.buttonObjPosXpZn.TabIndex = 30;
            this.buttonObjPosXpZn.Text = "X+Z-";
            this.buttonObjPosXpZn.UseVisualStyleBackColor = true;
            // 
            // buttonObjPosZn
            // 
            this.buttonObjPosZn.Location = new System.Drawing.Point(45, 16);
            this.buttonObjPosZn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjPosZn.Name = "buttonObjPosZn";
            this.buttonObjPosZn.Size = new System.Drawing.Size(42, 42);
            this.buttonObjPosZn.TabIndex = 29;
            this.buttonObjPosZn.Text = "Z-";
            this.buttonObjPosZn.UseVisualStyleBackColor = true;
            // 
            // buttonObjPosZp
            // 
            this.buttonObjPosZp.Location = new System.Drawing.Point(45, 100);
            this.buttonObjPosZp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjPosZp.Name = "buttonObjPosZp";
            this.buttonObjPosZp.Size = new System.Drawing.Size(42, 42);
            this.buttonObjPosZp.TabIndex = 28;
            this.buttonObjPosZp.Text = "Z+";
            this.buttonObjPosZp.UseVisualStyleBackColor = true;
            // 
            // buttonObjPosXnZp
            // 
            this.buttonObjPosXnZp.Location = new System.Drawing.Point(3, 100);
            this.buttonObjPosXnZp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjPosXnZp.Name = "buttonObjPosXnZp";
            this.buttonObjPosXnZp.Size = new System.Drawing.Size(42, 42);
            this.buttonObjPosXnZp.TabIndex = 27;
            this.buttonObjPosXnZp.Text = "X-Z+";
            this.buttonObjPosXnZp.UseVisualStyleBackColor = true;
            // 
            // buttonObjPosXn
            // 
            this.buttonObjPosXn.Location = new System.Drawing.Point(3, 58);
            this.buttonObjPosXn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjPosXn.Name = "buttonObjPosXn";
            this.buttonObjPosXn.Size = new System.Drawing.Size(42, 42);
            this.buttonObjPosXn.TabIndex = 26;
            this.buttonObjPosXn.Text = "X-";
            this.buttonObjPosXn.UseVisualStyleBackColor = true;
            // 
            // buttonObjPosXnZn
            // 
            this.buttonObjPosXnZn.Location = new System.Drawing.Point(3, 16);
            this.buttonObjPosXnZn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonObjPosXnZn.Name = "buttonObjPosXnZn";
            this.buttonObjPosXnZn.Size = new System.Drawing.Size(42, 42);
            this.buttonObjPosXnZn.TabIndex = 25;
            this.buttonObjPosXnZn.Text = "X-Z-";
            this.buttonObjPosXnZn.UseVisualStyleBackColor = true;
            // 
            // buttonObjGoToHome
            // 
            this.buttonObjGoToHome.Location = new System.Drawing.Point(2, 28);
            this.buttonObjGoToHome.Margin = new System.Windows.Forms.Padding(2);
            this.buttonObjGoToHome.Name = "buttonObjGoToHome";
            this.buttonObjGoToHome.Size = new System.Drawing.Size(91, 21);
            this.buttonObjGoToHome.TabIndex = 17;
            this.buttonObjGoToHome.Text = "Go To Home";
            this.buttonObjGoToHome.UseVisualStyleBackColor = true;
            // 
            // buttonObjRetrieve
            // 
            this.buttonObjRetrieve.Location = new System.Drawing.Point(97, 3);
            this.buttonObjRetrieve.Margin = new System.Windows.Forms.Padding(2);
            this.buttonObjRetrieve.Name = "buttonObjRetrieve";
            this.buttonObjRetrieve.Size = new System.Drawing.Size(91, 21);
            this.buttonObjRetrieve.TabIndex = 15;
            this.buttonObjRetrieve.Text = "Retrieve";
            this.buttonObjRetrieve.UseVisualStyleBackColor = true;
            // 
            // buttonObjRetrieveHome
            // 
            this.buttonObjRetrieveHome.Location = new System.Drawing.Point(97, 28);
            this.buttonObjRetrieveHome.Margin = new System.Windows.Forms.Padding(2);
            this.buttonObjRetrieveHome.Name = "buttonObjRetrieveHome";
            this.buttonObjRetrieveHome.Size = new System.Drawing.Size(91, 21);
            this.buttonObjRetrieveHome.TabIndex = 18;
            this.buttonObjRetrieveHome.Text = "Retrieve Home";
            this.buttonObjRetrieveHome.UseVisualStyleBackColor = true;
            // 
            // buttonObjGoTo
            // 
            this.buttonObjGoTo.Location = new System.Drawing.Point(2, 3);
            this.buttonObjGoTo.Margin = new System.Windows.Forms.Padding(2);
            this.buttonObjGoTo.Name = "buttonObjGoTo";
            this.buttonObjGoTo.Size = new System.Drawing.Size(91, 21);
            this.buttonObjGoTo.TabIndex = 13;
            this.buttonObjGoTo.Text = "Go To";
            this.buttonObjGoTo.UseVisualStyleBackColor = true;
            // 
            // buttonObjClone
            // 
            this.buttonObjClone.Location = new System.Drawing.Point(2, 78);
            this.buttonObjClone.Margin = new System.Windows.Forms.Padding(2);
            this.buttonObjClone.Name = "buttonObjClone";
            this.buttonObjClone.Size = new System.Drawing.Size(91, 21);
            this.buttonObjClone.TabIndex = 14;
            this.buttonObjClone.Text = "Clone";
            this.buttonObjClone.UseVisualStyleBackColor = true;
            // 
            // buttonObjUnload
            // 
            this.buttonObjUnload.Location = new System.Drawing.Point(97, 78);
            this.buttonObjUnload.Margin = new System.Windows.Forms.Padding(2);
            this.buttonObjUnload.Name = "buttonObjUnload";
            this.buttonObjUnload.Size = new System.Drawing.Size(91, 21);
            this.buttonObjUnload.TabIndex = 5;
            this.buttonObjUnload.Text = "Unload";
            this.buttonObjUnload.UseVisualStyleBackColor = true;
            // 
            // textBoxObjName
            // 
            this.textBoxObjName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxObjName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxObjName.Location = new System.Drawing.Point(64, 4);
            this.textBoxObjName.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxObjName.Multiline = true;
            this.textBoxObjName.Name = "textBoxObjName";
            this.textBoxObjName.ReadOnly = true;
            this.textBoxObjName.Size = new System.Drawing.Size(139, 26);
            this.textBoxObjName.TabIndex = 16;
            this.textBoxObjName.Text = "No Object Selected\r\n";
            this.textBoxObjName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelObjAddValue
            // 
            this.labelObjAddValue.Location = new System.Drawing.Point(84, 71);
            this.labelObjAddValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelObjAddValue.Name = "labelObjAddValue";
            this.labelObjAddValue.Size = new System.Drawing.Size(75, 17);
            this.labelObjAddValue.TabIndex = 12;
            this.labelObjAddValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelObjSlotIndValue
            // 
            this.labelObjSlotIndValue.Location = new System.Drawing.Point(119, 58);
            this.labelObjSlotIndValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelObjSlotIndValue.Name = "labelObjSlotIndValue";
            this.labelObjSlotIndValue.Size = new System.Drawing.Size(39, 13);
            this.labelObjSlotIndValue.TabIndex = 11;
            this.labelObjSlotIndValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panelObjectBorder
            // 
            this.panelObjectBorder.Controls.Add(this.pictureBoxObject);
            this.panelObjectBorder.Location = new System.Drawing.Point(3, 4);
            this.panelObjectBorder.Margin = new System.Windows.Forms.Padding(2);
            this.panelObjectBorder.Name = "panelObjectBorder";
            this.panelObjectBorder.Size = new System.Drawing.Size(57, 55);
            this.panelObjectBorder.TabIndex = 2;
            // 
            // labelObjSlotPosValue
            // 
            this.labelObjSlotPosValue.Location = new System.Drawing.Point(111, 44);
            this.labelObjSlotPosValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelObjSlotPosValue.Name = "labelObjSlotPosValue";
            this.labelObjSlotPosValue.Size = new System.Drawing.Size(47, 14);
            this.labelObjSlotPosValue.TabIndex = 10;
            this.labelObjSlotPosValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelObjBhv
            // 
            this.labelObjBhv.AutoSize = true;
            this.labelObjBhv.Location = new System.Drawing.Point(62, 31);
            this.labelObjBhv.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelObjBhv.Name = "labelObjBhv";
            this.labelObjBhv.Size = new System.Drawing.Size(29, 13);
            this.labelObjBhv.TabIndex = 5;
            this.labelObjBhv.Text = "Bhv:";
            // 
            // labelObjBhvValue
            // 
            this.labelObjBhvValue.Location = new System.Drawing.Point(90, 31);
            this.labelObjBhvValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelObjBhvValue.Name = "labelObjBhvValue";
            this.labelObjBhvValue.Size = new System.Drawing.Size(68, 13);
            this.labelObjBhvValue.TabIndex = 9;
            this.labelObjBhvValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelObjSlotPos
            // 
            this.labelObjSlotPos.AutoSize = true;
            this.labelObjSlotPos.Location = new System.Drawing.Point(62, 44);
            this.labelObjSlotPos.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelObjSlotPos.Name = "labelObjSlotPos";
            this.labelObjSlotPos.Size = new System.Drawing.Size(49, 13);
            this.labelObjSlotPos.TabIndex = 6;
            this.labelObjSlotPos.Text = "Slot Pos:";
            // 
            // labelObjAdd
            // 
            this.labelObjAdd.AutoSize = true;
            this.labelObjAdd.Location = new System.Drawing.Point(62, 71);
            this.labelObjAdd.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.labelObjAdd.Name = "labelObjAdd";
            this.labelObjAdd.Size = new System.Drawing.Size(29, 13);
            this.labelObjAdd.TabIndex = 8;
            this.labelObjAdd.Text = "Add:";
            // 
            // labelObjSlotInd
            // 
            this.labelObjSlotInd.AutoSize = true;
            this.labelObjSlotInd.Location = new System.Drawing.Point(62, 58);
            this.labelObjSlotInd.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelObjSlotInd.Name = "labelObjSlotInd";
            this.labelObjSlotInd.Size = new System.Drawing.Size(57, 13);
            this.labelObjSlotInd.TabIndex = 7;
            this.labelObjSlotInd.Text = "Slot Index:";
            // 
            // tabPageMario
            // 
            this.tabPageMario.BackColor = System.Drawing.Color.Transparent;
            this.tabPageMario.Controls.Add(this.splitContainerMario);
            this.tabPageMario.Location = new System.Drawing.Point(4, 22);
            this.tabPageMario.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageMario.Name = "tabPageMario";
            this.tabPageMario.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageMario.Size = new System.Drawing.Size(915, 463);
            this.tabPageMario.TabIndex = 1;
            this.tabPageMario.Text = "Mario";
            // 
            // splitContainerMario
            // 
            this.splitContainerMario.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerMario.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerMario.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerMario.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMario.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainerMario.Name = "splitContainerMario";
            // 
            // splitContainerMario.Panel1
            // 
            this.splitContainerMario.Panel1.AutoScroll = true;
            this.splitContainerMario.Panel1.Controls.Add(this.groupBoxMarioStats);
            this.splitContainerMario.Panel1.Controls.Add(this.groupBoxMarioHOLP);
            this.splitContainerMario.Panel1.Controls.Add(this.panelMarioBorder);
            this.splitContainerMario.Panel1.Controls.Add(this.groupBoxMarioPos);
            this.splitContainerMario.Panel1.Controls.Add(this.buttonMarioToggleHandsfree);
            this.splitContainerMario.Panel1.Controls.Add(this.buttonMarioVisibility);
            this.splitContainerMario.Panel1MinSize = 0;
            // 
            // splitContainerMario.Panel2
            // 
            this.splitContainerMario.Panel2.Controls.Add(this.NoTearFlowLayoutPanelMario);
            this.splitContainerMario.Panel2MinSize = 0;
            this.splitContainerMario.Size = new System.Drawing.Size(915, 463);
            this.splitContainerMario.SplitterDistance = 208;
            this.splitContainerMario.SplitterWidth = 1;
            this.splitContainerMario.TabIndex = 31;
            // 
            // groupBoxMarioStats
            // 
            this.groupBoxMarioStats.Controls.Add(this.textBoxMarioStatsVspd);
            this.groupBoxMarioStats.Controls.Add(this.textBoxMarioStatsHspd);
            this.groupBoxMarioStats.Controls.Add(this.textBoxMarioStatsYaw);
            this.groupBoxMarioStats.Controls.Add(this.buttonMarioStatsVspdN);
            this.groupBoxMarioStats.Controls.Add(this.buttonMarioStatsHspdN);
            this.groupBoxMarioStats.Controls.Add(this.buttonMarioStatsYawN);
            this.groupBoxMarioStats.Controls.Add(this.buttonMarioStatsVspdP);
            this.groupBoxMarioStats.Controls.Add(this.buttonMarioStatsHspdP);
            this.groupBoxMarioStats.Controls.Add(this.buttonMarioStatsYawP);
            this.groupBoxMarioStats.Location = new System.Drawing.Point(2, 263);
            this.groupBoxMarioStats.Name = "groupBoxMarioStats";
            this.groupBoxMarioStats.Size = new System.Drawing.Size(185, 95);
            this.groupBoxMarioStats.TabIndex = 30;
            this.groupBoxMarioStats.TabStop = false;
            this.groupBoxMarioStats.Text = "Stats";
            // 
            // textBoxMarioStatsVspd
            // 
            this.textBoxMarioStatsVspd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMarioStatsVspd.Location = new System.Drawing.Point(67, 69);
            this.textBoxMarioStatsVspd.Name = "textBoxMarioStatsVspd";
            this.textBoxMarioStatsVspd.Size = new System.Drawing.Size(51, 20);
            this.textBoxMarioStatsVspd.TabIndex = 33;
            this.textBoxMarioStatsVspd.Text = "100";
            this.textBoxMarioStatsVspd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxMarioStatsHspd
            // 
            this.textBoxMarioStatsHspd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMarioStatsHspd.Location = new System.Drawing.Point(67, 44);
            this.textBoxMarioStatsHspd.Name = "textBoxMarioStatsHspd";
            this.textBoxMarioStatsHspd.Size = new System.Drawing.Size(51, 20);
            this.textBoxMarioStatsHspd.TabIndex = 33;
            this.textBoxMarioStatsHspd.Text = "100";
            this.textBoxMarioStatsHspd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxMarioStatsYaw
            // 
            this.textBoxMarioStatsYaw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMarioStatsYaw.Location = new System.Drawing.Point(67, 19);
            this.textBoxMarioStatsYaw.Name = "textBoxMarioStatsYaw";
            this.textBoxMarioStatsYaw.Size = new System.Drawing.Size(51, 20);
            this.textBoxMarioStatsYaw.TabIndex = 33;
            this.textBoxMarioStatsYaw.Text = "1024";
            this.textBoxMarioStatsYaw.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonMarioStatsVspdN
            // 
            this.buttonMarioStatsVspdN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMarioStatsVspdN.Location = new System.Drawing.Point(3, 66);
            this.buttonMarioStatsVspdN.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMarioStatsVspdN.Name = "buttonMarioStatsVspdN";
            this.buttonMarioStatsVspdN.Size = new System.Drawing.Size(61, 25);
            this.buttonMarioStatsVspdN.TabIndex = 35;
            this.buttonMarioStatsVspdN.Text = "Vspd-";
            this.buttonMarioStatsVspdN.UseVisualStyleBackColor = true;
            // 
            // buttonMarioStatsHspdN
            // 
            this.buttonMarioStatsHspdN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMarioStatsHspdN.Location = new System.Drawing.Point(3, 41);
            this.buttonMarioStatsHspdN.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMarioStatsHspdN.Name = "buttonMarioStatsHspdN";
            this.buttonMarioStatsHspdN.Size = new System.Drawing.Size(61, 25);
            this.buttonMarioStatsHspdN.TabIndex = 35;
            this.buttonMarioStatsHspdN.Text = "Hspd-";
            this.buttonMarioStatsHspdN.UseVisualStyleBackColor = true;
            // 
            // buttonMarioStatsYawN
            // 
            this.buttonMarioStatsYawN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMarioStatsYawN.Location = new System.Drawing.Point(3, 16);
            this.buttonMarioStatsYawN.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMarioStatsYawN.Name = "buttonMarioStatsYawN";
            this.buttonMarioStatsYawN.Size = new System.Drawing.Size(61, 25);
            this.buttonMarioStatsYawN.TabIndex = 35;
            this.buttonMarioStatsYawN.Text = "Yaw-";
            this.buttonMarioStatsYawN.UseVisualStyleBackColor = true;
            // 
            // buttonMarioStatsVspdP
            // 
            this.buttonMarioStatsVspdP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMarioStatsVspdP.Location = new System.Drawing.Point(121, 66);
            this.buttonMarioStatsVspdP.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMarioStatsVspdP.Name = "buttonMarioStatsVspdP";
            this.buttonMarioStatsVspdP.Size = new System.Drawing.Size(61, 25);
            this.buttonMarioStatsVspdP.TabIndex = 35;
            this.buttonMarioStatsVspdP.Text = "Vspd+";
            this.buttonMarioStatsVspdP.UseVisualStyleBackColor = true;
            // 
            // buttonMarioStatsHspdP
            // 
            this.buttonMarioStatsHspdP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMarioStatsHspdP.Location = new System.Drawing.Point(121, 41);
            this.buttonMarioStatsHspdP.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMarioStatsHspdP.Name = "buttonMarioStatsHspdP";
            this.buttonMarioStatsHspdP.Size = new System.Drawing.Size(61, 25);
            this.buttonMarioStatsHspdP.TabIndex = 35;
            this.buttonMarioStatsHspdP.Text = "Hspd+";
            this.buttonMarioStatsHspdP.UseVisualStyleBackColor = true;
            // 
            // buttonMarioStatsYawP
            // 
            this.buttonMarioStatsYawP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMarioStatsYawP.Location = new System.Drawing.Point(121, 16);
            this.buttonMarioStatsYawP.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMarioStatsYawP.Name = "buttonMarioStatsYawP";
            this.buttonMarioStatsYawP.Size = new System.Drawing.Size(61, 25);
            this.buttonMarioStatsYawP.TabIndex = 35;
            this.buttonMarioStatsYawP.Text = "Yaw+";
            this.buttonMarioStatsYawP.UseVisualStyleBackColor = true;
            // 
            // groupBoxMarioHOLP
            // 
            this.groupBoxMarioHOLP.Controls.Add(this.checkBoxMarioHOLPRelative);
            this.groupBoxMarioHOLP.Controls.Add(this.textBoxMarioHOLPY);
            this.groupBoxMarioHOLP.Controls.Add(this.buttonMarioHOLPYp);
            this.groupBoxMarioHOLP.Controls.Add(this.buttonMarioHOLPYn);
            this.groupBoxMarioHOLP.Controls.Add(this.buttonMarioHOLPXpZp);
            this.groupBoxMarioHOLP.Controls.Add(this.textBoxMarioHOLPXZ);
            this.groupBoxMarioHOLP.Controls.Add(this.buttonMarioHOLPXp);
            this.groupBoxMarioHOLP.Controls.Add(this.buttonMarioHOLPXpZn);
            this.groupBoxMarioHOLP.Controls.Add(this.buttonMarioHOLPZn);
            this.groupBoxMarioHOLP.Controls.Add(this.buttonMarioHOLPZp);
            this.groupBoxMarioHOLP.Controls.Add(this.buttonMarioHOLPXnZp);
            this.groupBoxMarioHOLP.Controls.Add(this.buttonMarioHOLPXn);
            this.groupBoxMarioHOLP.Controls.Add(this.buttonMarioHOLPXnZn);
            this.groupBoxMarioHOLP.Location = new System.Drawing.Point(2, 364);
            this.groupBoxMarioHOLP.Name = "groupBoxMarioHOLP";
            this.groupBoxMarioHOLP.Size = new System.Drawing.Size(185, 146);
            this.groupBoxMarioHOLP.TabIndex = 29;
            this.groupBoxMarioHOLP.TabStop = false;
            this.groupBoxMarioHOLP.Text = "HOLP";
            // 
            // checkBoxMarioHOLPRelative
            // 
            this.checkBoxMarioHOLPRelative.AutoSize = true;
            this.checkBoxMarioHOLPRelative.Location = new System.Drawing.Point(120, 0);
            this.checkBoxMarioHOLPRelative.Name = "checkBoxMarioHOLPRelative";
            this.checkBoxMarioHOLPRelative.Size = new System.Drawing.Size(65, 17);
            this.checkBoxMarioHOLPRelative.TabIndex = 37;
            this.checkBoxMarioHOLPRelative.Text = "Relative";
            this.checkBoxMarioHOLPRelative.UseVisualStyleBackColor = true;
            // 
            // textBoxMarioHOLPY
            // 
            this.textBoxMarioHOLPY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMarioHOLPY.Location = new System.Drawing.Point(140, 70);
            this.textBoxMarioHOLPY.Name = "textBoxMarioHOLPY";
            this.textBoxMarioHOLPY.Size = new System.Drawing.Size(42, 20);
            this.textBoxMarioHOLPY.TabIndex = 33;
            this.textBoxMarioHOLPY.Text = "100";
            this.textBoxMarioHOLPY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonMarioHOLPYp
            // 
            this.buttonMarioHOLPYp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMarioHOLPYp.Location = new System.Drawing.Point(140, 16);
            this.buttonMarioHOLPYp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMarioHOLPYp.Name = "buttonMarioHOLPYp";
            this.buttonMarioHOLPYp.Size = new System.Drawing.Size(42, 42);
            this.buttonMarioHOLPYp.TabIndex = 35;
            this.buttonMarioHOLPYp.Text = "Y+";
            this.buttonMarioHOLPYp.UseVisualStyleBackColor = true;
            // 
            // buttonMarioHOLPYn
            // 
            this.buttonMarioHOLPYn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMarioHOLPYn.Location = new System.Drawing.Point(140, 100);
            this.buttonMarioHOLPYn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMarioHOLPYn.Name = "buttonMarioHOLPYn";
            this.buttonMarioHOLPYn.Size = new System.Drawing.Size(42, 42);
            this.buttonMarioHOLPYn.TabIndex = 34;
            this.buttonMarioHOLPYn.Text = "Y-";
            this.buttonMarioHOLPYn.UseVisualStyleBackColor = true;
            // 
            // buttonMarioHOLPXpZp
            // 
            this.buttonMarioHOLPXpZp.Location = new System.Drawing.Point(87, 100);
            this.buttonMarioHOLPXpZp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMarioHOLPXpZp.Name = "buttonMarioHOLPXpZp";
            this.buttonMarioHOLPXpZp.Size = new System.Drawing.Size(42, 42);
            this.buttonMarioHOLPXpZp.TabIndex = 32;
            this.buttonMarioHOLPXpZp.Text = "X+Z+";
            this.buttonMarioHOLPXpZp.UseVisualStyleBackColor = true;
            // 
            // textBoxMarioHOLPXZ
            // 
            this.textBoxMarioHOLPXZ.Location = new System.Drawing.Point(45, 70);
            this.textBoxMarioHOLPXZ.Name = "textBoxMarioHOLPXZ";
            this.textBoxMarioHOLPXZ.Size = new System.Drawing.Size(42, 20);
            this.textBoxMarioHOLPXZ.TabIndex = 27;
            this.textBoxMarioHOLPXZ.Text = "100";
            this.textBoxMarioHOLPXZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonMarioHOLPXp
            // 
            this.buttonMarioHOLPXp.Location = new System.Drawing.Point(87, 58);
            this.buttonMarioHOLPXp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMarioHOLPXp.Name = "buttonMarioHOLPXp";
            this.buttonMarioHOLPXp.Size = new System.Drawing.Size(42, 42);
            this.buttonMarioHOLPXp.TabIndex = 31;
            this.buttonMarioHOLPXp.Text = "X+";
            this.buttonMarioHOLPXp.UseVisualStyleBackColor = true;
            // 
            // buttonMarioHOLPXpZn
            // 
            this.buttonMarioHOLPXpZn.Location = new System.Drawing.Point(87, 16);
            this.buttonMarioHOLPXpZn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMarioHOLPXpZn.Name = "buttonMarioHOLPXpZn";
            this.buttonMarioHOLPXpZn.Size = new System.Drawing.Size(42, 42);
            this.buttonMarioHOLPXpZn.TabIndex = 30;
            this.buttonMarioHOLPXpZn.Text = "X+Z-";
            this.buttonMarioHOLPXpZn.UseVisualStyleBackColor = true;
            // 
            // buttonMarioHOLPZn
            // 
            this.buttonMarioHOLPZn.Location = new System.Drawing.Point(45, 16);
            this.buttonMarioHOLPZn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMarioHOLPZn.Name = "buttonMarioHOLPZn";
            this.buttonMarioHOLPZn.Size = new System.Drawing.Size(42, 42);
            this.buttonMarioHOLPZn.TabIndex = 29;
            this.buttonMarioHOLPZn.Text = "Z-";
            this.buttonMarioHOLPZn.UseVisualStyleBackColor = true;
            // 
            // buttonMarioHOLPZp
            // 
            this.buttonMarioHOLPZp.Location = new System.Drawing.Point(45, 100);
            this.buttonMarioHOLPZp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMarioHOLPZp.Name = "buttonMarioHOLPZp";
            this.buttonMarioHOLPZp.Size = new System.Drawing.Size(42, 42);
            this.buttonMarioHOLPZp.TabIndex = 28;
            this.buttonMarioHOLPZp.Text = "Z+";
            this.buttonMarioHOLPZp.UseVisualStyleBackColor = true;
            // 
            // buttonMarioHOLPXnZp
            // 
            this.buttonMarioHOLPXnZp.Location = new System.Drawing.Point(3, 100);
            this.buttonMarioHOLPXnZp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMarioHOLPXnZp.Name = "buttonMarioHOLPXnZp";
            this.buttonMarioHOLPXnZp.Size = new System.Drawing.Size(42, 42);
            this.buttonMarioHOLPXnZp.TabIndex = 27;
            this.buttonMarioHOLPXnZp.Text = "X-Z+";
            this.buttonMarioHOLPXnZp.UseVisualStyleBackColor = true;
            // 
            // buttonMarioHOLPXn
            // 
            this.buttonMarioHOLPXn.Location = new System.Drawing.Point(3, 58);
            this.buttonMarioHOLPXn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMarioHOLPXn.Name = "buttonMarioHOLPXn";
            this.buttonMarioHOLPXn.Size = new System.Drawing.Size(42, 42);
            this.buttonMarioHOLPXn.TabIndex = 26;
            this.buttonMarioHOLPXn.Text = "X-";
            this.buttonMarioHOLPXn.UseVisualStyleBackColor = true;
            // 
            // buttonMarioHOLPXnZn
            // 
            this.buttonMarioHOLPXnZn.Location = new System.Drawing.Point(3, 16);
            this.buttonMarioHOLPXnZn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMarioHOLPXnZn.Name = "buttonMarioHOLPXnZn";
            this.buttonMarioHOLPXnZn.Size = new System.Drawing.Size(42, 42);
            this.buttonMarioHOLPXnZn.TabIndex = 25;
            this.buttonMarioHOLPXnZn.Text = "X-Z-";
            this.buttonMarioHOLPXnZn.UseVisualStyleBackColor = true;
            // 
            // panelMarioBorder
            // 
            this.panelMarioBorder.Controls.Add(this.pictureBoxMario);
            this.panelMarioBorder.Location = new System.Drawing.Point(3, 3);
            this.panelMarioBorder.Margin = new System.Windows.Forms.Padding(2);
            this.panelMarioBorder.Name = "panelMarioBorder";
            this.panelMarioBorder.Size = new System.Drawing.Size(57, 55);
            this.panelMarioBorder.TabIndex = 0;
            // 
            // groupBoxMarioPos
            // 
            this.groupBoxMarioPos.Controls.Add(this.checkBoxMarioPosRelative);
            this.groupBoxMarioPos.Controls.Add(this.textBoxMarioPosY);
            this.groupBoxMarioPos.Controls.Add(this.buttonMarioPosYp);
            this.groupBoxMarioPos.Controls.Add(this.buttonMarioPosYn);
            this.groupBoxMarioPos.Controls.Add(this.buttonMarioPosXpZp);
            this.groupBoxMarioPos.Controls.Add(this.textBoxMarioPosXZ);
            this.groupBoxMarioPos.Controls.Add(this.buttonMarioPosXp);
            this.groupBoxMarioPos.Controls.Add(this.buttonMarioPosXpZn);
            this.groupBoxMarioPos.Controls.Add(this.buttonMarioPosZn);
            this.groupBoxMarioPos.Controls.Add(this.buttonMarioPosZp);
            this.groupBoxMarioPos.Controls.Add(this.buttonMarioPosXnZp);
            this.groupBoxMarioPos.Controls.Add(this.buttonMarioPosXn);
            this.groupBoxMarioPos.Controls.Add(this.buttonMarioPosXnZn);
            this.groupBoxMarioPos.Location = new System.Drawing.Point(2, 111);
            this.groupBoxMarioPos.Name = "groupBoxMarioPos";
            this.groupBoxMarioPos.Size = new System.Drawing.Size(185, 146);
            this.groupBoxMarioPos.TabIndex = 29;
            this.groupBoxMarioPos.TabStop = false;
            this.groupBoxMarioPos.Text = "Position";
            // 
            // checkBoxMarioPosRelative
            // 
            this.checkBoxMarioPosRelative.AutoSize = true;
            this.checkBoxMarioPosRelative.Location = new System.Drawing.Point(120, 0);
            this.checkBoxMarioPosRelative.Name = "checkBoxMarioPosRelative";
            this.checkBoxMarioPosRelative.Size = new System.Drawing.Size(65, 17);
            this.checkBoxMarioPosRelative.TabIndex = 36;
            this.checkBoxMarioPosRelative.Text = "Relative";
            this.checkBoxMarioPosRelative.UseVisualStyleBackColor = true;
            // 
            // textBoxMarioPosY
            // 
            this.textBoxMarioPosY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMarioPosY.Location = new System.Drawing.Point(140, 70);
            this.textBoxMarioPosY.Name = "textBoxMarioPosY";
            this.textBoxMarioPosY.Size = new System.Drawing.Size(42, 20);
            this.textBoxMarioPosY.TabIndex = 33;
            this.textBoxMarioPosY.Text = "100";
            this.textBoxMarioPosY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonMarioPosYp
            // 
            this.buttonMarioPosYp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMarioPosYp.Location = new System.Drawing.Point(140, 16);
            this.buttonMarioPosYp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMarioPosYp.Name = "buttonMarioPosYp";
            this.buttonMarioPosYp.Size = new System.Drawing.Size(42, 42);
            this.buttonMarioPosYp.TabIndex = 35;
            this.buttonMarioPosYp.Text = "Y+";
            this.buttonMarioPosYp.UseVisualStyleBackColor = true;
            // 
            // buttonMarioPosYn
            // 
            this.buttonMarioPosYn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMarioPosYn.Location = new System.Drawing.Point(140, 100);
            this.buttonMarioPosYn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMarioPosYn.Name = "buttonMarioPosYn";
            this.buttonMarioPosYn.Size = new System.Drawing.Size(42, 42);
            this.buttonMarioPosYn.TabIndex = 34;
            this.buttonMarioPosYn.Text = "Y-";
            this.buttonMarioPosYn.UseVisualStyleBackColor = true;
            // 
            // buttonMarioPosXpZp
            // 
            this.buttonMarioPosXpZp.Location = new System.Drawing.Point(87, 100);
            this.buttonMarioPosXpZp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMarioPosXpZp.Name = "buttonMarioPosXpZp";
            this.buttonMarioPosXpZp.Size = new System.Drawing.Size(42, 42);
            this.buttonMarioPosXpZp.TabIndex = 32;
            this.buttonMarioPosXpZp.Text = "X+Z+";
            this.buttonMarioPosXpZp.UseVisualStyleBackColor = true;
            // 
            // textBoxMarioPosXZ
            // 
            this.textBoxMarioPosXZ.Location = new System.Drawing.Point(45, 70);
            this.textBoxMarioPosXZ.Name = "textBoxMarioPosXZ";
            this.textBoxMarioPosXZ.Size = new System.Drawing.Size(42, 20);
            this.textBoxMarioPosXZ.TabIndex = 27;
            this.textBoxMarioPosXZ.Text = "100";
            this.textBoxMarioPosXZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonMarioPosXp
            // 
            this.buttonMarioPosXp.Location = new System.Drawing.Point(87, 58);
            this.buttonMarioPosXp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMarioPosXp.Name = "buttonMarioPosXp";
            this.buttonMarioPosXp.Size = new System.Drawing.Size(42, 42);
            this.buttonMarioPosXp.TabIndex = 31;
            this.buttonMarioPosXp.Text = "X+";
            this.buttonMarioPosXp.UseVisualStyleBackColor = true;
            // 
            // buttonMarioPosXpZn
            // 
            this.buttonMarioPosXpZn.Location = new System.Drawing.Point(87, 16);
            this.buttonMarioPosXpZn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMarioPosXpZn.Name = "buttonMarioPosXpZn";
            this.buttonMarioPosXpZn.Size = new System.Drawing.Size(42, 42);
            this.buttonMarioPosXpZn.TabIndex = 30;
            this.buttonMarioPosXpZn.Text = "X+Z-";
            this.buttonMarioPosXpZn.UseVisualStyleBackColor = true;
            // 
            // buttonMarioPosZn
            // 
            this.buttonMarioPosZn.Location = new System.Drawing.Point(45, 16);
            this.buttonMarioPosZn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMarioPosZn.Name = "buttonMarioPosZn";
            this.buttonMarioPosZn.Size = new System.Drawing.Size(42, 42);
            this.buttonMarioPosZn.TabIndex = 29;
            this.buttonMarioPosZn.Text = "Z-";
            this.buttonMarioPosZn.UseVisualStyleBackColor = true;
            // 
            // buttonMarioPosZp
            // 
            this.buttonMarioPosZp.Location = new System.Drawing.Point(45, 100);
            this.buttonMarioPosZp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMarioPosZp.Name = "buttonMarioPosZp";
            this.buttonMarioPosZp.Size = new System.Drawing.Size(42, 42);
            this.buttonMarioPosZp.TabIndex = 28;
            this.buttonMarioPosZp.Text = "Z+";
            this.buttonMarioPosZp.UseVisualStyleBackColor = true;
            // 
            // buttonMarioPosXnZp
            // 
            this.buttonMarioPosXnZp.Location = new System.Drawing.Point(3, 100);
            this.buttonMarioPosXnZp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMarioPosXnZp.Name = "buttonMarioPosXnZp";
            this.buttonMarioPosXnZp.Size = new System.Drawing.Size(42, 42);
            this.buttonMarioPosXnZp.TabIndex = 27;
            this.buttonMarioPosXnZp.Text = "X-Z+";
            this.buttonMarioPosXnZp.UseVisualStyleBackColor = true;
            // 
            // buttonMarioPosXn
            // 
            this.buttonMarioPosXn.Location = new System.Drawing.Point(3, 58);
            this.buttonMarioPosXn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMarioPosXn.Name = "buttonMarioPosXn";
            this.buttonMarioPosXn.Size = new System.Drawing.Size(42, 42);
            this.buttonMarioPosXn.TabIndex = 26;
            this.buttonMarioPosXn.Text = "X-";
            this.buttonMarioPosXn.UseVisualStyleBackColor = true;
            // 
            // buttonMarioPosXnZn
            // 
            this.buttonMarioPosXnZn.Location = new System.Drawing.Point(3, 16);
            this.buttonMarioPosXnZn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMarioPosXnZn.Name = "buttonMarioPosXnZn";
            this.buttonMarioPosXnZn.Size = new System.Drawing.Size(42, 42);
            this.buttonMarioPosXnZn.TabIndex = 25;
            this.buttonMarioPosXnZn.Text = "X-Z-";
            this.buttonMarioPosXnZn.UseVisualStyleBackColor = true;
            // 
            // buttonMarioToggleHandsfree
            // 
            this.buttonMarioToggleHandsfree.Location = new System.Drawing.Point(95, 63);
            this.buttonMarioToggleHandsfree.Name = "buttonMarioToggleHandsfree";
            this.buttonMarioToggleHandsfree.Size = new System.Drawing.Size(92, 42);
            this.buttonMarioToggleHandsfree.TabIndex = 2;
            this.buttonMarioToggleHandsfree.Text = "Toggle Handsfree";
            this.buttonMarioToggleHandsfree.UseVisualStyleBackColor = true;
            // 
            // buttonMarioVisibility
            // 
            this.buttonMarioVisibility.Location = new System.Drawing.Point(2, 63);
            this.buttonMarioVisibility.Name = "buttonMarioVisibility";
            this.buttonMarioVisibility.Size = new System.Drawing.Size(92, 42);
            this.buttonMarioVisibility.TabIndex = 3;
            this.buttonMarioVisibility.Text = "Toggle Visibility";
            this.buttonMarioVisibility.UseVisualStyleBackColor = true;
            // 
            // tabPageActions
            // 
            this.tabPageActions.Controls.Add(this.noTearFlowLayoutPanelActions);
            this.tabPageActions.Location = new System.Drawing.Point(4, 22);
            this.tabPageActions.Name = "tabPageActions";
            this.tabPageActions.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageActions.Size = new System.Drawing.Size(915, 463);
            this.tabPageActions.TabIndex = 13;
            this.tabPageActions.Text = "Actions";
            // 
            // tabPageHud
            // 
            this.tabPageHud.Controls.Add(this.splitContainerHud);
            this.tabPageHud.Location = new System.Drawing.Point(4, 22);
            this.tabPageHud.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageHud.Name = "tabPageHud";
            this.tabPageHud.Size = new System.Drawing.Size(915, 463);
            this.tabPageHud.TabIndex = 6;
            this.tabPageHud.Text = "HUD";
            // 
            // splitContainerHud
            // 
            this.splitContainerHud.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerHud.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerHud.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerHud.Location = new System.Drawing.Point(0, 0);
            this.splitContainerHud.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainerHud.Name = "splitContainerHud";
            // 
            // splitContainerHud.Panel1
            // 
            this.splitContainerHud.Panel1.AutoScroll = true;
            this.splitContainerHud.Panel1.Controls.Add(this.buttonStandardHud);
            this.splitContainerHud.Panel1.Controls.Add(this.panelHudBorder);
            this.splitContainerHud.Panel1.Controls.Add(this.buttonDie);
            this.splitContainerHud.Panel1.Controls.Add(this.buttonFillHp);
            this.splitContainerHud.Panel1MinSize = 0;
            // 
            // splitContainerHud.Panel2
            // 
            this.splitContainerHud.Panel2.Controls.Add(this.NoTearFlowLayoutPanelHud);
            this.splitContainerHud.Panel2MinSize = 0;
            this.splitContainerHud.Size = new System.Drawing.Size(915, 463);
            this.splitContainerHud.SplitterDistance = 117;
            this.splitContainerHud.SplitterWidth = 1;
            this.splitContainerHud.TabIndex = 20;
            // 
            // buttonStandardHud
            // 
            this.buttonStandardHud.Location = new System.Drawing.Point(3, 133);
            this.buttonStandardHud.Name = "buttonStandardHud";
            this.buttonStandardHud.Size = new System.Drawing.Size(93, 24);
            this.buttonStandardHud.TabIndex = 6;
            this.buttonStandardHud.Text = "Standard HUD";
            this.buttonStandardHud.UseVisualStyleBackColor = true;
            // 
            // panelHudBorder
            // 
            this.panelHudBorder.Controls.Add(this.pictureBoxHud);
            this.panelHudBorder.Location = new System.Drawing.Point(2, 2);
            this.panelHudBorder.Margin = new System.Windows.Forms.Padding(2);
            this.panelHudBorder.Name = "panelHudBorder";
            this.panelHudBorder.Size = new System.Drawing.Size(57, 55);
            this.panelHudBorder.TabIndex = 2;
            // 
            // buttonDie
            // 
            this.buttonDie.Location = new System.Drawing.Point(3, 104);
            this.buttonDie.Name = "buttonDie";
            this.buttonDie.Size = new System.Drawing.Size(93, 23);
            this.buttonDie.TabIndex = 5;
            this.buttonDie.Text = "Die";
            this.buttonDie.UseVisualStyleBackColor = true;
            // 
            // buttonFillHp
            // 
            this.buttonFillHp.Location = new System.Drawing.Point(3, 75);
            this.buttonFillHp.Name = "buttonFillHp";
            this.buttonFillHp.Size = new System.Drawing.Size(93, 23);
            this.buttonFillHp.TabIndex = 4;
            this.buttonFillHp.Text = "Fill HP";
            this.buttonFillHp.UseVisualStyleBackColor = true;
            // 
            // tabPageCamera
            // 
            this.tabPageCamera.Controls.Add(this.SplitContainerCamera);
            this.tabPageCamera.Location = new System.Drawing.Point(4, 22);
            this.tabPageCamera.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageCamera.Name = "tabPageCamera";
            this.tabPageCamera.Size = new System.Drawing.Size(915, 463);
            this.tabPageCamera.TabIndex = 7;
            this.tabPageCamera.Text = "Camera";
            // 
            // SplitContainerCamera
            // 
            this.SplitContainerCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SplitContainerCamera.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SplitContainerCamera.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.SplitContainerCamera.Location = new System.Drawing.Point(0, 0);
            this.SplitContainerCamera.Margin = new System.Windows.Forms.Padding(2);
            this.SplitContainerCamera.Name = "SplitContainerCamera";
            // 
            // SplitContainerCamera.Panel1
            // 
            this.SplitContainerCamera.Panel1.AutoScroll = true;
            this.SplitContainerCamera.Panel1.Controls.Add(this.groupBoxCameraSphericalPos);
            this.SplitContainerCamera.Panel1.Controls.Add(this.panelCameraBorder);
            this.SplitContainerCamera.Panel1.Controls.Add(this.groupBoxCameraPos);
            this.SplitContainerCamera.Panel1MinSize = 0;
            // 
            // SplitContainerCamera.Panel2
            // 
            this.SplitContainerCamera.Panel2.Controls.Add(this.NoTearFlowLayoutPanelCamera);
            this.SplitContainerCamera.Panel2MinSize = 0;
            this.SplitContainerCamera.Size = new System.Drawing.Size(915, 463);
            this.SplitContainerCamera.SplitterDistance = 207;
            this.SplitContainerCamera.SplitterWidth = 1;
            this.SplitContainerCamera.TabIndex = 31;
            // 
            // groupBoxCameraSphericalPos
            // 
            this.groupBoxCameraSphericalPos.Controls.Add(this.checkBoxCameraSphericalPosPivotOnFocus);
            this.groupBoxCameraSphericalPos.Controls.Add(this.textBoxCameraSphericalPosR);
            this.groupBoxCameraSphericalPos.Controls.Add(this.buttonCameraSphericalPosRn);
            this.groupBoxCameraSphericalPos.Controls.Add(this.buttonCameraSphericalPosRp);
            this.groupBoxCameraSphericalPos.Controls.Add(this.buttonCameraSphericalPosTpPp);
            this.groupBoxCameraSphericalPos.Controls.Add(this.textBoxCameraSphericalPosTP);
            this.groupBoxCameraSphericalPos.Controls.Add(this.buttonCameraSphericalPosTp);
            this.groupBoxCameraSphericalPos.Controls.Add(this.buttonCameraSphericalPosTpPn);
            this.groupBoxCameraSphericalPos.Controls.Add(this.buttonCameraSphericalPosPn);
            this.groupBoxCameraSphericalPos.Controls.Add(this.buttonCameraSphericalPosPp);
            this.groupBoxCameraSphericalPos.Controls.Add(this.buttonCameraSphericalPosTnPp);
            this.groupBoxCameraSphericalPos.Controls.Add(this.buttonCameraSphericalPosTn);
            this.groupBoxCameraSphericalPos.Controls.Add(this.buttonCameraSphericalPosTnPn);
            this.groupBoxCameraSphericalPos.Location = new System.Drawing.Point(2, 214);
            this.groupBoxCameraSphericalPos.Name = "groupBoxCameraSphericalPos";
            this.groupBoxCameraSphericalPos.Size = new System.Drawing.Size(185, 146);
            this.groupBoxCameraSphericalPos.TabIndex = 30;
            this.groupBoxCameraSphericalPos.TabStop = false;
            this.groupBoxCameraSphericalPos.Text = "Spherical";
            // 
            // checkBoxCameraSphericalPosPivotOnFocus
            // 
            this.checkBoxCameraSphericalPosPivotOnFocus.AutoSize = true;
            this.checkBoxCameraSphericalPosPivotOnFocus.Location = new System.Drawing.Point(91, 0);
            this.checkBoxCameraSphericalPosPivotOnFocus.Name = "checkBoxCameraSphericalPosPivotOnFocus";
            this.checkBoxCameraSphericalPosPivotOnFocus.Size = new System.Drawing.Size(97, 17);
            this.checkBoxCameraSphericalPosPivotOnFocus.TabIndex = 38;
            this.checkBoxCameraSphericalPosPivotOnFocus.Text = "Pivot on Focus";
            this.checkBoxCameraSphericalPosPivotOnFocus.UseVisualStyleBackColor = true;
            // 
            // textBoxCameraSphericalPosR
            // 
            this.textBoxCameraSphericalPosR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCameraSphericalPosR.Location = new System.Drawing.Point(140, 70);
            this.textBoxCameraSphericalPosR.Name = "textBoxCameraSphericalPosR";
            this.textBoxCameraSphericalPosR.Size = new System.Drawing.Size(42, 20);
            this.textBoxCameraSphericalPosR.TabIndex = 33;
            this.textBoxCameraSphericalPosR.Text = "100";
            this.textBoxCameraSphericalPosR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonCameraSphericalPosRn
            // 
            this.buttonCameraSphericalPosRn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCameraSphericalPosRn.Location = new System.Drawing.Point(140, 16);
            this.buttonCameraSphericalPosRn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraSphericalPosRn.Name = "buttonCameraSphericalPosRn";
            this.buttonCameraSphericalPosRn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraSphericalPosRn.TabIndex = 35;
            this.buttonCameraSphericalPosRn.Text = "R-";
            this.buttonCameraSphericalPosRn.UseVisualStyleBackColor = true;
            // 
            // buttonCameraSphericalPosRp
            // 
            this.buttonCameraSphericalPosRp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCameraSphericalPosRp.Location = new System.Drawing.Point(140, 100);
            this.buttonCameraSphericalPosRp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraSphericalPosRp.Name = "buttonCameraSphericalPosRp";
            this.buttonCameraSphericalPosRp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraSphericalPosRp.TabIndex = 34;
            this.buttonCameraSphericalPosRp.Text = "R+";
            this.buttonCameraSphericalPosRp.UseVisualStyleBackColor = true;
            // 
            // buttonCameraSphericalPosTpPp
            // 
            this.buttonCameraSphericalPosTpPp.Location = new System.Drawing.Point(87, 100);
            this.buttonCameraSphericalPosTpPp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraSphericalPosTpPp.Name = "buttonCameraSphericalPosTpPp";
            this.buttonCameraSphericalPosTpPp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraSphericalPosTpPp.TabIndex = 32;
            this.buttonCameraSphericalPosTpPp.Text = "θ+ϕ+";
            this.buttonCameraSphericalPosTpPp.UseVisualStyleBackColor = true;
            // 
            // textBoxCameraSphericalPosTP
            // 
            this.textBoxCameraSphericalPosTP.Location = new System.Drawing.Point(45, 70);
            this.textBoxCameraSphericalPosTP.Name = "textBoxCameraSphericalPosTP";
            this.textBoxCameraSphericalPosTP.Size = new System.Drawing.Size(42, 20);
            this.textBoxCameraSphericalPosTP.TabIndex = 27;
            this.textBoxCameraSphericalPosTP.Text = "1024";
            this.textBoxCameraSphericalPosTP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonCameraSphericalPosTp
            // 
            this.buttonCameraSphericalPosTp.Location = new System.Drawing.Point(87, 58);
            this.buttonCameraSphericalPosTp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraSphericalPosTp.Name = "buttonCameraSphericalPosTp";
            this.buttonCameraSphericalPosTp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraSphericalPosTp.TabIndex = 31;
            this.buttonCameraSphericalPosTp.Text = "θ+";
            this.buttonCameraSphericalPosTp.UseVisualStyleBackColor = true;
            // 
            // buttonCameraSphericalPosTpPn
            // 
            this.buttonCameraSphericalPosTpPn.Location = new System.Drawing.Point(87, 16);
            this.buttonCameraSphericalPosTpPn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraSphericalPosTpPn.Name = "buttonCameraSphericalPosTpPn";
            this.buttonCameraSphericalPosTpPn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraSphericalPosTpPn.TabIndex = 30;
            this.buttonCameraSphericalPosTpPn.Text = "θ+ϕ-";
            this.buttonCameraSphericalPosTpPn.UseVisualStyleBackColor = true;
            // 
            // buttonCameraSphericalPosPn
            // 
            this.buttonCameraSphericalPosPn.Location = new System.Drawing.Point(45, 16);
            this.buttonCameraSphericalPosPn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraSphericalPosPn.Name = "buttonCameraSphericalPosPn";
            this.buttonCameraSphericalPosPn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraSphericalPosPn.TabIndex = 29;
            this.buttonCameraSphericalPosPn.Text = "ϕ-";
            this.buttonCameraSphericalPosPn.UseVisualStyleBackColor = true;
            // 
            // buttonCameraSphericalPosPp
            // 
            this.buttonCameraSphericalPosPp.Location = new System.Drawing.Point(45, 100);
            this.buttonCameraSphericalPosPp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraSphericalPosPp.Name = "buttonCameraSphericalPosPp";
            this.buttonCameraSphericalPosPp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraSphericalPosPp.TabIndex = 28;
            this.buttonCameraSphericalPosPp.Text = "ϕ+";
            this.buttonCameraSphericalPosPp.UseVisualStyleBackColor = true;
            // 
            // buttonCameraSphericalPosTnPp
            // 
            this.buttonCameraSphericalPosTnPp.Location = new System.Drawing.Point(3, 100);
            this.buttonCameraSphericalPosTnPp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraSphericalPosTnPp.Name = "buttonCameraSphericalPosTnPp";
            this.buttonCameraSphericalPosTnPp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraSphericalPosTnPp.TabIndex = 27;
            this.buttonCameraSphericalPosTnPp.Text = "θ-ϕ+";
            this.buttonCameraSphericalPosTnPp.UseVisualStyleBackColor = true;
            // 
            // buttonCameraSphericalPosTn
            // 
            this.buttonCameraSphericalPosTn.Location = new System.Drawing.Point(3, 58);
            this.buttonCameraSphericalPosTn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraSphericalPosTn.Name = "buttonCameraSphericalPosTn";
            this.buttonCameraSphericalPosTn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraSphericalPosTn.TabIndex = 26;
            this.buttonCameraSphericalPosTn.Text = "θ-";
            this.buttonCameraSphericalPosTn.UseVisualStyleBackColor = true;
            // 
            // buttonCameraSphericalPosTnPn
            // 
            this.buttonCameraSphericalPosTnPn.Location = new System.Drawing.Point(3, 16);
            this.buttonCameraSphericalPosTnPn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraSphericalPosTnPn.Name = "buttonCameraSphericalPosTnPn";
            this.buttonCameraSphericalPosTnPn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraSphericalPosTnPn.TabIndex = 25;
            this.buttonCameraSphericalPosTnPn.Text = "θ-ϕ-";
            this.buttonCameraSphericalPosTnPn.UseVisualStyleBackColor = true;
            // 
            // panelCameraBorder
            // 
            this.panelCameraBorder.Controls.Add(this.pictureBoxCamera);
            this.panelCameraBorder.Location = new System.Drawing.Point(2, 2);
            this.panelCameraBorder.Margin = new System.Windows.Forms.Padding(2);
            this.panelCameraBorder.Name = "panelCameraBorder";
            this.panelCameraBorder.Size = new System.Drawing.Size(57, 55);
            this.panelCameraBorder.TabIndex = 2;
            // 
            // groupBoxCameraPos
            // 
            this.groupBoxCameraPos.Controls.Add(this.checkBoxCameraPosRelative);
            this.groupBoxCameraPos.Controls.Add(this.textBoxCameraPosY);
            this.groupBoxCameraPos.Controls.Add(this.buttonCameraPosYp);
            this.groupBoxCameraPos.Controls.Add(this.buttonCameraPosYn);
            this.groupBoxCameraPos.Controls.Add(this.buttonCameraPosXpZp);
            this.groupBoxCameraPos.Controls.Add(this.textBoxCameraPosXZ);
            this.groupBoxCameraPos.Controls.Add(this.buttonCameraPosXp);
            this.groupBoxCameraPos.Controls.Add(this.buttonCameraPosXpZn);
            this.groupBoxCameraPos.Controls.Add(this.buttonCameraPosZn);
            this.groupBoxCameraPos.Controls.Add(this.buttonCameraPosZp);
            this.groupBoxCameraPos.Controls.Add(this.buttonCameraPosXnZp);
            this.groupBoxCameraPos.Controls.Add(this.buttonCameraPosXn);
            this.groupBoxCameraPos.Controls.Add(this.buttonCameraPosXnZn);
            this.groupBoxCameraPos.Location = new System.Drawing.Point(2, 62);
            this.groupBoxCameraPos.Name = "groupBoxCameraPos";
            this.groupBoxCameraPos.Size = new System.Drawing.Size(185, 146);
            this.groupBoxCameraPos.TabIndex = 30;
            this.groupBoxCameraPos.TabStop = false;
            this.groupBoxCameraPos.Text = "Position";
            // 
            // checkBoxCameraPosRelative
            // 
            this.checkBoxCameraPosRelative.AutoSize = true;
            this.checkBoxCameraPosRelative.Location = new System.Drawing.Point(120, 0);
            this.checkBoxCameraPosRelative.Name = "checkBoxCameraPosRelative";
            this.checkBoxCameraPosRelative.Size = new System.Drawing.Size(65, 17);
            this.checkBoxCameraPosRelative.TabIndex = 37;
            this.checkBoxCameraPosRelative.Text = "Relative";
            this.checkBoxCameraPosRelative.UseVisualStyleBackColor = true;
            // 
            // textBoxCameraPosY
            // 
            this.textBoxCameraPosY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCameraPosY.Location = new System.Drawing.Point(140, 70);
            this.textBoxCameraPosY.Name = "textBoxCameraPosY";
            this.textBoxCameraPosY.Size = new System.Drawing.Size(42, 20);
            this.textBoxCameraPosY.TabIndex = 33;
            this.textBoxCameraPosY.Text = "100";
            this.textBoxCameraPosY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonCameraPosYp
            // 
            this.buttonCameraPosYp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCameraPosYp.Location = new System.Drawing.Point(140, 16);
            this.buttonCameraPosYp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraPosYp.Name = "buttonCameraPosYp";
            this.buttonCameraPosYp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraPosYp.TabIndex = 35;
            this.buttonCameraPosYp.Text = "Y+";
            this.buttonCameraPosYp.UseVisualStyleBackColor = true;
            // 
            // buttonCameraPosYn
            // 
            this.buttonCameraPosYn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCameraPosYn.Location = new System.Drawing.Point(140, 100);
            this.buttonCameraPosYn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraPosYn.Name = "buttonCameraPosYn";
            this.buttonCameraPosYn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraPosYn.TabIndex = 34;
            this.buttonCameraPosYn.Text = "Y-";
            this.buttonCameraPosYn.UseVisualStyleBackColor = true;
            // 
            // buttonCameraPosXpZp
            // 
            this.buttonCameraPosXpZp.Location = new System.Drawing.Point(87, 100);
            this.buttonCameraPosXpZp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraPosXpZp.Name = "buttonCameraPosXpZp";
            this.buttonCameraPosXpZp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraPosXpZp.TabIndex = 32;
            this.buttonCameraPosXpZp.Text = "X+Z+";
            this.buttonCameraPosXpZp.UseVisualStyleBackColor = true;
            // 
            // textBoxCameraPosXZ
            // 
            this.textBoxCameraPosXZ.Location = new System.Drawing.Point(45, 70);
            this.textBoxCameraPosXZ.Name = "textBoxCameraPosXZ";
            this.textBoxCameraPosXZ.Size = new System.Drawing.Size(42, 20);
            this.textBoxCameraPosXZ.TabIndex = 27;
            this.textBoxCameraPosXZ.Text = "100";
            this.textBoxCameraPosXZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonCameraPosXp
            // 
            this.buttonCameraPosXp.Location = new System.Drawing.Point(87, 58);
            this.buttonCameraPosXp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraPosXp.Name = "buttonCameraPosXp";
            this.buttonCameraPosXp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraPosXp.TabIndex = 31;
            this.buttonCameraPosXp.Text = "X+";
            this.buttonCameraPosXp.UseVisualStyleBackColor = true;
            // 
            // buttonCameraPosXpZn
            // 
            this.buttonCameraPosXpZn.Location = new System.Drawing.Point(87, 16);
            this.buttonCameraPosXpZn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraPosXpZn.Name = "buttonCameraPosXpZn";
            this.buttonCameraPosXpZn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraPosXpZn.TabIndex = 30;
            this.buttonCameraPosXpZn.Text = "X+Z-";
            this.buttonCameraPosXpZn.UseVisualStyleBackColor = true;
            // 
            // buttonCameraPosZn
            // 
            this.buttonCameraPosZn.Location = new System.Drawing.Point(45, 16);
            this.buttonCameraPosZn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraPosZn.Name = "buttonCameraPosZn";
            this.buttonCameraPosZn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraPosZn.TabIndex = 29;
            this.buttonCameraPosZn.Text = "Z-";
            this.buttonCameraPosZn.UseVisualStyleBackColor = true;
            // 
            // buttonCameraPosZp
            // 
            this.buttonCameraPosZp.Location = new System.Drawing.Point(45, 100);
            this.buttonCameraPosZp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraPosZp.Name = "buttonCameraPosZp";
            this.buttonCameraPosZp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraPosZp.TabIndex = 28;
            this.buttonCameraPosZp.Text = "Z+";
            this.buttonCameraPosZp.UseVisualStyleBackColor = true;
            // 
            // buttonCameraPosXnZp
            // 
            this.buttonCameraPosXnZp.Location = new System.Drawing.Point(3, 100);
            this.buttonCameraPosXnZp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraPosXnZp.Name = "buttonCameraPosXnZp";
            this.buttonCameraPosXnZp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraPosXnZp.TabIndex = 27;
            this.buttonCameraPosXnZp.Text = "X-Z+";
            this.buttonCameraPosXnZp.UseVisualStyleBackColor = true;
            // 
            // buttonCameraPosXn
            // 
            this.buttonCameraPosXn.Location = new System.Drawing.Point(3, 58);
            this.buttonCameraPosXn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraPosXn.Name = "buttonCameraPosXn";
            this.buttonCameraPosXn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraPosXn.TabIndex = 26;
            this.buttonCameraPosXn.Text = "X-";
            this.buttonCameraPosXn.UseVisualStyleBackColor = true;
            // 
            // buttonCameraPosXnZn
            // 
            this.buttonCameraPosXnZn.Location = new System.Drawing.Point(3, 16);
            this.buttonCameraPosXnZn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraPosXnZn.Name = "buttonCameraPosXnZn";
            this.buttonCameraPosXnZn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraPosXnZn.TabIndex = 25;
            this.buttonCameraPosXnZn.Text = "X-Z-";
            this.buttonCameraPosXnZn.UseVisualStyleBackColor = true;
            // 
            // tabPageTriangles
            // 
            this.tabPageTriangles.Controls.Add(this.splitContainerTriangles);
            this.tabPageTriangles.Location = new System.Drawing.Point(4, 22);
            this.tabPageTriangles.Name = "tabPageTriangles";
            this.tabPageTriangles.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTriangles.Size = new System.Drawing.Size(915, 463);
            this.tabPageTriangles.TabIndex = 11;
            this.tabPageTriangles.Text = "Triangles";
            // 
            // splitContainerTriangles
            // 
            this.splitContainerTriangles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerTriangles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerTriangles.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerTriangles.Location = new System.Drawing.Point(0, 0);
            this.splitContainerTriangles.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainerTriangles.Name = "splitContainerTriangles";
            // 
            // splitContainerTriangles.Panel1
            // 
            this.splitContainerTriangles.Panel1.AutoScroll = true;
            this.splitContainerTriangles.Panel1.Controls.Add(this.groupBoxTrianglePos);
            this.splitContainerTriangles.Panel1.Controls.Add(this.groupBoxTriangleNormal);
            this.splitContainerTriangles.Panel1.Controls.Add(this.radioButtonTriOther);
            this.splitContainerTriangles.Panel1.Controls.Add(this.buttonAnnihilateTriangle);
            this.splitContainerTriangles.Panel1.Controls.Add(this.radioButtonTriFloor);
            this.splitContainerTriangles.Panel1.Controls.Add(this.buttonNeutralizeTriangle);
            this.splitContainerTriangles.Panel1.Controls.Add(this.radioButtonTriWall);
            this.splitContainerTriangles.Panel1.Controls.Add(this.buttonGoToVClosest);
            this.splitContainerTriangles.Panel1.Controls.Add(this.radioButtonTriCeiling);
            this.splitContainerTriangles.Panel1.Controls.Add(this.checkBoxVertexMisalignment);
            this.splitContainerTriangles.Panel1.Controls.Add(this.maskedTextBoxOtherTriangle);
            this.splitContainerTriangles.Panel1.Controls.Add(this.buttonRetrieveTriangle);
            this.splitContainerTriangles.Panel1.Controls.Add(this.labelTriangleSelection);
            this.splitContainerTriangles.Panel1.Controls.Add(this.buttonGoToV3);
            this.splitContainerTriangles.Panel1.Controls.Add(this.buttonGoToV1);
            this.splitContainerTriangles.Panel1.Controls.Add(this.buttonGoToV2);
            this.splitContainerTriangles.Panel1MinSize = 0;
            // 
            // splitContainerTriangles.Panel2
            // 
            this.splitContainerTriangles.Panel2.Controls.Add(this.NoTearFlowLayoutPanelTriangles);
            this.splitContainerTriangles.Panel2MinSize = 0;
            this.splitContainerTriangles.Size = new System.Drawing.Size(915, 463);
            this.splitContainerTriangles.SplitterDistance = 208;
            this.splitContainerTriangles.SplitterWidth = 1;
            this.splitContainerTriangles.TabIndex = 32;
            // 
            // groupBoxTrianglePos
            // 
            this.groupBoxTrianglePos.Controls.Add(this.checkBoxTrianglePosRelative);
            this.groupBoxTrianglePos.Controls.Add(this.textBoxTrianglePosY);
            this.groupBoxTrianglePos.Controls.Add(this.buttonTrianglePosYp);
            this.groupBoxTrianglePos.Controls.Add(this.buttonTrianglePosYn);
            this.groupBoxTrianglePos.Controls.Add(this.buttonTrianglePosXpZp);
            this.groupBoxTrianglePos.Controls.Add(this.textBoxTrianglePosXZ);
            this.groupBoxTrianglePos.Controls.Add(this.buttonTrianglePosXp);
            this.groupBoxTrianglePos.Controls.Add(this.buttonTrianglePosXpZn);
            this.groupBoxTrianglePos.Controls.Add(this.buttonTrianglePosZn);
            this.groupBoxTrianglePos.Controls.Add(this.buttonTrianglePosZp);
            this.groupBoxTrianglePos.Controls.Add(this.buttonTrianglePosXnZp);
            this.groupBoxTrianglePos.Controls.Add(this.buttonTrianglePosXn);
            this.groupBoxTrianglePos.Controls.Add(this.buttonTrianglePosXnZn);
            this.groupBoxTrianglePos.Location = new System.Drawing.Point(4, 222);
            this.groupBoxTrianglePos.Name = "groupBoxTrianglePos";
            this.groupBoxTrianglePos.Size = new System.Drawing.Size(185, 146);
            this.groupBoxTrianglePos.TabIndex = 29;
            this.groupBoxTrianglePos.TabStop = false;
            this.groupBoxTrianglePos.Text = "Position";
            // 
            // checkBoxTrianglePosRelative
            // 
            this.checkBoxTrianglePosRelative.AutoSize = true;
            this.checkBoxTrianglePosRelative.Location = new System.Drawing.Point(118, 0);
            this.checkBoxTrianglePosRelative.Name = "checkBoxTrianglePosRelative";
            this.checkBoxTrianglePosRelative.Size = new System.Drawing.Size(65, 17);
            this.checkBoxTrianglePosRelative.TabIndex = 38;
            this.checkBoxTrianglePosRelative.Text = "Relative";
            this.checkBoxTrianglePosRelative.UseVisualStyleBackColor = true;
            // 
            // textBoxTrianglePosY
            // 
            this.textBoxTrianglePosY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTrianglePosY.Location = new System.Drawing.Point(140, 70);
            this.textBoxTrianglePosY.Name = "textBoxTrianglePosY";
            this.textBoxTrianglePosY.Size = new System.Drawing.Size(42, 20);
            this.textBoxTrianglePosY.TabIndex = 33;
            this.textBoxTrianglePosY.Text = "50";
            this.textBoxTrianglePosY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonTrianglePosYp
            // 
            this.buttonTrianglePosYp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTrianglePosYp.Location = new System.Drawing.Point(140, 16);
            this.buttonTrianglePosYp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTrianglePosYp.Name = "buttonTrianglePosYp";
            this.buttonTrianglePosYp.Size = new System.Drawing.Size(42, 42);
            this.buttonTrianglePosYp.TabIndex = 35;
            this.buttonTrianglePosYp.Text = "Y+";
            this.buttonTrianglePosYp.UseVisualStyleBackColor = true;
            // 
            // buttonTrianglePosYn
            // 
            this.buttonTrianglePosYn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTrianglePosYn.Location = new System.Drawing.Point(140, 100);
            this.buttonTrianglePosYn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTrianglePosYn.Name = "buttonTrianglePosYn";
            this.buttonTrianglePosYn.Size = new System.Drawing.Size(42, 42);
            this.buttonTrianglePosYn.TabIndex = 34;
            this.buttonTrianglePosYn.Text = "Y-";
            this.buttonTrianglePosYn.UseVisualStyleBackColor = true;
            // 
            // buttonTrianglePosXpZp
            // 
            this.buttonTrianglePosXpZp.Location = new System.Drawing.Point(87, 100);
            this.buttonTrianglePosXpZp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTrianglePosXpZp.Name = "buttonTrianglePosXpZp";
            this.buttonTrianglePosXpZp.Size = new System.Drawing.Size(42, 42);
            this.buttonTrianglePosXpZp.TabIndex = 32;
            this.buttonTrianglePosXpZp.Text = "X+Z+";
            this.buttonTrianglePosXpZp.UseVisualStyleBackColor = true;
            // 
            // textBoxTrianglePosXZ
            // 
            this.textBoxTrianglePosXZ.Location = new System.Drawing.Point(45, 70);
            this.textBoxTrianglePosXZ.Name = "textBoxTrianglePosXZ";
            this.textBoxTrianglePosXZ.Size = new System.Drawing.Size(42, 20);
            this.textBoxTrianglePosXZ.TabIndex = 27;
            this.textBoxTrianglePosXZ.Text = "50";
            this.textBoxTrianglePosXZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonTrianglePosXp
            // 
            this.buttonTrianglePosXp.Location = new System.Drawing.Point(87, 58);
            this.buttonTrianglePosXp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTrianglePosXp.Name = "buttonTrianglePosXp";
            this.buttonTrianglePosXp.Size = new System.Drawing.Size(42, 42);
            this.buttonTrianglePosXp.TabIndex = 31;
            this.buttonTrianglePosXp.Text = "X+";
            this.buttonTrianglePosXp.UseVisualStyleBackColor = true;
            // 
            // buttonTrianglePosXpZn
            // 
            this.buttonTrianglePosXpZn.Location = new System.Drawing.Point(87, 16);
            this.buttonTrianglePosXpZn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTrianglePosXpZn.Name = "buttonTrianglePosXpZn";
            this.buttonTrianglePosXpZn.Size = new System.Drawing.Size(42, 42);
            this.buttonTrianglePosXpZn.TabIndex = 30;
            this.buttonTrianglePosXpZn.Text = "X+Z-";
            this.buttonTrianglePosXpZn.UseVisualStyleBackColor = true;
            // 
            // buttonTrianglePosZn
            // 
            this.buttonTrianglePosZn.Location = new System.Drawing.Point(45, 16);
            this.buttonTrianglePosZn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTrianglePosZn.Name = "buttonTrianglePosZn";
            this.buttonTrianglePosZn.Size = new System.Drawing.Size(42, 42);
            this.buttonTrianglePosZn.TabIndex = 29;
            this.buttonTrianglePosZn.Text = "Z-";
            this.buttonTrianglePosZn.UseVisualStyleBackColor = true;
            // 
            // buttonTrianglePosZp
            // 
            this.buttonTrianglePosZp.Location = new System.Drawing.Point(45, 100);
            this.buttonTrianglePosZp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTrianglePosZp.Name = "buttonTrianglePosZp";
            this.buttonTrianglePosZp.Size = new System.Drawing.Size(42, 42);
            this.buttonTrianglePosZp.TabIndex = 28;
            this.buttonTrianglePosZp.Text = "Z+";
            this.buttonTrianglePosZp.UseVisualStyleBackColor = true;
            // 
            // buttonTrianglePosXnZp
            // 
            this.buttonTrianglePosXnZp.Location = new System.Drawing.Point(3, 100);
            this.buttonTrianglePosXnZp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTrianglePosXnZp.Name = "buttonTrianglePosXnZp";
            this.buttonTrianglePosXnZp.Size = new System.Drawing.Size(42, 42);
            this.buttonTrianglePosXnZp.TabIndex = 27;
            this.buttonTrianglePosXnZp.Text = "X-Z+";
            this.buttonTrianglePosXnZp.UseVisualStyleBackColor = true;
            // 
            // buttonTrianglePosXn
            // 
            this.buttonTrianglePosXn.Location = new System.Drawing.Point(3, 58);
            this.buttonTrianglePosXn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTrianglePosXn.Name = "buttonTrianglePosXn";
            this.buttonTrianglePosXn.Size = new System.Drawing.Size(42, 42);
            this.buttonTrianglePosXn.TabIndex = 26;
            this.buttonTrianglePosXn.Text = "X-";
            this.buttonTrianglePosXn.UseVisualStyleBackColor = true;
            // 
            // buttonTrianglePosXnZn
            // 
            this.buttonTrianglePosXnZn.Location = new System.Drawing.Point(3, 16);
            this.buttonTrianglePosXnZn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTrianglePosXnZn.Name = "buttonTrianglePosXnZn";
            this.buttonTrianglePosXnZn.Size = new System.Drawing.Size(42, 42);
            this.buttonTrianglePosXnZn.TabIndex = 25;
            this.buttonTrianglePosXnZn.Text = "X-Z-";
            this.buttonTrianglePosXnZn.UseVisualStyleBackColor = true;
            // 
            // groupBoxTriangleNormal
            // 
            this.groupBoxTriangleNormal.Controls.Add(this.textBoxTriangleNormal);
            this.groupBoxTriangleNormal.Controls.Add(this.buttonTriangleNormalN);
            this.groupBoxTriangleNormal.Controls.Add(this.buttonTriangleNormalP);
            this.groupBoxTriangleNormal.Location = new System.Drawing.Point(4, 374);
            this.groupBoxTriangleNormal.Name = "groupBoxTriangleNormal";
            this.groupBoxTriangleNormal.Size = new System.Drawing.Size(185, 45);
            this.groupBoxTriangleNormal.TabIndex = 31;
            this.groupBoxTriangleNormal.TabStop = false;
            this.groupBoxTriangleNormal.Text = "Normal";
            // 
            // textBoxTriangleNormal
            // 
            this.textBoxTriangleNormal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTriangleNormal.Location = new System.Drawing.Point(67, 19);
            this.textBoxTriangleNormal.Name = "textBoxTriangleNormal";
            this.textBoxTriangleNormal.Size = new System.Drawing.Size(51, 20);
            this.textBoxTriangleNormal.TabIndex = 33;
            this.textBoxTriangleNormal.Text = "50";
            this.textBoxTriangleNormal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonTriangleNormalN
            // 
            this.buttonTriangleNormalN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTriangleNormalN.Location = new System.Drawing.Point(3, 16);
            this.buttonTriangleNormalN.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTriangleNormalN.Name = "buttonTriangleNormalN";
            this.buttonTriangleNormalN.Size = new System.Drawing.Size(61, 25);
            this.buttonTriangleNormalN.TabIndex = 35;
            this.buttonTriangleNormalN.Text = "Normal-";
            this.buttonTriangleNormalN.UseVisualStyleBackColor = true;
            // 
            // buttonTriangleNormalP
            // 
            this.buttonTriangleNormalP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTriangleNormalP.Location = new System.Drawing.Point(121, 16);
            this.buttonTriangleNormalP.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTriangleNormalP.Name = "buttonTriangleNormalP";
            this.buttonTriangleNormalP.Size = new System.Drawing.Size(61, 25);
            this.buttonTriangleNormalP.TabIndex = 35;
            this.buttonTriangleNormalP.Text = "Normal+";
            this.buttonTriangleNormalP.UseVisualStyleBackColor = true;
            // 
            // radioButtonTriOther
            // 
            this.radioButtonTriOther.AutoSize = true;
            this.radioButtonTriOther.Location = new System.Drawing.Point(12, 88);
            this.radioButtonTriOther.Name = "radioButtonTriOther";
            this.radioButtonTriOther.Size = new System.Drawing.Size(54, 17);
            this.radioButtonTriOther.TabIndex = 3;
            this.radioButtonTriOther.Text = "Other:";
            this.radioButtonTriOther.UseVisualStyleBackColor = true;
            // 
            // buttonAnnihilateTriangle
            // 
            this.buttonAnnihilateTriangle.Location = new System.Drawing.Point(100, 170);
            this.buttonAnnihilateTriangle.Name = "buttonAnnihilateTriangle";
            this.buttonAnnihilateTriangle.Size = new System.Drawing.Size(87, 23);
            this.buttonAnnihilateTriangle.TabIndex = 15;
            this.buttonAnnihilateTriangle.Text = "Annihilate";
            this.buttonAnnihilateTriangle.UseVisualStyleBackColor = true;
            // 
            // radioButtonTriFloor
            // 
            this.radioButtonTriFloor.AutoSize = true;
            this.radioButtonTriFloor.Checked = true;
            this.radioButtonTriFloor.Location = new System.Drawing.Point(12, 19);
            this.radioButtonTriFloor.Name = "radioButtonTriFloor";
            this.radioButtonTriFloor.Size = new System.Drawing.Size(48, 17);
            this.radioButtonTriFloor.TabIndex = 0;
            this.radioButtonTriFloor.TabStop = true;
            this.radioButtonTriFloor.Text = "Floor";
            this.radioButtonTriFloor.UseVisualStyleBackColor = true;
            // 
            // buttonNeutralizeTriangle
            // 
            this.buttonNeutralizeTriangle.Location = new System.Drawing.Point(7, 170);
            this.buttonNeutralizeTriangle.Name = "buttonNeutralizeTriangle";
            this.buttonNeutralizeTriangle.Size = new System.Drawing.Size(87, 23);
            this.buttonNeutralizeTriangle.TabIndex = 14;
            this.buttonNeutralizeTriangle.Text = "Neutralize";
            this.buttonNeutralizeTriangle.UseVisualStyleBackColor = true;
            // 
            // radioButtonTriWall
            // 
            this.radioButtonTriWall.AutoSize = true;
            this.radioButtonTriWall.Location = new System.Drawing.Point(12, 42);
            this.radioButtonTriWall.Name = "radioButtonTriWall";
            this.radioButtonTriWall.Size = new System.Drawing.Size(46, 17);
            this.radioButtonTriWall.TabIndex = 1;
            this.radioButtonTriWall.Text = "Wall";
            this.radioButtonTriWall.UseVisualStyleBackColor = true;
            // 
            // buttonGoToVClosest
            // 
            this.buttonGoToVClosest.Location = new System.Drawing.Point(7, 141);
            this.buttonGoToVClosest.Name = "buttonGoToVClosest";
            this.buttonGoToVClosest.Size = new System.Drawing.Size(87, 23);
            this.buttonGoToVClosest.TabIndex = 13;
            this.buttonGoToVClosest.Text = "Goto Closest";
            this.buttonGoToVClosest.UseVisualStyleBackColor = true;
            // 
            // radioButtonTriCeiling
            // 
            this.radioButtonTriCeiling.AutoSize = true;
            this.radioButtonTriCeiling.Location = new System.Drawing.Point(12, 65);
            this.radioButtonTriCeiling.Name = "radioButtonTriCeiling";
            this.radioButtonTriCeiling.Size = new System.Drawing.Size(56, 17);
            this.radioButtonTriCeiling.TabIndex = 2;
            this.radioButtonTriCeiling.Text = "Ceiling";
            this.radioButtonTriCeiling.UseVisualStyleBackColor = true;
            // 
            // checkBoxVertexMisalignment
            // 
            this.checkBoxVertexMisalignment.AutoSize = true;
            this.checkBoxVertexMisalignment.Location = new System.Drawing.Point(7, 199);
            this.checkBoxVertexMisalignment.Name = "checkBoxVertexMisalignment";
            this.checkBoxVertexMisalignment.Size = new System.Drawing.Size(151, 17);
            this.checkBoxVertexMisalignment.TabIndex = 12;
            this.checkBoxVertexMisalignment.Text = "Vertex Misalignment Offset";
            this.checkBoxVertexMisalignment.UseVisualStyleBackColor = true;
            // 
            // maskedTextBoxOtherTriangle
            // 
            this.maskedTextBoxOtherTriangle.Location = new System.Drawing.Point(71, 87);
            this.maskedTextBoxOtherTriangle.Margin = new System.Windows.Forms.Padding(2);
            this.maskedTextBoxOtherTriangle.Mask = "\\0xaaAAAAAA";
            this.maskedTextBoxOtherTriangle.Name = "maskedTextBoxOtherTriangle";
            this.maskedTextBoxOtherTriangle.Size = new System.Drawing.Size(87, 20);
            this.maskedTextBoxOtherTriangle.TabIndex = 5;
            // 
            // buttonRetrieveTriangle
            // 
            this.buttonRetrieveTriangle.Location = new System.Drawing.Point(100, 141);
            this.buttonRetrieveTriangle.Name = "buttonRetrieveTriangle";
            this.buttonRetrieveTriangle.Size = new System.Drawing.Size(87, 23);
            this.buttonRetrieveTriangle.TabIndex = 11;
            this.buttonRetrieveTriangle.Text = "Retrieve";
            this.buttonRetrieveTriangle.UseVisualStyleBackColor = true;
            // 
            // labelTriangleSelection
            // 
            this.labelTriangleSelection.AutoSize = true;
            this.labelTriangleSelection.Location = new System.Drawing.Point(1, 3);
            this.labelTriangleSelection.Name = "labelTriangleSelection";
            this.labelTriangleSelection.Size = new System.Drawing.Size(48, 13);
            this.labelTriangleSelection.TabIndex = 6;
            this.labelTriangleSelection.Text = "Triangle:";
            // 
            // buttonGoToV3
            // 
            this.buttonGoToV3.Location = new System.Drawing.Point(131, 112);
            this.buttonGoToV3.Name = "buttonGoToV3";
            this.buttonGoToV3.Size = new System.Drawing.Size(56, 23);
            this.buttonGoToV3.TabIndex = 10;
            this.buttonGoToV3.Text = "Goto V3";
            this.buttonGoToV3.UseVisualStyleBackColor = true;
            // 
            // buttonGoToV1
            // 
            this.buttonGoToV1.Location = new System.Drawing.Point(7, 112);
            this.buttonGoToV1.Name = "buttonGoToV1";
            this.buttonGoToV1.Size = new System.Drawing.Size(57, 23);
            this.buttonGoToV1.TabIndex = 8;
            this.buttonGoToV1.Text = "Goto V1";
            this.buttonGoToV1.UseVisualStyleBackColor = true;
            // 
            // buttonGoToV2
            // 
            this.buttonGoToV2.Location = new System.Drawing.Point(70, 112);
            this.buttonGoToV2.Name = "buttonGoToV2";
            this.buttonGoToV2.Size = new System.Drawing.Size(55, 23);
            this.buttonGoToV2.TabIndex = 9;
            this.buttonGoToV2.Text = "Goto V2";
            this.buttonGoToV2.UseVisualStyleBackColor = true;
            // 
            // tabPageWater
            // 
            this.tabPageWater.Controls.Add(this.noTearFlowLayoutPanelWater);
            this.tabPageWater.Location = new System.Drawing.Point(4, 22);
            this.tabPageWater.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageWater.Name = "tabPageWater";
            this.tabPageWater.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageWater.Size = new System.Drawing.Size(915, 463);
            this.tabPageWater.TabIndex = 14;
            this.tabPageWater.Text = "Water";
            // 
            // tabPageController
            // 
            this.tabPageController.Controls.Add(this.splitContainerController);
            this.tabPageController.Location = new System.Drawing.Point(4, 22);
            this.tabPageController.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageController.Name = "tabPageController";
            this.tabPageController.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageController.Size = new System.Drawing.Size(915, 463);
            this.tabPageController.TabIndex = 14;
            this.tabPageController.Text = "Controller";
            // 
            // splitContainerController
            // 
            this.splitContainerController.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerController.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerController.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerController.Location = new System.Drawing.Point(0, 0);
            this.splitContainerController.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainerController.Name = "splitContainerController";
            // 
            // splitContainerController.Panel1
            // 
            this.splitContainerController.Panel1.Controls.Add(this.controllerDisplayPanel);
            this.splitContainerController.Panel1MinSize = 0;
            // 
            // splitContainerController.Panel2
            // 
            this.splitContainerController.Panel2.Controls.Add(this.NoTearFlowLayoutPanelController);
            this.splitContainerController.Panel2MinSize = 0;
            this.splitContainerController.Size = new System.Drawing.Size(915, 463);
            this.splitContainerController.SplitterDistance = 428;
            this.splitContainerController.SplitterWidth = 1;
            this.splitContainerController.TabIndex = 17;
            // 
            // tabPageFile
            // 
            this.tabPageFile.Controls.Add(this.splitContainerFile);
            this.tabPageFile.Location = new System.Drawing.Point(4, 22);
            this.tabPageFile.Name = "tabPageFile";
            this.tabPageFile.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFile.Size = new System.Drawing.Size(915, 463);
            this.tabPageFile.TabIndex = 10;
            this.tabPageFile.Text = "File";
            // 
            // splitContainerFile
            // 
            this.splitContainerFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerFile.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerFile.Location = new System.Drawing.Point(0, 0);
            this.splitContainerFile.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainerFile.Name = "splitContainerFile";
            // 
            // splitContainerFile.Panel1
            // 
            this.splitContainerFile.Panel1.AutoScroll = true;
            this.splitContainerFile.Panel1.Controls.Add(this.buttonFileErase);
            this.splitContainerFile.Panel1.Controls.Add(this.buttonFileNumStars);
            this.splitContainerFile.Panel1.Controls.Add(this.tableLayoutPanelFile);
            this.splitContainerFile.Panel1.Controls.Add(this.groupBoxHatLocation);
            this.splitContainerFile.Panel1.Controls.Add(this.groupBoxFile);
            this.splitContainerFile.Panel1.Controls.Add(this.buttonFileSave);
            this.splitContainerFile.Panel1MinSize = 0;
            // 
            // splitContainerFile.Panel2
            // 
            this.splitContainerFile.Panel2.Controls.Add(this.noTearFlowLayoutPanelFile);
            this.splitContainerFile.Panel2MinSize = 0;
            this.splitContainerFile.Size = new System.Drawing.Size(917, 463);
            this.splitContainerFile.SplitterDistance = 680;
            this.splitContainerFile.SplitterWidth = 1;
            this.splitContainerFile.TabIndex = 19;
            // 
            // buttonFileNumStars
            // 
            this.buttonFileNumStars.Location = new System.Drawing.Point(116, 152);
            this.buttonFileNumStars.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFileNumStars.Name = "buttonFileNumStars";
            this.buttonFileNumStars.Size = new System.Drawing.Size(81, 42);
            this.buttonFileNumStars.TabIndex = 40;
            this.buttonFileNumStars.Text = "Update HUD\r\nto # Stars";
            this.buttonFileNumStars.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelFile
            // 
            this.tableLayoutPanelFile.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanelFile.ColumnCount = 11;
            this.tableLayoutPanelFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanelFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanelFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanelFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanelFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanelFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanelFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanelFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanelFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanelFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanelFile.Controls.Add(this.textBoxTableRow15Col10, 10, 14);
            this.tableLayoutPanelFile.Controls.Add(this.textBoxTableRow14Col10, 10, 13);
            this.tableLayoutPanelFile.Controls.Add(this.textBoxTableRow13Col10, 10, 12);
            this.tableLayoutPanelFile.Controls.Add(this.textBoxTableRow12Col10, 10, 11);
            this.tableLayoutPanelFile.Controls.Add(this.textBoxTableRow11Col10, 10, 10);
            this.tableLayoutPanelFile.Controls.Add(this.textBoxTableRow10Col10, 10, 9);
            this.tableLayoutPanelFile.Controls.Add(this.textBoxTableRow9Col10, 10, 8);
            this.tableLayoutPanelFile.Controls.Add(this.textBoxTableRow8Col10, 10, 7);
            this.tableLayoutPanelFile.Controls.Add(this.textBoxTableRow7Col10, 10, 6);
            this.tableLayoutPanelFile.Controls.Add(this.textBoxTableRow6Col10, 10, 5);
            this.tableLayoutPanelFile.Controls.Add(this.textBoxTableRow5Col10, 10, 4);
            this.tableLayoutPanelFile.Controls.Add(this.textBoxTableRow4Col10, 10, 3);
            this.tableLayoutPanelFile.Controls.Add(this.textBoxTableRow3Col10, 10, 2);
            this.tableLayoutPanelFile.Controls.Add(this.textBoxTableRow2Col10, 10, 1);
            this.tableLayoutPanelFile.Controls.Add(this.textBoxTableRow1Col10, 10, 0);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow24Col9, 9, 23);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow23Col9, 9, 22);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow22Col9, 9, 21);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow4Col9, 9, 3);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow3Col9, 9, 2);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow2Col9, 9, 1);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow21Col8, 8, 20);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow15Col8, 8, 14);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow13Col8, 8, 12);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow12Col8, 8, 11);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow11Col8, 8, 10);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow10Col8, 8, 9);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow4Col8, 8, 3);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow3Col8, 8, 2);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow2Col8, 8, 1);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow1Col8, 8, 0);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow19Col2, 2, 18);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow26Col2, 2, 25);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow25Col3, 3, 24);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow25Col2, 2, 24);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow26Col1, 1, 25);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow25Col1, 1, 24);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow24Col1, 1, 23);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow23Col1, 1, 22);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow22Col1, 1, 21);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow21Col1, 1, 20);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow20Col1, 1, 19);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow19Col1, 1, 18);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow18Col1, 1, 17);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow17Col1, 1, 16);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow16Col1, 1, 15);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow15Col7, 7, 14);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow14Col7, 7, 13);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow13Col7, 7, 12);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow12Col7, 7, 11);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow11Col7, 7, 10);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow10Col7, 7, 9);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow9Col7, 7, 8);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow8Col7, 7, 7);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow7Col7, 7, 6);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow6Col7, 7, 5);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow5Col7, 7, 4);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow4Col7, 7, 3);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow3Col7, 7, 2);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow2Col7, 7, 1);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow1Col7, 7, 0);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow15Col6, 6, 14);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow14Col6, 6, 13);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow13Col6, 6, 12);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow12Col6, 6, 11);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow11Col6, 6, 10);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow10Col6, 6, 9);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow9Col6, 6, 8);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow8Col6, 6, 7);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow7Col6, 6, 6);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow6Col6, 6, 5);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow5Col6, 6, 4);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow4Col6, 6, 3);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow3Col6, 6, 2);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow2Col6, 6, 1);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow1Col6, 6, 0);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow15Col5, 5, 14);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow14Col5, 5, 13);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow13Col5, 5, 12);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow12Col5, 5, 11);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow11Col5, 5, 10);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow10Col5, 5, 9);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow9Col5, 5, 8);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow8Col5, 5, 7);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow7Col5, 5, 6);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow6Col5, 5, 5);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow5Col5, 5, 4);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow4Col5, 5, 3);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow3Col5, 5, 2);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow2Col5, 5, 1);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow1Col5, 5, 0);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow15Col4, 4, 14);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow14Col4, 4, 13);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow13Col4, 4, 12);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow12Col4, 4, 11);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow11Col4, 4, 10);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow10Col4, 4, 9);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow9Col4, 4, 8);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow8Col4, 4, 7);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow7Col4, 4, 6);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow6Col4, 4, 5);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow5Col4, 4, 4);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow4Col4, 4, 3);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow3Col4, 4, 2);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow2Col4, 4, 1);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow1Col4, 4, 0);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow15Col3, 3, 14);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow14Col3, 3, 13);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow13Col3, 3, 12);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow12Col3, 3, 11);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow11Col3, 3, 10);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow10Col3, 3, 9);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow9Col3, 3, 8);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow8Col3, 3, 7);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow7Col3, 3, 6);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow6Col3, 3, 5);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow5Col3, 3, 4);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow4Col3, 3, 3);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow3Col3, 3, 2);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow2Col3, 3, 1);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow1Col3, 3, 0);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow15Col2, 2, 14);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow14Col2, 2, 13);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow13Col2, 2, 12);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow12Col2, 2, 11);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow11Col2, 2, 10);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow10Col2, 2, 9);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow9Col2, 2, 8);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow8Col2, 2, 7);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow7Col2, 2, 6);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow6Col2, 2, 5);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow5Col2, 2, 4);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow4Col2, 2, 3);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow3Col2, 2, 2);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow2Col2, 2, 1);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow1Col2, 2, 0);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow15Col1, 1, 14);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow14Col1, 1, 13);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow13Col1, 1, 12);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow12Col1, 1, 11);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow11Col1, 1, 10);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow10Col1, 1, 9);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow9Col1, 1, 8);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow8Col1, 1, 7);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow7Col1, 1, 6);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow6Col1, 1, 5);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow5Col1, 1, 4);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow4Col1, 1, 3);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow3Col1, 1, 2);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow2Col1, 1, 1);
            this.tableLayoutPanelFile.Controls.Add(this.labelFileTableRow1, 0, 0);
            this.tableLayoutPanelFile.Controls.Add(this.labelFileTableRow2, 0, 1);
            this.tableLayoutPanelFile.Controls.Add(this.labelFileTableRow3, 0, 2);
            this.tableLayoutPanelFile.Controls.Add(this.labelFileTableRow4, 0, 3);
            this.tableLayoutPanelFile.Controls.Add(this.labelFileTableRow5, 0, 4);
            this.tableLayoutPanelFile.Controls.Add(this.labelFileTableRow6, 0, 5);
            this.tableLayoutPanelFile.Controls.Add(this.labelFileTableRow7, 0, 6);
            this.tableLayoutPanelFile.Controls.Add(this.labelFileTableRow8, 0, 7);
            this.tableLayoutPanelFile.Controls.Add(this.labelFileTableRow9, 0, 8);
            this.tableLayoutPanelFile.Controls.Add(this.labelFileTableRow10, 0, 9);
            this.tableLayoutPanelFile.Controls.Add(this.labelFileTableRow11, 0, 10);
            this.tableLayoutPanelFile.Controls.Add(this.labelFileTableRow12, 0, 11);
            this.tableLayoutPanelFile.Controls.Add(this.labelFileTableRow13, 0, 12);
            this.tableLayoutPanelFile.Controls.Add(this.labelFileTableRow14, 0, 13);
            this.tableLayoutPanelFile.Controls.Add(this.labelFileTableRow15, 0, 14);
            this.tableLayoutPanelFile.Controls.Add(this.labelFileTableRow16, 0, 15);
            this.tableLayoutPanelFile.Controls.Add(this.labelFileTableRow17, 0, 16);
            this.tableLayoutPanelFile.Controls.Add(this.labelFileTableRow18, 0, 17);
            this.tableLayoutPanelFile.Controls.Add(this.labelFileTableRow19, 0, 18);
            this.tableLayoutPanelFile.Controls.Add(this.labelFileTableRow20, 0, 19);
            this.tableLayoutPanelFile.Controls.Add(this.labelFileTableRow21, 0, 20);
            this.tableLayoutPanelFile.Controls.Add(this.labelFileTableRow22, 0, 21);
            this.tableLayoutPanelFile.Controls.Add(this.labelFileTableRow23, 0, 22);
            this.tableLayoutPanelFile.Controls.Add(this.labelFileTableRow24, 0, 23);
            this.tableLayoutPanelFile.Controls.Add(this.labelFileTableRow25, 0, 24);
            this.tableLayoutPanelFile.Controls.Add(this.labelFileTableRow26, 0, 25);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow1Col1, 1, 0);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow8Col8, 8, 7);
            this.tableLayoutPanelFile.Controls.Add(this.filePictureBoxTableRow19Col9, 9, 18);
            this.tableLayoutPanelFile.Location = new System.Drawing.Point(257, 18);
            this.tableLayoutPanelFile.Name = "tableLayoutPanelFile";
            this.tableLayoutPanelFile.RowCount = 26;
            this.tableLayoutPanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.846154F));
            this.tableLayoutPanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.846154F));
            this.tableLayoutPanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.846154F));
            this.tableLayoutPanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.846154F));
            this.tableLayoutPanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.846154F));
            this.tableLayoutPanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.846154F));
            this.tableLayoutPanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.846154F));
            this.tableLayoutPanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.846154F));
            this.tableLayoutPanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.846154F));
            this.tableLayoutPanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.846154F));
            this.tableLayoutPanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.846154F));
            this.tableLayoutPanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.846154F));
            this.tableLayoutPanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.846154F));
            this.tableLayoutPanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.846154F));
            this.tableLayoutPanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.846154F));
            this.tableLayoutPanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.846154F));
            this.tableLayoutPanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.846154F));
            this.tableLayoutPanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.846154F));
            this.tableLayoutPanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.846154F));
            this.tableLayoutPanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.846154F));
            this.tableLayoutPanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.846154F));
            this.tableLayoutPanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.846154F));
            this.tableLayoutPanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.846154F));
            this.tableLayoutPanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.846154F));
            this.tableLayoutPanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.846154F));
            this.tableLayoutPanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.846154F));
            this.tableLayoutPanelFile.Size = new System.Drawing.Size(374, 419);
            this.tableLayoutPanelFile.TabIndex = 39;
            // 
            // groupBoxHatLocation
            // 
            this.groupBoxHatLocation.Controls.Add(this.radioButtonHatLocationSLGround);
            this.groupBoxHatLocation.Controls.Add(this.radioButtonHatLocationTTMGround);
            this.groupBoxHatLocation.Controls.Add(this.radioButtonHatLocationTTMUkiki);
            this.groupBoxHatLocation.Controls.Add(this.radioButtonHatLocationSSLKlepto);
            this.groupBoxHatLocation.Controls.Add(this.radioButtonHatLocationMario);
            this.groupBoxHatLocation.Controls.Add(this.radioButtonHatLocationSLSnowman);
            this.groupBoxHatLocation.Controls.Add(this.radioButtonHatLocationSSLGround);
            this.groupBoxHatLocation.Location = new System.Drawing.Point(32, 201);
            this.groupBoxHatLocation.Name = "groupBoxHatLocation";
            this.groupBoxHatLocation.Size = new System.Drawing.Size(175, 182);
            this.groupBoxHatLocation.TabIndex = 38;
            this.groupBoxHatLocation.TabStop = false;
            this.groupBoxHatLocation.Text = "Hat Location";
            // 
            // radioButtonHatLocationSLGround
            // 
            this.radioButtonHatLocationSLGround.AutoSize = true;
            this.radioButtonHatLocationSLGround.Location = new System.Drawing.Point(11, 111);
            this.radioButtonHatLocationSLGround.Name = "radioButtonHatLocationSLGround";
            this.radioButtonHatLocationSLGround.Size = new System.Drawing.Size(122, 17);
            this.radioButtonHatLocationSLGround.TabIndex = 15;
            this.radioButtonHatLocationSLGround.Text = "Hat in SL on Ground";
            this.radioButtonHatLocationSLGround.UseVisualStyleBackColor = true;
            // 
            // radioButtonHatLocationTTMGround
            // 
            this.radioButtonHatLocationTTMGround.AutoSize = true;
            this.radioButtonHatLocationTTMGround.Location = new System.Drawing.Point(11, 157);
            this.radioButtonHatLocationTTMGround.Name = "radioButtonHatLocationTTMGround";
            this.radioButtonHatLocationTTMGround.Size = new System.Drawing.Size(132, 17);
            this.radioButtonHatLocationTTMGround.TabIndex = 17;
            this.radioButtonHatLocationTTMGround.Text = "Hat in TTM on Ground";
            this.radioButtonHatLocationTTMGround.UseVisualStyleBackColor = true;
            // 
            // radioButtonHatLocationTTMUkiki
            // 
            this.radioButtonHatLocationTTMUkiki.AutoSize = true;
            this.radioButtonHatLocationTTMUkiki.Location = new System.Drawing.Point(11, 134);
            this.radioButtonHatLocationTTMUkiki.Name = "radioButtonHatLocationTTMUkiki";
            this.radioButtonHatLocationTTMUkiki.Size = new System.Drawing.Size(121, 17);
            this.radioButtonHatLocationTTMUkiki.TabIndex = 16;
            this.radioButtonHatLocationTTMUkiki.Text = "Hat in TTM on Ukiki";
            this.radioButtonHatLocationTTMUkiki.UseVisualStyleBackColor = true;
            // 
            // radioButtonHatLocationSSLKlepto
            // 
            this.radioButtonHatLocationSSLKlepto.AutoSize = true;
            this.radioButtonHatLocationSSLKlepto.Location = new System.Drawing.Point(11, 42);
            this.radioButtonHatLocationSSLKlepto.Name = "radioButtonHatLocationSSLKlepto";
            this.radioButtonHatLocationSSLKlepto.Size = new System.Drawing.Size(124, 17);
            this.radioButtonHatLocationSSLKlepto.TabIndex = 12;
            this.radioButtonHatLocationSSLKlepto.Text = "Hat in SSL on Klepto";
            this.radioButtonHatLocationSSLKlepto.UseVisualStyleBackColor = true;
            // 
            // radioButtonHatLocationMario
            // 
            this.radioButtonHatLocationMario.AutoSize = true;
            this.radioButtonHatLocationMario.Checked = true;
            this.radioButtonHatLocationMario.Location = new System.Drawing.Point(11, 19);
            this.radioButtonHatLocationMario.Name = "radioButtonHatLocationMario";
            this.radioButtonHatLocationMario.Size = new System.Drawing.Size(86, 17);
            this.radioButtonHatLocationMario.TabIndex = 11;
            this.radioButtonHatLocationMario.TabStop = true;
            this.radioButtonHatLocationMario.Text = "Hat on Mario";
            this.radioButtonHatLocationMario.UseVisualStyleBackColor = true;
            // 
            // radioButtonHatLocationSLSnowman
            // 
            this.radioButtonHatLocationSLSnowman.AutoSize = true;
            this.radioButtonHatLocationSLSnowman.Location = new System.Drawing.Point(11, 88);
            this.radioButtonHatLocationSLSnowman.Name = "radioButtonHatLocationSLSnowman";
            this.radioButtonHatLocationSLSnowman.Size = new System.Drawing.Size(134, 17);
            this.radioButtonHatLocationSLSnowman.TabIndex = 14;
            this.radioButtonHatLocationSLSnowman.Text = "Hat in SL on Snowman";
            this.radioButtonHatLocationSLSnowman.UseVisualStyleBackColor = true;
            // 
            // radioButtonHatLocationSSLGround
            // 
            this.radioButtonHatLocationSSLGround.AutoSize = true;
            this.radioButtonHatLocationSSLGround.Location = new System.Drawing.Point(11, 65);
            this.radioButtonHatLocationSSLGround.Name = "radioButtonHatLocationSSLGround";
            this.radioButtonHatLocationSSLGround.Size = new System.Drawing.Size(129, 17);
            this.radioButtonHatLocationSSLGround.TabIndex = 13;
            this.radioButtonHatLocationSSLGround.Text = "Hat in SSL on Ground";
            this.radioButtonHatLocationSSLGround.UseVisualStyleBackColor = true;
            // 
            // groupBoxFile
            // 
            this.groupBoxFile.Controls.Add(this.radioButtonFileASaved);
            this.groupBoxFile.Controls.Add(this.radioButtonFileB);
            this.groupBoxFile.Controls.Add(this.radioButtonFileCSaved);
            this.groupBoxFile.Controls.Add(this.radioButtonFileA);
            this.groupBoxFile.Controls.Add(this.radioButtonFileDSaved);
            this.groupBoxFile.Controls.Add(this.radioButtonFileD);
            this.groupBoxFile.Controls.Add(this.radioButtonFileC);
            this.groupBoxFile.Controls.Add(this.radioButtonFileBSaved);
            this.groupBoxFile.Location = new System.Drawing.Point(32, 18);
            this.groupBoxFile.Name = "groupBoxFile";
            this.groupBoxFile.Size = new System.Drawing.Size(175, 114);
            this.groupBoxFile.TabIndex = 37;
            this.groupBoxFile.TabStop = false;
            this.groupBoxFile.Text = "File";
            // 
            // radioButtonFileASaved
            // 
            this.radioButtonFileASaved.AutoSize = true;
            this.radioButtonFileASaved.Location = new System.Drawing.Point(82, 19);
            this.radioButtonFileASaved.Name = "radioButtonFileASaved";
            this.radioButtonFileASaved.Size = new System.Drawing.Size(85, 17);
            this.radioButtonFileASaved.TabIndex = 15;
            this.radioButtonFileASaved.Text = "File A Saved";
            this.radioButtonFileASaved.UseVisualStyleBackColor = true;
            // 
            // radioButtonFileB
            // 
            this.radioButtonFileB.AutoSize = true;
            this.radioButtonFileB.Location = new System.Drawing.Point(11, 42);
            this.radioButtonFileB.Name = "radioButtonFileB";
            this.radioButtonFileB.Size = new System.Drawing.Size(51, 17);
            this.radioButtonFileB.TabIndex = 12;
            this.radioButtonFileB.Text = "File B";
            this.radioButtonFileB.UseVisualStyleBackColor = true;
            // 
            // radioButtonFileCSaved
            // 
            this.radioButtonFileCSaved.AutoSize = true;
            this.radioButtonFileCSaved.Location = new System.Drawing.Point(82, 65);
            this.radioButtonFileCSaved.Name = "radioButtonFileCSaved";
            this.radioButtonFileCSaved.Size = new System.Drawing.Size(85, 17);
            this.radioButtonFileCSaved.TabIndex = 17;
            this.radioButtonFileCSaved.Text = "File C Saved";
            this.radioButtonFileCSaved.UseVisualStyleBackColor = true;
            // 
            // radioButtonFileA
            // 
            this.radioButtonFileA.AutoSize = true;
            this.radioButtonFileA.Checked = true;
            this.radioButtonFileA.Location = new System.Drawing.Point(11, 19);
            this.radioButtonFileA.Name = "radioButtonFileA";
            this.radioButtonFileA.Size = new System.Drawing.Size(51, 17);
            this.radioButtonFileA.TabIndex = 11;
            this.radioButtonFileA.TabStop = true;
            this.radioButtonFileA.Text = "File A";
            this.radioButtonFileA.UseVisualStyleBackColor = true;
            // 
            // radioButtonFileDSaved
            // 
            this.radioButtonFileDSaved.AutoSize = true;
            this.radioButtonFileDSaved.Location = new System.Drawing.Point(82, 88);
            this.radioButtonFileDSaved.Name = "radioButtonFileDSaved";
            this.radioButtonFileDSaved.Size = new System.Drawing.Size(86, 17);
            this.radioButtonFileDSaved.TabIndex = 18;
            this.radioButtonFileDSaved.Text = "File D Saved";
            this.radioButtonFileDSaved.UseVisualStyleBackColor = true;
            // 
            // radioButtonFileD
            // 
            this.radioButtonFileD.AutoSize = true;
            this.radioButtonFileD.Location = new System.Drawing.Point(11, 88);
            this.radioButtonFileD.Name = "radioButtonFileD";
            this.radioButtonFileD.Size = new System.Drawing.Size(52, 17);
            this.radioButtonFileD.TabIndex = 14;
            this.radioButtonFileD.Text = "File D";
            this.radioButtonFileD.UseVisualStyleBackColor = true;
            // 
            // radioButtonFileC
            // 
            this.radioButtonFileC.AutoSize = true;
            this.radioButtonFileC.Location = new System.Drawing.Point(11, 65);
            this.radioButtonFileC.Name = "radioButtonFileC";
            this.radioButtonFileC.Size = new System.Drawing.Size(51, 17);
            this.radioButtonFileC.TabIndex = 13;
            this.radioButtonFileC.Text = "File C";
            this.radioButtonFileC.UseVisualStyleBackColor = true;
            // 
            // radioButtonFileBSaved
            // 
            this.radioButtonFileBSaved.AutoSize = true;
            this.radioButtonFileBSaved.Location = new System.Drawing.Point(82, 42);
            this.radioButtonFileBSaved.Name = "radioButtonFileBSaved";
            this.radioButtonFileBSaved.Size = new System.Drawing.Size(85, 17);
            this.radioButtonFileBSaved.TabIndex = 16;
            this.radioButtonFileBSaved.Text = "File B Saved";
            this.radioButtonFileBSaved.UseVisualStyleBackColor = true;
            // 
            // buttonFileSave
            // 
            this.buttonFileSave.Location = new System.Drawing.Point(32, 137);
            this.buttonFileSave.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFileSave.Name = "buttonFileSave";
            this.buttonFileSave.Size = new System.Drawing.Size(61, 25);
            this.buttonFileSave.TabIndex = 36;
            this.buttonFileSave.Text = "Save";
            this.buttonFileSave.UseVisualStyleBackColor = true;
            // 
            // tabPageMisc
            // 
            this.tabPageMisc.Controls.Add(this.panelMiscBorder);
            this.tabPageMisc.Controls.Add(this.NoTearFlowLayoutPanelMisc);
            this.tabPageMisc.Location = new System.Drawing.Point(4, 22);
            this.tabPageMisc.Name = "tabPageMisc";
            this.tabPageMisc.Size = new System.Drawing.Size(915, 463);
            this.tabPageMisc.TabIndex = 9;
            this.tabPageMisc.Text = "Misc";
            // 
            // panelMiscBorder
            // 
            this.panelMiscBorder.Controls.Add(this.pictureBoxMisc);
            this.panelMiscBorder.Location = new System.Drawing.Point(3, 6);
            this.panelMiscBorder.Margin = new System.Windows.Forms.Padding(2);
            this.panelMiscBorder.Name = "panelMiscBorder";
            this.panelMiscBorder.Size = new System.Drawing.Size(57, 55);
            this.panelMiscBorder.TabIndex = 4;
            // 
            // tabPageDebug
            // 
            this.tabPageDebug.Controls.Add(this.checkBoxDbgResource);
            this.tabPageDebug.Controls.Add(this.checkBoxDbgStageSelect);
            this.tabPageDebug.Controls.Add(this.checkBoxDbgClassicDbg);
            this.tabPageDebug.Controls.Add(this.buttonDbgFreeMovement);
            this.tabPageDebug.Controls.Add(this.checkBoxDbgSpawnDbg);
            this.tabPageDebug.Controls.Add(this.label2);
            this.tabPageDebug.Controls.Add(this.panelDebugBorder);
            this.tabPageDebug.Controls.Add(this.NoTearFlowLayoutPanelDebugDisplayType);
            this.tabPageDebug.Location = new System.Drawing.Point(4, 22);
            this.tabPageDebug.Name = "tabPageDebug";
            this.tabPageDebug.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDebug.Size = new System.Drawing.Size(915, 463);
            this.tabPageDebug.TabIndex = 8;
            this.tabPageDebug.Text = "Debug";
            // 
            // checkBoxDbgResource
            // 
            this.checkBoxDbgResource.AutoSize = true;
            this.checkBoxDbgResource.Location = new System.Drawing.Point(243, 49);
            this.checkBoxDbgResource.Name = "checkBoxDbgResource";
            this.checkBoxDbgResource.Size = new System.Drawing.Size(107, 17);
            this.checkBoxDbgResource.TabIndex = 25;
            this.checkBoxDbgResource.Text = "Resource Debug";
            this.checkBoxDbgResource.UseVisualStyleBackColor = true;
            // 
            // checkBoxDbgStageSelect
            // 
            this.checkBoxDbgStageSelect.AutoSize = true;
            this.checkBoxDbgStageSelect.Location = new System.Drawing.Point(243, 96);
            this.checkBoxDbgStageSelect.Name = "checkBoxDbgStageSelect";
            this.checkBoxDbgStageSelect.Size = new System.Drawing.Size(87, 17);
            this.checkBoxDbgStageSelect.TabIndex = 24;
            this.checkBoxDbgStageSelect.Text = "Stage Select";
            this.checkBoxDbgStageSelect.UseVisualStyleBackColor = true;
            // 
            // checkBoxDbgClassicDbg
            // 
            this.checkBoxDbgClassicDbg.AutoSize = true;
            this.checkBoxDbgClassicDbg.Location = new System.Drawing.Point(243, 26);
            this.checkBoxDbgClassicDbg.Name = "checkBoxDbgClassicDbg";
            this.checkBoxDbgClassicDbg.Size = new System.Drawing.Size(94, 17);
            this.checkBoxDbgClassicDbg.TabIndex = 23;
            this.checkBoxDbgClassicDbg.Text = "Classic Debug";
            this.checkBoxDbgClassicDbg.UseVisualStyleBackColor = true;
            // 
            // buttonDbgFreeMovement
            // 
            this.buttonDbgFreeMovement.Location = new System.Drawing.Point(243, 118);
            this.buttonDbgFreeMovement.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDbgFreeMovement.Name = "buttonDbgFreeMovement";
            this.buttonDbgFreeMovement.Size = new System.Drawing.Size(124, 21);
            this.buttonDbgFreeMovement.TabIndex = 22;
            this.buttonDbgFreeMovement.Text = "Free Movement";
            this.buttonDbgFreeMovement.UseVisualStyleBackColor = true;
            // 
            // checkBoxDbgSpawnDbg
            // 
            this.checkBoxDbgSpawnDbg.AutoSize = true;
            this.checkBoxDbgSpawnDbg.Location = new System.Drawing.Point(243, 73);
            this.checkBoxDbgSpawnDbg.Name = "checkBoxDbgSpawnDbg";
            this.checkBoxDbgSpawnDbg.Size = new System.Drawing.Size(94, 17);
            this.checkBoxDbgSpawnDbg.TabIndex = 12;
            this.checkBoxDbgSpawnDbg.Text = "Spawn Debug";
            this.checkBoxDbgSpawnDbg.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(68, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Debug:";
            // 
            // panelDebugBorder
            // 
            this.panelDebugBorder.Controls.Add(this.pictureBoxDebug);
            this.panelDebugBorder.Location = new System.Drawing.Point(5, 5);
            this.panelDebugBorder.Margin = new System.Windows.Forms.Padding(2);
            this.panelDebugBorder.Name = "panelDebugBorder";
            this.panelDebugBorder.Size = new System.Drawing.Size(57, 55);
            this.panelDebugBorder.TabIndex = 3;
            // 
            // tabPageMap
            // 
            this.tabPageMap.Controls.Add(this.splitContainerMap);
            this.tabPageMap.Location = new System.Drawing.Point(4, 22);
            this.tabPageMap.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageMap.Name = "tabPageMap";
            this.tabPageMap.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageMap.Size = new System.Drawing.Size(915, 463);
            this.tabPageMap.TabIndex = 4;
            this.tabPageMap.Text = "Map";
            // 
            // splitContainerMap
            // 
            this.splitContainerMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerMap.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerMap.Location = new System.Drawing.Point(3, 3);
            this.splitContainerMap.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainerMap.Name = "splitContainerMap";
            // 
            // splitContainerMap.Panel1
            // 
            this.splitContainerMap.Panel1.Controls.Add(this.checkBoxMapShowCeiling);
            this.splitContainerMap.Panel1.Controls.Add(this.checkBoxMapShowFloor);
            this.splitContainerMap.Panel1.Controls.Add(this.checkBoxMapShowCamera);
            this.splitContainerMap.Panel1.Controls.Add(this.checkBoxMapShowHolp);
            this.splitContainerMap.Panel1.Controls.Add(this.checkBoxMapShowMario);
            this.splitContainerMap.Panel1.Controls.Add(this.labelMapName);
            this.splitContainerMap.Panel1.Controls.Add(this.trackBarMapIconSize);
            this.splitContainerMap.Panel1.Controls.Add(this.labelMapIconSize);
            this.splitContainerMap.Panel1.Controls.Add(this.checkBoxMapShowInactive);
            this.splitContainerMap.Panel1.Controls.Add(this.labelMapZoom);
            this.splitContainerMap.Panel1.Controls.Add(this.labelMapId);
            this.splitContainerMap.Panel1.Controls.Add(this.labelMapSubName);
            this.splitContainerMap.Panel1.Controls.Add(this.trackBarMapZoom);
            this.splitContainerMap.Panel1MinSize = 0;
            // 
            // splitContainerMap.Panel2
            // 
            this.splitContainerMap.Panel2.Controls.Add(this.glControlMap);
            this.splitContainerMap.Panel2.Controls.Add(this.labelMapPu);
            this.splitContainerMap.Panel2.Controls.Add(this.labelMapPuValue);
            this.splitContainerMap.Panel2.Controls.Add(this.labelMapQpu);
            this.splitContainerMap.Panel2.Controls.Add(this.labelMapQpuValue);
            this.splitContainerMap.Panel2MinSize = 0;
            this.splitContainerMap.Size = new System.Drawing.Size(908, 456);
            this.splitContainerMap.SplitterDistance = 208;
            this.splitContainerMap.SplitterWidth = 1;
            this.splitContainerMap.TabIndex = 16;
            // 
            // checkBoxMapShowCeiling
            // 
            this.checkBoxMapShowCeiling.AutoSize = true;
            this.checkBoxMapShowCeiling.Location = new System.Drawing.Point(5, 122);
            this.checkBoxMapShowCeiling.Name = "checkBoxMapShowCeiling";
            this.checkBoxMapShowCeiling.Size = new System.Drawing.Size(105, 17);
            this.checkBoxMapShowCeiling.TabIndex = 19;
            this.checkBoxMapShowCeiling.Text = "Show Ceiling Tri.";
            this.checkBoxMapShowCeiling.UseVisualStyleBackColor = true;
            // 
            // checkBoxMapShowFloor
            // 
            this.checkBoxMapShowFloor.AutoSize = true;
            this.checkBoxMapShowFloor.Location = new System.Drawing.Point(5, 101);
            this.checkBoxMapShowFloor.Name = "checkBoxMapShowFloor";
            this.checkBoxMapShowFloor.Size = new System.Drawing.Size(97, 17);
            this.checkBoxMapShowFloor.TabIndex = 19;
            this.checkBoxMapShowFloor.Text = "Show Floor Tri.";
            this.checkBoxMapShowFloor.UseVisualStyleBackColor = true;
            // 
            // checkBoxMapShowCamera
            // 
            this.checkBoxMapShowCamera.AutoSize = true;
            this.checkBoxMapShowCamera.Location = new System.Drawing.Point(112, 122);
            this.checkBoxMapShowCamera.Name = "checkBoxMapShowCamera";
            this.checkBoxMapShowCamera.Size = new System.Drawing.Size(92, 17);
            this.checkBoxMapShowCamera.TabIndex = 18;
            this.checkBoxMapShowCamera.Text = "Show Camera";
            this.checkBoxMapShowCamera.UseVisualStyleBackColor = true;
            // 
            // checkBoxMapShowHolp
            // 
            this.checkBoxMapShowHolp.AutoSize = true;
            this.checkBoxMapShowHolp.Location = new System.Drawing.Point(112, 102);
            this.checkBoxMapShowHolp.Name = "checkBoxMapShowHolp";
            this.checkBoxMapShowHolp.Size = new System.Drawing.Size(85, 17);
            this.checkBoxMapShowHolp.TabIndex = 17;
            this.checkBoxMapShowHolp.Text = "Show HOLP";
            this.checkBoxMapShowHolp.UseVisualStyleBackColor = true;
            // 
            // checkBoxMapShowMario
            // 
            this.checkBoxMapShowMario.AutoSize = true;
            this.checkBoxMapShowMario.Checked = true;
            this.checkBoxMapShowMario.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMapShowMario.Location = new System.Drawing.Point(112, 81);
            this.checkBoxMapShowMario.Name = "checkBoxMapShowMario";
            this.checkBoxMapShowMario.Size = new System.Drawing.Size(82, 17);
            this.checkBoxMapShowMario.TabIndex = 15;
            this.checkBoxMapShowMario.Text = "Show Mario";
            this.checkBoxMapShowMario.UseVisualStyleBackColor = true;
            // 
            // labelMapName
            // 
            this.labelMapName.AutoSize = true;
            this.labelMapName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMapName.Location = new System.Drawing.Point(2, 3);
            this.labelMapName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelMapName.Name = "labelMapName";
            this.labelMapName.Size = new System.Drawing.Size(60, 13);
            this.labelMapName.TabIndex = 3;
            this.labelMapName.Text = "Unknown";
            // 
            // trackBarMapIconSize
            // 
            this.trackBarMapIconSize.Location = new System.Drawing.Point(106, 52);
            this.trackBarMapIconSize.Margin = new System.Windows.Forms.Padding(2);
            this.trackBarMapIconSize.Maximum = 100;
            this.trackBarMapIconSize.Minimum = 5;
            this.trackBarMapIconSize.Name = "trackBarMapIconSize";
            this.trackBarMapIconSize.Size = new System.Drawing.Size(94, 45);
            this.trackBarMapIconSize.SmallChange = 5;
            this.trackBarMapIconSize.TabIndex = 10;
            this.trackBarMapIconSize.TickFrequency = 10;
            this.trackBarMapIconSize.Value = 5;
            // 
            // labelMapIconSize
            // 
            this.labelMapIconSize.AutoSize = true;
            this.labelMapIconSize.Location = new System.Drawing.Point(129, 37);
            this.labelMapIconSize.Name = "labelMapIconSize";
            this.labelMapIconSize.Size = new System.Drawing.Size(54, 13);
            this.labelMapIconSize.TabIndex = 13;
            this.labelMapIconSize.Text = "Icon Size:";
            // 
            // checkBoxMapShowInactive
            // 
            this.checkBoxMapShowInactive.AutoSize = true;
            this.checkBoxMapShowInactive.Location = new System.Drawing.Point(5, 81);
            this.checkBoxMapShowInactive.Name = "checkBoxMapShowInactive";
            this.checkBoxMapShowInactive.Size = new System.Drawing.Size(94, 17);
            this.checkBoxMapShowInactive.TabIndex = 14;
            this.checkBoxMapShowInactive.Text = "Show Inactive";
            this.checkBoxMapShowInactive.UseVisualStyleBackColor = true;
            // 
            // labelMapZoom
            // 
            this.labelMapZoom.AutoSize = true;
            this.labelMapZoom.Location = new System.Drawing.Point(28, 37);
            this.labelMapZoom.Name = "labelMapZoom";
            this.labelMapZoom.Size = new System.Drawing.Size(37, 13);
            this.labelMapZoom.TabIndex = 12;
            this.labelMapZoom.Text = "Zoom:";
            // 
            // labelMapId
            // 
            this.labelMapId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMapId.Location = new System.Drawing.Point(94, 437);
            this.labelMapId.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelMapId.Name = "labelMapId";
            this.labelMapId.Size = new System.Drawing.Size(110, 13);
            this.labelMapId.TabIndex = 4;
            this.labelMapId.Text = "[0:0:0:0]";
            this.labelMapId.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelMapSubName
            // 
            this.labelMapSubName.AutoSize = true;
            this.labelMapSubName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMapSubName.Location = new System.Drawing.Point(2, 16);
            this.labelMapSubName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelMapSubName.Name = "labelMapSubName";
            this.labelMapSubName.Size = new System.Drawing.Size(53, 13);
            this.labelMapSubName.TabIndex = 7;
            this.labelMapSubName.Text = "Unknown";
            // 
            // trackBarMapZoom
            // 
            this.trackBarMapZoom.Location = new System.Drawing.Point(5, 52);
            this.trackBarMapZoom.Margin = new System.Windows.Forms.Padding(2);
            this.trackBarMapZoom.Maximum = 100;
            this.trackBarMapZoom.Name = "trackBarMapZoom";
            this.trackBarMapZoom.Size = new System.Drawing.Size(91, 45);
            this.trackBarMapZoom.SmallChange = 5;
            this.trackBarMapZoom.TabIndex = 9;
            this.trackBarMapZoom.TickFrequency = 10;
            // 
            // glControlMap
            // 
            this.glControlMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.glControlMap.BackColor = System.Drawing.Color.Black;
            this.glControlMap.Location = new System.Drawing.Point(4, 3);
            this.glControlMap.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.glControlMap.Name = "glControlMap";
            this.glControlMap.Size = new System.Drawing.Size(749, 430);
            this.glControlMap.TabIndex = 0;
            this.glControlMap.VSync = false;
            this.glControlMap.Load += new System.EventHandler(this.glControlMap_Load);
            // 
            // labelMapPu
            // 
            this.labelMapPu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelMapPu.AutoSize = true;
            this.labelMapPu.Location = new System.Drawing.Point(2, 438);
            this.labelMapPu.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelMapPu.Name = "labelMapPu";
            this.labelMapPu.Size = new System.Drawing.Size(61, 13);
            this.labelMapPu.TabIndex = 5;
            this.labelMapPu.Text = "PU [X:Y:Z]:";
            // 
            // labelMapPuValue
            // 
            this.labelMapPuValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelMapPuValue.AutoSize = true;
            this.labelMapPuValue.Location = new System.Drawing.Point(67, 438);
            this.labelMapPuValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelMapPuValue.Name = "labelMapPuValue";
            this.labelMapPuValue.Size = new System.Drawing.Size(37, 13);
            this.labelMapPuValue.TabIndex = 8;
            this.labelMapPuValue.Text = "[0:0:0]";
            // 
            // labelMapQpu
            // 
            this.labelMapQpu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelMapQpu.AutoSize = true;
            this.labelMapQpu.Location = new System.Drawing.Point(123, 438);
            this.labelMapQpu.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelMapQpu.Name = "labelMapQpu";
            this.labelMapQpu.Size = new System.Drawing.Size(69, 13);
            this.labelMapQpu.TabIndex = 6;
            this.labelMapQpu.Text = "QPU [X:Y:Z]:";
            // 
            // labelMapQpuValue
            // 
            this.labelMapQpuValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelMapQpuValue.AutoSize = true;
            this.labelMapQpuValue.Location = new System.Drawing.Point(196, 438);
            this.labelMapQpuValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelMapQpuValue.Name = "labelMapQpuValue";
            this.labelMapQpuValue.Size = new System.Drawing.Size(37, 13);
            this.labelMapQpuValue.TabIndex = 1;
            this.labelMapQpuValue.Text = "[0:0:0]";
            // 
            // tabPagePu
            // 
            this.tabPagePu.Controls.Add(this.groupBoxPuController);
            this.tabPagePu.Location = new System.Drawing.Point(4, 22);
            this.tabPagePu.Name = "tabPagePu";
            this.tabPagePu.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePu.Size = new System.Drawing.Size(915, 463);
            this.tabPagePu.TabIndex = 15;
            this.tabPagePu.Text = "PU";
            // 
            // groupBoxPuController
            // 
            this.groupBoxPuController.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.groupBoxPuController.Controls.Add(this.labelPuConPu);
            this.groupBoxPuController.Controls.Add(this.labelPuConQpuValue);
            this.groupBoxPuController.Controls.Add(this.labelPuConQpu);
            this.groupBoxPuController.Controls.Add(this.labelPuConPuValue);
            this.groupBoxPuController.Controls.Add(this.labelPuContXp);
            this.groupBoxPuController.Controls.Add(this.labelPuContXm);
            this.groupBoxPuController.Controls.Add(this.labelPuContZp);
            this.groupBoxPuController.Controls.Add(this.labelPuContZm);
            this.groupBoxPuController.Controls.Add(this.buttonPuConZnPu);
            this.groupBoxPuController.Controls.Add(this.buttonPuConXpQpu);
            this.groupBoxPuController.Controls.Add(this.buttonPuConXnQpu);
            this.groupBoxPuController.Controls.Add(this.buttonPuConXnPu);
            this.groupBoxPuController.Controls.Add(this.buttonPuConZnQpu);
            this.groupBoxPuController.Controls.Add(this.buttonPuConXpPu);
            this.groupBoxPuController.Controls.Add(this.buttonPuConZpPu);
            this.groupBoxPuController.Controls.Add(this.buttonPuConZpQpu);
            this.groupBoxPuController.Controls.Add(this.buttonPuConHome);
            this.groupBoxPuController.Location = new System.Drawing.Point(346, 3);
            this.groupBoxPuController.Name = "groupBoxPuController";
            this.groupBoxPuController.Size = new System.Drawing.Size(222, 454);
            this.groupBoxPuController.TabIndex = 7;
            this.groupBoxPuController.TabStop = false;
            this.groupBoxPuController.Text = "PU Controller";
            // 
            // labelPuConPu
            // 
            this.labelPuConPu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelPuConPu.AutoSize = true;
            this.labelPuConPu.Location = new System.Drawing.Point(5, 438);
            this.labelPuConPu.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPuConPu.Name = "labelPuConPu";
            this.labelPuConPu.Size = new System.Drawing.Size(51, 13);
            this.labelPuConPu.TabIndex = 20;
            this.labelPuConPu.Text = "PU [X:Z]:";
            // 
            // labelPuConQpuValue
            // 
            this.labelPuConQpuValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelPuConQpuValue.AutoSize = true;
            this.labelPuConQpuValue.Location = new System.Drawing.Point(174, 438);
            this.labelPuConQpuValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPuConQpuValue.Name = "labelPuConQpuValue";
            this.labelPuConQpuValue.Size = new System.Drawing.Size(28, 13);
            this.labelPuConQpuValue.TabIndex = 22;
            this.labelPuConQpuValue.Text = "[0:0]";
            // 
            // labelPuConQpu
            // 
            this.labelPuConQpu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelPuConQpu.AutoSize = true;
            this.labelPuConQpu.Location = new System.Drawing.Point(111, 438);
            this.labelPuConQpu.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPuConQpu.Name = "labelPuConQpu";
            this.labelPuConQpu.Size = new System.Drawing.Size(59, 13);
            this.labelPuConQpu.TabIndex = 21;
            this.labelPuConQpu.Text = "QPU [X:Z]:";
            // 
            // labelPuConPuValue
            // 
            this.labelPuConPuValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelPuConPuValue.AutoSize = true;
            this.labelPuConPuValue.Location = new System.Drawing.Point(60, 438);
            this.labelPuConPuValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPuConPuValue.Name = "labelPuConPuValue";
            this.labelPuConPuValue.Size = new System.Drawing.Size(28, 13);
            this.labelPuConPuValue.TabIndex = 19;
            this.labelPuConPuValue.Text = "[0:0]";
            // 
            // labelPuContXp
            // 
            this.labelPuContXp.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelPuContXp.AutoSize = true;
            this.labelPuContXp.Location = new System.Drawing.Point(171, 70);
            this.labelPuContXp.Name = "labelPuContXp";
            this.labelPuContXp.Size = new System.Drawing.Size(20, 13);
            this.labelPuContXp.TabIndex = 18;
            this.labelPuContXp.Text = "X+";
            // 
            // labelPuContXm
            // 
            this.labelPuContXm.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelPuContXm.AutoSize = true;
            this.labelPuContXm.Location = new System.Drawing.Point(35, 70);
            this.labelPuContXm.Name = "labelPuContXm";
            this.labelPuContXm.Size = new System.Drawing.Size(17, 13);
            this.labelPuContXm.TabIndex = 17;
            this.labelPuContXm.Text = "X-";
            // 
            // labelPuContZp
            // 
            this.labelPuContZp.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelPuContZp.AutoSize = true;
            this.labelPuContZp.Location = new System.Drawing.Point(75, 162);
            this.labelPuContZp.Name = "labelPuContZp";
            this.labelPuContZp.Size = new System.Drawing.Size(20, 13);
            this.labelPuContZp.TabIndex = 16;
            this.labelPuContZp.Text = "Z+";
            // 
            // labelPuContZm
            // 
            this.labelPuContZm.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelPuContZm.AutoSize = true;
            this.labelPuContZm.Location = new System.Drawing.Point(75, 21);
            this.labelPuContZm.Name = "labelPuContZm";
            this.labelPuContZm.Size = new System.Drawing.Size(17, 13);
            this.labelPuContZm.TabIndex = 15;
            this.labelPuContZm.Text = "Z-";
            // 
            // buttonPuConZnPu
            // 
            this.buttonPuConZnPu.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonPuConZnPu.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonPuConZnPu.BackgroundImage")));
            this.buttonPuConZnPu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonPuConZnPu.Location = new System.Drawing.Point(98, 52);
            this.buttonPuConZnPu.Name = "buttonPuConZnPu";
            this.buttonPuConZnPu.Padding = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.buttonPuConZnPu.Size = new System.Drawing.Size(25, 25);
            this.buttonPuConZnPu.TabIndex = 14;
            this.buttonPuConZnPu.UseVisualStyleBackColor = true;
            // 
            // buttonPuConXpQpu
            // 
            this.buttonPuConXpQpu.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonPuConXpQpu.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonPuConXpQpu.BackgroundImage")));
            this.buttonPuConXpQpu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonPuConXpQpu.Location = new System.Drawing.Point(163, 86);
            this.buttonPuConXpQpu.Name = "buttonPuConXpQpu";
            this.buttonPuConXpQpu.Padding = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.buttonPuConXpQpu.Size = new System.Drawing.Size(25, 25);
            this.buttonPuConXpQpu.TabIndex = 13;
            this.buttonPuConXpQpu.UseVisualStyleBackColor = true;
            // 
            // buttonPuConXnQpu
            // 
            this.buttonPuConXnQpu.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonPuConXnQpu.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonPuConXnQpu.BackgroundImage")));
            this.buttonPuConXnQpu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonPuConXnQpu.Location = new System.Drawing.Point(34, 86);
            this.buttonPuConXnQpu.Name = "buttonPuConXnQpu";
            this.buttonPuConXnQpu.Padding = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.buttonPuConXnQpu.Size = new System.Drawing.Size(25, 25);
            this.buttonPuConXnQpu.TabIndex = 12;
            this.buttonPuConXnQpu.UseVisualStyleBackColor = true;
            // 
            // buttonPuConXnPu
            // 
            this.buttonPuConXnPu.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonPuConXnPu.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonPuConXnPu.BackgroundImage")));
            this.buttonPuConXnPu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonPuConXnPu.Location = new System.Drawing.Point(65, 86);
            this.buttonPuConXnPu.Name = "buttonPuConXnPu";
            this.buttonPuConXnPu.Padding = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.buttonPuConXnPu.Size = new System.Drawing.Size(25, 25);
            this.buttonPuConXnPu.TabIndex = 11;
            this.buttonPuConXnPu.UseVisualStyleBackColor = true;
            // 
            // buttonPuConZnQpu
            // 
            this.buttonPuConZnQpu.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonPuConZnQpu.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonPuConZnQpu.BackgroundImage")));
            this.buttonPuConZnQpu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonPuConZnQpu.Location = new System.Drawing.Point(98, 21);
            this.buttonPuConZnQpu.Name = "buttonPuConZnQpu";
            this.buttonPuConZnQpu.Padding = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.buttonPuConZnQpu.Size = new System.Drawing.Size(25, 25);
            this.buttonPuConZnQpu.TabIndex = 10;
            this.buttonPuConZnQpu.UseVisualStyleBackColor = true;
            // 
            // buttonPuConXpPu
            // 
            this.buttonPuConXpPu.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonPuConXpPu.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonPuConXpPu.BackgroundImage")));
            this.buttonPuConXpPu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonPuConXpPu.Location = new System.Drawing.Point(132, 86);
            this.buttonPuConXpPu.Name = "buttonPuConXpPu";
            this.buttonPuConXpPu.Padding = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.buttonPuConXpPu.Size = new System.Drawing.Size(25, 25);
            this.buttonPuConXpPu.TabIndex = 9;
            this.buttonPuConXpPu.UseVisualStyleBackColor = true;
            // 
            // buttonPuConZpPu
            // 
            this.buttonPuConZpPu.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonPuConZpPu.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonPuConZpPu.BackgroundImage")));
            this.buttonPuConZpPu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonPuConZpPu.Location = new System.Drawing.Point(98, 119);
            this.buttonPuConZpPu.Name = "buttonPuConZpPu";
            this.buttonPuConZpPu.Padding = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.buttonPuConZpPu.Size = new System.Drawing.Size(25, 25);
            this.buttonPuConZpPu.TabIndex = 8;
            this.buttonPuConZpPu.UseVisualStyleBackColor = true;
            // 
            // buttonPuConZpQpu
            // 
            this.buttonPuConZpQpu.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonPuConZpQpu.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonPuConZpQpu.BackgroundImage")));
            this.buttonPuConZpQpu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonPuConZpQpu.Location = new System.Drawing.Point(98, 150);
            this.buttonPuConZpQpu.Name = "buttonPuConZpQpu";
            this.buttonPuConZpQpu.Padding = new System.Windows.Forms.Padding(2, 1, 0, 0);
            this.buttonPuConZpQpu.Size = new System.Drawing.Size(25, 25);
            this.buttonPuConZpQpu.TabIndex = 7;
            this.buttonPuConZpQpu.UseVisualStyleBackColor = true;
            // 
            // buttonPuConHome
            // 
            this.buttonPuConHome.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonPuConHome.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonPuConHome.BackgroundImage")));
            this.buttonPuConHome.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonPuConHome.Font = new System.Drawing.Font("Webdings", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonPuConHome.Location = new System.Drawing.Point(96, 83);
            this.buttonPuConHome.Name = "buttonPuConHome";
            this.buttonPuConHome.Size = new System.Drawing.Size(30, 30);
            this.buttonPuConHome.TabIndex = 1;
            this.buttonPuConHome.UseVisualStyleBackColor = true;
            // 
            // tabPageExpressions
            // 
            this.tabPageExpressions.BackColor = System.Drawing.Color.Transparent;
            this.tabPageExpressions.Controls.Add(this.checkBoxAbsoluteAddress);
            this.tabPageExpressions.Controls.Add(this.buttonOtherDelete);
            this.tabPageExpressions.Controls.Add(this.buttonOtherModify);
            this.tabPageExpressions.Controls.Add(this.buttonOtherAdd);
            this.tabPageExpressions.Controls.Add(this.dataGridViewExpressions);
            this.tabPageExpressions.Location = new System.Drawing.Point(4, 22);
            this.tabPageExpressions.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageExpressions.Name = "tabPageExpressions";
            this.tabPageExpressions.Size = new System.Drawing.Size(915, 463);
            this.tabPageExpressions.TabIndex = 2;
            this.tabPageExpressions.Text = "Expressions";
            // 
            // checkBoxAbsoluteAddress
            // 
            this.checkBoxAbsoluteAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxAbsoluteAddress.AutoSize = true;
            this.checkBoxAbsoluteAddress.Location = new System.Drawing.Point(780, 330);
            this.checkBoxAbsoluteAddress.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxAbsoluteAddress.Name = "checkBoxAbsoluteAddress";
            this.checkBoxAbsoluteAddress.Size = new System.Drawing.Size(134, 17);
            this.checkBoxAbsoluteAddress.TabIndex = 4;
            this.checkBoxAbsoluteAddress.Text = "View Absolute Address";
            this.checkBoxAbsoluteAddress.UseVisualStyleBackColor = true;
            // 
            // buttonOtherDelete
            // 
            this.buttonOtherDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOtherDelete.Location = new System.Drawing.Point(106, 328);
            this.buttonOtherDelete.Margin = new System.Windows.Forms.Padding(2);
            this.buttonOtherDelete.Name = "buttonOtherDelete";
            this.buttonOtherDelete.Size = new System.Drawing.Size(48, 21);
            this.buttonOtherDelete.TabIndex = 3;
            this.buttonOtherDelete.Text = "Delete";
            this.buttonOtherDelete.UseVisualStyleBackColor = true;
            this.buttonOtherDelete.Click += new System.EventHandler(this.buttonOtherDelete_Click);
            // 
            // buttonOtherModify
            // 
            this.buttonOtherModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOtherModify.Location = new System.Drawing.Point(54, 328);
            this.buttonOtherModify.Margin = new System.Windows.Forms.Padding(2);
            this.buttonOtherModify.Name = "buttonOtherModify";
            this.buttonOtherModify.Size = new System.Drawing.Size(48, 21);
            this.buttonOtherModify.TabIndex = 2;
            this.buttonOtherModify.Text = "Modify";
            this.buttonOtherModify.UseVisualStyleBackColor = true;
            this.buttonOtherModify.Click += new System.EventHandler(this.buttonOtherModify_Click);
            // 
            // buttonOtherAdd
            // 
            this.buttonOtherAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOtherAdd.Location = new System.Drawing.Point(2, 328);
            this.buttonOtherAdd.Margin = new System.Windows.Forms.Padding(2);
            this.buttonOtherAdd.Name = "buttonOtherAdd";
            this.buttonOtherAdd.Size = new System.Drawing.Size(48, 21);
            this.buttonOtherAdd.TabIndex = 1;
            this.buttonOtherAdd.Text = "Add";
            this.buttonOtherAdd.UseVisualStyleBackColor = true;
            this.buttonOtherAdd.Click += new System.EventHandler(this.buttonOtherAdd_Click);
            // 
            // dataGridViewExpressions
            // 
            this.dataGridViewExpressions.AllowUserToAddRows = false;
            this.dataGridViewExpressions.AllowUserToDeleteRows = false;
            this.dataGridViewExpressions.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.dataGridViewExpressions.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewExpressions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewExpressions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewExpressions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewExpressions.Location = new System.Drawing.Point(2, 2);
            this.dataGridViewExpressions.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewExpressions.Name = "dataGridViewExpressions";
            this.dataGridViewExpressions.RowTemplate.Height = 20;
            this.dataGridViewExpressions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewExpressions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewExpressions.Size = new System.Drawing.Size(913, 321);
            this.dataGridViewExpressions.TabIndex = 0;
            this.dataGridViewExpressions.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewOther_CellMouseDoubleClick);
            // 
            // tabPageDisassembly
            // 
            this.tabPageDisassembly.BackColor = System.Drawing.Color.Transparent;
            this.tabPageDisassembly.Controls.Add(this.buttonDisMore);
            this.tabPageDisassembly.Controls.Add(this.buttonDisGo);
            this.tabPageDisassembly.Controls.Add(this.maskedTextBoxDisStart);
            this.tabPageDisassembly.Controls.Add(this.labelDisStart);
            this.tabPageDisassembly.Controls.Add(this.richTextBoxDissasembly);
            this.tabPageDisassembly.Location = new System.Drawing.Point(4, 22);
            this.tabPageDisassembly.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageDisassembly.Name = "tabPageDisassembly";
            this.tabPageDisassembly.Size = new System.Drawing.Size(915, 463);
            this.tabPageDisassembly.TabIndex = 3;
            this.tabPageDisassembly.Text = "Disassembly";
            // 
            // buttonDisMore
            // 
            this.buttonDisMore.Location = new System.Drawing.Point(237, 2);
            this.buttonDisMore.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDisMore.Name = "buttonDisMore";
            this.buttonDisMore.Size = new System.Drawing.Size(50, 20);
            this.buttonDisMore.TabIndex = 6;
            this.buttonDisMore.Text = "More";
            this.buttonDisMore.UseVisualStyleBackColor = true;
            this.buttonDisMore.Visible = false;
            // 
            // buttonDisGo
            // 
            this.buttonDisGo.Location = new System.Drawing.Point(171, 2);
            this.buttonDisGo.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDisGo.Name = "buttonDisGo";
            this.buttonDisGo.Size = new System.Drawing.Size(62, 20);
            this.buttonDisGo.TabIndex = 5;
            this.buttonDisGo.Text = "Go";
            this.buttonDisGo.UseVisualStyleBackColor = true;
            // 
            // maskedTextBoxDisStart
            // 
            this.maskedTextBoxDisStart.Location = new System.Drawing.Point(79, 2);
            this.maskedTextBoxDisStart.Margin = new System.Windows.Forms.Padding(2);
            this.maskedTextBoxDisStart.Mask = "\\0xaaAAAAAA";
            this.maskedTextBoxDisStart.Name = "maskedTextBoxDisStart";
            this.maskedTextBoxDisStart.Size = new System.Drawing.Size(87, 20);
            this.maskedTextBoxDisStart.TabIndex = 4;
            // 
            // labelDisStart
            // 
            this.labelDisStart.AutoSize = true;
            this.labelDisStart.Location = new System.Drawing.Point(2, 6);
            this.labelDisStart.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelDisStart.Name = "labelDisStart";
            this.labelDisStart.Size = new System.Drawing.Size(73, 13);
            this.labelDisStart.TabIndex = 3;
            this.labelDisStart.Text = "Start Address:";
            // 
            // richTextBoxDissasembly
            // 
            this.richTextBoxDissasembly.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxDissasembly.Font = new System.Drawing.Font("Courier New", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxDissasembly.Location = new System.Drawing.Point(3, 23);
            this.richTextBoxDissasembly.Margin = new System.Windows.Forms.Padding(2);
            this.richTextBoxDissasembly.Name = "richTextBoxDissasembly";
            this.richTextBoxDissasembly.ReadOnly = true;
            this.richTextBoxDissasembly.Size = new System.Drawing.Size(910, 438);
            this.richTextBoxDissasembly.TabIndex = 0;
            this.richTextBoxDissasembly.Text = "";
            // 
            // tabPageHacks
            // 
            this.tabPageHacks.Controls.Add(this.splitContainerHacks);
            this.tabPageHacks.Location = new System.Drawing.Point(4, 22);
            this.tabPageHacks.Name = "tabPageHacks";
            this.tabPageHacks.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHacks.Size = new System.Drawing.Size(915, 463);
            this.tabPageHacks.TabIndex = 12;
            this.tabPageHacks.Text = "Hacks";
            // 
            // splitContainerHacks
            // 
            this.splitContainerHacks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerHacks.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerHacks.Location = new System.Drawing.Point(6, 3);
            this.splitContainerHacks.Name = "splitContainerHacks";
            // 
            // splitContainerHacks.Panel1
            // 
            this.splitContainerHacks.Panel1.Controls.Add(this.groupBoxHackRam);
            this.splitContainerHacks.Panel1MinSize = 0;
            // 
            // splitContainerHacks.Panel2
            // 
            this.splitContainerHacks.Panel2.Controls.Add(this.groupBoxHackSpawn);
            this.splitContainerHacks.Panel2MinSize = 0;
            this.splitContainerHacks.Size = new System.Drawing.Size(906, 454);
            this.splitContainerHacks.SplitterDistance = 301;
            this.splitContainerHacks.SplitterWidth = 1;
            this.splitContainerHacks.TabIndex = 14;
            // 
            // groupBoxHackRam
            // 
            this.groupBoxHackRam.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxHackRam.Controls.Add(this.labelPureInterpretterRequire);
            this.groupBoxHackRam.Controls.Add(this.checkedListBoxHacks);
            this.groupBoxHackRam.Location = new System.Drawing.Point(3, 3);
            this.groupBoxHackRam.Name = "groupBoxHackRam";
            this.groupBoxHackRam.Size = new System.Drawing.Size(295, 448);
            this.groupBoxHackRam.TabIndex = 13;
            this.groupBoxHackRam.TabStop = false;
            this.groupBoxHackRam.Text = "RAM Hacks*";
            // 
            // labelPureInterpretterRequire
            // 
            this.labelPureInterpretterRequire.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPureInterpretterRequire.AutoSize = true;
            this.labelPureInterpretterRequire.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPureInterpretterRequire.Location = new System.Drawing.Point(166, 1);
            this.labelPureInterpretterRequire.Name = "labelPureInterpretterRequire";
            this.labelPureInterpretterRequire.Size = new System.Drawing.Size(129, 13);
            this.labelPureInterpretterRequire.TabIndex = 8;
            this.labelPureInterpretterRequire.Text = "*Requires Pure Interpreter";
            // 
            // checkedListBoxHacks
            // 
            this.checkedListBoxHacks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxHacks.FormattingEnabled = true;
            this.checkedListBoxHacks.Location = new System.Drawing.Point(6, 19);
            this.checkedListBoxHacks.Name = "checkedListBoxHacks";
            this.checkedListBoxHacks.Size = new System.Drawing.Size(283, 349);
            this.checkedListBoxHacks.TabIndex = 9;
            // 
            // groupBoxHackSpawn
            // 
            this.groupBoxHackSpawn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxHackSpawn.Controls.Add(this.labelSpawnBehavior);
            this.groupBoxHackSpawn.Controls.Add(this.textBoxSpawnBehavior);
            this.groupBoxHackSpawn.Controls.Add(this.labelSpawnHint);
            this.groupBoxHackSpawn.Controls.Add(this.buttonSpawnReset);
            this.groupBoxHackSpawn.Controls.Add(this.labelSpawnExtra);
            this.groupBoxHackSpawn.Controls.Add(this.labelSpawnGfxId);
            this.groupBoxHackSpawn.Controls.Add(this.textBoxSpawnExtra);
            this.groupBoxHackSpawn.Controls.Add(this.textBoxSpawnGfxId);
            this.groupBoxHackSpawn.Controls.Add(this.buttonHackSpawn);
            this.groupBoxHackSpawn.Controls.Add(this.listBoxSpawn);
            this.groupBoxHackSpawn.Location = new System.Drawing.Point(4, 4);
            this.groupBoxHackSpawn.Name = "groupBoxHackSpawn";
            this.groupBoxHackSpawn.Size = new System.Drawing.Size(714, 447);
            this.groupBoxHackSpawn.TabIndex = 0;
            this.groupBoxHackSpawn.TabStop = false;
            this.groupBoxHackSpawn.Text = "Spawner";
            // 
            // labelSpawnBehavior
            // 
            this.labelSpawnBehavior.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelSpawnBehavior.AutoSize = true;
            this.labelSpawnBehavior.Location = new System.Drawing.Point(3, 399);
            this.labelSpawnBehavior.Name = "labelSpawnBehavior";
            this.labelSpawnBehavior.Size = new System.Drawing.Size(52, 13);
            this.labelSpawnBehavior.TabIndex = 27;
            this.labelSpawnBehavior.Text = "Behavior:";
            // 
            // textBoxSpawnBehavior
            // 
            this.textBoxSpawnBehavior.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxSpawnBehavior.Location = new System.Drawing.Point(57, 396);
            this.textBoxSpawnBehavior.Name = "textBoxSpawnBehavior";
            this.textBoxSpawnBehavior.Size = new System.Drawing.Size(138, 20);
            this.textBoxSpawnBehavior.TabIndex = 26;
            // 
            // labelSpawnHint
            // 
            this.labelSpawnHint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelSpawnHint.AutoSize = true;
            this.labelSpawnHint.Location = new System.Drawing.Point(315, 426);
            this.labelSpawnHint.Name = "labelSpawnHint";
            this.labelSpawnHint.Size = new System.Drawing.Size(127, 13);
            this.labelSpawnHint.TabIndex = 25;
            this.labelSpawnHint.Text = "(Press L button to spawn)";
            // 
            // buttonSpawnReset
            // 
            this.buttonSpawnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSpawnReset.Location = new System.Drawing.Point(560, 422);
            this.buttonSpawnReset.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSpawnReset.Name = "buttonSpawnReset";
            this.buttonSpawnReset.Size = new System.Drawing.Size(110, 21);
            this.buttonSpawnReset.TabIndex = 24;
            this.buttonSpawnReset.Text = "Reset (Turn Off)";
            this.buttonSpawnReset.UseVisualStyleBackColor = true;
            // 
            // labelSpawnExtra
            // 
            this.labelSpawnExtra.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelSpawnExtra.AutoSize = true;
            this.labelSpawnExtra.Location = new System.Drawing.Point(109, 425);
            this.labelSpawnExtra.Name = "labelSpawnExtra";
            this.labelSpawnExtra.Size = new System.Drawing.Size(34, 13);
            this.labelSpawnExtra.TabIndex = 23;
            this.labelSpawnExtra.Text = "Extra:";
            // 
            // labelSpawnGfxId
            // 
            this.labelSpawnGfxId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelSpawnGfxId.AutoSize = true;
            this.labelSpawnGfxId.Location = new System.Drawing.Point(6, 426);
            this.labelSpawnGfxId.Name = "labelSpawnGfxId";
            this.labelSpawnGfxId.Size = new System.Drawing.Size(45, 13);
            this.labelSpawnGfxId.TabIndex = 22;
            this.labelSpawnGfxId.Text = "GFX ID:";
            // 
            // textBoxSpawnExtra
            // 
            this.textBoxSpawnExtra.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxSpawnExtra.Location = new System.Drawing.Point(149, 422);
            this.textBoxSpawnExtra.Name = "textBoxSpawnExtra";
            this.textBoxSpawnExtra.Size = new System.Drawing.Size(46, 20);
            this.textBoxSpawnExtra.TabIndex = 21;
            // 
            // textBoxSpawnGfxId
            // 
            this.textBoxSpawnGfxId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxSpawnGfxId.Location = new System.Drawing.Point(57, 422);
            this.textBoxSpawnGfxId.Name = "textBoxSpawnGfxId";
            this.textBoxSpawnGfxId.Size = new System.Drawing.Size(46, 20);
            this.textBoxSpawnGfxId.TabIndex = 20;
            // 
            // buttonHackSpawn
            // 
            this.buttonHackSpawn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonHackSpawn.Location = new System.Drawing.Point(200, 422);
            this.buttonHackSpawn.Margin = new System.Windows.Forms.Padding(2);
            this.buttonHackSpawn.Name = "buttonHackSpawn";
            this.buttonHackSpawn.Size = new System.Drawing.Size(110, 21);
            this.buttonHackSpawn.TabIndex = 19;
            this.buttonHackSpawn.Text = "Set Spawn Type";
            this.buttonHackSpawn.UseVisualStyleBackColor = true;
            // 
            // listBoxSpawn
            // 
            this.listBoxSpawn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxSpawn.FormattingEnabled = true;
            this.listBoxSpawn.Location = new System.Drawing.Point(6, 19);
            this.listBoxSpawn.Name = "listBoxSpawn";
            this.listBoxSpawn.Size = new System.Drawing.Size(663, 342);
            this.listBoxSpawn.Sorted = true;
            this.listBoxSpawn.TabIndex = 12;
            // 
            // tabPageCamHack
            // 
            this.tabPageCamHack.Controls.Add(this.splitContainerCamHack);
            this.tabPageCamHack.Location = new System.Drawing.Point(4, 22);
            this.tabPageCamHack.Name = "tabPageCamHack";
            this.tabPageCamHack.Size = new System.Drawing.Size(915, 463);
            this.tabPageCamHack.TabIndex = 17;
            this.tabPageCamHack.Text = "Cam Hack";
            // 
            // splitContainerCamHack
            // 
            this.splitContainerCamHack.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerCamHack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerCamHack.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerCamHack.Location = new System.Drawing.Point(0, 0);
            this.splitContainerCamHack.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainerCamHack.Name = "splitContainerCamHack";
            // 
            // splitContainerCamHack.Panel1
            // 
            this.splitContainerCamHack.Panel1.AutoScroll = true;
            this.splitContainerCamHack.Panel1.Controls.Add(this.groupBoxCameraHackSphericalFocusPos);
            this.splitContainerCamHack.Panel1.Controls.Add(this.groupBoxCameraHackFocusPos);
            this.splitContainerCamHack.Panel1.Controls.Add(this.groupBoxCameraHackSphericalPos);
            this.splitContainerCamHack.Panel1.Controls.Add(this.groupBoxCameraHackPos);
            this.splitContainerCamHack.Panel1.Controls.Add(this.labelCamHackMode);
            this.splitContainerCamHack.Panel1.Controls.Add(this.radioButtonCamHackMode3);
            this.splitContainerCamHack.Panel1.Controls.Add(this.radioButtonCamHackMode0);
            this.splitContainerCamHack.Panel1.Controls.Add(this.radioButtonCamHackMode2);
            this.splitContainerCamHack.Panel1.Controls.Add(this.radioButtonCamHackMode1AbsoluteAngle);
            this.splitContainerCamHack.Panel1.Controls.Add(this.radioButtonCamHackMode1RelativeAngle);
            this.splitContainerCamHack.Panel1MinSize = 0;
            // 
            // splitContainerCamHack.Panel2
            // 
            this.splitContainerCamHack.Panel2.Controls.Add(this.noTearFlowLayoutPanelCamHack);
            this.splitContainerCamHack.Panel2MinSize = 0;
            this.splitContainerCamHack.Size = new System.Drawing.Size(915, 463);
            this.splitContainerCamHack.SplitterDistance = 439;
            this.splitContainerCamHack.SplitterWidth = 1;
            this.splitContainerCamHack.TabIndex = 18;
            // 
            // groupBoxCameraHackSphericalFocusPos
            // 
            this.groupBoxCameraHackSphericalFocusPos.Controls.Add(this.textBoxCameraHackSphericalFocusPosR);
            this.groupBoxCameraHackSphericalFocusPos.Controls.Add(this.buttonCameraHackSphericalFocusPosRp);
            this.groupBoxCameraHackSphericalFocusPos.Controls.Add(this.buttonCameraHackSphericalFocusPosRn);
            this.groupBoxCameraHackSphericalFocusPos.Controls.Add(this.buttonCameraHackSphericalFocusPosTnPp);
            this.groupBoxCameraHackSphericalFocusPos.Controls.Add(this.textBoxCameraHackSphericalFocusPosTP);
            this.groupBoxCameraHackSphericalFocusPos.Controls.Add(this.buttonCameraHackSphericalFocusPosTn);
            this.groupBoxCameraHackSphericalFocusPos.Controls.Add(this.buttonCameraHackSphericalFocusPosTnPn);
            this.groupBoxCameraHackSphericalFocusPos.Controls.Add(this.buttonCameraHackSphericalFocusPosPn);
            this.groupBoxCameraHackSphericalFocusPos.Controls.Add(this.buttonCameraHackSphericalFocusPosPp);
            this.groupBoxCameraHackSphericalFocusPos.Controls.Add(this.buttonCameraHackSphericalFocusPosTpPp);
            this.groupBoxCameraHackSphericalFocusPos.Controls.Add(this.buttonCameraHackSphericalFocusPosTp);
            this.groupBoxCameraHackSphericalFocusPos.Controls.Add(this.buttonCameraHackSphericalFocusPosTpPn);
            this.groupBoxCameraHackSphericalFocusPos.Location = new System.Drawing.Point(227, 238);
            this.groupBoxCameraHackSphericalFocusPos.Name = "groupBoxCameraHackSphericalFocusPos";
            this.groupBoxCameraHackSphericalFocusPos.Size = new System.Drawing.Size(185, 146);
            this.groupBoxCameraHackSphericalFocusPos.TabIndex = 33;
            this.groupBoxCameraHackSphericalFocusPos.TabStop = false;
            this.groupBoxCameraHackSphericalFocusPos.Text = "Focus Spherical";
            // 
            // textBoxCameraHackSphericalFocusPosR
            // 
            this.textBoxCameraHackSphericalFocusPosR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCameraHackSphericalFocusPosR.Location = new System.Drawing.Point(140, 70);
            this.textBoxCameraHackSphericalFocusPosR.Name = "textBoxCameraHackSphericalFocusPosR";
            this.textBoxCameraHackSphericalFocusPosR.Size = new System.Drawing.Size(42, 20);
            this.textBoxCameraHackSphericalFocusPosR.TabIndex = 33;
            this.textBoxCameraHackSphericalFocusPosR.Text = "100";
            this.textBoxCameraHackSphericalFocusPosR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonCameraHackSphericalFocusPosRp
            // 
            this.buttonCameraHackSphericalFocusPosRp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCameraHackSphericalFocusPosRp.Location = new System.Drawing.Point(140, 16);
            this.buttonCameraHackSphericalFocusPosRp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackSphericalFocusPosRp.Name = "buttonCameraHackSphericalFocusPosRp";
            this.buttonCameraHackSphericalFocusPosRp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackSphericalFocusPosRp.TabIndex = 35;
            this.buttonCameraHackSphericalFocusPosRp.Text = "R+";
            this.buttonCameraHackSphericalFocusPosRp.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackSphericalFocusPosRn
            // 
            this.buttonCameraHackSphericalFocusPosRn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCameraHackSphericalFocusPosRn.Location = new System.Drawing.Point(140, 100);
            this.buttonCameraHackSphericalFocusPosRn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackSphericalFocusPosRn.Name = "buttonCameraHackSphericalFocusPosRn";
            this.buttonCameraHackSphericalFocusPosRn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackSphericalFocusPosRn.TabIndex = 34;
            this.buttonCameraHackSphericalFocusPosRn.Text = "R-";
            this.buttonCameraHackSphericalFocusPosRn.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackSphericalFocusPosTnPp
            // 
            this.buttonCameraHackSphericalFocusPosTnPp.Location = new System.Drawing.Point(87, 100);
            this.buttonCameraHackSphericalFocusPosTnPp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackSphericalFocusPosTnPp.Name = "buttonCameraHackSphericalFocusPosTnPp";
            this.buttonCameraHackSphericalFocusPosTnPp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackSphericalFocusPosTnPp.TabIndex = 32;
            this.buttonCameraHackSphericalFocusPosTnPp.Text = "θ-ϕ+";
            this.buttonCameraHackSphericalFocusPosTnPp.UseVisualStyleBackColor = true;
            // 
            // textBoxCameraHackSphericalFocusPosTP
            // 
            this.textBoxCameraHackSphericalFocusPosTP.Location = new System.Drawing.Point(45, 70);
            this.textBoxCameraHackSphericalFocusPosTP.Name = "textBoxCameraHackSphericalFocusPosTP";
            this.textBoxCameraHackSphericalFocusPosTP.Size = new System.Drawing.Size(42, 20);
            this.textBoxCameraHackSphericalFocusPosTP.TabIndex = 27;
            this.textBoxCameraHackSphericalFocusPosTP.Text = "1024";
            this.textBoxCameraHackSphericalFocusPosTP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonCameraHackSphericalFocusPosTn
            // 
            this.buttonCameraHackSphericalFocusPosTn.Location = new System.Drawing.Point(87, 58);
            this.buttonCameraHackSphericalFocusPosTn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackSphericalFocusPosTn.Name = "buttonCameraHackSphericalFocusPosTn";
            this.buttonCameraHackSphericalFocusPosTn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackSphericalFocusPosTn.TabIndex = 31;
            this.buttonCameraHackSphericalFocusPosTn.Text = "θ-";
            this.buttonCameraHackSphericalFocusPosTn.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackSphericalFocusPosTnPn
            // 
            this.buttonCameraHackSphericalFocusPosTnPn.Location = new System.Drawing.Point(87, 16);
            this.buttonCameraHackSphericalFocusPosTnPn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackSphericalFocusPosTnPn.Name = "buttonCameraHackSphericalFocusPosTnPn";
            this.buttonCameraHackSphericalFocusPosTnPn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackSphericalFocusPosTnPn.TabIndex = 30;
            this.buttonCameraHackSphericalFocusPosTnPn.Text = "θ-ϕ-";
            this.buttonCameraHackSphericalFocusPosTnPn.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackSphericalFocusPosPn
            // 
            this.buttonCameraHackSphericalFocusPosPn.Location = new System.Drawing.Point(45, 16);
            this.buttonCameraHackSphericalFocusPosPn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackSphericalFocusPosPn.Name = "buttonCameraHackSphericalFocusPosPn";
            this.buttonCameraHackSphericalFocusPosPn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackSphericalFocusPosPn.TabIndex = 29;
            this.buttonCameraHackSphericalFocusPosPn.Text = "ϕ-";
            this.buttonCameraHackSphericalFocusPosPn.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackSphericalFocusPosPp
            // 
            this.buttonCameraHackSphericalFocusPosPp.Location = new System.Drawing.Point(45, 100);
            this.buttonCameraHackSphericalFocusPosPp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackSphericalFocusPosPp.Name = "buttonCameraHackSphericalFocusPosPp";
            this.buttonCameraHackSphericalFocusPosPp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackSphericalFocusPosPp.TabIndex = 28;
            this.buttonCameraHackSphericalFocusPosPp.Text = "ϕ+";
            this.buttonCameraHackSphericalFocusPosPp.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackSphericalFocusPosTpPp
            // 
            this.buttonCameraHackSphericalFocusPosTpPp.Location = new System.Drawing.Point(3, 100);
            this.buttonCameraHackSphericalFocusPosTpPp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackSphericalFocusPosTpPp.Name = "buttonCameraHackSphericalFocusPosTpPp";
            this.buttonCameraHackSphericalFocusPosTpPp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackSphericalFocusPosTpPp.TabIndex = 27;
            this.buttonCameraHackSphericalFocusPosTpPp.Text = "θ+ϕ+";
            this.buttonCameraHackSphericalFocusPosTpPp.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackSphericalFocusPosTp
            // 
            this.buttonCameraHackSphericalFocusPosTp.Location = new System.Drawing.Point(3, 58);
            this.buttonCameraHackSphericalFocusPosTp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackSphericalFocusPosTp.Name = "buttonCameraHackSphericalFocusPosTp";
            this.buttonCameraHackSphericalFocusPosTp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackSphericalFocusPosTp.TabIndex = 26;
            this.buttonCameraHackSphericalFocusPosTp.Text = "θ+";
            this.buttonCameraHackSphericalFocusPosTp.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackSphericalFocusPosTpPn
            // 
            this.buttonCameraHackSphericalFocusPosTpPn.Location = new System.Drawing.Point(3, 16);
            this.buttonCameraHackSphericalFocusPosTpPn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackSphericalFocusPosTpPn.Name = "buttonCameraHackSphericalFocusPosTpPn";
            this.buttonCameraHackSphericalFocusPosTpPn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackSphericalFocusPosTpPn.TabIndex = 25;
            this.buttonCameraHackSphericalFocusPosTpPn.Text = "θ+ϕ-";
            this.buttonCameraHackSphericalFocusPosTpPn.UseVisualStyleBackColor = true;
            // 
            // groupBoxCameraHackFocusPos
            // 
            this.groupBoxCameraHackFocusPos.Controls.Add(this.checkBoxCameraHackFocusPosRelative);
            this.groupBoxCameraHackFocusPos.Controls.Add(this.textBoxCameraHackFocusPosY);
            this.groupBoxCameraHackFocusPos.Controls.Add(this.buttonCameraHackFocusPosYp);
            this.groupBoxCameraHackFocusPos.Controls.Add(this.buttonCameraHackFocusPosYn);
            this.groupBoxCameraHackFocusPos.Controls.Add(this.buttonCameraHackFocusPosXpZp);
            this.groupBoxCameraHackFocusPos.Controls.Add(this.textBoxCameraHackFocusPosXZ);
            this.groupBoxCameraHackFocusPos.Controls.Add(this.buttonCameraHackFocusPosXp);
            this.groupBoxCameraHackFocusPos.Controls.Add(this.buttonCameraHackFocusPosXpZn);
            this.groupBoxCameraHackFocusPos.Controls.Add(this.buttonCameraHackFocusPosZn);
            this.groupBoxCameraHackFocusPos.Controls.Add(this.buttonCameraHackFocusPosZp);
            this.groupBoxCameraHackFocusPos.Controls.Add(this.buttonCameraHackFocusPosXnZp);
            this.groupBoxCameraHackFocusPos.Controls.Add(this.buttonCameraHackFocusPosXn);
            this.groupBoxCameraHackFocusPos.Controls.Add(this.buttonCameraHackFocusPosXnZn);
            this.groupBoxCameraHackFocusPos.Location = new System.Drawing.Point(227, 86);
            this.groupBoxCameraHackFocusPos.Name = "groupBoxCameraHackFocusPos";
            this.groupBoxCameraHackFocusPos.Size = new System.Drawing.Size(185, 146);
            this.groupBoxCameraHackFocusPos.TabIndex = 34;
            this.groupBoxCameraHackFocusPos.TabStop = false;
            this.groupBoxCameraHackFocusPos.Text = "Focus Position";
            // 
            // checkBoxCameraHackFocusPosRelative
            // 
            this.checkBoxCameraHackFocusPosRelative.AutoSize = true;
            this.checkBoxCameraHackFocusPosRelative.Location = new System.Drawing.Point(120, 0);
            this.checkBoxCameraHackFocusPosRelative.Name = "checkBoxCameraHackFocusPosRelative";
            this.checkBoxCameraHackFocusPosRelative.Size = new System.Drawing.Size(65, 17);
            this.checkBoxCameraHackFocusPosRelative.TabIndex = 37;
            this.checkBoxCameraHackFocusPosRelative.Text = "Relative";
            this.checkBoxCameraHackFocusPosRelative.UseVisualStyleBackColor = true;
            // 
            // textBoxCameraHackFocusPosY
            // 
            this.textBoxCameraHackFocusPosY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCameraHackFocusPosY.Location = new System.Drawing.Point(140, 70);
            this.textBoxCameraHackFocusPosY.Name = "textBoxCameraHackFocusPosY";
            this.textBoxCameraHackFocusPosY.Size = new System.Drawing.Size(42, 20);
            this.textBoxCameraHackFocusPosY.TabIndex = 33;
            this.textBoxCameraHackFocusPosY.Text = "100";
            this.textBoxCameraHackFocusPosY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonCameraHackFocusPosYp
            // 
            this.buttonCameraHackFocusPosYp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCameraHackFocusPosYp.Location = new System.Drawing.Point(140, 16);
            this.buttonCameraHackFocusPosYp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackFocusPosYp.Name = "buttonCameraHackFocusPosYp";
            this.buttonCameraHackFocusPosYp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackFocusPosYp.TabIndex = 35;
            this.buttonCameraHackFocusPosYp.Text = "Y+";
            this.buttonCameraHackFocusPosYp.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackFocusPosYn
            // 
            this.buttonCameraHackFocusPosYn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCameraHackFocusPosYn.Location = new System.Drawing.Point(140, 100);
            this.buttonCameraHackFocusPosYn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackFocusPosYn.Name = "buttonCameraHackFocusPosYn";
            this.buttonCameraHackFocusPosYn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackFocusPosYn.TabIndex = 34;
            this.buttonCameraHackFocusPosYn.Text = "Y-";
            this.buttonCameraHackFocusPosYn.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackFocusPosXpZp
            // 
            this.buttonCameraHackFocusPosXpZp.Location = new System.Drawing.Point(87, 100);
            this.buttonCameraHackFocusPosXpZp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackFocusPosXpZp.Name = "buttonCameraHackFocusPosXpZp";
            this.buttonCameraHackFocusPosXpZp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackFocusPosXpZp.TabIndex = 32;
            this.buttonCameraHackFocusPosXpZp.Text = "X+Z+";
            this.buttonCameraHackFocusPosXpZp.UseVisualStyleBackColor = true;
            // 
            // textBoxCameraHackFocusPosXZ
            // 
            this.textBoxCameraHackFocusPosXZ.Location = new System.Drawing.Point(45, 70);
            this.textBoxCameraHackFocusPosXZ.Name = "textBoxCameraHackFocusPosXZ";
            this.textBoxCameraHackFocusPosXZ.Size = new System.Drawing.Size(42, 20);
            this.textBoxCameraHackFocusPosXZ.TabIndex = 27;
            this.textBoxCameraHackFocusPosXZ.Text = "100";
            this.textBoxCameraHackFocusPosXZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonCameraHackFocusPosXp
            // 
            this.buttonCameraHackFocusPosXp.Location = new System.Drawing.Point(87, 58);
            this.buttonCameraHackFocusPosXp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackFocusPosXp.Name = "buttonCameraHackFocusPosXp";
            this.buttonCameraHackFocusPosXp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackFocusPosXp.TabIndex = 31;
            this.buttonCameraHackFocusPosXp.Text = "X+";
            this.buttonCameraHackFocusPosXp.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackFocusPosXpZn
            // 
            this.buttonCameraHackFocusPosXpZn.Location = new System.Drawing.Point(87, 16);
            this.buttonCameraHackFocusPosXpZn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackFocusPosXpZn.Name = "buttonCameraHackFocusPosXpZn";
            this.buttonCameraHackFocusPosXpZn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackFocusPosXpZn.TabIndex = 30;
            this.buttonCameraHackFocusPosXpZn.Text = "X+Z-";
            this.buttonCameraHackFocusPosXpZn.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackFocusPosZn
            // 
            this.buttonCameraHackFocusPosZn.Location = new System.Drawing.Point(45, 16);
            this.buttonCameraHackFocusPosZn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackFocusPosZn.Name = "buttonCameraHackFocusPosZn";
            this.buttonCameraHackFocusPosZn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackFocusPosZn.TabIndex = 29;
            this.buttonCameraHackFocusPosZn.Text = "Z-";
            this.buttonCameraHackFocusPosZn.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackFocusPosZp
            // 
            this.buttonCameraHackFocusPosZp.Location = new System.Drawing.Point(45, 100);
            this.buttonCameraHackFocusPosZp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackFocusPosZp.Name = "buttonCameraHackFocusPosZp";
            this.buttonCameraHackFocusPosZp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackFocusPosZp.TabIndex = 28;
            this.buttonCameraHackFocusPosZp.Text = "Z+";
            this.buttonCameraHackFocusPosZp.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackFocusPosXnZp
            // 
            this.buttonCameraHackFocusPosXnZp.Location = new System.Drawing.Point(3, 100);
            this.buttonCameraHackFocusPosXnZp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackFocusPosXnZp.Name = "buttonCameraHackFocusPosXnZp";
            this.buttonCameraHackFocusPosXnZp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackFocusPosXnZp.TabIndex = 27;
            this.buttonCameraHackFocusPosXnZp.Text = "X-Z+";
            this.buttonCameraHackFocusPosXnZp.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackFocusPosXn
            // 
            this.buttonCameraHackFocusPosXn.Location = new System.Drawing.Point(3, 58);
            this.buttonCameraHackFocusPosXn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackFocusPosXn.Name = "buttonCameraHackFocusPosXn";
            this.buttonCameraHackFocusPosXn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackFocusPosXn.TabIndex = 26;
            this.buttonCameraHackFocusPosXn.Text = "X-";
            this.buttonCameraHackFocusPosXn.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackFocusPosXnZn
            // 
            this.buttonCameraHackFocusPosXnZn.Location = new System.Drawing.Point(3, 16);
            this.buttonCameraHackFocusPosXnZn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackFocusPosXnZn.Name = "buttonCameraHackFocusPosXnZn";
            this.buttonCameraHackFocusPosXnZn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackFocusPosXnZn.TabIndex = 25;
            this.buttonCameraHackFocusPosXnZn.Text = "X-Z-";
            this.buttonCameraHackFocusPosXnZn.UseVisualStyleBackColor = true;
            // 
            // groupBoxCameraHackSphericalPos
            // 
            this.groupBoxCameraHackSphericalPos.Controls.Add(this.textBoxCameraHackSphericalPosR);
            this.groupBoxCameraHackSphericalPos.Controls.Add(this.buttonCameraHackSphericalPosRn);
            this.groupBoxCameraHackSphericalPos.Controls.Add(this.buttonCameraHackSphericalPosRp);
            this.groupBoxCameraHackSphericalPos.Controls.Add(this.buttonCameraHackSphericalPosTpPp);
            this.groupBoxCameraHackSphericalPos.Controls.Add(this.textBoxCameraHackSphericalPosTP);
            this.groupBoxCameraHackSphericalPos.Controls.Add(this.buttonCameraHackSphericalPosTp);
            this.groupBoxCameraHackSphericalPos.Controls.Add(this.buttonCameraHackSphericalPosTpPn);
            this.groupBoxCameraHackSphericalPos.Controls.Add(this.buttonCameraHackSphericalPosPn);
            this.groupBoxCameraHackSphericalPos.Controls.Add(this.buttonCameraHackSphericalPosPp);
            this.groupBoxCameraHackSphericalPos.Controls.Add(this.buttonCameraHackSphericalPosTnPp);
            this.groupBoxCameraHackSphericalPos.Controls.Add(this.buttonCameraHackSphericalPosTn);
            this.groupBoxCameraHackSphericalPos.Controls.Add(this.buttonCameraHackSphericalPosTnPn);
            this.groupBoxCameraHackSphericalPos.Location = new System.Drawing.Point(11, 236);
            this.groupBoxCameraHackSphericalPos.Name = "groupBoxCameraHackSphericalPos";
            this.groupBoxCameraHackSphericalPos.Size = new System.Drawing.Size(185, 146);
            this.groupBoxCameraHackSphericalPos.TabIndex = 31;
            this.groupBoxCameraHackSphericalPos.TabStop = false;
            this.groupBoxCameraHackSphericalPos.Text = "Camera Spherical";
            // 
            // textBoxCameraHackSphericalPosR
            // 
            this.textBoxCameraHackSphericalPosR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCameraHackSphericalPosR.Location = new System.Drawing.Point(140, 70);
            this.textBoxCameraHackSphericalPosR.Name = "textBoxCameraHackSphericalPosR";
            this.textBoxCameraHackSphericalPosR.Size = new System.Drawing.Size(42, 20);
            this.textBoxCameraHackSphericalPosR.TabIndex = 33;
            this.textBoxCameraHackSphericalPosR.Text = "100";
            this.textBoxCameraHackSphericalPosR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonCameraHackSphericalPosRn
            // 
            this.buttonCameraHackSphericalPosRn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCameraHackSphericalPosRn.Location = new System.Drawing.Point(140, 16);
            this.buttonCameraHackSphericalPosRn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackSphericalPosRn.Name = "buttonCameraHackSphericalPosRn";
            this.buttonCameraHackSphericalPosRn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackSphericalPosRn.TabIndex = 35;
            this.buttonCameraHackSphericalPosRn.Text = "R-";
            this.buttonCameraHackSphericalPosRn.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackSphericalPosRp
            // 
            this.buttonCameraHackSphericalPosRp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCameraHackSphericalPosRp.Location = new System.Drawing.Point(140, 100);
            this.buttonCameraHackSphericalPosRp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackSphericalPosRp.Name = "buttonCameraHackSphericalPosRp";
            this.buttonCameraHackSphericalPosRp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackSphericalPosRp.TabIndex = 34;
            this.buttonCameraHackSphericalPosRp.Text = "R+";
            this.buttonCameraHackSphericalPosRp.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackSphericalPosTpPp
            // 
            this.buttonCameraHackSphericalPosTpPp.Location = new System.Drawing.Point(87, 100);
            this.buttonCameraHackSphericalPosTpPp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackSphericalPosTpPp.Name = "buttonCameraHackSphericalPosTpPp";
            this.buttonCameraHackSphericalPosTpPp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackSphericalPosTpPp.TabIndex = 32;
            this.buttonCameraHackSphericalPosTpPp.Text = "θ+ϕ+";
            this.buttonCameraHackSphericalPosTpPp.UseVisualStyleBackColor = true;
            // 
            // textBoxCameraHackSphericalPosTP
            // 
            this.textBoxCameraHackSphericalPosTP.Location = new System.Drawing.Point(45, 70);
            this.textBoxCameraHackSphericalPosTP.Name = "textBoxCameraHackSphericalPosTP";
            this.textBoxCameraHackSphericalPosTP.Size = new System.Drawing.Size(42, 20);
            this.textBoxCameraHackSphericalPosTP.TabIndex = 27;
            this.textBoxCameraHackSphericalPosTP.Text = "1024";
            this.textBoxCameraHackSphericalPosTP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonCameraHackSphericalPosTp
            // 
            this.buttonCameraHackSphericalPosTp.Location = new System.Drawing.Point(87, 58);
            this.buttonCameraHackSphericalPosTp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackSphericalPosTp.Name = "buttonCameraHackSphericalPosTp";
            this.buttonCameraHackSphericalPosTp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackSphericalPosTp.TabIndex = 31;
            this.buttonCameraHackSphericalPosTp.Text = "θ+";
            this.buttonCameraHackSphericalPosTp.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackSphericalPosTpPn
            // 
            this.buttonCameraHackSphericalPosTpPn.Location = new System.Drawing.Point(87, 16);
            this.buttonCameraHackSphericalPosTpPn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackSphericalPosTpPn.Name = "buttonCameraHackSphericalPosTpPn";
            this.buttonCameraHackSphericalPosTpPn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackSphericalPosTpPn.TabIndex = 30;
            this.buttonCameraHackSphericalPosTpPn.Text = "θ+ϕ-";
            this.buttonCameraHackSphericalPosTpPn.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackSphericalPosPn
            // 
            this.buttonCameraHackSphericalPosPn.Location = new System.Drawing.Point(45, 16);
            this.buttonCameraHackSphericalPosPn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackSphericalPosPn.Name = "buttonCameraHackSphericalPosPn";
            this.buttonCameraHackSphericalPosPn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackSphericalPosPn.TabIndex = 29;
            this.buttonCameraHackSphericalPosPn.Text = "ϕ-";
            this.buttonCameraHackSphericalPosPn.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackSphericalPosPp
            // 
            this.buttonCameraHackSphericalPosPp.Location = new System.Drawing.Point(45, 100);
            this.buttonCameraHackSphericalPosPp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackSphericalPosPp.Name = "buttonCameraHackSphericalPosPp";
            this.buttonCameraHackSphericalPosPp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackSphericalPosPp.TabIndex = 28;
            this.buttonCameraHackSphericalPosPp.Text = "ϕ+";
            this.buttonCameraHackSphericalPosPp.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackSphericalPosTnPp
            // 
            this.buttonCameraHackSphericalPosTnPp.Location = new System.Drawing.Point(3, 100);
            this.buttonCameraHackSphericalPosTnPp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackSphericalPosTnPp.Name = "buttonCameraHackSphericalPosTnPp";
            this.buttonCameraHackSphericalPosTnPp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackSphericalPosTnPp.TabIndex = 27;
            this.buttonCameraHackSphericalPosTnPp.Text = "θ-ϕ+";
            this.buttonCameraHackSphericalPosTnPp.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackSphericalPosTn
            // 
            this.buttonCameraHackSphericalPosTn.Location = new System.Drawing.Point(3, 58);
            this.buttonCameraHackSphericalPosTn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackSphericalPosTn.Name = "buttonCameraHackSphericalPosTn";
            this.buttonCameraHackSphericalPosTn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackSphericalPosTn.TabIndex = 26;
            this.buttonCameraHackSphericalPosTn.Text = "θ-";
            this.buttonCameraHackSphericalPosTn.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackSphericalPosTnPn
            // 
            this.buttonCameraHackSphericalPosTnPn.Location = new System.Drawing.Point(3, 16);
            this.buttonCameraHackSphericalPosTnPn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackSphericalPosTnPn.Name = "buttonCameraHackSphericalPosTnPn";
            this.buttonCameraHackSphericalPosTnPn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackSphericalPosTnPn.TabIndex = 25;
            this.buttonCameraHackSphericalPosTnPn.Text = "θ-ϕ-";
            this.buttonCameraHackSphericalPosTnPn.UseVisualStyleBackColor = true;
            // 
            // groupBoxCameraHackPos
            // 
            this.groupBoxCameraHackPos.Controls.Add(this.checkBoxCameraHackPosRelative);
            this.groupBoxCameraHackPos.Controls.Add(this.textBoxCameraHackPosY);
            this.groupBoxCameraHackPos.Controls.Add(this.buttonCameraHackPosYp);
            this.groupBoxCameraHackPos.Controls.Add(this.buttonCameraHackPosYn);
            this.groupBoxCameraHackPos.Controls.Add(this.buttonCameraHackPosXpZp);
            this.groupBoxCameraHackPos.Controls.Add(this.textBoxCameraHackPosXZ);
            this.groupBoxCameraHackPos.Controls.Add(this.buttonCameraHackPosXp);
            this.groupBoxCameraHackPos.Controls.Add(this.buttonCameraHackPosXpZn);
            this.groupBoxCameraHackPos.Controls.Add(this.buttonCameraHackPosZn);
            this.groupBoxCameraHackPos.Controls.Add(this.buttonCameraHackPosZp);
            this.groupBoxCameraHackPos.Controls.Add(this.buttonCameraHackPosXnZp);
            this.groupBoxCameraHackPos.Controls.Add(this.buttonCameraHackPosXn);
            this.groupBoxCameraHackPos.Controls.Add(this.buttonCameraHackPosXnZn);
            this.groupBoxCameraHackPos.Location = new System.Drawing.Point(11, 84);
            this.groupBoxCameraHackPos.Name = "groupBoxCameraHackPos";
            this.groupBoxCameraHackPos.Size = new System.Drawing.Size(185, 146);
            this.groupBoxCameraHackPos.TabIndex = 32;
            this.groupBoxCameraHackPos.TabStop = false;
            this.groupBoxCameraHackPos.Text = "Camera Position";
            // 
            // checkBoxCameraHackPosRelative
            // 
            this.checkBoxCameraHackPosRelative.AutoSize = true;
            this.checkBoxCameraHackPosRelative.Location = new System.Drawing.Point(120, 0);
            this.checkBoxCameraHackPosRelative.Name = "checkBoxCameraHackPosRelative";
            this.checkBoxCameraHackPosRelative.Size = new System.Drawing.Size(65, 17);
            this.checkBoxCameraHackPosRelative.TabIndex = 37;
            this.checkBoxCameraHackPosRelative.Text = "Relative";
            this.checkBoxCameraHackPosRelative.UseVisualStyleBackColor = true;
            // 
            // textBoxCameraHackPosY
            // 
            this.textBoxCameraHackPosY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCameraHackPosY.Location = new System.Drawing.Point(140, 70);
            this.textBoxCameraHackPosY.Name = "textBoxCameraHackPosY";
            this.textBoxCameraHackPosY.Size = new System.Drawing.Size(42, 20);
            this.textBoxCameraHackPosY.TabIndex = 33;
            this.textBoxCameraHackPosY.Text = "100";
            this.textBoxCameraHackPosY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonCameraHackPosYp
            // 
            this.buttonCameraHackPosYp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCameraHackPosYp.Location = new System.Drawing.Point(140, 16);
            this.buttonCameraHackPosYp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackPosYp.Name = "buttonCameraHackPosYp";
            this.buttonCameraHackPosYp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackPosYp.TabIndex = 35;
            this.buttonCameraHackPosYp.Text = "Y+";
            this.buttonCameraHackPosYp.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackPosYn
            // 
            this.buttonCameraHackPosYn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCameraHackPosYn.Location = new System.Drawing.Point(140, 100);
            this.buttonCameraHackPosYn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackPosYn.Name = "buttonCameraHackPosYn";
            this.buttonCameraHackPosYn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackPosYn.TabIndex = 34;
            this.buttonCameraHackPosYn.Text = "Y-";
            this.buttonCameraHackPosYn.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackPosXpZp
            // 
            this.buttonCameraHackPosXpZp.Location = new System.Drawing.Point(87, 100);
            this.buttonCameraHackPosXpZp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackPosXpZp.Name = "buttonCameraHackPosXpZp";
            this.buttonCameraHackPosXpZp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackPosXpZp.TabIndex = 32;
            this.buttonCameraHackPosXpZp.Text = "X+Z+";
            this.buttonCameraHackPosXpZp.UseVisualStyleBackColor = true;
            // 
            // textBoxCameraHackPosXZ
            // 
            this.textBoxCameraHackPosXZ.Location = new System.Drawing.Point(45, 70);
            this.textBoxCameraHackPosXZ.Name = "textBoxCameraHackPosXZ";
            this.textBoxCameraHackPosXZ.Size = new System.Drawing.Size(42, 20);
            this.textBoxCameraHackPosXZ.TabIndex = 27;
            this.textBoxCameraHackPosXZ.Text = "100";
            this.textBoxCameraHackPosXZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonCameraHackPosXp
            // 
            this.buttonCameraHackPosXp.Location = new System.Drawing.Point(87, 58);
            this.buttonCameraHackPosXp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackPosXp.Name = "buttonCameraHackPosXp";
            this.buttonCameraHackPosXp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackPosXp.TabIndex = 31;
            this.buttonCameraHackPosXp.Text = "X+";
            this.buttonCameraHackPosXp.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackPosXpZn
            // 
            this.buttonCameraHackPosXpZn.Location = new System.Drawing.Point(87, 16);
            this.buttonCameraHackPosXpZn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackPosXpZn.Name = "buttonCameraHackPosXpZn";
            this.buttonCameraHackPosXpZn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackPosXpZn.TabIndex = 30;
            this.buttonCameraHackPosXpZn.Text = "X+Z-";
            this.buttonCameraHackPosXpZn.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackPosZn
            // 
            this.buttonCameraHackPosZn.Location = new System.Drawing.Point(45, 16);
            this.buttonCameraHackPosZn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackPosZn.Name = "buttonCameraHackPosZn";
            this.buttonCameraHackPosZn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackPosZn.TabIndex = 29;
            this.buttonCameraHackPosZn.Text = "Z-";
            this.buttonCameraHackPosZn.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackPosZp
            // 
            this.buttonCameraHackPosZp.Location = new System.Drawing.Point(45, 100);
            this.buttonCameraHackPosZp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackPosZp.Name = "buttonCameraHackPosZp";
            this.buttonCameraHackPosZp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackPosZp.TabIndex = 28;
            this.buttonCameraHackPosZp.Text = "Z+";
            this.buttonCameraHackPosZp.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackPosXnZp
            // 
            this.buttonCameraHackPosXnZp.Location = new System.Drawing.Point(3, 100);
            this.buttonCameraHackPosXnZp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackPosXnZp.Name = "buttonCameraHackPosXnZp";
            this.buttonCameraHackPosXnZp.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackPosXnZp.TabIndex = 27;
            this.buttonCameraHackPosXnZp.Text = "X-Z+";
            this.buttonCameraHackPosXnZp.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackPosXn
            // 
            this.buttonCameraHackPosXn.Location = new System.Drawing.Point(3, 58);
            this.buttonCameraHackPosXn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackPosXn.Name = "buttonCameraHackPosXn";
            this.buttonCameraHackPosXn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackPosXn.TabIndex = 26;
            this.buttonCameraHackPosXn.Text = "X-";
            this.buttonCameraHackPosXn.UseVisualStyleBackColor = true;
            // 
            // buttonCameraHackPosXnZn
            // 
            this.buttonCameraHackPosXnZn.Location = new System.Drawing.Point(3, 16);
            this.buttonCameraHackPosXnZn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraHackPosXnZn.Name = "buttonCameraHackPosXnZn";
            this.buttonCameraHackPosXnZn.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraHackPosXnZn.TabIndex = 25;
            this.buttonCameraHackPosXnZn.Text = "X-Z-";
            this.buttonCameraHackPosXnZn.UseVisualStyleBackColor = true;
            // 
            // labelCamHackMode
            // 
            this.labelCamHackMode.AutoSize = true;
            this.labelCamHackMode.Location = new System.Drawing.Point(8, 11);
            this.labelCamHackMode.Name = "labelCamHackMode";
            this.labelCamHackMode.Size = new System.Drawing.Size(76, 13);
            this.labelCamHackMode.TabIndex = 11;
            this.labelCamHackMode.Text = "Camera Mode:";
            // 
            // radioButtonCamHackMode3
            // 
            this.radioButtonCamHackMode3.AutoSize = true;
            this.radioButtonCamHackMode3.Location = new System.Drawing.Point(204, 55);
            this.radioButtonCamHackMode3.Name = "radioButtonCamHackMode3";
            this.radioButtonCamHackMode3.Size = new System.Drawing.Size(151, 17);
            this.radioButtonCamHackMode3.TabIndex = 10;
            this.radioButtonCamHackMode3.Text = "Fixed Position, Fixed Angle";
            this.radioButtonCamHackMode3.UseVisualStyleBackColor = true;
            // 
            // radioButtonCamHackMode0
            // 
            this.radioButtonCamHackMode0.AutoSize = true;
            this.radioButtonCamHackMode0.Checked = true;
            this.radioButtonCamHackMode0.Location = new System.Drawing.Point(204, 9);
            this.radioButtonCamHackMode0.Name = "radioButtonCamHackMode0";
            this.radioButtonCamHackMode0.Size = new System.Drawing.Size(62, 17);
            this.radioButtonCamHackMode0.TabIndex = 7;
            this.radioButtonCamHackMode0.TabStop = true;
            this.radioButtonCamHackMode0.Text = "Regular";
            this.radioButtonCamHackMode0.UseVisualStyleBackColor = true;
            // 
            // radioButtonCamHackMode2
            // 
            this.radioButtonCamHackMode2.AutoSize = true;
            this.radioButtonCamHackMode2.Location = new System.Drawing.Point(204, 32);
            this.radioButtonCamHackMode2.Name = "radioButtonCamHackMode2";
            this.radioButtonCamHackMode2.Size = new System.Drawing.Size(162, 17);
            this.radioButtonCamHackMode2.TabIndex = 9;
            this.radioButtonCamHackMode2.Text = "Fixed Position, Watch Object";
            this.radioButtonCamHackMode2.UseVisualStyleBackColor = true;
            // 
            // radioButtonCamHackMode1AbsoluteAngle
            // 
            this.radioButtonCamHackMode1AbsoluteAngle.AutoSize = true;
            this.radioButtonCamHackMode1AbsoluteAngle.Location = new System.Drawing.Point(11, 55);
            this.radioButtonCamHackMode1AbsoluteAngle.Name = "radioButtonCamHackMode1AbsoluteAngle";
            this.radioButtonCamHackMode1AbsoluteAngle.Size = new System.Drawing.Size(150, 17);
            this.radioButtonCamHackMode1AbsoluteAngle.TabIndex = 8;
            this.radioButtonCamHackMode1AbsoluteAngle.Text = "Follow Object, Fixed Angle";
            this.radioButtonCamHackMode1AbsoluteAngle.UseVisualStyleBackColor = true;
            // 
            // radioButtonCamHackMode1RelativeAngle
            // 
            this.radioButtonCamHackMode1RelativeAngle.AutoSize = true;
            this.radioButtonCamHackMode1RelativeAngle.Location = new System.Drawing.Point(11, 32);
            this.radioButtonCamHackMode1RelativeAngle.Name = "radioButtonCamHackMode1RelativeAngle";
            this.radioButtonCamHackMode1RelativeAngle.Size = new System.Drawing.Size(164, 17);
            this.radioButtonCamHackMode1RelativeAngle.TabIndex = 8;
            this.radioButtonCamHackMode1RelativeAngle.Text = "Follow Object, Relative Angle";
            this.radioButtonCamHackMode1RelativeAngle.UseVisualStyleBackColor = true;
            // 
            // tabPageQuarterFrame
            // 
            this.tabPageQuarterFrame.Controls.Add(this.noTearFlowLayoutPanelQuarterFrame);
            this.tabPageQuarterFrame.Location = new System.Drawing.Point(4, 22);
            this.tabPageQuarterFrame.Name = "tabPageQuarterFrame";
            this.tabPageQuarterFrame.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageQuarterFrame.Size = new System.Drawing.Size(915, 463);
            this.tabPageQuarterFrame.TabIndex = 16;
            this.tabPageQuarterFrame.Text = "Q Frames";
            // 
            // tabPageOptions
            // 
            this.tabPageOptions.Controls.Add(this.groupBoxGotoRetrieveOffsets);
            this.tabPageOptions.Controls.Add(this.checkBoxNeutralizeTriangleWith21);
            this.tabPageOptions.Controls.Add(this.checkBoxDisableActionUpdateWhenCloning);
            this.tabPageOptions.Controls.Add(this.groupBoxShowOverlay);
            this.tabPageOptions.Controls.Add(this.checkBoxScaleDiagonalPositionControllerButtons);
            this.tabPageOptions.Controls.Add(this.label3);
            this.tabPageOptions.Controls.Add(this.checkBoxMoveCamWithPu);
            this.tabPageOptions.Controls.Add(this.checkBoxUseRomHack);
            this.tabPageOptions.Controls.Add(this.checkBoxStartSlotIndexOne);
            this.tabPageOptions.Location = new System.Drawing.Point(4, 22);
            this.tabPageOptions.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageOptions.Name = "tabPageOptions";
            this.tabPageOptions.Size = new System.Drawing.Size(915, 463);
            this.tabPageOptions.TabIndex = 5;
            this.tabPageOptions.Text = "Options";
            // 
            // groupBoxGotoRetrieveOffsets
            // 
            this.groupBoxGotoRetrieveOffsets.Controls.Add(this.labelRetrieveInfrontSuffix);
            this.groupBoxGotoRetrieveOffsets.Controls.Add(this.labelRetrieveInfrontPrefix);
            this.groupBoxGotoRetrieveOffsets.Controls.Add(this.textBoxRetrieveInfront);
            this.groupBoxGotoRetrieveOffsets.Controls.Add(this.labelRetrieveAboveSuffix);
            this.groupBoxGotoRetrieveOffsets.Controls.Add(this.labelRetrieveAbovePrefix);
            this.groupBoxGotoRetrieveOffsets.Controls.Add(this.textBoxRetrieveAbove);
            this.groupBoxGotoRetrieveOffsets.Controls.Add(this.labelGotoInfrontSuffix);
            this.groupBoxGotoRetrieveOffsets.Controls.Add(this.labelGotoInfrontPrefix);
            this.groupBoxGotoRetrieveOffsets.Controls.Add(this.textBoxGotoInfront);
            this.groupBoxGotoRetrieveOffsets.Controls.Add(this.labelGotoAboveSuffix);
            this.groupBoxGotoRetrieveOffsets.Controls.Add(this.labelGotoAbovePrefix);
            this.groupBoxGotoRetrieveOffsets.Controls.Add(this.textBoxGotoAbove);
            this.groupBoxGotoRetrieveOffsets.Location = new System.Drawing.Point(450, 4);
            this.groupBoxGotoRetrieveOffsets.Name = "groupBoxGotoRetrieveOffsets";
            this.groupBoxGotoRetrieveOffsets.Size = new System.Drawing.Size(154, 121);
            this.groupBoxGotoRetrieveOffsets.TabIndex = 32;
            this.groupBoxGotoRetrieveOffsets.TabStop = false;
            this.groupBoxGotoRetrieveOffsets.Text = "Go to / Retrieve Offsets";
            // 
            // labelRetrieveInfrontSuffix
            // 
            this.labelRetrieveInfrontSuffix.AutoSize = true;
            this.labelRetrieveInfrontSuffix.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRetrieveInfrontSuffix.Location = new System.Drawing.Point(106, 99);
            this.labelRetrieveInfrontSuffix.Name = "labelRetrieveInfrontSuffix";
            this.labelRetrieveInfrontSuffix.Size = new System.Drawing.Size(39, 13);
            this.labelRetrieveInfrontSuffix.TabIndex = 39;
            this.labelRetrieveInfrontSuffix.Text = "in front";
            // 
            // labelRetrieveInfrontPrefix
            // 
            this.labelRetrieveInfrontPrefix.AutoSize = true;
            this.labelRetrieveInfrontPrefix.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRetrieveInfrontPrefix.Location = new System.Drawing.Point(11, 98);
            this.labelRetrieveInfrontPrefix.Name = "labelRetrieveInfrontPrefix";
            this.labelRetrieveInfrontPrefix.Size = new System.Drawing.Size(47, 13);
            this.labelRetrieveInfrontPrefix.TabIndex = 38;
            this.labelRetrieveInfrontPrefix.Text = "Retrieve";
            // 
            // textBoxRetrieveInfront
            // 
            this.textBoxRetrieveInfront.Location = new System.Drawing.Point(58, 96);
            this.textBoxRetrieveInfront.Name = "textBoxRetrieveInfront";
            this.textBoxRetrieveInfront.Size = new System.Drawing.Size(42, 20);
            this.textBoxRetrieveInfront.TabIndex = 37;
            this.textBoxRetrieveInfront.Text = "0";
            this.textBoxRetrieveInfront.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxRetrieveInfront.LostFocus += new System.EventHandler(this.textBoxRetrieveInfront_LostFocus);
            // 
            // labelRetrieveAboveSuffix
            // 
            this.labelRetrieveAboveSuffix.AutoSize = true;
            this.labelRetrieveAboveSuffix.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRetrieveAboveSuffix.Location = new System.Drawing.Point(106, 73);
            this.labelRetrieveAboveSuffix.Name = "labelRetrieveAboveSuffix";
            this.labelRetrieveAboveSuffix.Size = new System.Drawing.Size(37, 13);
            this.labelRetrieveAboveSuffix.TabIndex = 36;
            this.labelRetrieveAboveSuffix.Text = "above";
            // 
            // labelRetrieveAbovePrefix
            // 
            this.labelRetrieveAbovePrefix.AutoSize = true;
            this.labelRetrieveAbovePrefix.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRetrieveAbovePrefix.Location = new System.Drawing.Point(11, 72);
            this.labelRetrieveAbovePrefix.Name = "labelRetrieveAbovePrefix";
            this.labelRetrieveAbovePrefix.Size = new System.Drawing.Size(47, 13);
            this.labelRetrieveAbovePrefix.TabIndex = 35;
            this.labelRetrieveAbovePrefix.Text = "Retrieve";
            // 
            // textBoxRetrieveAbove
            // 
            this.textBoxRetrieveAbove.Location = new System.Drawing.Point(58, 70);
            this.textBoxRetrieveAbove.Name = "textBoxRetrieveAbove";
            this.textBoxRetrieveAbove.Size = new System.Drawing.Size(42, 20);
            this.textBoxRetrieveAbove.TabIndex = 34;
            this.textBoxRetrieveAbove.Text = "300";
            this.textBoxRetrieveAbove.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxRetrieveAbove.LostFocus += new System.EventHandler(this.textBoxRetrieveAbove_LostFocus);
            // 
            // labelGotoInfrontSuffix
            // 
            this.labelGotoInfrontSuffix.AutoSize = true;
            this.labelGotoInfrontSuffix.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGotoInfrontSuffix.Location = new System.Drawing.Point(106, 49);
            this.labelGotoInfrontSuffix.Name = "labelGotoInfrontSuffix";
            this.labelGotoInfrontSuffix.Size = new System.Drawing.Size(39, 13);
            this.labelGotoInfrontSuffix.TabIndex = 33;
            this.labelGotoInfrontSuffix.Text = "in front";
            // 
            // labelGotoInfrontPrefix
            // 
            this.labelGotoInfrontPrefix.AutoSize = true;
            this.labelGotoInfrontPrefix.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGotoInfrontPrefix.Location = new System.Drawing.Point(11, 48);
            this.labelGotoInfrontPrefix.Name = "labelGotoInfrontPrefix";
            this.labelGotoInfrontPrefix.Size = new System.Drawing.Size(33, 13);
            this.labelGotoInfrontPrefix.TabIndex = 32;
            this.labelGotoInfrontPrefix.Text = "Go to";
            // 
            // textBoxGotoInfront
            // 
            this.textBoxGotoInfront.Location = new System.Drawing.Point(58, 46);
            this.textBoxGotoInfront.Name = "textBoxGotoInfront";
            this.textBoxGotoInfront.Size = new System.Drawing.Size(42, 20);
            this.textBoxGotoInfront.TabIndex = 31;
            this.textBoxGotoInfront.Text = "0";
            this.textBoxGotoInfront.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxGotoInfront.LostFocus += new System.EventHandler(this.textBoxGotoInfront_LostFocus);
            // 
            // labelGotoAboveSuffix
            // 
            this.labelGotoAboveSuffix.AutoSize = true;
            this.labelGotoAboveSuffix.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGotoAboveSuffix.Location = new System.Drawing.Point(106, 23);
            this.labelGotoAboveSuffix.Name = "labelGotoAboveSuffix";
            this.labelGotoAboveSuffix.Size = new System.Drawing.Size(37, 13);
            this.labelGotoAboveSuffix.TabIndex = 30;
            this.labelGotoAboveSuffix.Text = "above";
            // 
            // labelGotoAbovePrefix
            // 
            this.labelGotoAbovePrefix.AutoSize = true;
            this.labelGotoAbovePrefix.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGotoAbovePrefix.Location = new System.Drawing.Point(11, 22);
            this.labelGotoAbovePrefix.Name = "labelGotoAbovePrefix";
            this.labelGotoAbovePrefix.Size = new System.Drawing.Size(33, 13);
            this.labelGotoAbovePrefix.TabIndex = 29;
            this.labelGotoAbovePrefix.Text = "Go to";
            // 
            // textBoxGotoAbove
            // 
            this.textBoxGotoAbove.Location = new System.Drawing.Point(58, 20);
            this.textBoxGotoAbove.Name = "textBoxGotoAbove";
            this.textBoxGotoAbove.Size = new System.Drawing.Size(42, 20);
            this.textBoxGotoAbove.TabIndex = 28;
            this.textBoxGotoAbove.Text = "300";
            this.textBoxGotoAbove.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxGotoAbove.LostFocus += new System.EventHandler(this.textBoxGotoAbove_LostFocus);
            // 
            // checkBoxNeutralizeTriangleWith21
            // 
            this.checkBoxNeutralizeTriangleWith21.AutoSize = true;
            this.checkBoxNeutralizeTriangleWith21.Checked = true;
            this.checkBoxNeutralizeTriangleWith21.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxNeutralizeTriangleWith21.Location = new System.Drawing.Point(3, 105);
            this.checkBoxNeutralizeTriangleWith21.Name = "checkBoxNeutralizeTriangleWith21";
            this.checkBoxNeutralizeTriangleWith21.Size = new System.Drawing.Size(189, 17);
            this.checkBoxNeutralizeTriangleWith21.TabIndex = 31;
            this.checkBoxNeutralizeTriangleWith21.Text = "Neutralize Triangle With 21 (Not 0)";
            this.checkBoxNeutralizeTriangleWith21.UseVisualStyleBackColor = true;
            this.checkBoxNeutralizeTriangleWith21.CheckedChanged += new System.EventHandler(this.checkBoxNeutralizeTriangleWith21_CheckedChanged);
            // 
            // checkBoxDisableActionUpdateWhenCloning
            // 
            this.checkBoxDisableActionUpdateWhenCloning.AutoSize = true;
            this.checkBoxDisableActionUpdateWhenCloning.Location = new System.Drawing.Point(3, 85);
            this.checkBoxDisableActionUpdateWhenCloning.Name = "checkBoxDisableActionUpdateWhenCloning";
            this.checkBoxDisableActionUpdateWhenCloning.Size = new System.Drawing.Size(202, 17);
            this.checkBoxDisableActionUpdateWhenCloning.TabIndex = 30;
            this.checkBoxDisableActionUpdateWhenCloning.Text = "Disable Action Update When Cloning";
            this.checkBoxDisableActionUpdateWhenCloning.UseVisualStyleBackColor = true;
            this.checkBoxDisableActionUpdateWhenCloning.CheckedChanged += new System.EventHandler(this.checkBoxDisableActionUpdateWhenCloning_CheckedChanged);
            // 
            // groupBoxShowOverlay
            // 
            this.groupBoxShowOverlay.Controls.Add(this.checkBoxShowOverlayCameraHackObject);
            this.groupBoxShowOverlay.Controls.Add(this.checkBoxShowOverlayCeilingObject);
            this.groupBoxShowOverlay.Controls.Add(this.checkBoxShowOverlayWallObject);
            this.groupBoxShowOverlay.Controls.Add(this.checkBoxShowOverlayFloorObject);
            this.groupBoxShowOverlay.Controls.Add(this.checkBoxShowOverlayUsedObject);
            this.groupBoxShowOverlay.Controls.Add(this.checkBoxShowOverlayInteractionObject);
            this.groupBoxShowOverlay.Controls.Add(this.checkBoxShowOverlayCameraObject);
            this.groupBoxShowOverlay.Controls.Add(this.checkBoxShowOverlayClosestObject);
            this.groupBoxShowOverlay.Controls.Add(this.checkBoxShowOverlayStoodOnObject);
            this.groupBoxShowOverlay.Controls.Add(this.checkBoxShowOverlayHeldObject);
            this.groupBoxShowOverlay.Location = new System.Drawing.Point(256, 4);
            this.groupBoxShowOverlay.Name = "groupBoxShowOverlay";
            this.groupBoxShowOverlay.Size = new System.Drawing.Size(170, 233);
            this.groupBoxShowOverlay.TabIndex = 29;
            this.groupBoxShowOverlay.TabStop = false;
            this.groupBoxShowOverlay.Text = "Object Slot Overlays to Show";
            // 
            // checkBoxShowOverlayCameraHackObject
            // 
            this.checkBoxShowOverlayCameraHackObject.AutoSize = true;
            this.checkBoxShowOverlayCameraHackObject.Checked = true;
            this.checkBoxShowOverlayCameraHackObject.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowOverlayCameraHackObject.Location = new System.Drawing.Point(5, 146);
            this.checkBoxShowOverlayCameraHackObject.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxShowOverlayCameraHackObject.Name = "checkBoxShowOverlayCameraHackObject";
            this.checkBoxShowOverlayCameraHackObject.Size = new System.Drawing.Size(125, 17);
            this.checkBoxShowOverlayCameraHackObject.TabIndex = 10;
            this.checkBoxShowOverlayCameraHackObject.Text = "Camera Hack Object";
            this.checkBoxShowOverlayCameraHackObject.UseVisualStyleBackColor = true;
            this.checkBoxShowOverlayCameraHackObject.CheckedChanged += new System.EventHandler(this.checkBoxShowOverlayCameraHackObject_CheckedChanged);
            // 
            // checkBoxShowOverlayCeilingObject
            // 
            this.checkBoxShowOverlayCeilingObject.AutoSize = true;
            this.checkBoxShowOverlayCeilingObject.Checked = true;
            this.checkBoxShowOverlayCeilingObject.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowOverlayCeilingObject.Location = new System.Drawing.Point(5, 209);
            this.checkBoxShowOverlayCeilingObject.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxShowOverlayCeilingObject.Name = "checkBoxShowOverlayCeilingObject";
            this.checkBoxShowOverlayCeilingObject.Size = new System.Drawing.Size(91, 17);
            this.checkBoxShowOverlayCeilingObject.TabIndex = 9;
            this.checkBoxShowOverlayCeilingObject.Text = "Ceiling Object";
            this.checkBoxShowOverlayCeilingObject.UseVisualStyleBackColor = true;
            this.checkBoxShowOverlayCeilingObject.CheckedChanged += new System.EventHandler(this.checkBoxShowOverlayCeilingObject_CheckedChanged);
            // 
            // checkBoxShowOverlayWallObject
            // 
            this.checkBoxShowOverlayWallObject.AutoSize = true;
            this.checkBoxShowOverlayWallObject.Checked = true;
            this.checkBoxShowOverlayWallObject.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowOverlayWallObject.Location = new System.Drawing.Point(5, 188);
            this.checkBoxShowOverlayWallObject.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxShowOverlayWallObject.Name = "checkBoxShowOverlayWallObject";
            this.checkBoxShowOverlayWallObject.Size = new System.Drawing.Size(81, 17);
            this.checkBoxShowOverlayWallObject.TabIndex = 8;
            this.checkBoxShowOverlayWallObject.Text = "Wall Object";
            this.checkBoxShowOverlayWallObject.UseVisualStyleBackColor = true;
            this.checkBoxShowOverlayWallObject.CheckedChanged += new System.EventHandler(this.checkBoxShowOverlayWallObject_CheckedChanged);
            // 
            // checkBoxShowOverlayFloorObject
            // 
            this.checkBoxShowOverlayFloorObject.AutoSize = true;
            this.checkBoxShowOverlayFloorObject.Checked = true;
            this.checkBoxShowOverlayFloorObject.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowOverlayFloorObject.Location = new System.Drawing.Point(5, 167);
            this.checkBoxShowOverlayFloorObject.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxShowOverlayFloorObject.Name = "checkBoxShowOverlayFloorObject";
            this.checkBoxShowOverlayFloorObject.Size = new System.Drawing.Size(83, 17);
            this.checkBoxShowOverlayFloorObject.TabIndex = 7;
            this.checkBoxShowOverlayFloorObject.Text = "Floor Object";
            this.checkBoxShowOverlayFloorObject.UseVisualStyleBackColor = true;
            this.checkBoxShowOverlayFloorObject.CheckedChanged += new System.EventHandler(this.checkBoxShowOverlayFloorObject_CheckedChanged);
            // 
            // checkBoxShowOverlayUsedObject
            // 
            this.checkBoxShowOverlayUsedObject.AutoSize = true;
            this.checkBoxShowOverlayUsedObject.Checked = true;
            this.checkBoxShowOverlayUsedObject.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowOverlayUsedObject.Location = new System.Drawing.Point(5, 83);
            this.checkBoxShowOverlayUsedObject.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxShowOverlayUsedObject.Name = "checkBoxShowOverlayUsedObject";
            this.checkBoxShowOverlayUsedObject.Size = new System.Drawing.Size(85, 17);
            this.checkBoxShowOverlayUsedObject.TabIndex = 4;
            this.checkBoxShowOverlayUsedObject.Text = "Used Object";
            this.checkBoxShowOverlayUsedObject.UseVisualStyleBackColor = true;
            this.checkBoxShowOverlayUsedObject.CheckedChanged += new System.EventHandler(this.checkBoxShowOverlayUsedObject_CheckedChanged);
            // 
            // checkBoxShowOverlayInteractionObject
            // 
            this.checkBoxShowOverlayInteractionObject.AutoSize = true;
            this.checkBoxShowOverlayInteractionObject.Checked = true;
            this.checkBoxShowOverlayInteractionObject.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowOverlayInteractionObject.Location = new System.Drawing.Point(5, 62);
            this.checkBoxShowOverlayInteractionObject.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxShowOverlayInteractionObject.Name = "checkBoxShowOverlayInteractionObject";
            this.checkBoxShowOverlayInteractionObject.Size = new System.Drawing.Size(110, 17);
            this.checkBoxShowOverlayInteractionObject.TabIndex = 3;
            this.checkBoxShowOverlayInteractionObject.Text = "Interaction Object";
            this.checkBoxShowOverlayInteractionObject.UseVisualStyleBackColor = true;
            this.checkBoxShowOverlayInteractionObject.CheckedChanged += new System.EventHandler(this.checkBoxShowOverlayInteractionObject_CheckedChanged);
            // 
            // checkBoxShowOverlayCameraObject
            // 
            this.checkBoxShowOverlayCameraObject.AutoSize = true;
            this.checkBoxShowOverlayCameraObject.Checked = true;
            this.checkBoxShowOverlayCameraObject.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowOverlayCameraObject.Location = new System.Drawing.Point(5, 125);
            this.checkBoxShowOverlayCameraObject.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxShowOverlayCameraObject.Name = "checkBoxShowOverlayCameraObject";
            this.checkBoxShowOverlayCameraObject.Size = new System.Drawing.Size(96, 17);
            this.checkBoxShowOverlayCameraObject.TabIndex = 5;
            this.checkBoxShowOverlayCameraObject.Text = "Camera Object";
            this.checkBoxShowOverlayCameraObject.UseVisualStyleBackColor = true;
            this.checkBoxShowOverlayCameraObject.CheckedChanged += new System.EventHandler(this.checkBoxShowOverlayCameraObject_CheckedChanged);
            // 
            // checkBoxShowOverlayClosestObject
            // 
            this.checkBoxShowOverlayClosestObject.AutoSize = true;
            this.checkBoxShowOverlayClosestObject.Checked = true;
            this.checkBoxShowOverlayClosestObject.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowOverlayClosestObject.Location = new System.Drawing.Point(5, 104);
            this.checkBoxShowOverlayClosestObject.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxShowOverlayClosestObject.Name = "checkBoxShowOverlayClosestObject";
            this.checkBoxShowOverlayClosestObject.Size = new System.Drawing.Size(94, 17);
            this.checkBoxShowOverlayClosestObject.TabIndex = 6;
            this.checkBoxShowOverlayClosestObject.Text = "Closest Object";
            this.checkBoxShowOverlayClosestObject.UseVisualStyleBackColor = true;
            this.checkBoxShowOverlayClosestObject.CheckedChanged += new System.EventHandler(this.checkBoxShowOverlayClosestObject_CheckedChanged);
            // 
            // checkBoxShowOverlayStoodOnObject
            // 
            this.checkBoxShowOverlayStoodOnObject.AutoSize = true;
            this.checkBoxShowOverlayStoodOnObject.Checked = true;
            this.checkBoxShowOverlayStoodOnObject.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowOverlayStoodOnObject.Location = new System.Drawing.Point(5, 41);
            this.checkBoxShowOverlayStoodOnObject.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxShowOverlayStoodOnObject.Name = "checkBoxShowOverlayStoodOnObject";
            this.checkBoxShowOverlayStoodOnObject.Size = new System.Drawing.Size(105, 17);
            this.checkBoxShowOverlayStoodOnObject.TabIndex = 2;
            this.checkBoxShowOverlayStoodOnObject.Text = "Stood On Object";
            this.checkBoxShowOverlayStoodOnObject.UseVisualStyleBackColor = true;
            this.checkBoxShowOverlayStoodOnObject.CheckedChanged += new System.EventHandler(this.checkBoxShowOverlayStoodOnObject_CheckedChanged);
            // 
            // checkBoxShowOverlayHeldObject
            // 
            this.checkBoxShowOverlayHeldObject.AutoSize = true;
            this.checkBoxShowOverlayHeldObject.Checked = true;
            this.checkBoxShowOverlayHeldObject.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowOverlayHeldObject.Location = new System.Drawing.Point(5, 20);
            this.checkBoxShowOverlayHeldObject.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxShowOverlayHeldObject.Name = "checkBoxShowOverlayHeldObject";
            this.checkBoxShowOverlayHeldObject.Size = new System.Drawing.Size(82, 17);
            this.checkBoxShowOverlayHeldObject.TabIndex = 1;
            this.checkBoxShowOverlayHeldObject.Text = "Held Object";
            this.checkBoxShowOverlayHeldObject.UseVisualStyleBackColor = true;
            this.checkBoxShowOverlayHeldObject.CheckedChanged += new System.EventHandler(this.checkBoxShowOverlayHeldObject_CheckedChanged);
            // 
            // checkBoxScaleDiagonalPositionControllerButtons
            // 
            this.checkBoxScaleDiagonalPositionControllerButtons.AutoSize = true;
            this.checkBoxScaleDiagonalPositionControllerButtons.Location = new System.Drawing.Point(3, 45);
            this.checkBoxScaleDiagonalPositionControllerButtons.Name = "checkBoxScaleDiagonalPositionControllerButtons";
            this.checkBoxScaleDiagonalPositionControllerButtons.Size = new System.Drawing.Size(224, 17);
            this.checkBoxScaleDiagonalPositionControllerButtons.TabIndex = 8;
            this.checkBoxScaleDiagonalPositionControllerButtons.Text = "Scale Diagonal Position Controller Buttons";
            this.checkBoxScaleDiagonalPositionControllerButtons.UseVisualStyleBackColor = true;
            this.checkBoxScaleDiagonalPositionControllerButtons.CheckedChanged += new System.EventHandler(this.checkBoxScaleDiagonalPositionControllerButtons_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(4, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "*Requires Pure Interpreter";
            // 
            // checkBoxMoveCamWithPu
            // 
            this.checkBoxMoveCamWithPu.AutoSize = true;
            this.checkBoxMoveCamWithPu.Checked = true;
            this.checkBoxMoveCamWithPu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMoveCamWithPu.Location = new System.Drawing.Point(3, 65);
            this.checkBoxMoveCamWithPu.Name = "checkBoxMoveCamWithPu";
            this.checkBoxMoveCamWithPu.Size = new System.Drawing.Size(162, 17);
            this.checkBoxMoveCamWithPu.TabIndex = 4;
            this.checkBoxMoveCamWithPu.Text = "PU Controller Moves Camera";
            this.checkBoxMoveCamWithPu.UseVisualStyleBackColor = true;
            this.checkBoxMoveCamWithPu.CheckedChanged += new System.EventHandler(this.checkBoxMoveCamWithPu_CheckedChanged);
            // 
            // checkBoxUseRomHack
            // 
            this.checkBoxUseRomHack.AutoSize = true;
            this.checkBoxUseRomHack.Location = new System.Drawing.Point(3, 5);
            this.checkBoxUseRomHack.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxUseRomHack.Name = "checkBoxUseRomHack";
            this.checkBoxUseRomHack.Size = new System.Drawing.Size(166, 17);
            this.checkBoxUseRomHack.TabIndex = 2;
            this.checkBoxUseRomHack.Text = "Enable STROOP ROM hack*";
            this.checkBoxUseRomHack.UseVisualStyleBackColor = true;
            // 
            // checkBoxStartSlotIndexOne
            // 
            this.checkBoxStartSlotIndexOne.AutoSize = true;
            this.checkBoxStartSlotIndexOne.Checked = true;
            this.checkBoxStartSlotIndexOne.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxStartSlotIndexOne.Location = new System.Drawing.Point(3, 25);
            this.checkBoxStartSlotIndexOne.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxStartSlotIndexOne.Name = "checkBoxStartSlotIndexOne";
            this.checkBoxStartSlotIndexOne.Size = new System.Drawing.Size(133, 17);
            this.checkBoxStartSlotIndexOne.TabIndex = 0;
            this.checkBoxStartSlotIndexOne.Text = "Start Slot Index From 1";
            this.checkBoxStartSlotIndexOne.UseVisualStyleBackColor = true;
            this.checkBoxStartSlotIndexOne.CheckedChanged += new System.EventHandler(this.checkBoxStartSlotIndexOne_CheckedChanged);
            // 
            // labelVersionNumber
            // 
            this.labelVersionNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelVersionNumber.AutoSize = true;
            this.labelVersionNumber.Location = new System.Drawing.Point(894, 15);
            this.labelVersionNumber.Name = "labelVersionNumber";
            this.labelVersionNumber.Size = new System.Drawing.Size(41, 13);
            this.labelVersionNumber.TabIndex = 5;
            this.labelVersionNumber.Text = "version";
            this.labelVersionNumber.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Location = new System.Drawing.Point(11, 11);
            this.buttonDisconnect.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(72, 21);
            this.buttonDisconnect.TabIndex = 17;
            this.buttonDisconnect.Text = "Disconnect";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // panelConnect
            // 
            this.panelConnect.Controls.Add(this.buttonRefreshAndConnect);
            this.panelConnect.Controls.Add(this.buttonRefresh);
            this.panelConnect.Controls.Add(this.labelNotConnected);
            this.panelConnect.Controls.Add(this.buttonConnect);
            this.panelConnect.Controls.Add(this.listBoxProcessesList);
            this.panelConnect.Location = new System.Drawing.Point(246, 12);
            this.panelConnect.Name = "panelConnect";
            this.panelConnect.Size = new System.Drawing.Size(441, 20);
            this.panelConnect.TabIndex = 17;
            // 
            // buttonRefreshAndConnect
            // 
            this.buttonRefreshAndConnect.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonRefreshAndConnect.Location = new System.Drawing.Point(134, 133);
            this.buttonRefreshAndConnect.Name = "buttonRefreshAndConnect";
            this.buttonRefreshAndConnect.Size = new System.Drawing.Size(172, 37);
            this.buttonRefreshAndConnect.TabIndex = 3;
            this.buttonRefreshAndConnect.Text = "Refresh && Connect";
            this.buttonRefreshAndConnect.UseVisualStyleBackColor = true;
            this.buttonRefreshAndConnect.Click += new System.EventHandler(this.buttonRefreshAndConnect_Click);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonRefresh.Location = new System.Drawing.Point(134, 92);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(84, 37);
            this.buttonRefresh.TabIndex = 3;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // labelNotConnected
            // 
            this.labelNotConnected.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelNotConnected.AutoSize = true;
            this.labelNotConnected.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNotConnected.Location = new System.Drawing.Point(141, -111);
            this.labelNotConnected.Name = "labelNotConnected";
            this.labelNotConnected.Size = new System.Drawing.Size(157, 26);
            this.labelNotConnected.TabIndex = 2;
            this.labelNotConnected.Text = "Not Connected";
            // 
            // buttonConnect
            // 
            this.buttonConnect.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonConnect.Location = new System.Drawing.Point(222, 92);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(84, 37);
            this.buttonConnect.TabIndex = 1;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // listBoxProcessesList
            // 
            this.listBoxProcessesList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.listBoxProcessesList.FormattingEnabled = true;
            this.listBoxProcessesList.Location = new System.Drawing.Point(134, -61);
            this.listBoxProcessesList.Name = "listBoxProcessesList";
            this.listBoxProcessesList.Size = new System.Drawing.Size(172, 147);
            this.listBoxProcessesList.TabIndex = 0;
            // 
            // labelFpsCounter
            // 
            this.labelFpsCounter.AutoSize = true;
            this.labelFpsCounter.Location = new System.Drawing.Point(88, 15);
            this.labelFpsCounter.Name = "labelFpsCounter";
            this.labelFpsCounter.Size = new System.Drawing.Size(39, 13);
            this.labelFpsCounter.TabIndex = 18;
            this.labelFpsCounter.Text = "FPS: 0";
            // 
            // buttonShowTopPane
            // 
            this.buttonShowTopPane.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonShowTopPane.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonShowTopPane.BackgroundImage")));
            this.buttonShowTopPane.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonShowTopPane.Location = new System.Drawing.Point(866, 11);
            this.buttonShowTopPane.Margin = new System.Windows.Forms.Padding(2);
            this.buttonShowTopPane.Name = "buttonShowTopPane";
            this.buttonShowTopPane.Size = new System.Drawing.Size(21, 21);
            this.buttonShowTopPane.TabIndex = 19;
            this.buttonShowTopPane.UseVisualStyleBackColor = true;
            this.buttonShowTopPane.Click += new System.EventHandler(this.buttonShowTopPanel_Click);
            // 
            // buttonShowTopBottomPane
            // 
            this.buttonShowTopBottomPane.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonShowTopBottomPane.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonShowTopBottomPane.BackgroundImage")));
            this.buttonShowTopBottomPane.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonShowTopBottomPane.Location = new System.Drawing.Point(841, 11);
            this.buttonShowTopBottomPane.Margin = new System.Windows.Forms.Padding(2);
            this.buttonShowTopBottomPane.Name = "buttonShowTopBottomPane";
            this.buttonShowTopBottomPane.Size = new System.Drawing.Size(21, 21);
            this.buttonShowTopBottomPane.TabIndex = 20;
            this.buttonShowTopBottomPane.UseVisualStyleBackColor = true;
            this.buttonShowTopBottomPane.Click += new System.EventHandler(this.buttonShowTopBottomPanel_Click);
            // 
            // buttonReadOnly
            // 
            this.buttonReadOnly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReadOnly.Location = new System.Drawing.Point(611, 11);
            this.buttonReadOnly.Margin = new System.Windows.Forms.Padding(2);
            this.buttonReadOnly.Name = "buttonReadOnly";
            this.buttonReadOnly.Size = new System.Drawing.Size(126, 21);
            this.buttonReadOnly.TabIndex = 21;
            this.buttonReadOnly.Tag = "";
            this.buttonReadOnly.Text = "Switch to Read-Only";
            this.buttonReadOnly.UseVisualStyleBackColor = true;
            this.buttonReadOnly.Click += new System.EventHandler(this.buttonReadOnly_Click);
            // 
            // buttonShowBottomPane
            // 
            this.buttonShowBottomPane.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonShowBottomPane.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonShowBottomPane.BackgroundImage")));
            this.buttonShowBottomPane.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonShowBottomPane.Location = new System.Drawing.Point(816, 11);
            this.buttonShowBottomPane.Margin = new System.Windows.Forms.Padding(2);
            this.buttonShowBottomPane.Name = "buttonShowBottomPane";
            this.buttonShowBottomPane.Size = new System.Drawing.Size(21, 21);
            this.buttonShowBottomPane.TabIndex = 20;
            this.buttonShowBottomPane.UseVisualStyleBackColor = true;
            this.buttonShowBottomPane.Click += new System.EventHandler(this.buttonShowBottomPanel_Click);
            // 
            // buttonShowRightPane
            // 
            this.buttonShowRightPane.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonShowRightPane.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonShowRightPane.BackgroundImage")));
            this.buttonShowRightPane.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonShowRightPane.Location = new System.Drawing.Point(791, 11);
            this.buttonShowRightPane.Margin = new System.Windows.Forms.Padding(2);
            this.buttonShowRightPane.Name = "buttonShowRightPane";
            this.buttonShowRightPane.Size = new System.Drawing.Size(21, 21);
            this.buttonShowRightPane.TabIndex = 19;
            this.buttonShowRightPane.UseVisualStyleBackColor = true;
            this.buttonShowRightPane.Click += new System.EventHandler(this.buttonShowRightPanel_Click);
            // 
            // buttonShowLeftRightPane
            // 
            this.buttonShowLeftRightPane.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonShowLeftRightPane.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonShowLeftRightPane.BackgroundImage")));
            this.buttonShowLeftRightPane.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonShowLeftRightPane.Location = new System.Drawing.Point(766, 11);
            this.buttonShowLeftRightPane.Margin = new System.Windows.Forms.Padding(2);
            this.buttonShowLeftRightPane.Name = "buttonShowLeftRightPane";
            this.buttonShowLeftRightPane.Size = new System.Drawing.Size(21, 21);
            this.buttonShowLeftRightPane.TabIndex = 20;
            this.buttonShowLeftRightPane.UseVisualStyleBackColor = true;
            this.buttonShowLeftRightPane.Click += new System.EventHandler(this.buttonShowLeftRightPanel_Click);
            // 
            // buttonShowLeftPane
            // 
            this.buttonShowLeftPane.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonShowLeftPane.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonShowLeftPane.BackgroundImage")));
            this.buttonShowLeftPane.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonShowLeftPane.Location = new System.Drawing.Point(741, 11);
            this.buttonShowLeftPane.Margin = new System.Windows.Forms.Padding(2);
            this.buttonShowLeftPane.Name = "buttonShowLeftPane";
            this.buttonShowLeftPane.Size = new System.Drawing.Size(21, 21);
            this.buttonShowLeftPane.TabIndex = 20;
            this.buttonShowLeftPane.UseVisualStyleBackColor = true;
            this.buttonShowLeftPane.Click += new System.EventHandler(this.buttonShowLeftPanel_Click);
            // 
            // pictureBoxObject
            // 
            this.pictureBoxObject.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxObject.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            this.pictureBoxObject.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxObject.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBoxObject.MaximumSize = new System.Drawing.Size(133, 130);
            this.pictureBoxObject.Name = "pictureBoxObject";
            this.pictureBoxObject.Size = new System.Drawing.Size(51, 50);
            this.pictureBoxObject.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxObject.TabIndex = 0;
            this.pictureBoxObject.TabStop = false;
            // 
            // NoTearFlowLayoutPanelObject
            // 
            this.NoTearFlowLayoutPanelObject.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NoTearFlowLayoutPanelObject.AutoScroll = true;
            this.NoTearFlowLayoutPanelObject.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.NoTearFlowLayoutPanelObject.Location = new System.Drawing.Point(2, 2);
            this.NoTearFlowLayoutPanelObject.Margin = new System.Windows.Forms.Padding(2);
            this.NoTearFlowLayoutPanelObject.Name = "NoTearFlowLayoutPanelObject";
            this.NoTearFlowLayoutPanelObject.Size = new System.Drawing.Size(750, 458);
            this.NoTearFlowLayoutPanelObject.TabIndex = 3;
            // 
            // pictureBoxMario
            // 
            this.pictureBoxMario.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxMario.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            this.pictureBoxMario.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxMario.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBoxMario.MaximumSize = new System.Drawing.Size(133, 130);
            this.pictureBoxMario.Name = "pictureBoxMario";
            this.pictureBoxMario.Size = new System.Drawing.Size(51, 50);
            this.pictureBoxMario.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxMario.TabIndex = 0;
            this.pictureBoxMario.TabStop = false;
            // 
            // NoTearFlowLayoutPanelMario
            // 
            this.NoTearFlowLayoutPanelMario.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NoTearFlowLayoutPanelMario.AutoScroll = true;
            this.NoTearFlowLayoutPanelMario.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.NoTearFlowLayoutPanelMario.Location = new System.Drawing.Point(2, 2);
            this.NoTearFlowLayoutPanelMario.Margin = new System.Windows.Forms.Padding(2);
            this.NoTearFlowLayoutPanelMario.Name = "NoTearFlowLayoutPanelMario";
            this.NoTearFlowLayoutPanelMario.Size = new System.Drawing.Size(759, 456);
            this.NoTearFlowLayoutPanelMario.TabIndex = 1;
            // 
            // noTearFlowLayoutPanelActions
            // 
            this.noTearFlowLayoutPanelActions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.noTearFlowLayoutPanelActions.AutoScroll = true;
            this.noTearFlowLayoutPanelActions.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.noTearFlowLayoutPanelActions.Location = new System.Drawing.Point(6, 6);
            this.noTearFlowLayoutPanelActions.Name = "noTearFlowLayoutPanelActions";
            this.noTearFlowLayoutPanelActions.Size = new System.Drawing.Size(903, 451);
            this.noTearFlowLayoutPanelActions.TabIndex = 0;
            // 
            // pictureBoxHud
            // 
            this.pictureBoxHud.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxHud.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            this.pictureBoxHud.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxHud.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBoxHud.MaximumSize = new System.Drawing.Size(133, 130);
            this.pictureBoxHud.Name = "pictureBoxHud";
            this.pictureBoxHud.Size = new System.Drawing.Size(51, 50);
            this.pictureBoxHud.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxHud.TabIndex = 0;
            this.pictureBoxHud.TabStop = false;
            // 
            // NoTearFlowLayoutPanelHud
            // 
            this.NoTearFlowLayoutPanelHud.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NoTearFlowLayoutPanelHud.AutoScroll = true;
            this.NoTearFlowLayoutPanelHud.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.NoTearFlowLayoutPanelHud.Location = new System.Drawing.Point(2, 2);
            this.NoTearFlowLayoutPanelHud.Margin = new System.Windows.Forms.Padding(2);
            this.NoTearFlowLayoutPanelHud.Name = "NoTearFlowLayoutPanelHud";
            this.NoTearFlowLayoutPanelHud.Size = new System.Drawing.Size(860, 457);
            this.NoTearFlowLayoutPanelHud.TabIndex = 3;
            // 
            // pictureBoxCamera
            // 
            this.pictureBoxCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxCamera.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            this.pictureBoxCamera.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxCamera.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBoxCamera.MaximumSize = new System.Drawing.Size(133, 130);
            this.pictureBoxCamera.Name = "pictureBoxCamera";
            this.pictureBoxCamera.Size = new System.Drawing.Size(51, 50);
            this.pictureBoxCamera.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxCamera.TabIndex = 0;
            this.pictureBoxCamera.TabStop = false;
            // 
            // NoTearFlowLayoutPanelCamera
            // 
            this.NoTearFlowLayoutPanelCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NoTearFlowLayoutPanelCamera.AutoScroll = true;
            this.NoTearFlowLayoutPanelCamera.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.NoTearFlowLayoutPanelCamera.Location = new System.Drawing.Point(2, 2);
            this.NoTearFlowLayoutPanelCamera.Margin = new System.Windows.Forms.Padding(2);
            this.NoTearFlowLayoutPanelCamera.Name = "NoTearFlowLayoutPanelCamera";
            this.NoTearFlowLayoutPanelCamera.Size = new System.Drawing.Size(770, 457);
            this.NoTearFlowLayoutPanelCamera.TabIndex = 3;
            // 
            // NoTearFlowLayoutPanelTriangles
            // 
            this.NoTearFlowLayoutPanelTriangles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NoTearFlowLayoutPanelTriangles.AutoScroll = true;
            this.NoTearFlowLayoutPanelTriangles.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.NoTearFlowLayoutPanelTriangles.Location = new System.Drawing.Point(2, 4);
            this.NoTearFlowLayoutPanelTriangles.Margin = new System.Windows.Forms.Padding(2);
            this.NoTearFlowLayoutPanelTriangles.Name = "NoTearFlowLayoutPanelTriangles";
            this.NoTearFlowLayoutPanelTriangles.Size = new System.Drawing.Size(767, 453);
            this.NoTearFlowLayoutPanelTriangles.TabIndex = 7;
            // 
            // noTearFlowLayoutPanelWater
            // 
            this.noTearFlowLayoutPanelWater.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.noTearFlowLayoutPanelWater.AutoScroll = true;
            this.noTearFlowLayoutPanelWater.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.noTearFlowLayoutPanelWater.Location = new System.Drawing.Point(2, 2);
            this.noTearFlowLayoutPanelWater.Margin = new System.Windows.Forms.Padding(2);
            this.noTearFlowLayoutPanelWater.Name = "noTearFlowLayoutPanelWater";
            this.noTearFlowLayoutPanelWater.Size = new System.Drawing.Size(909, 457);
            this.noTearFlowLayoutPanelWater.TabIndex = 2;
            // 
            // controllerDisplayPanel
            // 
            this.controllerDisplayPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.controllerDisplayPanel.AutoSize = true;
            this.controllerDisplayPanel.Location = new System.Drawing.Point(3, 3);
            this.controllerDisplayPanel.Margin = new System.Windows.Forms.Padding(2);
            this.controllerDisplayPanel.Name = "controllerDisplayPanel";
            this.controllerDisplayPanel.Size = new System.Drawing.Size(421, 457);
            this.controllerDisplayPanel.TabIndex = 4;
            // 
            // NoTearFlowLayoutPanelController
            // 
            this.NoTearFlowLayoutPanelController.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NoTearFlowLayoutPanelController.AutoScroll = true;
            this.NoTearFlowLayoutPanelController.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.NoTearFlowLayoutPanelController.Location = new System.Drawing.Point(2, 2);
            this.NoTearFlowLayoutPanelController.Margin = new System.Windows.Forms.Padding(2);
            this.NoTearFlowLayoutPanelController.Name = "NoTearFlowLayoutPanelController";
            this.NoTearFlowLayoutPanelController.Size = new System.Drawing.Size(548, 460);
            this.NoTearFlowLayoutPanelController.TabIndex = 2;
            // 
            // textBoxTableRow15Col10
            // 
            this.textBoxTableRow15Col10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTableRow15Col10.Location = new System.Drawing.Point(335, 225);
            this.textBoxTableRow15Col10.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxTableRow15Col10.MaximumSize = new System.Drawing.Size(38, 15);
            this.textBoxTableRow15Col10.Name = "textBoxTableRow15Col10";
            this.textBoxTableRow15Col10.Size = new System.Drawing.Size(38, 20);
            this.textBoxTableRow15Col10.TabIndex = 41;
            this.textBoxTableRow15Col10.Text = "100";
            this.textBoxTableRow15Col10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxTableRow14Col10
            // 
            this.textBoxTableRow14Col10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTableRow14Col10.Location = new System.Drawing.Point(335, 209);
            this.textBoxTableRow14Col10.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxTableRow14Col10.MaximumSize = new System.Drawing.Size(38, 15);
            this.textBoxTableRow14Col10.Name = "textBoxTableRow14Col10";
            this.textBoxTableRow14Col10.Size = new System.Drawing.Size(38, 20);
            this.textBoxTableRow14Col10.TabIndex = 41;
            this.textBoxTableRow14Col10.Text = "100";
            this.textBoxTableRow14Col10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxTableRow13Col10
            // 
            this.textBoxTableRow13Col10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTableRow13Col10.Location = new System.Drawing.Point(335, 193);
            this.textBoxTableRow13Col10.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxTableRow13Col10.MaximumSize = new System.Drawing.Size(38, 15);
            this.textBoxTableRow13Col10.Name = "textBoxTableRow13Col10";
            this.textBoxTableRow13Col10.Size = new System.Drawing.Size(38, 20);
            this.textBoxTableRow13Col10.TabIndex = 41;
            this.textBoxTableRow13Col10.Text = "100";
            this.textBoxTableRow13Col10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxTableRow12Col10
            // 
            this.textBoxTableRow12Col10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTableRow12Col10.Location = new System.Drawing.Point(335, 177);
            this.textBoxTableRow12Col10.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxTableRow12Col10.MaximumSize = new System.Drawing.Size(38, 15);
            this.textBoxTableRow12Col10.Name = "textBoxTableRow12Col10";
            this.textBoxTableRow12Col10.Size = new System.Drawing.Size(38, 20);
            this.textBoxTableRow12Col10.TabIndex = 41;
            this.textBoxTableRow12Col10.Text = "100";
            this.textBoxTableRow12Col10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxTableRow11Col10
            // 
            this.textBoxTableRow11Col10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTableRow11Col10.Location = new System.Drawing.Point(335, 161);
            this.textBoxTableRow11Col10.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxTableRow11Col10.MaximumSize = new System.Drawing.Size(38, 15);
            this.textBoxTableRow11Col10.Name = "textBoxTableRow11Col10";
            this.textBoxTableRow11Col10.Size = new System.Drawing.Size(38, 20);
            this.textBoxTableRow11Col10.TabIndex = 41;
            this.textBoxTableRow11Col10.Text = "100";
            this.textBoxTableRow11Col10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxTableRow10Col10
            // 
            this.textBoxTableRow10Col10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTableRow10Col10.Location = new System.Drawing.Point(335, 145);
            this.textBoxTableRow10Col10.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxTableRow10Col10.MaximumSize = new System.Drawing.Size(38, 15);
            this.textBoxTableRow10Col10.Name = "textBoxTableRow10Col10";
            this.textBoxTableRow10Col10.Size = new System.Drawing.Size(38, 20);
            this.textBoxTableRow10Col10.TabIndex = 41;
            this.textBoxTableRow10Col10.Text = "100";
            this.textBoxTableRow10Col10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxTableRow9Col10
            // 
            this.textBoxTableRow9Col10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTableRow9Col10.Location = new System.Drawing.Point(335, 129);
            this.textBoxTableRow9Col10.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxTableRow9Col10.MaximumSize = new System.Drawing.Size(38, 15);
            this.textBoxTableRow9Col10.Name = "textBoxTableRow9Col10";
            this.textBoxTableRow9Col10.Size = new System.Drawing.Size(38, 20);
            this.textBoxTableRow9Col10.TabIndex = 41;
            this.textBoxTableRow9Col10.Text = "100";
            this.textBoxTableRow9Col10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxTableRow8Col10
            // 
            this.textBoxTableRow8Col10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTableRow8Col10.Location = new System.Drawing.Point(335, 113);
            this.textBoxTableRow8Col10.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxTableRow8Col10.MaximumSize = new System.Drawing.Size(38, 15);
            this.textBoxTableRow8Col10.Name = "textBoxTableRow8Col10";
            this.textBoxTableRow8Col10.Size = new System.Drawing.Size(38, 20);
            this.textBoxTableRow8Col10.TabIndex = 41;
            this.textBoxTableRow8Col10.Text = "100";
            this.textBoxTableRow8Col10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxTableRow7Col10
            // 
            this.textBoxTableRow7Col10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTableRow7Col10.Location = new System.Drawing.Point(335, 97);
            this.textBoxTableRow7Col10.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxTableRow7Col10.MaximumSize = new System.Drawing.Size(38, 15);
            this.textBoxTableRow7Col10.Name = "textBoxTableRow7Col10";
            this.textBoxTableRow7Col10.Size = new System.Drawing.Size(38, 20);
            this.textBoxTableRow7Col10.TabIndex = 41;
            this.textBoxTableRow7Col10.Text = "100";
            this.textBoxTableRow7Col10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxTableRow6Col10
            // 
            this.textBoxTableRow6Col10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTableRow6Col10.Location = new System.Drawing.Point(335, 81);
            this.textBoxTableRow6Col10.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxTableRow6Col10.MaximumSize = new System.Drawing.Size(38, 15);
            this.textBoxTableRow6Col10.Name = "textBoxTableRow6Col10";
            this.textBoxTableRow6Col10.Size = new System.Drawing.Size(38, 20);
            this.textBoxTableRow6Col10.TabIndex = 41;
            this.textBoxTableRow6Col10.Text = "100";
            this.textBoxTableRow6Col10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxTableRow5Col10
            // 
            this.textBoxTableRow5Col10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTableRow5Col10.Location = new System.Drawing.Point(335, 65);
            this.textBoxTableRow5Col10.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxTableRow5Col10.MaximumSize = new System.Drawing.Size(38, 15);
            this.textBoxTableRow5Col10.Name = "textBoxTableRow5Col10";
            this.textBoxTableRow5Col10.Size = new System.Drawing.Size(38, 20);
            this.textBoxTableRow5Col10.TabIndex = 41;
            this.textBoxTableRow5Col10.Text = "100";
            this.textBoxTableRow5Col10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxTableRow4Col10
            // 
            this.textBoxTableRow4Col10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTableRow4Col10.Location = new System.Drawing.Point(335, 49);
            this.textBoxTableRow4Col10.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxTableRow4Col10.MaximumSize = new System.Drawing.Size(38, 15);
            this.textBoxTableRow4Col10.Name = "textBoxTableRow4Col10";
            this.textBoxTableRow4Col10.Size = new System.Drawing.Size(38, 20);
            this.textBoxTableRow4Col10.TabIndex = 41;
            this.textBoxTableRow4Col10.Text = "100";
            this.textBoxTableRow4Col10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxTableRow3Col10
            // 
            this.textBoxTableRow3Col10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTableRow3Col10.Location = new System.Drawing.Point(335, 33);
            this.textBoxTableRow3Col10.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxTableRow3Col10.MaximumSize = new System.Drawing.Size(38, 15);
            this.textBoxTableRow3Col10.Name = "textBoxTableRow3Col10";
            this.textBoxTableRow3Col10.Size = new System.Drawing.Size(38, 20);
            this.textBoxTableRow3Col10.TabIndex = 41;
            this.textBoxTableRow3Col10.Text = "100";
            this.textBoxTableRow3Col10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxTableRow2Col10
            // 
            this.textBoxTableRow2Col10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTableRow2Col10.Location = new System.Drawing.Point(335, 17);
            this.textBoxTableRow2Col10.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxTableRow2Col10.MaximumSize = new System.Drawing.Size(38, 15);
            this.textBoxTableRow2Col10.Name = "textBoxTableRow2Col10";
            this.textBoxTableRow2Col10.Size = new System.Drawing.Size(38, 20);
            this.textBoxTableRow2Col10.TabIndex = 41;
            this.textBoxTableRow2Col10.Text = "100";
            this.textBoxTableRow2Col10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxTableRow1Col10
            // 
            this.textBoxTableRow1Col10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTableRow1Col10.Location = new System.Drawing.Point(335, 1);
            this.textBoxTableRow1Col10.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxTableRow1Col10.MaximumSize = new System.Drawing.Size(38, 15);
            this.textBoxTableRow1Col10.Name = "textBoxTableRow1Col10";
            this.textBoxTableRow1Col10.Size = new System.Drawing.Size(38, 20);
            this.textBoxTableRow1Col10.TabIndex = 40;
            this.textBoxTableRow1Col10.Text = "100";
            this.textBoxTableRow1Col10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // filePictureBoxTableRow24Col9
            // 
            this.filePictureBoxTableRow24Col9.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow24Col9.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow24Col9.Image")));
            this.filePictureBoxTableRow24Col9.Location = new System.Drawing.Point(306, 369);
            this.filePictureBoxTableRow24Col9.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow24Col9.Name = "filePictureBoxTableRow24Col9";
            this.filePictureBoxTableRow24Col9.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow24Col9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow24Col9.TabIndex = 49;
            this.filePictureBoxTableRow24Col9.TabStop = false;
            // 
            // filePictureBoxTableRow23Col9
            // 
            this.filePictureBoxTableRow23Col9.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow23Col9.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow23Col9.Image")));
            this.filePictureBoxTableRow23Col9.Location = new System.Drawing.Point(306, 353);
            this.filePictureBoxTableRow23Col9.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow23Col9.Name = "filePictureBoxTableRow23Col9";
            this.filePictureBoxTableRow23Col9.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow23Col9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow23Col9.TabIndex = 49;
            this.filePictureBoxTableRow23Col9.TabStop = false;
            // 
            // filePictureBoxTableRow22Col9
            // 
            this.filePictureBoxTableRow22Col9.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow22Col9.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow22Col9.Image")));
            this.filePictureBoxTableRow22Col9.Location = new System.Drawing.Point(306, 337);
            this.filePictureBoxTableRow22Col9.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow22Col9.Name = "filePictureBoxTableRow22Col9";
            this.filePictureBoxTableRow22Col9.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow22Col9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow22Col9.TabIndex = 49;
            this.filePictureBoxTableRow22Col9.TabStop = false;
            // 
            // filePictureBoxTableRow4Col9
            // 
            this.filePictureBoxTableRow4Col9.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow4Col9.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow4Col9.Image")));
            this.filePictureBoxTableRow4Col9.Location = new System.Drawing.Point(306, 49);
            this.filePictureBoxTableRow4Col9.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow4Col9.Name = "filePictureBoxTableRow4Col9";
            this.filePictureBoxTableRow4Col9.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow4Col9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow4Col9.TabIndex = 49;
            this.filePictureBoxTableRow4Col9.TabStop = false;
            // 
            // filePictureBoxTableRow3Col9
            // 
            this.filePictureBoxTableRow3Col9.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow3Col9.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow3Col9.Image")));
            this.filePictureBoxTableRow3Col9.Location = new System.Drawing.Point(306, 33);
            this.filePictureBoxTableRow3Col9.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow3Col9.Name = "filePictureBoxTableRow3Col9";
            this.filePictureBoxTableRow3Col9.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow3Col9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow3Col9.TabIndex = 49;
            this.filePictureBoxTableRow3Col9.TabStop = false;
            // 
            // filePictureBoxTableRow2Col9
            // 
            this.filePictureBoxTableRow2Col9.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow2Col9.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow2Col9.Image")));
            this.filePictureBoxTableRow2Col9.Location = new System.Drawing.Point(306, 17);
            this.filePictureBoxTableRow2Col9.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow2Col9.Name = "filePictureBoxTableRow2Col9";
            this.filePictureBoxTableRow2Col9.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow2Col9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow2Col9.TabIndex = 49;
            this.filePictureBoxTableRow2Col9.TabStop = false;
            // 
            // filePictureBoxTableRow21Col8
            // 
            this.filePictureBoxTableRow21Col8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow21Col8.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow21Col8.Image")));
            this.filePictureBoxTableRow21Col8.Location = new System.Drawing.Point(277, 321);
            this.filePictureBoxTableRow21Col8.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow21Col8.Name = "filePictureBoxTableRow21Col8";
            this.filePictureBoxTableRow21Col8.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow21Col8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow21Col8.TabIndex = 49;
            this.filePictureBoxTableRow21Col8.TabStop = false;
            // 
            // filePictureBoxTableRow15Col8
            // 
            this.filePictureBoxTableRow15Col8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow15Col8.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow15Col8.Image")));
            this.filePictureBoxTableRow15Col8.Location = new System.Drawing.Point(277, 225);
            this.filePictureBoxTableRow15Col8.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow15Col8.Name = "filePictureBoxTableRow15Col8";
            this.filePictureBoxTableRow15Col8.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow15Col8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow15Col8.TabIndex = 49;
            this.filePictureBoxTableRow15Col8.TabStop = false;
            // 
            // filePictureBoxTableRow13Col8
            // 
            this.filePictureBoxTableRow13Col8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow13Col8.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow13Col8.Image")));
            this.filePictureBoxTableRow13Col8.Location = new System.Drawing.Point(277, 193);
            this.filePictureBoxTableRow13Col8.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow13Col8.Name = "filePictureBoxTableRow13Col8";
            this.filePictureBoxTableRow13Col8.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow13Col8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow13Col8.TabIndex = 49;
            this.filePictureBoxTableRow13Col8.TabStop = false;
            // 
            // filePictureBoxTableRow12Col8
            // 
            this.filePictureBoxTableRow12Col8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow12Col8.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow12Col8.Image")));
            this.filePictureBoxTableRow12Col8.Location = new System.Drawing.Point(277, 177);
            this.filePictureBoxTableRow12Col8.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow12Col8.Name = "filePictureBoxTableRow12Col8";
            this.filePictureBoxTableRow12Col8.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow12Col8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow12Col8.TabIndex = 49;
            this.filePictureBoxTableRow12Col8.TabStop = false;
            // 
            // filePictureBoxTableRow11Col8
            // 
            this.filePictureBoxTableRow11Col8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow11Col8.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow11Col8.Image")));
            this.filePictureBoxTableRow11Col8.Location = new System.Drawing.Point(277, 161);
            this.filePictureBoxTableRow11Col8.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow11Col8.Name = "filePictureBoxTableRow11Col8";
            this.filePictureBoxTableRow11Col8.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow11Col8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow11Col8.TabIndex = 49;
            this.filePictureBoxTableRow11Col8.TabStop = false;
            // 
            // filePictureBoxTableRow10Col8
            // 
            this.filePictureBoxTableRow10Col8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow10Col8.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow10Col8.Image")));
            this.filePictureBoxTableRow10Col8.Location = new System.Drawing.Point(277, 145);
            this.filePictureBoxTableRow10Col8.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow10Col8.Name = "filePictureBoxTableRow10Col8";
            this.filePictureBoxTableRow10Col8.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow10Col8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow10Col8.TabIndex = 49;
            this.filePictureBoxTableRow10Col8.TabStop = false;
            // 
            // filePictureBoxTableRow4Col8
            // 
            this.filePictureBoxTableRow4Col8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow4Col8.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow4Col8.Image")));
            this.filePictureBoxTableRow4Col8.Location = new System.Drawing.Point(277, 49);
            this.filePictureBoxTableRow4Col8.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow4Col8.Name = "filePictureBoxTableRow4Col8";
            this.filePictureBoxTableRow4Col8.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow4Col8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow4Col8.TabIndex = 49;
            this.filePictureBoxTableRow4Col8.TabStop = false;
            // 
            // filePictureBoxTableRow3Col8
            // 
            this.filePictureBoxTableRow3Col8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow3Col8.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow3Col8.Image")));
            this.filePictureBoxTableRow3Col8.Location = new System.Drawing.Point(277, 33);
            this.filePictureBoxTableRow3Col8.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow3Col8.Name = "filePictureBoxTableRow3Col8";
            this.filePictureBoxTableRow3Col8.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow3Col8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow3Col8.TabIndex = 49;
            this.filePictureBoxTableRow3Col8.TabStop = false;
            // 
            // filePictureBoxTableRow2Col8
            // 
            this.filePictureBoxTableRow2Col8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow2Col8.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow2Col8.Image")));
            this.filePictureBoxTableRow2Col8.Location = new System.Drawing.Point(277, 17);
            this.filePictureBoxTableRow2Col8.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow2Col8.Name = "filePictureBoxTableRow2Col8";
            this.filePictureBoxTableRow2Col8.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow2Col8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow2Col8.TabIndex = 49;
            this.filePictureBoxTableRow2Col8.TabStop = false;
            // 
            // filePictureBoxTableRow1Col8
            // 
            this.filePictureBoxTableRow1Col8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow1Col8.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow1Col8.Image")));
            this.filePictureBoxTableRow1Col8.Location = new System.Drawing.Point(277, 1);
            this.filePictureBoxTableRow1Col8.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow1Col8.Name = "filePictureBoxTableRow1Col8";
            this.filePictureBoxTableRow1Col8.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow1Col8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow1Col8.TabIndex = 48;
            this.filePictureBoxTableRow1Col8.TabStop = false;
            // 
            // filePictureBoxTableRow19Col2
            // 
            this.filePictureBoxTableRow19Col2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow19Col2.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow19Col2.Image")));
            this.filePictureBoxTableRow19Col2.Location = new System.Drawing.Point(103, 289);
            this.filePictureBoxTableRow19Col2.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow19Col2.Name = "filePictureBoxTableRow19Col2";
            this.filePictureBoxTableRow19Col2.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow19Col2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow19Col2.TabIndex = 42;
            this.filePictureBoxTableRow19Col2.TabStop = false;
            // 
            // filePictureBoxTableRow26Col2
            // 
            this.filePictureBoxTableRow26Col2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow26Col2.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow26Col2.Image")));
            this.filePictureBoxTableRow26Col2.Location = new System.Drawing.Point(103, 401);
            this.filePictureBoxTableRow26Col2.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow26Col2.Name = "filePictureBoxTableRow26Col2";
            this.filePictureBoxTableRow26Col2.Size = new System.Drawing.Size(28, 17);
            this.filePictureBoxTableRow26Col2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow26Col2.TabIndex = 42;
            this.filePictureBoxTableRow26Col2.TabStop = false;
            // 
            // filePictureBoxTableRow25Col3
            // 
            this.filePictureBoxTableRow25Col3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow25Col3.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow25Col3.Image")));
            this.filePictureBoxTableRow25Col3.Location = new System.Drawing.Point(132, 385);
            this.filePictureBoxTableRow25Col3.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow25Col3.Name = "filePictureBoxTableRow25Col3";
            this.filePictureBoxTableRow25Col3.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow25Col3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow25Col3.TabIndex = 42;
            this.filePictureBoxTableRow25Col3.TabStop = false;
            // 
            // filePictureBoxTableRow25Col2
            // 
            this.filePictureBoxTableRow25Col2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow25Col2.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow25Col2.Image")));
            this.filePictureBoxTableRow25Col2.Location = new System.Drawing.Point(103, 385);
            this.filePictureBoxTableRow25Col2.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow25Col2.Name = "filePictureBoxTableRow25Col2";
            this.filePictureBoxTableRow25Col2.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow25Col2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow25Col2.TabIndex = 42;
            this.filePictureBoxTableRow25Col2.TabStop = false;
            // 
            // filePictureBoxTableRow26Col1
            // 
            this.filePictureBoxTableRow26Col1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow26Col1.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow26Col1.Image")));
            this.filePictureBoxTableRow26Col1.Location = new System.Drawing.Point(74, 401);
            this.filePictureBoxTableRow26Col1.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow26Col1.Name = "filePictureBoxTableRow26Col1";
            this.filePictureBoxTableRow26Col1.Size = new System.Drawing.Size(28, 17);
            this.filePictureBoxTableRow26Col1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow26Col1.TabIndex = 42;
            this.filePictureBoxTableRow26Col1.TabStop = false;
            // 
            // filePictureBoxTableRow25Col1
            // 
            this.filePictureBoxTableRow25Col1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow25Col1.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow25Col1.Image")));
            this.filePictureBoxTableRow25Col1.Location = new System.Drawing.Point(74, 385);
            this.filePictureBoxTableRow25Col1.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow25Col1.Name = "filePictureBoxTableRow25Col1";
            this.filePictureBoxTableRow25Col1.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow25Col1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow25Col1.TabIndex = 42;
            this.filePictureBoxTableRow25Col1.TabStop = false;
            // 
            // filePictureBoxTableRow24Col1
            // 
            this.filePictureBoxTableRow24Col1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow24Col1.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow24Col1.Image")));
            this.filePictureBoxTableRow24Col1.Location = new System.Drawing.Point(74, 369);
            this.filePictureBoxTableRow24Col1.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow24Col1.Name = "filePictureBoxTableRow24Col1";
            this.filePictureBoxTableRow24Col1.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow24Col1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow24Col1.TabIndex = 42;
            this.filePictureBoxTableRow24Col1.TabStop = false;
            // 
            // filePictureBoxTableRow23Col1
            // 
            this.filePictureBoxTableRow23Col1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow23Col1.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow23Col1.Image")));
            this.filePictureBoxTableRow23Col1.Location = new System.Drawing.Point(74, 353);
            this.filePictureBoxTableRow23Col1.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow23Col1.Name = "filePictureBoxTableRow23Col1";
            this.filePictureBoxTableRow23Col1.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow23Col1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow23Col1.TabIndex = 42;
            this.filePictureBoxTableRow23Col1.TabStop = false;
            // 
            // filePictureBoxTableRow22Col1
            // 
            this.filePictureBoxTableRow22Col1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow22Col1.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow22Col1.Image")));
            this.filePictureBoxTableRow22Col1.Location = new System.Drawing.Point(74, 337);
            this.filePictureBoxTableRow22Col1.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow22Col1.Name = "filePictureBoxTableRow22Col1";
            this.filePictureBoxTableRow22Col1.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow22Col1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow22Col1.TabIndex = 42;
            this.filePictureBoxTableRow22Col1.TabStop = false;
            // 
            // filePictureBoxTableRow21Col1
            // 
            this.filePictureBoxTableRow21Col1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow21Col1.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow21Col1.Image")));
            this.filePictureBoxTableRow21Col1.Location = new System.Drawing.Point(74, 321);
            this.filePictureBoxTableRow21Col1.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow21Col1.Name = "filePictureBoxTableRow21Col1";
            this.filePictureBoxTableRow21Col1.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow21Col1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow21Col1.TabIndex = 42;
            this.filePictureBoxTableRow21Col1.TabStop = false;
            // 
            // filePictureBoxTableRow20Col1
            // 
            this.filePictureBoxTableRow20Col1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow20Col1.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow20Col1.Image")));
            this.filePictureBoxTableRow20Col1.Location = new System.Drawing.Point(74, 305);
            this.filePictureBoxTableRow20Col1.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow20Col1.Name = "filePictureBoxTableRow20Col1";
            this.filePictureBoxTableRow20Col1.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow20Col1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow20Col1.TabIndex = 42;
            this.filePictureBoxTableRow20Col1.TabStop = false;
            // 
            // filePictureBoxTableRow19Col1
            // 
            this.filePictureBoxTableRow19Col1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow19Col1.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow19Col1.Image")));
            this.filePictureBoxTableRow19Col1.Location = new System.Drawing.Point(74, 289);
            this.filePictureBoxTableRow19Col1.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow19Col1.Name = "filePictureBoxTableRow19Col1";
            this.filePictureBoxTableRow19Col1.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow19Col1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow19Col1.TabIndex = 42;
            this.filePictureBoxTableRow19Col1.TabStop = false;
            // 
            // filePictureBoxTableRow18Col1
            // 
            this.filePictureBoxTableRow18Col1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow18Col1.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow18Col1.Image")));
            this.filePictureBoxTableRow18Col1.Location = new System.Drawing.Point(74, 273);
            this.filePictureBoxTableRow18Col1.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow18Col1.Name = "filePictureBoxTableRow18Col1";
            this.filePictureBoxTableRow18Col1.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow18Col1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow18Col1.TabIndex = 42;
            this.filePictureBoxTableRow18Col1.TabStop = false;
            // 
            // filePictureBoxTableRow17Col1
            // 
            this.filePictureBoxTableRow17Col1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow17Col1.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow17Col1.Image")));
            this.filePictureBoxTableRow17Col1.Location = new System.Drawing.Point(74, 257);
            this.filePictureBoxTableRow17Col1.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow17Col1.Name = "filePictureBoxTableRow17Col1";
            this.filePictureBoxTableRow17Col1.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow17Col1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow17Col1.TabIndex = 42;
            this.filePictureBoxTableRow17Col1.TabStop = false;
            // 
            // filePictureBoxTableRow16Col1
            // 
            this.filePictureBoxTableRow16Col1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow16Col1.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow16Col1.Image")));
            this.filePictureBoxTableRow16Col1.Location = new System.Drawing.Point(74, 241);
            this.filePictureBoxTableRow16Col1.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow16Col1.Name = "filePictureBoxTableRow16Col1";
            this.filePictureBoxTableRow16Col1.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow16Col1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow16Col1.TabIndex = 42;
            this.filePictureBoxTableRow16Col1.TabStop = false;
            // 
            // filePictureBoxTableRow15Col7
            // 
            this.filePictureBoxTableRow15Col7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow15Col7.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow15Col7.Image")));
            this.filePictureBoxTableRow15Col7.Location = new System.Drawing.Point(248, 225);
            this.filePictureBoxTableRow15Col7.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow15Col7.Name = "filePictureBoxTableRow15Col7";
            this.filePictureBoxTableRow15Col7.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow15Col7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow15Col7.TabIndex = 47;
            this.filePictureBoxTableRow15Col7.TabStop = false;
            // 
            // filePictureBoxTableRow14Col7
            // 
            this.filePictureBoxTableRow14Col7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow14Col7.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow14Col7.Image")));
            this.filePictureBoxTableRow14Col7.Location = new System.Drawing.Point(248, 209);
            this.filePictureBoxTableRow14Col7.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow14Col7.Name = "filePictureBoxTableRow14Col7";
            this.filePictureBoxTableRow14Col7.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow14Col7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow14Col7.TabIndex = 47;
            this.filePictureBoxTableRow14Col7.TabStop = false;
            // 
            // filePictureBoxTableRow13Col7
            // 
            this.filePictureBoxTableRow13Col7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow13Col7.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow13Col7.Image")));
            this.filePictureBoxTableRow13Col7.Location = new System.Drawing.Point(248, 193);
            this.filePictureBoxTableRow13Col7.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow13Col7.Name = "filePictureBoxTableRow13Col7";
            this.filePictureBoxTableRow13Col7.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow13Col7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow13Col7.TabIndex = 47;
            this.filePictureBoxTableRow13Col7.TabStop = false;
            // 
            // filePictureBoxTableRow12Col7
            // 
            this.filePictureBoxTableRow12Col7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow12Col7.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow12Col7.Image")));
            this.filePictureBoxTableRow12Col7.Location = new System.Drawing.Point(248, 177);
            this.filePictureBoxTableRow12Col7.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow12Col7.Name = "filePictureBoxTableRow12Col7";
            this.filePictureBoxTableRow12Col7.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow12Col7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow12Col7.TabIndex = 47;
            this.filePictureBoxTableRow12Col7.TabStop = false;
            // 
            // filePictureBoxTableRow11Col7
            // 
            this.filePictureBoxTableRow11Col7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow11Col7.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow11Col7.Image")));
            this.filePictureBoxTableRow11Col7.Location = new System.Drawing.Point(248, 161);
            this.filePictureBoxTableRow11Col7.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow11Col7.Name = "filePictureBoxTableRow11Col7";
            this.filePictureBoxTableRow11Col7.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow11Col7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow11Col7.TabIndex = 47;
            this.filePictureBoxTableRow11Col7.TabStop = false;
            // 
            // filePictureBoxTableRow10Col7
            // 
            this.filePictureBoxTableRow10Col7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow10Col7.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow10Col7.Image")));
            this.filePictureBoxTableRow10Col7.Location = new System.Drawing.Point(248, 145);
            this.filePictureBoxTableRow10Col7.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow10Col7.Name = "filePictureBoxTableRow10Col7";
            this.filePictureBoxTableRow10Col7.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow10Col7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow10Col7.TabIndex = 47;
            this.filePictureBoxTableRow10Col7.TabStop = false;
            // 
            // filePictureBoxTableRow9Col7
            // 
            this.filePictureBoxTableRow9Col7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow9Col7.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow9Col7.Image")));
            this.filePictureBoxTableRow9Col7.Location = new System.Drawing.Point(248, 129);
            this.filePictureBoxTableRow9Col7.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow9Col7.Name = "filePictureBoxTableRow9Col7";
            this.filePictureBoxTableRow9Col7.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow9Col7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow9Col7.TabIndex = 47;
            this.filePictureBoxTableRow9Col7.TabStop = false;
            // 
            // filePictureBoxTableRow8Col7
            // 
            this.filePictureBoxTableRow8Col7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow8Col7.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow8Col7.Image")));
            this.filePictureBoxTableRow8Col7.Location = new System.Drawing.Point(248, 113);
            this.filePictureBoxTableRow8Col7.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow8Col7.Name = "filePictureBoxTableRow8Col7";
            this.filePictureBoxTableRow8Col7.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow8Col7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow8Col7.TabIndex = 47;
            this.filePictureBoxTableRow8Col7.TabStop = false;
            // 
            // filePictureBoxTableRow7Col7
            // 
            this.filePictureBoxTableRow7Col7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow7Col7.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow7Col7.Image")));
            this.filePictureBoxTableRow7Col7.Location = new System.Drawing.Point(248, 97);
            this.filePictureBoxTableRow7Col7.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow7Col7.Name = "filePictureBoxTableRow7Col7";
            this.filePictureBoxTableRow7Col7.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow7Col7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow7Col7.TabIndex = 47;
            this.filePictureBoxTableRow7Col7.TabStop = false;
            // 
            // filePictureBoxTableRow6Col7
            // 
            this.filePictureBoxTableRow6Col7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow6Col7.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow6Col7.Image")));
            this.filePictureBoxTableRow6Col7.Location = new System.Drawing.Point(248, 81);
            this.filePictureBoxTableRow6Col7.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow6Col7.Name = "filePictureBoxTableRow6Col7";
            this.filePictureBoxTableRow6Col7.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow6Col7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow6Col7.TabIndex = 47;
            this.filePictureBoxTableRow6Col7.TabStop = false;
            // 
            // filePictureBoxTableRow5Col7
            // 
            this.filePictureBoxTableRow5Col7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow5Col7.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow5Col7.Image")));
            this.filePictureBoxTableRow5Col7.Location = new System.Drawing.Point(248, 65);
            this.filePictureBoxTableRow5Col7.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow5Col7.Name = "filePictureBoxTableRow5Col7";
            this.filePictureBoxTableRow5Col7.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow5Col7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow5Col7.TabIndex = 47;
            this.filePictureBoxTableRow5Col7.TabStop = false;
            // 
            // filePictureBoxTableRow4Col7
            // 
            this.filePictureBoxTableRow4Col7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow4Col7.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow4Col7.Image")));
            this.filePictureBoxTableRow4Col7.Location = new System.Drawing.Point(248, 49);
            this.filePictureBoxTableRow4Col7.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow4Col7.Name = "filePictureBoxTableRow4Col7";
            this.filePictureBoxTableRow4Col7.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow4Col7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow4Col7.TabIndex = 47;
            this.filePictureBoxTableRow4Col7.TabStop = false;
            // 
            // filePictureBoxTableRow3Col7
            // 
            this.filePictureBoxTableRow3Col7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow3Col7.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow3Col7.Image")));
            this.filePictureBoxTableRow3Col7.Location = new System.Drawing.Point(248, 33);
            this.filePictureBoxTableRow3Col7.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow3Col7.Name = "filePictureBoxTableRow3Col7";
            this.filePictureBoxTableRow3Col7.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow3Col7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow3Col7.TabIndex = 47;
            this.filePictureBoxTableRow3Col7.TabStop = false;
            // 
            // filePictureBoxTableRow2Col7
            // 
            this.filePictureBoxTableRow2Col7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow2Col7.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow2Col7.Image")));
            this.filePictureBoxTableRow2Col7.Location = new System.Drawing.Point(248, 17);
            this.filePictureBoxTableRow2Col7.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow2Col7.Name = "filePictureBoxTableRow2Col7";
            this.filePictureBoxTableRow2Col7.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow2Col7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow2Col7.TabIndex = 47;
            this.filePictureBoxTableRow2Col7.TabStop = false;
            // 
            // filePictureBoxTableRow1Col7
            // 
            this.filePictureBoxTableRow1Col7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow1Col7.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow1Col7.Image")));
            this.filePictureBoxTableRow1Col7.Location = new System.Drawing.Point(248, 1);
            this.filePictureBoxTableRow1Col7.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow1Col7.Name = "filePictureBoxTableRow1Col7";
            this.filePictureBoxTableRow1Col7.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow1Col7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow1Col7.TabIndex = 47;
            this.filePictureBoxTableRow1Col7.TabStop = false;
            // 
            // filePictureBoxTableRow15Col6
            // 
            this.filePictureBoxTableRow15Col6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow15Col6.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow15Col6.Image")));
            this.filePictureBoxTableRow15Col6.Location = new System.Drawing.Point(219, 225);
            this.filePictureBoxTableRow15Col6.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow15Col6.Name = "filePictureBoxTableRow15Col6";
            this.filePictureBoxTableRow15Col6.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow15Col6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow15Col6.TabIndex = 46;
            this.filePictureBoxTableRow15Col6.TabStop = false;
            // 
            // filePictureBoxTableRow14Col6
            // 
            this.filePictureBoxTableRow14Col6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow14Col6.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow14Col6.Image")));
            this.filePictureBoxTableRow14Col6.Location = new System.Drawing.Point(219, 209);
            this.filePictureBoxTableRow14Col6.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow14Col6.Name = "filePictureBoxTableRow14Col6";
            this.filePictureBoxTableRow14Col6.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow14Col6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow14Col6.TabIndex = 46;
            this.filePictureBoxTableRow14Col6.TabStop = false;
            // 
            // filePictureBoxTableRow13Col6
            // 
            this.filePictureBoxTableRow13Col6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow13Col6.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow13Col6.Image")));
            this.filePictureBoxTableRow13Col6.Location = new System.Drawing.Point(219, 193);
            this.filePictureBoxTableRow13Col6.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow13Col6.Name = "filePictureBoxTableRow13Col6";
            this.filePictureBoxTableRow13Col6.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow13Col6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow13Col6.TabIndex = 46;
            this.filePictureBoxTableRow13Col6.TabStop = false;
            // 
            // filePictureBoxTableRow12Col6
            // 
            this.filePictureBoxTableRow12Col6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow12Col6.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow12Col6.Image")));
            this.filePictureBoxTableRow12Col6.Location = new System.Drawing.Point(219, 177);
            this.filePictureBoxTableRow12Col6.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow12Col6.Name = "filePictureBoxTableRow12Col6";
            this.filePictureBoxTableRow12Col6.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow12Col6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow12Col6.TabIndex = 46;
            this.filePictureBoxTableRow12Col6.TabStop = false;
            // 
            // filePictureBoxTableRow11Col6
            // 
            this.filePictureBoxTableRow11Col6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow11Col6.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow11Col6.Image")));
            this.filePictureBoxTableRow11Col6.Location = new System.Drawing.Point(219, 161);
            this.filePictureBoxTableRow11Col6.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow11Col6.Name = "filePictureBoxTableRow11Col6";
            this.filePictureBoxTableRow11Col6.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow11Col6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow11Col6.TabIndex = 46;
            this.filePictureBoxTableRow11Col6.TabStop = false;
            // 
            // filePictureBoxTableRow10Col6
            // 
            this.filePictureBoxTableRow10Col6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow10Col6.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow10Col6.Image")));
            this.filePictureBoxTableRow10Col6.Location = new System.Drawing.Point(219, 145);
            this.filePictureBoxTableRow10Col6.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow10Col6.Name = "filePictureBoxTableRow10Col6";
            this.filePictureBoxTableRow10Col6.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow10Col6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow10Col6.TabIndex = 46;
            this.filePictureBoxTableRow10Col6.TabStop = false;
            // 
            // filePictureBoxTableRow9Col6
            // 
            this.filePictureBoxTableRow9Col6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow9Col6.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow9Col6.Image")));
            this.filePictureBoxTableRow9Col6.Location = new System.Drawing.Point(219, 129);
            this.filePictureBoxTableRow9Col6.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow9Col6.Name = "filePictureBoxTableRow9Col6";
            this.filePictureBoxTableRow9Col6.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow9Col6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow9Col6.TabIndex = 46;
            this.filePictureBoxTableRow9Col6.TabStop = false;
            // 
            // filePictureBoxTableRow8Col6
            // 
            this.filePictureBoxTableRow8Col6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow8Col6.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow8Col6.Image")));
            this.filePictureBoxTableRow8Col6.Location = new System.Drawing.Point(219, 113);
            this.filePictureBoxTableRow8Col6.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow8Col6.Name = "filePictureBoxTableRow8Col6";
            this.filePictureBoxTableRow8Col6.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow8Col6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow8Col6.TabIndex = 46;
            this.filePictureBoxTableRow8Col6.TabStop = false;
            // 
            // filePictureBoxTableRow7Col6
            // 
            this.filePictureBoxTableRow7Col6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow7Col6.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow7Col6.Image")));
            this.filePictureBoxTableRow7Col6.Location = new System.Drawing.Point(219, 97);
            this.filePictureBoxTableRow7Col6.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow7Col6.Name = "filePictureBoxTableRow7Col6";
            this.filePictureBoxTableRow7Col6.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow7Col6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow7Col6.TabIndex = 46;
            this.filePictureBoxTableRow7Col6.TabStop = false;
            // 
            // filePictureBoxTableRow6Col6
            // 
            this.filePictureBoxTableRow6Col6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow6Col6.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow6Col6.Image")));
            this.filePictureBoxTableRow6Col6.Location = new System.Drawing.Point(219, 81);
            this.filePictureBoxTableRow6Col6.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow6Col6.Name = "filePictureBoxTableRow6Col6";
            this.filePictureBoxTableRow6Col6.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow6Col6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow6Col6.TabIndex = 46;
            this.filePictureBoxTableRow6Col6.TabStop = false;
            // 
            // filePictureBoxTableRow5Col6
            // 
            this.filePictureBoxTableRow5Col6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow5Col6.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow5Col6.Image")));
            this.filePictureBoxTableRow5Col6.Location = new System.Drawing.Point(219, 65);
            this.filePictureBoxTableRow5Col6.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow5Col6.Name = "filePictureBoxTableRow5Col6";
            this.filePictureBoxTableRow5Col6.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow5Col6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow5Col6.TabIndex = 46;
            this.filePictureBoxTableRow5Col6.TabStop = false;
            // 
            // filePictureBoxTableRow4Col6
            // 
            this.filePictureBoxTableRow4Col6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow4Col6.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow4Col6.Image")));
            this.filePictureBoxTableRow4Col6.Location = new System.Drawing.Point(219, 49);
            this.filePictureBoxTableRow4Col6.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow4Col6.Name = "filePictureBoxTableRow4Col6";
            this.filePictureBoxTableRow4Col6.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow4Col6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow4Col6.TabIndex = 46;
            this.filePictureBoxTableRow4Col6.TabStop = false;
            // 
            // filePictureBoxTableRow3Col6
            // 
            this.filePictureBoxTableRow3Col6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow3Col6.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow3Col6.Image")));
            this.filePictureBoxTableRow3Col6.Location = new System.Drawing.Point(219, 33);
            this.filePictureBoxTableRow3Col6.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow3Col6.Name = "filePictureBoxTableRow3Col6";
            this.filePictureBoxTableRow3Col6.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow3Col6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow3Col6.TabIndex = 46;
            this.filePictureBoxTableRow3Col6.TabStop = false;
            // 
            // filePictureBoxTableRow2Col6
            // 
            this.filePictureBoxTableRow2Col6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow2Col6.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow2Col6.Image")));
            this.filePictureBoxTableRow2Col6.Location = new System.Drawing.Point(219, 17);
            this.filePictureBoxTableRow2Col6.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow2Col6.Name = "filePictureBoxTableRow2Col6";
            this.filePictureBoxTableRow2Col6.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow2Col6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow2Col6.TabIndex = 46;
            this.filePictureBoxTableRow2Col6.TabStop = false;
            // 
            // filePictureBoxTableRow1Col6
            // 
            this.filePictureBoxTableRow1Col6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow1Col6.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow1Col6.Image")));
            this.filePictureBoxTableRow1Col6.Location = new System.Drawing.Point(219, 1);
            this.filePictureBoxTableRow1Col6.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow1Col6.Name = "filePictureBoxTableRow1Col6";
            this.filePictureBoxTableRow1Col6.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow1Col6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow1Col6.TabIndex = 46;
            this.filePictureBoxTableRow1Col6.TabStop = false;
            // 
            // filePictureBoxTableRow15Col5
            // 
            this.filePictureBoxTableRow15Col5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow15Col5.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow15Col5.Image")));
            this.filePictureBoxTableRow15Col5.Location = new System.Drawing.Point(190, 225);
            this.filePictureBoxTableRow15Col5.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow15Col5.Name = "filePictureBoxTableRow15Col5";
            this.filePictureBoxTableRow15Col5.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow15Col5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow15Col5.TabIndex = 46;
            this.filePictureBoxTableRow15Col5.TabStop = false;
            // 
            // filePictureBoxTableRow14Col5
            // 
            this.filePictureBoxTableRow14Col5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow14Col5.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow14Col5.Image")));
            this.filePictureBoxTableRow14Col5.Location = new System.Drawing.Point(190, 209);
            this.filePictureBoxTableRow14Col5.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow14Col5.Name = "filePictureBoxTableRow14Col5";
            this.filePictureBoxTableRow14Col5.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow14Col5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow14Col5.TabIndex = 46;
            this.filePictureBoxTableRow14Col5.TabStop = false;
            // 
            // filePictureBoxTableRow13Col5
            // 
            this.filePictureBoxTableRow13Col5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow13Col5.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow13Col5.Image")));
            this.filePictureBoxTableRow13Col5.Location = new System.Drawing.Point(190, 193);
            this.filePictureBoxTableRow13Col5.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow13Col5.Name = "filePictureBoxTableRow13Col5";
            this.filePictureBoxTableRow13Col5.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow13Col5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow13Col5.TabIndex = 46;
            this.filePictureBoxTableRow13Col5.TabStop = false;
            // 
            // filePictureBoxTableRow12Col5
            // 
            this.filePictureBoxTableRow12Col5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow12Col5.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow12Col5.Image")));
            this.filePictureBoxTableRow12Col5.Location = new System.Drawing.Point(190, 177);
            this.filePictureBoxTableRow12Col5.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow12Col5.Name = "filePictureBoxTableRow12Col5";
            this.filePictureBoxTableRow12Col5.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow12Col5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow12Col5.TabIndex = 46;
            this.filePictureBoxTableRow12Col5.TabStop = false;
            // 
            // filePictureBoxTableRow11Col5
            // 
            this.filePictureBoxTableRow11Col5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow11Col5.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow11Col5.Image")));
            this.filePictureBoxTableRow11Col5.Location = new System.Drawing.Point(190, 161);
            this.filePictureBoxTableRow11Col5.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow11Col5.Name = "filePictureBoxTableRow11Col5";
            this.filePictureBoxTableRow11Col5.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow11Col5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow11Col5.TabIndex = 46;
            this.filePictureBoxTableRow11Col5.TabStop = false;
            // 
            // filePictureBoxTableRow10Col5
            // 
            this.filePictureBoxTableRow10Col5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow10Col5.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow10Col5.Image")));
            this.filePictureBoxTableRow10Col5.Location = new System.Drawing.Point(190, 145);
            this.filePictureBoxTableRow10Col5.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow10Col5.Name = "filePictureBoxTableRow10Col5";
            this.filePictureBoxTableRow10Col5.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow10Col5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow10Col5.TabIndex = 46;
            this.filePictureBoxTableRow10Col5.TabStop = false;
            // 
            // filePictureBoxTableRow9Col5
            // 
            this.filePictureBoxTableRow9Col5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow9Col5.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow9Col5.Image")));
            this.filePictureBoxTableRow9Col5.Location = new System.Drawing.Point(190, 129);
            this.filePictureBoxTableRow9Col5.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow9Col5.Name = "filePictureBoxTableRow9Col5";
            this.filePictureBoxTableRow9Col5.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow9Col5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow9Col5.TabIndex = 46;
            this.filePictureBoxTableRow9Col5.TabStop = false;
            // 
            // filePictureBoxTableRow8Col5
            // 
            this.filePictureBoxTableRow8Col5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow8Col5.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow8Col5.Image")));
            this.filePictureBoxTableRow8Col5.Location = new System.Drawing.Point(190, 113);
            this.filePictureBoxTableRow8Col5.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow8Col5.Name = "filePictureBoxTableRow8Col5";
            this.filePictureBoxTableRow8Col5.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow8Col5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow8Col5.TabIndex = 46;
            this.filePictureBoxTableRow8Col5.TabStop = false;
            // 
            // filePictureBoxTableRow7Col5
            // 
            this.filePictureBoxTableRow7Col5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow7Col5.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow7Col5.Image")));
            this.filePictureBoxTableRow7Col5.Location = new System.Drawing.Point(190, 97);
            this.filePictureBoxTableRow7Col5.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow7Col5.Name = "filePictureBoxTableRow7Col5";
            this.filePictureBoxTableRow7Col5.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow7Col5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow7Col5.TabIndex = 46;
            this.filePictureBoxTableRow7Col5.TabStop = false;
            // 
            // filePictureBoxTableRow6Col5
            // 
            this.filePictureBoxTableRow6Col5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow6Col5.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow6Col5.Image")));
            this.filePictureBoxTableRow6Col5.Location = new System.Drawing.Point(190, 81);
            this.filePictureBoxTableRow6Col5.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow6Col5.Name = "filePictureBoxTableRow6Col5";
            this.filePictureBoxTableRow6Col5.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow6Col5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow6Col5.TabIndex = 46;
            this.filePictureBoxTableRow6Col5.TabStop = false;
            // 
            // filePictureBoxTableRow5Col5
            // 
            this.filePictureBoxTableRow5Col5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow5Col5.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow5Col5.Image")));
            this.filePictureBoxTableRow5Col5.Location = new System.Drawing.Point(190, 65);
            this.filePictureBoxTableRow5Col5.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow5Col5.Name = "filePictureBoxTableRow5Col5";
            this.filePictureBoxTableRow5Col5.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow5Col5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow5Col5.TabIndex = 46;
            this.filePictureBoxTableRow5Col5.TabStop = false;
            // 
            // filePictureBoxTableRow4Col5
            // 
            this.filePictureBoxTableRow4Col5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow4Col5.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow4Col5.Image")));
            this.filePictureBoxTableRow4Col5.Location = new System.Drawing.Point(190, 49);
            this.filePictureBoxTableRow4Col5.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow4Col5.Name = "filePictureBoxTableRow4Col5";
            this.filePictureBoxTableRow4Col5.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow4Col5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow4Col5.TabIndex = 46;
            this.filePictureBoxTableRow4Col5.TabStop = false;
            // 
            // filePictureBoxTableRow3Col5
            // 
            this.filePictureBoxTableRow3Col5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow3Col5.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow3Col5.Image")));
            this.filePictureBoxTableRow3Col5.Location = new System.Drawing.Point(190, 33);
            this.filePictureBoxTableRow3Col5.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow3Col5.Name = "filePictureBoxTableRow3Col5";
            this.filePictureBoxTableRow3Col5.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow3Col5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow3Col5.TabIndex = 46;
            this.filePictureBoxTableRow3Col5.TabStop = false;
            // 
            // filePictureBoxTableRow2Col5
            // 
            this.filePictureBoxTableRow2Col5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow2Col5.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow2Col5.Image")));
            this.filePictureBoxTableRow2Col5.Location = new System.Drawing.Point(190, 17);
            this.filePictureBoxTableRow2Col5.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow2Col5.Name = "filePictureBoxTableRow2Col5";
            this.filePictureBoxTableRow2Col5.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow2Col5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow2Col5.TabIndex = 45;
            this.filePictureBoxTableRow2Col5.TabStop = false;
            // 
            // filePictureBoxTableRow1Col5
            // 
            this.filePictureBoxTableRow1Col5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow1Col5.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow1Col5.Image")));
            this.filePictureBoxTableRow1Col5.Location = new System.Drawing.Point(190, 1);
            this.filePictureBoxTableRow1Col5.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow1Col5.Name = "filePictureBoxTableRow1Col5";
            this.filePictureBoxTableRow1Col5.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow1Col5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow1Col5.TabIndex = 45;
            this.filePictureBoxTableRow1Col5.TabStop = false;
            // 
            // filePictureBoxTableRow15Col4
            // 
            this.filePictureBoxTableRow15Col4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow15Col4.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow15Col4.Image")));
            this.filePictureBoxTableRow15Col4.Location = new System.Drawing.Point(161, 225);
            this.filePictureBoxTableRow15Col4.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow15Col4.Name = "filePictureBoxTableRow15Col4";
            this.filePictureBoxTableRow15Col4.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow15Col4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow15Col4.TabIndex = 44;
            this.filePictureBoxTableRow15Col4.TabStop = false;
            // 
            // filePictureBoxTableRow14Col4
            // 
            this.filePictureBoxTableRow14Col4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow14Col4.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow14Col4.Image")));
            this.filePictureBoxTableRow14Col4.Location = new System.Drawing.Point(161, 209);
            this.filePictureBoxTableRow14Col4.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow14Col4.Name = "filePictureBoxTableRow14Col4";
            this.filePictureBoxTableRow14Col4.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow14Col4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow14Col4.TabIndex = 44;
            this.filePictureBoxTableRow14Col4.TabStop = false;
            // 
            // filePictureBoxTableRow13Col4
            // 
            this.filePictureBoxTableRow13Col4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow13Col4.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow13Col4.Image")));
            this.filePictureBoxTableRow13Col4.Location = new System.Drawing.Point(161, 193);
            this.filePictureBoxTableRow13Col4.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow13Col4.Name = "filePictureBoxTableRow13Col4";
            this.filePictureBoxTableRow13Col4.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow13Col4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow13Col4.TabIndex = 44;
            this.filePictureBoxTableRow13Col4.TabStop = false;
            // 
            // filePictureBoxTableRow12Col4
            // 
            this.filePictureBoxTableRow12Col4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow12Col4.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow12Col4.Image")));
            this.filePictureBoxTableRow12Col4.Location = new System.Drawing.Point(161, 177);
            this.filePictureBoxTableRow12Col4.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow12Col4.Name = "filePictureBoxTableRow12Col4";
            this.filePictureBoxTableRow12Col4.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow12Col4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow12Col4.TabIndex = 44;
            this.filePictureBoxTableRow12Col4.TabStop = false;
            // 
            // filePictureBoxTableRow11Col4
            // 
            this.filePictureBoxTableRow11Col4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow11Col4.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow11Col4.Image")));
            this.filePictureBoxTableRow11Col4.Location = new System.Drawing.Point(161, 161);
            this.filePictureBoxTableRow11Col4.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow11Col4.Name = "filePictureBoxTableRow11Col4";
            this.filePictureBoxTableRow11Col4.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow11Col4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow11Col4.TabIndex = 44;
            this.filePictureBoxTableRow11Col4.TabStop = false;
            // 
            // filePictureBoxTableRow10Col4
            // 
            this.filePictureBoxTableRow10Col4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow10Col4.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow10Col4.Image")));
            this.filePictureBoxTableRow10Col4.Location = new System.Drawing.Point(161, 145);
            this.filePictureBoxTableRow10Col4.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow10Col4.Name = "filePictureBoxTableRow10Col4";
            this.filePictureBoxTableRow10Col4.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow10Col4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow10Col4.TabIndex = 44;
            this.filePictureBoxTableRow10Col4.TabStop = false;
            // 
            // filePictureBoxTableRow9Col4
            // 
            this.filePictureBoxTableRow9Col4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow9Col4.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow9Col4.Image")));
            this.filePictureBoxTableRow9Col4.Location = new System.Drawing.Point(161, 129);
            this.filePictureBoxTableRow9Col4.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow9Col4.Name = "filePictureBoxTableRow9Col4";
            this.filePictureBoxTableRow9Col4.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow9Col4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow9Col4.TabIndex = 44;
            this.filePictureBoxTableRow9Col4.TabStop = false;
            // 
            // filePictureBoxTableRow8Col4
            // 
            this.filePictureBoxTableRow8Col4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow8Col4.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow8Col4.Image")));
            this.filePictureBoxTableRow8Col4.Location = new System.Drawing.Point(161, 113);
            this.filePictureBoxTableRow8Col4.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow8Col4.Name = "filePictureBoxTableRow8Col4";
            this.filePictureBoxTableRow8Col4.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow8Col4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow8Col4.TabIndex = 44;
            this.filePictureBoxTableRow8Col4.TabStop = false;
            // 
            // filePictureBoxTableRow7Col4
            // 
            this.filePictureBoxTableRow7Col4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow7Col4.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow7Col4.Image")));
            this.filePictureBoxTableRow7Col4.Location = new System.Drawing.Point(161, 97);
            this.filePictureBoxTableRow7Col4.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow7Col4.Name = "filePictureBoxTableRow7Col4";
            this.filePictureBoxTableRow7Col4.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow7Col4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow7Col4.TabIndex = 44;
            this.filePictureBoxTableRow7Col4.TabStop = false;
            // 
            // filePictureBoxTableRow6Col4
            // 
            this.filePictureBoxTableRow6Col4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow6Col4.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow6Col4.Image")));
            this.filePictureBoxTableRow6Col4.Location = new System.Drawing.Point(161, 81);
            this.filePictureBoxTableRow6Col4.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow6Col4.Name = "filePictureBoxTableRow6Col4";
            this.filePictureBoxTableRow6Col4.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow6Col4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow6Col4.TabIndex = 44;
            this.filePictureBoxTableRow6Col4.TabStop = false;
            // 
            // filePictureBoxTableRow5Col4
            // 
            this.filePictureBoxTableRow5Col4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow5Col4.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow5Col4.Image")));
            this.filePictureBoxTableRow5Col4.Location = new System.Drawing.Point(161, 65);
            this.filePictureBoxTableRow5Col4.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow5Col4.Name = "filePictureBoxTableRow5Col4";
            this.filePictureBoxTableRow5Col4.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow5Col4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow5Col4.TabIndex = 44;
            this.filePictureBoxTableRow5Col4.TabStop = false;
            // 
            // filePictureBoxTableRow4Col4
            // 
            this.filePictureBoxTableRow4Col4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow4Col4.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow4Col4.Image")));
            this.filePictureBoxTableRow4Col4.Location = new System.Drawing.Point(161, 49);
            this.filePictureBoxTableRow4Col4.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow4Col4.Name = "filePictureBoxTableRow4Col4";
            this.filePictureBoxTableRow4Col4.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow4Col4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow4Col4.TabIndex = 44;
            this.filePictureBoxTableRow4Col4.TabStop = false;
            // 
            // filePictureBoxTableRow3Col4
            // 
            this.filePictureBoxTableRow3Col4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow3Col4.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow3Col4.Image")));
            this.filePictureBoxTableRow3Col4.Location = new System.Drawing.Point(161, 33);
            this.filePictureBoxTableRow3Col4.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow3Col4.Name = "filePictureBoxTableRow3Col4";
            this.filePictureBoxTableRow3Col4.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow3Col4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow3Col4.TabIndex = 44;
            this.filePictureBoxTableRow3Col4.TabStop = false;
            // 
            // filePictureBoxTableRow2Col4
            // 
            this.filePictureBoxTableRow2Col4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow2Col4.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow2Col4.Image")));
            this.filePictureBoxTableRow2Col4.Location = new System.Drawing.Point(161, 17);
            this.filePictureBoxTableRow2Col4.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow2Col4.Name = "filePictureBoxTableRow2Col4";
            this.filePictureBoxTableRow2Col4.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow2Col4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow2Col4.TabIndex = 44;
            this.filePictureBoxTableRow2Col4.TabStop = false;
            // 
            // filePictureBoxTableRow1Col4
            // 
            this.filePictureBoxTableRow1Col4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow1Col4.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow1Col4.Image")));
            this.filePictureBoxTableRow1Col4.Location = new System.Drawing.Point(161, 1);
            this.filePictureBoxTableRow1Col4.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow1Col4.Name = "filePictureBoxTableRow1Col4";
            this.filePictureBoxTableRow1Col4.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow1Col4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow1Col4.TabIndex = 44;
            this.filePictureBoxTableRow1Col4.TabStop = false;
            // 
            // filePictureBoxTableRow15Col3
            // 
            this.filePictureBoxTableRow15Col3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow15Col3.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow15Col3.Image")));
            this.filePictureBoxTableRow15Col3.Location = new System.Drawing.Point(132, 225);
            this.filePictureBoxTableRow15Col3.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow15Col3.Name = "filePictureBoxTableRow15Col3";
            this.filePictureBoxTableRow15Col3.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow15Col3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow15Col3.TabIndex = 43;
            this.filePictureBoxTableRow15Col3.TabStop = false;
            // 
            // filePictureBoxTableRow14Col3
            // 
            this.filePictureBoxTableRow14Col3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow14Col3.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow14Col3.Image")));
            this.filePictureBoxTableRow14Col3.Location = new System.Drawing.Point(132, 209);
            this.filePictureBoxTableRow14Col3.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow14Col3.Name = "filePictureBoxTableRow14Col3";
            this.filePictureBoxTableRow14Col3.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow14Col3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow14Col3.TabIndex = 43;
            this.filePictureBoxTableRow14Col3.TabStop = false;
            // 
            // filePictureBoxTableRow13Col3
            // 
            this.filePictureBoxTableRow13Col3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow13Col3.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow13Col3.Image")));
            this.filePictureBoxTableRow13Col3.Location = new System.Drawing.Point(132, 193);
            this.filePictureBoxTableRow13Col3.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow13Col3.Name = "filePictureBoxTableRow13Col3";
            this.filePictureBoxTableRow13Col3.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow13Col3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow13Col3.TabIndex = 43;
            this.filePictureBoxTableRow13Col3.TabStop = false;
            // 
            // filePictureBoxTableRow12Col3
            // 
            this.filePictureBoxTableRow12Col3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow12Col3.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow12Col3.Image")));
            this.filePictureBoxTableRow12Col3.Location = new System.Drawing.Point(132, 177);
            this.filePictureBoxTableRow12Col3.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow12Col3.Name = "filePictureBoxTableRow12Col3";
            this.filePictureBoxTableRow12Col3.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow12Col3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow12Col3.TabIndex = 43;
            this.filePictureBoxTableRow12Col3.TabStop = false;
            // 
            // filePictureBoxTableRow11Col3
            // 
            this.filePictureBoxTableRow11Col3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow11Col3.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow11Col3.Image")));
            this.filePictureBoxTableRow11Col3.Location = new System.Drawing.Point(132, 161);
            this.filePictureBoxTableRow11Col3.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow11Col3.Name = "filePictureBoxTableRow11Col3";
            this.filePictureBoxTableRow11Col3.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow11Col3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow11Col3.TabIndex = 43;
            this.filePictureBoxTableRow11Col3.TabStop = false;
            // 
            // filePictureBoxTableRow10Col3
            // 
            this.filePictureBoxTableRow10Col3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow10Col3.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow10Col3.Image")));
            this.filePictureBoxTableRow10Col3.Location = new System.Drawing.Point(132, 145);
            this.filePictureBoxTableRow10Col3.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow10Col3.Name = "filePictureBoxTableRow10Col3";
            this.filePictureBoxTableRow10Col3.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow10Col3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow10Col3.TabIndex = 43;
            this.filePictureBoxTableRow10Col3.TabStop = false;
            // 
            // filePictureBoxTableRow9Col3
            // 
            this.filePictureBoxTableRow9Col3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow9Col3.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow9Col3.Image")));
            this.filePictureBoxTableRow9Col3.Location = new System.Drawing.Point(132, 129);
            this.filePictureBoxTableRow9Col3.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow9Col3.Name = "filePictureBoxTableRow9Col3";
            this.filePictureBoxTableRow9Col3.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow9Col3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow9Col3.TabIndex = 43;
            this.filePictureBoxTableRow9Col3.TabStop = false;
            // 
            // filePictureBoxTableRow8Col3
            // 
            this.filePictureBoxTableRow8Col3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow8Col3.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow8Col3.Image")));
            this.filePictureBoxTableRow8Col3.Location = new System.Drawing.Point(132, 113);
            this.filePictureBoxTableRow8Col3.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow8Col3.Name = "filePictureBoxTableRow8Col3";
            this.filePictureBoxTableRow8Col3.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow8Col3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow8Col3.TabIndex = 43;
            this.filePictureBoxTableRow8Col3.TabStop = false;
            // 
            // filePictureBoxTableRow7Col3
            // 
            this.filePictureBoxTableRow7Col3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow7Col3.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow7Col3.Image")));
            this.filePictureBoxTableRow7Col3.Location = new System.Drawing.Point(132, 97);
            this.filePictureBoxTableRow7Col3.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow7Col3.Name = "filePictureBoxTableRow7Col3";
            this.filePictureBoxTableRow7Col3.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow7Col3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow7Col3.TabIndex = 43;
            this.filePictureBoxTableRow7Col3.TabStop = false;
            // 
            // filePictureBoxTableRow6Col3
            // 
            this.filePictureBoxTableRow6Col3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow6Col3.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow6Col3.Image")));
            this.filePictureBoxTableRow6Col3.Location = new System.Drawing.Point(132, 81);
            this.filePictureBoxTableRow6Col3.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow6Col3.Name = "filePictureBoxTableRow6Col3";
            this.filePictureBoxTableRow6Col3.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow6Col3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow6Col3.TabIndex = 43;
            this.filePictureBoxTableRow6Col3.TabStop = false;
            // 
            // filePictureBoxTableRow5Col3
            // 
            this.filePictureBoxTableRow5Col3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow5Col3.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow5Col3.Image")));
            this.filePictureBoxTableRow5Col3.Location = new System.Drawing.Point(132, 65);
            this.filePictureBoxTableRow5Col3.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow5Col3.Name = "filePictureBoxTableRow5Col3";
            this.filePictureBoxTableRow5Col3.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow5Col3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow5Col3.TabIndex = 43;
            this.filePictureBoxTableRow5Col3.TabStop = false;
            // 
            // filePictureBoxTableRow4Col3
            // 
            this.filePictureBoxTableRow4Col3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow4Col3.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow4Col3.Image")));
            this.filePictureBoxTableRow4Col3.Location = new System.Drawing.Point(132, 49);
            this.filePictureBoxTableRow4Col3.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow4Col3.Name = "filePictureBoxTableRow4Col3";
            this.filePictureBoxTableRow4Col3.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow4Col3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow4Col3.TabIndex = 43;
            this.filePictureBoxTableRow4Col3.TabStop = false;
            // 
            // filePictureBoxTableRow3Col3
            // 
            this.filePictureBoxTableRow3Col3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow3Col3.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow3Col3.Image")));
            this.filePictureBoxTableRow3Col3.Location = new System.Drawing.Point(132, 33);
            this.filePictureBoxTableRow3Col3.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow3Col3.Name = "filePictureBoxTableRow3Col3";
            this.filePictureBoxTableRow3Col3.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow3Col3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow3Col3.TabIndex = 43;
            this.filePictureBoxTableRow3Col3.TabStop = false;
            // 
            // filePictureBoxTableRow2Col3
            // 
            this.filePictureBoxTableRow2Col3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow2Col3.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow2Col3.Image")));
            this.filePictureBoxTableRow2Col3.Location = new System.Drawing.Point(132, 17);
            this.filePictureBoxTableRow2Col3.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow2Col3.Name = "filePictureBoxTableRow2Col3";
            this.filePictureBoxTableRow2Col3.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow2Col3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow2Col3.TabIndex = 43;
            this.filePictureBoxTableRow2Col3.TabStop = false;
            // 
            // filePictureBoxTableRow1Col3
            // 
            this.filePictureBoxTableRow1Col3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow1Col3.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow1Col3.Image")));
            this.filePictureBoxTableRow1Col3.Location = new System.Drawing.Point(132, 1);
            this.filePictureBoxTableRow1Col3.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow1Col3.Name = "filePictureBoxTableRow1Col3";
            this.filePictureBoxTableRow1Col3.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow1Col3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow1Col3.TabIndex = 43;
            this.filePictureBoxTableRow1Col3.TabStop = false;
            // 
            // filePictureBoxTableRow15Col2
            // 
            this.filePictureBoxTableRow15Col2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow15Col2.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow15Col2.Image")));
            this.filePictureBoxTableRow15Col2.Location = new System.Drawing.Point(103, 225);
            this.filePictureBoxTableRow15Col2.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow15Col2.Name = "filePictureBoxTableRow15Col2";
            this.filePictureBoxTableRow15Col2.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow15Col2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow15Col2.TabIndex = 42;
            this.filePictureBoxTableRow15Col2.TabStop = false;
            // 
            // filePictureBoxTableRow14Col2
            // 
            this.filePictureBoxTableRow14Col2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow14Col2.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow14Col2.Image")));
            this.filePictureBoxTableRow14Col2.Location = new System.Drawing.Point(103, 209);
            this.filePictureBoxTableRow14Col2.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow14Col2.Name = "filePictureBoxTableRow14Col2";
            this.filePictureBoxTableRow14Col2.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow14Col2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow14Col2.TabIndex = 42;
            this.filePictureBoxTableRow14Col2.TabStop = false;
            // 
            // filePictureBoxTableRow13Col2
            // 
            this.filePictureBoxTableRow13Col2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow13Col2.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow13Col2.Image")));
            this.filePictureBoxTableRow13Col2.Location = new System.Drawing.Point(103, 193);
            this.filePictureBoxTableRow13Col2.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow13Col2.Name = "filePictureBoxTableRow13Col2";
            this.filePictureBoxTableRow13Col2.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow13Col2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow13Col2.TabIndex = 42;
            this.filePictureBoxTableRow13Col2.TabStop = false;
            // 
            // filePictureBoxTableRow12Col2
            // 
            this.filePictureBoxTableRow12Col2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow12Col2.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow12Col2.Image")));
            this.filePictureBoxTableRow12Col2.Location = new System.Drawing.Point(103, 177);
            this.filePictureBoxTableRow12Col2.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow12Col2.Name = "filePictureBoxTableRow12Col2";
            this.filePictureBoxTableRow12Col2.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow12Col2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow12Col2.TabIndex = 42;
            this.filePictureBoxTableRow12Col2.TabStop = false;
            // 
            // filePictureBoxTableRow11Col2
            // 
            this.filePictureBoxTableRow11Col2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow11Col2.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow11Col2.Image")));
            this.filePictureBoxTableRow11Col2.Location = new System.Drawing.Point(103, 161);
            this.filePictureBoxTableRow11Col2.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow11Col2.Name = "filePictureBoxTableRow11Col2";
            this.filePictureBoxTableRow11Col2.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow11Col2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow11Col2.TabIndex = 42;
            this.filePictureBoxTableRow11Col2.TabStop = false;
            // 
            // filePictureBoxTableRow10Col2
            // 
            this.filePictureBoxTableRow10Col2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow10Col2.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow10Col2.Image")));
            this.filePictureBoxTableRow10Col2.Location = new System.Drawing.Point(103, 145);
            this.filePictureBoxTableRow10Col2.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow10Col2.Name = "filePictureBoxTableRow10Col2";
            this.filePictureBoxTableRow10Col2.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow10Col2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow10Col2.TabIndex = 42;
            this.filePictureBoxTableRow10Col2.TabStop = false;
            // 
            // filePictureBoxTableRow9Col2
            // 
            this.filePictureBoxTableRow9Col2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow9Col2.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow9Col2.Image")));
            this.filePictureBoxTableRow9Col2.Location = new System.Drawing.Point(103, 129);
            this.filePictureBoxTableRow9Col2.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow9Col2.Name = "filePictureBoxTableRow9Col2";
            this.filePictureBoxTableRow9Col2.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow9Col2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow9Col2.TabIndex = 42;
            this.filePictureBoxTableRow9Col2.TabStop = false;
            // 
            // filePictureBoxTableRow8Col2
            // 
            this.filePictureBoxTableRow8Col2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow8Col2.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow8Col2.Image")));
            this.filePictureBoxTableRow8Col2.Location = new System.Drawing.Point(103, 113);
            this.filePictureBoxTableRow8Col2.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow8Col2.Name = "filePictureBoxTableRow8Col2";
            this.filePictureBoxTableRow8Col2.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow8Col2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow8Col2.TabIndex = 42;
            this.filePictureBoxTableRow8Col2.TabStop = false;
            // 
            // filePictureBoxTableRow7Col2
            // 
            this.filePictureBoxTableRow7Col2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow7Col2.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow7Col2.Image")));
            this.filePictureBoxTableRow7Col2.Location = new System.Drawing.Point(103, 97);
            this.filePictureBoxTableRow7Col2.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow7Col2.Name = "filePictureBoxTableRow7Col2";
            this.filePictureBoxTableRow7Col2.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow7Col2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow7Col2.TabIndex = 42;
            this.filePictureBoxTableRow7Col2.TabStop = false;
            // 
            // filePictureBoxTableRow6Col2
            // 
            this.filePictureBoxTableRow6Col2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow6Col2.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow6Col2.Image")));
            this.filePictureBoxTableRow6Col2.Location = new System.Drawing.Point(103, 81);
            this.filePictureBoxTableRow6Col2.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow6Col2.Name = "filePictureBoxTableRow6Col2";
            this.filePictureBoxTableRow6Col2.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow6Col2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow6Col2.TabIndex = 42;
            this.filePictureBoxTableRow6Col2.TabStop = false;
            // 
            // filePictureBoxTableRow5Col2
            // 
            this.filePictureBoxTableRow5Col2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow5Col2.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow5Col2.Image")));
            this.filePictureBoxTableRow5Col2.Location = new System.Drawing.Point(103, 65);
            this.filePictureBoxTableRow5Col2.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow5Col2.Name = "filePictureBoxTableRow5Col2";
            this.filePictureBoxTableRow5Col2.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow5Col2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow5Col2.TabIndex = 42;
            this.filePictureBoxTableRow5Col2.TabStop = false;
            // 
            // filePictureBoxTableRow4Col2
            // 
            this.filePictureBoxTableRow4Col2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow4Col2.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow4Col2.Image")));
            this.filePictureBoxTableRow4Col2.Location = new System.Drawing.Point(103, 49);
            this.filePictureBoxTableRow4Col2.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow4Col2.Name = "filePictureBoxTableRow4Col2";
            this.filePictureBoxTableRow4Col2.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow4Col2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow4Col2.TabIndex = 42;
            this.filePictureBoxTableRow4Col2.TabStop = false;
            // 
            // filePictureBoxTableRow3Col2
            // 
            this.filePictureBoxTableRow3Col2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow3Col2.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow3Col2.Image")));
            this.filePictureBoxTableRow3Col2.Location = new System.Drawing.Point(103, 33);
            this.filePictureBoxTableRow3Col2.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow3Col2.Name = "filePictureBoxTableRow3Col2";
            this.filePictureBoxTableRow3Col2.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow3Col2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow3Col2.TabIndex = 42;
            this.filePictureBoxTableRow3Col2.TabStop = false;
            // 
            // filePictureBoxTableRow2Col2
            // 
            this.filePictureBoxTableRow2Col2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow2Col2.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow2Col2.Image")));
            this.filePictureBoxTableRow2Col2.Location = new System.Drawing.Point(103, 17);
            this.filePictureBoxTableRow2Col2.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow2Col2.Name = "filePictureBoxTableRow2Col2";
            this.filePictureBoxTableRow2Col2.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow2Col2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow2Col2.TabIndex = 42;
            this.filePictureBoxTableRow2Col2.TabStop = false;
            // 
            // filePictureBoxTableRow1Col2
            // 
            this.filePictureBoxTableRow1Col2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow1Col2.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow1Col2.Image")));
            this.filePictureBoxTableRow1Col2.Location = new System.Drawing.Point(103, 1);
            this.filePictureBoxTableRow1Col2.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow1Col2.Name = "filePictureBoxTableRow1Col2";
            this.filePictureBoxTableRow1Col2.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow1Col2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow1Col2.TabIndex = 42;
            this.filePictureBoxTableRow1Col2.TabStop = false;
            // 
            // filePictureBoxTableRow15Col1
            // 
            this.filePictureBoxTableRow15Col1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow15Col1.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow15Col1.Image")));
            this.filePictureBoxTableRow15Col1.Location = new System.Drawing.Point(74, 225);
            this.filePictureBoxTableRow15Col1.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow15Col1.Name = "filePictureBoxTableRow15Col1";
            this.filePictureBoxTableRow15Col1.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow15Col1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow15Col1.TabIndex = 43;
            this.filePictureBoxTableRow15Col1.TabStop = false;
            // 
            // filePictureBoxTableRow14Col1
            // 
            this.filePictureBoxTableRow14Col1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow14Col1.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow14Col1.Image")));
            this.filePictureBoxTableRow14Col1.Location = new System.Drawing.Point(74, 209);
            this.filePictureBoxTableRow14Col1.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow14Col1.Name = "filePictureBoxTableRow14Col1";
            this.filePictureBoxTableRow14Col1.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow14Col1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow14Col1.TabIndex = 43;
            this.filePictureBoxTableRow14Col1.TabStop = false;
            // 
            // filePictureBoxTableRow13Col1
            // 
            this.filePictureBoxTableRow13Col1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow13Col1.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow13Col1.Image")));
            this.filePictureBoxTableRow13Col1.Location = new System.Drawing.Point(74, 193);
            this.filePictureBoxTableRow13Col1.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow13Col1.Name = "filePictureBoxTableRow13Col1";
            this.filePictureBoxTableRow13Col1.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow13Col1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow13Col1.TabIndex = 43;
            this.filePictureBoxTableRow13Col1.TabStop = false;
            // 
            // filePictureBoxTableRow12Col1
            // 
            this.filePictureBoxTableRow12Col1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow12Col1.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow12Col1.Image")));
            this.filePictureBoxTableRow12Col1.Location = new System.Drawing.Point(74, 177);
            this.filePictureBoxTableRow12Col1.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow12Col1.Name = "filePictureBoxTableRow12Col1";
            this.filePictureBoxTableRow12Col1.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow12Col1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow12Col1.TabIndex = 43;
            this.filePictureBoxTableRow12Col1.TabStop = false;
            // 
            // filePictureBoxTableRow11Col1
            // 
            this.filePictureBoxTableRow11Col1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow11Col1.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow11Col1.Image")));
            this.filePictureBoxTableRow11Col1.Location = new System.Drawing.Point(74, 161);
            this.filePictureBoxTableRow11Col1.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow11Col1.Name = "filePictureBoxTableRow11Col1";
            this.filePictureBoxTableRow11Col1.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow11Col1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow11Col1.TabIndex = 43;
            this.filePictureBoxTableRow11Col1.TabStop = false;
            // 
            // filePictureBoxTableRow10Col1
            // 
            this.filePictureBoxTableRow10Col1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow10Col1.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow10Col1.Image")));
            this.filePictureBoxTableRow10Col1.Location = new System.Drawing.Point(74, 145);
            this.filePictureBoxTableRow10Col1.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow10Col1.Name = "filePictureBoxTableRow10Col1";
            this.filePictureBoxTableRow10Col1.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow10Col1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow10Col1.TabIndex = 43;
            this.filePictureBoxTableRow10Col1.TabStop = false;
            // 
            // filePictureBoxTableRow9Col1
            // 
            this.filePictureBoxTableRow9Col1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow9Col1.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow9Col1.Image")));
            this.filePictureBoxTableRow9Col1.Location = new System.Drawing.Point(74, 129);
            this.filePictureBoxTableRow9Col1.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow9Col1.Name = "filePictureBoxTableRow9Col1";
            this.filePictureBoxTableRow9Col1.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow9Col1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow9Col1.TabIndex = 43;
            this.filePictureBoxTableRow9Col1.TabStop = false;
            // 
            // filePictureBoxTableRow8Col1
            // 
            this.filePictureBoxTableRow8Col1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow8Col1.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow8Col1.Image")));
            this.filePictureBoxTableRow8Col1.Location = new System.Drawing.Point(74, 113);
            this.filePictureBoxTableRow8Col1.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow8Col1.Name = "filePictureBoxTableRow8Col1";
            this.filePictureBoxTableRow8Col1.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow8Col1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow8Col1.TabIndex = 43;
            this.filePictureBoxTableRow8Col1.TabStop = false;
            // 
            // filePictureBoxTableRow7Col1
            // 
            this.filePictureBoxTableRow7Col1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow7Col1.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow7Col1.Image")));
            this.filePictureBoxTableRow7Col1.Location = new System.Drawing.Point(74, 97);
            this.filePictureBoxTableRow7Col1.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow7Col1.Name = "filePictureBoxTableRow7Col1";
            this.filePictureBoxTableRow7Col1.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow7Col1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow7Col1.TabIndex = 43;
            this.filePictureBoxTableRow7Col1.TabStop = false;
            // 
            // filePictureBoxTableRow6Col1
            // 
            this.filePictureBoxTableRow6Col1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow6Col1.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow6Col1.Image")));
            this.filePictureBoxTableRow6Col1.Location = new System.Drawing.Point(74, 81);
            this.filePictureBoxTableRow6Col1.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow6Col1.Name = "filePictureBoxTableRow6Col1";
            this.filePictureBoxTableRow6Col1.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow6Col1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow6Col1.TabIndex = 43;
            this.filePictureBoxTableRow6Col1.TabStop = false;
            // 
            // filePictureBoxTableRow5Col1
            // 
            this.filePictureBoxTableRow5Col1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow5Col1.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow5Col1.Image")));
            this.filePictureBoxTableRow5Col1.Location = new System.Drawing.Point(74, 65);
            this.filePictureBoxTableRow5Col1.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow5Col1.Name = "filePictureBoxTableRow5Col1";
            this.filePictureBoxTableRow5Col1.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow5Col1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow5Col1.TabIndex = 42;
            this.filePictureBoxTableRow5Col1.TabStop = false;
            // 
            // filePictureBoxTableRow4Col1
            // 
            this.filePictureBoxTableRow4Col1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow4Col1.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow4Col1.Image")));
            this.filePictureBoxTableRow4Col1.Location = new System.Drawing.Point(74, 49);
            this.filePictureBoxTableRow4Col1.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow4Col1.Name = "filePictureBoxTableRow4Col1";
            this.filePictureBoxTableRow4Col1.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow4Col1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow4Col1.TabIndex = 42;
            this.filePictureBoxTableRow4Col1.TabStop = false;
            // 
            // filePictureBoxTableRow3Col1
            // 
            this.filePictureBoxTableRow3Col1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow3Col1.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow3Col1.Image")));
            this.filePictureBoxTableRow3Col1.Location = new System.Drawing.Point(74, 33);
            this.filePictureBoxTableRow3Col1.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow3Col1.Name = "filePictureBoxTableRow3Col1";
            this.filePictureBoxTableRow3Col1.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow3Col1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow3Col1.TabIndex = 42;
            this.filePictureBoxTableRow3Col1.TabStop = false;
            // 
            // filePictureBoxTableRow2Col1
            // 
            this.filePictureBoxTableRow2Col1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow2Col1.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow2Col1.Image")));
            this.filePictureBoxTableRow2Col1.Location = new System.Drawing.Point(74, 17);
            this.filePictureBoxTableRow2Col1.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow2Col1.Name = "filePictureBoxTableRow2Col1";
            this.filePictureBoxTableRow2Col1.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow2Col1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow2Col1.TabIndex = 42;
            this.filePictureBoxTableRow2Col1.TabStop = false;
            // 
            // labelFileTableRow1
            // 
            this.labelFileTableRow1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileTableRow1.AutoSize = true;
            this.labelFileTableRow1.Location = new System.Drawing.Point(4, 1);
            this.labelFileTableRow1.Name = "labelFileTableRow1";
            this.labelFileTableRow1.Size = new System.Drawing.Size(66, 15);
            this.labelFileTableRow1.TabIndex = 7;
            this.labelFileTableRow1.Text = "BoB";
            this.labelFileTableRow1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFileTableRow2
            // 
            this.labelFileTableRow2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileTableRow2.AutoSize = true;
            this.labelFileTableRow2.Location = new System.Drawing.Point(4, 17);
            this.labelFileTableRow2.Name = "labelFileTableRow2";
            this.labelFileTableRow2.Size = new System.Drawing.Size(66, 15);
            this.labelFileTableRow2.TabIndex = 8;
            this.labelFileTableRow2.Text = "WF";
            this.labelFileTableRow2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFileTableRow3
            // 
            this.labelFileTableRow3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileTableRow3.AutoSize = true;
            this.labelFileTableRow3.Location = new System.Drawing.Point(4, 33);
            this.labelFileTableRow3.Name = "labelFileTableRow3";
            this.labelFileTableRow3.Size = new System.Drawing.Size(66, 15);
            this.labelFileTableRow3.TabIndex = 9;
            this.labelFileTableRow3.Text = "JRB";
            this.labelFileTableRow3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFileTableRow4
            // 
            this.labelFileTableRow4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileTableRow4.AutoSize = true;
            this.labelFileTableRow4.Location = new System.Drawing.Point(4, 49);
            this.labelFileTableRow4.Name = "labelFileTableRow4";
            this.labelFileTableRow4.Size = new System.Drawing.Size(66, 15);
            this.labelFileTableRow4.TabIndex = 9;
            this.labelFileTableRow4.Text = "CCM";
            this.labelFileTableRow4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFileTableRow5
            // 
            this.labelFileTableRow5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileTableRow5.AutoSize = true;
            this.labelFileTableRow5.Location = new System.Drawing.Point(4, 65);
            this.labelFileTableRow5.Name = "labelFileTableRow5";
            this.labelFileTableRow5.Size = new System.Drawing.Size(66, 15);
            this.labelFileTableRow5.TabIndex = 9;
            this.labelFileTableRow5.Text = "BBH";
            this.labelFileTableRow5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFileTableRow6
            // 
            this.labelFileTableRow6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileTableRow6.AutoSize = true;
            this.labelFileTableRow6.Location = new System.Drawing.Point(4, 81);
            this.labelFileTableRow6.Name = "labelFileTableRow6";
            this.labelFileTableRow6.Size = new System.Drawing.Size(66, 15);
            this.labelFileTableRow6.TabIndex = 9;
            this.labelFileTableRow6.Text = "HMC";
            this.labelFileTableRow6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFileTableRow7
            // 
            this.labelFileTableRow7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileTableRow7.AutoSize = true;
            this.labelFileTableRow7.Location = new System.Drawing.Point(4, 97);
            this.labelFileTableRow7.Name = "labelFileTableRow7";
            this.labelFileTableRow7.Size = new System.Drawing.Size(66, 15);
            this.labelFileTableRow7.TabIndex = 9;
            this.labelFileTableRow7.Text = "LLL";
            this.labelFileTableRow7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFileTableRow8
            // 
            this.labelFileTableRow8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileTableRow8.AutoSize = true;
            this.labelFileTableRow8.Location = new System.Drawing.Point(4, 113);
            this.labelFileTableRow8.Name = "labelFileTableRow8";
            this.labelFileTableRow8.Size = new System.Drawing.Size(66, 15);
            this.labelFileTableRow8.TabIndex = 9;
            this.labelFileTableRow8.Text = "SSL";
            this.labelFileTableRow8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFileTableRow9
            // 
            this.labelFileTableRow9.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileTableRow9.AutoSize = true;
            this.labelFileTableRow9.Location = new System.Drawing.Point(4, 129);
            this.labelFileTableRow9.Name = "labelFileTableRow9";
            this.labelFileTableRow9.Size = new System.Drawing.Size(66, 15);
            this.labelFileTableRow9.TabIndex = 9;
            this.labelFileTableRow9.Text = "DDD";
            this.labelFileTableRow9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFileTableRow10
            // 
            this.labelFileTableRow10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileTableRow10.AutoSize = true;
            this.labelFileTableRow10.Location = new System.Drawing.Point(4, 145);
            this.labelFileTableRow10.Name = "labelFileTableRow10";
            this.labelFileTableRow10.Size = new System.Drawing.Size(66, 15);
            this.labelFileTableRow10.TabIndex = 9;
            this.labelFileTableRow10.Text = "SL";
            this.labelFileTableRow10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFileTableRow11
            // 
            this.labelFileTableRow11.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileTableRow11.AutoSize = true;
            this.labelFileTableRow11.Location = new System.Drawing.Point(4, 161);
            this.labelFileTableRow11.Name = "labelFileTableRow11";
            this.labelFileTableRow11.Size = new System.Drawing.Size(66, 15);
            this.labelFileTableRow11.TabIndex = 9;
            this.labelFileTableRow11.Text = "WDW";
            this.labelFileTableRow11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFileTableRow12
            // 
            this.labelFileTableRow12.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileTableRow12.AutoSize = true;
            this.labelFileTableRow12.Location = new System.Drawing.Point(4, 177);
            this.labelFileTableRow12.Name = "labelFileTableRow12";
            this.labelFileTableRow12.Size = new System.Drawing.Size(66, 15);
            this.labelFileTableRow12.TabIndex = 9;
            this.labelFileTableRow12.Text = "TTM";
            this.labelFileTableRow12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFileTableRow13
            // 
            this.labelFileTableRow13.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileTableRow13.AutoSize = true;
            this.labelFileTableRow13.Location = new System.Drawing.Point(4, 193);
            this.labelFileTableRow13.Name = "labelFileTableRow13";
            this.labelFileTableRow13.Size = new System.Drawing.Size(66, 15);
            this.labelFileTableRow13.TabIndex = 9;
            this.labelFileTableRow13.Text = "THI";
            this.labelFileTableRow13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFileTableRow14
            // 
            this.labelFileTableRow14.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileTableRow14.AutoSize = true;
            this.labelFileTableRow14.Location = new System.Drawing.Point(4, 209);
            this.labelFileTableRow14.Name = "labelFileTableRow14";
            this.labelFileTableRow14.Size = new System.Drawing.Size(66, 15);
            this.labelFileTableRow14.TabIndex = 9;
            this.labelFileTableRow14.Text = "TTC";
            this.labelFileTableRow14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFileTableRow15
            // 
            this.labelFileTableRow15.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileTableRow15.AutoSize = true;
            this.labelFileTableRow15.Location = new System.Drawing.Point(4, 225);
            this.labelFileTableRow15.Name = "labelFileTableRow15";
            this.labelFileTableRow15.Size = new System.Drawing.Size(66, 15);
            this.labelFileTableRow15.TabIndex = 9;
            this.labelFileTableRow15.Text = "RR";
            this.labelFileTableRow15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFileTableRow16
            // 
            this.labelFileTableRow16.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileTableRow16.AutoSize = true;
            this.labelFileTableRow16.Location = new System.Drawing.Point(4, 241);
            this.labelFileTableRow16.Name = "labelFileTableRow16";
            this.labelFileTableRow16.Size = new System.Drawing.Size(66, 15);
            this.labelFileTableRow16.TabIndex = 9;
            this.labelFileTableRow16.Text = "TotWC";
            this.labelFileTableRow16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFileTableRow17
            // 
            this.labelFileTableRow17.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileTableRow17.AutoSize = true;
            this.labelFileTableRow17.Location = new System.Drawing.Point(4, 257);
            this.labelFileTableRow17.Name = "labelFileTableRow17";
            this.labelFileTableRow17.Size = new System.Drawing.Size(66, 15);
            this.labelFileTableRow17.TabIndex = 9;
            this.labelFileTableRow17.Text = "CotMC";
            this.labelFileTableRow17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFileTableRow18
            // 
            this.labelFileTableRow18.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileTableRow18.AutoSize = true;
            this.labelFileTableRow18.Location = new System.Drawing.Point(4, 273);
            this.labelFileTableRow18.Name = "labelFileTableRow18";
            this.labelFileTableRow18.Size = new System.Drawing.Size(66, 15);
            this.labelFileTableRow18.TabIndex = 9;
            this.labelFileTableRow18.Text = "VCutM";
            this.labelFileTableRow18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFileTableRow19
            // 
            this.labelFileTableRow19.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileTableRow19.AutoSize = true;
            this.labelFileTableRow19.Location = new System.Drawing.Point(4, 289);
            this.labelFileTableRow19.Name = "labelFileTableRow19";
            this.labelFileTableRow19.Size = new System.Drawing.Size(66, 15);
            this.labelFileTableRow19.TabIndex = 9;
            this.labelFileTableRow19.Text = "PSS";
            this.labelFileTableRow19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFileTableRow20
            // 
            this.labelFileTableRow20.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileTableRow20.AutoSize = true;
            this.labelFileTableRow20.Location = new System.Drawing.Point(4, 305);
            this.labelFileTableRow20.Name = "labelFileTableRow20";
            this.labelFileTableRow20.Size = new System.Drawing.Size(66, 15);
            this.labelFileTableRow20.TabIndex = 9;
            this.labelFileTableRow20.Text = "SA";
            this.labelFileTableRow20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFileTableRow21
            // 
            this.labelFileTableRow21.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileTableRow21.AutoSize = true;
            this.labelFileTableRow21.Location = new System.Drawing.Point(4, 321);
            this.labelFileTableRow21.Name = "labelFileTableRow21";
            this.labelFileTableRow21.Size = new System.Drawing.Size(66, 15);
            this.labelFileTableRow21.TabIndex = 9;
            this.labelFileTableRow21.Text = "WMotR";
            this.labelFileTableRow21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFileTableRow22
            // 
            this.labelFileTableRow22.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileTableRow22.AutoSize = true;
            this.labelFileTableRow22.Location = new System.Drawing.Point(4, 337);
            this.labelFileTableRow22.Name = "labelFileTableRow22";
            this.labelFileTableRow22.Size = new System.Drawing.Size(66, 15);
            this.labelFileTableRow22.TabIndex = 9;
            this.labelFileTableRow22.Text = "BitDW";
            this.labelFileTableRow22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFileTableRow23
            // 
            this.labelFileTableRow23.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileTableRow23.AutoSize = true;
            this.labelFileTableRow23.Location = new System.Drawing.Point(4, 353);
            this.labelFileTableRow23.Name = "labelFileTableRow23";
            this.labelFileTableRow23.Size = new System.Drawing.Size(66, 15);
            this.labelFileTableRow23.TabIndex = 9;
            this.labelFileTableRow23.Text = "BitFS";
            this.labelFileTableRow23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFileTableRow24
            // 
            this.labelFileTableRow24.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileTableRow24.AutoSize = true;
            this.labelFileTableRow24.Location = new System.Drawing.Point(4, 369);
            this.labelFileTableRow24.Name = "labelFileTableRow24";
            this.labelFileTableRow24.Size = new System.Drawing.Size(66, 15);
            this.labelFileTableRow24.TabIndex = 9;
            this.labelFileTableRow24.Text = "BitS";
            this.labelFileTableRow24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFileTableRow25
            // 
            this.labelFileTableRow25.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileTableRow25.AutoSize = true;
            this.labelFileTableRow25.Location = new System.Drawing.Point(4, 385);
            this.labelFileTableRow25.Name = "labelFileTableRow25";
            this.labelFileTableRow25.Size = new System.Drawing.Size(66, 15);
            this.labelFileTableRow25.TabIndex = 9;
            this.labelFileTableRow25.Text = "Toad";
            this.labelFileTableRow25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFileTableRow26
            // 
            this.labelFileTableRow26.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileTableRow26.AutoSize = true;
            this.labelFileTableRow26.Location = new System.Drawing.Point(4, 401);
            this.labelFileTableRow26.Name = "labelFileTableRow26";
            this.labelFileTableRow26.Size = new System.Drawing.Size(66, 17);
            this.labelFileTableRow26.TabIndex = 9;
            this.labelFileTableRow26.Text = "MIPS";
            this.labelFileTableRow26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // filePictureBoxTableRow1Col1
            // 
            this.filePictureBoxTableRow1Col1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow1Col1.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow1Col1.Image")));
            this.filePictureBoxTableRow1Col1.Location = new System.Drawing.Point(74, 1);
            this.filePictureBoxTableRow1Col1.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow1Col1.Name = "filePictureBoxTableRow1Col1";
            this.filePictureBoxTableRow1Col1.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow1Col1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow1Col1.TabIndex = 41;
            this.filePictureBoxTableRow1Col1.TabStop = false;
            // 
            // filePictureBoxTableRow8Col8
            // 
            this.filePictureBoxTableRow8Col8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow8Col8.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow8Col8.Image")));
            this.filePictureBoxTableRow8Col8.Location = new System.Drawing.Point(277, 113);
            this.filePictureBoxTableRow8Col8.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow8Col8.Name = "filePictureBoxTableRow8Col8";
            this.filePictureBoxTableRow8Col8.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow8Col8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow8Col8.TabIndex = 49;
            this.filePictureBoxTableRow8Col8.TabStop = false;
            // 
            // filePictureBoxTableRow19Col9
            // 
            this.filePictureBoxTableRow19Col9.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePictureBoxTableRow19Col9.Image = ((System.Drawing.Image)(resources.GetObject("filePictureBoxTableRow19Col9.Image")));
            this.filePictureBoxTableRow19Col9.Location = new System.Drawing.Point(306, 289);
            this.filePictureBoxTableRow19Col9.Margin = new System.Windows.Forms.Padding(0);
            this.filePictureBoxTableRow19Col9.Name = "filePictureBoxTableRow19Col9";
            this.filePictureBoxTableRow19Col9.Size = new System.Drawing.Size(28, 15);
            this.filePictureBoxTableRow19Col9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.filePictureBoxTableRow19Col9.TabIndex = 49;
            this.filePictureBoxTableRow19Col9.TabStop = false;
            // 
            // noTearFlowLayoutPanelFile
            // 
            this.noTearFlowLayoutPanelFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.noTearFlowLayoutPanelFile.AutoScroll = true;
            this.noTearFlowLayoutPanelFile.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.noTearFlowLayoutPanelFile.Location = new System.Drawing.Point(0, -1);
            this.noTearFlowLayoutPanelFile.Margin = new System.Windows.Forms.Padding(2);
            this.noTearFlowLayoutPanelFile.Name = "noTearFlowLayoutPanelFile";
            this.noTearFlowLayoutPanelFile.Size = new System.Drawing.Size(237, 463);
            this.noTearFlowLayoutPanelFile.TabIndex = 2;
            // 
            // pictureBoxMisc
            // 
            this.pictureBoxMisc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxMisc.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            this.pictureBoxMisc.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxMisc.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBoxMisc.MaximumSize = new System.Drawing.Size(133, 130);
            this.pictureBoxMisc.Name = "pictureBoxMisc";
            this.pictureBoxMisc.Size = new System.Drawing.Size(51, 50);
            this.pictureBoxMisc.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxMisc.TabIndex = 0;
            this.pictureBoxMisc.TabStop = false;
            // 
            // NoTearFlowLayoutPanelMisc
            // 
            this.NoTearFlowLayoutPanelMisc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NoTearFlowLayoutPanelMisc.AutoScroll = true;
            this.NoTearFlowLayoutPanelMisc.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.NoTearFlowLayoutPanelMisc.Location = new System.Drawing.Point(64, 5);
            this.NoTearFlowLayoutPanelMisc.Margin = new System.Windows.Forms.Padding(2);
            this.NoTearFlowLayoutPanelMisc.Name = "NoTearFlowLayoutPanelMisc";
            this.NoTearFlowLayoutPanelMisc.Size = new System.Drawing.Size(849, 456);
            this.NoTearFlowLayoutPanelMisc.TabIndex = 5;
            // 
            // pictureBoxDebug
            // 
            this.pictureBoxDebug.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxDebug.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            this.pictureBoxDebug.Location = new System.Drawing.Point(2, 2);
            this.pictureBoxDebug.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBoxDebug.MaximumSize = new System.Drawing.Size(133, 130);
            this.pictureBoxDebug.Name = "pictureBoxDebug";
            this.pictureBoxDebug.Size = new System.Drawing.Size(51, 50);
            this.pictureBoxDebug.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxDebug.TabIndex = 0;
            this.pictureBoxDebug.TabStop = false;
            // 
            // NoTearFlowLayoutPanelDebugDisplayType
            // 
            this.NoTearFlowLayoutPanelDebugDisplayType.AutoScroll = true;
            this.NoTearFlowLayoutPanelDebugDisplayType.Controls.Add(this.radioButtonDbgOff);
            this.NoTearFlowLayoutPanelDebugDisplayType.Controls.Add(this.radioButtonDbgObjCnt);
            this.NoTearFlowLayoutPanelDebugDisplayType.Controls.Add(this.radioButtonDbgChkInfo);
            this.NoTearFlowLayoutPanelDebugDisplayType.Controls.Add(this.radioButtonDbgMapInfo);
            this.NoTearFlowLayoutPanelDebugDisplayType.Controls.Add(this.radioButtonDbgStgInfo);
            this.NoTearFlowLayoutPanelDebugDisplayType.Controls.Add(this.radioButtonDbgFxInfo);
            this.NoTearFlowLayoutPanelDebugDisplayType.Controls.Add(this.radioButtonDbgEnemyInfo);
            this.NoTearFlowLayoutPanelDebugDisplayType.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.NoTearFlowLayoutPanelDebugDisplayType.Location = new System.Drawing.Point(71, 23);
            this.NoTearFlowLayoutPanelDebugDisplayType.Name = "NoTearFlowLayoutPanelDebugDisplayType";
            this.NoTearFlowLayoutPanelDebugDisplayType.Size = new System.Drawing.Size(167, 177);
            this.NoTearFlowLayoutPanelDebugDisplayType.TabIndex = 11;
            this.NoTearFlowLayoutPanelDebugDisplayType.WrapContents = false;
            // 
            // radioButtonDbgOff
            // 
            this.radioButtonDbgOff.AutoSize = true;
            this.radioButtonDbgOff.Checked = true;
            this.radioButtonDbgOff.Location = new System.Drawing.Point(3, 3);
            this.radioButtonDbgOff.Name = "radioButtonDbgOff";
            this.radioButtonDbgOff.Size = new System.Drawing.Size(39, 17);
            this.radioButtonDbgOff.TabIndex = 1;
            this.radioButtonDbgOff.TabStop = true;
            this.radioButtonDbgOff.Text = "Off";
            this.radioButtonDbgOff.UseVisualStyleBackColor = true;
            // 
            // radioButtonDbgObjCnt
            // 
            this.radioButtonDbgObjCnt.AutoSize = true;
            this.radioButtonDbgObjCnt.Location = new System.Drawing.Point(3, 26);
            this.radioButtonDbgObjCnt.Name = "radioButtonDbgObjCnt";
            this.radioButtonDbgObjCnt.Size = new System.Drawing.Size(96, 17);
            this.radioButtonDbgObjCnt.TabIndex = 5;
            this.radioButtonDbgObjCnt.Text = "Object Counter";
            this.radioButtonDbgObjCnt.UseVisualStyleBackColor = true;
            // 
            // radioButtonDbgChkInfo
            // 
            this.radioButtonDbgChkInfo.AutoSize = true;
            this.radioButtonDbgChkInfo.Location = new System.Drawing.Point(3, 49);
            this.radioButtonDbgChkInfo.Name = "radioButtonDbgChkInfo";
            this.radioButtonDbgChkInfo.Size = new System.Drawing.Size(77, 17);
            this.radioButtonDbgChkInfo.TabIndex = 6;
            this.radioButtonDbgChkInfo.Text = "Check Info";
            this.radioButtonDbgChkInfo.UseVisualStyleBackColor = true;
            // 
            // radioButtonDbgMapInfo
            // 
            this.radioButtonDbgMapInfo.AutoSize = true;
            this.radioButtonDbgMapInfo.Location = new System.Drawing.Point(3, 72);
            this.radioButtonDbgMapInfo.Name = "radioButtonDbgMapInfo";
            this.radioButtonDbgMapInfo.Size = new System.Drawing.Size(67, 17);
            this.radioButtonDbgMapInfo.TabIndex = 7;
            this.radioButtonDbgMapInfo.Text = "Map Info";
            this.radioButtonDbgMapInfo.UseVisualStyleBackColor = true;
            // 
            // radioButtonDbgStgInfo
            // 
            this.radioButtonDbgStgInfo.AutoSize = true;
            this.radioButtonDbgStgInfo.Location = new System.Drawing.Point(3, 95);
            this.radioButtonDbgStgInfo.Name = "radioButtonDbgStgInfo";
            this.radioButtonDbgStgInfo.Size = new System.Drawing.Size(74, 17);
            this.radioButtonDbgStgInfo.TabIndex = 8;
            this.radioButtonDbgStgInfo.Text = "Stage Info";
            this.radioButtonDbgStgInfo.UseVisualStyleBackColor = true;
            // 
            // radioButtonDbgFxInfo
            // 
            this.radioButtonDbgFxInfo.AutoSize = true;
            this.radioButtonDbgFxInfo.Location = new System.Drawing.Point(3, 118);
            this.radioButtonDbgFxInfo.Name = "radioButtonDbgFxInfo";
            this.radioButtonDbgFxInfo.Size = new System.Drawing.Size(74, 17);
            this.radioButtonDbgFxInfo.TabIndex = 9;
            this.radioButtonDbgFxInfo.Text = "Effect Info";
            this.radioButtonDbgFxInfo.UseVisualStyleBackColor = true;
            // 
            // radioButtonDbgEnemyInfo
            // 
            this.radioButtonDbgEnemyInfo.AutoSize = true;
            this.radioButtonDbgEnemyInfo.Location = new System.Drawing.Point(3, 141);
            this.radioButtonDbgEnemyInfo.Name = "radioButtonDbgEnemyInfo";
            this.radioButtonDbgEnemyInfo.Size = new System.Drawing.Size(78, 17);
            this.radioButtonDbgEnemyInfo.TabIndex = 10;
            this.radioButtonDbgEnemyInfo.Text = "Enemy Info";
            this.radioButtonDbgEnemyInfo.UseVisualStyleBackColor = true;
            // 
            // noTearFlowLayoutPanelCamHack
            // 
            this.noTearFlowLayoutPanelCamHack.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.noTearFlowLayoutPanelCamHack.AutoScroll = true;
            this.noTearFlowLayoutPanelCamHack.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.noTearFlowLayoutPanelCamHack.Location = new System.Drawing.Point(2, 2);
            this.noTearFlowLayoutPanelCamHack.Margin = new System.Windows.Forms.Padding(2);
            this.noTearFlowLayoutPanelCamHack.Name = "noTearFlowLayoutPanelCamHack";
            this.noTearFlowLayoutPanelCamHack.Size = new System.Drawing.Size(538, 458);
            this.noTearFlowLayoutPanelCamHack.TabIndex = 2;
            // 
            // noTearFlowLayoutPanelQuarterFrame
            // 
            this.noTearFlowLayoutPanelQuarterFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.noTearFlowLayoutPanelQuarterFrame.AutoScroll = true;
            this.noTearFlowLayoutPanelQuarterFrame.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.noTearFlowLayoutPanelQuarterFrame.Location = new System.Drawing.Point(5, 5);
            this.noTearFlowLayoutPanelQuarterFrame.Margin = new System.Windows.Forms.Padding(2);
            this.noTearFlowLayoutPanelQuarterFrame.Name = "noTearFlowLayoutPanelQuarterFrame";
            this.noTearFlowLayoutPanelQuarterFrame.Size = new System.Drawing.Size(905, 453);
            this.noTearFlowLayoutPanelQuarterFrame.TabIndex = 2;
            // 
            // NoTearFlowLayoutPanelObjects
            // 
            this.NoTearFlowLayoutPanelObjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NoTearFlowLayoutPanelObjects.AutoScroll = true;
            this.NoTearFlowLayoutPanelObjects.Location = new System.Drawing.Point(4, 45);
            this.NoTearFlowLayoutPanelObjects.Margin = new System.Windows.Forms.Padding(2);
            this.NoTearFlowLayoutPanelObjects.Name = "NoTearFlowLayoutPanelObjects";
            this.NoTearFlowLayoutPanelObjects.Size = new System.Drawing.Size(915, 184);
            this.NoTearFlowLayoutPanelObjects.TabIndex = 0;
            this.NoTearFlowLayoutPanelObjects.Resize += new System.EventHandler(this.NoTearFlowLayoutPanelObjects_Resize);
            // 
            // buttonFileErase
            // 
            this.buttonFileErase.Location = new System.Drawing.Point(33, 163);
            this.buttonFileErase.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFileErase.Name = "buttonFileErase";
            this.buttonFileErase.Size = new System.Drawing.Size(61, 25);
            this.buttonFileErase.TabIndex = 41;
            this.buttonFileErase.Text = "Erase";
            this.buttonFileErase.UseVisualStyleBackColor = true;
            // 
            // StroopMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(947, 741);
            this.Controls.Add(this.panelConnect);
            this.Controls.Add(this.buttonReadOnly);
            this.Controls.Add(this.buttonDisconnect);
            this.Controls.Add(this.labelVersionNumber);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.labelProcessSelect);
            this.Controls.Add(this.labelFpsCounter);
            this.Controls.Add(this.buttonShowLeftPane);
            this.Controls.Add(this.buttonShowLeftRightPane);
            this.Controls.Add(this.buttonShowBottomPane);
            this.Controls.Add(this.buttonShowRightPane);
            this.Controls.Add(this.buttonShowTopBottomPane);
            this.Controls.Add(this.buttonShowTopPane);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "StroopMainForm";
            this.Text = "STROOP";
            this.Load += new System.EventHandler(this.StroopMainForm_Load);
            this.Resize += new System.EventHandler(this.StroopMainForm_Resize);
            this.groupBoxObjects.ResumeLayout(false);
            this.groupBoxObjects.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarObjSlotSize)).EndInit();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageObjects.ResumeLayout(false);
            this.splitContainerObject.Panel1.ResumeLayout(false);
            this.splitContainerObject.Panel1.PerformLayout();
            this.splitContainerObject.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerObject)).EndInit();
            this.splitContainerObject.ResumeLayout(false);
            this.panelObj.ResumeLayout(false);
            this.groupBoxObjHome.ResumeLayout(false);
            this.groupBoxObjHome.PerformLayout();
            this.groupBoxObjScale.ResumeLayout(false);
            this.groupBoxObjScale.PerformLayout();
            this.groupBoxObjAngle.ResumeLayout(false);
            this.groupBoxObjAngle.PerformLayout();
            this.groupBoxObjPos.ResumeLayout(false);
            this.groupBoxObjPos.PerformLayout();
            this.panelObjectBorder.ResumeLayout(false);
            this.tabPageMario.ResumeLayout(false);
            this.splitContainerMario.Panel1.ResumeLayout(false);
            this.splitContainerMario.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMario)).EndInit();
            this.splitContainerMario.ResumeLayout(false);
            this.groupBoxMarioStats.ResumeLayout(false);
            this.groupBoxMarioStats.PerformLayout();
            this.groupBoxMarioHOLP.ResumeLayout(false);
            this.groupBoxMarioHOLP.PerformLayout();
            this.panelMarioBorder.ResumeLayout(false);
            this.groupBoxMarioPos.ResumeLayout(false);
            this.groupBoxMarioPos.PerformLayout();
            this.tabPageActions.ResumeLayout(false);
            this.tabPageHud.ResumeLayout(false);
            this.splitContainerHud.Panel1.ResumeLayout(false);
            this.splitContainerHud.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerHud)).EndInit();
            this.splitContainerHud.ResumeLayout(false);
            this.panelHudBorder.ResumeLayout(false);
            this.tabPageCamera.ResumeLayout(false);
            this.SplitContainerCamera.Panel1.ResumeLayout(false);
            this.SplitContainerCamera.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainerCamera)).EndInit();
            this.SplitContainerCamera.ResumeLayout(false);
            this.groupBoxCameraSphericalPos.ResumeLayout(false);
            this.groupBoxCameraSphericalPos.PerformLayout();
            this.panelCameraBorder.ResumeLayout(false);
            this.groupBoxCameraPos.ResumeLayout(false);
            this.groupBoxCameraPos.PerformLayout();
            this.tabPageTriangles.ResumeLayout(false);
            this.splitContainerTriangles.Panel1.ResumeLayout(false);
            this.splitContainerTriangles.Panel1.PerformLayout();
            this.splitContainerTriangles.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTriangles)).EndInit();
            this.splitContainerTriangles.ResumeLayout(false);
            this.groupBoxTrianglePos.ResumeLayout(false);
            this.groupBoxTrianglePos.PerformLayout();
            this.groupBoxTriangleNormal.ResumeLayout(false);
            this.groupBoxTriangleNormal.PerformLayout();
            this.tabPageWater.ResumeLayout(false);
            this.tabPageController.ResumeLayout(false);
            this.splitContainerController.Panel1.ResumeLayout(false);
            this.splitContainerController.Panel1.PerformLayout();
            this.splitContainerController.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerController)).EndInit();
            this.splitContainerController.ResumeLayout(false);
            this.tabPageFile.ResumeLayout(false);
            this.splitContainerFile.Panel1.ResumeLayout(false);
            this.splitContainerFile.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerFile)).EndInit();
            this.splitContainerFile.ResumeLayout(false);
            this.tableLayoutPanelFile.ResumeLayout(false);
            this.tableLayoutPanelFile.PerformLayout();
            this.groupBoxHatLocation.ResumeLayout(false);
            this.groupBoxHatLocation.PerformLayout();
            this.groupBoxFile.ResumeLayout(false);
            this.groupBoxFile.PerformLayout();
            this.tabPageMisc.ResumeLayout(false);
            this.panelMiscBorder.ResumeLayout(false);
            this.tabPageDebug.ResumeLayout(false);
            this.tabPageDebug.PerformLayout();
            this.panelDebugBorder.ResumeLayout(false);
            this.tabPageMap.ResumeLayout(false);
            this.splitContainerMap.Panel1.ResumeLayout(false);
            this.splitContainerMap.Panel1.PerformLayout();
            this.splitContainerMap.Panel2.ResumeLayout(false);
            this.splitContainerMap.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMap)).EndInit();
            this.splitContainerMap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMapIconSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMapZoom)).EndInit();
            this.tabPagePu.ResumeLayout(false);
            this.groupBoxPuController.ResumeLayout(false);
            this.groupBoxPuController.PerformLayout();
            this.tabPageExpressions.ResumeLayout(false);
            this.tabPageExpressions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExpressions)).EndInit();
            this.tabPageDisassembly.ResumeLayout(false);
            this.tabPageDisassembly.PerformLayout();
            this.tabPageHacks.ResumeLayout(false);
            this.splitContainerHacks.Panel1.ResumeLayout(false);
            this.splitContainerHacks.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerHacks)).EndInit();
            this.splitContainerHacks.ResumeLayout(false);
            this.groupBoxHackRam.ResumeLayout(false);
            this.groupBoxHackRam.PerformLayout();
            this.groupBoxHackSpawn.ResumeLayout(false);
            this.groupBoxHackSpawn.PerformLayout();
            this.tabPageCamHack.ResumeLayout(false);
            this.splitContainerCamHack.Panel1.ResumeLayout(false);
            this.splitContainerCamHack.Panel1.PerformLayout();
            this.splitContainerCamHack.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerCamHack)).EndInit();
            this.splitContainerCamHack.ResumeLayout(false);
            this.groupBoxCameraHackSphericalFocusPos.ResumeLayout(false);
            this.groupBoxCameraHackSphericalFocusPos.PerformLayout();
            this.groupBoxCameraHackFocusPos.ResumeLayout(false);
            this.groupBoxCameraHackFocusPos.PerformLayout();
            this.groupBoxCameraHackSphericalPos.ResumeLayout(false);
            this.groupBoxCameraHackSphericalPos.PerformLayout();
            this.groupBoxCameraHackPos.ResumeLayout(false);
            this.groupBoxCameraHackPos.PerformLayout();
            this.tabPageQuarterFrame.ResumeLayout(false);
            this.tabPageOptions.ResumeLayout(false);
            this.tabPageOptions.PerformLayout();
            this.groupBoxGotoRetrieveOffsets.ResumeLayout(false);
            this.groupBoxGotoRetrieveOffsets.PerformLayout();
            this.groupBoxShowOverlay.ResumeLayout(false);
            this.groupBoxShowOverlay.PerformLayout();
            this.panelConnect.ResumeLayout(false);
            this.panelConnect.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxObject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMario)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCamera)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow24Col9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow23Col9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow22Col9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow4Col9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow3Col9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow2Col9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow21Col8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow15Col8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow13Col8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow12Col8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow11Col8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow10Col8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow4Col8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow3Col8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow2Col8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow1Col8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow19Col2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow26Col2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow25Col3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow25Col2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow26Col1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow25Col1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow24Col1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow23Col1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow22Col1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow21Col1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow20Col1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow19Col1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow18Col1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow17Col1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow16Col1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow15Col7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow14Col7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow13Col7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow12Col7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow11Col7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow10Col7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow9Col7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow8Col7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow7Col7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow6Col7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow5Col7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow4Col7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow3Col7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow2Col7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow1Col7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow15Col6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow14Col6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow13Col6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow12Col6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow11Col6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow10Col6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow9Col6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow8Col6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow7Col6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow6Col6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow5Col6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow4Col6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow3Col6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow2Col6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow1Col6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow15Col5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow14Col5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow13Col5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow12Col5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow11Col5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow10Col5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow9Col5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow8Col5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow7Col5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow6Col5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow5Col5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow4Col5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow3Col5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow2Col5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow1Col5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow15Col4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow14Col4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow13Col4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow12Col4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow11Col4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow10Col4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow9Col4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow8Col4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow7Col4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow6Col4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow5Col4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow4Col4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow3Col4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow2Col4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow1Col4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow15Col3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow14Col3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow13Col3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow12Col3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow11Col3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow10Col3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow9Col3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow8Col3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow7Col3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow6Col3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow5Col3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow4Col3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow3Col3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow2Col3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow1Col3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow15Col2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow14Col2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow13Col2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow12Col2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow11Col2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow10Col2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow9Col2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow8Col2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow7Col2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow6Col2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow5Col2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow4Col2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow3Col2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow2Col2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow1Col2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow15Col1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow14Col1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow13Col1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow12Col1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow11Col1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow10Col1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow9Col1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow8Col1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow7Col1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow6Col1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow5Col1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow4Col1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow3Col1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow2Col1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow1Col1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow8Col8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePictureBoxTableRow19Col9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMisc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDebug)).EndInit();
            this.NoTearFlowLayoutPanelDebugDisplayType.ResumeLayout(false);
            this.NoTearFlowLayoutPanelDebugDisplayType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelProcessSelect;
        private System.Windows.Forms.GroupBox groupBoxObjects;
        private System.Windows.Forms.ComboBox comboBoxSortMethod;
        private System.Windows.Forms.Label labelSortMethod;
        private NoTearFlowLayoutPanel NoTearFlowLayoutPanelObjects;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.CheckBox checkBoxObjLockLabels;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageObjects;
        private System.Windows.Forms.Label labelObjSlotIndValue;
        private System.Windows.Forms.Label labelObjSlotPosValue;
        private System.Windows.Forms.Label labelObjBhvValue;
        private System.Windows.Forms.Label labelObjAdd;
        private NoTearFlowLayoutPanel NoTearFlowLayoutPanelObject;
        private System.Windows.Forms.Label labelObjSlotInd;
        private System.Windows.Forms.Label labelObjSlotPos;
        private System.Windows.Forms.Label labelObjBhv;
        private System.Windows.Forms.Panel panelObjectBorder;
        private IntPictureBox pictureBoxObject;
        private System.Windows.Forms.Label labelObjAddValue;
        private System.Windows.Forms.TabPage tabPageMario;
        private NoTearFlowLayoutPanel NoTearFlowLayoutPanelMario;
        private System.Windows.Forms.Panel panelMarioBorder;
        private IntPictureBox pictureBoxMario;
        private System.Windows.Forms.TabPage tabPageExpressions;
        private System.Windows.Forms.CheckBox checkBoxAbsoluteAddress;
        private System.Windows.Forms.Button buttonOtherDelete;
        private System.Windows.Forms.Button buttonOtherModify;
        private System.Windows.Forms.Button buttonOtherAdd;
        private System.Windows.Forms.DataGridView dataGridViewExpressions;
        private System.Windows.Forms.TabPage tabPageDisassembly;
        private System.Windows.Forms.Button buttonDisGo;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxDisStart;
        private System.Windows.Forms.Label labelDisStart;
        private System.Windows.Forms.RichTextBox richTextBoxDissasembly;
        private System.Windows.Forms.TabPage tabPageMap;
        private System.Windows.Forms.TabPage tabPageOptions;
        private System.Windows.Forms.CheckBox checkBoxStartSlotIndexOne;
        private OpenTK.GLControl glControlMap;
        private System.Windows.Forms.Button buttonObjRetrieve;
        private System.Windows.Forms.Button buttonObjClone;
        private System.Windows.Forms.Button buttonObjGoTo;
        private System.Windows.Forms.Button buttonObjUnload;
        private System.Windows.Forms.Label labelMapPuValue;
        private System.Windows.Forms.Label labelMapSubName;
        private System.Windows.Forms.Label labelMapQpu;
        private System.Windows.Forms.Label labelMapPu;
        private System.Windows.Forms.Label labelMapId;
        private System.Windows.Forms.Label labelMapName;
        private System.Windows.Forms.Label labelMapQpuValue;
        private System.Windows.Forms.Label labelVersionNumber;
        private System.Windows.Forms.TrackBar trackBarMapIconSize;
        private System.Windows.Forms.TrackBar trackBarMapZoom;
        private System.Windows.Forms.Label labelMapIconSize;
        private System.Windows.Forms.Label labelMapZoom;
        private System.Windows.Forms.CheckBox checkBoxMapShowInactive;
        private System.Windows.Forms.CheckBox checkBoxMapShowMario;
        private System.Windows.Forms.Label labelToggleMode;
        private System.Windows.Forms.ComboBox comboBoxMapToggleMode;
        private System.Windows.Forms.TextBox textBoxObjName;
        private System.Windows.Forms.SplitContainer splitContainerMap;
        private System.Windows.Forms.CheckBox checkBoxMapShowHolp;
        private System.Windows.Forms.CheckBox checkBoxUseRomHack;
        private System.Windows.Forms.TabPage tabPageHud;
        private System.Windows.Forms.TabPage tabPageCamera;
        private NoTearFlowLayoutPanel NoTearFlowLayoutPanelHud;
        private System.Windows.Forms.Panel panelHudBorder;
        private IntPictureBox pictureBoxHud;
        private NoTearFlowLayoutPanel NoTearFlowLayoutPanelCamera;
        private System.Windows.Forms.Panel panelCameraBorder;
        private IntPictureBox pictureBoxCamera;
        private System.Windows.Forms.CheckBox checkBoxMapShowCamera;
        private System.Windows.Forms.TrackBar trackBarObjSlotSize;
        private System.Windows.Forms.TabPage tabPageDebug;
        private System.Windows.Forms.RadioButton radioButtonDbgOff;
        private System.Windows.Forms.TabPage tabPageMisc;
        private System.Windows.Forms.RadioButton radioButtonDbgEnemyInfo;
        private System.Windows.Forms.RadioButton radioButtonDbgFxInfo;
        private System.Windows.Forms.RadioButton radioButtonDbgStgInfo;
        private System.Windows.Forms.RadioButton radioButtonDbgMapInfo;
        private System.Windows.Forms.RadioButton radioButtonDbgChkInfo;
        private System.Windows.Forms.RadioButton radioButtonDbgObjCnt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelDebugBorder;
        private IntPictureBox pictureBoxDebug;
        private NoTearFlowLayoutPanel NoTearFlowLayoutPanelMisc;
        private System.Windows.Forms.Panel panelMiscBorder;
        private IntPictureBox pictureBoxMisc;
        private System.Windows.Forms.TabPage tabPageFile;
        private System.Windows.Forms.Label labelSlotSize;
        private System.Windows.Forms.CheckBox checkBoxMapShowFloor;
        private CheckBox checkBoxMoveCamWithPu;
        private Label label3;
        private TabPage tabPageTriangles;
        private RadioButton radioButtonTriCeiling;
        private RadioButton radioButtonTriWall;
        private RadioButton radioButtonTriFloor;
        private Label labelTriangleSelection;
        private MaskedTextBox maskedTextBoxOtherTriangle;
        private RadioButton radioButtonTriOther;
        private NoTearFlowLayoutPanel NoTearFlowLayoutPanelTriangles;
        private Button buttonStandardHud;
        private Button buttonDie;
        private Button buttonFillHp;
        private TabPage tabPageHacks;
        private CheckedListBox checkedListBoxHacks;
        private Label labelPureInterpretterRequire;
        private NoTearFlowLayoutPanel NoTearFlowLayoutPanelDebugDisplayType;
        private Button buttonGoToV3;
        private Button buttonGoToV2;
        private Button buttonGoToV1;
        private Button buttonRetrieveTriangle;
        private CheckBox checkBoxVertexMisalignment;
        private ComboBox comboBoxLabelMethod;
        private Label labelLabelMethod;
        private Button buttonDisconnect;
        private Panel panelConnect;
        private Button buttonRefresh;
        private Label labelNotConnected;
        private Button buttonConnect;
        private ListBox listBoxProcessesList;
        private Button buttonGoToVClosest;
        private Button buttonObjRetrieveHome;
        private Button buttonObjGoToHome;
        private Label labelFpsCounter;
        private Button buttonShowTopPane;
        private Button buttonShowTopBottomPane;
        private SplitContainer splitContainerHacks;
        private GroupBox groupBoxHackRam;
        private GroupBox groupBoxHackSpawn;
        private ListBox listBoxSpawn;
        private Button buttonHackSpawn;
        private Label labelSpawnExtra;
        private Label labelSpawnGfxId;
        private TextBox textBoxSpawnExtra;
        private TextBox textBoxSpawnGfxId;
        private Label labelSpawnHint;
        private Button buttonSpawnReset;
        private Button buttonReadOnly;
        private Button buttonDisMore;
        private Button buttonMarioToggleHandsfree;
        private Button buttonMarioVisibility;
        private CheckBox checkBoxDbgStageSelect;
        private CheckBox checkBoxDbgClassicDbg;
        private Button buttonDbgFreeMovement;
        private CheckBox checkBoxDbgSpawnDbg;
        private CheckBox checkBoxDbgResource;
        private Panel panelObj;
        private TextBox textBoxObjPosXZ;
        private Button buttonObjPosXnZn;
        private GroupBox groupBoxObjPos;
        private TextBox textBoxObjPosY;
        private Button buttonObjPosYp;
        private Button buttonObjPosYn;
        private Button buttonObjPosXpZp;
        private Button buttonObjPosXp;
        private Button buttonObjPosXpZn;
        private Button buttonObjPosZn;
        private Button buttonObjPosZp;
        private Button buttonObjPosXnZp;
        private Button buttonObjPosXn;
        private TabPage tabPageActions;
        private NoTearFlowLayoutPanel noTearFlowLayoutPanelActions;
        private TabPage tabPageWater;
        private NoTearFlowLayoutPanel noTearFlowLayoutPanelWater;
        private TabPage tabPageController;
        private NoTearFlowLayoutPanel NoTearFlowLayoutPanelController;
        private TabPage tabPagePu;
        private GroupBox groupBoxPuController;
        private Label labelPuConPu;
        private Label labelPuConQpuValue;
        private Label labelPuConQpu;
        private Label labelPuConPuValue;
        private Label labelPuContXp;
        private Label labelPuContXm;
        private Label labelPuContZp;
        private Label labelPuContZm;
        private Button buttonPuConZnPu;
        private Button buttonPuConXpQpu;
        private Button buttonPuConXnQpu;
        private Button buttonPuConXnPu;
        private Button buttonPuConZnQpu;
        private Button buttonPuConXpPu;
        private Button buttonPuConZpPu;
        private Button buttonPuConZpQpu;
        private Button buttonPuConHome;
        private TabPage tabPageQuarterFrame;
        private TabPage tabPageCamHack;
        private NoTearFlowLayoutPanel noTearFlowLayoutPanelFile;
        private NoTearFlowLayoutPanel noTearFlowLayoutPanelQuarterFrame;
        private NoTearFlowLayoutPanel noTearFlowLayoutPanelCamHack;
        private Button buttonAnnihilateTriangle;
        private Button buttonNeutralizeTriangle;
        private GroupBox groupBoxObjAngle;
        private TextBox textBoxObjAngleYaw;
        private Button buttonObjAngleYawP;
        private Button buttonObjAngleYawN;
        private TextBox textBoxObjAngleRoll;
        private TextBox textBoxObjAnglePitch;
        private Button buttonObjAngleRollN;
        private Button buttonObjAnglePitchN;
        private Button buttonObjAngleRollP;
        private Button buttonObjAnglePitchP;
        private GroupBox groupBoxObjHome;
        private TextBox textBoxObjHomeY;
        private Button buttonObjHomeYp;
        private Button buttonObjHomeYn;
        private Button buttonObjHomeXpZp;
        private TextBox textBoxObjHomeXZ;
        private Button buttonObjHomeXp;
        private Button buttonObjHomeXpZn;
        private Button buttonObjHomeZn;
        private Button buttonObjHomeZp;
        private Button buttonObjHomeXnZp;
        private Button buttonObjHomeXn;
        private Button buttonObjHomeXnZn;
        private Button buttonObjRelease;
        private Button buttonObjInteract;
        private GroupBox groupBoxMarioPos;
        private TextBox textBoxMarioPosY;
        private Button buttonMarioPosYp;
        private Button buttonMarioPosYn;
        private Button buttonMarioPosXpZp;
        private TextBox textBoxMarioPosXZ;
        private Button buttonMarioPosXp;
        private Button buttonMarioPosXpZn;
        private Button buttonMarioPosZn;
        private Button buttonMarioPosZp;
        private Button buttonMarioPosXnZp;
        private Button buttonMarioPosXn;
        private Button buttonMarioPosXnZn;
        private GroupBox groupBoxMarioStats;
        private TextBox textBoxMarioStatsVspd;
        private TextBox textBoxMarioStatsHspd;
        private TextBox textBoxMarioStatsYaw;
        private Button buttonMarioStatsVspdN;
        private Button buttonMarioStatsHspdN;
        private Button buttonMarioStatsYawN;
        private Button buttonMarioStatsVspdP;
        private Button buttonMarioStatsHspdP;
        private Button buttonMarioStatsYawP;
        private GroupBox groupBoxMarioHOLP;
        private TextBox textBoxMarioHOLPY;
        private Button buttonMarioHOLPYp;
        private Button buttonMarioHOLPYn;
        private Button buttonMarioHOLPXpZp;
        private TextBox textBoxMarioHOLPXZ;
        private Button buttonMarioHOLPXp;
        private Button buttonMarioHOLPXpZn;
        private Button buttonMarioHOLPZn;
        private Button buttonMarioHOLPZp;
        private Button buttonMarioHOLPXnZp;
        private Button buttonMarioHOLPXn;
        private Button buttonMarioHOLPXnZn;
        private GroupBox groupBoxTrianglePos;
        private TextBox textBoxTrianglePosY;
        private Button buttonTrianglePosYp;
        private Button buttonTrianglePosYn;
        private Button buttonTrianglePosXpZp;
        private TextBox textBoxTrianglePosXZ;
        private Button buttonTrianglePosXp;
        private Button buttonTrianglePosXpZn;
        private Button buttonTrianglePosZn;
        private Button buttonTrianglePosZp;
        private Button buttonTrianglePosXnZp;
        private Button buttonTrianglePosXn;
        private Button buttonTrianglePosXnZn;
        private GroupBox groupBoxCameraPos;
        private TextBox textBoxCameraPosY;
        private Button buttonCameraPosYp;
        private Button buttonCameraPosYn;
        private Button buttonCameraPosXpZp;
        private TextBox textBoxCameraPosXZ;
        private Button buttonCameraPosXp;
        private Button buttonCameraPosXpZn;
        private Button buttonCameraPosZn;
        private Button buttonCameraPosZp;
        private Button buttonCameraPosXnZp;
        private Button buttonCameraPosXn;
        private Button buttonCameraPosXnZn;
        private GroupBox groupBoxTriangleNormal;
        private TextBox textBoxTriangleNormal;
        private Button buttonTriangleNormalN;
        private Button buttonTriangleNormalP;
        private GroupBox groupBoxCameraSphericalPos;
        private TextBox textBoxCameraSphericalPosR;
        private Button buttonCameraSphericalPosRn;
        private Button buttonCameraSphericalPosRp;
        private Button buttonCameraSphericalPosTpPp;
        private TextBox textBoxCameraSphericalPosTP;
        private Button buttonCameraSphericalPosTp;
        private Button buttonCameraSphericalPosTpPn;
        private Button buttonCameraSphericalPosPn;
        private Button buttonCameraSphericalPosPp;
        private Button buttonCameraSphericalPosTnPp;
        private Button buttonCameraSphericalPosTn;
        private Button buttonCameraSphericalPosTnPn;
        private CheckBox checkBoxScaleDiagonalPositionControllerButtons;
        private CheckBox checkBoxMarioPosRelative;
        private CheckBox checkBoxMarioHOLPRelative;
        private CheckBox checkBoxObjHomeRelative;
        private CheckBox checkBoxObjPosRelative;
        private CheckBox checkBoxTrianglePosRelative;
        private CheckBox checkBoxCameraPosRelative;
        private CheckBox checkBoxCameraSphericalPosPivotOnFocus;
        private Button buttonRefreshAndConnect;
        private GroupBox groupBoxShowOverlay;
        private CheckBox checkBoxShowOverlayClosestObject;
        private CheckBox checkBoxShowOverlayStoodOnObject;
        private CheckBox checkBoxShowOverlayHeldObject;
        private CheckBox checkBoxShowOverlayUsedObject;
        private CheckBox checkBoxShowOverlayInteractionObject;
        private CheckBox checkBoxShowOverlayCameraObject;
        private CheckBox checkBoxMapShowCeiling;
        private CheckBox checkBoxDisableActionUpdateWhenCloning;
        private CheckBox checkBoxNeutralizeTriangleWith21;
        private GroupBox groupBoxObjScale;
        private TextBox textBoxObjScaleDepth;
        private TextBox textBoxObjScaleHeight;
        private TextBox textBoxObjScaleWidth;
        private Button buttonObjScaleDepthN;
        private Button buttonObjScaleHeightN;
        private Button buttonObjScaleWidthN;
        private Button buttonObjScaleDepthP;
        private Button buttonObjScaleHeightP;
        private Button buttonObjScaleWidthP;
        private CheckBox checkBoxObjScaleMultiply;
        private CheckBox checkBoxObjScaleAggregate;
        private TextBox textBoxObjScaleAggregate;
        private Button buttonObjScaleAggregateN;
        private Button buttonObjScaleAggregateP;
        private Button buttonShowBottomPane;
        private Button buttonShowRightPane;
        private Button buttonShowLeftRightPane;
        private Button buttonShowLeftPane;
        private CheckBox checkBoxShowOverlayCeilingObject;
        private CheckBox checkBoxShowOverlayWallObject;
        private CheckBox checkBoxShowOverlayFloorObject;
        private SplitContainer splitContainerController;
        private Label labelCamHackMode;
        private RadioButton radioButtonCamHackMode3;
        private RadioButton radioButtonCamHackMode2;
        private RadioButton radioButtonCamHackMode1RelativeAngle;
        private RadioButton radioButtonCamHackMode0;
        private SplitContainer splitContainerCamHack;
        private CheckBox checkBoxShowOverlayCameraHackObject;
        private RadioButton radioButtonCamHackMode1AbsoluteAngle;
        private GroupBox groupBoxCameraHackSphericalFocusPos;
        private TextBox textBoxCameraHackSphericalFocusPosR;
        private Button buttonCameraHackSphericalFocusPosRp;
        private Button buttonCameraHackSphericalFocusPosRn;
        private Button buttonCameraHackSphericalFocusPosTnPp;
        private TextBox textBoxCameraHackSphericalFocusPosTP;
        private Button buttonCameraHackSphericalFocusPosTn;
        private Button buttonCameraHackSphericalFocusPosTnPn;
        private Button buttonCameraHackSphericalFocusPosPn;
        private Button buttonCameraHackSphericalFocusPosPp;
        private Button buttonCameraHackSphericalFocusPosTpPp;
        private Button buttonCameraHackSphericalFocusPosTp;
        private Button buttonCameraHackSphericalFocusPosTpPn;
        private GroupBox groupBoxCameraHackFocusPos;
        private CheckBox checkBoxCameraHackFocusPosRelative;
        private TextBox textBoxCameraHackFocusPosY;
        private Button buttonCameraHackFocusPosYp;
        private Button buttonCameraHackFocusPosYn;
        private Button buttonCameraHackFocusPosXpZp;
        private TextBox textBoxCameraHackFocusPosXZ;
        private Button buttonCameraHackFocusPosXp;
        private Button buttonCameraHackFocusPosXpZn;
        private Button buttonCameraHackFocusPosZn;
        private Button buttonCameraHackFocusPosZp;
        private Button buttonCameraHackFocusPosXnZp;
        private Button buttonCameraHackFocusPosXn;
        private Button buttonCameraHackFocusPosXnZn;
        private GroupBox groupBoxCameraHackSphericalPos;
        private TextBox textBoxCameraHackSphericalPosR;
        private Button buttonCameraHackSphericalPosRn;
        private Button buttonCameraHackSphericalPosRp;
        private Button buttonCameraHackSphericalPosTpPp;
        private TextBox textBoxCameraHackSphericalPosTP;
        private Button buttonCameraHackSphericalPosTp;
        private Button buttonCameraHackSphericalPosTpPn;
        private Button buttonCameraHackSphericalPosPn;
        private Button buttonCameraHackSphericalPosPp;
        private Button buttonCameraHackSphericalPosTnPp;
        private Button buttonCameraHackSphericalPosTn;
        private Button buttonCameraHackSphericalPosTnPn;
        private GroupBox groupBoxCameraHackPos;
        private CheckBox checkBoxCameraHackPosRelative;
        private TextBox textBoxCameraHackPosY;
        private Button buttonCameraHackPosYp;
        private Button buttonCameraHackPosYn;
        private Button buttonCameraHackPosXpZp;
        private TextBox textBoxCameraHackPosXZ;
        private Button buttonCameraHackPosXp;
        private Button buttonCameraHackPosXpZn;
        private Button buttonCameraHackPosZn;
        private Button buttonCameraHackPosZp;
        private Button buttonCameraHackPosXnZp;
        private Button buttonCameraHackPosXn;
        private Button buttonCameraHackPosXnZn;
        private GroupBox groupBoxGotoRetrieveOffsets;
        private Label labelRetrieveInfrontSuffix;
        private Label labelRetrieveInfrontPrefix;
        private TextBox textBoxRetrieveInfront;
        private Label labelRetrieveAboveSuffix;
        private Label labelRetrieveAbovePrefix;
        private TextBox textBoxRetrieveAbove;
        private Label labelGotoInfrontSuffix;
        private Label labelGotoInfrontPrefix;
        private TextBox textBoxGotoInfront;
        private Label labelGotoAboveSuffix;
        private Label labelGotoAbovePrefix;
        private TextBox textBoxGotoAbove;
        private SplitContainer splitContainerFile;
        private SplitContainer SplitContainerCamera;
        private SplitContainer splitContainerTriangles;
        private SplitContainer splitContainerHud;
        private SplitContainer splitContainerMario;
        private SplitContainer splitContainerObject;
        private ControllerDisplayPanel controllerDisplayPanel;
        private RadioButton radioButtonFileCSaved;
        private RadioButton radioButtonFileDSaved;
        private RadioButton radioButtonFileASaved;
        private RadioButton radioButtonFileBSaved;
        private RadioButton radioButtonFileC;
        private RadioButton radioButtonFileD;
        private RadioButton radioButtonFileA;
        private RadioButton radioButtonFileB;
        private Button buttonFileSave;
        private Label labelSpawnBehavior;
        private TextBox textBoxSpawnBehavior;
        private GroupBox groupBoxFile;
        private GroupBox groupBoxHatLocation;
        private RadioButton radioButtonHatLocationSLGround;
        private RadioButton radioButtonHatLocationTTMGround;
        private RadioButton radioButtonHatLocationTTMUkiki;
        private RadioButton radioButtonHatLocationSSLKlepto;
        private RadioButton radioButtonHatLocationMario;
        private RadioButton radioButtonHatLocationSLSnowman;
        private RadioButton radioButtonHatLocationSSLGround;
        private TableLayoutPanel tableLayoutPanelFile;
        private FileCourseLabel labelFileTableRow1;
        private FileCourseLabel labelFileTableRow2;
        private FileCourseLabel labelFileTableRow3;
        private FileCourseLabel labelFileTableRow4;
        private FileCourseLabel labelFileTableRow5;
        private FileCourseLabel labelFileTableRow6;
        private FileCourseLabel labelFileTableRow7;
        private FileCourseLabel labelFileTableRow8;
        private FileCourseLabel labelFileTableRow9;
        private FileCourseLabel labelFileTableRow10;
        private FileCourseLabel labelFileTableRow11;
        private FileCourseLabel labelFileTableRow12;
        private FileCourseLabel labelFileTableRow13;
        private FileCourseLabel labelFileTableRow14;
        private FileCourseLabel labelFileTableRow15;
        private FileCourseLabel labelFileTableRow16;
        private FileCourseLabel labelFileTableRow17;
        private FileCourseLabel labelFileTableRow18;
        private FileCourseLabel labelFileTableRow19;
        private FileCourseLabel labelFileTableRow20;
        private FileCourseLabel labelFileTableRow21;
        private FileCourseLabel labelFileTableRow22;
        private FileCourseLabel labelFileTableRow23;
        private FileCourseLabel labelFileTableRow24;
        private FileCourseLabel labelFileTableRow25;
        private FileCourseLabel labelFileTableRow26;
        private FileStarPictureBox filePictureBoxTableRow1Col1;
        private FileStarPictureBox filePictureBoxTableRow5Col1;
        private FileStarPictureBox filePictureBoxTableRow4Col1;
        private FileStarPictureBox filePictureBoxTableRow3Col1;
        private FileStarPictureBox filePictureBoxTableRow2Col1;
        private FileStarPictureBox filePictureBoxTableRow15Col1;
        private FileStarPictureBox filePictureBoxTableRow14Col1;
        private FileStarPictureBox filePictureBoxTableRow13Col1;
        private FileStarPictureBox filePictureBoxTableRow12Col1;
        private FileStarPictureBox filePictureBoxTableRow11Col1;
        private FileStarPictureBox filePictureBoxTableRow10Col1;
        private FileStarPictureBox filePictureBoxTableRow9Col1;
        private FileStarPictureBox filePictureBoxTableRow8Col1;
        private FileStarPictureBox filePictureBoxTableRow7Col1;
        private FileStarPictureBox filePictureBoxTableRow6Col1;
        private FileStarPictureBox filePictureBoxTableRow15Col2;
        private FileStarPictureBox filePictureBoxTableRow14Col2;
        private FileStarPictureBox filePictureBoxTableRow13Col2;
        private FileStarPictureBox filePictureBoxTableRow12Col2;
        private FileStarPictureBox filePictureBoxTableRow11Col2;
        private FileStarPictureBox filePictureBoxTableRow10Col2;
        private FileStarPictureBox filePictureBoxTableRow9Col2;
        private FileStarPictureBox filePictureBoxTableRow8Col2;
        private FileStarPictureBox filePictureBoxTableRow7Col2;
        private FileStarPictureBox filePictureBoxTableRow6Col2;
        private FileStarPictureBox filePictureBoxTableRow5Col2;
        private FileStarPictureBox filePictureBoxTableRow4Col2;
        private FileStarPictureBox filePictureBoxTableRow3Col2;
        private FileStarPictureBox filePictureBoxTableRow2Col2;
        private FileStarPictureBox filePictureBoxTableRow1Col2;
        private FileStarPictureBox filePictureBoxTableRow15Col3;
        private FileStarPictureBox filePictureBoxTableRow14Col3;
        private FileStarPictureBox filePictureBoxTableRow13Col3;
        private FileStarPictureBox filePictureBoxTableRow12Col3;
        private FileStarPictureBox filePictureBoxTableRow11Col3;
        private FileStarPictureBox filePictureBoxTableRow10Col3;
        private FileStarPictureBox filePictureBoxTableRow9Col3;
        private FileStarPictureBox filePictureBoxTableRow8Col3;
        private FileStarPictureBox filePictureBoxTableRow7Col3;
        private FileStarPictureBox filePictureBoxTableRow6Col3;
        private FileStarPictureBox filePictureBoxTableRow5Col3;
        private FileStarPictureBox filePictureBoxTableRow4Col3;
        private FileStarPictureBox filePictureBoxTableRow3Col3;
        private FileStarPictureBox filePictureBoxTableRow2Col3;
        private FileStarPictureBox filePictureBoxTableRow1Col3;
        private FileStarPictureBox filePictureBoxTableRow15Col4;
        private FileStarPictureBox filePictureBoxTableRow14Col4;
        private FileStarPictureBox filePictureBoxTableRow13Col4;
        private FileStarPictureBox filePictureBoxTableRow12Col4;
        private FileStarPictureBox filePictureBoxTableRow11Col4;
        private FileStarPictureBox filePictureBoxTableRow10Col4;
        private FileStarPictureBox filePictureBoxTableRow9Col4;
        private FileStarPictureBox filePictureBoxTableRow8Col4;
        private FileStarPictureBox filePictureBoxTableRow7Col4;
        private FileStarPictureBox filePictureBoxTableRow6Col4;
        private FileStarPictureBox filePictureBoxTableRow5Col4;
        private FileStarPictureBox filePictureBoxTableRow4Col4;
        private FileStarPictureBox filePictureBoxTableRow3Col4;
        private FileStarPictureBox filePictureBoxTableRow2Col4;
        private FileStarPictureBox filePictureBoxTableRow1Col4;
        private FileStarPictureBox filePictureBoxTableRow15Col5;
        private FileStarPictureBox filePictureBoxTableRow14Col5;
        private FileStarPictureBox filePictureBoxTableRow13Col5;
        private FileStarPictureBox filePictureBoxTableRow12Col5;
        private FileStarPictureBox filePictureBoxTableRow11Col5;
        private FileStarPictureBox filePictureBoxTableRow10Col5;
        private FileStarPictureBox filePictureBoxTableRow9Col5;
        private FileStarPictureBox filePictureBoxTableRow8Col5;
        private FileStarPictureBox filePictureBoxTableRow7Col5;
        private FileStarPictureBox filePictureBoxTableRow6Col5;
        private FileStarPictureBox filePictureBoxTableRow5Col5;
        private FileStarPictureBox filePictureBoxTableRow4Col5;
        private FileStarPictureBox filePictureBoxTableRow3Col5;
        private FileStarPictureBox filePictureBoxTableRow2Col5;
        private FileStarPictureBox filePictureBoxTableRow1Col5;
        private FileStarPictureBox filePictureBoxTableRow15Col6;
        private FileStarPictureBox filePictureBoxTableRow14Col6;
        private FileStarPictureBox filePictureBoxTableRow13Col6;
        private FileStarPictureBox filePictureBoxTableRow12Col6;
        private FileStarPictureBox filePictureBoxTableRow11Col6;
        private FileStarPictureBox filePictureBoxTableRow10Col6;
        private FileStarPictureBox filePictureBoxTableRow9Col6;
        private FileStarPictureBox filePictureBoxTableRow8Col6;
        private FileStarPictureBox filePictureBoxTableRow7Col6;
        private FileStarPictureBox filePictureBoxTableRow6Col6;
        private FileStarPictureBox filePictureBoxTableRow5Col6;
        private FileStarPictureBox filePictureBoxTableRow4Col6;
        private FileStarPictureBox filePictureBoxTableRow3Col6;
        private FileStarPictureBox filePictureBoxTableRow2Col6;
        private FileStarPictureBox filePictureBoxTableRow1Col6;
        private FileStarPictureBox filePictureBoxTableRow15Col7;
        private FileStarPictureBox filePictureBoxTableRow14Col7;
        private FileStarPictureBox filePictureBoxTableRow13Col7;
        private FileStarPictureBox filePictureBoxTableRow12Col7;
        private FileStarPictureBox filePictureBoxTableRow11Col7;
        private FileStarPictureBox filePictureBoxTableRow10Col7;
        private FileStarPictureBox filePictureBoxTableRow9Col7;
        private FileStarPictureBox filePictureBoxTableRow8Col7;
        private FileStarPictureBox filePictureBoxTableRow7Col7;
        private FileStarPictureBox filePictureBoxTableRow6Col7;
        private FileStarPictureBox filePictureBoxTableRow5Col7;
        private FileStarPictureBox filePictureBoxTableRow4Col7;
        private FileStarPictureBox filePictureBoxTableRow3Col7;
        private FileStarPictureBox filePictureBoxTableRow2Col7;
        private FileStarPictureBox filePictureBoxTableRow1Col7;
        private FileStarPictureBox filePictureBoxTableRow19Col2;
        private FileStarPictureBox filePictureBoxTableRow26Col2;
        private FileStarPictureBox filePictureBoxTableRow25Col3;
        private FileStarPictureBox filePictureBoxTableRow25Col2;
        private FileStarPictureBox filePictureBoxTableRow26Col1;
        private FileStarPictureBox filePictureBoxTableRow25Col1;
        private FileStarPictureBox filePictureBoxTableRow24Col1;
        private FileStarPictureBox filePictureBoxTableRow23Col1;
        private FileStarPictureBox filePictureBoxTableRow22Col1;
        private FileStarPictureBox filePictureBoxTableRow21Col1;
        private FileStarPictureBox filePictureBoxTableRow20Col1;
        private FileStarPictureBox filePictureBoxTableRow19Col1;
        private FileStarPictureBox filePictureBoxTableRow18Col1;
        private FileStarPictureBox filePictureBoxTableRow17Col1;
        private FileStarPictureBox filePictureBoxTableRow16Col1;
        private FileBinaryPictureBox filePictureBoxTableRow21Col8;
        private FileBinaryPictureBox filePictureBoxTableRow15Col8;
        private FileBinaryPictureBox filePictureBoxTableRow13Col8;
        private FileBinaryPictureBox filePictureBoxTableRow12Col8;
        private FileBinaryPictureBox filePictureBoxTableRow11Col8;
        private FileBinaryPictureBox filePictureBoxTableRow10Col8;
        private FileBinaryPictureBox filePictureBoxTableRow4Col8;
        private FileBinaryPictureBox filePictureBoxTableRow3Col8;
        private FileBinaryPictureBox filePictureBoxTableRow2Col8;
        private FileBinaryPictureBox filePictureBoxTableRow1Col8;
        private FileBinaryPictureBox filePictureBoxTableRow8Col8;
        private FileBinaryPictureBox filePictureBoxTableRow24Col9;
        private FileBinaryPictureBox filePictureBoxTableRow23Col9;
        private FileBinaryPictureBox filePictureBoxTableRow22Col9;
        private FileBinaryPictureBox filePictureBoxTableRow4Col9;
        private FileBinaryPictureBox filePictureBoxTableRow3Col9;
        private FileBinaryPictureBox filePictureBoxTableRow2Col9;
        private FileBinaryPictureBox filePictureBoxTableRow19Col9;
        private FileCoinScoreTextbox textBoxTableRow1Col10;
        private FileCoinScoreTextbox textBoxTableRow15Col10;
        private FileCoinScoreTextbox textBoxTableRow14Col10;
        private FileCoinScoreTextbox textBoxTableRow13Col10;
        private FileCoinScoreTextbox textBoxTableRow12Col10;
        private FileCoinScoreTextbox textBoxTableRow11Col10;
        private FileCoinScoreTextbox textBoxTableRow10Col10;
        private FileCoinScoreTextbox textBoxTableRow9Col10;
        private FileCoinScoreTextbox textBoxTableRow8Col10;
        private FileCoinScoreTextbox textBoxTableRow7Col10;
        private FileCoinScoreTextbox textBoxTableRow6Col10;
        private FileCoinScoreTextbox textBoxTableRow5Col10;
        private FileCoinScoreTextbox textBoxTableRow4Col10;
        private FileCoinScoreTextbox textBoxTableRow3Col10;
        private FileCoinScoreTextbox textBoxTableRow2Col10;
        private Button buttonFileNumStars;
        private Button buttonFileErase;
    }
}


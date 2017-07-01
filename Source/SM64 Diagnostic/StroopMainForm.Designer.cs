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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StroopMainForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.labelProcessSelect = new System.Windows.Forms.Label();
            this.groupBoxObjects = new System.Windows.Forms.GroupBox();
            this.comboBoxLabelMethod = new System.Windows.Forms.ComboBox();
            this.labelLabelMethod = new System.Windows.Forms.Label();
            this.labelSlotSize = new System.Windows.Forms.Label();
            this.labelToggleMode = new System.Windows.Forms.Label();
            this.comboBoxMapToggleMode = new System.Windows.Forms.ComboBox();
            this.checkBoxObjLockLabels = new System.Windows.Forms.CheckBox();
            this.labelSortMethod = new System.Windows.Forms.Label();
            this.NoTearFlowLayoutPanelObjects = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.comboBoxSortMethod = new System.Windows.Forms.ComboBox();
            this.trackBarObjSlotSize = new System.Windows.Forms.TrackBar();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageObjects = new System.Windows.Forms.TabPage();
            this.panelObj = new System.Windows.Forms.Panel();
            this.buttonObjDebilitate = new System.Windows.Forms.Button();
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
            this.labelObjSlotIndValue = new System.Windows.Forms.Label();
            this.labelObjSlotPosValue = new System.Windows.Forms.Label();
            this.labelObjBhvValue = new System.Windows.Forms.Label();
            this.labelObjAdd = new System.Windows.Forms.Label();
            this.NoTearFlowLayoutPanelObject = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.labelObjSlotInd = new System.Windows.Forms.Label();
            this.labelObjSlotPos = new System.Windows.Forms.Label();
            this.labelObjBhv = new System.Windows.Forms.Label();
            this.panelObjectBorder = new System.Windows.Forms.Panel();
            this.pictureBoxObject = new SM64_Diagnostic.Controls.IntPictureBox();
            this.labelObjAddValue = new System.Windows.Forms.Label();
            this.tabPageMario = new System.Windows.Forms.TabPage();
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
            this.buttonMarioVisibility = new System.Windows.Forms.Button();
            this.buttonMarioToggleHandsfree = new System.Windows.Forms.Button();
            this.panelMarioBorder = new System.Windows.Forms.Panel();
            this.pictureBoxMario = new SM64_Diagnostic.Controls.IntPictureBox();
            this.NoTearFlowLayoutPanelMario = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.tabPageActions = new System.Windows.Forms.TabPage();
            this.noTearFlowLayoutPanelActions = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.tabPageHud = new System.Windows.Forms.TabPage();
            this.buttonStandardHud = new System.Windows.Forms.Button();
            this.buttonDie = new System.Windows.Forms.Button();
            this.buttonFillHp = new System.Windows.Forms.Button();
            this.panelHudBorder = new System.Windows.Forms.Panel();
            this.pictureBoxHud = new SM64_Diagnostic.Controls.IntPictureBox();
            this.NoTearFlowLayoutPanelHud = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.tabPageCamera = new System.Windows.Forms.TabPage();
            this.groupBoxCameraSphericalPos = new System.Windows.Forms.GroupBox();
            this.checkBoxCameraSphericalPosRelative = new System.Windows.Forms.CheckBox();
            this.textBoxCameraSphericalPosRadius = new System.Windows.Forms.TextBox();
            this.buttonCameraSphericalPosRadiusN = new System.Windows.Forms.Button();
            this.buttonCameraSphericalPosRadiusP = new System.Windows.Forms.Button();
            this.buttonCameraSphericalPosThetaPPhiP = new System.Windows.Forms.Button();
            this.textBoxCameraSphericalPosThetaPhi = new System.Windows.Forms.TextBox();
            this.buttonCameraSphericalPosThetaP = new System.Windows.Forms.Button();
            this.buttonCameraSphericalPosThetaPPhiN = new System.Windows.Forms.Button();
            this.buttonCameraSphericalPosPhiN = new System.Windows.Forms.Button();
            this.buttonCameraSphericalPosPhiP = new System.Windows.Forms.Button();
            this.buttonCameraSphericalPosThetaNPhiP = new System.Windows.Forms.Button();
            this.buttonCameraSphericalPosThetaN = new System.Windows.Forms.Button();
            this.buttonCameraSphericalPosThetaNPhiN = new System.Windows.Forms.Button();
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
            this.panelCameraBorder = new System.Windows.Forms.Panel();
            this.pictureBoxCamera = new SM64_Diagnostic.Controls.IntPictureBox();
            this.NoTearFlowLayoutPanelCamera = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.tabPageTriangles = new System.Windows.Forms.TabPage();
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
            this.buttonAnnihilateTriangle = new System.Windows.Forms.Button();
            this.buttonNeutralizeTriangle = new System.Windows.Forms.Button();
            this.buttonGoToVClosest = new System.Windows.Forms.Button();
            this.checkBoxVertexMisalignment = new System.Windows.Forms.CheckBox();
            this.buttonRetrieveTriangle = new System.Windows.Forms.Button();
            this.buttonGoToV3 = new System.Windows.Forms.Button();
            this.buttonGoToV2 = new System.Windows.Forms.Button();
            this.buttonGoToV1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.maskedTextBoxOtherTriangle = new System.Windows.Forms.MaskedTextBox();
            this.radioButtonTriOther = new System.Windows.Forms.RadioButton();
            this.radioButtonTriCeiling = new System.Windows.Forms.RadioButton();
            this.radioButtonTriWall = new System.Windows.Forms.RadioButton();
            this.radioButtonTriFloor = new System.Windows.Forms.RadioButton();
            this.NoTearFlowLayoutPanelTriangles = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.tabPageWater = new System.Windows.Forms.TabPage();
            this.noTearFlowLayoutPanelWater = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.tabPageController = new System.Windows.Forms.TabPage();
            this.pictureBoxController = new SM64_Diagnostic.Controls.IntPictureBox();
            this.NoTearFlowLayoutPanelController = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.tabPageFile = new System.Windows.Forms.TabPage();
            this.noTearFlowLayoutPanelFile = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.tabPageLevel = new System.Windows.Forms.TabPage();
            this.noTearFlowLayoutPanelLevel = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.tabPageMisc = new System.Windows.Forms.TabPage();
            this.panelMiscBorder = new System.Windows.Forms.Panel();
            this.pictureBoxMisc = new SM64_Diagnostic.Controls.IntPictureBox();
            this.NoTearFlowLayoutPanelMisc = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.tabPageDebug = new System.Windows.Forms.TabPage();
            this.checkBoxDbgResource = new System.Windows.Forms.CheckBox();
            this.checkBoxDbgStageSelect = new System.Windows.Forms.CheckBox();
            this.checkBoxDbgClassicDbg = new System.Windows.Forms.CheckBox();
            this.buttonDbgFreeMovement = new System.Windows.Forms.Button();
            this.checkBoxDbgSpawnDbg = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panelDebugBorder = new System.Windows.Forms.Panel();
            this.pictureBoxDebug = new SM64_Diagnostic.Controls.IntPictureBox();
            this.NoTearFlowLayoutPanelDebugDisplayType = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.radioButtonDbgOff = new System.Windows.Forms.RadioButton();
            this.radioButtonDbgObjCnt = new System.Windows.Forms.RadioButton();
            this.radioButtonDbgChkInfo = new System.Windows.Forms.RadioButton();
            this.radioButtonDbgMapInfo = new System.Windows.Forms.RadioButton();
            this.radioButtonDbgStgInfo = new System.Windows.Forms.RadioButton();
            this.radioButtonDbgFxInfo = new System.Windows.Forms.RadioButton();
            this.radioButtonDbgEnemyInfo = new System.Windows.Forms.RadioButton();
            this.tabPageMap = new System.Windows.Forms.TabPage();
            this.splitContainerMap = new System.Windows.Forms.SplitContainer();
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
            this.labelSpawnHint = new System.Windows.Forms.Label();
            this.buttonSpawnReset = new System.Windows.Forms.Button();
            this.labelSpawnExtra = new System.Windows.Forms.Label();
            this.labelSpawnGfxId = new System.Windows.Forms.Label();
            this.textBoxSpawnExtra = new System.Windows.Forms.TextBox();
            this.textBoxSpawnGfxId = new System.Windows.Forms.TextBox();
            this.buttonHackSpawn = new System.Windows.Forms.Button();
            this.listBoxSpawn = new System.Windows.Forms.ListBox();
            this.tabPageCamHack = new System.Windows.Forms.TabPage();
            this.noTearFlowLayoutPanelCamHack = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.tabPageOptions = new System.Windows.Forms.TabPage();
            this.groupBoxShowOverlay = new System.Windows.Forms.GroupBox();
            this.checkBoxShowOverlayUsedObject = new System.Windows.Forms.CheckBox();
            this.checkBoxShowOverlayInteractionObject = new System.Windows.Forms.CheckBox();
            this.checkBoxShowOverlayCameraSecondaryObject = new System.Windows.Forms.CheckBox();
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
            this.buttonCollapseTop = new System.Windows.Forms.Button();
            this.buttonCollapseBottom = new System.Windows.Forms.Button();
            this.buttonReadOnly = new System.Windows.Forms.Button();
            this.checkBoxMapShowCeiling = new System.Windows.Forms.CheckBox();
            this.groupBoxObjects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarObjSlotSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageObjects.SuspendLayout();
            this.panelObj.SuspendLayout();
            this.groupBoxObjHome.SuspendLayout();
            this.groupBoxObjAngle.SuspendLayout();
            this.groupBoxObjPos.SuspendLayout();
            this.panelObjectBorder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxObject)).BeginInit();
            this.tabPageMario.SuspendLayout();
            this.groupBoxMarioStats.SuspendLayout();
            this.groupBoxMarioHOLP.SuspendLayout();
            this.groupBoxMarioPos.SuspendLayout();
            this.panelMarioBorder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMario)).BeginInit();
            this.tabPageActions.SuspendLayout();
            this.tabPageHud.SuspendLayout();
            this.panelHudBorder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHud)).BeginInit();
            this.tabPageCamera.SuspendLayout();
            this.groupBoxCameraSphericalPos.SuspendLayout();
            this.groupBoxCameraPos.SuspendLayout();
            this.panelCameraBorder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCamera)).BeginInit();
            this.tabPageTriangles.SuspendLayout();
            this.groupBoxTrianglePos.SuspendLayout();
            this.groupBoxTriangleNormal.SuspendLayout();
            this.tabPageWater.SuspendLayout();
            this.tabPageController.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxController)).BeginInit();
            this.tabPageFile.SuspendLayout();
            this.tabPageLevel.SuspendLayout();
            this.tabPageMisc.SuspendLayout();
            this.panelMiscBorder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMisc)).BeginInit();
            this.tabPageDebug.SuspendLayout();
            this.panelDebugBorder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDebug)).BeginInit();
            this.NoTearFlowLayoutPanelDebugDisplayType.SuspendLayout();
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
            this.tabPageOptions.SuspendLayout();
            this.groupBoxShowOverlay.SuspendLayout();
            this.panelConnect.SuspendLayout();
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
            this.groupBoxObjects.Size = new System.Drawing.Size(923, 109);
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
            // NoTearFlowLayoutPanelObjects
            // 
            this.NoTearFlowLayoutPanelObjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NoTearFlowLayoutPanelObjects.AutoScroll = true;
            this.NoTearFlowLayoutPanelObjects.Location = new System.Drawing.Point(4, 45);
            this.NoTearFlowLayoutPanelObjects.Margin = new System.Windows.Forms.Padding(2);
            this.NoTearFlowLayoutPanelObjects.Name = "NoTearFlowLayoutPanelObjects";
            this.NoTearFlowLayoutPanelObjects.Size = new System.Drawing.Size(915, 60);
            this.NoTearFlowLayoutPanelObjects.TabIndex = 0;
            this.NoTearFlowLayoutPanelObjects.Resize += new System.EventHandler(this.NoTearFlowLayoutPanelObjects_Resize);
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
            this.splitContainerMain.Location = new System.Drawing.Point(12, 36);
            this.splitContainerMain.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.tabControlMain);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.groupBoxObjects);
            this.splitContainerMain.Size = new System.Drawing.Size(927, 698);
            this.splitContainerMain.SplitterDistance = 604;
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
            this.tabControlMain.Controls.Add(this.tabPageLevel);
            this.tabControlMain.Controls.Add(this.tabPageMisc);
            this.tabControlMain.Controls.Add(this.tabPageDebug);
            this.tabControlMain.Controls.Add(this.tabPageMap);
            this.tabControlMain.Controls.Add(this.tabPagePu);
            this.tabControlMain.Controls.Add(this.tabPageExpressions);
            this.tabControlMain.Controls.Add(this.tabPageDisassembly);
            this.tabControlMain.Controls.Add(this.tabPageHacks);
            this.tabControlMain.Controls.Add(this.tabPageCamHack);
            this.tabControlMain.Controls.Add(this.tabPageOptions);
            this.tabControlMain.HotTrack = true;
            this.tabControlMain.Location = new System.Drawing.Point(2, 2);
            this.tabControlMain.Margin = new System.Windows.Forms.Padding(2);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(923, 602);
            this.tabControlMain.TabIndex = 3;
            this.tabControlMain.SelectedIndexChanged += new System.EventHandler(this.tabControlMain_SelectedIndexChanged);
            // 
            // tabPageObjects
            // 
            this.tabPageObjects.BackColor = System.Drawing.Color.Transparent;
            this.tabPageObjects.Controls.Add(this.panelObj);
            this.tabPageObjects.Controls.Add(this.textBoxObjName);
            this.tabPageObjects.Controls.Add(this.labelObjSlotIndValue);
            this.tabPageObjects.Controls.Add(this.labelObjSlotPosValue);
            this.tabPageObjects.Controls.Add(this.labelObjBhvValue);
            this.tabPageObjects.Controls.Add(this.labelObjAdd);
            this.tabPageObjects.Controls.Add(this.NoTearFlowLayoutPanelObject);
            this.tabPageObjects.Controls.Add(this.labelObjSlotInd);
            this.tabPageObjects.Controls.Add(this.labelObjSlotPos);
            this.tabPageObjects.Controls.Add(this.labelObjBhv);
            this.tabPageObjects.Controls.Add(this.panelObjectBorder);
            this.tabPageObjects.Controls.Add(this.labelObjAddValue);
            this.tabPageObjects.Location = new System.Drawing.Point(4, 22);
            this.tabPageObjects.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageObjects.Name = "tabPageObjects";
            this.tabPageObjects.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageObjects.Size = new System.Drawing.Size(915, 576);
            this.tabPageObjects.TabIndex = 0;
            this.tabPageObjects.Text = "Object";
            // 
            // panelObj
            // 
            this.panelObj.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panelObj.AutoScroll = true;
            this.panelObj.Controls.Add(this.buttonObjDebilitate);
            this.panelObj.Controls.Add(this.buttonObjInteract);
            this.panelObj.Controls.Add(this.groupBoxObjHome);
            this.panelObj.Controls.Add(this.groupBoxObjAngle);
            this.panelObj.Controls.Add(this.groupBoxObjPos);
            this.panelObj.Controls.Add(this.buttonObjGoToHome);
            this.panelObj.Controls.Add(this.buttonObjRetrieve);
            this.panelObj.Controls.Add(this.buttonObjRetrieveHome);
            this.panelObj.Controls.Add(this.buttonObjGoTo);
            this.panelObj.Controls.Add(this.buttonObjClone);
            this.panelObj.Controls.Add(this.buttonObjUnload);
            this.panelObj.Location = new System.Drawing.Point(3, 88);
            this.panelObj.Name = "panelObj";
            this.panelObj.Size = new System.Drawing.Size(211, 483);
            this.panelObj.TabIndex = 19;
            // 
            // buttonObjDebilitate
            // 
            this.buttonObjDebilitate.Location = new System.Drawing.Point(2, 53);
            this.buttonObjDebilitate.Margin = new System.Windows.Forms.Padding(2);
            this.buttonObjDebilitate.Name = "buttonObjDebilitate";
            this.buttonObjDebilitate.Size = new System.Drawing.Size(91, 21);
            this.buttonObjDebilitate.TabIndex = 38;
            this.buttonObjDebilitate.Text = "Debilitate";
            this.buttonObjDebilitate.UseVisualStyleBackColor = true;
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
            this.groupBoxObjHome.Location = new System.Drawing.Point(3, 357);
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
            this.textBoxObjName.Location = new System.Drawing.Point(65, 5);
            this.textBoxObjName.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxObjName.Multiline = true;
            this.textBoxObjName.Name = "textBoxObjName";
            this.textBoxObjName.ReadOnly = true;
            this.textBoxObjName.Size = new System.Drawing.Size(139, 26);
            this.textBoxObjName.TabIndex = 16;
            this.textBoxObjName.Text = "No Object Selected\r\n";
            this.textBoxObjName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelObjSlotIndValue
            // 
            this.labelObjSlotIndValue.Location = new System.Drawing.Point(120, 59);
            this.labelObjSlotIndValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelObjSlotIndValue.Name = "labelObjSlotIndValue";
            this.labelObjSlotIndValue.Size = new System.Drawing.Size(39, 13);
            this.labelObjSlotIndValue.TabIndex = 11;
            this.labelObjSlotIndValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelObjSlotPosValue
            // 
            this.labelObjSlotPosValue.Location = new System.Drawing.Point(112, 45);
            this.labelObjSlotPosValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelObjSlotPosValue.Name = "labelObjSlotPosValue";
            this.labelObjSlotPosValue.Size = new System.Drawing.Size(47, 14);
            this.labelObjSlotPosValue.TabIndex = 10;
            this.labelObjSlotPosValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelObjBhvValue
            // 
            this.labelObjBhvValue.Location = new System.Drawing.Point(91, 32);
            this.labelObjBhvValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelObjBhvValue.Name = "labelObjBhvValue";
            this.labelObjBhvValue.Size = new System.Drawing.Size(68, 13);
            this.labelObjBhvValue.TabIndex = 9;
            this.labelObjBhvValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelObjAdd
            // 
            this.labelObjAdd.AutoSize = true;
            this.labelObjAdd.Location = new System.Drawing.Point(63, 72);
            this.labelObjAdd.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.labelObjAdd.Name = "labelObjAdd";
            this.labelObjAdd.Size = new System.Drawing.Size(29, 13);
            this.labelObjAdd.TabIndex = 8;
            this.labelObjAdd.Text = "Add:";
            // 
            // NoTearFlowLayoutPanelObject
            // 
            this.NoTearFlowLayoutPanelObject.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NoTearFlowLayoutPanelObject.AutoScroll = true;
            this.NoTearFlowLayoutPanelObject.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.NoTearFlowLayoutPanelObject.Location = new System.Drawing.Point(219, 3);
            this.NoTearFlowLayoutPanelObject.Margin = new System.Windows.Forms.Padding(2);
            this.NoTearFlowLayoutPanelObject.Name = "NoTearFlowLayoutPanelObject";
            this.NoTearFlowLayoutPanelObject.Size = new System.Drawing.Size(694, 569);
            this.NoTearFlowLayoutPanelObject.TabIndex = 3;
            // 
            // labelObjSlotInd
            // 
            this.labelObjSlotInd.AutoSize = true;
            this.labelObjSlotInd.Location = new System.Drawing.Point(63, 59);
            this.labelObjSlotInd.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelObjSlotInd.Name = "labelObjSlotInd";
            this.labelObjSlotInd.Size = new System.Drawing.Size(57, 13);
            this.labelObjSlotInd.TabIndex = 7;
            this.labelObjSlotInd.Text = "Slot Index:";
            // 
            // labelObjSlotPos
            // 
            this.labelObjSlotPos.AutoSize = true;
            this.labelObjSlotPos.Location = new System.Drawing.Point(63, 45);
            this.labelObjSlotPos.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelObjSlotPos.Name = "labelObjSlotPos";
            this.labelObjSlotPos.Size = new System.Drawing.Size(49, 13);
            this.labelObjSlotPos.TabIndex = 6;
            this.labelObjSlotPos.Text = "Slot Pos:";
            // 
            // labelObjBhv
            // 
            this.labelObjBhv.AutoSize = true;
            this.labelObjBhv.Location = new System.Drawing.Point(63, 32);
            this.labelObjBhv.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelObjBhv.Name = "labelObjBhv";
            this.labelObjBhv.Size = new System.Drawing.Size(29, 13);
            this.labelObjBhv.TabIndex = 5;
            this.labelObjBhv.Text = "Bhv:";
            // 
            // panelObjectBorder
            // 
            this.panelObjectBorder.Controls.Add(this.pictureBoxObject);
            this.panelObjectBorder.Location = new System.Drawing.Point(4, 5);
            this.panelObjectBorder.Margin = new System.Windows.Forms.Padding(2);
            this.panelObjectBorder.Name = "panelObjectBorder";
            this.panelObjectBorder.Size = new System.Drawing.Size(57, 55);
            this.panelObjectBorder.TabIndex = 2;
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
            // labelObjAddValue
            // 
            this.labelObjAddValue.Location = new System.Drawing.Point(85, 72);
            this.labelObjAddValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelObjAddValue.Name = "labelObjAddValue";
            this.labelObjAddValue.Size = new System.Drawing.Size(75, 17);
            this.labelObjAddValue.TabIndex = 12;
            this.labelObjAddValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tabPageMario
            // 
            this.tabPageMario.BackColor = System.Drawing.Color.Transparent;
            this.tabPageMario.Controls.Add(this.groupBoxMarioStats);
            this.tabPageMario.Controls.Add(this.groupBoxMarioHOLP);
            this.tabPageMario.Controls.Add(this.groupBoxMarioPos);
            this.tabPageMario.Controls.Add(this.buttonMarioVisibility);
            this.tabPageMario.Controls.Add(this.buttonMarioToggleHandsfree);
            this.tabPageMario.Controls.Add(this.panelMarioBorder);
            this.tabPageMario.Controls.Add(this.NoTearFlowLayoutPanelMario);
            this.tabPageMario.Location = new System.Drawing.Point(4, 22);
            this.tabPageMario.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageMario.Name = "tabPageMario";
            this.tabPageMario.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageMario.Size = new System.Drawing.Size(915, 576);
            this.tabPageMario.TabIndex = 1;
            this.tabPageMario.Text = "Mario";
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
            this.groupBoxMarioStats.Location = new System.Drawing.Point(3, 264);
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
            this.groupBoxMarioHOLP.Location = new System.Drawing.Point(3, 365);
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
            this.groupBoxMarioPos.Location = new System.Drawing.Point(3, 112);
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
            // buttonMarioVisibility
            // 
            this.buttonMarioVisibility.Location = new System.Drawing.Point(3, 64);
            this.buttonMarioVisibility.Name = "buttonMarioVisibility";
            this.buttonMarioVisibility.Size = new System.Drawing.Size(92, 42);
            this.buttonMarioVisibility.TabIndex = 3;
            this.buttonMarioVisibility.Text = "Toggle Visibility";
            this.buttonMarioVisibility.UseVisualStyleBackColor = true;
            // 
            // buttonMarioToggleHandsfree
            // 
            this.buttonMarioToggleHandsfree.Location = new System.Drawing.Point(96, 64);
            this.buttonMarioToggleHandsfree.Name = "buttonMarioToggleHandsfree";
            this.buttonMarioToggleHandsfree.Size = new System.Drawing.Size(92, 42);
            this.buttonMarioToggleHandsfree.TabIndex = 2;
            this.buttonMarioToggleHandsfree.Text = "Toggle Handsfree";
            this.buttonMarioToggleHandsfree.UseVisualStyleBackColor = true;
            // 
            // panelMarioBorder
            // 
            this.panelMarioBorder.Controls.Add(this.pictureBoxMario);
            this.panelMarioBorder.Location = new System.Drawing.Point(4, 4);
            this.panelMarioBorder.Margin = new System.Windows.Forms.Padding(2);
            this.panelMarioBorder.Name = "panelMarioBorder";
            this.panelMarioBorder.Size = new System.Drawing.Size(57, 55);
            this.panelMarioBorder.TabIndex = 0;
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
            this.NoTearFlowLayoutPanelMario.Location = new System.Drawing.Point(193, 6);
            this.NoTearFlowLayoutPanelMario.Margin = new System.Windows.Forms.Padding(2);
            this.NoTearFlowLayoutPanelMario.Name = "NoTearFlowLayoutPanelMario";
            this.NoTearFlowLayoutPanelMario.Size = new System.Drawing.Size(720, 566);
            this.NoTearFlowLayoutPanelMario.TabIndex = 1;
            // 
            // tabPageActions
            // 
            this.tabPageActions.Controls.Add(this.noTearFlowLayoutPanelActions);
            this.tabPageActions.Location = new System.Drawing.Point(4, 22);
            this.tabPageActions.Name = "tabPageActions";
            this.tabPageActions.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageActions.Size = new System.Drawing.Size(915, 576);
            this.tabPageActions.TabIndex = 13;
            this.tabPageActions.Text = "Actions";
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
            this.noTearFlowLayoutPanelActions.Size = new System.Drawing.Size(903, 564);
            this.noTearFlowLayoutPanelActions.TabIndex = 0;
            // 
            // tabPageHud
            // 
            this.tabPageHud.Controls.Add(this.buttonStandardHud);
            this.tabPageHud.Controls.Add(this.buttonDie);
            this.tabPageHud.Controls.Add(this.buttonFillHp);
            this.tabPageHud.Controls.Add(this.panelHudBorder);
            this.tabPageHud.Controls.Add(this.NoTearFlowLayoutPanelHud);
            this.tabPageHud.Location = new System.Drawing.Point(4, 22);
            this.tabPageHud.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageHud.Name = "tabPageHud";
            this.tabPageHud.Size = new System.Drawing.Size(915, 576);
            this.tabPageHud.TabIndex = 6;
            this.tabPageHud.Text = "HUD";
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
            // panelHudBorder
            // 
            this.panelHudBorder.Controls.Add(this.pictureBoxHud);
            this.panelHudBorder.Location = new System.Drawing.Point(2, 2);
            this.panelHudBorder.Margin = new System.Windows.Forms.Padding(2);
            this.panelHudBorder.Name = "panelHudBorder";
            this.panelHudBorder.Size = new System.Drawing.Size(57, 55);
            this.panelHudBorder.TabIndex = 2;
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
            this.NoTearFlowLayoutPanelHud.Location = new System.Drawing.Point(101, 5);
            this.NoTearFlowLayoutPanelHud.Margin = new System.Windows.Forms.Padding(2);
            this.NoTearFlowLayoutPanelHud.Name = "NoTearFlowLayoutPanelHud";
            this.NoTearFlowLayoutPanelHud.Size = new System.Drawing.Size(812, 569);
            this.NoTearFlowLayoutPanelHud.TabIndex = 3;
            // 
            // tabPageCamera
            // 
            this.tabPageCamera.Controls.Add(this.groupBoxCameraSphericalPos);
            this.tabPageCamera.Controls.Add(this.groupBoxCameraPos);
            this.tabPageCamera.Controls.Add(this.panelCameraBorder);
            this.tabPageCamera.Controls.Add(this.NoTearFlowLayoutPanelCamera);
            this.tabPageCamera.Location = new System.Drawing.Point(4, 22);
            this.tabPageCamera.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageCamera.Name = "tabPageCamera";
            this.tabPageCamera.Size = new System.Drawing.Size(915, 576);
            this.tabPageCamera.TabIndex = 7;
            this.tabPageCamera.Text = "Camera";
            // 
            // groupBoxCameraSphericalPos
            // 
            this.groupBoxCameraSphericalPos.Controls.Add(this.checkBoxCameraSphericalPosRelative);
            this.groupBoxCameraSphericalPos.Controls.Add(this.textBoxCameraSphericalPosRadius);
            this.groupBoxCameraSphericalPos.Controls.Add(this.buttonCameraSphericalPosRadiusN);
            this.groupBoxCameraSphericalPos.Controls.Add(this.buttonCameraSphericalPosRadiusP);
            this.groupBoxCameraSphericalPos.Controls.Add(this.buttonCameraSphericalPosThetaPPhiP);
            this.groupBoxCameraSphericalPos.Controls.Add(this.textBoxCameraSphericalPosThetaPhi);
            this.groupBoxCameraSphericalPos.Controls.Add(this.buttonCameraSphericalPosThetaP);
            this.groupBoxCameraSphericalPos.Controls.Add(this.buttonCameraSphericalPosThetaPPhiN);
            this.groupBoxCameraSphericalPos.Controls.Add(this.buttonCameraSphericalPosPhiN);
            this.groupBoxCameraSphericalPos.Controls.Add(this.buttonCameraSphericalPosPhiP);
            this.groupBoxCameraSphericalPos.Controls.Add(this.buttonCameraSphericalPosThetaNPhiP);
            this.groupBoxCameraSphericalPos.Controls.Add(this.buttonCameraSphericalPosThetaN);
            this.groupBoxCameraSphericalPos.Controls.Add(this.buttonCameraSphericalPosThetaNPhiN);
            this.groupBoxCameraSphericalPos.Location = new System.Drawing.Point(3, 215);
            this.groupBoxCameraSphericalPos.Name = "groupBoxCameraSphericalPos";
            this.groupBoxCameraSphericalPos.Size = new System.Drawing.Size(185, 146);
            this.groupBoxCameraSphericalPos.TabIndex = 30;
            this.groupBoxCameraSphericalPos.TabStop = false;
            this.groupBoxCameraSphericalPos.Text = "Spherical";
            // 
            // checkBoxCameraSphericalPosRelative
            // 
            this.checkBoxCameraSphericalPosRelative.AutoSize = true;
            this.checkBoxCameraSphericalPosRelative.Location = new System.Drawing.Point(91, 0);
            this.checkBoxCameraSphericalPosRelative.Name = "checkBoxCameraSphericalPosRelative";
            this.checkBoxCameraSphericalPosRelative.Size = new System.Drawing.Size(94, 17);
            this.checkBoxCameraSphericalPosRelative.TabIndex = 38;
            this.checkBoxCameraSphericalPosRelative.Text = "Pivot on Mario";
            this.checkBoxCameraSphericalPosRelative.UseVisualStyleBackColor = true;
            // 
            // textBoxCameraSphericalPosRadius
            // 
            this.textBoxCameraSphericalPosRadius.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCameraSphericalPosRadius.Location = new System.Drawing.Point(140, 70);
            this.textBoxCameraSphericalPosRadius.Name = "textBoxCameraSphericalPosRadius";
            this.textBoxCameraSphericalPosRadius.Size = new System.Drawing.Size(42, 20);
            this.textBoxCameraSphericalPosRadius.TabIndex = 33;
            this.textBoxCameraSphericalPosRadius.Text = "100";
            this.textBoxCameraSphericalPosRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonCameraSphericalPosRadiusN
            // 
            this.buttonCameraSphericalPosRadiusN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCameraSphericalPosRadiusN.Location = new System.Drawing.Point(140, 16);
            this.buttonCameraSphericalPosRadiusN.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraSphericalPosRadiusN.Name = "buttonCameraSphericalPosRadiusN";
            this.buttonCameraSphericalPosRadiusN.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraSphericalPosRadiusN.TabIndex = 35;
            this.buttonCameraSphericalPosRadiusN.Text = "R-";
            this.buttonCameraSphericalPosRadiusN.UseVisualStyleBackColor = true;
            // 
            // buttonCameraSphericalPosRadiusP
            // 
            this.buttonCameraSphericalPosRadiusP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCameraSphericalPosRadiusP.Location = new System.Drawing.Point(140, 100);
            this.buttonCameraSphericalPosRadiusP.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraSphericalPosRadiusP.Name = "buttonCameraSphericalPosRadiusP";
            this.buttonCameraSphericalPosRadiusP.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraSphericalPosRadiusP.TabIndex = 34;
            this.buttonCameraSphericalPosRadiusP.Text = "R+";
            this.buttonCameraSphericalPosRadiusP.UseVisualStyleBackColor = true;
            // 
            // buttonCameraSphericalPosThetaPPhiP
            // 
            this.buttonCameraSphericalPosThetaPPhiP.Location = new System.Drawing.Point(87, 100);
            this.buttonCameraSphericalPosThetaPPhiP.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraSphericalPosThetaPPhiP.Name = "buttonCameraSphericalPosThetaPPhiP";
            this.buttonCameraSphericalPosThetaPPhiP.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraSphericalPosThetaPPhiP.TabIndex = 32;
            this.buttonCameraSphericalPosThetaPPhiP.Text = "θ+ϕ+";
            this.buttonCameraSphericalPosThetaPPhiP.UseVisualStyleBackColor = true;
            // 
            // textBoxCameraSphericalPosThetaPhi
            // 
            this.textBoxCameraSphericalPosThetaPhi.Location = new System.Drawing.Point(45, 70);
            this.textBoxCameraSphericalPosThetaPhi.Name = "textBoxCameraSphericalPosThetaPhi";
            this.textBoxCameraSphericalPosThetaPhi.Size = new System.Drawing.Size(42, 20);
            this.textBoxCameraSphericalPosThetaPhi.TabIndex = 27;
            this.textBoxCameraSphericalPosThetaPhi.Text = "1024";
            this.textBoxCameraSphericalPosThetaPhi.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonCameraSphericalPosThetaP
            // 
            this.buttonCameraSphericalPosThetaP.Location = new System.Drawing.Point(87, 58);
            this.buttonCameraSphericalPosThetaP.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraSphericalPosThetaP.Name = "buttonCameraSphericalPosThetaP";
            this.buttonCameraSphericalPosThetaP.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraSphericalPosThetaP.TabIndex = 31;
            this.buttonCameraSphericalPosThetaP.Text = "θ+";
            this.buttonCameraSphericalPosThetaP.UseVisualStyleBackColor = true;
            // 
            // buttonCameraSphericalPosThetaPPhiN
            // 
            this.buttonCameraSphericalPosThetaPPhiN.Location = new System.Drawing.Point(87, 16);
            this.buttonCameraSphericalPosThetaPPhiN.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraSphericalPosThetaPPhiN.Name = "buttonCameraSphericalPosThetaPPhiN";
            this.buttonCameraSphericalPosThetaPPhiN.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraSphericalPosThetaPPhiN.TabIndex = 30;
            this.buttonCameraSphericalPosThetaPPhiN.Text = "θ+ϕ-";
            this.buttonCameraSphericalPosThetaPPhiN.UseVisualStyleBackColor = true;
            // 
            // buttonCameraSphericalPosPhiN
            // 
            this.buttonCameraSphericalPosPhiN.Location = new System.Drawing.Point(45, 16);
            this.buttonCameraSphericalPosPhiN.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraSphericalPosPhiN.Name = "buttonCameraSphericalPosPhiN";
            this.buttonCameraSphericalPosPhiN.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraSphericalPosPhiN.TabIndex = 29;
            this.buttonCameraSphericalPosPhiN.Text = "ϕ-";
            this.buttonCameraSphericalPosPhiN.UseVisualStyleBackColor = true;
            // 
            // buttonCameraSphericalPosPhiP
            // 
            this.buttonCameraSphericalPosPhiP.Location = new System.Drawing.Point(45, 100);
            this.buttonCameraSphericalPosPhiP.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraSphericalPosPhiP.Name = "buttonCameraSphericalPosPhiP";
            this.buttonCameraSphericalPosPhiP.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraSphericalPosPhiP.TabIndex = 28;
            this.buttonCameraSphericalPosPhiP.Text = "ϕ+";
            this.buttonCameraSphericalPosPhiP.UseVisualStyleBackColor = true;
            // 
            // buttonCameraSphericalPosThetaNPhiP
            // 
            this.buttonCameraSphericalPosThetaNPhiP.Location = new System.Drawing.Point(3, 100);
            this.buttonCameraSphericalPosThetaNPhiP.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraSphericalPosThetaNPhiP.Name = "buttonCameraSphericalPosThetaNPhiP";
            this.buttonCameraSphericalPosThetaNPhiP.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraSphericalPosThetaNPhiP.TabIndex = 27;
            this.buttonCameraSphericalPosThetaNPhiP.Text = "θ-ϕ+";
            this.buttonCameraSphericalPosThetaNPhiP.UseVisualStyleBackColor = true;
            // 
            // buttonCameraSphericalPosThetaN
            // 
            this.buttonCameraSphericalPosThetaN.Location = new System.Drawing.Point(3, 58);
            this.buttonCameraSphericalPosThetaN.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraSphericalPosThetaN.Name = "buttonCameraSphericalPosThetaN";
            this.buttonCameraSphericalPosThetaN.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraSphericalPosThetaN.TabIndex = 26;
            this.buttonCameraSphericalPosThetaN.Text = "θ-";
            this.buttonCameraSphericalPosThetaN.UseVisualStyleBackColor = true;
            // 
            // buttonCameraSphericalPosThetaNPhiN
            // 
            this.buttonCameraSphericalPosThetaNPhiN.Location = new System.Drawing.Point(3, 16);
            this.buttonCameraSphericalPosThetaNPhiN.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraSphericalPosThetaNPhiN.Name = "buttonCameraSphericalPosThetaNPhiN";
            this.buttonCameraSphericalPosThetaNPhiN.Size = new System.Drawing.Size(42, 42);
            this.buttonCameraSphericalPosThetaNPhiN.TabIndex = 25;
            this.buttonCameraSphericalPosThetaNPhiN.Text = "θ-ϕ-";
            this.buttonCameraSphericalPosThetaNPhiN.UseVisualStyleBackColor = true;
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
            this.groupBoxCameraPos.Location = new System.Drawing.Point(3, 63);
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
            // panelCameraBorder
            // 
            this.panelCameraBorder.Controls.Add(this.pictureBoxCamera);
            this.panelCameraBorder.Location = new System.Drawing.Point(4, 3);
            this.panelCameraBorder.Margin = new System.Windows.Forms.Padding(2);
            this.panelCameraBorder.Name = "panelCameraBorder";
            this.panelCameraBorder.Size = new System.Drawing.Size(57, 55);
            this.panelCameraBorder.TabIndex = 2;
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
            this.NoTearFlowLayoutPanelCamera.Location = new System.Drawing.Point(190, 2);
            this.NoTearFlowLayoutPanelCamera.Margin = new System.Windows.Forms.Padding(2);
            this.NoTearFlowLayoutPanelCamera.Name = "NoTearFlowLayoutPanelCamera";
            this.NoTearFlowLayoutPanelCamera.Size = new System.Drawing.Size(723, 572);
            this.NoTearFlowLayoutPanelCamera.TabIndex = 3;
            // 
            // tabPageTriangles
            // 
            this.tabPageTriangles.Controls.Add(this.groupBoxTrianglePos);
            this.tabPageTriangles.Controls.Add(this.groupBoxTriangleNormal);
            this.tabPageTriangles.Controls.Add(this.buttonAnnihilateTriangle);
            this.tabPageTriangles.Controls.Add(this.buttonNeutralizeTriangle);
            this.tabPageTriangles.Controls.Add(this.buttonGoToVClosest);
            this.tabPageTriangles.Controls.Add(this.checkBoxVertexMisalignment);
            this.tabPageTriangles.Controls.Add(this.buttonRetrieveTriangle);
            this.tabPageTriangles.Controls.Add(this.buttonGoToV3);
            this.tabPageTriangles.Controls.Add(this.buttonGoToV2);
            this.tabPageTriangles.Controls.Add(this.buttonGoToV1);
            this.tabPageTriangles.Controls.Add(this.label4);
            this.tabPageTriangles.Controls.Add(this.maskedTextBoxOtherTriangle);
            this.tabPageTriangles.Controls.Add(this.radioButtonTriOther);
            this.tabPageTriangles.Controls.Add(this.radioButtonTriCeiling);
            this.tabPageTriangles.Controls.Add(this.radioButtonTriWall);
            this.tabPageTriangles.Controls.Add(this.radioButtonTriFloor);
            this.tabPageTriangles.Controls.Add(this.NoTearFlowLayoutPanelTriangles);
            this.tabPageTriangles.Location = new System.Drawing.Point(4, 22);
            this.tabPageTriangles.Name = "tabPageTriangles";
            this.tabPageTriangles.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTriangles.Size = new System.Drawing.Size(915, 576);
            this.tabPageTriangles.TabIndex = 11;
            this.tabPageTriangles.Text = "Triangles";
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
            this.groupBoxTrianglePos.Location = new System.Drawing.Point(3, 222);
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
            this.groupBoxTriangleNormal.Location = new System.Drawing.Point(3, 374);
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
            // buttonAnnihilateTriangle
            // 
            this.buttonAnnihilateTriangle.Location = new System.Drawing.Point(99, 170);
            this.buttonAnnihilateTriangle.Name = "buttonAnnihilateTriangle";
            this.buttonAnnihilateTriangle.Size = new System.Drawing.Size(87, 23);
            this.buttonAnnihilateTriangle.TabIndex = 15;
            this.buttonAnnihilateTriangle.Text = "Annihilate";
            this.buttonAnnihilateTriangle.UseVisualStyleBackColor = true;
            // 
            // buttonNeutralizeTriangle
            // 
            this.buttonNeutralizeTriangle.Location = new System.Drawing.Point(6, 170);
            this.buttonNeutralizeTriangle.Name = "buttonNeutralizeTriangle";
            this.buttonNeutralizeTriangle.Size = new System.Drawing.Size(87, 23);
            this.buttonNeutralizeTriangle.TabIndex = 14;
            this.buttonNeutralizeTriangle.Text = "Neutralize";
            this.buttonNeutralizeTriangle.UseVisualStyleBackColor = true;
            // 
            // buttonGoToVClosest
            // 
            this.buttonGoToVClosest.Location = new System.Drawing.Point(6, 141);
            this.buttonGoToVClosest.Name = "buttonGoToVClosest";
            this.buttonGoToVClosest.Size = new System.Drawing.Size(87, 23);
            this.buttonGoToVClosest.TabIndex = 13;
            this.buttonGoToVClosest.Text = "Goto Closest";
            this.buttonGoToVClosest.UseVisualStyleBackColor = true;
            // 
            // checkBoxVertexMisalignment
            // 
            this.checkBoxVertexMisalignment.AutoSize = true;
            this.checkBoxVertexMisalignment.Location = new System.Drawing.Point(6, 199);
            this.checkBoxVertexMisalignment.Name = "checkBoxVertexMisalignment";
            this.checkBoxVertexMisalignment.Size = new System.Drawing.Size(151, 17);
            this.checkBoxVertexMisalignment.TabIndex = 12;
            this.checkBoxVertexMisalignment.Text = "Vertex Misalignment Offset";
            this.checkBoxVertexMisalignment.UseVisualStyleBackColor = true;
            // 
            // buttonRetrieveTriangle
            // 
            this.buttonRetrieveTriangle.Location = new System.Drawing.Point(99, 141);
            this.buttonRetrieveTriangle.Name = "buttonRetrieveTriangle";
            this.buttonRetrieveTriangle.Size = new System.Drawing.Size(87, 23);
            this.buttonRetrieveTriangle.TabIndex = 11;
            this.buttonRetrieveTriangle.Text = "Retrieve";
            this.buttonRetrieveTriangle.UseVisualStyleBackColor = true;
            // 
            // buttonGoToV3
            // 
            this.buttonGoToV3.Location = new System.Drawing.Point(130, 112);
            this.buttonGoToV3.Name = "buttonGoToV3";
            this.buttonGoToV3.Size = new System.Drawing.Size(56, 23);
            this.buttonGoToV3.TabIndex = 10;
            this.buttonGoToV3.Text = "Goto V3";
            this.buttonGoToV3.UseVisualStyleBackColor = true;
            // 
            // buttonGoToV2
            // 
            this.buttonGoToV2.Location = new System.Drawing.Point(69, 112);
            this.buttonGoToV2.Name = "buttonGoToV2";
            this.buttonGoToV2.Size = new System.Drawing.Size(55, 23);
            this.buttonGoToV2.TabIndex = 9;
            this.buttonGoToV2.Text = "Goto V2";
            this.buttonGoToV2.UseVisualStyleBackColor = true;
            // 
            // buttonGoToV1
            // 
            this.buttonGoToV1.Location = new System.Drawing.Point(6, 112);
            this.buttonGoToV1.Name = "buttonGoToV1";
            this.buttonGoToV1.Size = new System.Drawing.Size(57, 23);
            this.buttonGoToV1.TabIndex = 8;
            this.buttonGoToV1.Text = "Goto V1";
            this.buttonGoToV1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Triangle:";
            // 
            // maskedTextBoxOtherTriangle
            // 
            this.maskedTextBoxOtherTriangle.Location = new System.Drawing.Point(76, 87);
            this.maskedTextBoxOtherTriangle.Margin = new System.Windows.Forms.Padding(2);
            this.maskedTextBoxOtherTriangle.Mask = "\\0xaaAAAAAA";
            this.maskedTextBoxOtherTriangle.Name = "maskedTextBoxOtherTriangle";
            this.maskedTextBoxOtherTriangle.Size = new System.Drawing.Size(87, 20);
            this.maskedTextBoxOtherTriangle.TabIndex = 5;
            // 
            // radioButtonTriOther
            // 
            this.radioButtonTriOther.AutoSize = true;
            this.radioButtonTriOther.Location = new System.Drawing.Point(17, 88);
            this.radioButtonTriOther.Name = "radioButtonTriOther";
            this.radioButtonTriOther.Size = new System.Drawing.Size(54, 17);
            this.radioButtonTriOther.TabIndex = 3;
            this.radioButtonTriOther.Text = "Other:";
            this.radioButtonTriOther.UseVisualStyleBackColor = true;
            // 
            // radioButtonTriCeiling
            // 
            this.radioButtonTriCeiling.AutoSize = true;
            this.radioButtonTriCeiling.Location = new System.Drawing.Point(17, 65);
            this.radioButtonTriCeiling.Name = "radioButtonTriCeiling";
            this.radioButtonTriCeiling.Size = new System.Drawing.Size(56, 17);
            this.radioButtonTriCeiling.TabIndex = 2;
            this.radioButtonTriCeiling.Text = "Ceiling";
            this.radioButtonTriCeiling.UseVisualStyleBackColor = true;
            // 
            // radioButtonTriWall
            // 
            this.radioButtonTriWall.AutoSize = true;
            this.radioButtonTriWall.Location = new System.Drawing.Point(17, 42);
            this.radioButtonTriWall.Name = "radioButtonTriWall";
            this.radioButtonTriWall.Size = new System.Drawing.Size(46, 17);
            this.radioButtonTriWall.TabIndex = 1;
            this.radioButtonTriWall.Text = "Wall";
            this.radioButtonTriWall.UseVisualStyleBackColor = true;
            // 
            // radioButtonTriFloor
            // 
            this.radioButtonTriFloor.AutoSize = true;
            this.radioButtonTriFloor.Checked = true;
            this.radioButtonTriFloor.Location = new System.Drawing.Point(17, 19);
            this.radioButtonTriFloor.Name = "radioButtonTriFloor";
            this.radioButtonTriFloor.Size = new System.Drawing.Size(48, 17);
            this.radioButtonTriFloor.TabIndex = 0;
            this.radioButtonTriFloor.TabStop = true;
            this.radioButtonTriFloor.Text = "Floor";
            this.radioButtonTriFloor.UseVisualStyleBackColor = true;
            // 
            // NoTearFlowLayoutPanelTriangles
            // 
            this.NoTearFlowLayoutPanelTriangles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NoTearFlowLayoutPanelTriangles.AutoScroll = true;
            this.NoTearFlowLayoutPanelTriangles.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.NoTearFlowLayoutPanelTriangles.Location = new System.Drawing.Point(191, 5);
            this.NoTearFlowLayoutPanelTriangles.Margin = new System.Windows.Forms.Padding(2);
            this.NoTearFlowLayoutPanelTriangles.Name = "NoTearFlowLayoutPanelTriangles";
            this.NoTearFlowLayoutPanelTriangles.Size = new System.Drawing.Size(719, 566);
            this.NoTearFlowLayoutPanelTriangles.TabIndex = 7;
            // 
            // tabPageWater
            // 
            this.tabPageWater.Controls.Add(this.noTearFlowLayoutPanelWater);
            this.tabPageWater.Location = new System.Drawing.Point(4, 22);
            this.tabPageWater.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageWater.Name = "tabPageWater";
            this.tabPageWater.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageWater.Size = new System.Drawing.Size(915, 576);
            this.tabPageWater.TabIndex = 14;
            this.tabPageWater.Text = "Water";
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
            this.noTearFlowLayoutPanelWater.Size = new System.Drawing.Size(911, 570);
            this.noTearFlowLayoutPanelWater.TabIndex = 2;
            // 
            // tabPageController
            // 
            this.tabPageController.Controls.Add(this.pictureBoxController);
            this.tabPageController.Controls.Add(this.NoTearFlowLayoutPanelController);
            this.tabPageController.Location = new System.Drawing.Point(4, 22);
            this.tabPageController.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageController.Name = "tabPageController";
            this.tabPageController.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageController.Size = new System.Drawing.Size(915, 576);
            this.tabPageController.TabIndex = 14;
            this.tabPageController.Text = "Controller";
            // 
            // pictureBoxController
            // 
            this.pictureBoxController.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxController.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            this.pictureBoxController.Location = new System.Drawing.Point(19, 4);
            this.pictureBoxController.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBoxController.MaximumSize = new System.Drawing.Size(300, 300);
            this.pictureBoxController.Name = "pictureBoxController";
            this.pictureBoxController.Size = new System.Drawing.Size(300, 300);
            this.pictureBoxController.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxController.TabIndex = 3;
            this.pictureBoxController.TabStop = false;
            // 
            // NoTearFlowLayoutPanelController
            // 
            this.NoTearFlowLayoutPanelController.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NoTearFlowLayoutPanelController.AutoScroll = true;
            this.NoTearFlowLayoutPanelController.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.NoTearFlowLayoutPanelController.Location = new System.Drawing.Point(339, 2);
            this.NoTearFlowLayoutPanelController.Margin = new System.Windows.Forms.Padding(2);
            this.NoTearFlowLayoutPanelController.Name = "NoTearFlowLayoutPanelController";
            this.NoTearFlowLayoutPanelController.Size = new System.Drawing.Size(574, 570);
            this.NoTearFlowLayoutPanelController.TabIndex = 2;
            // 
            // tabPageFile
            // 
            this.tabPageFile.Controls.Add(this.noTearFlowLayoutPanelFile);
            this.tabPageFile.Location = new System.Drawing.Point(4, 22);
            this.tabPageFile.Name = "tabPageFile";
            this.tabPageFile.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFile.Size = new System.Drawing.Size(915, 576);
            this.tabPageFile.TabIndex = 10;
            this.tabPageFile.Text = "File";
            // 
            // noTearFlowLayoutPanelFile
            // 
            this.noTearFlowLayoutPanelFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.noTearFlowLayoutPanelFile.AutoScroll = true;
            this.noTearFlowLayoutPanelFile.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.noTearFlowLayoutPanelFile.Location = new System.Drawing.Point(5, 5);
            this.noTearFlowLayoutPanelFile.Margin = new System.Windows.Forms.Padding(2);
            this.noTearFlowLayoutPanelFile.Name = "noTearFlowLayoutPanelFile";
            this.noTearFlowLayoutPanelFile.Size = new System.Drawing.Size(905, 566);
            this.noTearFlowLayoutPanelFile.TabIndex = 2;
            // 
            // tabPageLevel
            // 
            this.tabPageLevel.Controls.Add(this.noTearFlowLayoutPanelLevel);
            this.tabPageLevel.Location = new System.Drawing.Point(4, 22);
            this.tabPageLevel.Name = "tabPageLevel";
            this.tabPageLevel.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLevel.Size = new System.Drawing.Size(915, 576);
            this.tabPageLevel.TabIndex = 16;
            this.tabPageLevel.Text = "Level";
            // 
            // noTearFlowLayoutPanelLevel
            // 
            this.noTearFlowLayoutPanelLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.noTearFlowLayoutPanelLevel.AutoScroll = true;
            this.noTearFlowLayoutPanelLevel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.noTearFlowLayoutPanelLevel.Location = new System.Drawing.Point(5, 5);
            this.noTearFlowLayoutPanelLevel.Margin = new System.Windows.Forms.Padding(2);
            this.noTearFlowLayoutPanelLevel.Name = "noTearFlowLayoutPanelLevel";
            this.noTearFlowLayoutPanelLevel.Size = new System.Drawing.Size(905, 566);
            this.noTearFlowLayoutPanelLevel.TabIndex = 2;
            // 
            // tabPageMisc
            // 
            this.tabPageMisc.Controls.Add(this.panelMiscBorder);
            this.tabPageMisc.Controls.Add(this.NoTearFlowLayoutPanelMisc);
            this.tabPageMisc.Location = new System.Drawing.Point(4, 22);
            this.tabPageMisc.Name = "tabPageMisc";
            this.tabPageMisc.Size = new System.Drawing.Size(915, 576);
            this.tabPageMisc.TabIndex = 9;
            this.tabPageMisc.Text = "Misc.";
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
            this.NoTearFlowLayoutPanelMisc.Size = new System.Drawing.Size(849, 569);
            this.NoTearFlowLayoutPanelMisc.TabIndex = 5;
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
            this.tabPageDebug.Size = new System.Drawing.Size(915, 576);
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
            // tabPageMap
            // 
            this.tabPageMap.Controls.Add(this.splitContainerMap);
            this.tabPageMap.Location = new System.Drawing.Point(4, 22);
            this.tabPageMap.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageMap.Name = "tabPageMap";
            this.tabPageMap.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageMap.Size = new System.Drawing.Size(915, 576);
            this.tabPageMap.TabIndex = 4;
            this.tabPageMap.Text = "Map";
            // 
            // splitContainerMap
            // 
            this.splitContainerMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
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
            // 
            // splitContainerMap.Panel2
            // 
            this.splitContainerMap.Panel2.Controls.Add(this.glControlMap);
            this.splitContainerMap.Panel2.Controls.Add(this.labelMapPu);
            this.splitContainerMap.Panel2.Controls.Add(this.labelMapPuValue);
            this.splitContainerMap.Panel2.Controls.Add(this.labelMapQpu);
            this.splitContainerMap.Panel2.Controls.Add(this.labelMapQpuValue);
            this.splitContainerMap.Size = new System.Drawing.Size(908, 569);
            this.splitContainerMap.SplitterDistance = 299;
            this.splitContainerMap.SplitterWidth = 3;
            this.splitContainerMap.TabIndex = 16;
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
            this.labelMapId.Location = new System.Drawing.Point(185, 550);
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
            this.glControlMap.Size = new System.Drawing.Size(623, 543);
            this.glControlMap.TabIndex = 0;
            this.glControlMap.VSync = false;
            this.glControlMap.Load += new System.EventHandler(this.glControlMap_Load);
            // 
            // labelMapPu
            // 
            this.labelMapPu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelMapPu.AutoSize = true;
            this.labelMapPu.Location = new System.Drawing.Point(2, 551);
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
            this.labelMapPuValue.Location = new System.Drawing.Point(67, 551);
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
            this.labelMapQpu.Location = new System.Drawing.Point(123, 551);
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
            this.labelMapQpuValue.Location = new System.Drawing.Point(196, 551);
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
            this.tabPagePu.Size = new System.Drawing.Size(915, 576);
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
            this.groupBoxPuController.Size = new System.Drawing.Size(222, 567);
            this.groupBoxPuController.TabIndex = 7;
            this.groupBoxPuController.TabStop = false;
            this.groupBoxPuController.Text = "PU Controller";
            // 
            // labelPuConPu
            // 
            this.labelPuConPu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelPuConPu.AutoSize = true;
            this.labelPuConPu.Location = new System.Drawing.Point(5, 551);
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
            this.labelPuConQpuValue.Location = new System.Drawing.Point(174, 551);
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
            this.labelPuConQpu.Location = new System.Drawing.Point(111, 551);
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
            this.labelPuConPuValue.Location = new System.Drawing.Point(60, 551);
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
            this.tabPageExpressions.Size = new System.Drawing.Size(915, 576);
            this.tabPageExpressions.TabIndex = 2;
            this.tabPageExpressions.Text = "Expressions";
            // 
            // checkBoxAbsoluteAddress
            // 
            this.checkBoxAbsoluteAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxAbsoluteAddress.AutoSize = true;
            this.checkBoxAbsoluteAddress.Location = new System.Drawing.Point(780, 443);
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
            this.buttonOtherDelete.Location = new System.Drawing.Point(106, 441);
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
            this.buttonOtherModify.Location = new System.Drawing.Point(54, 441);
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
            this.buttonOtherAdd.Location = new System.Drawing.Point(2, 441);
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
            this.dataGridViewExpressions.Size = new System.Drawing.Size(913, 434);
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
            this.tabPageDisassembly.Size = new System.Drawing.Size(915, 576);
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
            this.richTextBoxDissasembly.Size = new System.Drawing.Size(910, 551);
            this.richTextBoxDissasembly.TabIndex = 0;
            this.richTextBoxDissasembly.Text = "";
            // 
            // tabPageHacks
            // 
            this.tabPageHacks.Controls.Add(this.splitContainerHacks);
            this.tabPageHacks.Location = new System.Drawing.Point(4, 22);
            this.tabPageHacks.Name = "tabPageHacks";
            this.tabPageHacks.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHacks.Size = new System.Drawing.Size(915, 576);
            this.tabPageHacks.TabIndex = 12;
            this.tabPageHacks.Text = "Hacks";
            // 
            // splitContainerHacks
            // 
            this.splitContainerHacks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerHacks.Location = new System.Drawing.Point(6, 3);
            this.splitContainerHacks.Name = "splitContainerHacks";
            // 
            // splitContainerHacks.Panel1
            // 
            this.splitContainerHacks.Panel1.Controls.Add(this.groupBoxHackRam);
            // 
            // splitContainerHacks.Panel2
            // 
            this.splitContainerHacks.Panel2.Controls.Add(this.groupBoxHackSpawn);
            this.splitContainerHacks.Size = new System.Drawing.Size(906, 567);
            this.splitContainerHacks.SplitterDistance = 301;
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
            this.groupBoxHackRam.Size = new System.Drawing.Size(295, 561);
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
            this.checkedListBoxHacks.Size = new System.Drawing.Size(283, 469);
            this.checkedListBoxHacks.TabIndex = 9;
            // 
            // groupBoxHackSpawn
            // 
            this.groupBoxHackSpawn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.groupBoxHackSpawn.Size = new System.Drawing.Size(594, 560);
            this.groupBoxHackSpawn.TabIndex = 0;
            this.groupBoxHackSpawn.TabStop = false;
            this.groupBoxHackSpawn.Text = "Spawner";
            // 
            // labelSpawnHint
            // 
            this.labelSpawnHint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelSpawnHint.AutoSize = true;
            this.labelSpawnHint.Location = new System.Drawing.Point(315, 539);
            this.labelSpawnHint.Name = "labelSpawnHint";
            this.labelSpawnHint.Size = new System.Drawing.Size(127, 13);
            this.labelSpawnHint.TabIndex = 25;
            this.labelSpawnHint.Text = "(Press L button to spawn)";
            // 
            // buttonSpawnReset
            // 
            this.buttonSpawnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSpawnReset.Location = new System.Drawing.Point(478, 535);
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
            this.labelSpawnExtra.Location = new System.Drawing.Point(109, 538);
            this.labelSpawnExtra.Name = "labelSpawnExtra";
            this.labelSpawnExtra.Size = new System.Drawing.Size(34, 13);
            this.labelSpawnExtra.TabIndex = 23;
            this.labelSpawnExtra.Text = "Extra:";
            // 
            // labelSpawnGfxId
            // 
            this.labelSpawnGfxId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelSpawnGfxId.AutoSize = true;
            this.labelSpawnGfxId.Location = new System.Drawing.Point(6, 539);
            this.labelSpawnGfxId.Name = "labelSpawnGfxId";
            this.labelSpawnGfxId.Size = new System.Drawing.Size(45, 13);
            this.labelSpawnGfxId.TabIndex = 22;
            this.labelSpawnGfxId.Text = "GFX ID:";
            // 
            // textBoxSpawnExtra
            // 
            this.textBoxSpawnExtra.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxSpawnExtra.Location = new System.Drawing.Point(149, 535);
            this.textBoxSpawnExtra.Name = "textBoxSpawnExtra";
            this.textBoxSpawnExtra.Size = new System.Drawing.Size(46, 20);
            this.textBoxSpawnExtra.TabIndex = 21;
            // 
            // textBoxSpawnGfxId
            // 
            this.textBoxSpawnGfxId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxSpawnGfxId.Location = new System.Drawing.Point(57, 535);
            this.textBoxSpawnGfxId.Name = "textBoxSpawnGfxId";
            this.textBoxSpawnGfxId.Size = new System.Drawing.Size(46, 20);
            this.textBoxSpawnGfxId.TabIndex = 20;
            // 
            // buttonHackSpawn
            // 
            this.buttonHackSpawn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonHackSpawn.Location = new System.Drawing.Point(200, 535);
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
            this.listBoxSpawn.Size = new System.Drawing.Size(582, 472);
            this.listBoxSpawn.Sorted = true;
            this.listBoxSpawn.TabIndex = 12;
            // 
            // tabPageCamHack
            // 
            this.tabPageCamHack.Controls.Add(this.noTearFlowLayoutPanelCamHack);
            this.tabPageCamHack.Location = new System.Drawing.Point(4, 22);
            this.tabPageCamHack.Name = "tabPageCamHack";
            this.tabPageCamHack.Size = new System.Drawing.Size(915, 576);
            this.tabPageCamHack.TabIndex = 17;
            this.tabPageCamHack.Text = "Cam Hack";
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
            this.noTearFlowLayoutPanelCamHack.Size = new System.Drawing.Size(911, 572);
            this.noTearFlowLayoutPanelCamHack.TabIndex = 2;
            // 
            // tabPageOptions
            // 
            this.tabPageOptions.Controls.Add(this.groupBoxShowOverlay);
            this.tabPageOptions.Controls.Add(this.checkBoxScaleDiagonalPositionControllerButtons);
            this.tabPageOptions.Controls.Add(this.label3);
            this.tabPageOptions.Controls.Add(this.checkBoxMoveCamWithPu);
            this.tabPageOptions.Controls.Add(this.checkBoxUseRomHack);
            this.tabPageOptions.Controls.Add(this.checkBoxStartSlotIndexOne);
            this.tabPageOptions.Location = new System.Drawing.Point(4, 22);
            this.tabPageOptions.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageOptions.Name = "tabPageOptions";
            this.tabPageOptions.Size = new System.Drawing.Size(915, 576);
            this.tabPageOptions.TabIndex = 5;
            this.tabPageOptions.Text = "Options";
            // 
            // groupBoxShowOverlay
            // 
            this.groupBoxShowOverlay.Controls.Add(this.checkBoxShowOverlayUsedObject);
            this.groupBoxShowOverlay.Controls.Add(this.checkBoxShowOverlayInteractionObject);
            this.groupBoxShowOverlay.Controls.Add(this.checkBoxShowOverlayCameraSecondaryObject);
            this.groupBoxShowOverlay.Controls.Add(this.checkBoxShowOverlayClosestObject);
            this.groupBoxShowOverlay.Controls.Add(this.checkBoxShowOverlayStoodOnObject);
            this.groupBoxShowOverlay.Controls.Add(this.checkBoxShowOverlayHeldObject);
            this.groupBoxShowOverlay.Location = new System.Drawing.Point(256, 4);
            this.groupBoxShowOverlay.Name = "groupBoxShowOverlay";
            this.groupBoxShowOverlay.Size = new System.Drawing.Size(170, 154);
            this.groupBoxShowOverlay.TabIndex = 29;
            this.groupBoxShowOverlay.TabStop = false;
            this.groupBoxShowOverlay.Text = "Object Overlays to Show";
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
            // checkBoxShowOverlayCameraSecondaryObject
            // 
            this.checkBoxShowOverlayCameraSecondaryObject.AutoSize = true;
            this.checkBoxShowOverlayCameraSecondaryObject.Checked = true;
            this.checkBoxShowOverlayCameraSecondaryObject.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowOverlayCameraSecondaryObject.Location = new System.Drawing.Point(5, 104);
            this.checkBoxShowOverlayCameraSecondaryObject.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxShowOverlayCameraSecondaryObject.Name = "checkBoxShowOverlayCameraSecondaryObject";
            this.checkBoxShowOverlayCameraSecondaryObject.Size = new System.Drawing.Size(150, 17);
            this.checkBoxShowOverlayCameraSecondaryObject.TabIndex = 5;
            this.checkBoxShowOverlayCameraSecondaryObject.Text = "Camera Secondary Object";
            this.checkBoxShowOverlayCameraSecondaryObject.UseVisualStyleBackColor = true;
            this.checkBoxShowOverlayCameraSecondaryObject.CheckedChanged += new System.EventHandler(this.checkBoxShowOverlayCameraSecondaryObject_CheckedChanged);
            // 
            // checkBoxShowOverlayClosestObject
            // 
            this.checkBoxShowOverlayClosestObject.AutoSize = true;
            this.checkBoxShowOverlayClosestObject.Checked = true;
            this.checkBoxShowOverlayClosestObject.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowOverlayClosestObject.Location = new System.Drawing.Point(5, 125);
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
            this.checkBoxScaleDiagonalPositionControllerButtons.Location = new System.Drawing.Point(3, 64);
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
            this.label3.Location = new System.Drawing.Point(3, 131);
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
            this.checkBoxMoveCamWithPu.Location = new System.Drawing.Point(3, 44);
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
            this.checkBoxUseRomHack.Location = new System.Drawing.Point(3, 24);
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
            this.checkBoxStartSlotIndexOne.Location = new System.Drawing.Point(3, 4);
            this.checkBoxStartSlotIndexOne.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxStartSlotIndexOne.Name = "checkBoxStartSlotIndexOne";
            this.checkBoxStartSlotIndexOne.Size = new System.Drawing.Size(133, 17);
            this.checkBoxStartSlotIndexOne.TabIndex = 0;
            this.checkBoxStartSlotIndexOne.Text = "Start Slot Index From 1";
            this.checkBoxStartSlotIndexOne.UseVisualStyleBackColor = true;
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
            // buttonCollapseTop
            // 
            this.buttonCollapseTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCollapseTop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonCollapseTop.BackgroundImage")));
            this.buttonCollapseTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonCollapseTop.Location = new System.Drawing.Point(865, 11);
            this.buttonCollapseTop.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCollapseTop.Name = "buttonCollapseTop";
            this.buttonCollapseTop.Size = new System.Drawing.Size(24, 21);
            this.buttonCollapseTop.TabIndex = 19;
            this.buttonCollapseTop.UseVisualStyleBackColor = true;
            this.buttonCollapseTop.Click += new System.EventHandler(this.buttonCollapseTop_Click);
            // 
            // buttonCollapseBottom
            // 
            this.buttonCollapseBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCollapseBottom.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonCollapseBottom.BackgroundImage")));
            this.buttonCollapseBottom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonCollapseBottom.Location = new System.Drawing.Point(837, 11);
            this.buttonCollapseBottom.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCollapseBottom.Name = "buttonCollapseBottom";
            this.buttonCollapseBottom.Size = new System.Drawing.Size(24, 21);
            this.buttonCollapseBottom.TabIndex = 20;
            this.buttonCollapseBottom.UseVisualStyleBackColor = true;
            this.buttonCollapseBottom.Click += new System.EventHandler(this.buttonCollapseBottom_Click);
            // 
            // buttonReadOnly
            // 
            this.buttonReadOnly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReadOnly.Location = new System.Drawing.Point(701, 11);
            this.buttonReadOnly.Margin = new System.Windows.Forms.Padding(2);
            this.buttonReadOnly.Name = "buttonReadOnly";
            this.buttonReadOnly.Size = new System.Drawing.Size(132, 21);
            this.buttonReadOnly.TabIndex = 21;
            this.buttonReadOnly.Tag = "";
            this.buttonReadOnly.Text = "Disable Read-only";
            this.buttonReadOnly.UseVisualStyleBackColor = true;
            this.buttonReadOnly.Click += new System.EventHandler(this.buttonReadOnly_Click);
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
            // StroopMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(947, 741);
            this.Controls.Add(this.panelConnect);
            this.Controls.Add(this.buttonDisconnect);
            this.Controls.Add(this.labelVersionNumber);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.labelProcessSelect);
            this.Controls.Add(this.labelFpsCounter);
            this.Controls.Add(this.buttonCollapseBottom);
            this.Controls.Add(this.buttonCollapseTop);
            this.Controls.Add(this.buttonReadOnly);
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
            this.tabPageObjects.PerformLayout();
            this.panelObj.ResumeLayout(false);
            this.groupBoxObjHome.ResumeLayout(false);
            this.groupBoxObjHome.PerformLayout();
            this.groupBoxObjAngle.ResumeLayout(false);
            this.groupBoxObjAngle.PerformLayout();
            this.groupBoxObjPos.ResumeLayout(false);
            this.groupBoxObjPos.PerformLayout();
            this.panelObjectBorder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxObject)).EndInit();
            this.tabPageMario.ResumeLayout(false);
            this.groupBoxMarioStats.ResumeLayout(false);
            this.groupBoxMarioStats.PerformLayout();
            this.groupBoxMarioHOLP.ResumeLayout(false);
            this.groupBoxMarioHOLP.PerformLayout();
            this.groupBoxMarioPos.ResumeLayout(false);
            this.groupBoxMarioPos.PerformLayout();
            this.panelMarioBorder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMario)).EndInit();
            this.tabPageActions.ResumeLayout(false);
            this.tabPageHud.ResumeLayout(false);
            this.panelHudBorder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHud)).EndInit();
            this.tabPageCamera.ResumeLayout(false);
            this.groupBoxCameraSphericalPos.ResumeLayout(false);
            this.groupBoxCameraSphericalPos.PerformLayout();
            this.groupBoxCameraPos.ResumeLayout(false);
            this.groupBoxCameraPos.PerformLayout();
            this.panelCameraBorder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCamera)).EndInit();
            this.tabPageTriangles.ResumeLayout(false);
            this.tabPageTriangles.PerformLayout();
            this.groupBoxTrianglePos.ResumeLayout(false);
            this.groupBoxTrianglePos.PerformLayout();
            this.groupBoxTriangleNormal.ResumeLayout(false);
            this.groupBoxTriangleNormal.PerformLayout();
            this.tabPageWater.ResumeLayout(false);
            this.tabPageController.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxController)).EndInit();
            this.tabPageFile.ResumeLayout(false);
            this.tabPageLevel.ResumeLayout(false);
            this.tabPageMisc.ResumeLayout(false);
            this.panelMiscBorder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMisc)).EndInit();
            this.tabPageDebug.ResumeLayout(false);
            this.tabPageDebug.PerformLayout();
            this.panelDebugBorder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDebug)).EndInit();
            this.NoTearFlowLayoutPanelDebugDisplayType.ResumeLayout(false);
            this.NoTearFlowLayoutPanelDebugDisplayType.PerformLayout();
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
            this.tabPageOptions.ResumeLayout(false);
            this.tabPageOptions.PerformLayout();
            this.groupBoxShowOverlay.ResumeLayout(false);
            this.groupBoxShowOverlay.PerformLayout();
            this.panelConnect.ResumeLayout(false);
            this.panelConnect.PerformLayout();
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
        private Label label4;
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
        private Button buttonCollapseTop;
        private Button buttonCollapseBottom;
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
        private TabPage tabPageLevel;
        private TabPage tabPageCamHack;
        private NoTearFlowLayoutPanel noTearFlowLayoutPanelFile;
        private NoTearFlowLayoutPanel noTearFlowLayoutPanelLevel;
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
        private Button buttonObjDebilitate;
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
        private TextBox textBoxCameraSphericalPosRadius;
        private Button buttonCameraSphericalPosRadiusN;
        private Button buttonCameraSphericalPosRadiusP;
        private Button buttonCameraSphericalPosThetaPPhiP;
        private TextBox textBoxCameraSphericalPosThetaPhi;
        private Button buttonCameraSphericalPosThetaP;
        private Button buttonCameraSphericalPosThetaPPhiN;
        private Button buttonCameraSphericalPosPhiN;
        private Button buttonCameraSphericalPosPhiP;
        private Button buttonCameraSphericalPosThetaNPhiP;
        private Button buttonCameraSphericalPosThetaN;
        private Button buttonCameraSphericalPosThetaNPhiN;
        private CheckBox checkBoxScaleDiagonalPositionControllerButtons;
        private CheckBox checkBoxMarioPosRelative;
        private CheckBox checkBoxMarioHOLPRelative;
        private CheckBox checkBoxObjHomeRelative;
        private CheckBox checkBoxObjPosRelative;
        private CheckBox checkBoxTrianglePosRelative;
        private IntPictureBox pictureBoxController;
        private CheckBox checkBoxCameraPosRelative;
        private CheckBox checkBoxCameraSphericalPosRelative;
        private Button buttonRefreshAndConnect;
        private GroupBox groupBoxShowOverlay;
        private CheckBox checkBoxShowOverlayClosestObject;
        private CheckBox checkBoxShowOverlayStoodOnObject;
        private CheckBox checkBoxShowOverlayHeldObject;
        private CheckBox checkBoxShowOverlayUsedObject;
        private CheckBox checkBoxShowOverlayInteractionObject;
        private CheckBox checkBoxShowOverlayCameraSecondaryObject;
        private CheckBox checkBoxMapShowCeiling;
    }
}


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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.groupBoxObjPos = new System.Windows.Forms.GroupBox();
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
            this.buttonMarioVisibility = new System.Windows.Forms.Button();
            this.buttonMarioToggleHandsfree = new System.Windows.Forms.Button();
            this.panelMarioBorder = new System.Windows.Forms.Panel();
            this.pictureBoxMario = new SM64_Diagnostic.Controls.IntPictureBox();
            this.NoTearFlowLayoutPanelMario = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.tabPageHud = new System.Windows.Forms.TabPage();
            this.buttonStandardHud = new System.Windows.Forms.Button();
            this.buttonDie = new System.Windows.Forms.Button();
            this.buttonFillHp = new System.Windows.Forms.Button();
            this.panelHudBorder = new System.Windows.Forms.Panel();
            this.pictureBoxHud = new SM64_Diagnostic.Controls.IntPictureBox();
            this.NoTearFlowLayoutPanelHud = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.tabPageActions = new System.Windows.Forms.TabPage();
            this.noTearFlowLayoutPanelActions = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.tabPageCamera = new System.Windows.Forms.TabPage();
            this.NoTearFlowLayoutPanelCamera = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.panelCameraBorder = new System.Windows.Forms.Panel();
            this.pictureBoxCamera = new SM64_Diagnostic.Controls.IntPictureBox();
            this.tabPageWater = new System.Windows.Forms.TabPage();
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
            this.tabPageMisc = new System.Windows.Forms.TabPage();
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
            this.panelMiscBorder = new System.Windows.Forms.Panel();
            this.pictureBoxMisc = new SM64_Diagnostic.Controls.IntPictureBox();
            this.NoTearFlowLayoutPanelMisc = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.tabPageTriangles = new System.Windows.Forms.TabPage();
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
            this.tabPageStars = new System.Windows.Forms.TabPage();
            this.NoTearFlowLayoutPanel1 = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.radioButtonFileD = new System.Windows.Forms.RadioButton();
            this.radioButtonFileC = new System.Windows.Forms.RadioButton();
            this.radioButtonFileB = new System.Windows.Forms.RadioButton();
            this.radioButtonFileA = new System.Windows.Forms.RadioButton();
            this.tabPageMap = new System.Windows.Forms.TabPage();
            this.splitContainerMap = new System.Windows.Forms.SplitContainer();
            this.checkBoxMapShowFloor = new System.Windows.Forms.CheckBox();
            this.checkBoxMapShowCamera = new System.Windows.Forms.CheckBox();
            this.checkBoxMapShowHolp = new System.Windows.Forms.CheckBox();
            this.checkBoxMapShowObj = new System.Windows.Forms.CheckBox();
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
            this.tabPageOptions = new System.Windows.Forms.TabPage();
            this.checkBoxUseOverlays = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBoxMoveCamWithPu = new System.Windows.Forms.CheckBox();
            this.checkBoxUseRomHack = new System.Windows.Forms.CheckBox();
            this.checkBoxStartSlotIndexOne = new System.Windows.Forms.CheckBox();
            this.labelVersionNumber = new System.Windows.Forms.Label();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.panelConnect = new System.Windows.Forms.Panel();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.labelNotConnected = new System.Windows.Forms.Label();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.listBoxProcessesList = new System.Windows.Forms.ListBox();
            this.labelFpsCounter = new System.Windows.Forms.Label();
            this.buttonCollapseTop = new System.Windows.Forms.Button();
            this.buttonCollapseBottom = new System.Windows.Forms.Button();
            this.buttonReadOnly = new System.Windows.Forms.Button();
            this.noTearFlowLayoutPanelWater = new SM64_Diagnostic.Controls.NoTearFlowLayoutPanel();
            this.groupBoxObjects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarObjSlotSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageObjects.SuspendLayout();
            this.panelObj.SuspendLayout();
            this.groupBoxObjPos.SuspendLayout();
            this.panelObjectBorder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxObject)).BeginInit();
            this.tabPageMario.SuspendLayout();
            this.panelMarioBorder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMario)).BeginInit();
            this.tabPageHud.SuspendLayout();
            this.panelHudBorder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHud)).BeginInit();
            this.tabPageActions.SuspendLayout();
            this.tabPageCamera.SuspendLayout();
            this.panelCameraBorder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCamera)).BeginInit();
            this.tabPageWater.SuspendLayout();
            this.tabPageDebug.SuspendLayout();
            this.panelDebugBorder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDebug)).BeginInit();
            this.NoTearFlowLayoutPanelDebugDisplayType.SuspendLayout();
            this.tabPageMisc.SuspendLayout();
            this.groupBoxPuController.SuspendLayout();
            this.panelMiscBorder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMisc)).BeginInit();
            this.tabPageTriangles.SuspendLayout();
            this.tabPageStars.SuspendLayout();
            this.tabPageMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMap)).BeginInit();
            this.splitContainerMap.Panel1.SuspendLayout();
            this.splitContainerMap.Panel2.SuspendLayout();
            this.splitContainerMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMapIconSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMapZoom)).BeginInit();
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
            this.tabPageOptions.SuspendLayout();
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
            this.groupBoxObjects.Size = new System.Drawing.Size(1020, 486);
            this.groupBoxObjects.TabIndex = 2;
            this.groupBoxObjects.TabStop = false;
            this.groupBoxObjects.Text = "Objects";
            // 
            // comboBoxLabelMethod
            // 
            this.comboBoxLabelMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxLabelMethod.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBoxLabelMethod.Location = new System.Drawing.Point(738, 15);
            this.comboBoxLabelMethod.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxLabelMethod.Name = "comboBoxLabelMethod";
            this.comboBoxLabelMethod.Size = new System.Drawing.Size(102, 21);
            this.comboBoxLabelMethod.TabIndex = 13;
            // 
            // labelLabelMethod
            // 
            this.labelLabelMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLabelMethod.AutoSize = true;
            this.labelLabelMethod.Location = new System.Drawing.Point(658, 18);
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
            this.labelToggleMode.Location = new System.Drawing.Point(454, 19);
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
            this.comboBoxMapToggleMode.Location = new System.Drawing.Point(531, 15);
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
            this.labelSortMethod.Location = new System.Drawing.Point(844, 18);
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
            this.NoTearFlowLayoutPanelObjects.Size = new System.Drawing.Size(1012, 437);
            this.NoTearFlowLayoutPanelObjects.TabIndex = 0;
            this.NoTearFlowLayoutPanelObjects.Resize += new System.EventHandler(this.NoTearFlowLayoutPanelObjects_Resize);
            // 
            // comboBoxSortMethod
            // 
            this.comboBoxSortMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSortMethod.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBoxSortMethod.Location = new System.Drawing.Point(914, 15);
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
            this.splitContainerMain.Size = new System.Drawing.Size(1024, 698);
            this.splitContainerMain.SplitterDistance = 230;
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
            this.tabControlMain.Controls.Add(this.tabPageHud);
            this.tabControlMain.Controls.Add(this.tabPageActions);
            this.tabControlMain.Controls.Add(this.tabPageCamera);
            this.tabControlMain.Controls.Add(this.tabPageWater);
            this.tabControlMain.Controls.Add(this.tabPageDebug);
            this.tabControlMain.Controls.Add(this.tabPageMisc);
            this.tabControlMain.Controls.Add(this.tabPageTriangles);
            this.tabControlMain.Controls.Add(this.tabPageStars);
            this.tabControlMain.Controls.Add(this.tabPageMap);
            this.tabControlMain.Controls.Add(this.tabPageExpressions);
            this.tabControlMain.Controls.Add(this.tabPageDisassembly);
            this.tabControlMain.Controls.Add(this.tabPageHacks);
            this.tabControlMain.Controls.Add(this.tabPageOptions);
            this.tabControlMain.HotTrack = true;
            this.tabControlMain.Location = new System.Drawing.Point(2, 2);
            this.tabControlMain.Margin = new System.Windows.Forms.Padding(2);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1020, 228);
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
            this.tabPageObjects.Size = new System.Drawing.Size(1012, 202);
            this.tabPageObjects.TabIndex = 0;
            this.tabPageObjects.Text = "Object";
            // 
            // panelObj
            // 
            this.panelObj.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panelObj.AutoScroll = true;
            this.panelObj.Controls.Add(this.groupBoxObjPos);
            this.panelObj.Controls.Add(this.buttonObjGoToHome);
            this.panelObj.Controls.Add(this.buttonObjRetrieve);
            this.panelObj.Controls.Add(this.buttonObjRetrieveHome);
            this.panelObj.Controls.Add(this.buttonObjGoTo);
            this.panelObj.Controls.Add(this.buttonObjClone);
            this.panelObj.Controls.Add(this.buttonObjUnload);
            this.panelObj.Location = new System.Drawing.Point(3, 88);
            this.panelObj.Name = "panelObj";
            this.panelObj.Size = new System.Drawing.Size(211, 109);
            this.panelObj.TabIndex = 19;
            // 
            // groupBoxObjPos
            // 
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
            this.groupBoxObjPos.Location = new System.Drawing.Point(3, 78);
            this.groupBoxObjPos.Name = "groupBoxObjPos";
            this.groupBoxObjPos.Size = new System.Drawing.Size(185, 146);
            this.groupBoxObjPos.TabIndex = 28;
            this.groupBoxObjPos.TabStop = false;
            this.groupBoxObjPos.Text = "Position";
            // 
            // textBoxObjPosY
            // 
            this.textBoxObjPosY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxObjPosY.Location = new System.Drawing.Point(143, 70);
            this.textBoxObjPosY.Name = "textBoxObjPosY";
            this.textBoxObjPosY.Size = new System.Drawing.Size(36, 20);
            this.textBoxObjPosY.TabIndex = 33;
            this.textBoxObjPosY.Text = "100";
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
            this.textBoxObjPosXZ.Location = new System.Drawing.Point(48, 70);
            this.textBoxObjPosXZ.Name = "textBoxObjPosXZ";
            this.textBoxObjPosXZ.Size = new System.Drawing.Size(36, 20);
            this.textBoxObjPosXZ.TabIndex = 27;
            this.textBoxObjPosXZ.Text = "100";
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
            this.buttonObjClone.Location = new System.Drawing.Point(2, 52);
            this.buttonObjClone.Margin = new System.Windows.Forms.Padding(2);
            this.buttonObjClone.Name = "buttonObjClone";
            this.buttonObjClone.Size = new System.Drawing.Size(91, 21);
            this.buttonObjClone.TabIndex = 14;
            this.buttonObjClone.Text = "Clone";
            this.buttonObjClone.UseVisualStyleBackColor = true;
            // 
            // buttonObjUnload
            // 
            this.buttonObjUnload.Location = new System.Drawing.Point(97, 52);
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
            this.NoTearFlowLayoutPanelObject.Size = new System.Drawing.Size(791, 195);
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
            this.tabPageMario.Controls.Add(this.buttonMarioVisibility);
            this.tabPageMario.Controls.Add(this.buttonMarioToggleHandsfree);
            this.tabPageMario.Controls.Add(this.panelMarioBorder);
            this.tabPageMario.Controls.Add(this.NoTearFlowLayoutPanelMario);
            this.tabPageMario.Location = new System.Drawing.Point(4, 22);
            this.tabPageMario.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageMario.Name = "tabPageMario";
            this.tabPageMario.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageMario.Size = new System.Drawing.Size(1012, 202);
            this.tabPageMario.TabIndex = 1;
            this.tabPageMario.Text = "Mario";
            // 
            // buttonMarioVisibility
            // 
            this.buttonMarioVisibility.Location = new System.Drawing.Point(3, 64);
            this.buttonMarioVisibility.Name = "buttonMarioVisibility";
            this.buttonMarioVisibility.Size = new System.Drawing.Size(75, 42);
            this.buttonMarioVisibility.TabIndex = 3;
            this.buttonMarioVisibility.Text = "Toggle Visibility";
            this.buttonMarioVisibility.UseVisualStyleBackColor = true;
            // 
            // buttonMarioToggleHandsfree
            // 
            this.buttonMarioToggleHandsfree.Location = new System.Drawing.Point(3, 112);
            this.buttonMarioToggleHandsfree.Name = "buttonMarioToggleHandsfree";
            this.buttonMarioToggleHandsfree.Size = new System.Drawing.Size(75, 42);
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
            this.NoTearFlowLayoutPanelMario.Location = new System.Drawing.Point(83, 6);
            this.NoTearFlowLayoutPanelMario.Margin = new System.Windows.Forms.Padding(2);
            this.NoTearFlowLayoutPanelMario.Name = "NoTearFlowLayoutPanelMario";
            this.NoTearFlowLayoutPanelMario.Size = new System.Drawing.Size(927, 193);
            this.NoTearFlowLayoutPanelMario.TabIndex = 1;
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
            this.tabPageHud.Size = new System.Drawing.Size(1012, 202);
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
            this.NoTearFlowLayoutPanelHud.Size = new System.Drawing.Size(907, 189);
            this.NoTearFlowLayoutPanelHud.TabIndex = 3;
            // 
            // tabPageActions
            // 
            this.tabPageActions.Controls.Add(this.noTearFlowLayoutPanelActions);
            this.tabPageActions.Location = new System.Drawing.Point(4, 22);
            this.tabPageActions.Name = "tabPageActions";
            this.tabPageActions.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageActions.Size = new System.Drawing.Size(1012, 202);
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
            this.noTearFlowLayoutPanelActions.Size = new System.Drawing.Size(1000, 190);
            this.noTearFlowLayoutPanelActions.TabIndex = 0;
            // 
            // tabPageCamera
            // 
            this.tabPageCamera.Controls.Add(this.NoTearFlowLayoutPanelCamera);
            this.tabPageCamera.Controls.Add(this.panelCameraBorder);
            this.tabPageCamera.Location = new System.Drawing.Point(4, 22);
            this.tabPageCamera.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageCamera.Name = "tabPageCamera";
            this.tabPageCamera.Size = new System.Drawing.Size(1012, 202);
            this.tabPageCamera.TabIndex = 7;
            this.tabPageCamera.Text = "Camera";
            // 
            // NoTearFlowLayoutPanelCamera
            // 
            this.NoTearFlowLayoutPanelCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NoTearFlowLayoutPanelCamera.AutoScroll = true;
            this.NoTearFlowLayoutPanelCamera.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.NoTearFlowLayoutPanelCamera.Location = new System.Drawing.Point(65, 2);
            this.NoTearFlowLayoutPanelCamera.Margin = new System.Windows.Forms.Padding(2);
            this.NoTearFlowLayoutPanelCamera.Name = "NoTearFlowLayoutPanelCamera";
            this.NoTearFlowLayoutPanelCamera.Size = new System.Drawing.Size(945, 193);
            this.NoTearFlowLayoutPanelCamera.TabIndex = 3;
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
            // tabPageWater
            // 
            this.tabPageWater.Controls.Add(this.noTearFlowLayoutPanelWater);
            this.tabPageWater.Location = new System.Drawing.Point(4, 22);
            this.tabPageWater.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageWater.Name = "tabPageWater";
            this.tabPageWater.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageWater.Size = new System.Drawing.Size(1012, 202);
            this.tabPageWater.TabIndex = 14;
            this.tabPageWater.Text = "Water";
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
            this.tabPageDebug.Size = new System.Drawing.Size(1012, 202);
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
            // tabPageMisc
            // 
            this.tabPageMisc.Controls.Add(this.groupBoxPuController);
            this.tabPageMisc.Controls.Add(this.panelMiscBorder);
            this.tabPageMisc.Controls.Add(this.NoTearFlowLayoutPanelMisc);
            this.tabPageMisc.Location = new System.Drawing.Point(4, 22);
            this.tabPageMisc.Name = "tabPageMisc";
            this.tabPageMisc.Size = new System.Drawing.Size(1012, 202);
            this.tabPageMisc.TabIndex = 9;
            this.tabPageMisc.Text = "Misc.";
            // 
            // groupBoxPuController
            // 
            this.groupBoxPuController.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
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
            this.groupBoxPuController.Location = new System.Drawing.Point(787, 1);
            this.groupBoxPuController.Name = "groupBoxPuController";
            this.groupBoxPuController.Size = new System.Drawing.Size(222, 197);
            this.groupBoxPuController.TabIndex = 6;
            this.groupBoxPuController.TabStop = false;
            this.groupBoxPuController.Text = "PU Controller";
            // 
            // labelPuConPu
            // 
            this.labelPuConPu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelPuConPu.AutoSize = true;
            this.labelPuConPu.Location = new System.Drawing.Point(5, 181);
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
            this.labelPuConQpuValue.Location = new System.Drawing.Point(174, 181);
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
            this.labelPuConQpu.Location = new System.Drawing.Point(111, 181);
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
            this.labelPuConPuValue.Location = new System.Drawing.Point(60, 181);
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
            this.NoTearFlowLayoutPanelMisc.Size = new System.Drawing.Size(718, 193);
            this.NoTearFlowLayoutPanelMisc.TabIndex = 5;
            // 
            // tabPageTriangles
            // 
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
            this.tabPageTriangles.Size = new System.Drawing.Size(1012, 202);
            this.tabPageTriangles.TabIndex = 11;
            this.tabPageTriangles.Text = "Triangles";
            // 
            // buttonGoToVClosest
            // 
            this.buttonGoToVClosest.Location = new System.Drawing.Point(6, 141);
            this.buttonGoToVClosest.Name = "buttonGoToVClosest";
            this.buttonGoToVClosest.Size = new System.Drawing.Size(90, 23);
            this.buttonGoToVClosest.TabIndex = 13;
            this.buttonGoToVClosest.Text = "Goto Closest";
            this.buttonGoToVClosest.UseVisualStyleBackColor = true;
            // 
            // checkBoxVertexMisalignment
            // 
            this.checkBoxVertexMisalignment.AutoSize = true;
            this.checkBoxVertexMisalignment.Location = new System.Drawing.Point(6, 170);
            this.checkBoxVertexMisalignment.Name = "checkBoxVertexMisalignment";
            this.checkBoxVertexMisalignment.Size = new System.Drawing.Size(151, 17);
            this.checkBoxVertexMisalignment.TabIndex = 12;
            this.checkBoxVertexMisalignment.Text = "Vertex Misalignment Offset";
            this.checkBoxVertexMisalignment.UseVisualStyleBackColor = true;
            // 
            // buttonRetrieveTriangle
            // 
            this.buttonRetrieveTriangle.Location = new System.Drawing.Point(102, 141);
            this.buttonRetrieveTriangle.Name = "buttonRetrieveTriangle";
            this.buttonRetrieveTriangle.Size = new System.Drawing.Size(84, 23);
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
            this.NoTearFlowLayoutPanelTriangles.Size = new System.Drawing.Size(816, 193);
            this.NoTearFlowLayoutPanelTriangles.TabIndex = 7;
            // 
            // tabPageStars
            // 
            this.tabPageStars.Controls.Add(this.NoTearFlowLayoutPanel1);
            this.tabPageStars.Controls.Add(this.radioButtonFileD);
            this.tabPageStars.Controls.Add(this.radioButtonFileC);
            this.tabPageStars.Controls.Add(this.radioButtonFileB);
            this.tabPageStars.Controls.Add(this.radioButtonFileA);
            this.tabPageStars.Location = new System.Drawing.Point(4, 22);
            this.tabPageStars.Name = "tabPageStars";
            this.tabPageStars.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageStars.Size = new System.Drawing.Size(1012, 202);
            this.tabPageStars.TabIndex = 10;
            this.tabPageStars.Text = "Stars";
            // 
            // NoTearFlowLayoutPanel1
            // 
            this.NoTearFlowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NoTearFlowLayoutPanel1.AutoScroll = true;
            this.NoTearFlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.NoTearFlowLayoutPanel1.Location = new System.Drawing.Point(63, 7);
            this.NoTearFlowLayoutPanel1.Name = "NoTearFlowLayoutPanel1";
            this.NoTearFlowLayoutPanel1.Size = new System.Drawing.Size(943, 190);
            this.NoTearFlowLayoutPanel1.TabIndex = 4;
            this.NoTearFlowLayoutPanel1.WrapContents = false;
            // 
            // radioButtonFileD
            // 
            this.radioButtonFileD.AutoSize = true;
            this.radioButtonFileD.Location = new System.Drawing.Point(6, 75);
            this.radioButtonFileD.Name = "radioButtonFileD";
            this.radioButtonFileD.Size = new System.Drawing.Size(52, 17);
            this.radioButtonFileD.TabIndex = 3;
            this.radioButtonFileD.Text = "File D";
            this.radioButtonFileD.UseVisualStyleBackColor = true;
            // 
            // radioButtonFileC
            // 
            this.radioButtonFileC.AutoSize = true;
            this.radioButtonFileC.Location = new System.Drawing.Point(6, 52);
            this.radioButtonFileC.Name = "radioButtonFileC";
            this.radioButtonFileC.Size = new System.Drawing.Size(51, 17);
            this.radioButtonFileC.TabIndex = 2;
            this.radioButtonFileC.Text = "File C";
            this.radioButtonFileC.UseVisualStyleBackColor = true;
            // 
            // radioButtonFileB
            // 
            this.radioButtonFileB.AutoSize = true;
            this.radioButtonFileB.Location = new System.Drawing.Point(6, 29);
            this.radioButtonFileB.Name = "radioButtonFileB";
            this.radioButtonFileB.Size = new System.Drawing.Size(51, 17);
            this.radioButtonFileB.TabIndex = 1;
            this.radioButtonFileB.Text = "File B";
            this.radioButtonFileB.UseVisualStyleBackColor = true;
            // 
            // radioButtonFileA
            // 
            this.radioButtonFileA.AutoSize = true;
            this.radioButtonFileA.Checked = true;
            this.radioButtonFileA.Location = new System.Drawing.Point(6, 6);
            this.radioButtonFileA.Name = "radioButtonFileA";
            this.radioButtonFileA.Size = new System.Drawing.Size(51, 17);
            this.radioButtonFileA.TabIndex = 0;
            this.radioButtonFileA.TabStop = true;
            this.radioButtonFileA.Text = "File A";
            this.radioButtonFileA.UseVisualStyleBackColor = true;
            // 
            // tabPageMap
            // 
            this.tabPageMap.Controls.Add(this.splitContainerMap);
            this.tabPageMap.Location = new System.Drawing.Point(4, 22);
            this.tabPageMap.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageMap.Name = "tabPageMap";
            this.tabPageMap.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageMap.Size = new System.Drawing.Size(1012, 202);
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
            this.splitContainerMap.Panel1.Controls.Add(this.checkBoxMapShowFloor);
            this.splitContainerMap.Panel1.Controls.Add(this.checkBoxMapShowCamera);
            this.splitContainerMap.Panel1.Controls.Add(this.checkBoxMapShowHolp);
            this.splitContainerMap.Panel1.Controls.Add(this.checkBoxMapShowObj);
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
            this.splitContainerMap.Size = new System.Drawing.Size(1011, 193);
            this.splitContainerMap.SplitterDistance = 334;
            this.splitContainerMap.SplitterWidth = 3;
            this.splitContainerMap.TabIndex = 16;
            // 
            // checkBoxMapShowFloor
            // 
            this.checkBoxMapShowFloor.AutoSize = true;
            this.checkBoxMapShowFloor.Location = new System.Drawing.Point(5, 122);
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
            // checkBoxMapShowObj
            // 
            this.checkBoxMapShowObj.AutoSize = true;
            this.checkBoxMapShowObj.Location = new System.Drawing.Point(5, 102);
            this.checkBoxMapShowObj.Name = "checkBoxMapShowObj";
            this.checkBoxMapShowObj.Size = new System.Drawing.Size(92, 17);
            this.checkBoxMapShowObj.TabIndex = 16;
            this.checkBoxMapShowObj.Text = "Show Objects";
            this.checkBoxMapShowObj.UseVisualStyleBackColor = true;
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
            this.labelMapId.Location = new System.Drawing.Point(220, 174);
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
            this.glControlMap.Size = new System.Drawing.Size(668, 167);
            this.glControlMap.TabIndex = 0;
            this.glControlMap.VSync = false;
            this.glControlMap.Load += new System.EventHandler(this.glControlMap_Load);
            // 
            // labelMapPu
            // 
            this.labelMapPu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelMapPu.AutoSize = true;
            this.labelMapPu.Location = new System.Drawing.Point(2, 175);
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
            this.labelMapPuValue.Location = new System.Drawing.Point(67, 175);
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
            this.labelMapQpu.Location = new System.Drawing.Point(123, 175);
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
            this.labelMapQpuValue.Location = new System.Drawing.Point(196, 175);
            this.labelMapQpuValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelMapQpuValue.Name = "labelMapQpuValue";
            this.labelMapQpuValue.Size = new System.Drawing.Size(37, 13);
            this.labelMapQpuValue.TabIndex = 1;
            this.labelMapQpuValue.Text = "[0:0:0]";
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
            this.tabPageExpressions.Size = new System.Drawing.Size(1012, 202);
            this.tabPageExpressions.TabIndex = 2;
            this.tabPageExpressions.Text = "Expressions";
            // 
            // checkBoxAbsoluteAddress
            // 
            this.checkBoxAbsoluteAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxAbsoluteAddress.AutoSize = true;
            this.checkBoxAbsoluteAddress.Location = new System.Drawing.Point(877, 182);
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
            this.buttonOtherDelete.Location = new System.Drawing.Point(106, 180);
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
            this.buttonOtherModify.Location = new System.Drawing.Point(54, 180);
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
            this.buttonOtherAdd.Location = new System.Drawing.Point(2, 180);
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
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.dataGridViewExpressions.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
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
            this.dataGridViewExpressions.Size = new System.Drawing.Size(1010, 173);
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
            this.tabPageDisassembly.Size = new System.Drawing.Size(1012, 202);
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
            this.richTextBoxDissasembly.Size = new System.Drawing.Size(1007, 175);
            this.richTextBoxDissasembly.TabIndex = 0;
            this.richTextBoxDissasembly.Text = "";
            // 
            // tabPageHacks
            // 
            this.tabPageHacks.Controls.Add(this.splitContainerHacks);
            this.tabPageHacks.Location = new System.Drawing.Point(4, 22);
            this.tabPageHacks.Name = "tabPageHacks";
            this.tabPageHacks.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHacks.Size = new System.Drawing.Size(1012, 202);
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
            this.splitContainerHacks.Size = new System.Drawing.Size(1003, 196);
            this.splitContainerHacks.SplitterDistance = 334;
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
            this.groupBoxHackRam.Size = new System.Drawing.Size(328, 190);
            this.groupBoxHackRam.TabIndex = 13;
            this.groupBoxHackRam.TabStop = false;
            this.groupBoxHackRam.Text = "RAM Hacks*";
            // 
            // labelPureInterpretterRequire
            // 
            this.labelPureInterpretterRequire.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPureInterpretterRequire.AutoSize = true;
            this.labelPureInterpretterRequire.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPureInterpretterRequire.Location = new System.Drawing.Point(199, 1);
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
            this.checkedListBoxHacks.Size = new System.Drawing.Size(316, 109);
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
            this.groupBoxHackSpawn.Size = new System.Drawing.Size(658, 189);
            this.groupBoxHackSpawn.TabIndex = 0;
            this.groupBoxHackSpawn.TabStop = false;
            this.groupBoxHackSpawn.Text = "Spawner";
            // 
            // labelSpawnHint
            // 
            this.labelSpawnHint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelSpawnHint.AutoSize = true;
            this.labelSpawnHint.Location = new System.Drawing.Point(315, 168);
            this.labelSpawnHint.Name = "labelSpawnHint";
            this.labelSpawnHint.Size = new System.Drawing.Size(127, 13);
            this.labelSpawnHint.TabIndex = 25;
            this.labelSpawnHint.Text = "(Press L button to spawn)";
            // 
            // buttonSpawnReset
            // 
            this.buttonSpawnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSpawnReset.Location = new System.Drawing.Point(542, 164);
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
            this.labelSpawnExtra.Location = new System.Drawing.Point(109, 167);
            this.labelSpawnExtra.Name = "labelSpawnExtra";
            this.labelSpawnExtra.Size = new System.Drawing.Size(34, 13);
            this.labelSpawnExtra.TabIndex = 23;
            this.labelSpawnExtra.Text = "Extra:";
            // 
            // labelSpawnGfxId
            // 
            this.labelSpawnGfxId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelSpawnGfxId.AutoSize = true;
            this.labelSpawnGfxId.Location = new System.Drawing.Point(6, 168);
            this.labelSpawnGfxId.Name = "labelSpawnGfxId";
            this.labelSpawnGfxId.Size = new System.Drawing.Size(45, 13);
            this.labelSpawnGfxId.TabIndex = 22;
            this.labelSpawnGfxId.Text = "GFX ID:";
            // 
            // textBoxSpawnExtra
            // 
            this.textBoxSpawnExtra.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxSpawnExtra.Location = new System.Drawing.Point(149, 164);
            this.textBoxSpawnExtra.Name = "textBoxSpawnExtra";
            this.textBoxSpawnExtra.Size = new System.Drawing.Size(46, 20);
            this.textBoxSpawnExtra.TabIndex = 21;
            // 
            // textBoxSpawnGfxId
            // 
            this.textBoxSpawnGfxId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxSpawnGfxId.Location = new System.Drawing.Point(57, 164);
            this.textBoxSpawnGfxId.Name = "textBoxSpawnGfxId";
            this.textBoxSpawnGfxId.Size = new System.Drawing.Size(46, 20);
            this.textBoxSpawnGfxId.TabIndex = 20;
            // 
            // buttonHackSpawn
            // 
            this.buttonHackSpawn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonHackSpawn.Location = new System.Drawing.Point(200, 164);
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
            this.listBoxSpawn.Size = new System.Drawing.Size(646, 82);
            this.listBoxSpawn.Sorted = true;
            this.listBoxSpawn.TabIndex = 12;
            // 
            // tabPageOptions
            // 
            this.tabPageOptions.Controls.Add(this.checkBoxUseOverlays);
            this.tabPageOptions.Controls.Add(this.label3);
            this.tabPageOptions.Controls.Add(this.checkBoxMoveCamWithPu);
            this.tabPageOptions.Controls.Add(this.checkBoxUseRomHack);
            this.tabPageOptions.Controls.Add(this.checkBoxStartSlotIndexOne);
            this.tabPageOptions.Location = new System.Drawing.Point(4, 22);
            this.tabPageOptions.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageOptions.Name = "tabPageOptions";
            this.tabPageOptions.Size = new System.Drawing.Size(1012, 202);
            this.tabPageOptions.TabIndex = 5;
            this.tabPageOptions.Text = "Options";
            // 
            // checkBoxUseOverlays
            // 
            this.checkBoxUseOverlays.AutoSize = true;
            this.checkBoxUseOverlays.Checked = true;
            this.checkBoxUseOverlays.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUseOverlays.Location = new System.Drawing.Point(3, 69);
            this.checkBoxUseOverlays.Name = "checkBoxUseOverlays";
            this.checkBoxUseOverlays.Size = new System.Drawing.Size(171, 17);
            this.checkBoxUseOverlays.TabIndex = 8;
            this.checkBoxUseOverlays.Text = "Use Mario Interaction Overlays";
            this.checkBoxUseOverlays.UseVisualStyleBackColor = true;
            this.checkBoxUseOverlays.CheckedChanged += new System.EventHandler(this.checkBoxUseOverlays_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 189);
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
            this.checkBoxMoveCamWithPu.Location = new System.Drawing.Point(3, 46);
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
            this.checkBoxStartSlotIndexOne.Location = new System.Drawing.Point(3, 3);
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
            this.labelVersionNumber.Location = new System.Drawing.Point(991, 15);
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
            this.panelConnect.Controls.Add(this.buttonRefresh);
            this.panelConnect.Controls.Add(this.labelNotConnected);
            this.panelConnect.Controls.Add(this.buttonConnect);
            this.panelConnect.Controls.Add(this.listBoxProcessesList);
            this.panelConnect.Location = new System.Drawing.Point(246, 12);
            this.panelConnect.Name = "panelConnect";
            this.panelConnect.Size = new System.Drawing.Size(441, 20);
            this.panelConnect.TabIndex = 17;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonRefresh.Location = new System.Drawing.Point(134, 92);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(75, 37);
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
            this.buttonConnect.Location = new System.Drawing.Point(231, 92);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 37);
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
            this.buttonCollapseTop.Location = new System.Drawing.Point(962, 11);
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
            this.buttonCollapseBottom.Location = new System.Drawing.Point(934, 11);
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
            this.buttonReadOnly.Location = new System.Drawing.Point(798, 11);
            this.buttonReadOnly.Margin = new System.Windows.Forms.Padding(2);
            this.buttonReadOnly.Name = "buttonReadOnly";
            this.buttonReadOnly.Size = new System.Drawing.Size(132, 21);
            this.buttonReadOnly.TabIndex = 21;
            this.buttonReadOnly.Tag = "";
            this.buttonReadOnly.Text = "Disable Read-only";
            this.buttonReadOnly.UseVisualStyleBackColor = true;
            this.buttonReadOnly.Click += new System.EventHandler(this.buttonReadOnly_Click);
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
            this.noTearFlowLayoutPanelWater.Size = new System.Drawing.Size(1008, 198);
            this.noTearFlowLayoutPanelWater.TabIndex = 2;
            // 
            // StroopMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 741);
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
            this.groupBoxObjPos.ResumeLayout(false);
            this.groupBoxObjPos.PerformLayout();
            this.panelObjectBorder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxObject)).EndInit();
            this.tabPageMario.ResumeLayout(false);
            this.panelMarioBorder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMario)).EndInit();
            this.tabPageHud.ResumeLayout(false);
            this.panelHudBorder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHud)).EndInit();
            this.tabPageActions.ResumeLayout(false);
            this.tabPageCamera.ResumeLayout(false);
            this.panelCameraBorder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCamera)).EndInit();
            this.tabPageWater.ResumeLayout(false);
            this.tabPageDebug.ResumeLayout(false);
            this.tabPageDebug.PerformLayout();
            this.panelDebugBorder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDebug)).EndInit();
            this.NoTearFlowLayoutPanelDebugDisplayType.ResumeLayout(false);
            this.NoTearFlowLayoutPanelDebugDisplayType.PerformLayout();
            this.tabPageMisc.ResumeLayout(false);
            this.groupBoxPuController.ResumeLayout(false);
            this.groupBoxPuController.PerformLayout();
            this.panelMiscBorder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMisc)).EndInit();
            this.tabPageTriangles.ResumeLayout(false);
            this.tabPageTriangles.PerformLayout();
            this.tabPageStars.ResumeLayout(false);
            this.tabPageStars.PerformLayout();
            this.tabPageMap.ResumeLayout(false);
            this.splitContainerMap.Panel1.ResumeLayout(false);
            this.splitContainerMap.Panel1.PerformLayout();
            this.splitContainerMap.Panel2.ResumeLayout(false);
            this.splitContainerMap.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMap)).EndInit();
            this.splitContainerMap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMapIconSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMapZoom)).EndInit();
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
            this.tabPageOptions.ResumeLayout(false);
            this.tabPageOptions.PerformLayout();
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
        private System.Windows.Forms.CheckBox checkBoxMapShowObj;
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
        private System.Windows.Forms.TabPage tabPageStars;
        private NoTearFlowLayoutPanel NoTearFlowLayoutPanel1;
        private System.Windows.Forms.RadioButton radioButtonFileD;
        private System.Windows.Forms.RadioButton radioButtonFileC;
        private System.Windows.Forms.RadioButton radioButtonFileB;
        private System.Windows.Forms.RadioButton radioButtonFileA;
        private System.Windows.Forms.GroupBox groupBoxPuController;
        private System.Windows.Forms.Button buttonPuConZpPu;
        private System.Windows.Forms.Button buttonPuConZpQpu;
        private System.Windows.Forms.Button buttonPuConHome;
        private System.Windows.Forms.Label labelPuContXp;
        private System.Windows.Forms.Label labelPuContXm;
        private System.Windows.Forms.Label labelPuContZp;
        private System.Windows.Forms.Label labelPuContZm;
        private System.Windows.Forms.Button buttonPuConZnPu;
        private System.Windows.Forms.Button buttonPuConXpQpu;
        private System.Windows.Forms.Button buttonPuConXnQpu;
        private System.Windows.Forms.Button buttonPuConXnPu;
        private System.Windows.Forms.Button buttonPuConZnQpu;
        private System.Windows.Forms.Button buttonPuConXpPu;
        private System.Windows.Forms.Label labelSlotSize;
        private System.Windows.Forms.CheckBox checkBoxMapShowFloor;
        private CheckBox checkBoxMoveCamWithPu;
        private Label labelPuConPu;
        private Label labelPuConQpuValue;
        private Label labelPuConQpu;
        private Label labelPuConPuValue;
        private Label label3;
        private CheckBox checkBoxUseOverlays;
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
    }
}


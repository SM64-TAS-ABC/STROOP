namespace SM64_Diagnostic
{
    partial class SM64DiagnosticForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SM64DiagnosticForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.comboBoxProcessSelection = new System.Windows.Forms.ComboBox();
            this.labelProcessSelect = new System.Windows.Forms.Label();
            this.groupBoxObjects = new System.Windows.Forms.GroupBox();
            this.pictureBoxObjClone = new System.Windows.Forms.PictureBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.pictureBoxTrash = new System.Windows.Forms.PictureBox();
            this.labelSortMethod = new System.Windows.Forms.Label();
            this.flowLayoutPanelObjects = new System.Windows.Forms.FlowLayoutPanel();
            this.comboBoxSortMethod = new System.Windows.Forms.ComboBox();
            this.buttonPauseResume = new System.Windows.Forms.Button();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageObjects = new System.Windows.Forms.TabPage();
            this.buttonObjRetrieve = new System.Windows.Forms.Button();
            this.buttonObjClone = new System.Windows.Forms.Button();
            this.buttonObjGoTo = new System.Windows.Forms.Button();
            this.buttonObjUnload = new System.Windows.Forms.Button();
            this.labelObjSlotIndValue = new System.Windows.Forms.Label();
            this.labelObjSlotPosValue = new System.Windows.Forms.Label();
            this.labelObjBhvValue = new System.Windows.Forms.Label();
            this.labelObjAdd = new System.Windows.Forms.Label();
            this.flowLayoutPanelObject = new System.Windows.Forms.FlowLayoutPanel();
            this.labelObjSlotInd = new System.Windows.Forms.Label();
            this.labelObjSlotPos = new System.Windows.Forms.Label();
            this.labelObjBhv = new System.Windows.Forms.Label();
            this.labelObjName = new System.Windows.Forms.Label();
            this.panelObjectBorder = new System.Windows.Forms.Panel();
            this.pictureBoxObject = new System.Windows.Forms.PictureBox();
            this.labelObjAddValue = new System.Windows.Forms.Label();
            this.tabPageMario = new System.Windows.Forms.TabPage();
            this.flowLayoutPanelMario = new System.Windows.Forms.FlowLayoutPanel();
            this.panelMarioBorder = new System.Windows.Forms.Panel();
            this.pictureBoxMario = new System.Windows.Forms.PictureBox();
            this.tabPageOther = new System.Windows.Forms.TabPage();
            this.checkBoxAbsoluteAddress = new System.Windows.Forms.CheckBox();
            this.buttonOtherDelete = new System.Windows.Forms.Button();
            this.buttonOtherModify = new System.Windows.Forms.Button();
            this.buttonOtherAdd = new System.Windows.Forms.Button();
            this.dataGridViewOther = new System.Windows.Forms.DataGridView();
            this.tabPageDisassembly = new System.Windows.Forms.TabPage();
            this.buttonDisGo = new System.Windows.Forms.Button();
            this.maskedTextBoxDisStart = new System.Windows.Forms.MaskedTextBox();
            this.labelDisStart = new System.Windows.Forms.Label();
            this.richTextBoxDissasembly = new System.Windows.Forms.RichTextBox();
            this.tabPageMap = new System.Windows.Forms.TabPage();
            this.labelMapPuValue = new System.Windows.Forms.Label();
            this.labelMapSubName = new System.Windows.Forms.Label();
            this.labelMapQpu = new System.Windows.Forms.Label();
            this.labelMapPu = new System.Windows.Forms.Label();
            this.labelMapId = new System.Windows.Forms.Label();
            this.labelMapName = new System.Windows.Forms.Label();
            this.labelMapQpuValue = new System.Windows.Forms.Label();
            this.glControlMap = new OpenTK.GLControl();
            this.tabPageOptions = new System.Windows.Forms.TabPage();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBoxStartSlotIndexOne = new System.Windows.Forms.CheckBox();
            this.groupBoxObjects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxObjClone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTrash)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageObjects.SuspendLayout();
            this.panelObjectBorder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxObject)).BeginInit();
            this.tabPageMario.SuspendLayout();
            this.panelMarioBorder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMario)).BeginInit();
            this.tabPageOther.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOther)).BeginInit();
            this.tabPageDisassembly.SuspendLayout();
            this.tabPageMap.SuspendLayout();
            this.tabPageOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxProcessSelection
            // 
            this.comboBoxProcessSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxProcessSelection.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBoxProcessSelection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxProcessSelection.FormattingEnabled = true;
            this.comboBoxProcessSelection.Location = new System.Drawing.Point(190, 12);
            this.comboBoxProcessSelection.Name = "comboBoxProcessSelection";
            this.comboBoxProcessSelection.Size = new System.Drawing.Size(296, 28);
            this.comboBoxProcessSelection.TabIndex = 0;
            this.comboBoxProcessSelection.DropDown += new System.EventHandler(this.comboBoxProcessSelection_DropDown);
            this.comboBoxProcessSelection.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // labelProcessSelect
            // 
            this.labelProcessSelect.AutoSize = true;
            this.labelProcessSelect.Location = new System.Drawing.Point(14, 15);
            this.labelProcessSelect.Name = "labelProcessSelect";
            this.labelProcessSelect.Size = new System.Drawing.Size(172, 20);
            this.labelProcessSelect.TabIndex = 1;
            this.labelProcessSelect.Text = "Select Mupen Process:";
            // 
            // groupBoxObjects
            // 
            this.groupBoxObjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxObjects.Controls.Add(this.pictureBoxObjClone);
            this.groupBoxObjects.Controls.Add(this.checkBox1);
            this.groupBoxObjects.Controls.Add(this.pictureBoxTrash);
            this.groupBoxObjects.Controls.Add(this.labelSortMethod);
            this.groupBoxObjects.Controls.Add(this.flowLayoutPanelObjects);
            this.groupBoxObjects.Controls.Add(this.comboBoxSortMethod);
            this.groupBoxObjects.Location = new System.Drawing.Point(3, 3);
            this.groupBoxObjects.Name = "groupBoxObjects";
            this.groupBoxObjects.Size = new System.Drawing.Size(728, 385);
            this.groupBoxObjects.TabIndex = 2;
            this.groupBoxObjects.TabStop = false;
            this.groupBoxObjects.Text = "Objects";
            // 
            // pictureBoxObjClone
            // 
            this.pictureBoxObjClone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxObjClone.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxObjClone.Image")));
            this.pictureBoxObjClone.Location = new System.Drawing.Point(382, 20);
            this.pictureBoxObjClone.Name = "pictureBoxObjClone";
            this.pictureBoxObjClone.Size = new System.Drawing.Size(34, 28);
            this.pictureBoxObjClone.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxObjClone.TabIndex = 8;
            this.pictureBoxObjClone.TabStop = false;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(6, 28);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(120, 24);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Lock Labels";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // pictureBoxTrash
            // 
            this.pictureBoxTrash.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxTrash.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxTrash.Image")));
            this.pictureBoxTrash.Location = new System.Drawing.Point(423, 20);
            this.pictureBoxTrash.Name = "pictureBoxTrash";
            this.pictureBoxTrash.Size = new System.Drawing.Size(34, 28);
            this.pictureBoxTrash.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxTrash.TabIndex = 6;
            this.pictureBoxTrash.TabStop = false;
            // 
            // labelSortMethod
            // 
            this.labelSortMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSortMethod.AutoSize = true;
            this.labelSortMethod.Location = new System.Drawing.Point(464, 28);
            this.labelSortMethod.Name = "labelSortMethod";
            this.labelSortMethod.Size = new System.Drawing.Size(101, 20);
            this.labelSortMethod.TabIndex = 5;
            this.labelSortMethod.Text = "Sort Method:";
            // 
            // flowLayoutPanelObjects
            // 
            this.flowLayoutPanelObjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanelObjects.AutoScroll = true;
            this.flowLayoutPanelObjects.Location = new System.Drawing.Point(6, 58);
            this.flowLayoutPanelObjects.Name = "flowLayoutPanelObjects";
            this.flowLayoutPanelObjects.Size = new System.Drawing.Size(716, 320);
            this.flowLayoutPanelObjects.TabIndex = 0;
            this.flowLayoutPanelObjects.Resize += new System.EventHandler(this.flowLayoutPanelObjects_Resize);
            // 
            // comboBoxSortMethod
            // 
            this.comboBoxSortMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSortMethod.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBoxSortMethod.Location = new System.Drawing.Point(570, 25);
            this.comboBoxSortMethod.Name = "comboBoxSortMethod";
            this.comboBoxSortMethod.Size = new System.Drawing.Size(151, 28);
            this.comboBoxSortMethod.TabIndex = 4;
            this.comboBoxSortMethod.SelectedIndexChanged += new System.EventHandler(this.comboBoxSortMethod_SelectedIndexChanged);
            // 
            // buttonPauseResume
            // 
            this.buttonPauseResume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPauseResume.Location = new System.Drawing.Point(494, 9);
            this.buttonPauseResume.Name = "buttonPauseResume";
            this.buttonPauseResume.Size = new System.Drawing.Size(96, 32);
            this.buttonPauseResume.TabIndex = 4;
            this.buttonPauseResume.Text = "Pause";
            this.buttonPauseResume.UseVisualStyleBackColor = true;
            this.buttonPauseResume.Click += new System.EventHandler(this.buttonPauseResume_Click);
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerMain.Location = new System.Drawing.Point(18, 46);
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
            this.splitContainerMain.Size = new System.Drawing.Size(734, 632);
            this.splitContainerMain.SplitterDistance = 239;
            this.splitContainerMain.SplitterWidth = 5;
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
            this.tabControlMain.Controls.Add(this.tabPageOther);
            this.tabControlMain.Controls.Add(this.tabPageDisassembly);
            this.tabControlMain.Controls.Add(this.tabPageMap);
            this.tabControlMain.Controls.Add(this.tabPageOptions);
            this.tabControlMain.HotTrack = true;
            this.tabControlMain.Location = new System.Drawing.Point(3, 3);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(728, 237);
            this.tabControlMain.TabIndex = 3;
            this.tabControlMain.DragEnter += new System.Windows.Forms.DragEventHandler(this.tabControlMain_DragEnter);
            // 
            // tabPageObjects
            // 
            this.tabPageObjects.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageObjects.Controls.Add(this.buttonObjRetrieve);
            this.tabPageObjects.Controls.Add(this.buttonObjClone);
            this.tabPageObjects.Controls.Add(this.buttonObjGoTo);
            this.tabPageObjects.Controls.Add(this.buttonObjUnload);
            this.tabPageObjects.Controls.Add(this.labelObjSlotIndValue);
            this.tabPageObjects.Controls.Add(this.labelObjSlotPosValue);
            this.tabPageObjects.Controls.Add(this.labelObjBhvValue);
            this.tabPageObjects.Controls.Add(this.labelObjAdd);
            this.tabPageObjects.Controls.Add(this.flowLayoutPanelObject);
            this.tabPageObjects.Controls.Add(this.labelObjSlotInd);
            this.tabPageObjects.Controls.Add(this.labelObjSlotPos);
            this.tabPageObjects.Controls.Add(this.labelObjBhv);
            this.tabPageObjects.Controls.Add(this.labelObjName);
            this.tabPageObjects.Controls.Add(this.panelObjectBorder);
            this.tabPageObjects.Controls.Add(this.labelObjAddValue);
            this.tabPageObjects.Location = new System.Drawing.Point(4, 29);
            this.tabPageObjects.Name = "tabPageObjects";
            this.tabPageObjects.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPageObjects.Size = new System.Drawing.Size(720, 204);
            this.tabPageObjects.TabIndex = 0;
            this.tabPageObjects.Text = "Object";
            // 
            // buttonObjRetrieve
            // 
            this.buttonObjRetrieve.Location = new System.Drawing.Point(6, 160);
            this.buttonObjRetrieve.Name = "buttonObjRetrieve";
            this.buttonObjRetrieve.Size = new System.Drawing.Size(96, 32);
            this.buttonObjRetrieve.TabIndex = 15;
            this.buttonObjRetrieve.Text = "Retrieve";
            this.buttonObjRetrieve.UseVisualStyleBackColor = true;
            // 
            // buttonObjClone
            // 
            this.buttonObjClone.Location = new System.Drawing.Point(108, 160);
            this.buttonObjClone.Name = "buttonObjClone";
            this.buttonObjClone.Size = new System.Drawing.Size(96, 32);
            this.buttonObjClone.TabIndex = 14;
            this.buttonObjClone.Text = "Clone";
            this.buttonObjClone.UseVisualStyleBackColor = true;
            // 
            // buttonObjGoTo
            // 
            this.buttonObjGoTo.Location = new System.Drawing.Point(6, 122);
            this.buttonObjGoTo.Name = "buttonObjGoTo";
            this.buttonObjGoTo.Size = new System.Drawing.Size(96, 32);
            this.buttonObjGoTo.TabIndex = 13;
            this.buttonObjGoTo.Text = "Go To";
            this.buttonObjGoTo.UseVisualStyleBackColor = true;
            // 
            // buttonObjUnload
            // 
            this.buttonObjUnload.Location = new System.Drawing.Point(108, 122);
            this.buttonObjUnload.Name = "buttonObjUnload";
            this.buttonObjUnload.Size = new System.Drawing.Size(96, 32);
            this.buttonObjUnload.TabIndex = 5;
            this.buttonObjUnload.Text = "Unload";
            this.buttonObjUnload.UseVisualStyleBackColor = true;
            // 
            // labelObjSlotIndValue
            // 
            this.labelObjSlotIndValue.Location = new System.Drawing.Point(184, 72);
            this.labelObjSlotIndValue.Name = "labelObjSlotIndValue";
            this.labelObjSlotIndValue.Size = new System.Drawing.Size(58, 20);
            this.labelObjSlotIndValue.TabIndex = 11;
            this.labelObjSlotIndValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelObjSlotPosValue
            // 
            this.labelObjSlotPosValue.Location = new System.Drawing.Point(172, 51);
            this.labelObjSlotPosValue.Name = "labelObjSlotPosValue";
            this.labelObjSlotPosValue.Size = new System.Drawing.Size(70, 22);
            this.labelObjSlotPosValue.TabIndex = 10;
            this.labelObjSlotPosValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelObjBhvValue
            // 
            this.labelObjBhvValue.Location = new System.Drawing.Point(140, 31);
            this.labelObjBhvValue.Name = "labelObjBhvValue";
            this.labelObjBhvValue.Size = new System.Drawing.Size(102, 20);
            this.labelObjBhvValue.TabIndex = 9;
            this.labelObjBhvValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelObjAdd
            // 
            this.labelObjAdd.AutoSize = true;
            this.labelObjAdd.Location = new System.Drawing.Point(98, 92);
            this.labelObjAdd.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelObjAdd.Name = "labelObjAdd";
            this.labelObjAdd.Size = new System.Drawing.Size(42, 20);
            this.labelObjAdd.TabIndex = 8;
            this.labelObjAdd.Text = "Add:";
            // 
            // flowLayoutPanelObject
            // 
            this.flowLayoutPanelObject.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanelObject.AutoScroll = true;
            this.flowLayoutPanelObject.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelObject.Location = new System.Drawing.Point(248, 5);
            this.flowLayoutPanelObject.Name = "flowLayoutPanelObject";
            this.flowLayoutPanelObject.Size = new System.Drawing.Size(465, 190);
            this.flowLayoutPanelObject.TabIndex = 3;
            // 
            // labelObjSlotInd
            // 
            this.labelObjSlotInd.AutoSize = true;
            this.labelObjSlotInd.Location = new System.Drawing.Point(98, 72);
            this.labelObjSlotInd.Name = "labelObjSlotInd";
            this.labelObjSlotInd.Size = new System.Drawing.Size(84, 20);
            this.labelObjSlotInd.TabIndex = 7;
            this.labelObjSlotInd.Text = "Slot Index:";
            // 
            // labelObjSlotPos
            // 
            this.labelObjSlotPos.AutoSize = true;
            this.labelObjSlotPos.Location = new System.Drawing.Point(98, 51);
            this.labelObjSlotPos.Name = "labelObjSlotPos";
            this.labelObjSlotPos.Size = new System.Drawing.Size(72, 20);
            this.labelObjSlotPos.TabIndex = 6;
            this.labelObjSlotPos.Text = "Slot Pos:";
            // 
            // labelObjBhv
            // 
            this.labelObjBhv.AutoSize = true;
            this.labelObjBhv.Location = new System.Drawing.Point(98, 31);
            this.labelObjBhv.Name = "labelObjBhv";
            this.labelObjBhv.Size = new System.Drawing.Size(40, 20);
            this.labelObjBhv.TabIndex = 5;
            this.labelObjBhv.Text = "Bhv:";
            // 
            // labelObjName
            // 
            this.labelObjName.AutoSize = true;
            this.labelObjName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelObjName.Location = new System.Drawing.Point(98, 11);
            this.labelObjName.Name = "labelObjName";
            this.labelObjName.Size = new System.Drawing.Size(61, 20);
            this.labelObjName.TabIndex = 4;
            this.labelObjName.Text = "Object";
            // 
            // panelObjectBorder
            // 
            this.panelObjectBorder.Controls.Add(this.pictureBoxObject);
            this.panelObjectBorder.Location = new System.Drawing.Point(6, 8);
            this.panelObjectBorder.Name = "panelObjectBorder";
            this.panelObjectBorder.Size = new System.Drawing.Size(86, 85);
            this.panelObjectBorder.TabIndex = 2;
            // 
            // pictureBoxObject
            // 
            this.pictureBoxObject.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxObject.Location = new System.Drawing.Point(4, 5);
            this.pictureBoxObject.MaximumSize = new System.Drawing.Size(200, 200);
            this.pictureBoxObject.Name = "pictureBoxObject";
            this.pictureBoxObject.Size = new System.Drawing.Size(76, 77);
            this.pictureBoxObject.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxObject.TabIndex = 0;
            this.pictureBoxObject.TabStop = false;
            // 
            // labelObjAddValue
            // 
            this.labelObjAddValue.Location = new System.Drawing.Point(132, 92);
            this.labelObjAddValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelObjAddValue.Name = "labelObjAddValue";
            this.labelObjAddValue.Size = new System.Drawing.Size(112, 26);
            this.labelObjAddValue.TabIndex = 12;
            this.labelObjAddValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tabPageMario
            // 
            this.tabPageMario.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageMario.Controls.Add(this.flowLayoutPanelMario);
            this.tabPageMario.Controls.Add(this.panelMarioBorder);
            this.tabPageMario.Location = new System.Drawing.Point(4, 29);
            this.tabPageMario.Name = "tabPageMario";
            this.tabPageMario.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPageMario.Size = new System.Drawing.Size(720, 204);
            this.tabPageMario.TabIndex = 1;
            this.tabPageMario.Text = "Mario";
            // 
            // flowLayoutPanelMario
            // 
            this.flowLayoutPanelMario.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanelMario.AutoScroll = true;
            this.flowLayoutPanelMario.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelMario.Location = new System.Drawing.Point(98, 5);
            this.flowLayoutPanelMario.Name = "flowLayoutPanelMario";
            this.flowLayoutPanelMario.Size = new System.Drawing.Size(615, 190);
            this.flowLayoutPanelMario.TabIndex = 1;
            // 
            // panelMarioBorder
            // 
            this.panelMarioBorder.Controls.Add(this.pictureBoxMario);
            this.panelMarioBorder.Location = new System.Drawing.Point(6, 6);
            this.panelMarioBorder.Name = "panelMarioBorder";
            this.panelMarioBorder.Size = new System.Drawing.Size(86, 85);
            this.panelMarioBorder.TabIndex = 0;
            // 
            // pictureBoxMario
            // 
            this.pictureBoxMario.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxMario.Location = new System.Drawing.Point(4, 5);
            this.pictureBoxMario.MaximumSize = new System.Drawing.Size(200, 200);
            this.pictureBoxMario.Name = "pictureBoxMario";
            this.pictureBoxMario.Size = new System.Drawing.Size(76, 77);
            this.pictureBoxMario.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxMario.TabIndex = 0;
            this.pictureBoxMario.TabStop = false;
            // 
            // tabPageOther
            // 
            this.tabPageOther.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageOther.Controls.Add(this.checkBoxAbsoluteAddress);
            this.tabPageOther.Controls.Add(this.buttonOtherDelete);
            this.tabPageOther.Controls.Add(this.buttonOtherModify);
            this.tabPageOther.Controls.Add(this.buttonOtherAdd);
            this.tabPageOther.Controls.Add(this.dataGridViewOther);
            this.tabPageOther.Location = new System.Drawing.Point(4, 29);
            this.tabPageOther.Name = "tabPageOther";
            this.tabPageOther.Size = new System.Drawing.Size(720, 204);
            this.tabPageOther.TabIndex = 2;
            this.tabPageOther.Text = "Other";
            // 
            // checkBoxAbsoluteAddress
            // 
            this.checkBoxAbsoluteAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxAbsoluteAddress.AutoSize = true;
            this.checkBoxAbsoluteAddress.Location = new System.Drawing.Point(516, 170);
            this.checkBoxAbsoluteAddress.Name = "checkBoxAbsoluteAddress";
            this.checkBoxAbsoluteAddress.Size = new System.Drawing.Size(199, 24);
            this.checkBoxAbsoluteAddress.TabIndex = 4;
            this.checkBoxAbsoluteAddress.Text = "View Absolute Address";
            this.checkBoxAbsoluteAddress.UseVisualStyleBackColor = true;
            // 
            // buttonOtherDelete
            // 
            this.buttonOtherDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOtherDelete.Location = new System.Drawing.Point(159, 165);
            this.buttonOtherDelete.Name = "buttonOtherDelete";
            this.buttonOtherDelete.Size = new System.Drawing.Size(72, 32);
            this.buttonOtherDelete.TabIndex = 3;
            this.buttonOtherDelete.Text = "Delete";
            this.buttonOtherDelete.UseVisualStyleBackColor = true;
            this.buttonOtherDelete.Click += new System.EventHandler(this.buttonOtherDelete_Click);
            // 
            // buttonOtherModify
            // 
            this.buttonOtherModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOtherModify.Location = new System.Drawing.Point(81, 165);
            this.buttonOtherModify.Name = "buttonOtherModify";
            this.buttonOtherModify.Size = new System.Drawing.Size(72, 32);
            this.buttonOtherModify.TabIndex = 2;
            this.buttonOtherModify.Text = "Modify";
            this.buttonOtherModify.UseVisualStyleBackColor = true;
            this.buttonOtherModify.Click += new System.EventHandler(this.buttonOtherModify_Click);
            // 
            // buttonOtherAdd
            // 
            this.buttonOtherAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOtherAdd.Location = new System.Drawing.Point(3, 165);
            this.buttonOtherAdd.Name = "buttonOtherAdd";
            this.buttonOtherAdd.Size = new System.Drawing.Size(72, 32);
            this.buttonOtherAdd.TabIndex = 1;
            this.buttonOtherAdd.Text = "Add";
            this.buttonOtherAdd.UseVisualStyleBackColor = true;
            this.buttonOtherAdd.Click += new System.EventHandler(this.buttonOtherAdd_Click);
            // 
            // dataGridViewOther
            // 
            this.dataGridViewOther.AllowUserToAddRows = false;
            this.dataGridViewOther.AllowUserToDeleteRows = false;
            this.dataGridViewOther.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.dataGridViewOther.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewOther.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewOther.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewOther.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewOther.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewOther.Name = "dataGridViewOther";
            this.dataGridViewOther.RowTemplate.Height = 20;
            this.dataGridViewOther.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewOther.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewOther.Size = new System.Drawing.Size(712, 156);
            this.dataGridViewOther.TabIndex = 0;
            this.dataGridViewOther.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewOther_CellMouseDoubleClick);
            // 
            // tabPageDisassembly
            // 
            this.tabPageDisassembly.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageDisassembly.Controls.Add(this.buttonDisGo);
            this.tabPageDisassembly.Controls.Add(this.maskedTextBoxDisStart);
            this.tabPageDisassembly.Controls.Add(this.labelDisStart);
            this.tabPageDisassembly.Controls.Add(this.richTextBoxDissasembly);
            this.tabPageDisassembly.Location = new System.Drawing.Point(4, 29);
            this.tabPageDisassembly.Name = "tabPageDisassembly";
            this.tabPageDisassembly.Size = new System.Drawing.Size(720, 204);
            this.tabPageDisassembly.TabIndex = 3;
            this.tabPageDisassembly.Text = "Disassembly";
            // 
            // buttonDisGo
            // 
            this.buttonDisGo.Enabled = false;
            this.buttonDisGo.Location = new System.Drawing.Point(256, 3);
            this.buttonDisGo.Name = "buttonDisGo";
            this.buttonDisGo.Size = new System.Drawing.Size(75, 31);
            this.buttonDisGo.TabIndex = 5;
            this.buttonDisGo.Text = "Go";
            this.buttonDisGo.UseVisualStyleBackColor = true;
            // 
            // maskedTextBoxDisStart
            // 
            this.maskedTextBoxDisStart.Enabled = false;
            this.maskedTextBoxDisStart.Location = new System.Drawing.Point(118, 3);
            this.maskedTextBoxDisStart.Mask = "\\0xaaAAAAAA";
            this.maskedTextBoxDisStart.Name = "maskedTextBoxDisStart";
            this.maskedTextBoxDisStart.Size = new System.Drawing.Size(128, 26);
            this.maskedTextBoxDisStart.TabIndex = 4;
            // 
            // labelDisStart
            // 
            this.labelDisStart.AutoSize = true;
            this.labelDisStart.Location = new System.Drawing.Point(3, 9);
            this.labelDisStart.Name = "labelDisStart";
            this.labelDisStart.Size = new System.Drawing.Size(111, 20);
            this.labelDisStart.TabIndex = 3;
            this.labelDisStart.Text = "Start Address:";
            // 
            // richTextBoxDissasembly
            // 
            this.richTextBoxDissasembly.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxDissasembly.Font = new System.Drawing.Font("Courier New", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxDissasembly.Location = new System.Drawing.Point(4, 35);
            this.richTextBoxDissasembly.Name = "richTextBoxDissasembly";
            this.richTextBoxDissasembly.ReadOnly = true;
            this.richTextBoxDissasembly.Size = new System.Drawing.Size(712, 161);
            this.richTextBoxDissasembly.TabIndex = 0;
            this.richTextBoxDissasembly.Text = "";
            // 
            // tabPageMap
            // 
            this.tabPageMap.Controls.Add(this.labelMapPuValue);
            this.tabPageMap.Controls.Add(this.labelMapSubName);
            this.tabPageMap.Controls.Add(this.labelMapQpu);
            this.tabPageMap.Controls.Add(this.labelMapPu);
            this.tabPageMap.Controls.Add(this.labelMapId);
            this.tabPageMap.Controls.Add(this.labelMapName);
            this.tabPageMap.Controls.Add(this.labelMapQpuValue);
            this.tabPageMap.Controls.Add(this.glControlMap);
            this.tabPageMap.Location = new System.Drawing.Point(4, 29);
            this.tabPageMap.Name = "tabPageMap";
            this.tabPageMap.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPageMap.Size = new System.Drawing.Size(720, 204);
            this.tabPageMap.TabIndex = 4;
            this.tabPageMap.Text = "Map";
            // 
            // labelMapPuValue
            // 
            this.labelMapPuValue.AutoSize = true;
            this.labelMapPuValue.Location = new System.Drawing.Point(86, 78);
            this.labelMapPuValue.Name = "labelMapPuValue";
            this.labelMapPuValue.Size = new System.Drawing.Size(0, 20);
            this.labelMapPuValue.TabIndex = 8;
            // 
            // labelMapSubName
            // 
            this.labelMapSubName.AutoSize = true;
            this.labelMapSubName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMapSubName.Location = new System.Drawing.Point(6, 23);
            this.labelMapSubName.Name = "labelMapSubName";
            this.labelMapSubName.Size = new System.Drawing.Size(0, 20);
            this.labelMapSubName.TabIndex = 7;
            // 
            // labelMapQpu
            // 
            this.labelMapQpu.AutoSize = true;
            this.labelMapQpu.Location = new System.Drawing.Point(3, 58);
            this.labelMapQpu.Name = "labelMapQpu";
            this.labelMapQpu.Size = new System.Drawing.Size(85, 20);
            this.labelMapQpu.TabIndex = 6;
            this.labelMapQpu.Text = "QPU [X:Y]:";
            // 
            // labelMapPu
            // 
            this.labelMapPu.AutoSize = true;
            this.labelMapPu.Location = new System.Drawing.Point(6, 78);
            this.labelMapPu.Name = "labelMapPu";
            this.labelMapPu.Size = new System.Drawing.Size(73, 20);
            this.labelMapPu.TabIndex = 5;
            this.labelMapPu.Text = "PU [X:Y]:";
            // 
            // labelMapId
            // 
            this.labelMapId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelMapId.AutoSize = true;
            this.labelMapId.Location = new System.Drawing.Point(6, 162);
            this.labelMapId.Name = "labelMapId";
            this.labelMapId.Size = new System.Drawing.Size(52, 20);
            this.labelMapId.TabIndex = 4;
            this.labelMapId.Text = "[0:0:0]";
            this.labelMapId.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelMapName
            // 
            this.labelMapName.AutoSize = true;
            this.labelMapName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMapName.Location = new System.Drawing.Point(3, 3);
            this.labelMapName.Name = "labelMapName";
            this.labelMapName.Size = new System.Drawing.Size(83, 20);
            this.labelMapName.TabIndex = 3;
            this.labelMapName.Text = "Unknown";
            // 
            // labelMapQpuValue
            // 
            this.labelMapQpuValue.AutoSize = true;
            this.labelMapQpuValue.Location = new System.Drawing.Point(94, 58);
            this.labelMapQpuValue.Name = "labelMapQpuValue";
            this.labelMapQpuValue.Size = new System.Drawing.Size(0, 20);
            this.labelMapQpuValue.TabIndex = 1;
            // 
            // glControlMap
            // 
            this.glControlMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.glControlMap.BackColor = System.Drawing.Color.Black;
            this.glControlMap.Location = new System.Drawing.Point(242, 5);
            this.glControlMap.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.glControlMap.Name = "glControlMap";
            this.glControlMap.Size = new System.Drawing.Size(472, 177);
            this.glControlMap.TabIndex = 0;
            this.glControlMap.VSync = false;
            this.glControlMap.Load += new System.EventHandler(this.glControlMap_Load);
            // 
            // tabPageOptions
            // 
            this.tabPageOptions.Controls.Add(this.checkBox2);
            this.tabPageOptions.Controls.Add(this.checkBoxStartSlotIndexOne);
            this.tabPageOptions.Location = new System.Drawing.Point(4, 29);
            this.tabPageOptions.Name = "tabPageOptions";
            this.tabPageOptions.Size = new System.Drawing.Size(720, 204);
            this.tabPageOptions.TabIndex = 5;
            this.tabPageOptions.Text = "Options";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(4, 34);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(150, 24);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "Crash Program?";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBoxStartSlotIndexOne
            // 
            this.checkBoxStartSlotIndexOne.AutoSize = true;
            this.checkBoxStartSlotIndexOne.Location = new System.Drawing.Point(4, 5);
            this.checkBoxStartSlotIndexOne.Name = "checkBoxStartSlotIndexOne";
            this.checkBoxStartSlotIndexOne.Size = new System.Drawing.Size(199, 24);
            this.checkBoxStartSlotIndexOne.TabIndex = 0;
            this.checkBoxStartSlotIndexOne.Text = "Start Slot Index From 1";
            this.checkBoxStartSlotIndexOne.UseVisualStyleBackColor = true;
            // 
            // SM64DiagnosticForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 689);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.buttonPauseResume);
            this.Controls.Add(this.labelProcessSelect);
            this.Controls.Add(this.comboBoxProcessSelection);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SM64DiagnosticForm";
            this.Text = "Stroop";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBoxObjects.ResumeLayout(false);
            this.groupBoxObjects.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxObjClone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTrash)).EndInit();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageObjects.ResumeLayout(false);
            this.tabPageObjects.PerformLayout();
            this.panelObjectBorder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxObject)).EndInit();
            this.tabPageMario.ResumeLayout(false);
            this.panelMarioBorder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMario)).EndInit();
            this.tabPageOther.ResumeLayout(false);
            this.tabPageOther.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOther)).EndInit();
            this.tabPageDisassembly.ResumeLayout(false);
            this.tabPageDisassembly.PerformLayout();
            this.tabPageMap.ResumeLayout(false);
            this.tabPageMap.PerformLayout();
            this.tabPageOptions.ResumeLayout(false);
            this.tabPageOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxProcessSelection;
        private System.Windows.Forms.Label labelProcessSelect;
        private System.Windows.Forms.GroupBox groupBoxObjects;
        private System.Windows.Forms.ComboBox comboBoxSortMethod;
        private System.Windows.Forms.Label labelSortMethod;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelObjects;
        private System.Windows.Forms.Button buttonPauseResume;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.PictureBox pictureBoxTrash;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageObjects;
        private System.Windows.Forms.Label labelObjSlotIndValue;
        private System.Windows.Forms.Label labelObjSlotPosValue;
        private System.Windows.Forms.Label labelObjBhvValue;
        private System.Windows.Forms.Label labelObjAdd;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelObject;
        private System.Windows.Forms.Label labelObjSlotInd;
        private System.Windows.Forms.Label labelObjSlotPos;
        private System.Windows.Forms.Label labelObjBhv;
        private System.Windows.Forms.Label labelObjName;
        private System.Windows.Forms.Panel panelObjectBorder;
        private System.Windows.Forms.PictureBox pictureBoxObject;
        private System.Windows.Forms.Label labelObjAddValue;
        private System.Windows.Forms.TabPage tabPageMario;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelMario;
        private System.Windows.Forms.Panel panelMarioBorder;
        private System.Windows.Forms.PictureBox pictureBoxMario;
        private System.Windows.Forms.TabPage tabPageOther;
        private System.Windows.Forms.CheckBox checkBoxAbsoluteAddress;
        private System.Windows.Forms.Button buttonOtherDelete;
        private System.Windows.Forms.Button buttonOtherModify;
        private System.Windows.Forms.Button buttonOtherAdd;
        private System.Windows.Forms.DataGridView dataGridViewOther;
        private System.Windows.Forms.TabPage tabPageDisassembly;
        private System.Windows.Forms.Button buttonDisGo;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxDisStart;
        private System.Windows.Forms.Label labelDisStart;
        private System.Windows.Forms.RichTextBox richTextBoxDissasembly;
        private System.Windows.Forms.TabPage tabPageMap;
        private System.Windows.Forms.TabPage tabPageOptions;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBoxStartSlotIndexOne;
        private OpenTK.GLControl glControlMap;
        private System.Windows.Forms.Button buttonObjRetrieve;
        private System.Windows.Forms.Button buttonObjClone;
        private System.Windows.Forms.Button buttonObjGoTo;
        private System.Windows.Forms.Button buttonObjUnload;
        private System.Windows.Forms.PictureBox pictureBoxObjClone;
        private System.Windows.Forms.Label labelMapPuValue;
        private System.Windows.Forms.Label labelMapSubName;
        private System.Windows.Forms.Label labelMapQpu;
        private System.Windows.Forms.Label labelMapPu;
        private System.Windows.Forms.Label labelMapId;
        private System.Windows.Forms.Label labelMapName;
        private System.Windows.Forms.Label labelMapQpuValue;
    }
}


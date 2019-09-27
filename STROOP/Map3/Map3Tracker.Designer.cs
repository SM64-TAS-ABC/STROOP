using STROOP.Controls;
using STROOP.Controls.Map.Trackers;

namespace STROOP.Map3
{
    partial class Map3Tracker
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel = new STROOP.Controls.BorderedTableLayoutPanel();
            this.trackBarOutlineWidth = new System.Windows.Forms.TrackBar();
            this.colorSelectorOutline = new STROOP.Controls.ColorSelector();
            this.pictureBoxDownArrow = new System.Windows.Forms.PictureBox();
            this.pictureBoxUpArrow = new System.Windows.Forms.PictureBox();
            this.pictureBoxRedX = new System.Windows.Forms.PictureBox();
            this.pictureBoxEye = new System.Windows.Forms.PictureBox();
            this.comboBoxVisibilityType = new System.Windows.Forms.ComboBox();
            this.comboBoxOrderType = new System.Windows.Forms.ComboBox();
            this.checkBoxRotates = new System.Windows.Forms.CheckBox();
            this.textBoxName = new STROOP.BetterTextbox();
            this.panelPicture = new System.Windows.Forms.Panel();
            this.pictureBoxPicture = new STROOP.Controls.IntPictureBox();
            this.trackBarOpacity = new System.Windows.Forms.TrackBar();
            this.trackBarSize = new System.Windows.Forms.TrackBar();
            this.textBoxSize = new STROOP.BetterTextbox();
            this.textBoxOpacity = new STROOP.BetterTextbox();
            this.labelSize = new System.Windows.Forms.Label();
            this.labelOpacity = new System.Windows.Forms.Label();
            this.colorSelector = new STROOP.Controls.ColorSelector();
            this.pictureBoxPlus = new System.Windows.Forms.PictureBox();
            this.labelOutlineWidth = new System.Windows.Forms.Label();
            this.textBoxOutlineWidth = new STROOP.BetterTextbox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOutlineWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDownArrow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUpArrow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRedX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEye)).BeginInit();
            this.panelPicture.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOpacity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPlus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel.BorderColor = System.Drawing.Color.Black;
            this.tableLayoutPanel.BorderWidth = 1F;
            this.tableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel.ColumnCount = 5;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 106F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 51F));
            this.tableLayoutPanel.Controls.Add(this.trackBarOutlineWidth, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.colorSelectorOutline, 3, 4);
            this.tableLayoutPanel.Controls.Add(this.pictureBoxDownArrow, 4, 3);
            this.tableLayoutPanel.Controls.Add(this.pictureBoxUpArrow, 4, 2);
            this.tableLayoutPanel.Controls.Add(this.pictureBoxRedX, 4, 0);
            this.tableLayoutPanel.Controls.Add(this.pictureBoxEye, 4, 1);
            this.tableLayoutPanel.Controls.Add(this.comboBoxVisibilityType, 3, 1);
            this.tableLayoutPanel.Controls.Add(this.comboBoxOrderType, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.checkBoxRotates, 3, 2);
            this.tableLayoutPanel.Controls.Add(this.textBoxName, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.panelPicture, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.trackBarOpacity, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.trackBarSize, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.textBoxSize, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxOpacity, 2, 2);
            this.tableLayoutPanel.Controls.Add(this.labelSize, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.labelOpacity, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.colorSelector, 3, 3);
            this.tableLayoutPanel.Controls.Add(this.pictureBoxPlus, 4, 5);
            this.tableLayoutPanel.Controls.Add(this.labelOutlineWidth, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.textBoxOutlineWidth, 2, 4);
            this.tableLayoutPanel.Controls.Add(this.pictureBox1, 4, 4);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 6;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel.ShowBorder = false;
            this.tableLayoutPanel.Size = new System.Drawing.Size(334, 128);
            this.tableLayoutPanel.TabIndex = 40;
            // 
            // trackBarOutlineWidth
            // 
            this.tableLayoutPanel.SetColumnSpan(this.trackBarOutlineWidth, 2);
            this.trackBarOutlineWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBarOutlineWidth.Location = new System.Drawing.Point(73, 109);
            this.trackBarOutlineWidth.Name = "trackBarOutlineWidth";
            this.trackBarOutlineWidth.Size = new System.Drawing.Size(113, 15);
            this.trackBarOutlineWidth.TabIndex = 42;
            this.trackBarOutlineWidth.TickFrequency = 10;
            this.trackBarOutlineWidth.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarOutlineWidth.Value = 1;
            this.trackBarOutlineWidth.ValueChanged += new System.EventHandler(this.trackBarOutlineWidth_ValueChanged);
            // 
            // colorSelectorOutline
            // 
            this.colorSelectorOutline.BackColor = System.Drawing.Color.Transparent;
            this.colorSelectorOutline.Dock = System.Windows.Forms.DockStyle.Fill;
            this.colorSelectorOutline.Location = new System.Drawing.Point(190, 85);
            this.colorSelectorOutline.Margin = new System.Windows.Forms.Padding(0);
            this.colorSelectorOutline.Name = "colorSelectorOutline";
            this.colorSelectorOutline.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.colorSelectorOutline.SelectedColor = System.Drawing.SystemColors.Control;
            this.colorSelectorOutline.Size = new System.Drawing.Size(106, 20);
            this.colorSelectorOutline.TabIndex = 39;
            // 
            // pictureBoxDownArrow
            // 
            this.pictureBoxDownArrow.BackgroundImage = global::STROOP.Properties.Resources.Down_Arrow;
            this.pictureBoxDownArrow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxDownArrow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxDownArrow.Location = new System.Drawing.Point(300, 67);
            this.pictureBoxDownArrow.Name = "pictureBoxDownArrow";
            this.pictureBoxDownArrow.Size = new System.Drawing.Size(30, 14);
            this.pictureBoxDownArrow.TabIndex = 12;
            this.pictureBoxDownArrow.TabStop = false;
            this.pictureBoxDownArrow.Click += new System.EventHandler(this.pictureBoxDownArrow_Click);
            // 
            // pictureBoxUpArrow
            // 
            this.pictureBoxUpArrow.BackgroundImage = global::STROOP.Properties.Resources.Up_Arrow;
            this.pictureBoxUpArrow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxUpArrow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxUpArrow.Location = new System.Drawing.Point(300, 46);
            this.pictureBoxUpArrow.Name = "pictureBoxUpArrow";
            this.pictureBoxUpArrow.Size = new System.Drawing.Size(30, 14);
            this.pictureBoxUpArrow.TabIndex = 12;
            this.pictureBoxUpArrow.TabStop = false;
            this.pictureBoxUpArrow.Click += new System.EventHandler(this.pictureBoxUpArrow_Click);
            // 
            // pictureBoxRedX
            // 
            this.pictureBoxRedX.BackgroundImage = global::STROOP.Properties.Resources.Red_X;
            this.pictureBoxRedX.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxRedX.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxRedX.Location = new System.Drawing.Point(300, 4);
            this.pictureBoxRedX.Name = "pictureBoxRedX";
            this.pictureBoxRedX.Size = new System.Drawing.Size(30, 14);
            this.pictureBoxRedX.TabIndex = 12;
            this.pictureBoxRedX.TabStop = false;
            this.pictureBoxRedX.Click += new System.EventHandler(this.pictureBoxRedX_Click);
            // 
            // pictureBoxEye
            // 
            this.pictureBoxEye.BackgroundImage = global::STROOP.Properties.Resources.image_eye_open2;
            this.pictureBoxEye.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxEye.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxEye.Location = new System.Drawing.Point(300, 25);
            this.pictureBoxEye.Name = "pictureBoxEye";
            this.pictureBoxEye.Size = new System.Drawing.Size(30, 14);
            this.pictureBoxEye.TabIndex = 12;
            this.pictureBoxEye.TabStop = false;
            this.pictureBoxEye.Click += new System.EventHandler(this.pictureBoxEye_Click);
            // 
            // comboBoxVisibilityType
            // 
            this.comboBoxVisibilityType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxVisibilityType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxVisibilityType.Location = new System.Drawing.Point(192, 24);
            this.comboBoxVisibilityType.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxVisibilityType.Name = "comboBoxVisibilityType";
            this.comboBoxVisibilityType.Size = new System.Drawing.Size(102, 21);
            this.comboBoxVisibilityType.TabIndex = 14;
            // 
            // comboBoxOrderType
            // 
            this.comboBoxOrderType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxOrderType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOrderType.Location = new System.Drawing.Point(192, 3);
            this.comboBoxOrderType.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxOrderType.Name = "comboBoxOrderType";
            this.comboBoxOrderType.Size = new System.Drawing.Size(102, 21);
            this.comboBoxOrderType.TabIndex = 15;
            // 
            // checkBoxRotates
            // 
            this.checkBoxRotates.AutoSize = true;
            this.checkBoxRotates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxRotates.Location = new System.Drawing.Point(193, 46);
            this.checkBoxRotates.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.checkBoxRotates.Name = "checkBoxRotates";
            this.checkBoxRotates.Size = new System.Drawing.Size(103, 17);
            this.checkBoxRotates.TabIndex = 16;
            this.checkBoxRotates.Text = "Rotates";
            this.checkBoxRotates.UseVisualStyleBackColor = true;
            this.checkBoxRotates.CheckedChanged += new System.EventHandler(this.checkBoxRotates_CheckedChanged);
            // 
            // textBoxName
            // 
            this.textBoxName.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxName.Location = new System.Drawing.Point(4, 67);
            this.textBoxName.Multiline = true;
            this.textBoxName.Name = "textBoxName";
            this.tableLayoutPanel.SetRowSpan(this.textBoxName, 3);
            this.textBoxName.Size = new System.Drawing.Size(62, 57);
            this.textBoxName.TabIndex = 10;
            this.textBoxName.Text = "Mario";
            this.textBoxName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panelPicture
            // 
            this.panelPicture.BackColor = System.Drawing.Color.Red;
            this.panelPicture.Controls.Add(this.pictureBoxPicture);
            this.panelPicture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPicture.Location = new System.Drawing.Point(3, 3);
            this.panelPicture.Margin = new System.Windows.Forms.Padding(2);
            this.panelPicture.Name = "panelPicture";
            this.tableLayoutPanel.SetRowSpan(this.panelPicture, 3);
            this.panelPicture.Size = new System.Drawing.Size(64, 58);
            this.panelPicture.TabIndex = 18;
            // 
            // pictureBoxPicture
            // 
            this.pictureBoxPicture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxPicture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.pictureBoxPicture.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            this.pictureBoxPicture.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxPicture.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBoxPicture.MaximumSize = new System.Drawing.Size(133, 130);
            this.pictureBoxPicture.Name = "pictureBoxPicture";
            this.pictureBoxPicture.Size = new System.Drawing.Size(58, 52);
            this.pictureBoxPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxPicture.TabIndex = 0;
            this.pictureBoxPicture.TabStop = false;
            // 
            // trackBarOpacity
            // 
            this.tableLayoutPanel.SetColumnSpan(this.trackBarOpacity, 2);
            this.trackBarOpacity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBarOpacity.Location = new System.Drawing.Point(73, 67);
            this.trackBarOpacity.Maximum = 100;
            this.trackBarOpacity.Name = "trackBarOpacity";
            this.trackBarOpacity.Size = new System.Drawing.Size(113, 14);
            this.trackBarOpacity.TabIndex = 19;
            this.trackBarOpacity.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarOpacity.Value = 100;
            this.trackBarOpacity.ValueChanged += new System.EventHandler(this.trackBarOpacity_ValueChanged);
            // 
            // trackBarSize
            // 
            this.tableLayoutPanel.SetColumnSpan(this.trackBarSize, 2);
            this.trackBarSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBarSize.Location = new System.Drawing.Point(73, 25);
            this.trackBarSize.Maximum = 100;
            this.trackBarSize.Name = "trackBarSize";
            this.trackBarSize.Size = new System.Drawing.Size(113, 14);
            this.trackBarSize.TabIndex = 19;
            this.trackBarSize.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarSize.Value = 100;
            this.trackBarSize.ValueChanged += new System.EventHandler(this.trackBarSize_ValueChanged);
            // 
            // textBoxSize
            // 
            this.textBoxSize.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSize.Location = new System.Drawing.Point(124, 4);
            this.textBoxSize.Name = "textBoxSize";
            this.textBoxSize.Size = new System.Drawing.Size(62, 20);
            this.textBoxSize.TabIndex = 35;
            this.textBoxSize.Text = "100";
            this.textBoxSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxOpacity
            // 
            this.textBoxOpacity.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOpacity.Location = new System.Drawing.Point(124, 46);
            this.textBoxOpacity.Name = "textBoxOpacity";
            this.textBoxOpacity.Size = new System.Drawing.Size(62, 20);
            this.textBoxOpacity.TabIndex = 35;
            this.textBoxOpacity.Text = "100";
            this.textBoxOpacity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelSize
            // 
            this.labelSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelSize.AutoSize = true;
            this.labelSize.Location = new System.Drawing.Point(72, 8);
            this.labelSize.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(30, 13);
            this.labelSize.TabIndex = 36;
            this.labelSize.Text = "Size:";
            // 
            // labelOpacity
            // 
            this.labelOpacity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelOpacity.AutoSize = true;
            this.labelOpacity.Location = new System.Drawing.Point(72, 50);
            this.labelOpacity.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelOpacity.Name = "labelOpacity";
            this.labelOpacity.Size = new System.Drawing.Size(46, 13);
            this.labelOpacity.TabIndex = 37;
            this.labelOpacity.Text = "Opacity:";
            // 
            // colorSelector
            // 
            this.colorSelector.BackColor = System.Drawing.Color.Transparent;
            this.colorSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.colorSelector.Location = new System.Drawing.Point(190, 64);
            this.colorSelector.Margin = new System.Windows.Forms.Padding(0);
            this.colorSelector.Name = "colorSelector";
            this.colorSelector.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.colorSelector.SelectedColor = System.Drawing.SystemColors.Control;
            this.colorSelector.Size = new System.Drawing.Size(106, 20);
            this.colorSelector.TabIndex = 38;
            // 
            // pictureBoxPlus
            // 
            this.pictureBoxPlus.BackgroundImage = global::STROOP.Properties.Resources.image_plus;
            this.pictureBoxPlus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxPlus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxPlus.Location = new System.Drawing.Point(300, 109);
            this.pictureBoxPlus.Name = "pictureBoxPlus";
            this.pictureBoxPlus.Size = new System.Drawing.Size(30, 15);
            this.pictureBoxPlus.TabIndex = 17;
            this.pictureBoxPlus.TabStop = false;
            // 
            // labelOutlineWidth
            // 
            this.labelOutlineWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelOutlineWidth.AutoSize = true;
            this.labelOutlineWidth.Location = new System.Drawing.Point(72, 92);
            this.labelOutlineWidth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelOutlineWidth.Name = "labelOutlineWidth";
            this.labelOutlineWidth.Size = new System.Drawing.Size(38, 13);
            this.labelOutlineWidth.TabIndex = 40;
            this.labelOutlineWidth.Text = "Width:";
            // 
            // textBoxOutlineWidth
            // 
            this.textBoxOutlineWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutlineWidth.Location = new System.Drawing.Point(124, 88);
            this.textBoxOutlineWidth.Name = "textBoxOutlineWidth";
            this.textBoxOutlineWidth.Size = new System.Drawing.Size(62, 20);
            this.textBoxOutlineWidth.TabIndex = 41;
            this.textBoxOutlineWidth.Text = "1";
            this.textBoxOutlineWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::STROOP.Properties.Resources.cog;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Location = new System.Drawing.Point(300, 88);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(30, 14);
            this.pictureBox1.TabIndex = 43;
            this.pictureBox1.TabStop = false;
            // 
            // Map3Tracker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Map3Tracker";
            this.Size = new System.Drawing.Size(334, 128);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOutlineWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDownArrow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUpArrow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRedX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEye)).EndInit();
            this.panelPicture.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOpacity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPlus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BorderedTableLayoutPanel tableLayoutPanel;
        private BetterTextbox textBoxName;
        private System.Windows.Forms.PictureBox pictureBoxUpArrow;
        private System.Windows.Forms.PictureBox pictureBoxDownArrow;
        private System.Windows.Forms.PictureBox pictureBoxRedX;
        private System.Windows.Forms.PictureBox pictureBoxEye;
        private System.Windows.Forms.ComboBox comboBoxVisibilityType;
        private System.Windows.Forms.ComboBox comboBoxOrderType;
        private System.Windows.Forms.CheckBox checkBoxRotates;
        private System.Windows.Forms.PictureBox pictureBoxPlus;
        private System.Windows.Forms.Panel panelPicture;
        private IntPictureBox pictureBoxPicture;
        private System.Windows.Forms.TrackBar trackBarOpacity;
        private System.Windows.Forms.TrackBar trackBarSize;
        private BetterTextbox textBoxSize;
        private BetterTextbox textBoxOpacity;
        private System.Windows.Forms.Label labelSize;
        private System.Windows.Forms.Label labelOpacity;
        private ColorSelector colorSelector;
        private System.Windows.Forms.TrackBar trackBarOutlineWidth;
        private ColorSelector colorSelectorOutline;
        private System.Windows.Forms.Label labelOutlineWidth;
        private BetterTextbox textBoxOutlineWidth;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

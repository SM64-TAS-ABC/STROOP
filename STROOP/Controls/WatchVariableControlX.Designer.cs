namespace STROOP.Controls
{
    partial class WatchVariableControl
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
            this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this._namePanel = new System.Windows.Forms.Panel();
            this._pinPictureBox = new System.Windows.Forms.PictureBox();
            this._lockPictureBox = new System.Windows.Forms.PictureBox();
            this._nameTextBox = new System.Windows.Forms.TextBox();
            this._valuePanel = new System.Windows.Forms.Panel();
            this._valueTextBox = new System.Windows.Forms.TextBox();
            this._valueCheckBox = new System.Windows.Forms.CheckBox();
            this._tableLayoutPanel.SuspendLayout();
            this._namePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._pinPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._lockPictureBox)).BeginInit();
            this._valuePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tableLayoutPanel
            // 
            this._tableLayoutPanel.BackColor = System.Drawing.Color.Transparent;
            this._tableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this._tableLayoutPanel.ColumnCount = 2;
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this._tableLayoutPanel.Controls.Add(this._namePanel, 0, 0);
            this._tableLayoutPanel.Controls.Add(this._valuePanel, 1, 0);
            this._tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this._tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutPanel.Name = "_tableLayoutPanel";
            this._tableLayoutPanel.RowCount = 1;
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel.Size = new System.Drawing.Size(205, 22);
            this._tableLayoutPanel.TabIndex = 0;
            // 
            // _namePanel
            // 
            this._namePanel.BackColor = System.Drawing.Color.Transparent;
            this._namePanel.Controls.Add(this._pinPictureBox);
            this._namePanel.Controls.Add(this._lockPictureBox);
            this._namePanel.Controls.Add(this._nameTextBox);
            this._namePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._namePanel.Location = new System.Drawing.Point(1, 1);
            this._namePanel.Margin = new System.Windows.Forms.Padding(0);
            this._namePanel.Name = "_namePanel";
            this._namePanel.Size = new System.Drawing.Size(120, 20);
            this._namePanel.TabIndex = 0;
            // 
            // _pinPictureBox
            // 
            this._pinPictureBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this._pinPictureBox.Image = global::STROOP.Properties.Resources.img_pin;
            this._pinPictureBox.Location = new System.Drawing.Point(109, 2);
            this._pinPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this._pinPictureBox.Name = "_pinPictureBox";
            this._pinPictureBox.Size = new System.Drawing.Size(10, 17);
            this._pinPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._pinPictureBox.TabIndex = 2;
            this._pinPictureBox.TabStop = false;
            this._pinPictureBox.Visible = false;
            // 
            // _lockPictureBox
            // 
            this._lockPictureBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this._lockPictureBox.Image = global::STROOP.Properties.Resources.img_lock;
            this._lockPictureBox.Location = new System.Drawing.Point(104, 1);
            this._lockPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this._lockPictureBox.Name = "_lockPictureBox";
            this._lockPictureBox.Size = new System.Drawing.Size(16, 18);
            this._lockPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._lockPictureBox.TabIndex = 1;
            this._lockPictureBox.TabStop = false;
            this._lockPictureBox.Visible = false;
            // 
            // _nameTextBox
            // 
            this._nameTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this._nameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._nameTextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this._nameTextBox.Location = new System.Drawing.Point(4, 3);
            this._nameTextBox.Name = "_nameTextBox";
            this._nameTextBox.ReadOnly = true;
            this._nameTextBox.Size = new System.Drawing.Size(200, 13);
            this._nameTextBox.TabIndex = 0;
            // 
            // _valuePanel
            // 
            this._valuePanel.BackColor = System.Drawing.Color.Transparent;
            this._valuePanel.Controls.Add(this._valueTextBox);
            this._valuePanel.Controls.Add(this._valueCheckBox);
            this._valuePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._valuePanel.Location = new System.Drawing.Point(122, 1);
            this._valuePanel.Margin = new System.Windows.Forms.Padding(0);
            this._valuePanel.Name = "_valuePanel";
            this._valuePanel.Size = new System.Drawing.Size(85, 20);
            this._valuePanel.TabIndex = 1;
            // 
            // _valueTextBox
            // 
            this._valueTextBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this._valueTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._valueTextBox.Location = new System.Drawing.Point(-121, 3);
            this._valueTextBox.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this._valueTextBox.Name = "_valueTextBox";
            this._valueTextBox.ReadOnly = true;
            this._valueTextBox.Size = new System.Drawing.Size(200, 13);
            this._valueTextBox.TabIndex = 0;
            this._valueTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // _valueCheckBox
            // 
            this._valueCheckBox.AutoSize = true;
            this._valueCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._valueCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._valueCheckBox.Location = new System.Drawing.Point(0, 0);
            this._valueCheckBox.Margin = new System.Windows.Forms.Padding(0);
            this._valueCheckBox.Name = "_valueCheckBox";
            this._valueCheckBox.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this._valueCheckBox.Size = new System.Drawing.Size(85, 20);
            this._valueCheckBox.TabIndex = 1;
            this._valueCheckBox.UseVisualStyleBackColor = true;
            // 
            // WatchVariableControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._tableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "WatchVariableControl";
            this.Size = new System.Drawing.Size(205, 22);
            this._tableLayoutPanel.ResumeLayout(false);
            this._namePanel.ResumeLayout(false);
            this._namePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._pinPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._lockPictureBox)).EndInit();
            this._valuePanel.ResumeLayout(false);
            this._valuePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
        private System.Windows.Forms.Panel _namePanel;
        private System.Windows.Forms.Panel _valuePanel;
        private System.Windows.Forms.TextBox _nameTextBox;
        private System.Windows.Forms.TextBox _valueTextBox;
        private System.Windows.Forms.PictureBox _pinPictureBox;
        private System.Windows.Forms.PictureBox _lockPictureBox;
        private System.Windows.Forms.CheckBox _valueCheckBox;
    }
}

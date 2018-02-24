namespace STROOP.Controls
{
    partial class WatchVariableControlX
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
            this._valuePanel = new System.Windows.Forms.Panel();
            this._nameTextBox = new System.Windows.Forms.TextBox();
            this._valueTextBox = new System.Windows.Forms.TextBox();
            this._valueCheckBox = new System.Windows.Forms.CheckBox();
            this._lockPictureBox = new System.Windows.Forms.PictureBox();
            this._pinPictureBox = new System.Windows.Forms.PictureBox();
            this._tableLayoutPanel.SuspendLayout();
            this._namePanel.SuspendLayout();
            this._valuePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._lockPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._pinPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // _tableLayoutPanel
            // 
            this._tableLayoutPanel.ColumnCount = 2;
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanel.Controls.Add(this._namePanel, 0, 0);
            this._tableLayoutPanel.Controls.Add(this._valuePanel, 1, 0);
            this._tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this._tableLayoutPanel.Name = "_tableLayoutPanel";
            this._tableLayoutPanel.RowCount = 1;
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._tableLayoutPanel.Size = new System.Drawing.Size(517, 129);
            this._tableLayoutPanel.TabIndex = 0;
            // 
            // _namePanel
            // 
            this._namePanel.BackColor = System.Drawing.Color.Transparent;
            this._namePanel.Controls.Add(this._pinPictureBox);
            this._namePanel.Controls.Add(this._lockPictureBox);
            this._namePanel.Controls.Add(this._nameTextBox);
            this._namePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._namePanel.Location = new System.Drawing.Point(0, 0);
            this._namePanel.Margin = new System.Windows.Forms.Padding(0);
            this._namePanel.Name = "_namePanel";
            this._namePanel.Size = new System.Drawing.Size(258, 129);
            this._namePanel.TabIndex = 0;
            // 
            // _valuePanel
            // 
            this._valuePanel.BackColor = System.Drawing.Color.Transparent;
            this._valuePanel.Controls.Add(this._valueCheckBox);
            this._valuePanel.Controls.Add(this._valueTextBox);
            this._valuePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._valuePanel.Location = new System.Drawing.Point(258, 0);
            this._valuePanel.Margin = new System.Windows.Forms.Padding(0);
            this._valuePanel.Name = "_valuePanel";
            this._valuePanel.Size = new System.Drawing.Size(259, 129);
            this._valuePanel.TabIndex = 1;
            // 
            // _nameTextBox
            // 
            this._nameTextBox.Location = new System.Drawing.Point(64, 55);
            this._nameTextBox.Name = "_nameTextBox";
            this._nameTextBox.Size = new System.Drawing.Size(144, 20);
            this._nameTextBox.TabIndex = 0;
            // 
            // _valueTextBox
            // 
            this._valueTextBox.Location = new System.Drawing.Point(51, 36);
            this._valueTextBox.Name = "_valueTextBox";
            this._valueTextBox.Size = new System.Drawing.Size(154, 20);
            this._valueTextBox.TabIndex = 0;
            // 
            // _valueCheckBox
            // 
            this._valueCheckBox.AutoSize = true;
            this._valueCheckBox.Location = new System.Drawing.Point(72, 93);
            this._valueCheckBox.Name = "_valueCheckBox";
            this._valueCheckBox.Size = new System.Drawing.Size(80, 17);
            this._valueCheckBox.TabIndex = 1;
            this._valueCheckBox.Text = "checkBox1";
            this._valueCheckBox.UseVisualStyleBackColor = true;
            // 
            // _lockPictureBox
            // 
            this._lockPictureBox.Location = new System.Drawing.Point(125, 90);
            this._lockPictureBox.Name = "_lockPictureBox";
            this._lockPictureBox.Size = new System.Drawing.Size(37, 19);
            this._lockPictureBox.TabIndex = 1;
            this._lockPictureBox.TabStop = false;
            // 
            // _pinPictureBox
            // 
            this._pinPictureBox.Location = new System.Drawing.Point(64, 77);
            this._pinPictureBox.Name = "_pinPictureBox";
            this._pinPictureBox.Size = new System.Drawing.Size(40, 32);
            this._pinPictureBox.TabIndex = 2;
            this._pinPictureBox.TabStop = false;
            // 
            // WatchVariableControlX
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._tableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "WatchVariableControlX";
            this.Size = new System.Drawing.Size(517, 129);
            this._tableLayoutPanel.ResumeLayout(false);
            this._namePanel.ResumeLayout(false);
            this._namePanel.PerformLayout();
            this._valuePanel.ResumeLayout(false);
            this._valuePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._lockPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._pinPictureBox)).EndInit();
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

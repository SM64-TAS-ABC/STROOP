namespace STROOP.Forms
{
    partial class VariableCreationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VariableCreationForm));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxBaseLabel = new System.Windows.Forms.TextBox();
            this.textBoxTypeLabel = new System.Windows.Forms.TextBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.textBoxNameValue = new System.Windows.Forms.TextBox();
            this.textBoxNameLabel = new System.Windows.Forms.TextBox();
            this.textBoxOffsetLabel = new System.Windows.Forms.TextBox();
            this.textBoxOffsetValue = new System.Windows.Forms.TextBox();
            this.comboBoxTypeValue = new System.Windows.Forms.ComboBox();
            this.comboBoxBaseValue = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelMain.ColumnCount = 2;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.comboBoxBaseValue, 1, 2);
            this.tableLayoutPanelMain.Controls.Add(this.buttonOk, 0, 4);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxNameLabel, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxNameValue, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxTypeLabel, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxBaseLabel, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxOffsetLabel, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxOffsetValue, 1, 3);
            this.tableLayoutPanelMain.Controls.Add(this.comboBoxTypeValue, 1, 1);
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 5;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(277, 143);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // textBoxBaseLabel
            // 
            this.textBoxBaseLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBaseLabel.Location = new System.Drawing.Point(3, 59);
            this.textBoxBaseLabel.Name = "textBoxBaseLabel";
            this.textBoxBaseLabel.ReadOnly = true;
            this.textBoxBaseLabel.Size = new System.Drawing.Size(44, 20);
            this.textBoxBaseLabel.TabIndex = 15;
            this.textBoxBaseLabel.Text = "Base:";
            this.textBoxBaseLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxTypeLabel
            // 
            this.textBoxTypeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTypeLabel.Location = new System.Drawing.Point(3, 31);
            this.textBoxTypeLabel.Name = "textBoxTypeLabel";
            this.textBoxTypeLabel.ReadOnly = true;
            this.textBoxTypeLabel.Size = new System.Drawing.Size(44, 20);
            this.textBoxTypeLabel.TabIndex = 14;
            this.textBoxTypeLabel.Text = "Type:";
            this.textBoxTypeLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // buttonOk
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonOk, 2);
            this.buttonOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOk.Location = new System.Drawing.Point(3, 115);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(271, 25);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // textBoxNameValue
            // 
            this.textBoxNameValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNameValue.BackColor = System.Drawing.Color.White;
            this.textBoxNameValue.Location = new System.Drawing.Point(53, 3);
            this.textBoxNameValue.Name = "textBoxNameValue";
            this.textBoxNameValue.Size = new System.Drawing.Size(221, 20);
            this.textBoxNameValue.TabIndex = 11;
            this.textBoxNameValue.Text = "My New Variable";
            // 
            // textBoxNameLabel
            // 
            this.textBoxNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNameLabel.Location = new System.Drawing.Point(3, 3);
            this.textBoxNameLabel.Name = "textBoxNameLabel";
            this.textBoxNameLabel.ReadOnly = true;
            this.textBoxNameLabel.Size = new System.Drawing.Size(44, 20);
            this.textBoxNameLabel.TabIndex = 14;
            this.textBoxNameLabel.Text = "Name:";
            this.textBoxNameLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxOffsetLabel
            // 
            this.textBoxOffsetLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOffsetLabel.Location = new System.Drawing.Point(3, 87);
            this.textBoxOffsetLabel.Name = "textBoxOffsetLabel";
            this.textBoxOffsetLabel.ReadOnly = true;
            this.textBoxOffsetLabel.Size = new System.Drawing.Size(44, 20);
            this.textBoxOffsetLabel.TabIndex = 15;
            this.textBoxOffsetLabel.Text = "Offset:";
            this.textBoxOffsetLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxOffsetValue
            // 
            this.textBoxOffsetValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOffsetValue.BackColor = System.Drawing.Color.White;
            this.textBoxOffsetValue.Location = new System.Drawing.Point(53, 87);
            this.textBoxOffsetValue.Name = "textBoxOffsetValue";
            this.textBoxOffsetValue.Size = new System.Drawing.Size(221, 20);
            this.textBoxOffsetValue.TabIndex = 10;
            this.textBoxOffsetValue.Text = "0x100";
            // 
            // comboBoxTypeValue
            // 
            this.comboBoxTypeValue.BackColor = System.Drawing.Color.White;
            this.comboBoxTypeValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxTypeValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTypeValue.FormattingEnabled = true;
            this.comboBoxTypeValue.Location = new System.Drawing.Point(53, 31);
            this.comboBoxTypeValue.Name = "comboBoxTypeValue";
            this.comboBoxTypeValue.Size = new System.Drawing.Size(221, 21);
            this.comboBoxTypeValue.TabIndex = 16;
            // 
            // comboBoxBaseValue
            // 
            this.comboBoxBaseValue.BackColor = System.Drawing.Color.White;
            this.comboBoxBaseValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxBaseValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBaseValue.FormattingEnabled = true;
            this.comboBoxBaseValue.Location = new System.Drawing.Point(53, 59);
            this.comboBoxBaseValue.Name = "comboBoxBaseValue";
            this.comboBoxBaseValue.Size = new System.Drawing.Size(221, 21);
            this.comboBoxBaseValue.TabIndex = 17;
            // 
            // VariableCreationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 167);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(10000, 10000);
            this.MinimizeBox = false;
            this.Name = "VariableCreationForm";
            this.ShowIcon = false;
            this.Text = "Variable Info";
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.TextBox textBoxTypeLabel;
        private System.Windows.Forms.TextBox textBoxBaseLabel;
        private System.Windows.Forms.ComboBox comboBoxBaseValue;
        private System.Windows.Forms.TextBox textBoxNameLabel;
        private System.Windows.Forms.TextBox textBoxNameValue;
        private System.Windows.Forms.TextBox textBoxOffsetLabel;
        private System.Windows.Forms.TextBox textBoxOffsetValue;
        private System.Windows.Forms.ComboBox comboBoxTypeValue;
    }
}